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
        private readonly UserConfiguration UserConfiguration;
        private bool languageDirty = false;
        private bool sampleDirty = false;

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

            this.FormClosing += This_FormClosing;

            UserConfiguration = UserConfiguration.LoadFromFile();

            if (UserConfiguration.UseSharedPolly)
            {
                useSharedAmazonPollyToolStripMenuItem.Checked = true;
                setAmazonPollyURIToolStripMenuItem.Visible = true;
                setAmazonPollyAuthorizationEmailToolStripMenuItem.Visible = true;
                setAmazonPollyAuthorizationPasswordToolStripMenuItem.Visible = true;
                setAmazonPollyProfileToolStripMenuItem.Visible = false;
                setAmazonPollyS3BucketNameToolStripMenuItem.Visible = false;
            }
            else
            {
                useSharedAmazonPollyToolStripMenuItem.Checked = false;
                setAmazonPollyURIToolStripMenuItem.Visible = false;
                setAmazonPollyAuthorizationEmailToolStripMenuItem.Visible = false;
                setAmazonPollyAuthorizationPasswordToolStripMenuItem.Visible = false;
                setAmazonPollyProfileToolStripMenuItem.Visible = true;
                setAmazonPollyS3BucketNameToolStripMenuItem.Visible = true;
            }


            NoEngineSpeak noEngineSpeak = new();
            speechEngines.Add(noEngineSpeak.Description, noEngineSpeak);
            voices.Add(noEngineSpeak.Description, noEngineSpeak.GetVoices());

            if (UserConfiguration.IsESpeakNGSupported)
            {
                ESpeakNGSpeak.ESpeakNGPath = UserConfiguration.ESpeakNgPath;
                ESpeakNGSpeak eSpeakNGSpeak = new();
                Dictionary<string, SpeechEngine.VoiceData> espeakVoices = eSpeakNGSpeak.GetVoices();
                speechEngines.Add(eSpeakNGSpeak.Description, eSpeakNGSpeak);
                voices.Add(eSpeakNGSpeak.Description, espeakVoices);
            }

            if (UserConfiguration.IsPollySupported)
            {
                if (UserConfiguration.UseSharedPolly)
                {
                    SharedPollySpeech.PollyURI = UserConfiguration.PollyURI;
                    SharedPollySpeech.PollyEmail = UserConfiguration.PollyEmail;
                    SharedPollySpeech.PollyPassword = UserConfiguration.PollyPassword;
                    SharedPollySpeech pollySpeech = new();
                    Dictionary<string, SpeechEngine.VoiceData> amazonPollyVoices = pollySpeech.GetVoices();
                    speechEngines.Add(pollySpeech.Description, pollySpeech);
                    voices.Add(pollySpeech.Description, amazonPollyVoices);
                }
                else // Nonshared Polly must be set.
                {
                    NonSharedPollySpeech.PollySSOProfile = UserConfiguration.PollyProfile;
                    NonSharedPollySpeech.PollyS3Bucket = UserConfiguration.PollyS3Bucket;
                    NonSharedPollySpeech nonSharedPollySpeech = new();
                    Dictionary<string, SpeechEngine.VoiceData> amazonPollyVoices = nonSharedPollySpeech.GetVoices();
                    speechEngines.Add(nonSharedPollySpeech.Description, nonSharedPollySpeech);
                    voices.Add(nonSharedPollySpeech.Description, amazonPollyVoices);
                }
            }

            if (UserConfiguration.IsAzureSupported)
            {
                AzureSpeak azureSpeak = new();
                Dictionary<String, SpeechEngine.VoiceData> azureVoices = azureSpeak.GetVoices();
                speechEngines.Add(azureSpeak.Description, azureSpeak);
                voices.Add(azureSpeak.Description, azureVoices);
            }

            phoneticChanger = new PhoneticChanger();

            changesToBeMade.Clear();
            LoadSpeechEngines();
            LoadVoices();
            LoadSpeeds();
            tabPhoneticAlterations.SelectedIndexChanged += TabPhoneticAlterations_SelectedIndexChanged;

            // Hide controls on the Diphthong tab that only show up when certain radio buttons are pressed
            lbl_diphthongStartVowel.Visible = false;
            cbx_diphthongStartVowel.Visible = false;
            lbl_DiphthongEndVowel.Visible = false;
            cbx_diphthongEndVowel.Visible = false;

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
            foreach (LexiconEntry word in addLexicon)
            {
                language.lexicon.Add(word);
            }
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
                if (engineName != "None")
                {
                    _ = cbx_speechEngine.Items.Add(engineName);
                }
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
            _ = languageDescription.preferred_voices.TryAdd("azure", "en-US-AndrewNeural");
            IpaUtilities.SubstituteLatinIpaReplacements(languageDescription);
            IpaUtilities.BuildPhoneticInventory(languageDescription);
            phoneticChanger.Language = languageDescription;

            // Empty the speech files and list box
            speechFiles.Clear();
            cbx_recordings.Items.Clear();
            cbx_speed.SelectedText = "slow";

            if (languageDescription.declined)
            {
                declineToolStripMenuItem.Text = "Remove Declined Words";
            }
            if (languageDescription.derived)
            {
                deriveToolStripMenuItem.Text = "Remove Derived Words";
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

            // Clear the dirty setting
            languageDirty = false;
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

                // Clear the dirty setting
                languageDirty = false;
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

            // Clear the dirty indication
            sampleDirty = false;
        }

        private void SaveSampleText(string filename)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
            File.WriteAllText(filename, sampleText);

            // Clear the dirty indication
            sampleDirty = false;
        }

        private void OnApplicationExit(object? sender, EventArgs e)
        {
            UserConfiguration.SaveConfiguration(UserConfiguration.GetJsonSerializerOptions());
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
                    cbx_diphthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_diphthongEndVowel.Visible = false;
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

                    cbx_diphthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_diphthongEndVowel.Visible = false;
                    lbl_DiphthongEndVowel.Visible = false;
                    cbx_replacementPhoneme.Visible = true;
                    lbl_replacementCbx.Visible = true;

                    cbx_phonemeToChange.SelectedIndex = -1;

                    break;
                case 2: // Vowel diphthongs
                    cbx_phonemeToChange.Items.Clear();
                    // The SpeakingSpeeds to go into the combo box depend on the selected radio button
                    if (rbn_vowelToDiphthongStart.Checked || rbn_vowelToDiphthongEnd.Checked)
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
                        cbx_diphthongStartVowel.Visible = true;
                        lbl_diphthongStartVowel.Visible = true;
                        cbx_diphthongEndVowel.Visible = true;
                        lbl_DiphthongEndVowel.Visible = true;
                        cbx_replacementPhoneme.Visible = false;
                        lbl_replacementCbx.Visible = false;
                    }
                    else
                    {
                        cbx_diphthongStartVowel.Visible = false;
                        lbl_diphthongStartVowel.Visible = false;
                        cbx_diphthongEndVowel.Visible = false;
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
                    cbx_diphthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_diphthongEndVowel.Visible = false;
                    lbl_DiphthongEndVowel.Visible = false;
                    cbx_replacementPhoneme.Visible = true;
                    lbl_replacementCbx.Visible = true;
                    break;
                case 4: // Special Operations
                    cbx_diphthongStartVowel.Visible = false;
                    lbl_diphthongStartVowel.Visible = false;
                    cbx_diphthongEndVowel.Visible = false;
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
                else if (rbn_vowelToDiphthongEnd.Checked)
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
                    cbx_diphthongStartVowel.Items.Clear();
                    cbx_diphthongEndVowel.Items.Clear();
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0} -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = cbx_diphthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_diphthongEndVowel.Items.Add(sb.ToString());
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ː -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = sb.Append(" lengthened");
                        _ = cbx_diphthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_diphthongEndVowel.Items.Add(sb.ToString());
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}ˑ -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = sb.Append(" half-lengthened");
                        _ = cbx_diphthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_diphthongEndVowel.Items.Add(sb.ToString());
                    }
                    foreach (string replacementVowel in IpaUtilities.Vowels)
                    {
                        StringBuilder sb = new();
                        _ = sb.AppendFormat("{0}\u032F -- ", replacementVowel);
                        _ = sb.Append(IpaUtilities.IpaPhonemesMap[replacementVowel[0..1]]);
                        _ = sb.Append(" semi-vowel");
                        _ = cbx_diphthongStartVowel.Items.Add(sb.ToString());
                        _ = cbx_diphthongEndVowel.Items.Add(sb.ToString());
                    }
                    cbx_diphthongStartVowel.SelectedIndex = -1;
                    cbx_diphthongEndVowel.SelectedIndex = -1;
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
                string engineName;
                if (cbx_speechEngine.SelectedIndex == -1)
                {
                    engineName = "None";
                }
                else
                {
                    engineName = cbx_speechEngine.Text.Trim();
                }
                string voiceName;
                if (cbx_voice.SelectedIndex == -1)
                {
                    voiceName = string.Empty;
                }
                else
                {
                    voiceName = cbx_voice.Text.Trim().Split()[0];
                }
                SpeechEngine.VoiceData? voiceData = string.IsNullOrEmpty(voiceName) ? null : voices[engineName][voiceName];
                speechEngines[engineName].Generate(speed, this, voiceData);
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

        private bool BuildNewDiphthong(ref string newPhoneme)
        {
            string startVowel = cbx_diphthongStartVowel.Text.Split()[0];
            string endVowel = cbx_diphthongEndVowel.Text.Split()[0];

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

