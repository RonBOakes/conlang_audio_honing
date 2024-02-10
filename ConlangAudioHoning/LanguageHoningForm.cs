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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using ConlangJson;
using LanguageEditor;
using System.Drawing.Printing;
using Timer = System.Windows.Forms.Timer;
using System.Reflection.Metadata;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConlangAudioHoning
{
    public partial class LanguageHoningForm : Form
    {
        private LanguageDescription? languageDescription = null;
        private FileInfo? languageFileInfo = null;
        private string? sampleText = null;
        private PollySpeech? pollySpeech = null;
        private Font? printFont;
        private TextReader? readerToPrint;
        private Dictionary<string, FileInfo> speechFiles = new Dictionary<string, FileInfo>();
        private PhoneticChanger phoneticChanger;
        private Dictionary<string, PollySpeech.VoiceData> amazonPollyVoices = new Dictionary<string, PollySpeech.VoiceData>();
        private List<(string, string)> changesToBeMade = new List<(string, string)>();

        public System.Windows.Forms.ProgressBar ProgressBar
        {
            get => pb_status;
        }

        public LanguageHoningForm()
        {
            InitializeComponent();
            phoneticChanger = new PhoneticChanger();
            amazonPollyVoices = PollySpeech.getAmazonPollyVoices();
            changesToBeMade.Clear();
            LoadVoices();
            LoadSpeeds();
        }

        private void LoadVoices()
        {
            cbx_voice.SuspendLayout();
            cbx_voice.Items.Clear();
            foreach (string voiceName in amazonPollyVoices.Keys)
            {
                string voiceMenu = string.Format("{0} ({1}, {2})", voiceName, amazonPollyVoices[voiceName].LanguageName, amazonPollyVoices[voiceName].Gender);
                cbx_voice.Items.Add(voiceMenu);
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

            languageDescription = JsonSerializer.Deserialize<LanguageDescription>(jsonString);

            if (languageDescription == null)
            {
                MessageBox.Show("Unable to decode Language file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            languageFileInfo = new FileInfo(filename);

            IpaUtilities.SubstituteLatinIpaReplacements(languageDescription);
            IpaUtilities.BuildPhoneticInventory(languageDescription);
            phoneticChanger.Language = languageDescription;

            // Empty the speech files and list box
            speechFiles.Clear();
            cbx_recordings.Items.Clear();
            cbx_speed.SelectedText = "slow";

            if (languageDescription.preferred_voice != null)
            {
                string voiceMenu = string.Format("{0} ({1}, {2})", languageDescription.preferred_voice,
                    amazonPollyVoices[languageDescription.preferred_voice].LanguageName, amazonPollyVoices[languageDescription.preferred_voice].Gender);
                cbx_voice.SelectedText = voiceMenu;
            }

            if (languageDescription.declined)
            {
                declineToolStripMenuItem.Enabled = false;
            }
            if (languageDescription.derived)
            {
                deriveToolStripMenuItem.Enabled = false;
            }

            if (pollySpeech == null)
            {
                pollySpeech = new PollySpeech(languageDescription);
            }
            else
            {
                pollySpeech.LanguageDescription = languageDescription;
            }
            // Clear the list of change and combo boxes
            cbx_phonemeToChange.Items.Clear();
            cbx_phonemeToChange.Items.Clear();
            changesToBeMade.Clear();
            txt_changeList.Text = string.Empty;
            // Set the default settings for the language alteration selections.
            rbn_normalVowel.Checked = true;
            rbn_consonants.Checked = true;
            rbn_l1.Checked = true;

        }

        private void SaveLanguage(string filename)
        {
            if (languageDescription == null)
            {
                MessageBox.Show("Unable to save language File, no language information present", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // TODO: Pull the form data into the structure to allow for saving.

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
            if (pollySpeech != null)
            {
                pollySpeech.sampleText = sampleText;
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

        private void txt_SampleText_TextChanged(object sender, EventArgs e)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            if ((languageDescription != null) && (pollySpeech != null) && (sampleText != null) && (!sampleText.Trim().Equals(string.Empty)))
            {
                string speed = cbx_speed.Text.Trim();
                pollySpeech.Generate(speed, this);
                txt_phonetic.Text = pollySpeech.phoneticText;
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
                ConLangUtilities.removeDeclinedEntries(languageDescription);
                declineToolStripMenuItem.Text = "Decline Language";
            }

        }

        private void btn_generateSpeech_Click(object sender, EventArgs e)
        {
            if (pollySpeech == null)
            {
                return;

            }
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
                printFont = new Font("Charis SIL", 12f);
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                pd.Print();
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
                addLexicon.AddRange(ConLangUtilities.DeclineWord(word, language.affix_map, language.sound_map_list));
                pb_status.PerformStep();
            }
            pb_status.Style = ProgressBarStyle.Marquee;
            pbTimer.Interval = 20;
            pbTimer.Enabled = true;
            pb_status.MarqueeAnimationSpeed = 200;
            pb_status.Minimum = 0;
            pb_status.Maximum = 100;
            language.lexicon.AddRange(addLexicon);
            List<LexiconEntry> cleanLexicon = ConLangUtilities.deDuplicateLexicon(language.lexicon);
            if (cleanLexicon.Count < language.lexicon.Count)
            {
                language.lexicon = cleanLexicon;
            }
            language.lexicon.Sort(new LexiconEntry.LexicalOrderCompSpelling());
            pb_status.Visible = false;
            pb_status.SendToBack();
            pbTimer.Enabled = false;
            language.declined = true;
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

        private void rbn_consonants_CheckedChanged(object sender, EventArgs e)
        {
            if (rbn_consonants.Checked)
            {
                if (languageDescription == null)
                {
                    return;
                }
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

                cbx_phonemeToChange.SelectedIndex = -1;
            }
        }

        private void cbx_phonemeToChange_SelectedIndexChanged(object sender, EventArgs e)
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
            else if (rbn_consonants.Checked)
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
            else if (rbn_vowels.Checked)
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
            if (!(rbn_consonants.Checked || rbn_vowels.Checked)) // TODO: Add the other options to ensure that at least one is checked
            {
                return;
            }
            if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
            {
                return;
            }

            if ((rbn_consonants.Checked) || (rbn_vowels.Checked))
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
                    if (pollySpeech != null)
                    {
                        pollySpeech.sampleText = sampleText;
                    }
                }
                // Clear the combo boxes
                rbn_consonants.Checked = false;
                cbx_phonemeToChange.Items.Clear();
                cbx_replacementPhoneme.Items.Clear();
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
            if (rbn_consonants.Checked)
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

        private void rbn_vowels_CheckedChanged(object sender, EventArgs e)
        {
            if (rbn_vowels.Checked)
            {
                if (languageDescription == null)
                {
                    return;
                }
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
            }
        }

        private void btn_addCurrentChangeToList_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if (!(rbn_consonants.Checked || rbn_vowels.Checked)) // TODO: Add the other options to ensure that at least one is checked
            {
                return;
            }
            if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
            {
                return;
            }
            if(changesToBeMade.Count >= 10)
            {
                MessageBox.Show("No more than 10 changes can be grouped for simultaneous update.  Apply existing updates before adding additional changes", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((rbn_consonants.Checked) || (rbn_vowels.Checked))
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
                if (pollySpeech != null)
                {
                    pollySpeech.sampleText = sampleText;
                }
            }
            // Clear the combo boxes
            rbn_consonants.Checked = false;
            cbx_phonemeToChange.Items.Clear();
            cbx_replacementPhoneme.Items.Clear();
            changesToBeMade.Clear();
            txt_changeList.Text = string.Empty;
            cbx_phonemeToChange.SelectedIndex = -1;
            cbx_replacementPhoneme.SelectedIndex = -1;
        }
    }
}

