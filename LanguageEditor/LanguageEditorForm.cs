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
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace LanguageEditor
{
    public partial class LanguageEditorForm : Form
    {
        private LanguageDescription? languageDescription;
        private FileInfo? languageFileInfo = null;
        private LexiconEditor? lexiconEditor = null;
        private SpellingPronunciationRuleListEditor? spellingPronunciationRulesEditor = null;
        private DeclensionAffixMapPane? declensionAffixMapPane = null;
        private LexicalOrderEditor? lexicalOrderEditor = null;

        private string nounGenderText = "";
        private bool nounGenderUpdateRunning = false;
        private string partOfSpeechText = "";
        private bool partOfSpeechUpdateRunning = false;
        private string derivedWordText = "";
        private bool derivedWordUpdateRunning = false;

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
                txt_nounGender.Enter += Txt_nounGender_Enter;
                txt_nounGender.Leave += Txt_nounGender_Leave;
                txt_nounGender.KeyPress += Txt_nounGender_KeyPress;
                panel_nounGender.Controls.Add(txt_nounGender);
            }
            TextBox txt_nounGenderBlank = new()
            {
                Text = "",
                Location = new Point(xPos, yPos),
                Size = new Size(125, 12)
            };
            txt_nounGenderBlank.Enter += Txt_nounGender_Enter;
            txt_nounGenderBlank.Leave += Txt_nounGender_Leave;
            txt_nounGenderBlank.KeyPress += Txt_nounGender_KeyPress;
            panel_nounGender.Controls.Add(txt_nounGenderBlank);
            panel_nounGender.ResumeLayout(true);

            tab_spellingPronounciationRules.SuspendLayout();
            xPos = 0;
            yPos = 0;
            spellingPronunciationRulesEditor = new SpellingPronunciationRuleListEditor
            {
                Location = new Point(xPos, yPos),
                Size = new Size(895, 355),
                SoundMapList = languageDescription.spelling_pronunciation_rules
            };
            tab_spellingPronounciationRules.Controls.Add(spellingPronunciationRulesEditor);
            tab_spellingPronounciationRules.Enter += Tab_spellingPronunciationRules_Enter;
            tab_spellingPronounciationRules.Leave += Tab_spellingPronunciationRules_Leave;
            tab_spellingPronounciationRules.ResumeLayout(true);

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
                txt_partOfSpeech.Enter += Txt_partOfSpeech_Enter;
                txt_partOfSpeech.Leave += Txt_partOfSpeech_Leave;
                txt_partOfSpeech.KeyPress += Txt_partOfSpeech_KeyPress;
                panel_partsOfSpeechList.Controls.Add(txt_partOfSpeech);
            }
            TextBox txt_partOfSpeechBlank = new()
            {
                Text = "",
                Location = new Point(xPos, yPos),
                Size = new Size(125, 12)
            };
            txt_partOfSpeechBlank.Enter += Txt_partOfSpeech_Enter;
            txt_partOfSpeechBlank.Leave += Txt_partOfSpeech_Leave;
            txt_partOfSpeechBlank.KeyPress += Txt_partOfSpeech_KeyPress;
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
                    Location = new Point(5, 5),
                    SpellingPronunciationRules = languageDescription.spelling_pronunciation_rules,
                    DerivationKey = derivationKey,
                };
                editor.Delete += DerivationalAffixEditor_Delete;
                tabPage.Controls.Add(editor);
                tpn_DerivationalAffixMap.TabPages.Add(tabPage);
            }
            TabPage addDerivationalAffix = new("+");
            addDerivationalAffix.Enter += AddDerivationalAffix_Enter;
            tpn_DerivationalAffixMap.TabPages.Add(addDerivationalAffix);
            tpn_DerivationalAffixMap.ResumeLayout(true);
            tab_derivationalAffixMap.ResumeLayout(true);

            tab_declensionAffixes.SuspendLayout();
            declensionAffixMapPane = new DeclensionAffixMapPane
            {
                SpellingPronunciationRules = languageDescription.spelling_pronunciation_rules,
                AffixMap = languageDescription.affix_map,
                Size = tab_declensionAffixes.Size,
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
                txt_derivedWord.Enter += Txt_derivedWord_Enter;
                txt_derivedWord.Leave += Txt_derivedWord_Leave;
                txt_derivedWord.KeyPress += Txt_derivedWord_KeyPress;
                tab_derivedWordList.Controls.Add(txt_derivedWord);
            }
            TextBox txt_derivedWordBlank = new()
            {
                Text = string.Empty,
                Location = new Point(xPos, yPos),
                Size = new Size(850, 20)
            };
            txt_derivedWordBlank.Enter += Txt_derivedWord_Enter;
            txt_derivedWordBlank.Leave += Txt_derivedWord_Leave;
            txt_derivedWordBlank.KeyPress += Txt_derivedWord_KeyPress;
            tab_derivedWordList.Controls.Add(txt_derivedWordBlank);
            tab_derivedWordList.ResumeLayout(true);

            lexicalOrderEditor = new(languageDescription, true)
            {
                Size = new Size(895, 355)
            };
            lexicalOrderEditor.SaveAndCloseToolStripMenuItem.Text = "Save";
            lexicalOrderEditor.CloseWithoutSavingToolStripMenuItem.Text = "Do Not Save";
            tab_LexicalOrder.Enter += Tab_lexicalOrder_Enter;
            tab_LexicalOrder.Leave += Tab_lexicalOrder_Leave;
            tab_LexicalOrder.SuspendLayout();
            tab_LexicalOrder.Controls.Clear();
            tab_LexicalOrder.Controls.Add(lexicalOrderEditor);
            tab_LexicalOrder.ResumeLayout(true);

            txt_languageNativePhonetic.TextChanged += Txt_languageNativePhonetic_TextChanged;
            txt_languageNameNativeEnglish.TextChanged += Txt_languageNameNativeEnglish_TextChanged;

            languageFileInfo = new FileInfo(filename);
        }

        private void Txt_derivedWord_Enter(object? sender, EventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                return;
            }
            TextBox derivedWord = (TextBox)sender;
            derivedWordText = derivedWord.Text.Trim();
        }

        private void Txt_derivedWord_Leave(object? sender, EventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                return;
            }
            if (languageDescription == null)
            {
                return;
            }
            if (derivedWordUpdateRunning)
            {
                return;
            }
            derivedWordUpdateRunning = true;
            TextBox derivedWord = (TextBox)sender;
            if (derivedWord.Text.Trim().Equals(derivedWordText))
            {
                derivedWordUpdateRunning = false;
                return;
            }
            if (string.IsNullOrEmpty(derivedWordText))
            {
                languageDescription.derived_word_list.Add(derivedWord.Text.Trim());
            }
            else if (string.IsNullOrEmpty(derivedWord.Text.Trim()))
            {
                languageDescription.derived_word_list.Remove(derivedWordText);
            }
            else
            {
                for (int i = 0; i < languageDescription.derived_word_list.Count; i++)
                {
                    if (languageDescription.derived_word_list[i].Equals(derivedWordText))
                    {
                        languageDescription.derived_word_list[i] = derivedWord.Text.Trim();
                        break;
                    }
                }
            }
            int xPos = 0, yPos = 0;
            tab_derivedWordList.SuspendLayout();
            tab_derivedWordList.Controls.Clear();
            tab_derivedWordList.ResumeLayout(true);
            tab_derivedWordList.SuspendLayout();
            tab_derivedWordList.AutoScroll = true;
            foreach (string derivedWordEntry in languageDescription.derived_word_list)
            {
                TextBox txt_derivedWord = new()
                {
                    Text = derivedWordEntry,
                    Location = new Point(xPos, yPos),
                    Size = new Size(850, 20)
                };
                yPos += 25;
                txt_derivedWord.Enter += Txt_derivedWord_Enter;
                txt_derivedWord.Leave += Txt_derivedWord_Leave;
                tab_derivedWordList.Controls.Add(txt_derivedWord);
            }
            TextBox txt_derivedWordBlank = new()
            {
                Text = string.Empty,
                Location = new Point(xPos, yPos),
                Size = new Size(850, 20)
            };
            txt_derivedWordBlank.Enter += Txt_derivedWord_Enter;
            txt_derivedWordBlank.Leave += Txt_derivedWord_Leave;
            tab_derivedWordList.Controls.Add(txt_derivedWordBlank);
            tab_derivedWordList.ResumeLayout(true);
            derivedWordUpdateRunning = false;
        }

        private void Txt_derivedWord_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                e.Handled = false;
                return;
            }
            if (languageDescription == null)
            {
                e.Handled = false;
                return;
            }
            if (derivedWordUpdateRunning)
            {
                e.Handled = false;
                return;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                derivedWordUpdateRunning = true;
                TextBox derivedWord = (TextBox)sender;
                if (derivedWord.Text.Trim().Equals(derivedWordText))
                {
                    derivedWordUpdateRunning = false;
                    return;
                }
                if (string.IsNullOrEmpty(derivedWordText))
                {
                    languageDescription.derived_word_list.Add(derivedWord.Text.Trim());
                }
                else if (string.IsNullOrEmpty(derivedWord.Text.Trim()))
                {
                    languageDescription.derived_word_list.Remove(derivedWordText);
                }
                else
                {
                    for (int i = 0; i < languageDescription.derived_word_list.Count; i++)
                    {
                        if (languageDescription.derived_word_list[i].Equals(derivedWordText))
                        {
                            languageDescription.derived_word_list[i] = derivedWord.Text.Trim();
                            break;
                        }
                    }
                }
                int xPos = 0, yPos = 0;
                tab_derivedWordList.SuspendLayout();
                tab_derivedWordList.Controls.Clear();
                tab_derivedWordList.ResumeLayout(true);
                tab_derivedWordList.SuspendLayout();
                tab_derivedWordList.AutoScroll = true;
                foreach (string derivedWordEntry in languageDescription.derived_word_list)
                {
                    TextBox txt_derivedWord = new()
                    {
                        Text = derivedWordEntry,
                        Location = new Point(xPos, yPos),
                        Size = new Size(850, 20)
                    };
                    yPos += 25;
                    txt_derivedWord.Enter += Txt_derivedWord_Enter;
                    txt_derivedWord.Leave += Txt_derivedWord_Leave;
                    txt_derivedWord.KeyPress += Txt_derivedWord_KeyPress;
                    tab_derivedWordList.Controls.Add(txt_derivedWord);
                }
                TextBox txt_derivedWordBlank = new()
                {
                    Text = string.Empty,
                    Location = new Point(xPos, yPos),
                    Size = new Size(850, 20)
                };
                txt_derivedWordBlank.Enter += Txt_derivedWord_Enter;
                txt_derivedWordBlank.Leave += Txt_derivedWord_Leave;
                txt_derivedWordBlank.KeyPress += Txt_derivedWord_KeyPress;
                tab_derivedWordList.Controls.Add(txt_derivedWordBlank);
                tab_derivedWordList.ResumeLayout(true);
                derivedWordUpdateRunning = false;
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }


            private void DerivationalAffixEditor_Delete(object? sender, EventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.GetType() != typeof(DerivationalAffixEditor.DerivationalAffixEntryDeleteEventArgs))
            {
                return;
            }
            if (sender?.GetType() != typeof(DerivationalAffixEditor))
            {
                return;
            }
            if (languageDescription == null)
            {
                return;
            }
            DerivationalAffixEditor.DerivationalAffixEntryDeleteEventArgs eventArgs = (DerivationalAffixEditor.DerivationalAffixEntryDeleteEventArgs)e;

#pragma warning disable S2589 // Boolean expressions should not be gratuitous
            string derivationKey = eventArgs?.DerivationKey ?? string.Empty;
#pragma warning restore S2589 // Boolean expressions should not be gratuitous

#pragma warning disable CA1853 // Unnecessary call to 'Dictionary.ContainsKey(key)'
            if (languageDescription.derivational_affix_map.ContainsKey(derivationKey))
            {
                languageDescription.derivational_affix_map.Remove(derivationKey);
                DialogResult result = MessageBox.Show(string.Format("Remove Derived Word Rules based on {0}?", derivationKey), "Remove Rules", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    List<string> rulesToRemove =
                    [
                        .. from string rule in languageDescription.derived_word_list
                                               where rule.Contains(derivationKey)
                                               select rule,
                    ];
                    foreach (string ruleToRemove in rulesToRemove)
                    {
                        languageDescription.derived_word_list.Remove(ruleToRemove);
                    }
                }
                tpn_DerivationalAffixMap.SuspendLayout();
                // current should be set to the tab containing the editor where "Delete" was pressed.
                TabPage? current = tpn_DerivationalAffixMap.SelectedTab;
                if (current != null)
                {
                    tpn_DerivationalAffixMap.SelectedTab = null;
                    tpn_DerivationalAffixMap.TabPages.Remove(current);
                }
                tpn_DerivationalAffixMap.ResumeLayout(true);
            }
#pragma warning restore CA1853 // Unnecessary call to 'Dictionary.ContainsKey(key)'
        }

        private void AddDerivationalAffix_Enter(object? sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            string newIndex = Microsoft.VisualBasic.Interaction.InputBox("Enter the new Derivation Key", "Derivation Key", "");
            newIndex = newIndex.ToUpper();
            newIndex = WhiteSpaceRegex().Replace(newIndex, ".");
            DerivationalAffix derivationalAffix = new();
            languageDescription.derivational_affix_map[newIndex] = derivationalAffix;
            DerivationalAffixEditor editor = new()
            {
                AffixRules = languageDescription.derivational_affix_map[newIndex],
                Location = new Point(5, 5),
                SpellingPronunciationRules = languageDescription.spelling_pronunciation_rules,
                DerivationKey = newIndex,
            };
            editor.Delete += DerivationalAffixEditor_Delete;
            TabPage tabPage = new(newIndex);
            tabPage.Controls.Add(editor);
            tpn_DerivationalAffixMap.SuspendLayout();
            tpn_DerivationalAffixMap.TabPages.Insert(tpn_DerivationalAffixMap.TabPages.Count - 1, tabPage);
            tpn_DerivationalAffixMap.ResumeLayout(true);
            tpn_DerivationalAffixMap.SelectedTab = tabPage;
        }

        private void Tab_spellingPronunciationRules_Enter(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (spellingPronunciationRulesEditor != null))
            {
                spellingPronunciationRulesEditor.SoundMapList = languageDescription.spelling_pronunciation_rules;
            }
        }

        private void Tab_spellingPronunciationRules_Leave(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (spellingPronunciationRulesEditor != null))
            {
                if (spellingPronunciationRulesEditor.SoundMapSaved)
                {
                    languageDescription.spelling_pronunciation_rules = spellingPronunciationRulesEditor.SoundMapList;
                    DialogResult result = SpellingPronunciationRuleListEditorForm.SpellingPronunciationDialogBox();
                    if (result == DialogResult.Yes)
                    {
                        PhoneticChanger phoneticChanger = new()
                        {
                            Language = languageDescription
                        };
                        phoneticChanger.UpdatePronunciation();
                        languageDescription.lexicon = phoneticChanger.Language.lexicon;
                    }
                    else if (result == DialogResult.No)
                    {
                        PhoneticChanger phoneticChanger = new()
                        {
                            Language = languageDescription
                        };
                        phoneticChanger.UpdateSpelling();
                        languageDescription.lexicon = phoneticChanger.Language.lexicon;
                    }
                }
                else
                {
                    spellingPronunciationRulesEditor.SoundMapList = languageDescription.spelling_pronunciation_rules;
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

        private void Tab_lexicalOrder_Enter(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (lexicalOrderEditor != null))
            {
                lexicalOrderEditor.Language = languageDescription;
                lexicalOrderEditor.RegenerateLexicalOrder();
            }
        }

        private void Tab_lexicalOrder_Leave(object? sender, EventArgs e)
        {
            if ((languageDescription != null) && (lexicalOrderEditor != null))
            {
                if (lexicalOrderEditor.Saved)
                {
                    languageDescription.lexical_order_list = lexicalOrderEditor.Language.lexical_order_list;
                }
                else
                {
                    lexicalOrderEditor.Language = languageDescription;
                    lexicalOrderEditor.RegenerateLexicalOrder();
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
                    foreach (object control in tabPage.Controls)
                    {
                        if (control.GetType() == typeof(DerivationalAffixEditor))
                        {
                            DerivationalAffixEditor editor = (DerivationalAffixEditor)control;
                            languageDescription.derivational_affix_map[tabPage.Text.Trim()] = editor.AffixRules;
                            break;
                        }
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
#pragma warning disable S1075 // URIs should not be hardcoded
            string helpURL = @"https://github.com/RonBOakes/conlang_audio_honing/wiki/Language-Editor-Help";
#pragma warning restore S1075 // URIs should not be hardcoded
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

        private void Txt_nounGender_Enter(object? sender, EventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                return;
            }
            TextBox nounGender = (TextBox)sender;
            nounGenderText = nounGender.Text.Trim();
        }

        private void Txt_nounGender_Leave(object? sender, EventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                return;
            }
            if (languageDescription == null)
            {
                return;
            }
            if (nounGenderUpdateRunning)
            {
                return;
            }
            nounGenderUpdateRunning = true;
            TextBox nounGender = (TextBox)sender;
            if (nounGender.Text.Trim().Equals(nounGenderText))
            {
                nounGenderUpdateRunning = false;
                return;
            }
            if (string.IsNullOrEmpty(nounGenderText))
            {
                languageDescription.noun_gender_list.Add(nounGender.Text.Trim());
            }
            else if (string.IsNullOrEmpty(nounGender.Text.Trim()))
            {
                languageDescription.noun_gender_list.Remove(nounGenderText);
            }
            else
            {
                for (int i = 0; i < languageDescription.noun_gender_list.Count; i++)
                {
                    if (languageDescription.noun_gender_list[i].Equals(nounGenderText))
                    {
                        languageDescription.noun_gender_list[i] = nounGender.Text.Trim();
                        break;
                    }
                }
            }
            int xPos = 0, yPos = 0;
            panel_nounGender.SuspendLayout();
            panel_nounGender.Controls.Clear();
            foreach (string noun2 in languageDescription.noun_gender_list)
            {
                TextBox txt_nounGender = new()
                {
                    Text = noun2,
                    Location = new Point(xPos, yPos),
                    Size = new Size(125, 12)
                };
                yPos += 20;
                txt_nounGender.Enter += Txt_nounGender_Enter;
                txt_nounGender.Leave += Txt_nounGender_Leave;
                panel_nounGender.Controls.Add(txt_nounGender);
            }
            TextBox txt_nounGenderBlank = new()
            {
                Text = "",
                Location = new Point(xPos, yPos),
                Size = new Size(125, 12)
            };
            txt_nounGenderBlank.Enter += Txt_nounGender_Enter;
            txt_nounGenderBlank.Leave += Txt_nounGender_Leave;
            panel_nounGender.Controls.Add(txt_nounGenderBlank);
            panel_nounGender.ResumeLayout(true);
            nounGenderUpdateRunning = false;
        }

        private void Txt_nounGender_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                e.Handled = false;
                return;
            }
            if (languageDescription == null)
            {
                e.Handled = false;
                return;
            }
            if (nounGenderUpdateRunning)
            {
                e.Handled = false;
                return;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                nounGenderUpdateRunning = true;
                TextBox nounGender = (TextBox)sender;
                if (nounGender.Text.Trim().Equals(nounGenderText))
                {
                    nounGenderUpdateRunning = false;
                    return;
                }
                if (string.IsNullOrEmpty(nounGenderText))
                {
                    languageDescription.noun_gender_list.Add(nounGender.Text.Trim());
                }
                else if (string.IsNullOrEmpty(nounGender.Text.Trim()))
                {
                    languageDescription.noun_gender_list.Remove(nounGenderText);
                }
                else
                {
                    for (int i = 0; i < languageDescription.noun_gender_list.Count; i++)
                    {
                        if (languageDescription.noun_gender_list[i].Equals(nounGenderText))
                        {
                            languageDescription.noun_gender_list[i] = nounGender.Text.Trim();
                            break;
                        }
                    }
                }
                int xPos = 0, yPos = 0;
                panel_nounGender.SuspendLayout();
                panel_nounGender.Controls.Clear();
                foreach (string noun2 in languageDescription.noun_gender_list)
                {
                    TextBox txt_nounGender = new()
                    {
                        Text = noun2,
                        Location = new Point(xPos, yPos),
                        Size = new Size(125, 12)
                    };
                    yPos += 20;
                    txt_nounGender.Enter += Txt_nounGender_Enter;
                    txt_nounGender.Leave += Txt_nounGender_Leave;
                    txt_nounGender.KeyPress += Txt_nounGender_KeyPress;
                    panel_nounGender.Controls.Add(txt_nounGender);
                }
                TextBox txt_nounGenderBlank = new()
                {
                    Text = "",
                    Location = new Point(xPos, yPos),
                    Size = new Size(125, 12)
                };
                txt_nounGenderBlank.Enter += Txt_nounGender_Enter;
                txt_nounGenderBlank.Leave += Txt_nounGender_Leave;
                txt_nounGenderBlank.KeyPress += Txt_nounGender_KeyPress;
                panel_nounGender.Controls.Add(txt_nounGenderBlank);
                panel_nounGender.ResumeLayout(true);
                nounGenderUpdateRunning = false;
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void Txt_partOfSpeech_Enter(object? sender, EventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                return;
            }
            TextBox partOfSpeech = (TextBox)sender;
            partOfSpeechText = partOfSpeech.Text.Trim();
        }

        private void Txt_partOfSpeech_Leave(object? sender, EventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                return;
            }
            if (languageDescription == null)
            {
                return;
            }
            if (partOfSpeechUpdateRunning)
            {
                return;
            }
            partOfSpeechUpdateRunning = true;
            TextBox partOfSpeech = (TextBox)sender;
            if (partOfSpeech.Text.Trim().Equals(partOfSpeechText))
            {
                return;
            }
            if (string.IsNullOrEmpty(partOfSpeechText))
            {
                languageDescription.part_of_speech_list.Add(partOfSpeech.Text.Trim());
            }
            else if (string.IsNullOrEmpty(partOfSpeech.Text.Trim()))
            {
                languageDescription.part_of_speech_list.Remove(partOfSpeechText);
                DialogResult result = MessageBox.Show("Remove all words of this Part of Speech from the Lexicon?", "Remove Words", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    languageDescription.lexicon = lexiconEditor?.Lexicon ?? [];
#pragma warning disable IDE0028 // Simplify collection initialization
                    List<LexiconEntry> entriesToRemove = new();
#pragma warning restore IDE0028 // Simplify collection initialization
                    entriesToRemove.AddRange(from LexiconEntry entry in languageDescription.lexicon
                                             where entry.part_of_speech.Equals(partOfSpeechText)
                                             select entry);
                    foreach (LexiconEntry entry in entriesToRemove)
                    {
                        languageDescription.lexicon.Remove(entry);
                    }
                    if (lexiconEditor != null)
                    {
                        lexiconEditor.Lexicon = languageDescription.lexicon;
                    }
                }
            }
            else
            {
                for (int i = 0; i < languageDescription.part_of_speech_list.Count; i++)
                {
                    if (languageDescription.part_of_speech_list[i].Equals(partOfSpeechText))
                    {
                        languageDescription.part_of_speech_list[i] = partOfSpeech.Text.Trim();
                        foreach (var entry in from LexiconEntry entry in languageDescription.lexicon
                                              where entry.part_of_speech.Equals(partOfSpeechText)
                                              select entry)
                        {
                            entry.part_of_speech = partOfSpeech.Text.Trim();
                        }
                    }
                }
            }
            languageDescription.part_of_speech_list.Sort();
            int xPos = 0;
            int yPos = 0;
            panel_partsOfSpeechList.SuspendLayout();
            panel_partsOfSpeechList.Controls.Clear();
            foreach (string partOfSpeech2 in languageDescription.part_of_speech_list)
            {
                TextBox txt_partOfSpeech = new()
                {
                    Text = partOfSpeech2,
                    Location = new Point(xPos, yPos),
                    Size = new Size(125, 12)
                };
                yPos += 20;
                txt_partOfSpeech.Enter += Txt_partOfSpeech_Enter;
                txt_partOfSpeech.Leave += Txt_partOfSpeech_Leave;
                panel_partsOfSpeechList.Controls.Add(txt_partOfSpeech);
            }
            TextBox txt_partOfSpeechBlank = new()
            {
                Text = "",
                Location = new Point(xPos, yPos),
                Size = new Size(125, 12)
            };
            txt_partOfSpeechBlank.Enter += Txt_partOfSpeech_Enter;
            txt_partOfSpeechBlank.Leave += Txt_partOfSpeech_Leave;
            panel_partsOfSpeechList.Controls.Add(txt_partOfSpeechBlank);
            panel_partsOfSpeechList.ResumeLayout(true);
            partOfSpeechUpdateRunning = false;
        }

        private void Txt_partOfSpeech_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(TextBox)))
            {
                e.Handled = false;
                return;
            }
            if (languageDescription == null)
            {
                e.Handled = false;
                return;
            }
            if (partOfSpeechUpdateRunning)
            {
                e.Handled = false;
                return;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                partOfSpeechUpdateRunning = true;
                TextBox partOfSpeech = (TextBox)sender;
                if (partOfSpeech.Text.Trim().Equals(partOfSpeechText))
                {
                    return;
                }
                if (string.IsNullOrEmpty(partOfSpeechText))
                {
                    languageDescription.part_of_speech_list.Add(partOfSpeech.Text.Trim());
                }
                else if (string.IsNullOrEmpty(partOfSpeech.Text.Trim()))
                {
                    languageDescription.part_of_speech_list.Remove(partOfSpeechText);
                    DialogResult result = MessageBox.Show("Remove all words of this Part of Speech from the Lexicon?", "Remove Words", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        languageDescription.lexicon = lexiconEditor?.Lexicon ?? [];
#pragma warning disable IDE0028 // Simplify collection initialization
                        List<LexiconEntry> entriesToRemove = new();
#pragma warning restore IDE0028 // Simplify collection initialization
                        entriesToRemove.AddRange(from LexiconEntry entry in languageDescription.lexicon
                                                 where entry.part_of_speech.Equals(partOfSpeechText)
                                                 select entry);
                        foreach (LexiconEntry entry in entriesToRemove)
                        {
                            languageDescription.lexicon.Remove(entry);
                        }
                        if (lexiconEditor != null)
                        {
                            lexiconEditor.Lexicon = languageDescription.lexicon;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < languageDescription.part_of_speech_list.Count; i++)
                    {
                        if (languageDescription.part_of_speech_list[i].Equals(partOfSpeechText))
                        {
                            languageDescription.part_of_speech_list[i] = partOfSpeech.Text.Trim();
                            foreach (var entry in from LexiconEntry entry in languageDescription.lexicon
                                                  where entry.part_of_speech.Equals(partOfSpeechText)
                                                  select entry)
                            {
                                entry.part_of_speech = partOfSpeech.Text.Trim();
                            }
                        }
                    }
                }
                languageDescription.part_of_speech_list.Sort();
                int xPos = 0;
                int yPos = 0;
                panel_partsOfSpeechList.SuspendLayout();
                panel_partsOfSpeechList.Controls.Clear();
                foreach (string partOfSpeech2 in languageDescription.part_of_speech_list)
                {
                    TextBox txt_partOfSpeech = new()
                    {
                        Text = partOfSpeech2,
                        Location = new Point(xPos, yPos),
                        Size = new Size(125, 12)
                    };
                    yPos += 20;
                    txt_partOfSpeech.Enter += Txt_partOfSpeech_Enter;
                    txt_partOfSpeech.Leave += Txt_partOfSpeech_Leave;
                    txt_partOfSpeech.KeyPress += Txt_partOfSpeech_KeyPress;
                    panel_partsOfSpeechList.Controls.Add(txt_partOfSpeech);
                }
                TextBox txt_partOfSpeechBlank = new()
                {
                    Text = "",
                    Location = new Point(xPos, yPos),
                    Size = new Size(125, 12)
                };
                txt_partOfSpeechBlank.Enter += Txt_partOfSpeech_Enter;
                txt_partOfSpeechBlank.Leave += Txt_partOfSpeech_Leave;
                txt_partOfSpeechBlank.KeyPress += Txt_partOfSpeech_KeyPress;
                panel_partsOfSpeechList.Controls.Add(txt_partOfSpeechBlank);
                panel_partsOfSpeechList.ResumeLayout(true);
                partOfSpeechUpdateRunning = false;
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhiteSpaceRegex();
    }
}
