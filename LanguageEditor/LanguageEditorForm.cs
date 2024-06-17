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
using System.Diagnostics;
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
        private LexiconEditor? lexiconEditor = null;
        private SoundMapListEditor? soundMapEditor = null;
        private DeclensionAffixMapPane? declensionAffixMapPane = null;

        public LanguageEditorForm()
        {
            InitializeComponent();
            useCompactJsonToolStripItem.Checked = false;
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
            int xPos = 0, yPos = 0;
            panel_nounGender.SuspendLayout();
            panel_nounGender.Controls.Clear();
            foreach (string nounGender in languageDescription.noun_gender_list)
            {
                TextBox txt_nounGender = new()
                {
                    Text = nounGender,
                    Location = new Point(xPos, yPos),
                    Size = new Size(125, 12)
                };
                yPos += 20;
                panel_nounGender.Controls.Add(txt_nounGender);
            }
            TextBox txt_nounGenderBlank = new()
            {
                Text = "",
                Location = new Point(xPos, yPos),
                Size = new Size(125, 12)
            };
            panel_nounGender.Controls.Add(txt_nounGenderBlank);
            panel_nounGender.ResumeLayout(true);

            tab_soundMapList.SuspendLayout();
            xPos = 0;
            yPos = 0;
            soundMapEditor = new SoundMapListEditor
            {
                Location = new Point(xPos, yPos),
                Size = new Size(895, 355)
            };
            soundMapEditor.SoundMapList = languageDescription.spelling_pronunciation_rules;
            tab_soundMapList.Controls.Add(soundMapEditor);
            tab_soundMapList.Enter += Tab_soundMapList_Enter;
            tab_soundMapList.Leave += Tab_soundMapList_Leave;
            tab_soundMapList.ResumeLayout(true);

            xPos = 0;
            yPos = 0;
            panel_partsOfSpeechList.SuspendLayout();
            panel_partsOfSpeechList.Controls.Clear();
            foreach (string partOfSpeech in languageDescription.part_of_speech_list)
            {
                TextBox txt_partOfSpeech = new()
                {
                    Text = partOfSpeech,
                    Location = new Point(xPos, yPos),
                    Size = new Size(125, 12)
                };
                yPos += 20;
                panel_partsOfSpeechList.Controls.Add(txt_partOfSpeech);
            }
            TextBox txt_partOfSpeechBlank = new()
            {
                Text = "",
                Location = new Point(xPos, yPos),
                Size = new Size(125, 12)
            };
            panel_partsOfSpeechList.Controls.Add(txt_partOfSpeechBlank);
            panel_partsOfSpeechList.ResumeLayout(true);

            languageFileInfo = new FileInfo(filename);

            xPos = 0;
            yPos = 0;
            panel_phonemeInventory.SuspendLayout();
            panel_phonemeInventory.Controls.Clear();
            foreach (string phoneme in languageDescription.phoneme_inventory)
            {
                TextBox txt_phoneme = new()
                {
                    Text = phoneme,
                    Location = new Point(xPos, yPos),
                    Size = new Size(125, 12)
                };
                yPos += 20;
                panel_phonemeInventory.Controls.Add(txt_phoneme);
            }
            TextBox txt_phonemeBlank = new()
            {
                Text = "",
                Location = new Point(xPos, yPos),
                Size = new Size(125, 12)
            };
            panel_phonemeInventory.Controls.Add(txt_phonemeBlank);
            panel_phonemeInventory.ResumeLayout(true);

            lexiconEditor = new LexiconEditor
            {
                Size = new Size(895, 355),
                Lexicon = languageDescription.lexicon,
                PartOfSpeechList = languageDescription.part_of_speech_list,
                SoundMapList = languageDescription.spelling_pronunciation_rules
            };
            lexiconEditor.SaveAndCloseToolStripMenuItem.Text = "Save";
            lexiconEditor.CloseWithoutSavingToolStripMenuItem.Text = "Do Not Save";
            tab_lexicon.Enter += Tab_lexicon_Enter;
            tab_lexicon.Leave += Tab_lexicon_Leave;
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
                DerivationalAffixEditor editor = new()
                {
                    AffixRules = languageDescription.derivational_affix_map[derivationKey],
                    Location = new Point(5, 5)
                };
                tabPage.Controls.Add(editor);
                tpn_DerivationalAffixMap.TabPages.Add(tabPage);
            }
            tpn_DerivationalAffixMap.ResumeLayout(true);
            tab_derivationalAffixMap.ResumeLayout(true);

            tab_declensionAffixes.SuspendLayout();
            declensionAffixMapPane = new DeclensionAffixMapPane
            {
                AffixMap = languageDescription.affix_map,
                Size = tab_declensionAffixes.Size
            };
            tab_declensionAffixes.Controls.Add(declensionAffixMapPane);
            tab_declensionAffixes.ResumeLayout(true);

            tab_derivedWordList.SuspendLayout();
            tab_derivedWordList.AutoScroll = true;
            xPos = 0;
            yPos = 0;
            foreach (string derivedWordEntry in languageDescription.derived_word_list)
            {
                TextBox txt_derivedWord = new()
                {
                    Text = derivedWordEntry,
                    Location = new Point(xPos, yPos),
                    Size = new Size(850, 20)
                };
                yPos += 25;
                tab_derivedWordList.Controls.Add(txt_derivedWord);
            }
            TextBox txt_derivedWordBlank = new()
            {
                Text = string.Empty,
                Location = new Point(xPos, yPos),
                Size = new Size(850, 20)
            };
            tab_derivedWordList.Controls.Add(txt_derivedWordBlank);
            tab_derivedWordList.ResumeLayout(true);

            txt_languageNativePhonetic.TextChanged += Txt_languageNativePhonetic_TextChanged;
            txt_languageNameNativeEnglish.TextChanged += Txt_languageNameNativeEnglish_TextChanged;

            languageFileInfo = new FileInfo(filename);
        }

        private void Tab_soundMapList_Enter(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (soundMapEditor != null))
            {
                soundMapEditor.SoundMapList = languageDescription.spelling_pronunciation_rules;
            }
        }

        private void Tab_soundMapList_Leave(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (soundMapEditor != null))
            {
                if (soundMapEditor.SoundMapSaved)
                {
                    languageDescription.spelling_pronunciation_rules = soundMapEditor.SoundMapList;
                }
                else
                {
                    soundMapEditor.SoundMapList = languageDescription.spelling_pronunciation_rules;
                }
            }
        }

        private void Tab_lexicon_Enter(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (lexiconEditor != null))
            {
                lexiconEditor.PartOfSpeechList = languageDescription.part_of_speech_list;
                lexiconEditor.SoundMapList = languageDescription.spelling_pronunciation_rules;
                lexiconEditor.Lexicon = languageDescription.lexicon;
            }
        }

        private void Tab_lexicon_Leave(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (lexiconEditor != null))
            {
                if (lexiconEditor.Saved)
                {
                    languageDescription.lexicon = lexiconEditor.Lexicon;
                }
                else
                {
                    lexiconEditor.Lexicon = languageDescription.lexicon;
                }
            }
        }

        private void Txt_languageNameNativeEnglish_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                txt_languageNameNativeEnglish.TextChanged -= Txt_languageNameNativeEnglish_TextChanged;
                txt_languageNativePhonetic.TextChanged -= Txt_languageNativePhonetic_TextChanged;

                if (languageDescription?.spelling_pronunciation_rules != null)
                {
                    txt_languageNativePhonetic.Text = ConlangUtilities.SoundOutWord(txt_languageNameNativeEnglish.Text.Trim(), languageDescription.spelling_pronunciation_rules ?? []);
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

                if (languageDescription?.spelling_pronunciation_rules != null)
                {
                    txt_languageNameNativeEnglish.Text = ConlangUtilities.SpellWord(txt_languageNativePhonetic.Text.Trim(), languageDescription.spelling_pronunciation_rules ?? []);
                }
            }
            finally
            {
                txt_languageNameNativeEnglish.TextChanged += Txt_languageNativePhonetic_TextChanged;
                txt_languageNativePhonetic.TextChanged += Txt_languageNativePhonetic_TextChanged;
            }
        }

        private readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

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
                    DerivationalAffixEditor editor = (DerivationalAffixEditor)tabPage.Controls[0];
                    languageDescription.derivational_affix_map[tabPage.Text.Trim()] = editor.AffixRules;
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

#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
                JsonSerializerOptions jsonSerializerOptions = new(DefaultJsonSerializerOptions);
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
                JavaScriptEncoder encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                jsonSerializerOptions.Encoder = encoder;

                string jsonString = JsonSerializer.Serialize<LanguageDescription>(languageDescription, jsonSerializerOptions);
                File.WriteAllText(filename, jsonString, System.Text.Encoding.UTF8);
            }
        }

        private void ViewHelpPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpURL = @"https://github.com/RonBOakes/conlang_audio_honing/wiki/Language-Editor-Help";
            Process myProcess = new();
            try
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = helpURL;
                myProcess.Start();
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void AboutLanguageEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new();
            aboutBox.ShowDialog();
        }

        private void UseCompactJsonToolStripItem_Click(object sender, EventArgs e)
        {
            if (!useCompactJsonToolStripItem.Checked)
            {
                DefaultJsonSerializerOptions.WriteIndented = false;
                useCompactJsonToolStripItem.Checked = true;
            }
            else
            {
                DefaultJsonSerializerOptions.WriteIndented = true;
                useCompactJsonToolStripItem.Checked = false;
            }
        }
    }
}
