/*
 * Class for manipulating phonetics.
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
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using ConlangJson;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Handles the phonetic changes for the Conlang Honing Application.
    /// </summary>
    public partial class PhoneticChanger
    {

        /// <summary>
        /// Constructs a PhoneticChanges with a known language and sample text.
        /// </summary>
        /// <param name="languageDescription">LanguageDescription object for the language to be worked on</param>
        /// <param name="sampleText">string containing the sample text used for speaking and phonetic representation.</param>
        public PhoneticChanger(LanguageDescription languageDescription, string sampleText)
        {
            this.Language = languageDescription;
            this.SampleText = sampleText;
        }

        /// <summary>
        /// Default constructor for a PhoneticChanges object.
        /// </summary>
        public PhoneticChanger()
        {
            this.Language = new LanguageDescription();
            this.SampleText = string.Empty;
        }

        /// <summary>
        /// LanguageDescription object of the language the phonetic changer object is working on.
        /// </summary>
        public LanguageDescription Language { get; set; }

        /// <summary>
        /// String containing the sample text used for speaking and phonetic representation.
        /// </summary>
        public string SampleText { get; set; }

        /// <summary>
        /// Update the language loaded into this PhoneticChanger by replacing the oldPhoneme with
        /// the newPhoneme in the lexicon.  The phonetic_inventory will also be updated.  The
        /// spelling_pronunciation_rules may also be updated depending on user interactions.
        /// </summary>
        /// <param name="oldPhoneme">Phoneme to be replaced.</param>
        /// <param name="newPhoneme">Replacement phoneme.</param>
        public void PhoneticChange(string oldPhoneme, string newPhoneme)
        {
            if (this.Language == null)
            {
                return;
            }

            SpellingPronunciationRuleListEditorForm soundMapListEditorForm = new();
            _ = Language.spelling_pronunciation_rules.GetRange(0, Language.spelling_pronunciation_rules.Count);
            soundMapListEditorForm.SoundMapList = Language.spelling_pronunciation_rules;
            soundMapListEditorForm.PhonemeReplacementPairs.Add((oldPhoneme, newPhoneme));
            soundMapListEditorForm.UpdatePhonemeReplacements();
            _ = soundMapListEditorForm.ShowDialog();
            // ShowDialog is modal
            if (soundMapListEditorForm.SoundMapSaved)
            {
                Language.spelling_pronunciation_rules = soundMapListEditorForm.SoundMapList;
            }

            Cursor.Current = Cursors.WaitCursor;

            string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";

            // Update the Lexicon
            foreach (LexiconEntry word in Language.lexicon)
            {
                // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                LexiconEntry oldVersion = word.copy();
                string oldPhonetic = word.phonetic;

                // Preserve vowel diphthongs before doing the main replacement
                Dictionary<string, string> diphthongReplacementMap = [];
                int ipaReplacementIndex = 0;
                foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                {
                    if (!diphthong.Equals(oldPhoneme))
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        word.phonetic = word.phonetic.Replace(diphthong, ipaReplacement);
                    }
                }

                word.phonetic = Regex.Replace(word.phonetic, replacementPattern, newPhoneme);

                // Put the replaced diphthongs back
                foreach (string diphthong in diphthongReplacementMap.Keys)
                {
                    word.phonetic = word.phonetic.Replace(diphthongReplacementMap[diphthong], diphthong);
                }
                if (!oldPhonetic.Equals(word.phonetic))
                {
                    string oldSpelled = word.spelled;
                    word.spelled = ConlangUtilities.SpellWord(word.phonetic, Language.spelling_pronunciation_rules);
                    UpdateSampleText(oldSpelled, word.spelled);
                    word.metadata ??= [];
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    phoneticChangeHistories ??= [];
                    MetadataWasher.CleanLexiconEntryMetadata(oldVersion, true);
                    PhoneticChangeHistory pch = new()
                    {
                        OldPhoneme = oldPhoneme,
                        NewPhoneme = newPhoneme,
                        OldVersion = oldVersion
                    };
                    string timestamp = string.Format("{0:yyyyMMddhhmmss.ffff}", DateTime.Now);
                    phoneticChangeHistories.Add(timestamp, pch);
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);
                }
            }

            // Update the phoneme inventory - the next update depends on it for diphthongs (at least)
            for (int i = 0; i < Language.phoneme_inventory.Count; i++)
            {

                if (Language.phoneme_inventory[i].Trim() == oldPhoneme.Trim())
                {
                    Language.phoneme_inventory[i] = newPhoneme;
                }
            }

            // Update the Affix Map
            foreach (string partOfSpeech in Language.affix_map.Keys)
            {
                //foreach(List<Dictionary<string, List<Dictionary<string, Affix>>>> byPartOfSpeech in Language.affix_map[partOfSpeech])
                foreach (Dictionary<string, List<Dictionary<string, Affix>>> byPartOfSpeech in Language.affix_map[partOfSpeech])
                {
                    foreach (string affixType in byPartOfSpeech.Keys)
                    {
                        foreach (Dictionary<string, Affix> byAffixType in byPartOfSpeech[affixType])
                        {
                            foreach (string declension in byAffixType.Keys)
                            {
                                Affix affix = byAffixType[declension];

                                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        if (!diphthong.Equals(oldPhoneme))
                                        {
                                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                            affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                                        }
                                    }
                                    affix.pronunciation_add = Regex.Replace(affix.pronunciation_add, replacementPattern, newPhoneme);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        if (!diphthong.Equals(oldPhoneme))
                                        {
                                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                            affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                                        }
                                    }
                                    affix.pronunciation_regex = Regex.Replace(affix.pronunciation_regex, replacementPattern, newPhoneme);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                }
                                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        if (!diphthong.Equals(oldPhoneme))
                                        {
                                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                            affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                                        }
                                    }
                                    affix.t_pronunciation_add = Regex.Replace(affix.t_pronunciation_add, replacementPattern, newPhoneme);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        if (!diphthong.Equals(oldPhoneme))
                                        {
                                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                            affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                                        }
                                    }
                                    affix.f_pronunciation_add = Regex.Replace(affix.f_pronunciation_add, replacementPattern, newPhoneme);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                            }
                        }
                    }
                }
            }

            // Update the derivational affix map
            foreach (string derivation in Language.derivational_affix_map.Keys)
            {
                DerivationalAffix affix = Language.derivational_affix_map[derivation];
                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        if (!diphthong.Equals(oldPhoneme))
                        {
                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                            affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                        }
                    }
                    affix.pronunciation_add = Regex.Replace(affix.pronunciation_add, replacementPattern, newPhoneme);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        if (!diphthong.Equals(oldPhoneme))
                        {
                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                            affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                        }
                    }
                    affix.pronunciation_regex = Regex.Replace(affix.pronunciation_regex, replacementPattern, newPhoneme);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                }
                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        if (!diphthong.Equals(oldPhoneme))
                        {
                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                            affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                        }
                    }
                    affix.t_pronunciation_add = Regex.Replace(affix.t_pronunciation_add, replacementPattern, newPhoneme);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        if (!diphthong.Equals(oldPhoneme))
                        {
                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                            affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                        }
                    }
                    affix.f_pronunciation_add = Regex.Replace(affix.f_pronunciation_add, replacementPattern, newPhoneme);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                }
            }

            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Update the language loaded into this PhoneticChanger by replacing each of the the oldPhoneme with
        /// the newPhoneme from the change list within the lexicon.  The phonetic_inventory will also be updated.  
        /// The spelling_pronunciation_rules may also be updated depending on user interactions.
        /// <br/>The changes will be applied so that they impact independently without any inherent sequence.  
        /// If one entry has a value in "oldPhoneme" and it also in "newPhoneme" these changes will be made
        /// independently and will not impact each other.  Circular changes are supported: e.g. p-&gt;b, 
        /// b-&gt;k, k-&gt;p
        /// </summary>
        /// <param name="changeList"></param>
        public void PhoneticChange(List<(string oldPhoneme, string newPhoneme)> changeList)
        {
            if (this.Language == null)
            {
                return;
            }

            // The limit is 10 changes in a list due to how the replacements are done
            if (changeList.Count > 10)
            {
                return;
            }

            // Sort the list by size and then reverse it so that the longest 
            changeList.Sort(new StringTupleLengthComp());
            changeList.Reverse();

            SpellingPronunciationRuleListEditorForm soundMapListEditorForm = new()
            {
                SoundMapList = Language.spelling_pronunciation_rules
            };
            soundMapListEditorForm.PhonemeReplacementPairs.AddRange(changeList);
            soundMapListEditorForm.UpdatePhonemeReplacements();
            _ = soundMapListEditorForm.ShowDialog();
            // ShowDialog is modal
            if (soundMapListEditorForm.SoundMapSaved)
            {
                Language.spelling_pronunciation_rules = soundMapListEditorForm.SoundMapList;
            }
            Cursor.Current = Cursors.WaitCursor;

            string changeHistoryTimestamp = string.Format("{0:yyyyMMdd.hhmmssfffffff}", DateTime.Now);


            Dictionary<string, (string oldPhoneme, string newPhoneme)> interimReplacementMap = [];

            int interimReplacementSymbolInt = 0;
            foreach ((string oldPhoneme, string newPhoneme) in changeList)
            {
                string interimReplacementSymbol = string.Format("{0}", interimReplacementSymbolInt++);
                interimReplacementMap.Add(interimReplacementSymbol, (oldPhoneme, newPhoneme));
                string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                // Update the Lexicon phase 1
                foreach (LexiconEntry word in Language.lexicon)
                {
                    LexiconEntry oldVersion = word.copy();
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        if (!diphthong.Equals(oldPhoneme))
                        {
                            string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                            diphthongReplacementMap.Add(diphthong, ipaReplacement);
                            word.phonetic = word.phonetic.Replace(diphthong, ipaReplacement);
                        }
                    }

                    // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                    word.phonetic = Regex.Replace(word.phonetic, replacementPattern, interimReplacementSymbol);
                    // Put the replaced diphthongs back
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        word.phonetic = word.phonetic.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    word.metadata ??= [];
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    phoneticChangeHistories ??= [];
                    if (!phoneticChangeHistories.ContainsKey(changeHistoryTimestamp))
                    {
                        PhoneticChangeHistory pch = new()
                        {
                            OldPhoneme = oldPhoneme,
                            NewPhoneme = newPhoneme,
                            OldVersion = oldVersion
                        };
                        phoneticChangeHistories.Add(changeHistoryTimestamp, pch);
                    }
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);

                }
                // Update the phoneme inventory - the next update depends on it for diphthongs (at least)
                for (int i = 0; i < Language.phoneme_inventory.Count; i++)
                {

                    if (Language.phoneme_inventory[i].Trim() == oldPhoneme.Trim())
                    {
                        Language.phoneme_inventory[i] = newPhoneme;
                    }
                }
            }

            // Update the Lexicon
            foreach (LexiconEntry word in Language.lexicon)
            {
                LexiconEntry oldVersion = word.copy();
                foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                {
                    (string oldPhoneme2, string newPhoneme2) = interimReplacementMap[interimReplacementSymbol];
                    // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                    // Recreate the original phonetic version.
                    _ = oldVersion.phonetic.Replace(interimReplacementSymbol, oldPhoneme2);
                    word.phonetic = word.phonetic.Replace(interimReplacementSymbol, newPhoneme2);
                }
                if (!oldVersion.phonetic.Equals(word.phonetic))
                {
                    string oldSpelled = word.spelled;
                    word.spelled = ConlangUtilities.SpellWord(word.phonetic, Language.spelling_pronunciation_rules);
                    UpdateSampleText(oldSpelled, word.spelled);
                }
            }

            Dictionary<(string, string), string> reverseInterimReplacementMap = [];
            foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
            {
                reverseInterimReplacementMap.Add(interimReplacementMap[interimReplacementSymbol], interimReplacementSymbol);
            }

#pragma warning disable S1481 // Unused local variables should be removed
            // Update the Affix Map
            foreach (string partOfSpeech in Language.affix_map.Keys)
            {
                foreach (Dictionary<string, List<Dictionary<string, Affix>>> byPartOfSpeech in Language.affix_map[partOfSpeech])
                {
                    foreach (string affixType in byPartOfSpeech.Keys)
                    {
                        foreach (Dictionary<string, Affix> byAffixType in byPartOfSpeech[affixType])
                        {
                            foreach (string declension in byAffixType.Keys)
                            {
                                Affix affix = byAffixType[declension];

                                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                                {
                                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                                    {
                                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                                        // Preserve vowel diphthongs before doing the main replacement
                                        Dictionary<string, string> diphthongReplacementMap = [];
                                        int ipaReplacementIndex = 0;
                                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                        {
                                            if (!diphthong.Equals(oldPhoneme))
                                            {
                                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                                affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                                            }
                                        }
                                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                                        affix.pronunciation_add = Regex.Replace(affix.pronunciation_add, replacementPattern, interimReplacementSymbol);
                                        // Put the replaced diphthongs back
                                        foreach (string diphthong in diphthongReplacementMap.Keys)
                                        {
                                            affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                        }
                                    }
                                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                                    {
                                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                                        affix.pronunciation_add = affix.pronunciation_add.Replace(interimReplacementSymbol, newPhoneme);
                                    }
                                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                                {
                                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                                    {
                                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                                        // Preserve vowel diphthongs before doing the main replacement
                                        Dictionary<string, string> diphthongReplacementMap = [];
                                        int ipaReplacementIndex = 0;
                                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                        {
                                            if (!diphthong.Equals(oldPhoneme))
                                            {
                                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                                affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                                            }
                                        }
                                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                                        affix.pronunciation_regex = Regex.Replace(affix.pronunciation_regex, replacementPattern, interimReplacementSymbol);
                                        // Put the replaced diphthongs back
                                        foreach (string diphthong in diphthongReplacementMap.Keys)
                                        {
                                            affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                                        }
                                    }
                                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                                    {
                                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(interimReplacementSymbol, newPhoneme);
                                    }
                                }
                                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                                {
                                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                                    {
                                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                                        // Preserve vowel diphthongs before doing the main replacement
                                        Dictionary<string, string> diphthongReplacementMap = [];
                                        int ipaReplacementIndex = 0;
                                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                        {
                                            if (!diphthong.Equals(oldPhoneme))
                                            {
                                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                                affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                                            }
                                        }
                                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                                        affix.t_pronunciation_add = Regex.Replace(affix.t_pronunciation_add, replacementPattern, interimReplacementSymbol);
                                        // Put the replaced diphthongs back
                                        foreach (string diphthong in diphthongReplacementMap.Keys)
                                        {
                                            affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                        }
                                    }
                                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                                    {
                                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(interimReplacementSymbol, newPhoneme);
                                    }
                                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                                {
                                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                                    {
                                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                                        // Preserve vowel diphthongs before doing the main replacement
                                        Dictionary<string, string> diphthongReplacementMap = [];
                                        int ipaReplacementIndex = 0;
                                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                        {
                                            if (!diphthong.Equals(oldPhoneme))
                                            {
                                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                                affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                                            }
                                        }
                                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                                        affix.f_pronunciation_add = Regex.Replace(affix.f_pronunciation_add, replacementPattern, interimReplacementSymbol);
                                        // Put the replaced diphthongs back
                                        foreach (string diphthong in diphthongReplacementMap.Keys)
                                        {
                                            affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                        }
                                    }
                                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                                    {
                                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(interimReplacementSymbol, newPhoneme);
                                    }
                                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                            }
                        }
                    }
                }
            }

            // Update the derivational affix map
            foreach (string derivation in Language.derivational_affix_map.Keys)
            {
                DerivationalAffix affix = Language.derivational_affix_map[derivation];

                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                {
                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                    {
                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                        // Preserve vowel diphthongs before doing the main replacement
                        Dictionary<string, string> diphthongReplacementMap = [];
                        int ipaReplacementIndex = 0;
                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                        {
                            if (!diphthong.Equals(oldPhoneme))
                            {
                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                            }
                        }
                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                        affix.pronunciation_add = Regex.Replace(affix.pronunciation_add, replacementPattern, interimReplacementSymbol);
                        // Put the replaced diphthongs back
                        foreach (string diphthong in diphthongReplacementMap.Keys)
                        {
                            affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                        }
                    }
                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                    {
                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                        affix.pronunciation_add = affix.pronunciation_add.Replace(interimReplacementSymbol, newPhoneme);
                    }
                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                {
                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                    {
                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                        // Preserve vowel diphthongs before doing the main replacement
                        Dictionary<string, string> diphthongReplacementMap = [];
                        int ipaReplacementIndex = 0;
                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                        {
                            if (!diphthong.Equals(oldPhoneme))
                            {
                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                            }
                        }
                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                        affix.pronunciation_regex = Regex.Replace(affix.pronunciation_regex, replacementPattern, interimReplacementSymbol);
                        // Put the replaced diphthongs back
                        foreach (string diphthong in diphthongReplacementMap.Keys)
                        {
                            affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                        }
                    }
                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                    {
                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(interimReplacementSymbol, newPhoneme);
                    }
                }
                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                {
                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                    {
                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                        // Preserve vowel diphthongs before doing the main replacement
                        Dictionary<string, string> diphthongReplacementMap = [];
                        int ipaReplacementIndex = 0;
                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                        {
                            if (!diphthong.Equals(oldPhoneme))
                            {
                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                            }
                        }
                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                        affix.t_pronunciation_add = Regex.Replace(affix.t_pronunciation_add, replacementPattern, interimReplacementSymbol);
                        // Put the replaced diphthongs back
                        foreach (string diphthong in diphthongReplacementMap.Keys)
                        {
                            affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                        }
                    }
                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                    {
                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(interimReplacementSymbol, newPhoneme);
                    }
                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                {
                    foreach ((string oldPhoneme, string newPhoneme) in changeList)
                    {
                        string interimReplacementSymbol = reverseInterimReplacementMap[(oldPhoneme, newPhoneme)];
                        string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                        // Preserve vowel diphthongs before doing the main replacement
                        Dictionary<string, string> diphthongReplacementMap = [];
                        int ipaReplacementIndex = 0;
                        foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                        {
                            if (!diphthong.Equals(oldPhoneme))
                            {
                                string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                            }
                        }
                        // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                        affix.f_pronunciation_add = Regex.Replace(affix.f_pronunciation_add, replacementPattern, interimReplacementSymbol);
                        // Put the replaced diphthongs back
                        foreach (string diphthong in diphthongReplacementMap.Keys)
                        {
                            affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                        }
                    }
                    foreach (string interimReplacementSymbol in interimReplacementMap.Keys)
                    {
                        (string oldPhoneme, string newPhoneme) = interimReplacementMap[interimReplacementSymbol];
                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(interimReplacementSymbol, newPhoneme);
                    }
                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                }
            }
#pragma warning restore S1481 // Unused local variables should be removed

            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Update the language loaded into this PhoneticChanger by replacing the oldPhoneme with
        /// the newPhoneme in the lexicon, but only within the matching cluster pattern.  
        /// The phonetic_inventory will also be updated.
        /// </summary>
        /// <param name="clusterPattern"></param>
        /// <param name="oldPhoneme"></param>
        /// <param name="newPhoneme"></param>
        public void ConsonantClusterPhoneticChange(string clusterPattern, string oldPhoneme, string newPhoneme)
        {
            string sourcePattern = clusterPattern;
            string replacementCluster = clusterPattern;
            replacementCluster = replacementCluster.Replace(oldPhoneme, newPhoneme);

            ConsonantClusterPhoneticChange(sourcePattern, replacementCluster);
        }

        /// <summary>
        /// Update the language loaded into this PhoneticChanger by replacing the sourcePattern Cluster with
        /// the replacementCluster in the lexicon, but only within the matching cluster pattern.  
        /// The phonetic_inventory will also be updated.
        /// </summary>
        /// <param name="sourcePattern"></param>
        /// <param name="replacementCluster"></param>
        public void ConsonantClusterPhoneticChange(string sourcePattern, string replacementCluster)
        {
            Cursor.Current = Cursors.WaitCursor;
            // Replace V and C characters in the clusterPattern with captures for vowels or consonants.

            sourcePattern = VowelSourcePatternRegex().Replace(sourcePattern, @"((?:" + IpaUtilities.VowelPattern + IpaUtilities.VowelModifierPattern + @"?)+)");
            sourcePattern = ConsonantSourcePatternRegex().Replace(sourcePattern, @"((?:" + IpaUtilities.ConsonantPattern + IpaUtilities.DiacriticPattern + @"?)+)");

            // Build the pattern for replacement

            StringBuilder replacementClusterBuilder = new();
            int matchCount = 1;

            Match startMatch = VowerConsonantAtStartOfPatternRegex().Match(replacementCluster);
            if (startMatch.Success)
            {
                replacementClusterBuilder.AppendFormat("${0}", matchCount++);
                replacementCluster = startMatch.Groups[1].Value.Trim();
            }

            MatchCollection vowelMatches = VowelConsonantMatchesRegex().Matches(replacementCluster);
            if ((vowelMatches != null) && (vowelMatches.Count > 0))
            {
                for (int i = 0; i < vowelMatches.Count; i++)
                {
                    Match vowelMatch = vowelMatches[i];
                    replacementClusterBuilder.Append(vowelMatch.Groups[1].Value);
                    replacementClusterBuilder.AppendFormat("${0}", matchCount++);
                    if (!string.IsNullOrEmpty(vowelMatch.Groups[2].Value))
                    {
                        replacementClusterBuilder.Append(vowelMatch.Groups[2].Value);
                    }
                }
                replacementCluster = replacementClusterBuilder.ToString();
                Match endMatch = Regex.Match(@"^\s*(.+)[VC]\s*$", replacementCluster);
                if (endMatch.Success)
                {
                    replacementCluster += string.Format("${0}", matchCount);
                }
            }
            else
            {
                replacementClusterBuilder.Append(replacementCluster);
                replacementCluster = replacementClusterBuilder.ToString();
                Match endMatch = Regex.Match(@"^\s*(.+)[VC]\s*$", replacementCluster);
                if (endMatch.Success)
                {
                    replacementCluster += string.Format("${0}", matchCount);
                }
            }

            // Update the Lexicon
            foreach (LexiconEntry word in Language.lexicon)
            {
                // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                LexiconEntry oldVersion = word.copy();
                string oldPhonetic = word.phonetic;

                // Preserve vowel diphthongs before doing the main replacement
                Dictionary<string, string> diphthongReplacementMap = [];
                int ipaReplacementIndex = 0;
                foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                {
                    string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                    diphthongReplacementMap.Add(diphthong, ipaReplacement);
                    word.phonetic = word.phonetic.Replace(diphthong, ipaReplacement);
                }

                word.phonetic = Regex.Replace(word.phonetic, sourcePattern, replacementCluster);

                // Put the replaced diphthongs back
                foreach (string diphthong in diphthongReplacementMap.Keys)
                {
                    word.phonetic = word.phonetic.Replace(diphthongReplacementMap[diphthong], diphthong);
                }
                if (!oldPhonetic.Equals(word.phonetic))
                {
                    string oldSpelled = word.spelled;
                    word.spelled = ConlangUtilities.SpellWord(word.phonetic, Language.spelling_pronunciation_rules);
                    UpdateSampleText(oldSpelled, word.spelled);
                    word.metadata ??= [];
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    phoneticChangeHistories ??= [];
                    MetadataWasher.CleanLexiconEntryMetadata(oldVersion, true);
                    PhoneticChangeHistory pch = new()
                    {
                        OldPhoneme = sourcePattern,
                        NewPhoneme = replacementCluster,
                        OldVersion = oldVersion
                    };
                    string timestamp = string.Format("{0:yyyyMMddhhmmss.ffff}", DateTime.Now);
                    phoneticChangeHistories.Add(timestamp, pch);
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);
                }
            }

            // Update the phoneme inventory - the next update depends on it for diphthongs (at least)
            for (int i = 0; i < Language.phoneme_inventory.Count; i++)
            {

                if (Language.phoneme_inventory[i].Trim() == sourcePattern.Trim())
                {
                    Language.phoneme_inventory[i] = replacementCluster;
                }
            }

            // Update the Affix Map
            foreach (string partOfSpeech in Language.affix_map.Keys)
            {
                //foreach(List<Dictionary<string, List<Dictionary<string, Affix>>>> byPartOfSpeech in Language.affix_map[partOfSpeech])
                foreach (Dictionary<string, List<Dictionary<string, Affix>>> byPartOfSpeech in Language.affix_map[partOfSpeech])
                {
                    foreach (string affixType in byPartOfSpeech.Keys)
                    {
                        foreach (Dictionary<string, Affix> byAffixType in byPartOfSpeech[affixType])
                        {
                            foreach (string declension in byAffixType.Keys)
                            {
                                Affix affix = byAffixType[declension];

                                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.pronunciation_add = Regex.Replace(affix.pronunciation_add, sourcePattern, replacementCluster);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.pronunciation_regex = Regex.Replace(affix.pronunciation_regex, sourcePattern, replacementCluster);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                }
                                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.t_pronunciation_add = Regex.Replace(affix.t_pronunciation_add, sourcePattern, replacementCluster);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.f_pronunciation_add = Regex.Replace(affix.f_pronunciation_add, sourcePattern, replacementCluster);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                            }
                        }
                    }
                }
            }

            // Update the derivational affix map
            foreach (string derivation in Language.derivational_affix_map.Keys)
            {
                DerivationalAffix affix = Language.derivational_affix_map[derivation];
                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                    }
                    affix.pronunciation_add = Regex.Replace(affix.pronunciation_add, sourcePattern, replacementCluster);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                    }
                    affix.pronunciation_regex = Regex.Replace(affix.pronunciation_regex, sourcePattern, replacementCluster);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                }
                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                    }
                    affix.t_pronunciation_add = Regex.Replace(affix.t_pronunciation_add, sourcePattern, replacementCluster);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                    }
                    affix.f_pronunciation_add = Regex.Replace(affix.f_pronunciation_add, sourcePattern, replacementCluster);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                }
            }

            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Update the language loaded into this PhoneticChanger by replacing the oldPhoneme with the newPhoneme in the lexicon, 
        /// but only outside the matching cluster pattern. The phonetic_inventory will also be updated.
        /// </summary>
        /// <param name="clusterPattern"></param>
        /// <param name="oldPhoneme"></param>
        /// <param name="newPhoneme"></param>
        public void ConsonantClusterExcludePhoneticChange(string clusterPattern, string oldPhoneme, string newPhoneme)
        {
            Cursor.Current = Cursors.WaitCursor;
            // Replace V and C characters in the clusterPattern with captures for vowels or consonants.
            clusterPattern = VowelSourcePatternRegex().Replace(clusterPattern, @"(?:(?:" + IpaUtilities.VowelPattern + IpaUtilities.VowelModifierPattern + @"?))+");
            clusterPattern = ConsonantSourcePatternRegex().Replace(clusterPattern, @"(?:(?:" + IpaUtilities.ConsonantPattern + IpaUtilities.DiacriticPattern + @"?))+");
            clusterPattern = string.Format("({0})", clusterPattern);



            // Process the language similar to the other cases - except for when doing the phoneme replacement.
            // Update the Lexicon
            foreach (LexiconEntry word in Language.lexicon)
            {
                // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                LexiconEntry oldVersion = word.copy();
                string oldPhonetic = word.phonetic;

                // Preserve vowel diphthongs before doing the main replacement
                Dictionary<string, string> diphthongReplacementMap = [];
                int ipaReplacementIndex = 0;
                foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                {
                    string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                    diphthongReplacementMap.Add(diphthong, ipaReplacement);
                    word.phonetic = word.phonetic.Replace(diphthong, ipaReplacement);
                }

                word.phonetic = clusterPatternUpdate(word.phonetic, oldPhoneme, newPhoneme, clusterPattern);

                // Put the replaced diphthongs back
                foreach (string diphthong in diphthongReplacementMap.Keys)
                {
                    word.phonetic = word.phonetic.Replace(diphthongReplacementMap[diphthong], diphthong);
                }
                if (!oldPhonetic.Equals(word.phonetic))
                {
                    string oldSpelled = word.spelled;
                    word.spelled = ConlangUtilities.SpellWord(word.phonetic, Language.spelling_pronunciation_rules);
                    UpdateSampleText(oldSpelled, word.spelled);
                    word.metadata ??= [];
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    phoneticChangeHistories ??= [];
                    MetadataWasher.CleanLexiconEntryMetadata(oldVersion, true);
                    PhoneticChangeHistory pch = new()
                    {
                        OldPhoneme = oldPhoneme,
                        NewPhoneme = newPhoneme,
                        OldVersion = oldVersion
                    };
                    string timestamp = string.Format("{0:yyyyMMddhhmmss.ffff}", DateTime.Now);
                    phoneticChangeHistories.Add(timestamp, pch);
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);
                }
            }

            // Update the phoneme inventory - the next update depends on it for diphthongs (at least)
            for (int i = 0; i < Language.phoneme_inventory.Count; i++)
            {

                if (Language.phoneme_inventory[i].Trim() == oldPhoneme.Trim())
                {
                    Language.phoneme_inventory[i] = newPhoneme;
                }
            }

            // Update the Affix Map
            foreach (string partOfSpeech in Language.affix_map.Keys)
            {
                //foreach(List<Dictionary<string, List<Dictionary<string, Affix>>>> byPartOfSpeech in Language.affix_map[partOfSpeech])
                foreach (Dictionary<string, List<Dictionary<string, Affix>>> byPartOfSpeech in Language.affix_map[partOfSpeech])
                {
                    foreach (string affixType in byPartOfSpeech.Keys)
                    {
                        foreach (Dictionary<string, Affix> byAffixType in byPartOfSpeech[affixType])
                        {
                            foreach (string declension in byAffixType.Keys)
                            {
                                Affix affix = byAffixType[declension];

                                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.pronunciation_add = clusterPatternUpdate(affix.pronunciation_add, oldPhoneme, newPhoneme, clusterPattern);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.pronunciation_regex = clusterPatternUpdate(affix.pronunciation_regex, oldPhoneme, newPhoneme, clusterPattern);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                }
                                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.t_pronunciation_add = clusterPatternUpdate(affix.t_pronunciation_add, oldPhoneme, newPhoneme, clusterPattern);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                                {
                                    // Preserve vowel diphthongs before doing the main replacement
                                    Dictionary<string, string> diphthongReplacementMap = [];
                                    int ipaReplacementIndex = 0;
                                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                                    {
                                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                                    }
                                    affix.f_pronunciation_add = clusterPatternUpdate(affix.f_pronunciation_add, oldPhoneme, newPhoneme, clusterPattern);
                                    foreach (string diphthong in diphthongReplacementMap.Keys)
                                    {
                                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                                    }
                                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                                }
                            }
                        }
                    }
                }
            }

            // Update the derivational affix map
            foreach (string derivation in Language.derivational_affix_map.Keys)
            {
                DerivationalAffix affix = Language.derivational_affix_map[derivation];
                if (!string.IsNullOrEmpty(affix.pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthong, ipaReplacement);
                    }
                    affix.pronunciation_add = clusterPatternUpdate(affix.pronunciation_add, oldPhoneme, newPhoneme, clusterPattern);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.pronunciation_add = affix.pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.spelling_add = ConlangUtilities.SpellWord(affix.pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.pronunciation_regex))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthong, ipaReplacement);
                    }
                    affix.pronunciation_regex = clusterPatternUpdate(affix.pronunciation_regex, oldPhoneme, newPhoneme, clusterPattern);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.pronunciation_regex = affix.pronunciation_regex.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                }
                if (!string.IsNullOrEmpty(affix.t_pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthong, ipaReplacement);
                    }
                    affix.t_pronunciation_add = clusterPatternUpdate(affix.t_pronunciation_add, oldPhoneme, newPhoneme, clusterPattern);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.t_pronunciation_add = affix.t_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.t_spelling_add = ConlangUtilities.SpellWord(affix.t_pronunciation_add, Language.spelling_pronunciation_rules);
                }
                if (!string.IsNullOrEmpty(affix.f_pronunciation_add))
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = [];
                    int ipaReplacementIndex = 0;
                    foreach (string diphthong in Language.phonetic_inventory["v_diphthongs"])
                    {
                        string ipaReplacement = IpaUtilities.Ipa_replacements[ipaReplacementIndex++];
                        diphthongReplacementMap.Add(diphthong, ipaReplacement);
                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthong, ipaReplacement);
                    }
                    affix.f_pronunciation_add = clusterPatternUpdate(affix.f_pronunciation_add, oldPhoneme, newPhoneme, clusterPattern);
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        affix.f_pronunciation_add = affix.f_pronunciation_add.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    affix.f_spelling_add = ConlangUtilities.SpellWord(affix.f_pronunciation_add, Language.spelling_pronunciation_rules);
                }
            }

            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
            Cursor.Current = Cursors.Default;

        }


        /// <summary>
        /// Update the spelling of every word in the lexicon based on the current spelling_pronunciation_rules.
        /// </summary>
        public void UpdateSpelling()
        {
            if (this.Language == null)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            foreach (LexiconEntry word in Language.lexicon)
            {
                string oldSpelling = word.spelled;
                string newSpelling = ConlangUtilities.SpellWord(word.phonetic, Language.spelling_pronunciation_rules);
                LexiconEntry oldVersion = word.copy();
                if (!oldSpelling.Equals(newSpelling))
                {
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    phoneticChangeHistories ??= [];
                    PhoneticChangeHistory pch = new()
                    {
                        OldPhoneme = "n/a",
                        NewPhoneme = "n/a",
                        OldVersion = oldVersion
                    };
                    string timestamp = string.Format("{0:yyyyMMddhhmmss.ffff}", DateTime.Now);
                    phoneticChangeHistories.Add(timestamp, pch);
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);

                    word.spelled = newSpelling;
                    if (!string.IsNullOrEmpty(SampleText))
                    {
                        UpdateSampleText(oldSpelling, word.spelled);
                    }
                }
            }
            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Update the pronunciation of every word in the lexicon based on the current spelling_pronunciation_rules.
        /// </summary>
        public void UpdatePronunciation()
        {
            if (this.Language == null)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            foreach (LexiconEntry word in Language.lexicon)
            {
                string oldPhonetic = word.phonetic;
                string newPhonetic = ConlangUtilities.SoundOutWord(word.spelled, Language.spelling_pronunciation_rules);
                LexiconEntry oldVersion = word.copy();
                if (!oldPhonetic.Equals(newPhonetic))
                {
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    phoneticChangeHistories ??= [];
                    PhoneticChangeHistory pch = new()
                    {
                        OldPhoneme = "n/a",
                        NewPhoneme = "n/a",
                        OldVersion = oldVersion
                    };
                    string timestamp = string.Format("{0:yyyyMMddhhmmss.ffff}", DateTime.Now);
                    phoneticChangeHistories.Add(timestamp, pch);
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);

                    word.phonetic = newPhonetic;
                }
            }
            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Reverts the most recent phonetic change made in the current Language.  
        /// This works off of the LexiconEntry metadata, so even changes made in
        /// prior sessions can be reverted.  It also removes the history, so reverted
        /// changes cannot be reapplied.
        /// </summary>
        public void RevertMostRecentChange()
        {
            Cursor.Current = Cursors.WaitCursor;
            Dictionary<double, string> changeHistoryKeyMap = [];
            foreach (LexiconEntry lexiconEntry in Language.lexicon)
            {
                lexiconEntry.metadata ??= [];
                Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                if (lexiconEntry.metadata.ContainsKey("PhoneticChangeHistory"))
                {
                    phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(lexiconEntry.metadata["PhoneticChangeHistory"]);
                }
                phoneticChangeHistories ??= [];
                foreach (string key in phoneticChangeHistories.Keys)
                {
                    double keyValue = double.Parse(key);
                    _ = changeHistoryKeyMap.TryAdd(keyValue, key);
                }
            }
            if (changeHistoryKeyMap.Count <= 0)
            {
                return;
            }
            List<double> keyList = [.. changeHistoryKeyMap.Keys];
            keyList.Sort();
            keyList.Reverse();
            // The first entry in keyList should now be the double precision representation of the most recent key.
            string mostRecentKey = changeHistoryKeyMap[keyList[0]];

            // Having (finally) found the most recent key via this convoluted method, now go through the lexicon again, and revert any changes that have it.
            SortedSet<LexiconEntry> newLexicon = [];
            newLexicon.Clear();
            foreach (LexiconEntry lexiconEntry in Language.lexicon)
            {
                lexiconEntry.metadata ??= [];
                Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                if (lexiconEntry.metadata.ContainsKey("PhoneticChangeHistory"))
                {
                    phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(lexiconEntry.metadata["PhoneticChangeHistory"]);
                }
                phoneticChangeHistories ??= [];
                if (phoneticChangeHistories.TryGetValue(mostRecentKey, out PhoneticChangeHistory value))
                {
                    LexiconEntry oldVersion = value.OldVersion;
                    newLexicon.Add(oldVersion);
                    if (!string.IsNullOrEmpty(SampleText))
                    {
                        string oldSpelled = lexiconEntry.spelled;
                        string newSpelled = oldVersion.spelled;
                        string wordPattern = @"(\s+)" + oldSpelled + @"([.,?!]?\s+)";
                        SampleText = Regex.Replace(SampleText, wordPattern, "$1" + newSpelled + "$2");
                        wordPattern = "^" + oldSpelled + @"([.,?!]?\s+)";
                        SampleText = Regex.Replace(SampleText, wordPattern, newSpelled + "$1");
                        wordPattern = @"(\s+)" + oldSpelled + "$";
                        SampleText = Regex.Replace(SampleText, wordPattern, "$1" + newSpelled);
                    }
                }
                else
                {
                    newLexicon.Add(lexiconEntry);
                }
            }
            Language.lexicon = newLexicon;
            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Get a list of sound map entries that are used to replace an "r" in a spelled
        /// word with a non-rhotacized vowel and no consonant that sounds like an "r" to 
        /// at least a U.S. English speaker.  Sound map entries that will take a pronunciation
        /// without an "r" like consonant and put an "r" into the spelled version will also be
        /// listed.
        /// </summary>
        /// <param name="soundMapList">List of spelling/pronunciation rules to be searched.</param>
        /// <returns>List of rules that create non-rhotacized vowels, or add "r" to the spelled word</returns>
        public static List<SpellingPronunciationRules> GetRAddingSoundMapEntries(List<SpellingPronunciationRules> soundMapList)
        {
            List<SpellingPronunciationRules> rAddingEntries = [];

            // Build the pattern to look for "r" phonemes and rhoticity.
            StringBuilder patternBuilder = new();
            _ = patternBuilder.Append("[ɚ˞");
            foreach (string phoneme in IpaUtilities.RPhonemes)
            {
                _ = patternBuilder.Append(phoneme);
            }
            _ = patternBuilder.Append(']');
            Regex rPresentRegex = new(patternBuilder.ToString().Trim(), RegexOptions.Compiled);

            foreach (SpellingPronunciationRules soundMap in soundMapList)
            {
                if (((soundMap.romanization.Contains('r')) && !rPresentRegex.IsMatch(soundMap.spelling_regex)) ||
                    ((soundMap.pronunciation_regex.Contains('r')) && !rPresentRegex.IsMatch(soundMap.phoneme)))
                {
                    rAddingEntries.Add(soundMap);
                }
            }
            return rAddingEntries;
        }

        /// <summary>
        /// Update the Spelling and Pronunciation rules based.
        /// </summary>
        /// <param name="oldEntry">Entry to be updated</param>
        /// <param name="newEntry">Updated Entry </param>
        /// <param name="spellingPronunciationRuleList">Rule list to be updated.</param>
        public static void ReplaceSpellingPronunciationRuleEntry(SpellingPronunciationRules oldEntry,
            SpellingPronunciationRules newEntry,
            List<SpellingPronunciationRules> spellingPronunciationRuleList)
        {
            for (int i = 0; i < spellingPronunciationRuleList.Count; i++)
            {
                if (spellingPronunciationRuleList[i] == oldEntry)
                {
                    spellingPronunciationRuleList[i] = newEntry;
                    break;
                }
            }
        }

        private void UpdateSampleText(string oldSpelled, string newSpelled)
        {
            Regex wordMatcher = WordPatternRegex();
            StringBuilder sampleTextBuilder = new();

            using (StringReader sampleTextReader = new(SampleText))
            {
                string? line;
                do
                {
                    line = sampleTextReader.ReadLine();
                    if ((line != null) && (!line.Trim().Equals(string.Empty)))
                    {
                        foreach (string wordIterator in line.Trim().Split())
                        {
                            string word = wordIterator.Trim();
                            bool firstCharUpper = char.IsUpper(word[0]);
                            word = word.ToLower();
                            string punctuation = string.Empty;
                            Match wordMatch = wordMatcher.Match(word);
                            if (wordMatch.Success)
                            {
                                word = wordMatch.Groups[1].Value;
                                punctuation = wordMatch.Groups[2].Value;
                            }
                            if (word.Equals(oldSpelled))
                            {
                                if (firstCharUpper)
                                {
                                    sampleTextBuilder.AppendFormat("{0}{1} ", StringExtensions.FirstCharToUpper(newSpelled), punctuation);
                                }
                                else
                                {
                                    sampleTextBuilder.AppendFormat("{0}{1} ", newSpelled, punctuation);
                                }
                            }
                            else
                            {
                                if (firstCharUpper)
                                {
                                    sampleTextBuilder.AppendFormat("{0}{1} ", StringExtensions.FirstCharToUpper(word), punctuation);
                                }
                                else
                                {
                                    sampleTextBuilder.AppendFormat("{0}{1} ", word, punctuation);
                                }

                            }
                        }
                    }
                }
                while (line != null);
            }

            SampleText = sampleTextBuilder.ToString().Trim();
        }

        private static string clusterPatternUpdate(string source, string oldPhoneme, string newPhoneme, string cluster)
        {
            StringBuilder sb = new();
            string workingString = source;
            Match startMatch = Regex.Match(source, string.Format(@"^\s*{0}(\w+)\s*$", workingString));
            if (startMatch.Success)
            {
                sb.Append(startMatch.Groups[1].Value);
                workingString = startMatch.Groups[2].Value;
            }

            Regex clusterRegex = new Regex(string.Format(@"(\w+?){0}", cluster), RegexOptions.Compiled);
            MatchCollection matches = clusterRegex.Matches(workingString);
            if ((matches != null) && (matches.Count > 0))
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    Match match = matches[i];
                    sb.Append(match.Groups[1].Value.Replace(oldPhoneme, newPhoneme));
                    if (!string.IsNullOrEmpty(match.Groups[2].Value))
                    {
                        sb.Append(match.Groups[2].Value);
                    }
                }
                Match extraStringMatch = Regex.Match(workingString, string.Format(@"^.*{0}(\w*?)\s*$", cluster));
                if ((extraStringMatch.Success) && (!extraStringMatch.Groups[2].Value.Contains(cluster)))
                {
                    sb.Append(extraStringMatch.Groups[2].Value.Replace(oldPhoneme, newPhoneme));
                }
            }
            else
            {
                sb.Append(workingString.Replace(oldPhoneme, newPhoneme));
            }

            return sb.ToString();
        }

        private sealed class StringTupleLengthComp : IComparer<(string, string)>
        {
            private static int Compare((string, string) x, (string, string) y)
            {
                (string x1, _) = x;
                (string y1, _) = y;
                return x1.Length.CompareTo(y1.Length);
            }

            int IComparer<(string, string)>.Compare((string, string) x, (string, string) y)
            {
                return Compare(x, y);
            }
        }

        /// <summary>
        /// Encapsulates the history of a phonetic change.
        /// </summary>
        public struct PhoneticChangeHistory
        {
            /// <summary>
            /// Indicates the phoneme that was replaced.
            /// </summary>
            public string OldPhoneme { get; set; }
            /// <summary>
            /// Indicates the phoneme that was used in the replacement.
            /// </summary>
            public string NewPhoneme { get; set; }
            /// <summary>
            /// Lexicon entry prior to this update.
            /// </summary>
            public LexiconEntry OldVersion { get; set; }
        }

        [GeneratedRegex(@"(\w+)([.,?!])", RegexOptions.Compiled)]
        private static partial Regex WordPatternRegex();
        [GeneratedRegex(@"V")]
        private static partial Regex VowelSourcePatternRegex();
        [GeneratedRegex(@"C")]
        private static partial Regex ConsonantSourcePatternRegex();
        [GeneratedRegex(@"^\s*?[VC](\w+)\s*?$")]
        private static partial Regex VowerConsonantAtStartOfPatternRegex();
        [GeneratedRegex(@"([^VC\s]+)[VC]([^VC\s]*?)")]
        private static partial Regex VowelConsonantMatchesRegex();
    }

    /// <summary>
    /// String extensions - currently only FirstCharToUpper
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Make the first character upper case
        /// </summary>
        /// <param name="input">String to convert</param>
        /// <returns>String with the first character of each word in upper case.</returns>
        /// <exception cref="ArgumentNullException">If a null string is passed in.</exception>
        /// <exception cref="ArgumentException">If an empty string is passed in.</exception>
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
    }
}
