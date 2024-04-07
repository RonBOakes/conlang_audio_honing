/*
 * User Methods for the top level Conlang Editor.
 
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
using ConlangAudioHoning;
using ConlangJson;
using Microsoft.VisualBasic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Channels;
namespace LanguageEditor
{
    public partial class LanguageEditorForm : Form
    {
        private LanguageDescription? languageDescription;
        private FileInfo? languageFileInfo = null;
        private List<TextBox>? nounGenderTxtBoxes = null;
        private LexiconEditor? lexiconEditor = null;
        private DeclensionAffixMapPane? declensionAffixMapPane = null;

        public LanguageEditorForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

            txt_languageNameEnglish.Text = languageDescription.english_name;
            txt_languageNameNativeEnglish.TextChanged -= Txt_languageNameNativeEnglish_TextChanged;
            txt_languageNativePhonetic.TextChanged -= Txt_languageNativePhonetic_TextChanged;
            txt_languageNativePhonetic.Text = languageDescription.native_name_phonetic;
            txt_languageNameNativeEnglish.Text = languageDescription.native_name_english;
            nounGenderTxtBoxes = new List<TextBox>();
            int xPos = 0, yPos = 0;
            panel_nounGender.SuspendLayout();
            panel_nounGender.Controls.Clear();
            foreach (string nounGender in languageDescription.noun_gender_list)
            {
                TextBox txt_nounGender = new();
                txt_nounGender.Text = nounGender;
                txt_nounGender.Location = new Point(xPos, yPos);
                txt_nounGender.Size = new Size(125, 12);
                yPos += 20;
                panel_nounGender.Controls.Add(txt_nounGender);
            }
            TextBox txt_nounGenderBlank = new();
            txt_nounGenderBlank.Text = "";
            txt_nounGenderBlank.Location = new Point(xPos, yPos);
            txt_nounGenderBlank.Size = new Size(125, 12);
            panel_nounGender.Controls.Add(txt_nounGenderBlank);
            panel_nounGender.ResumeLayout(true);

            tab_soundMapList.SuspendLayout();
            xPos = 0;
            yPos = 0;
            SoundMapListEditor soundMapEditor = new SoundMapListEditor();
            soundMapEditor.Location = new Point(xPos, yPos);
            tab_soundMapList.Controls.Add(soundMapEditor);
            tab_soundMapList.ResumeLayout(true);

            xPos = 0;
            yPos = 0;
            panel_partsOfSpeechList.SuspendLayout();
            panel_partsOfSpeechList.Controls.Clear();
            foreach (string partOfSpeech in languageDescription.part_of_speech_list)
            {
                TextBox txt_partOfSpeech = new();
                txt_partOfSpeech.Text = partOfSpeech;
                txt_partOfSpeech.Location = new Point(xPos, yPos);
                txt_partOfSpeech.Size = new Size(125, 12);
                yPos += 20;
                panel_partsOfSpeechList.Controls.Add(txt_partOfSpeech);
            }
            TextBox txt_partOfSpeechBlank = new();
            txt_partOfSpeechBlank.Text = "";
            txt_partOfSpeechBlank.Location = new Point(xPos, yPos);
            txt_partOfSpeechBlank.Size = new Size(125, 12);
            panel_partsOfSpeechList.Controls.Add(txt_partOfSpeechBlank);
            panel_partsOfSpeechList.ResumeLayout(true);

            languageFileInfo = new FileInfo(filename);

            xPos = 0;
            yPos = 0;
            panel_phonemeInventory.SuspendLayout();
            panel_phonemeInventory.Controls.Clear();
            foreach (string phoneme in languageDescription.phoneme_inventory)
            {
                TextBox txt_phoneme = new();
                txt_phoneme.Text = phoneme;
                txt_phoneme.Location = new Point(xPos, yPos);
                txt_phoneme.Size = new Size(125, 12);
                yPos += 20;
                panel_phonemeInventory.Controls.Add(txt_phoneme);
            }
            TextBox txt_phonemeBlank = new();
            txt_phonemeBlank.Text = "";
            txt_phonemeBlank.Location = new Point(xPos, yPos);
            txt_phonemeBlank.Size = new Size(125, 12);
            panel_phonemeInventory.Controls.Add(txt_phonemeBlank);
            panel_phonemeInventory.ResumeLayout(true);

            lexiconEditor = new LexiconEditor();
            tab_lexicon.SuspendLayout();
            tab_lexicon.Controls.Clear();
            tab_lexicon.Controls.Add(lexiconEditor);
            tab_lexicon.ResumeLayout(true);

            tab_derivationalAffixMap.SuspendLayout();
            tpn_DerivationalAffixMap.SuspendLayout();
            tpn_DerivationalAffixMap.TabPages.Clear();
            foreach (string derivationKey in languageDescription.derivational_affix_map.Keys)
            {
                TabPage tabPage = new(derivationKey);
                DerivationalAffixEditor editor = new();
                editor.AffixRules = languageDescription.derivational_affix_map[derivationKey];
                editor.Location = new Point(5, 5);
                tabPage.Controls.Add(editor);
                tpn_DerivationalAffixMap.TabPages.Add(tabPage);
            }
            tpn_DerivationalAffixMap.ResumeLayout(true);
            tab_derivationalAffixMap.ResumeLayout(true);

            tab_declinsionAffixes.SuspendLayout();
            declensionAffixMapPane = new DeclensionAffixMapPane();
            declensionAffixMapPane.AffixMap = languageDescription.affix_map;
            declensionAffixMapPane.Size = tab_declinsionAffixes.Size;
            tab_declinsionAffixes.Controls.Add(declensionAffixMapPane);
            tab_declinsionAffixes.ResumeLayout(true);
            DeclensionAffixEditor.SoundMapList = languageDescription.sound_map_list;

            tab_derivedWordList.SuspendLayout();
            tab_derivedWordList.AutoScroll = true;
            xPos = 0;
            yPos = 0;
            foreach (string derivedWordEntry in languageDescription.derived_word_list)
            {
                TextBox txt_derivedWord = new();
                txt_derivedWord.Text = derivedWordEntry;
                txt_derivedWord.Location = new Point(xPos, yPos);
                txt_derivedWord.Size = new Size(850, 20);
                yPos += 25;
                tab_derivedWordList.Controls.Add(txt_derivedWord);
            }
            TextBox txt_derivedWordBlank = new();
            txt_derivedWordBlank.Text = string.Empty;
            txt_derivedWordBlank.Location = new Point(xPos, yPos);
            txt_derivedWordBlank.Size = new Size(850, 20);
            tab_derivedWordList.Controls.Add(txt_derivedWordBlank);
            tab_derivedWordList.ResumeLayout(true);

            txt_languageNativePhonetic.TextChanged += Txt_languageNativePhonetic_TextChanged;
            txt_languageNameNativeEnglish.TextChanged += Txt_languageNameNativeEnglish_TextChanged;

            languageFileInfo = new FileInfo(filename);
        }

        private void Txt_languageNameNativeEnglish_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                txt_languageNameNativeEnglish.TextChanged -= Txt_languageNameNativeEnglish_TextChanged;
                txt_languageNativePhonetic.TextChanged -= Txt_languageNativePhonetic_TextChanged;

                if (languageDescription?.sound_map_list != null)
                {
                    txt_languageNativePhonetic.Text = ConlangUtilities.SoundOutWord(txt_languageNameNativeEnglish.Text.Trim(), languageDescription?.sound_map_list ?? []);
                }
            }
            finally
            {
                txt_languageNameNativeEnglish.TextChanged += Txt_languageNativePhonetic_TextChanged;
                txt_languageNativePhonetic.TextChanged += Txt_languageNativePhonetic_TextChanged;
            }
        }

        private void Txt_languageNativePhonetic_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                txt_languageNameNativeEnglish.TextChanged -= Txt_languageNameNativeEnglish_TextChanged;
                txt_languageNativePhonetic.TextChanged -= Txt_languageNativePhonetic_TextChanged;

                if (languageDescription?.sound_map_list != null)
                {
                    txt_languageNameNativeEnglish.Text = ConlangUtilities.SpellWord(txt_languageNativePhonetic.Text.Trim(), languageDescription?.sound_map_list ?? []);
                }
            }
            finally
            {
                txt_languageNameNativeEnglish.TextChanged += Txt_languageNativePhonetic_TextChanged;
                txt_languageNativePhonetic.TextChanged += Txt_languageNativePhonetic_TextChanged;
            }
        }

        private void SaveLanguage(string filename)
        {
            if (languageDescription == null)
            {
                MessageBox.Show("Unable to save language File, no language information present", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                languageDescription.english_name = txt_languageNameEnglish.Text.Trim();
                languageDescription.native_name_phonetic = txt_languageNativePhonetic.Text.Trim();
                languageDescription.native_name_phonetic = txt_languageNativePhonetic.Text.Trim();

                languageDescription.noun_gender_list.Clear();
                foreach (TextBox txt_nounGender in panel_nounGender.Controls)
                {
                    if (txt_nounGender.Text.Trim() != string.Empty)
                    {
                        languageDescription.noun_gender_list.Add(txt_nounGender.Text.Trim());
                    }
                }

                languageDescription.sound_map_list.Clear();
                foreach (SoundMapEditor editor in tab_soundMapList.Controls)
                {
                    languageDescription.sound_map_list.Add(editor.SoundMapData);
                }

                languageDescription.part_of_speech_list.Clear();
                foreach (TextBox txt_partOfSpeech in panel_partsOfSpeechList.Controls)
                {
                    if (txt_partOfSpeech.Text.Trim() != string.Empty)
                    {
                        languageDescription.part_of_speech_list.Add(txt_partOfSpeech.Text.Trim());
                    }
                }

                languageDescription.phoneme_inventory.Clear();
                foreach (TextBox txt_phoneme in panel_phonemeInventory.Controls)
                {
                    if (txt_phoneme.Text.Trim() != string.Empty)
                    {
                        languageDescription.phoneme_inventory.Add(txt_phoneme.Text.Trim());
                    }
                }

                languageDescription.lexicon = lexiconEditor?.Lexicon ?? [];

                languageDescription.derivational_affix_map.Clear();
                foreach (TabPage tabPage in tpn_DerivationalAffixMap.Controls)
                {
                    foreach (DerivationalAffixEditor editor in tabPage.Controls)
                    {
                        languageDescription.derivational_affix_map[tabPage.Text.Trim()] = editor.AffixRules;
                        break; // Ensure that loop only executes once.
                    }
                }

                languageDescription.affix_map = declensionAffixMapPane?.AffixMap ?? [];

                languageDescription.derived_word_list.Clear();
                foreach (TextBox txt_derivedWord in tab_derivedWordList.Controls)
                {
                    if (txt_derivedWord.Text.Trim() != string.Empty)
                    {
                        languageDescription.derived_word_list.Add(txt_derivedWord.Text.Trim());
                    }
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

                string jsonString = JsonSerializer.Serialize<LanguageDescription>(languageDescription,jsonSerializerOptions);
                File.WriteAllText(filename, jsonString, System.Text.Encoding.UTF8);
            }
        }
    }
}
