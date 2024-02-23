/*
* User code for the top-level form for the Conlang Audio Honing program.
* 
* Copyright (C) 2024 Ronald B. Oakes
*
* This program is free software: you can redistribute it and/or modify it
* under the terms of the GNU General Public License as published by the Free
* Software Foundation, either version 3 of the License, or (at your option)
* any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT
* ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or
* FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
* more details.
*
* You should have received a copy of the GNU General Public License along with
* this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using ConlangJson;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Main form for the Conlang Language Honing Application.
    /// </summary>
    public partial class LanguageHoningForm : Form
    {
        private LanguageDescription? languageDescription = null;
        private FileInfo? languageFileInfo = null;
        private string? sampleText = null;
        private Dictionary<string, SpeechEngine> speechEngines = new Dictionary<string, SpeechEngine>();
        private Dictionary<string, Dictionary<string, SpeechEngine.VoiceData>> voices =
            new Dictionary<string, Dictionary<string, SpeechEngine.VoiceData>>();
        private Font? printFont;
        private TextReader? readerToPrint;
        private Dictionary<string, FileInfo> speechFiles = new Dictionary<string, FileInfo>();
        private PhoneticChanger phoneticChanger;
        private List<(string, string)> changesToBeMade = new List<(string, string)>();
        private UserConfiguration config;

        /// <summary>
        /// Provides access to the ProgressBar on the main form so that it can be accessed by other classes and methods.
        /// </summary>
        public System.Windows.Forms.ProgressBar ProgressBar
        {
            get => pb_status;
        }

        /// <summary>
        /// Constructor for the LanguageHoningForm.
        /// </summary>
        public LanguageHoningForm()
        {
            InitializeComponent();

            // Checks to ensure that the form from the designer doesn't exceed 1024x768
            if (this.Width > 1024)
            {
                throw new ConlangAudioHoningException("The default/design width of LanguageHoningForm exceeds the 1024 small screen size limit");
            }
            if (this.Height > 768)
            {
                throw new ConlangAudioHoningException("The default/design height of LanguageHoningForm exceeds the 768 small screen size limit");
            }

            config = UserConfiguration.LoadFromFile();

            if (config.IsESpeakNGSupported)
            {
                ESpeakNGSpeak.ESpeakNGPath = config.ESpeakNgPath;
                ESpeakNGSpeak eSpeakNGSpeak = new ESpeakNGSpeak();
                Dictionary<string, SpeechEngine.VoiceData> espeakVoices = eSpeakNGSpeak.getVoices();
                speechEngines.Add(eSpeakNGSpeak.Description, eSpeakNGSpeak);
                voices.Add(eSpeakNGSpeak.Description, espeakVoices);
                //eSpeakNGSpeak.Test();
            }

            if (config.IsPollySupported)
            {
                PollySpeech.PollyURI = config.PollyURI;
                PollySpeech pollySpeech = new PollySpeech();
                Dictionary<string, SpeechEngine.VoiceData> amazonPollyVoices = pollySpeech.getVoices();
                speechEngines.Add(pollySpeech.Description, pollySpeech);
                voices.Add(pollySpeech.Description, amazonPollyVoices);
            }

            phoneticChanger = new PhoneticChanger();

            changesToBeMade.Clear();
            LoadSpeechEngines();
            LoadVoices();
            LoadSpeeds();
            tabPhoneticAlterations.SelectedIndexChanged += TabPhoneticAlterations_SelectedIndexChanged;

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
        }

        private void TabPhoneticAlterations_SelectedIndexChanged(object? sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
            if ((tabPhoneticAlterations.SelectedIndex != 3) || ((tabPhoneticAlterations.SelectedIndex == 3) && (!rbn_replaceRSpelling.Checked)))
            {
                lbl_choiceCbx.Text = "Phoneme to change";
                lbl_replacementCbx.Text = "Replacement Phoneme";
            }
        }

        /// <summary>
        /// Decline the complete Lexicon of the supplied language.
        /// </summary>
        /// <param name="language">Language to be declined.</param>
        internal void DeclineLexicon(LanguageDescription language)
        {
            pb_status.Style = ProgressBarStyle.Continuous;
            pb_status.BringToFront();
            pb_status.Minimum = 0;
            pb_status.Maximum = language.lexicon.Count;
            pb_status.Step = 1;
            pb_status.Font = new Font("CharisSIL", 12f);
            pb_status.ResetText();
            pb_status.Visible = true;
            List<LexiconEntry> addLexicon = new List<LexiconEntry>();
            foreach (LexiconEntry word in language.lexicon)
            {
                pb_status.Text = word.phonetic.ToString();
                addLexicon.AddRange(ConlangUtilities.DeclineWord(word, language.affix_map, language.sound_map_list));
                pb_status.PerformStep();
            }
            pb_status.Style = ProgressBarStyle.Marquee;
            pbTimer.Interval = 20;
            pbTimer.Enabled = true;
            pb_status.MarqueeAnimationSpeed = 200;
            pb_status.Minimum = 0;
            pb_status.Maximum = 100;
            language.lexicon.AddRange(addLexicon);
            pb_status.Visible = false;
            pb_status.SendToBack();
            pbTimer.Enabled = false;
            language.declined = true;
        }

        private void LoadSpeechEngines()
        {
            cbx_speechEngine.SuspendLayout();
            cbx_speechEngine.Items.Clear();
            foreach (string engineName in speechEngines.Keys)
            {
                cbx_speechEngine.Items.Add(engineName);
            }
            if (cbx_speechEngine.Items.Count > 0)
            {
                cbx_speechEngine.SelectedIndex = 0;
            }
            else
            {
                cbx_speechEngine.SelectedIndex = -1;
            }
            cbx_speechEngine.ResumeLayout();
        }

        private void LoadVoices()
        {
            cbx_voice.SuspendLayout();
            cbx_voice.Items.Clear();
            if (cbx_speechEngine.SelectedIndex != -1)
            {
                string selectedEngine = cbx_speechEngine.Text.Trim();
                Dictionary<string, SpeechEngine.VoiceData> voiceData = voices[selectedEngine];
                foreach (string voiceName in voiceData.Keys)
                {
                    string voiceMenu = string.Format("{0} ({1}, {2})", voiceName, voiceData[voiceName].LanguageName, voiceData[voiceName].Gender);
                    cbx_voice.Items.Add(voiceMenu);
                }
                if ((languageDescription != null) &&
                    (languageDescription.preferred_voices.ContainsKey(speechEngines[selectedEngine].preferredVoiceKey)) &&
                    (!string.IsNullOrEmpty(languageDescription.preferred_voices[speechEngines[selectedEngine].preferredVoiceKey])))
                {
                    string preferredVoice = languageDescription.preferred_voices[speechEngines[selectedEngine].preferredVoiceKey].Trim();
                    string voiceMenu = string.Format("{0} ({1}, {2})", preferredVoice, voiceData[preferredVoice].LanguageName, voiceData[preferredVoice].Gender);
                    cbx_voice.Text = voiceMenu;
                }
            }
            cbx_voice.ResumeLayout();
        }

        private void LoadSpeeds()
        {
            cbx_speed.SuspendLayout();
            cbx_speed.Items.Clear();
            cbx_speed.Items.AddRange(new string[] { "x-slow", "slow", "medium", "fast", "x-fast" });
            cbx_speed.ResumeLayout();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    openFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    openFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                openFileDialog.Filter = "JSON file (*.json)|*.json|txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadLanguage(openFileDialog.FileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    saveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    saveFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                saveFileDialog.Filter = "JSON file (*.json)|*.json|txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.OverwritePrompt = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveLanguage(saveFileDialog.FileName);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadLanguage(string filename)
        {
            string jsonString = File.ReadAllText(filename);

            try
            {
                languageDescription = JsonSerializer.Deserialize<LanguageDescription>(jsonString);
            }
            catch (Exception)
            {
                languageDescription = null;
            }

            if (languageDescription == null)
            {
                MessageBox.Show("Unable to decode Language file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (languageDescription.version != 1.0)
            {
                MessageBox.Show("Incorrect language file version", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                languageDescription = new LanguageDescription();
                return;
            }

            languageFileInfo = new FileInfo(filename);

            // Temporary work around to put the Polly voice into the preferred voices map.  
            // Once the JSON structure is reworked, this will not be needed.
            if (!languageDescription.preferred_voices.ContainsKey("Polly"))
            {
                languageDescription.preferred_voices.Add("Polly", "Brian");
            }
            if (!languageDescription.preferred_voices.ContainsKey("espeak-ng"))
            {
                languageDescription.preferred_voices.Add("espeak-ng", "en-us");
            }

            IpaUtilities.SubstituteLatinIpaReplacements(languageDescription);
            IpaUtilities.BuildPhoneticInventory(languageDescription);
            phoneticChanger.Language = languageDescription;

            // Empty the speech files and list box
            speechFiles.Clear();
            cbx_recordings.Items.Clear();
            cbx_speed.SelectedText = "slow";

            if (languageDescription.declined)
            {
                declineToolStripMenuItem.Enabled = false;
            }
            if (languageDescription.derived)
            {
                deriveToolStripMenuItem.Enabled = false;
            }

            foreach (string engineKey in speechEngines.Keys)
            {
                speechEngines[engineKey].LanguageDescription = languageDescription;
            }

            // Clear the list of change and combo boxes
            cbx_phonemeToChange.Items.Clear();
            cbx_phonemeToChange.Items.Clear();
            changesToBeMade.Clear();
            txt_changeList.Text = string.Empty;
            // Set the default settings for the language alteration selections.
            rbn_normalVowel.Checked = true;
            tabPhoneticAlterations.SelectedIndex = 0; // Consonants.
            rbn_l1.Checked = true;
            rbn_addRhoticityRegular.Checked = true;
            updatePhonemeToChangeCbx();
            LoadVoices();
        }

        private void SaveLanguage(string filename)
        {
            if (languageDescription == null)
            {
                MessageBox.Show("Unable to save language File, no language information present", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // If the language is currently declined, it might have been declined by this tool, 
                // If so, we want to sort it before saving it.
                if (languageDescription.declined)
                {
                    languageDescription.lexicon.Sort(new LexiconEntry.LexicalOrderCompSpelling());
                }

                DateTime now = DateTime.UtcNow;
                string timestamp = now.ToString("o");
                string history = "Edited in LanguageEditor, saved at " + timestamp;

                if (!languageDescription.metadata.ContainsKey("history"))
                {
                    languageDescription.metadata["history"] = new JsonArray();
                }
#pragma warning disable CS8600
                JsonArray historyEntries = (JsonArray)languageDescription.metadata["history"] ?? [];
#pragma warning restore CS8600
                historyEntries.Add(history);

                JsonSerializerOptions jsonSerializerOptions = new();
                jsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                JavaScriptEncoder encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                jsonSerializerOptions.Encoder = encoder;
                jsonSerializerOptions.WriteIndented = true; // TODO: make an option

                string jsonString = JsonSerializer.Serialize<LanguageDescription>(languageDescription, jsonSerializerOptions);
                File.WriteAllText(filename, jsonString, System.Text.Encoding.UTF8);
            }
        }

        private void loadSampleTextFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    openFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    openFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                openFileDialog.Filter = "txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadSampleText(openFileDialog.FileName);
                    // Empty the speech files and list box
                    speechFiles.Clear();
                    cbx_recordings.Items.Clear();
                }
            }
        }

        private void LoadSampleText(string fileName)
        {
            string fileText = File.ReadAllText(fileName);

            sampleText = fileText;
            txt_SampleText.Text = sampleText;
            foreach (string engineKey in speechEngines.Keys)
            {
                speechEngines[engineKey].sampleText = sampleText;
            }
            phoneticChanger.SampleText = sampleText;
        }

        private void saveSampleMenu_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    saveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    saveFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                saveFileDialog.Filter = "txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.OverwritePrompt = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveSampleText(saveFileDialog.FileName);
                }
            }
        }

        private void SaveSampleText(string filename)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
            File.WriteAllText(filename, sampleText);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            // TODO: Add "dirty" informaton for LanguageDescription and sampleText, prompt for saving.
            config.SaveConfiguration();
        }

        private void txt_SampleText_TextChanged(object sender, EventArgs e)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            if ((languageDescription != null) && (!string.IsNullOrEmpty(sampleText)))
            {
                string engineKey = cbx_speechEngine.Text.Trim();
                SpeechEngine engine = speechEngines[engineKey];
                string speed = cbx_speed.Text.Trim();
                engine.Generate(speed, this);
                txt_phonetic.Text = engine.phoneticText;
            }
        }

        private void declineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if (!languageDescription.declined)
            {
                DeclineLexicon(languageDescription);
                //MessageBox.Show("Finished Declining the Language", "Done",MessageBoxButtons.OK);
                declineToolStripMenuItem.Text = "Remove Declensions";
            }
            else
            {
                ConlangUtilities.removeDeclinedEntries(languageDescription);
                declineToolStripMenuItem.Text = "Decline Language";
            }

        }

        private void btn_generateSpeech_Click(object sender, EventArgs e)
        {
            if ((languageDescription == null) || (string.IsNullOrEmpty(sampleText)))
            {
                return;
            }
            string engineName = cbx_speechEngine.Text.Trim();
            if ((engineName.Equals("Amazon Polly")) && (config.IsPollySupported))
            {
                PollySpeech pollySpeech = (PollySpeech)speechEngines[engineName];
                DateTime now = DateTime.Now;
                string targetFileBaseName = string.Format("speech_{0:s}.mp3", now);
                targetFileBaseName = targetFileBaseName.Replace(":", "_");

                string targetFileName;
                if (languageFileInfo != null)
                {
                    targetFileName = languageFileInfo.Directory + "\\" + targetFileBaseName;
                }
                else
                {
                    targetFileName = Path.GetTempPath() + targetFileBaseName;
                }

                string voice = cbx_voice.Text.Trim().Split()[0];
                string speed = cbx_speed.Text.Trim();

                bool ok = pollySpeech.GenerateSpeech(targetFileName, voice, speed, this);
                if (!ok)
                {
                    MessageBox.Show("Unable to generate speech file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txt_phonetic.Text = pollySpeech.phoneticText;
                    // Play the audio (MP3) file with the Windows Media Player
                    WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
                    player.URL = targetFileName;
                    player.controls.play();

                    FileInfo fileInfo = new FileInfo(targetFileName);
                    speechFiles.Add(fileInfo.Name, fileInfo);
                    cbx_recordings.Items.Add(fileInfo.Name);
                    cbx_recordings.SelectedText = fileInfo.Name;
                }
            }
            else if ((engineName.Equals("espeak-ng")) && (config.IsESpeakNGSupported))
            {
                ESpeakNGSpeak speechEngine = (ESpeakNGSpeak)speechEngines[engineName];
                DateTime now = DateTime.Now;
                string targetFileBaseName = string.Format("speech_{0:s}.wav", now);
                targetFileBaseName = targetFileBaseName.Replace(":", "_");

                string targetFileName;
                if (languageFileInfo != null)
                {
                    targetFileName = languageFileInfo.Directory + "\\" + targetFileBaseName;
                }
                else
                {
                    targetFileName = Path.GetTempPath() + targetFileBaseName;
                }
                string voiceKey = cbx_voice.Text.Trim().Split()[0];
                string voice = string.Empty;
                if (voices[engineName].ContainsKey(voiceKey))
                {
                    SpeechEngine.VoiceData voiceData = voices[engineName][voiceKey];
                    voice = voiceData.LanguageCode;
                }
                string speed = cbx_speed.Text.Trim();
                speechEngine.sampleText = sampleText;
                bool ok = speechEngine.GenerateSpeech(targetFileName, voice, speed, this);
                if (!ok)
                {
                    MessageBox.Show("Unable to generate speech file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txt_phonetic.Text = speechEngine.phoneticText;
                    // Play the audio (wav) file with the Windows Media Player
                    WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
                    player.URL = targetFileName;
                    player.controls.play();

                    FileInfo fileInfo = new FileInfo(targetFileName);
                    speechFiles.Add(fileInfo.Name, fileInfo);
                    cbx_recordings.Items.Add(fileInfo.Name);
                    cbx_recordings.SelectedText = fileInfo.Name;
                }
            }
        }

        private void displayPulmonicConsonantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string c in IpaUtilities.PConsonants)
            {
                sb.Append(c);
                sb.Append(" ");
            }
            txt_SampleText.Text = sb.ToString();
            txt_phonetic.Text = sb.ToString();
        }

        private string WrapText(string text, int cols)
        {
            int col = 0;
            StringBuilder sb = new StringBuilder();
            using (StringReader textReader = new StringReader(text))
            {
                string? line;
                do
                {
                    line = textReader.ReadLine();
                    if ((line != null) && (!line.Trim().Equals(string.Empty)))
                    {
                        foreach (string word in line.Split(null))
                        {
                            sb.Append(word);
                            col += word.Length;
                            if (col < ((cols * 8) / 10))
                            {
                                sb.Append(" ");
                                col += 1;
                            }
                            else
                            {
                                sb.AppendLine();
                                col = 0;
                            }
                        }
                    }
                }
                while (line != null);
            }


            return sb.ToString();
        }

        // The PrintPage event is raised for each page to be printed.
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            if ((readerToPrint == null) || (printFont == null) || (ev == null))
            {
                return;
            }

            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string? line = string.Empty;

            // Calculate the number of lines per page.
#pragma warning disable CS8604 // Possible null reference argument.
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
#pragma warning restore CS8604 // Possible null reference argument.

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = readerToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
            {
                ev.HasMorePages = true;
            }
            else
            {
                ev.HasMorePages = false;
            }
        }

        private void printIPAMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaPhonemes = IpaUtilities.IpaPhonemes();
            StringReader ipaPhonemeReader = new StringReader(ipaPhonemes);
            readerToPrint = ipaPhonemeReader;
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printFont = new Font("Charis SIL", 12f);
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                printDialog.Document = pd;
                DialogResult result = printDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pd.Print();
                }
            }
            finally
            {
                readerToPrint.Close();
            }
        }

        private void displaySupersegmentals_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string c in IpaUtilities.Suprasegmentals)
            {
                sb.Append(c);
                sb.Append(" ");
            }
            txt_SampleText.Text = sb.ToString();
            txt_phonetic.Text = sb.ToString();
        }

        private void pbTimer_Tick(object sender, EventArgs e)
        {
            pb_status.PerformStep();
        }

        private void btn_replaySpeech_Click(object sender, EventArgs e)
        {
            string fileName = cbx_recordings.Text;
            string targetFileName = speechFiles[fileName].FullName;
            // Play the audio (OGG) file with the default application
            WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
            player.URL = targetFileName;
            player.controls.play();
        }

        private void cbx_phonemeToChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateReplacementPhonemeCbx();
        }

        private void updatePhonemeToChangeCbx()
        {
            if (languageDescription == null)
            {
                return;
            }
            switch (tabPhoneticAlterations.SelectedIndex)
            {
                case 0:  // Consonants
                    // Ensure that there is a blank at the top of the drop down list
                    cbx_phonemeToChange.Items.Clear();
                    // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                    // use it to populate the combo box of pulmonic consonants to be changed.
                    foreach (string consonant in languageDescription.phonetic_inventory["p_consonants"])
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0} -- ", consonant);
                        sb.Append(IpaUtilities.IpaPhonemesMap[consonant]);
                        cbx_phonemeToChange.Items.Add(sb.ToString());
                    }
                    foreach (string consonant in languageDescription.phonetic_inventory["np_consonants"])
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0} -- ", consonant);
                        sb.Append(IpaUtilities.IpaPhonemesMap[consonant]);
                        cbx_phonemeToChange.Items.Add(sb.ToString());
                    }
                    break;
                case 1: // Vowel (single)
                    // Ensure that there is a blank at the top of the drop down list
                    cbx_phonemeToChange.Items.Clear();
                    // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                    // use it to populate the combo box of pulmonic consonants to be changed.
                    foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0} -- ", vowel);
                        string vowelKey = vowel.Trim().Substring(0, 1);
                        sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                        if (vowel.Contains("ː"))
                        {
                            sb.Append(" lengthened");
                        }
                        else if (vowel.Contains("ˑ"))
                        {
                            sb.Append(" half-lengthened");
                        }
                        else if (vowel.Contains("\u032f"))
                        {
                            sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                        }
                        cbx_phonemeToChange.Items.Add(sb.ToString());
                    }

                    cbx_phonemeToChange.SelectedIndex = -1;

                    break;
                case 2: // Vowel diphthongs
                    break;
                case 3: // Rhoticity
                    if (rbn_addRhoticityRegular.Checked)
                    {
                        // Ensure that there is a blank at the top of the drop down list
                        cbx_phonemeToChange.Items.Clear();
                        // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                        // use it to populate the combo box of pulmonic consonants to be changed.
                        foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
                        {
                            if (!((vowel.Contains("ː")) || (vowel.Contains("ˑ")) || (vowel.Contains("\u032f")) || (vowel.Contains("˞")) || (vowel.Equals("ɚ"))))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("{0} -- ", vowel);
                                string vowelKey = vowel.Trim().Substring(0, 1);
                                sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                                if (vowel.Contains("ː"))
                                {
                                    sb.Append(" lengthened");
                                }
                                else if (vowel.Contains("ˑ"))
                                {
                                    sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains("\u032f"))
                                {
                                    sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                                cbx_phonemeToChange.Items.Add(sb.ToString());
                            }
                        }

                        cbx_phonemeToChange.SelectedIndex = -1;
                    }
                    else if (rbn_longToRhotacized.Checked)
                    {
                        // Ensure that there is a blank at the top of the drop down list
                        cbx_phonemeToChange.Items.Clear();
                        // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                        // use it to populate the combo box of pulmonic consonants to be changed.
                        foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
                        {
                            if ((vowel.Contains("ː")) || (vowel.Contains("ˑ")))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("{0} -- ", vowel);
                                string vowelKey = vowel.Trim().Substring(0, 1);
                                sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                                if (vowel.Contains("ː"))
                                {
                                    sb.Append(" lengthened");
                                }
                                else if (vowel.Contains("ˑ"))
                                {
                                    sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains("\u032f"))
                                {
                                    sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                                cbx_phonemeToChange.Items.Add(sb.ToString());
                            }
                        }

                        cbx_phonemeToChange.SelectedIndex = -1;
                    }
                    else if ((rbn_removeRhoticity.Checked) || (rbn_replaceRhotacized.Checked))
                    {
                        // Ensure that there is a blank at the top of the drop down list
                        cbx_phonemeToChange.Items.Clear();
                        // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                        // use it to populate the combo box of pulmonic consonants to be changed.
                        foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
                        {
                            if ((vowel.Contains("˞")) || (vowel.Equals("ɚ")))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("{0} -- ", vowel);
                                string vowelKey = vowel.Trim().Substring(0, 1);
                                sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                                if (vowel.Contains("ː"))
                                {
                                    sb.Append(" lengthened");
                                }
                                else if (vowel.Contains("ˑ"))
                                {
                                    sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains("\u032f"))
                                {
                                    sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                                cbx_phonemeToChange.Items.Add(sb.ToString());
                            }
                        }

                        cbx_phonemeToChange.SelectedIndex = -1;
                    }
                    else if (rbn_replaceRSpelling.Checked)
                    {
                        // Ensure that there is a blank at the top of the drop down list
                        cbx_phonemeToChange.Items.Clear();
                        List<SoundMap> rAddingEntries = phoneticChanger.GetRAddingSoundMapEntries(languageDescription.sound_map_list);
                        cbx_phonemeToChange.Items.AddRange(rAddingEntries.ToArray());
                    }
                    break;
                case 4: // Special Operations
                    break;
                default:
                    break;

            }

        }

        private void updateReplacementPhonemeCbx()
        {
            if (languageDescription == null)
            {
                return;
            }
            int phonemeIndex = cbx_phonemeToChange.SelectedIndex;
            if (phonemeIndex == -1)
            {
                cbx_replacementPhoneme.Items.Clear();
                cbx_replacementPhoneme.SelectedIndex = -1;
            }
            else if (tabPhoneticAlterations.SelectedIndex == 0)
            {
                string consonant;
                {
                    if (phonemeIndex < languageDescription.phonetic_inventory["p_consonants"].Length)
                    {
                        consonant = languageDescription.phonetic_inventory["p_consonants"][phonemeIndex].ToString();
                    }
                    else
                    {
                        consonant = languageDescription.phonetic_inventory["np_consonants"][phonemeIndex - languageDescription.phonetic_inventory["p_consonants"].Length].ToString();
                    }
                }
                cbx_replacementPhoneme.Items.Clear();
                List<string> replacementPhonemes;
                if (rbn_l1.Checked)
                {
                    replacementPhonemes = IpaUtilities.Consonant_changes[consonant];
                }
                else if (rbn_l2.Checked)
                {
                    replacementPhonemes = IpaUtilities.Consonant_changes_l2[consonant];
                }
                else if (rbn_l3.Checked)
                {
                    replacementPhonemes = IpaUtilities.Consonant_changes_l3[consonant];
                }
                else if (rbn_allPhonemes.Checked)
                {
                    replacementPhonemes = new List<string>();
                    replacementPhonemes.AddRange(IpaUtilities.Consonant_changes.Keys);
                }
                else
                {
                    replacementPhonemes = IpaUtilities.Consonant_changes[consonant];
                }
                foreach (string replacement in replacementPhonemes)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0} -- ", replacement);
                    sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                    cbx_replacementPhoneme.Items.Add(sb.ToString());
                }
                cbx_replacementPhoneme.DrawMode = DrawMode.OwnerDrawFixed;
                cbx_replacementPhoneme.MeasureItem += Cbx_replacementPhoneme_MeasureItem;
                cbx_replacementPhoneme.DrawItem += cbx_replacementPhoneme_DrawItem;
            }
            else if (tabPhoneticAlterations.SelectedIndex == 1)
            {
                string vowel;
                vowel = languageDescription.phonetic_inventory["vowels"][phonemeIndex].ToString();
                cbx_replacementPhoneme.Items.Clear();
                if (rbn_normalVowel.Checked)
                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(vowel))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("{0} -- ", replacement);
                            sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
                else if (rbn_halfLongVowels.Checked)
                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(vowel))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("{0}ˑ -- ", replacement);
                            sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            sb.Append(" half-lengthened");
                            cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
                else if (rbn_longVowels.Checked)
                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(vowel))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("{0}ː -- ", replacement);
                            sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            sb.Append(" lengthened");
                            cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
                else if (rbn_normalDiphthong.Checked)
                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{1}{0} -- ", replacement, vowel);
                        sb.Append(IpaUtilities.IpaPhonemesMap[vowel]);
                        sb.Append(", ");
                        sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                        sb.Append(" diphthong");
                        cbx_replacementPhoneme.Items.Add(sb.ToString());
                    }
                }
                else if (rbn_semivowelDiphthong.Checked)
                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(vowel))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("{1}̯{0} -- ", replacement, vowel);
                            sb.Append(IpaUtilities.IpaPhonemesMap[vowel]);
                            sb.Append(" semivowel, ");
                            sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            sb.Append(" diphthong");
                            cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
            }
            else if (tabPhoneticAlterations.SelectedIndex == 3)  // Rhoticity
            {
                cbx_replacementPhoneme.Items.Clear();
                if (rbn_addRhoticityRegular.Checked)
                {
                    string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                    string newPhoneme;
                    string newPhonemeDescription;
                    if (oldPhoneme.Equals("ə"))
                    {
                        newPhoneme = "ɚ";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                        newPhonemeDescription = sb.ToString();
                    }
                    else
                    {
                        newPhoneme = oldPhoneme + "\u02de";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme[0].ToString()]);
                        sb.Append(" Rhotacized");
                        newPhonemeDescription = sb.ToString();
                    }
                    cbx_replacementPhoneme.Items.Add(newPhoneme);
                    cbx_replacementPhoneme.SelectedIndex = 0;
                }
                else if (rbn_longToRhotacized.Checked)
                {
                    string oldPhoneme = cbx_phonemeToChange.Text.Split()[0].Trim()[0].ToString();
                    string newPhoneme;
                    string newPhonemeDescription;
                    if (oldPhoneme.Equals("ə"))
                    {
                        newPhoneme = "ɚ";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                        newPhonemeDescription = sb.ToString();
                    }
                    else
                    {
                        newPhoneme = oldPhoneme + "\u02de";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme[0].ToString()]);
                        sb.Append(" Rhotacized");
                        newPhonemeDescription = sb.ToString();
                    }
                    cbx_replacementPhoneme.Items.Add(newPhonemeDescription);
                    cbx_replacementPhoneme.SelectedIndex = 0;
                }
                else if (rbn_removeRhoticity.Checked)
                {
                    string oldPhoneme = cbx_phonemeToChange.Text.Split()[0].Trim()[0].ToString();
                    string newPhoneme;
                    string newPhonemeDescription;
                    if (oldPhoneme.Equals("ɚ"))
                    {
                        newPhoneme = "ə";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                        newPhonemeDescription = sb.ToString();
                    }
                    else
                    {
                        newPhoneme = oldPhoneme;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                        newPhonemeDescription = sb.ToString();
                    }
                    cbx_replacementPhoneme.Items.Add(newPhonemeDescription);
                    cbx_replacementPhoneme.SelectedIndex = 0;
                }
                else if (rbn_replaceRhotacized.Checked)
                {
                    string oldPhoneme = cbx_phonemeToChange.Text.Split()[0].Trim()[0].ToString();
                    string newPhoneme;
                    string newPhonemeDescription;
                    if (oldPhoneme.Equals("ɚ"))
                    {
                        newPhoneme = "ə";
                    }
                    else
                    {
                        newPhoneme = oldPhoneme;
                    }
                    string[] rConsonants = IpaUtilities.RPhonemes;
                    foreach (string rConsonant in rConsonants)
                    {
                        string newPhoneme2 = newPhoneme + rConsonant;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0}ˑ -- ", newPhoneme2);
                        sb.Append(IpaUtilities.IpaPhonemesMap[rConsonant]);
                        newPhonemeDescription = sb.ToString();
                        cbx_replacementPhoneme.Items.Add(newPhonemeDescription);
                    }
                    cbx_replacementPhoneme.SelectedIndex = -1;
                }
                else if (rbn_replaceRSpelling.Checked)
                {
                    foreach (string phoneme in IpaUtilities.RPhonemes)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("{0} -- ", phoneme);
                        sb.Append(IpaUtilities.IpaPhonemesMap[phoneme]);
                        cbx_replacementPhoneme.Items.Add(sb.ToString());
                    }
                    string phoneme2 = "˞";
                    StringBuilder sb2 = new StringBuilder();
                    sb2.AppendFormat("{0} -- ", phoneme2);
                    sb2.Append(IpaUtilities.IpaPhonemesMap[phoneme2]);
                    cbx_replacementPhoneme.Items.Add(sb2.ToString());
                }
            }
        }

        private void Cbx_replacementPhoneme_MeasureItem(object? sender, MeasureItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btn_applyChangeToLanguage_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex != 0) && (tabPhoneticAlterations.SelectedIndex != 1) && (tabPhoneticAlterations.SelectedIndex != 3))
            {
                return;
            }
            if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
            {
                return;
            }

            if ((tabPhoneticAlterations.SelectedIndex == 0) ||
                (tabPhoneticAlterations.SelectedIndex == 1) ||
                ((tabPhoneticAlterations.SelectedIndex == 3) && (!rbn_replaceRSpelling.Checked)))
            {
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = cbx_replacementPhoneme.Text.Split()[0];
                // Make the change
                phoneticChanger.PhoneticChange(oldPhoneme, newPhoneme);
                // Update sample text
                if (sampleText != string.Empty)
                {
                    sampleText = phoneticChanger.SampleText;
                    txt_SampleText.Text = sampleText;
                    txt_phonetic.Text = string.Empty;
                    foreach (string engineName in speechEngines.Keys)
                    {
                        SpeechEngine speechEngine = speechEngines[engineName];
                        speechEngine.sampleText = sampleText;
                    }
                }
                // Clear the combo boxes
                cbx_phonemeToChange.Items.Clear();
                cbx_replacementPhoneme.Items.Clear();
                tabPhoneticAlterations.SelectedIndex = -1;
            }
            else if ((tabPhoneticAlterations.SelectedIndex == 3) && rbn_replaceRSpelling.Checked)
            {
                if (cbx_phonemeToChange.SelectedItem != null)
                {
                    SoundMap mapToReplace = (SoundMap)cbx_phonemeToChange.SelectedItem;
                    SoundMap replacementMap = mapToReplace.copy();
                    string phonemeToAdd = cbx_replacementPhoneme.Text.Split()[0];
                    string diacriticAtEnd = string.Format("{0}$", IpaUtilities.DiacriticPattern);
                    if (!string.IsNullOrEmpty(replacementMap.spelling_regex))
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(replacementMap.spelling_regex, diacriticAtEnd))
                        {
                            replacementMap.spelling_regex = replacementMap.spelling_regex.Remove(replacementMap.spelling_regex.Length - 1, 1);
                        }
                        replacementMap.spelling_regex += phonemeToAdd;
                    }
                    if (!string.IsNullOrEmpty(replacementMap.phoneme))
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(replacementMap.phoneme, diacriticAtEnd))
                        {
                            replacementMap.phoneme = replacementMap.phoneme.Remove(replacementMap.phoneme.Length - 1, 1);
                        }
                        replacementMap.phoneme += phonemeToAdd;
                    }
                    phoneticChanger.ReplaceSoundMapEntry(mapToReplace, replacementMap, languageDescription.sound_map_list);
                    phoneticChanger.updatePronunciation();
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speech = speechEngines[engineName];
                            speech.sampleText = sampleText;
                        }
                    }
                    // Clear the combo boxes
                    cbx_phonemeToChange.Items.Clear();
                    cbx_replacementPhoneme.Items.Clear();
                    tabPhoneticAlterations.SelectedIndex = -1;
                }
            }
            // TODO: Add other options
        }

        private void cbx_replacementPhoneme_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
        {
            e.ItemWidth = cbx_phonemeToChange.DropDownWidth;
            e.ItemHeight = cbx_replacementPhoneme.Font.Height + 10;
        }

        private void cbx_replacementPhoneme_DrawItem(object? sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
                return;
            }
            Brush comboBrush = Brushes.Black; // Set the default color to black

            string cbxEntry = string.Empty;
            if ((cbx_replacementPhoneme.Items[e.Index] != null) && (cbx_replacementPhoneme.Items[e.Index] is string))
            {
#pragma warning disable CS8600
                cbxEntry = (string)cbx_replacementPhoneme.Items[e.Index];
#pragma warning restore CS8600
                if (cbxEntry == null)
                {
                    cbxEntry = string.Empty;
                }
            }
            bool inInventory = IsInInventory(cbxEntry);
            bool hasSpellingMap = HasSpellingMap(cbxEntry);
            if (inInventory && hasSpellingMap)
            {
                comboBrush = Brushes.Blue;
            }
            else if (inInventory)
            {
                comboBrush = Brushes.Green;
            }
            else if (hasSpellingMap)
            {
                comboBrush = Brushes.Red;
            }

            e.DrawBackground();
            Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2, e.Bounds.Height, e.Bounds.Height - 4);
            e.Graphics.DrawString(cbxEntry, cbx_replacementPhoneme.Font, comboBrush,
                new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
        }

        private bool IsInInventory(string cbxEntry)
        {
            if (languageDescription == null)
            {
                return false;
            }
            bool isInInventory = false;
            string checkChar = cbxEntry.Split()[0]; // The combo boxes to be checked always have the character first, followed by whitespace
            if (tabPhoneticAlterations.SelectedIndex == 0)
            {
                isInInventory = languageDescription.phonetic_inventory["p_consonants"].Contains(checkChar);
            }

            return isInInventory;
        }
        private bool HasSpellingMap(string cbxEntry)
        {
            if (languageDescription == null)
            {
                return false;
            }
            bool hasSpellingMap = false;
            string checkChar = cbxEntry.Split()[0]; // The combo boxes to be checked always have the character first, followed by whitespace
            if (checkChar.Length == 1)
            {
                char c = checkChar[0];
                if (Char.IsLetter(c))
                {
                    hasSpellingMap = true;
                }
            }
            else
            {
                foreach (SoundMap soundMap in languageDescription.sound_map_list)
                {
                    if ((soundMap.phoneme.Contains(checkChar)) || (soundMap.spelling_regex.Contains(checkChar)))
                    {
                        hasSpellingMap = true;
                    }
                }
            }

            return hasSpellingMap;
        }

        private void btn_addCurrentChangeToList_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex != 0) && (tabPhoneticAlterations.SelectedIndex != 1) && (tabPhoneticAlterations.SelectedIndex != 3))
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex == 3) && (rbn_replaceRSpelling.Checked))
            {
                MessageBox.Show("Rhotacize based on spelling can only be performed singly", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
            {
                return;
            }
            if (changesToBeMade.Count >= 10)
            {
                MessageBox.Show("No more than 10 changes can be grouped for simultaneous update.  Apply existing updates before adding additional changes",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex == 0) || (tabPhoneticAlterations.SelectedIndex == 1) || (tabPhoneticAlterations.SelectedIndex == 3))
            {
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = cbx_replacementPhoneme.Text.Split()[0];

                changesToBeMade.Add((oldPhoneme, newPhoneme));
                StringBuilder sb = new StringBuilder();
                foreach ((string oph, string nph) in changesToBeMade)
                {
                    sb.Append(oph);
                    sb.Append(" -> ");
                    sb.Append(nph);
                    sb.Append(", ");
                }
                txt_changeList.Text = sb.ToString();
            }

        }

        private void btn_applyListOfChanges_Click(object sender, EventArgs e)
        {
            phoneticChanger.PhoneticChange(changesToBeMade);
            if (sampleText != string.Empty)
            {
                sampleText = phoneticChanger.SampleText;
                txt_SampleText.Text = sampleText;
                txt_phonetic.Text = string.Empty;
                foreach (string engineName in speechEngines.Keys)
                {
                    SpeechEngine speech = speechEngines[engineName];
                    speech.sampleText = sampleText;
                }
            }
            // Clear the combo boxes
            cbx_phonemeToChange.Items.Clear();
            cbx_replacementPhoneme.Items.Clear();
            changesToBeMade.Clear();
            txt_changeList.Text = string.Empty;
            cbx_phonemeToChange.SelectedIndex = -1;
            cbx_replacementPhoneme.SelectedIndex = -1;
            tabPhoneticAlterations.SelectedIndex = -1;
        }

        private void btn_revertLastChange_Click(object sender, EventArgs e)
        {
            phoneticChanger.RevertMostRecentChange();
            txt_phonetic.Text = string.Empty;
            if (!string.IsNullOrEmpty(phoneticChanger.SampleText))
            {
                sampleText = phoneticChanger.SampleText;
                txt_SampleText.Text = sampleText;
            }
            else
            {
                txt_SampleText.Text = string.Empty;
            }
        }

        private void displayGlossOfSampleTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sampleText))
            {
                return;
            }
            if (languageDescription == null)
            {
                return;
            }
            string glossText = LatinUtilities.GlossText(sampleText, languageDescription, this);

            MessageBox.Show(glossText, "Glossed Text", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void printSampleTextSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sampleText))
            {
                return;
            }
            if (languageDescription == null)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(WrapText(sampleText, 80));
            sb.AppendLine("\n------------------------------------------------------------------------");
            sb.AppendLine("Gloss:");
            string gloss = LatinUtilities.GlossText(sampleText, languageDescription, this);
            sb.AppendLine(WrapText(gloss, 80));
            sb.AppendLine("\n------------------------------------------------------------------------");
            sb.AppendLine("Phonetic:");
            if (string.IsNullOrEmpty(txt_phonetic.Text))
            {
                string speed = cbx_speed.Text.Trim();
                string engineName = cbx_speechEngine.Text.Trim();
                speechEngines[engineName].Generate(speed, this);
                txt_phonetic.Text = speechEngines[engineName].phoneticText;
            }
            sb.AppendLine(txt_phonetic.Text.Trim());
            StringReader sampleTextSummaryReader = new StringReader(sb.ToString());
            readerToPrint = sampleTextSummaryReader;
            printFont = new Font("Charis SIL", 12.0f);
            try
            {
                PrintDialog printDialog = new PrintDialog();
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                printDialog.Document = pd;
                DialogResult result = printDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pd.Print();
                }
            }
            finally
            {
                readerToPrint.Close();
            }
        }

        private void printKiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> missingPhonemes = KirshenbaumUtilities.UnmappedPhonemes;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string phoneme in missingPhonemes)
            {
                if (IpaUtilities.IpaPhonemesMap.ContainsKey(phoneme))
                {
                    StringBuilder sb2 = new StringBuilder();
                    foreach (char c in phoneme)
                    {
                        if (Char.IsAsciiLetterOrDigit(c))
                        {
                            sb2.Append(c);
                        }
                        else
                        {
                            int cInt = (int)c;
                            sb2.AppendFormat("U+{0,4:x4}", cInt);
                        }
                    }
                    stringBuilder.AppendFormat("\"{0} ({2}):\t{1}\n", phoneme, IpaUtilities.IpaPhonemesMap[phoneme], sb2.ToString());
                }
            }
            StringReader sampleTextSummaryReader = new StringReader(stringBuilder.ToString());
            readerToPrint = sampleTextSummaryReader;
            printFont = new Font("Charis SIL", 12.0f);
            try
            {
                PrintDialog printDialog = new PrintDialog();
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                printDialog.Document = pd;
                DialogResult result = printDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pd.Print();
                }
            }
            finally
            {
                readerToPrint.Close();
            }
        }

        private void cbx_speechEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVoices();
        }

        private void btn_updateSoundMapList_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            SoundMapListEditor soundMapListEditor = new SoundMapListEditor();
            List<SoundMap> soundMapList = languageDescription.sound_map_list.GetRange(0, languageDescription.sound_map_list.Count);
            soundMapListEditor.SoundMapList = languageDescription.sound_map_list;
            soundMapListEditor.headerText = "No specific changes - editing the entire list";
            soundMapListEditor.UpdatePhonemeReplacements();
            soundMapListEditor.ShowDialog();
            // ShowDialog is modal
            if (soundMapListEditor.SoundMapSaved)
            {
                DialogResult result = MessageBox.Show("Preserve the spelling (Yes)?\nNo preserves the pronunciation.", "Spelling or pronunciation", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    languageDescription.sound_map_list = soundMapListEditor.SoundMapList;
                    phoneticChanger.updatePronunciation();
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speech = speechEngines[engineName];
                            speech.sampleText = sampleText;
                        }
                    }
                }
                else if (result == DialogResult.No)
                {
                    phoneticChanger.updateSpelling();
                    languageDescription.sound_map_list = soundMapListEditor.SoundMapList;
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speech = speechEngines[engineName];
                            speech.sampleText = sampleText;
                        }
                    }
                }
            }

        }

        private void rbn_l1_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_normalVowel_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_addRhoticityRegular_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_l2_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_l3_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_allPhonemes_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_halfLongVowels_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_longVowels_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_normalDiphthong_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_semivowelDiphthong_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_longToRhotacized_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_removeRhoticity_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_replaceRhotacized_CheckedChanged(object sender, EventArgs e)
        {
            updatePhonemeToChangeCbx();
        }

        private void rbn_replaceRSpelling_CheckedChanged(object sender, EventArgs e)
        {
            if (rbn_replaceRSpelling.Checked)
            {
                lbl_choiceCbx.Text = "Pronunciation Patter to Update";
                lbl_replacementCbx.Text = "Phoneme to add/replace";
                updatePhonemeToChangeCbx();
            }
            else
            {
                lbl_choiceCbx.Text = "Phoneme to change";
                lbl_replacementCbx.Text = "Replacement Phoneme";
            }
        }

        private void setAmazonPollyURIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pollyURI = Microsoft.VisualBasic.Interaction.InputBox("Enter the URI for Amazon Polly", "Amazon Polly URI", PollySpeech.PollyURI ?? "");
            if (PollySpeech.TestURI(pollyURI))
            {
                PollySpeech.PollyURI = pollyURI;
                PollySpeech pollySpeech = new PollySpeech();
                Dictionary<string, SpeechEngine.VoiceData> amazonPollyVoices = pollySpeech.getVoices();
                if (speechEngines.ContainsKey(pollySpeech.Description))
                {
                    speechEngines[pollySpeech.Description] = pollySpeech;
                }
                else
                {
                    speechEngines.Add(pollySpeech.Description, pollySpeech);
                }
                if (voices.ContainsKey(pollySpeech.Description))
                {
                    voices[pollySpeech.Description] = amazonPollyVoices;
                }
                else
                {
                    voices.Add(pollySpeech.Description, amazonPollyVoices);
                }
            }
        }

        private void SetESpeakNgLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new())
            {
                if(string.IsNullOrEmpty(ESpeakNGSpeak.ESpeakNGPath))
                {
                    openFileDialog.InitialDirectory = Environment.SpecialFolder.ProgramFiles.ToString();

                }
                else
                {
                    FileInfo fi = new FileInfo(ESpeakNGSpeak.ESpeakNGPath);
                    openFileDialog.InitialDirectory = fi.DirectoryName;
                }
                openFileDialog.Filter = "Executable file (*.exe)|*.exe|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = false;
                openFileDialog.CheckFileExists = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ESpeakNGSpeak.ESpeakNGPath = openFileDialog.FileName;
                    ESpeakNGSpeak eSpeakNGSpeak = new ESpeakNGSpeak();
                    Dictionary<string, SpeechEngine.VoiceData> espeakVoices = eSpeakNGSpeak.getVoices();
                    if (speechEngines.ContainsKey(eSpeakNGSpeak.Description))
                    {
                        speechEngines[eSpeakNGSpeak.Description] = eSpeakNGSpeak;
                    }
                    else
                    {
                        speechEngines.Add(eSpeakNGSpeak.Description, eSpeakNGSpeak);
                    }
                    if (voices.ContainsKey(eSpeakNGSpeak.Description))
                    {
                        voices[eSpeakNGSpeak.Description] = espeakVoices;
                    }
                    else
                    {
                        voices.Add(eSpeakNGSpeak.Description, espeakVoices);
                    }
                    //eSpeakNGSpeak.Test();
                }
            }

        }
    }
}

