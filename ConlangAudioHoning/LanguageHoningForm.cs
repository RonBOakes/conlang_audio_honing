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
        private readonly Dictionary<string, SpeechEngine> speechEngines = [];
        private readonly Dictionary<string, Dictionary<string, SpeechEngine.VoiceData>> voices =
            [];
        private Font? printFont;
        private TextReader? readerToPrint;
        private readonly Dictionary<string, FileInfo> speechFiles = [];
        private readonly PhoneticChanger phoneticChanger;
        private readonly List<(string, string)> changesToBeMade = [];
        private readonly UserConfiguration config;

        /// <summary>
        /// Provides access to the ProgressBar on the main form so that it can be accessed by other classes and methods.
        /// </summary>
        public System.Windows.Forms.ProgressBar ProgressBar { get; private set; }

        private static readonly string[] SpeakingSpeeds = ["x-slow", "slow", "medium", "fast", "x-fast"];

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
                ESpeakNGSpeak eSpeakNGSpeak = new();
                Dictionary<string, SpeechEngine.VoiceData> espeakVoices = eSpeakNGSpeak.GetVoices();
                speechEngines.Add(eSpeakNGSpeak.Description, eSpeakNGSpeak);
                voices.Add(eSpeakNGSpeak.Description, espeakVoices);
            }

            if (config.IsPollySupported)
            {
                PollySpeech.PollyURI = config.PollyURI;
                PollySpeech pollySpeech = new();
                Dictionary<string, SpeechEngine.VoiceData> amazonPollyVoices = pollySpeech.GetVoices();
                speechEngines.Add(pollySpeech.Description, pollySpeech);
                voices.Add(pollySpeech.Description, amazonPollyVoices);
            }

            phoneticChanger = new PhoneticChanger();

            changesToBeMade.Clear();
            LoadSpeechEngines();
            LoadVoices();
            LoadSpeeds();
            tabPhoneticAlterations.SelectedIndexChanged += TabPhoneticAlterations_SelectedIndexChanged;

            // Hide controls on the Diphthong tab that only show up when certain radio buttons are pressed
            lbl_diphthongStartVowel.Visible = false;
            cbx_dipthongStartVowel.Visible = false;
            lbl_DiphthongEndVowel.Visible = false;
            cbx_dipthongEndVowel.Visible = false;

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            ProgressBar = new();
        }

        private void TabPhoneticAlterations_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
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
            ProgressBar.Style = ProgressBarStyle.Continuous;
            ProgressBar.BringToFront();
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = language.lexicon.Count;
            ProgressBar.Step = 1;
            ProgressBar.Font = new Font("CharisSIL", 12f);
            ProgressBar.ResetText();
            ProgressBar.Visible = true;
            List<LexiconEntry> addLexicon = [];
            foreach (LexiconEntry word in language.lexicon)
            {
                ProgressBar.Text = word.phonetic.ToString();
                addLexicon.AddRange(ConlangUtilities.DeclineWord(word, language.affix_map, language.sound_map_list));
                ProgressBar.PerformStep();
            }
            ProgressBar.Style = ProgressBarStyle.Marquee;
            pbTimer.Interval = 20;
            pbTimer.Enabled = true;
            ProgressBar.MarqueeAnimationSpeed = 200;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 100;
            language.lexicon.AddRange(addLexicon);
            ProgressBar.Visible = false;
            ProgressBar.SendToBack();
            pbTimer.Enabled = false;
            language.declined = true;
        }

        private void LoadSpeechEngines()
        {
            cbx_speechEngine.SuspendLayout();
            cbx_speechEngine.Items.Clear();
            foreach (string engineName in speechEngines.Keys)
            {
                _ = cbx_speechEngine.Items.Add(engineName);
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
                    _ = cbx_voice.Items.Add(voiceMenu);
                }
                if ((languageDescription != null) &&
                    (languageDescription.preferred_voices.TryGetValue(speechEngines[selectedEngine].PreferredVoiceKey, out string? value)) &&
                    (!string.IsNullOrEmpty(value)))
                {
                    string preferredVoice = value.Trim();
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
            cbx_speed.Items.AddRange(SpeakingSpeeds);
            cbx_speed.ResumeLayout();
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
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

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog saveFileDialog = new();
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
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
                _ = MessageBox.Show("Unable to decode Language file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (languageDescription.version != 1.0)
            {
                _ = MessageBox.Show("Incorrect language file version", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                languageDescription = new LanguageDescription();
                return;
            }

            languageFileInfo = new FileInfo(filename);

            // Temporary work around to put the Polly voice into the preferred voices map.  
            // Once the JSON structure is reworked, this will not be needed.
            _ = languageDescription.preferred_voices.TryAdd("Polly", "Brian");
            _ = languageDescription.preferred_voices.TryAdd("espeak-ng", "en-us");
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
            rbn_vowelToDiphthongStart.Checked = true;
            UpdatePhonemeToChangeCbx();
            LoadVoices();
        }

        private readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private void SaveLanguage(string filename)
        {
            if (languageDescription == null)
            {
                _ = MessageBox.Show("Unable to save language File, no language information present", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                JsonSerializerOptions jsonSerializerOptions = JsonSerializerOptions;
                JavaScriptEncoder encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                jsonSerializerOptions.Encoder = encoder;
                jsonSerializerOptions.WriteIndented = true;

                string jsonString = JsonSerializer.Serialize<LanguageDescription>(languageDescription, jsonSerializerOptions);
                File.WriteAllText(filename, jsonString, System.Text.Encoding.UTF8);
            }
        }

        private void LoadSampleTextFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
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

        private void LoadSampleText(string fileName)
        {
            string fileText = File.ReadAllText(fileName);

            sampleText = fileText;
            txt_SampleText.Text = sampleText;
            foreach (string engineKey in speechEngines.Keys)
            {
                speechEngines[engineKey].SampleText = sampleText;
            }
            phoneticChanger.SampleText = sampleText;
        }

        private void SaveSampleMenu_Click(object sender, EventArgs e)
        {
            using SaveFileDialog saveFileDialog = new();
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

        private void SaveSampleText(string filename)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
            File.WriteAllText(filename, sampleText);
        }

        private void OnApplicationExit(object? sender, EventArgs e)
        {
            config.SaveConfiguration();
        }

        private void Txt_SampleText_TextChanged(object sender, EventArgs e)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
        }

        private void Btn_generate_Click(object sender, EventArgs e)
        {
            if ((languageDescription != null) && (!string.IsNullOrEmpty(sampleText)))
            {
                string engineKey = cbx_speechEngine.Text.Trim();
                SpeechEngine engine = speechEngines[engineKey];
                string speed = cbx_speed.Text.Trim();
                engine.Generate(speed, this);
                txt_phonetic.Text = engine.PhoneticText;
            }
        }

        private void DeclineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if (!languageDescription.declined)
            {
                DeclineLexicon(languageDescription);
                declineToolStripMenuItem.Text = "Remove Declensions";
            }
            else
            {
                ConlangUtilities.removeDeclinedEntries(languageDescription);
                declineToolStripMenuItem.Text = "Decline Language";
            }

        }

        private void Btn_generateSpeech_Click(object sender, EventArgs e)
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
                    _ = MessageBox.Show("Unable to generate speech file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txt_phonetic.Text = pollySpeech.PhoneticText;
                    // Play the audio (MP3) file with the Windows Media Player
                    WMPLib.WindowsMediaPlayer player = new()
                    {
                        URL = targetFileName
                    };
                    player.controls.play();

                    FileInfo fileInfo = new(targetFileName);
                    speechFiles.Add(fileInfo.Name, fileInfo);
                    _ = cbx_recordings.Items.Add(fileInfo.Name);
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
                if (voices[engineName].TryGetValue(voiceKey, out SpeechEngine.VoiceData value))
                {
                    SpeechEngine.VoiceData voiceData = value;
                    voice = voiceData.LanguageCode;
                }
                string speed = cbx_speed.Text.Trim();
                speechEngine.SampleText = sampleText;
                bool ok = speechEngine.GenerateSpeech(targetFileName, voice, speed, this);
                if (!ok)
                {
                    _ = MessageBox.Show("Unable to generate speech file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txt_phonetic.Text = speechEngine.PhoneticText;
                    // Play the audio (wav) file with the Windows Media Player
                    WMPLib.WindowsMediaPlayer player = new()
                    {
                        URL = targetFileName
                    };
                    player.controls.play();

                    FileInfo fileInfo = new(targetFileName);
                    speechFiles.Add(fileInfo.Name, fileInfo);
                    _ = cbx_recordings.Items.Add(fileInfo.Name);
                    cbx_recordings.SelectedText = fileInfo.Name;
                }
            }
        }

        private void DisplayPulmonicConsonantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new();
            foreach (string c in IpaUtilities.PConsonants)
            {
                _ = sb.Append(c);
                _ = sb.Append(' ');
            }
            txt_SampleText.Text = sb.ToString();
            txt_phonetic.Text = sb.ToString();
        }

        private static string WrapText(string text, int cols)
        {
            int col = 0;
            StringBuilder sb = new();
            using (StringReader textReader = new(text))
            {
                string? line;
                do
                {
                    line = textReader.ReadLine();
                    if ((line != null) && (!line.Trim().Equals(string.Empty)))
                    {
#pragma warning disable S3220 // Method calls should not resolve ambiguously to overloads with "params"
                        foreach (string word in line.Split(separator: null))
                        {
                            _ = sb.Append(word);
                            col += word.Length;
                            if (col < ((cols * 8) / 10))
                            {
                                _ = sb.Append(' ');
                                col += 1;
                            }
                            else
                            {
                                _ = sb.AppendLine();
                                col = 0;
                            }
                        }
#pragma warning restore S3220 // Method calls should not resolve ambiguously to overloads with "params"
                    }
                }
                while (line != null);
            }


            return sb.ToString();
        }

        // The PrintPage event is raised for each page to be printed.
        private void Pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            if ((readerToPrint == null) || (printFont == null) || (ev == null))
            {
                return;
            }

            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string? line = string.Empty;

            // Calculate the number of lines per page.
#pragma warning disable CS8604 // Possible null reference argument.
            float linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
#pragma warning restore CS8604 // Possible null reference argument.

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = readerToPrint.ReadLine()) != null))
            {
                float yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
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

        private void PrintIPAMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaPhonemes = IpaUtilities.IpaPhonemes();
            StringReader ipaPhonemeReader = new(ipaPhonemes);
            readerToPrint = ipaPhonemeReader;
            try
            {
                PrintDialog printDialog = new();
                printFont = new Font("Charis SIL", 12f);
                PrintDocument pd = new();
                pd.PrintPage += new PrintPageEventHandler(this.Pd_PrintPage);
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

        private void DisplaySupersegmentals_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new();
            foreach (string c in IpaUtilities.Suprasegmentals)
            {
                _ = sb.Append(c);
                _ = sb.Append(' ');
            }
            txt_SampleText.Text = sb.ToString();
            txt_phonetic.Text = sb.ToString();
        }

        private void PbTimer_Tick(object sender, EventArgs e)
        {
            ProgressBar.PerformStep();
        }

        private void Btn_replaySpeech_Click(object sender, EventArgs e)
        {
            string fileName = cbx_recordings.Text;
            string targetFileName = speechFiles[fileName].FullName;
            // Play the audio (OGG) file with the default application
            WMPLib.WindowsMediaPlayer player = new()
            {
                URL = targetFileName
            };
            player.controls.play();
        }

        private void Cbx_phonemeToChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReplacementPhonemeCbx();
        }

        private void UpdatePhonemeToChangeCbx()
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
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0} -- ", consonant);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[consonant]);
                        _ = cbx_phonemeToChange.Items.Add(sb.ToString());
                    }
                    foreach (string consonant in languageDescription.phonetic_inventory["np_consonants"])
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0} -- ", consonant);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[consonant]);
                        _ = cbx_phonemeToChange.Items.Add(sb.ToString());
                    }
                    cbx_dipthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_dipthongEndVowel.Visible = false;
                    lbl_DiphthongEndVowel.Visible = false;
                    cbx_replacementPhoneme.Visible = true;
                    lbl_replacementCbx.Visible = true;
                    break;
                case 1: // Vowel (single)
                    // Ensure that there is a blank at the top of the drop down list
                    cbx_phonemeToChange.Items.Clear();
                    // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                    // use it to populate the combo box of pulmonic consonants to be changed.
                    foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0} -- ", vowel);
                        string vowelKey = vowel.Trim()[..1];
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                        if (vowel.Contains('ː'))
                        {
                            _ = sb.Append(" lengthened");
                        }
                        else if (vowel.Contains('ˑ'))
                        {
                            _ = sb.Append(" half-lengthened");
                        }
                        else if (vowel.Contains('̯'))
                        {
                            _ = sb.Append(" semi-diphthong");  // Probably should be part of a diphthong
                        }
                        _ = cbx_phonemeToChange.Items.Add(sb.ToString());
                    }

                    cbx_dipthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_dipthongEndVowel.Visible = false;
                    lbl_DiphthongEndVowel.Visible = false;
                    cbx_replacementPhoneme.Visible = true;
                    lbl_replacementCbx.Visible = true;

                    cbx_phonemeToChange.SelectedIndex = -1;

                    break;
                case 2: // Vowel diphthongs
                    cbx_phonemeToChange.Items.Clear();
                    // The SpeakingSpeeds to go into the combo box depend on the selected radio button
                    if (rbn_vowelToDiphthongStart.Checked || rbn_vowelToDipthongEnd.Checked)
                    {
                        // Populate with vowels
                        // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                        // use it to populate the combo box of pulmonic consonants to be changed.
                        foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0} -- ", vowel);
                            string vowelKey = vowel.Trim()[..1];
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                            if (vowel.Contains('ː'))
                            {
                                _ = sb.Append(" lengthened");
                            }
                            else if (vowel.Contains('ˑ'))
                            {
                                _ = sb.Append(" half-lengthened");
                            }
                            else if (vowel.Contains('̯'))
                            {
                                _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                            }
                            _ = cbx_phonemeToChange.Items.Add(sb.ToString());
                        }
                    }
                    else
                    {
                        // populate with diphthongs
                        // When the language was loaded, the phonetic inventory was built, or rebuilt, so we can
                        // use it to populate the combo box of pulmonic consonants to be changed.
                        foreach (string diphthong in languageDescription.phonetic_inventory["v_diphthongs"])
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0} -- ", diphthong);
                            string vowelKey = diphthong.Trim()[..1];
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                            vowelKey = diphthong.Trim()[1..2];
                            if (IpaUtilities.Suprasegmentals.Contains(vowelKey)) // If this is an IPA suprasegmental
                            {
                                if (vowelKey == "ː")
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowelKey == "ˑ")
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowelKey == "\u032f")
                                {
                                    _ = sb.Append(" semi-vowel");
                                }
                                vowelKey = diphthong.Trim()[2..3];
                            }
                            _ = sb.AppendFormat(" to {0}", IpaUtilities.IpaPhonemesMap[vowelKey]);
                            if ((diphthong.Trim().Length > 3) || ((diphthong.Trim().Length == 3) && (diphthong.Trim()[1..2].Equals(vowelKey))))
                            {
                                vowelKey = diphthong.Trim()[^1..];
                                if (vowelKey == "ː")
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowelKey == "ˑ")
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowelKey == "\u032f")
                                {
                                    _ = sb.Append(" semi-vowel");
                                }
                            }
                            _ = cbx_phonemeToChange.Items.Add(sb.ToString());
                        }
                    }

                    if (rbn_diphthongReplacement.Checked)
                    {
                        cbx_dipthongStartVowel.Visible = true;
                        lbl_diphthongStartVowel.Visible = true;
                        cbx_dipthongEndVowel.Visible = true;
                        lbl_DiphthongEndVowel.Visible = true;
                        cbx_replacementPhoneme.Visible = false;
                        lbl_replacementCbx.Visible = false;
                    }
                    else
                    {
                        cbx_dipthongStartVowel.Visible = false;
                        lbl_diphthongStartVowel.Visible = false;
                        cbx_dipthongEndVowel.Visible = false;
                        lbl_DiphthongEndVowel.Visible = false;
                        cbx_replacementPhoneme.Visible = true;
                        lbl_replacementCbx.Visible = true;
                    }
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
                            if (!((vowel.Contains('ː')) || (vowel.Contains('ˑ')) || (vowel.Contains('̯')) || (vowel.Contains('˞')) || (vowel.Equals("ɚ"))))
                            {
                                StringBuilder sb = new();
                                _ = sb.AppendFormat("{0} -- ", vowel);
                                string vowelKey = vowel.Trim()[..1];
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-diphthong");  // Probably should be part of a diphthong
                                }
                                _ = cbx_phonemeToChange.Items.Add(sb.ToString());
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
                            if ((vowel.Contains('ː')) || (vowel.Contains('ˑ')))
                            {
                                StringBuilder sb = new();
                                _ = sb.AppendFormat("{0} -- ", vowel);
                                string vowelKey = vowel.Trim()[..1];
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-diphthong");  // Probably should be part of a diphthong
                                }
                                _ = cbx_phonemeToChange.Items.Add(sb.ToString());
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
                            if ((vowel.Contains('˞')) || (vowel.Equals("ɚ")))
                            {
                                StringBuilder sb = new();
                                _ = sb.AppendFormat("{0} -- ", vowel);
                                string vowelKey = vowel.Trim()[..1];
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[vowelKey]);
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-diphthong");  // Probably should be part of a diphthong
                                }
                                _ = cbx_phonemeToChange.Items.Add(sb.ToString());
                            }
                        }

                    }
                    else if (rbn_replaceRSpelling.Checked)
                    {
                        // Ensure that there is a blank at the top of the drop down list
                        cbx_phonemeToChange.Items.Clear();
                        List<SoundMap> rAddingEntries = PhoneticChanger.GetRAddingSoundMapEntries(languageDescription.sound_map_list);
                        cbx_phonemeToChange.Items.AddRange(rAddingEntries.ToArray());
                    }
                    cbx_dipthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_dipthongEndVowel.Visible = false;
                    lbl_DiphthongEndVowel.Visible = false;
                    cbx_replacementPhoneme.Visible = true;
                    lbl_replacementCbx.Visible = true;
                    break;
                case 4: // Special Operations
                    cbx_dipthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_dipthongEndVowel.Visible = false;
                    lbl_DiphthongEndVowel.Visible = false;
                    cbx_replacementPhoneme.Visible = true;
                    lbl_replacementCbx.Visible = true;
                    break;
                default:
                    break;

            }
            cbx_phonemeToChange.SelectedIndex = -1;

        }

        private void UpdateReplacementPhonemeCbx()
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
                if (phonemeIndex < languageDescription.phonetic_inventory["p_consonants"].Length)
                {
                    consonant = languageDescription.phonetic_inventory["p_consonants"][phonemeIndex].ToString();
                }
                else
                {
                    consonant = languageDescription.phonetic_inventory["np_consonants"][phonemeIndex - languageDescription.phonetic_inventory["p_consonants"].Length].ToString();
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
                    replacementPhonemes = [.. IpaUtilities.Consonant_changes.Keys];
                }
                else
                {
                    replacementPhonemes = IpaUtilities.Consonant_changes[consonant];
                }
                foreach (string replacement in replacementPhonemes)
                {
                    StringBuilder sb = new();
                    _ = sb.AppendFormat("{0} -- ", replacement);
                    _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                    _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                }
                cbx_replacementPhoneme.DrawMode = DrawMode.OwnerDrawFixed;
                cbx_replacementPhoneme.MeasureItem += Cbx_replacementPhoneme_MeasureItem;
                cbx_replacementPhoneme.DrawItem += Cbx_replacementPhoneme_DrawItem;
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
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0} -- ", replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
                else if (rbn_halfLongVowels.Checked)
                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(vowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}ˑ -- ", replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            _ = sb.Append(" half-lengthened");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
                else if (rbn_longVowels.Checked)

                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(vowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}ː -- ", replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            _ = sb.Append(" lengthened");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
                else if (rbn_semivowelDiphthong.Checked)
                {
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(vowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{1}̯{0} -- ", replacement, vowel);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[vowel]);
                            _ = sb.Append(" semivowel, ");
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            _ = sb.Append(" diphthong");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                }
            }
            else if (tabPhoneticAlterations.SelectedIndex == 2) // Diphthongs
            {
                cbx_replacementPhoneme.Items.Clear();
                if (rbn_vowelToDiphthongStart.Checked)
                {
                    // Build a list of potential diphthongs from this vower
                    string vowel = languageDescription.phonetic_inventory["vowels"][phonemeIndex].ToString();
                    // Trim off any supersegmentals or diacritics
                    string rootVowel = vowel[0..1];
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(rootVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", vowel, replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                            if (vowel.Trim().Length > 1)
                            {
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-diphthong");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            _ = sb.Append(" diphthong");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ː';
                        if (!replacementVowel.Equals(rootVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", vowel, replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                            if (vowel.Trim().Length > 1)
                            {
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel]);
                            _ = sb.Append(" lengthened");
                            _ = sb.Append(" diphthong");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ˑ';
                        if (!replacementVowel.Equals(rootVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", vowel, replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                            if (vowel.Trim().Length > 1)
                            {
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel]);
                            _ = sb.Append(" half-lengthened");
                            _ = sb.Append(" diphthong");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    if (!vowel.Contains('̯'))
                    {
                        foreach (string replacementVowel in IpaUtilities.Vowels)
                        {
                            string replacement = replacementVowel + '̯';
                            if (!replacement.Equals(rootVowel))
                            {
                                StringBuilder sb = new();
                                _ = sb.AppendFormat("{0}{1} -- ", vowel, replacement);
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                                if (vowel.Trim().Length > 1)
                                {
                                    if (vowel.Contains('ː'))
                                    {
                                        _ = sb.Append(" lengthened");
                                    }
                                    else if (vowel.Contains('ˑ'))
                                    {
                                        _ = sb.Append(" half-lengthened");
                                    }
                                    else if (vowel.Contains('̯'))
                                    {
                                        _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                    }
                                }
                                _ = sb.Append(' ');
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel]);
                                _ = sb.Append(" semi-vowel");
                                _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                            }
                        }
                    }
                }
                else if (rbn_vowelToDipthongEnd.Checked)
                {
                    // Build a list of potential diphthongs from this vowel
                    string vowel = languageDescription.phonetic_inventory["vowels"][phonemeIndex].ToString();
                    // Trim off any supersegmentals or diacritics
                    string rootVowel = vowel[0..1];
                    foreach (string replacement in IpaUtilities.Vowels)
                    {
                        if (!replacement.Equals(rootVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", replacement, vowel);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement]);
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                            if (vowel.Trim().Length > 1)
                            {
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-diphthong");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(" diphthong");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ː';
                        if (!replacementVowel.Equals(rootVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", replacement, vowel);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel]);
                            _ = sb.Append(" lengthened");
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                            if (vowel.Trim().Length > 1)
                            {
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-diphthong");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(" diphthong");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ˑ';
                        if (!replacementVowel.Equals(rootVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", replacement, vowel);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel]);
                            _ = sb.Append(" half-lengthened");
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                            if (vowel.Trim().Length > 1)
                            {
                                if (vowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (vowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (vowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(" diphthong");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    if (!vowel.Contains('̯'))
                    {
                        foreach (string replacementVowel in IpaUtilities.Vowels)
                        {
                            string replacement = replacementVowel + '̯';
                            if (!replacement.Equals(rootVowel))
                            {
                                StringBuilder sb = new();
                                _ = sb.AppendFormat("{0}{1} -- ", replacement, vowel);
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel]);
                                _ = sb.Append(" semi-vowel");
                                _ = sb.Append(' ');
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[rootVowel]);
                                if (vowel.Trim().Length > 1)
                                {
                                    if (vowel.Contains('ː'))
                                    {
                                        _ = sb.Append(" lengthened");
                                    }
                                    else if (vowel.Contains('ˑ'))
                                    {
                                        _ = sb.Append(" half-lengthened");
                                    }
                                    else if (vowel.Contains('̯'))
                                    {
                                        _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                    }
                                }
                                _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                            }
                        }
                    }
                }
                else if (rbn_changeStartVowel.Checked)
                {
                    string diphthong = languageDescription.phonetic_inventory["v_diphthongs"][phonemeIndex];
                    string endVowel = diphthong[^1..];
                    if ((IpaUtilities.Suprasegmentals.Contains(endVowel)) || (IpaUtilities.Diacritics.Contains(endVowel)))
                    {
                        endVowel = diphthong[^2..];
                    }
                    string startVowel = diphthong[0..1];
                    if ((IpaUtilities.Suprasegmentals.Contains(diphthong[1..2])) || (IpaUtilities.Diacritics.Contains(diphthong[1..2])))
                    {
                        startVowel = diphthong[0..2];
                    }

                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel;
                        if ((replacementVowel != endVowel[0..1]) && (replacement != startVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", replacement, endVowel);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement[0..1]]);
                            if (replacement.Trim().Length > 1)
                            {
                                if (replacement.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (replacement.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (replacement.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[endVowel[0..1]]);
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ː';
                        if ((replacementVowel != endVowel[0..1]) && (replacement != startVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", replacement, endVowel);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement[0..1]]);
                            if (replacement.Trim().Length > 1)
                            {
                                if (replacement.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (replacement.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (replacement.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[endVowel[0..1]]);
                            _ = sb.Append(" lengthened");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ˑ';
                        if ((replacementVowel != endVowel[0..1]) && (replacement != startVowel))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", replacement, endVowel);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement[0..1]]);
                            if (replacement.Trim().Length > 1)
                            {
                                if (replacement.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (replacement.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (replacement.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[endVowel[0..1]]);
                            _ = sb.Append(" half-lengthened");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    if (!endVowel.Contains('̯'))
                    {
                        foreach (string replacementVowel in IpaUtilities.Vowels)
                        {
                            string replacement = replacementVowel + '̯';
                            if ((replacementVowel != endVowel[0..1]) && (replacement != startVowel))
                            {
                                StringBuilder sb = new();
                                _ = sb.AppendFormat("{0}{1} -- ", replacement, endVowel);
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacement[0..1]]);
                                if (startVowel.Trim().Length > 1)
                                {
                                    if (startVowel.Contains('ː'))
                                    {
                                        _ = sb.Append(" lengthened");
                                    }
                                    else if (startVowel.Contains('ˑ'))
                                    {
                                        _ = sb.Append(" half-lengthened");
                                    }
                                    else if (startVowel.Contains('̯'))
                                    {
                                        _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                    }
                                }
                                _ = sb.Append(' ');
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[endVowel[0..1]]);
                                _ = sb.Append(" half-lengthened");
                                _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                            }
                        }
                    }
                }
                else if (rbn_changeEndVowel.Checked)
                {
                    string diphthong = languageDescription.phonetic_inventory["v_diphthongs"][phonemeIndex];
                    string endVowel = diphthong[^1..];
                    if ((IpaUtilities.Suprasegmentals.Contains(endVowel)) || (IpaUtilities.Diacritics.Contains(endVowel)))
                    {
                        endVowel = diphthong[^2..];
                    }
                    string startVowel = diphthong[0..1];
                    if ((IpaUtilities.Suprasegmentals.Contains(diphthong[1..2])) || (IpaUtilities.Diacritics.Contains(diphthong[1..2])))
                    {
                        startVowel = diphthong[0..2];
                    }

                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel;
                        if ((replacement != endVowel) && (replacementVowel != startVowel[0..1]))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", startVowel, replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[startVowel[0..1]]);
                            if (startVowel.Trim().Length > 1)
                            {
                                if (startVowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (startVowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (startVowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ː';
                        if ((replacement != endVowel) && (replacementVowel != startVowel[0..1]))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", startVowel, replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[startVowel[0..1]]);
                            if (startVowel.Trim().Length > 1)
                            {
                                if (startVowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (startVowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (startVowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                            _ = sb.Append(" lengthened");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        string replacement = replacementVowel + 'ˑ';
                        if ((replacement != endVowel) && (replacementVowel != startVowel[0..1]))
                        {
                            StringBuilder sb = new();
                            _ = sb.AppendFormat("{0}{1} -- ", startVowel, replacement);
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[startVowel[0..1]]);
                            if (startVowel.Trim().Length > 1)
                            {
                                if (startVowel.Contains('ː'))
                                {
                                    _ = sb.Append(" lengthened");
                                }
                                else if (startVowel.Contains('ˑ'))
                                {
                                    _ = sb.Append(" half-lengthened");
                                }
                                else if (startVowel.Contains('̯'))
                                {
                                    _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                }
                            }
                            _ = sb.Append(' ');
                            _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                            _ = sb.Append(" half-lengthened");
                            _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                        }
                    }
                    if (!startVowel.Contains('̯'))
                    {
                        foreach (string replacementVowel in IpaUtilities.Vowels)
                        {
                            string replacement = replacementVowel + '̯';
                            if ((replacement != endVowel) && (replacementVowel != startVowel[0..1]))
                            {
                                StringBuilder sb = new();
                                _ = sb.AppendFormat("{0}{1} -- ", startVowel, replacement);
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[startVowel[0..1]]);
                                if (startVowel.Trim().Length > 1)
                                {
                                    if (startVowel.Contains('ː'))
                                    {
                                        _ = sb.Append(" lengthened");
                                    }
                                    else if (startVowel.Contains('ˑ'))
                                    {
                                        _ = sb.Append(" half-lengthened");
                                    }
                                    else if (startVowel.Contains('̯'))
                                    {
                                        _ = sb.Append(" semi-vowel");  // Probably should be part of a diphthong
                                    }
                                }
                                _ = sb.Append(' ');
                                _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                                _ = sb.Append(" half-lengthened");
                                _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                            }
                        }
                    }
                }
                else if (rbn_diphthongReplacement.Checked)
                {
                    cbx_dipthongStartVowel.Items.Clear();
                    cbx_dipthongEndVowel.Items.Clear();
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0} -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = cbx_dipthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_dipthongEndVowel.Items.Add(sb.ToString());
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ː -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = sb.Append(" lengthened");
                        _ = cbx_dipthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_dipthongEndVowel.Items.Add(sb.ToString());
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ˑ -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = sb.Append(" half-lengthened");
                        _ = cbx_dipthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_dipthongEndVowel.Items.Add(sb.ToString());
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}\u032F -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = sb.Append(" semi-vowel");
                        _ = cbx_dipthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_dipthongEndVowel.Items.Add(sb.ToString());
                    }
                    cbx_dipthongStartVowel.SelectedIndex = -1;
                    cbx_dipthongEndVowel.SelectedIndex = -1;
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
                    StringBuilder sb = new();
                    if (oldPhoneme.Equals("ə"))
                    {
                        newPhoneme = "ɚ";
                        _ = sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                    }
                    else
                    {
                        newPhoneme = oldPhoneme + "\u02de";
                        _ = sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme[0].ToString()]);
                        _ = sb.Append(" Rhotacized");
                    }
                    newPhonemeDescription = sb.ToString();
                    _ = cbx_replacementPhoneme.Items.Add(newPhonemeDescription);
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
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                        newPhonemeDescription = sb.ToString();
                    }
                    else
                    {
                        newPhoneme = oldPhoneme + "\u02de";
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme[0].ToString()]);
                        _ = sb.Append(" Rhotacized");
                        newPhonemeDescription = sb.ToString();
                    }
                    _ = cbx_replacementPhoneme.Items.Add(newPhonemeDescription);
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
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                        newPhonemeDescription = sb.ToString();
                    }
                    else
                    {
                        newPhoneme = oldPhoneme;
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ˑ -- ", newPhoneme);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[newPhoneme]);
                        newPhonemeDescription = sb.ToString();
                    }
                    _ = cbx_replacementPhoneme.Items.Add(newPhonemeDescription);
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
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ˑ -- ", newPhoneme2);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[rConsonant]);
                        newPhonemeDescription = sb.ToString();
                        _ = cbx_replacementPhoneme.Items.Add(newPhonemeDescription);
                    }
                    cbx_replacementPhoneme.SelectedIndex = -1;
                }
                else if (rbn_replaceRSpelling.Checked)
                {
                    foreach (string phoneme in IpaUtilities.RPhonemes)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0} -- ", phoneme);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[phoneme]);
                        _ = cbx_replacementPhoneme.Items.Add(sb.ToString());
                    }
                    string phoneme2 = "˞";
                    StringBuilder sb2 = new();
                    _ = sb2.AppendFormat("{0} -- ", phoneme2);
                    _ = sb2.Append(IpaUtilities.IpaPhonemesMap[phoneme2]);
                    _ = cbx_replacementPhoneme.Items.Add(sb2.ToString());
                }
            }
        }

        private void Btn_applyChangeToLanguage_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex != 0) &&
                (tabPhoneticAlterations.SelectedIndex != 1) &&
                (tabPhoneticAlterations.SelectedIndex != 2) &&
                (tabPhoneticAlterations.SelectedIndex != 3))
            {
                return;
            }

            if ((tabPhoneticAlterations.SelectedIndex == 0) ||
                (tabPhoneticAlterations.SelectedIndex == 1) ||
                ((tabPhoneticAlterations.SelectedIndex == 3) && (!rbn_replaceRSpelling.Checked)) ||
                ((tabPhoneticAlterations.SelectedIndex == 2) && (!rbn_diphthongReplacement.Checked)))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
                {
                    return;
                }
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
                        speechEngine.SampleText = sampleText;
                    }
                }
                // Clear the combo boxes
                cbx_phonemeToChange.Items.Clear();
                cbx_replacementPhoneme.Items.Clear();
                tabPhoneticAlterations.SelectedIndex = -1;
            }
            else if ((tabPhoneticAlterations.SelectedIndex == 3) && rbn_replaceRSpelling.Checked && cbx_phonemeToChange.SelectedItem != null)
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
                PhoneticChanger.ReplaceSoundMapEntry(mapToReplace, replacementMap, languageDescription.sound_map_list);
                phoneticChanger.UpdatePronunciation();
                if (sampleText != string.Empty)
                {
                    sampleText = phoneticChanger.SampleText;
                    txt_SampleText.Text = sampleText;
                    txt_phonetic.Text = string.Empty;
                    foreach (string engineName in speechEngines.Keys)
                    {
                        SpeechEngine speech = speechEngines[engineName];
                        speech.SampleText = sampleText;
                    }
                }
                // Clear the combo boxes
                cbx_phonemeToChange.Items.Clear();
                cbx_replacementPhoneme.Items.Clear();
                tabPhoneticAlterations.SelectedIndex = -1;
            }
            else if ((tabPhoneticAlterations.SelectedIndex == 2) && (rbn_diphthongReplacement.Checked))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_dipthongStartVowel.SelectedIndex == -1) || (cbx_dipthongEndVowel.SelectedIndex == -1))
                {
                    return;
                }
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = "";
                if (BuildNewDiphthong(ref newPhoneme))
                {
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
                            speechEngine.SampleText = sampleText;
                        }
                    }
                    // Clear the combo boxes
                    cbx_phonemeToChange.Items.Clear();
                    cbx_replacementPhoneme.Items.Clear();
                    tabPhoneticAlterations.SelectedIndex = -1;
                }
            }
            // Add other options as needed.
        }

        private void Cbx_replacementPhoneme_MeasureItem(object? sender, System.Windows.Forms.MeasureItemEventArgs e)
        {
            e.ItemWidth = cbx_phonemeToChange.DropDownWidth;
            e.ItemHeight = cbx_replacementPhoneme.Font.Height + 10;
        }

        private void Cbx_replacementPhoneme_DrawItem(object? sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
                return;
            }
            Brush comboBrush = Brushes.Black; // Set the default color to black

            string cbxEntry = string.Empty;
            if (cbx_replacementPhoneme.Items[e.Index] is string replacementPhoneme)
            {
                cbxEntry = replacementPhoneme;
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
            Rectangle rectangle = new(2, e.Bounds.Top + 2, e.Bounds.Height, e.Bounds.Height - 4);
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

        private void Btn_addCurrentChangeToList_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex != 0) &&
                (tabPhoneticAlterations.SelectedIndex != 1) &&
                (tabPhoneticAlterations.SelectedIndex != 2) &&
                (tabPhoneticAlterations.SelectedIndex != 3))
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex == 3) && (rbn_replaceRSpelling.Checked))
            {
                _ = MessageBox.Show("Rhotacize based on spelling can only be performed singly", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (changesToBeMade.Count >= 10)
            {
                _ = MessageBox.Show("No more than 10 changes can be grouped for simultaneous update.  Apply existing updates before adding additional changes",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex == 0) ||
                (tabPhoneticAlterations.SelectedIndex == 1) ||
                ((tabPhoneticAlterations.SelectedIndex == 3) && (!rbn_replaceRSpelling.Checked)) ||
                ((tabPhoneticAlterations.SelectedIndex == 2) && (!rbn_diphthongReplacement.Checked)))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
                {
                    return;
                }
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = cbx_replacementPhoneme.Text.Split()[0];

                changesToBeMade.Add((oldPhoneme, newPhoneme));
                StringBuilder sb = new();
                foreach ((string oph, string nph) in changesToBeMade)
                {
                    _ = sb.Append(oph);
                    _ = sb.Append(" -> ");
                    _ = sb.Append(nph);
                    _ = sb.Append(", ");
                }
                txt_changeList.Text = sb.ToString();
            }
            else if ((tabPhoneticAlterations.SelectedIndex == 2) && (rbn_diphthongReplacement.Checked))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_dipthongStartVowel.SelectedIndex == -1) || (cbx_dipthongEndVowel.SelectedIndex == -1))
                {
                    return;
                }
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = "";
                if (BuildNewDiphthong(ref newPhoneme))
                {
                    phoneticChanger.PhoneticChange(oldPhoneme, newPhoneme);
                    // Update sample text
                    if (sampleText != string.Empty)
                    {
                        changesToBeMade.Add((oldPhoneme, newPhoneme));
                        StringBuilder sb = new();
                        foreach ((string oph, string nph) in changesToBeMade)
                        {
                            _ = sb.Append(oph);
                            _ = sb.Append(" -> ");
                            _ = sb.Append(nph);
                            _ = sb.Append(", ");
                        }
                        txt_changeList.Text = sb.ToString();
                    }
                }
            }

        }

        private void Btn_applyListOfChanges_Click(object sender, EventArgs e)
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
                    speech.SampleText = sampleText;
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

        private void Btn_revertLastChange_Click(object sender, EventArgs e)
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

        private void DisplayGlossOfSampleTextToolStripMenuItem_Click(object sender, EventArgs e)
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

            _ = MessageBox.Show(glossText, "Glossed Text", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PrintSampleTextSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sampleText))
            {
                return;
            }
            if (languageDescription == null)
            {
                return;
            }
            StringBuilder sb = new();
            _ = sb.AppendLine(WrapText(sampleText, 80));
            _ = sb.AppendLine("\n------------------------------------------------------------------------");
            _ = sb.AppendLine("Gloss:");
            string gloss = LatinUtilities.GlossText(sampleText, languageDescription, this);
            _ = sb.AppendLine(WrapText(gloss, 80));
            _ = sb.AppendLine("\n------------------------------------------------------------------------");
            _ = sb.AppendLine("Phonetic:");
            if (string.IsNullOrEmpty(txt_phonetic.Text))
            {
                string speed = cbx_speed.Text.Trim();
                string engineName = cbx_speechEngine.Text.Trim();
                speechEngines[engineName].Generate(speed, this);
                txt_phonetic.Text = speechEngines[engineName].PhoneticText;
            }
            _ = sb.AppendLine(txt_phonetic.Text.Trim());
            StringReader sampleTextSummaryReader = new(sb.ToString());
            readerToPrint = sampleTextSummaryReader;
            printFont = new Font("Charis SIL", 12.0f);
            try
            {
                PrintDialog printDialog = new();
                PrintDocument pd = new();
                pd.PrintPage += new PrintPageEventHandler(this.Pd_PrintPage);
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

        private void PrintKiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> missingPhonemes = KirshenbaumUtilities.UnmappedPhonemes;
            StringBuilder stringBuilder = new();
            foreach (var (phoneme, sb2) in from string phoneme in missingPhonemes
                                           where IpaUtilities.IpaPhonemesMap.ContainsKey(phoneme)
                                           let sb2 = new StringBuilder()
                                           select (phoneme, sb2))
            {
                foreach (char c in phoneme)
                {
                    if (Char.IsAsciiLetterOrDigit(c))
                    {
                        _ = sb2.Append(c);
                    }
                    else
                    {
                        int cInt = c;
                        _ = sb2.AppendFormat("U+{0,4:x4}", cInt);
                    }
                }

                _ = stringBuilder.AppendFormat("\"{0} ({2}):\t{1}\n", phoneme, IpaUtilities.IpaPhonemesMap[phoneme], sb2.ToString());
            }

            StringReader sampleTextSummaryReader = new(stringBuilder.ToString());
            readerToPrint = sampleTextSummaryReader;
            printFont = new Font("Charis SIL", 12.0f);
            try
            {
                PrintDialog printDialog = new();
                PrintDocument pd = new();
                pd.PrintPage += new PrintPageEventHandler(this.Pd_PrintPage);
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

        private void Cbx_speechEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVoices();
        }

        private void Btn_updateSoundMapList_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            SoundMapListEditor soundMapListEditor = new()
            {
                SoundMapList = languageDescription.sound_map_list,
                headerText = "No specific changes - editing the entire list"
            };
            soundMapListEditor.UpdatePhonemeReplacements();
            _ = soundMapListEditor.ShowDialog();
            // ShowDialog is modal
            if (soundMapListEditor.SoundMapSaved)
            {
                DialogResult result = MessageBox.Show("Preserve the spelling (Yes)?\nNo preserves the pronunciation.", "Spelling or pronunciation", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    languageDescription.sound_map_list = soundMapListEditor.SoundMapList;
                    phoneticChanger.UpdatePronunciation();
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speech = speechEngines[engineName];
                            speech.SampleText = sampleText;
                        }
                    }
                }
                else if (result == DialogResult.No)
                {
                    phoneticChanger.UpdateSpelling();
                    languageDescription.sound_map_list = soundMapListEditor.SoundMapList;
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speech = speechEngines[engineName];
                            speech.SampleText = sampleText;
                        }
                    }
                }
            }

        }

        private void Rbn_l1_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_normalVowel_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_addRhoticityRegular_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_l2_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_l3_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_allPhonemes_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_halfLongVowels_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_longVowels_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_normalDiphthong_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_semivowelDiphthong_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_longToRhotacized_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_removeRhoticity_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_replaceRhotacized_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_replaceRSpelling_CheckedChanged(object sender, EventArgs e)
        {
            if (rbn_replaceRSpelling.Checked)
            {
                lbl_choiceCbx.Text = "Pronunciation Patter to Update";
                lbl_replacementCbx.Text = "Phoneme to add/replace";
                UpdatePhonemeToChangeCbx();
            }
            else
            {
                lbl_choiceCbx.Text = "Phoneme to change";
                lbl_replacementCbx.Text = "Replacement Phoneme";
            }
        }

        private void SetAmazonPollyURIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pollyURI = Microsoft.VisualBasic.Interaction.InputBox("Enter the URI for Amazon Polly", "Amazon Polly URI", PollySpeech.PollyURI ?? "");
            if (PollySpeech.TestURI(pollyURI))
            {
                PollySpeech.PollyURI = pollyURI;
                PollySpeech pollySpeech = new();
                Dictionary<string, SpeechEngine.VoiceData> amazonPollyVoices = pollySpeech.GetVoices();
                if (speechEngines.ContainsKey(pollySpeech.Description))
                {
                    speechEngines[pollySpeech.Description] = pollySpeech;
                }
                else
                {
                    speechEngines.Add(pollySpeech.Description, pollySpeech);
                }
                if (!voices.TryAdd(pollySpeech.Description, amazonPollyVoices))
                {
                    voices[pollySpeech.Description] = amazonPollyVoices;
                }

                config.PollyURI = pollyURI;
            }
        }

        private void SetESpeakNgLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
            if (string.IsNullOrEmpty(ESpeakNGSpeak.ESpeakNGPath))
            {
                openFileDialog.InitialDirectory = Environment.SpecialFolder.ProgramFiles.ToString();

            }
            else
            {
                FileInfo fi = new(ESpeakNGSpeak.ESpeakNGPath);
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
                ESpeakNGSpeak eSpeakNGSpeak = new();
                Dictionary<string, SpeechEngine.VoiceData> espeakVoices = eSpeakNGSpeak.GetVoices();
                if (speechEngines.ContainsKey(eSpeakNGSpeak.Description))
                {
                    speechEngines[eSpeakNGSpeak.Description] = eSpeakNGSpeak;
                }
                else
                {
                    speechEngines.Add(eSpeakNGSpeak.Description, eSpeakNGSpeak);
                }
                if (!voices.TryAdd(eSpeakNGSpeak.Description, espeakVoices))
                {
                    voices[eSpeakNGSpeak.Description] = espeakVoices;
                }

                config.ESpeakNgPath = ESpeakNGSpeak.ESpeakNGPath;
            }

        }

        private void Rbn_vowelToDiphthongStart_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_vowelToDiphthongEnd_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_changeStartVowel_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_ChangeEndVowel_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_diphthongReplacement_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private bool BuildNewDiphthong(ref string newPhoneme)
        {
            string startVowel = cbx_dipthongStartVowel.Text.Split()[0];
            string endVowel = cbx_dipthongEndVowel.Text.Split()[0];

            // Validate the potential diphthong to confirm it is valid
            if ((startVowel[0..1] == endVowel[0..1]) ||
                (startVowel.Contains('\u032f') && endVowel.Contains('\u032f')))
            {
                string errorString = string.Format("{0}{1} is not a valid diphthong", startVowel, endVowel);
                _ = MessageBox.Show(errorString, "Invalid Diphthong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            newPhoneme = string.Format("{0}{1}", startVowel, endVowel);
            return true;
        }
    }
}

