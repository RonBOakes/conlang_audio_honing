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
using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    internal class PhoneticChanger
    {
        private LanguageDescription _language;
        private string _sampleText;

        public PhoneticChanger(LanguageDescription languageDescription, string sampleText)
        {
            this._language = languageDescription;
            this._sampleText = sampleText;
        }
        public PhoneticChanger()
        {
            this._language = new LanguageDescription();
            this._sampleText = string.Empty;
        }

        public LanguageDescription Language
        {
            get => this._language;
            set => this._language = value;
        }
        public string SampleText
        {
            get => this._sampleText;
            set => this._sampleText = value;
        }

        public void PhoneticChange(string oldPhoneme, string newPhoneme)
        {
            if (this._language == null)
            {
                return;
            }

            SoundMapListEditor soundMapListEditor = new SoundMapListEditor();
            List<SoundMap> soundMapList = Language.sound_map_list.GetRange(0, Language.sound_map_list.Count);
            soundMapListEditor.SoundMapList = Language.sound_map_list;
            soundMapListEditor.PhonemeReplacementPairs.Add((oldPhoneme, newPhoneme));
            soundMapListEditor.UpdatePhonemeReplacements();
            soundMapListEditor.ShowDialog();
            // ShowDialog is modal
            if (soundMapListEditor.SoundMapSaved)
            {
                Language.sound_map_list = soundMapListEditor.SoundMapList;
            }

            bool spellingChange = false;
            foreach (SoundMap soundMap in Language.sound_map_list)
            {
                if ((soundMap.spelling_regex.Contains(newPhoneme)) || (soundMap.phoneme.Contains(newPhoneme)))
                {
                    spellingChange = true;
                    break;
                }
            }

            string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";

            // Update the Lexicon
            foreach (LexiconEntry word in Language.lexicon)
            {
                // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                LexiconEntry oldVersion = word.copy();
                string oldPhonetic = word.phonetic;

                // Preserve vowel diphthongs before doing the main replacement
                Dictionary<string, string> diphthongReplacementMap = new Dictionary<string, string>();
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
                    if (spellingChange)
                    {
                        string oldSpelled = word.spelled;
                        word.spelled = ConLangUtilities.SpellWord(word.phonetic, Language.sound_map_list);
                        string wordPattern = @"(\s+)" + oldSpelled + @"([.,?!]?\s+)";
                        SampleText = Regex.Replace(SampleText, wordPattern, "$1" + word.spelled + "$2");
                        wordPattern = "^" + oldSpelled + @"([.,?!]?\s+)";
                        SampleText = Regex.Replace(SampleText, wordPattern, word.spelled + "$1");
                        wordPattern = @"(\s+)" + oldSpelled + "$";
                        SampleText = Regex.Replace(SampleText, wordPattern, "$1" + word.spelled);
                    }
                    if (word.metadata == null)
                    {
                        word.metadata = new System.Text.Json.Nodes.JsonObject();
                    }
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    if (phoneticChangeHistories == null)
                    {
                        phoneticChangeHistories = new Dictionary<string, PhoneticChangeHistory>();
                    }
                    PhoneticChangeHistory pch = new PhoneticChangeHistory();
                    pch.OldPhoneme = oldPhoneme;
                    pch.NewPhoneme = newPhoneme;
                    pch.OldVersion = oldVersion;
                    string timestamp = string.Format("{0:yyyyMMdd.hhmmss.ffff}", DateTime.Now);
                    phoneticChangeHistories.Add(timestamp, pch);
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);
                }

            }

            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
        }

        public void PhoneticChange(List<(string oldPhoneme, string newPhoneme)> changeList)
        {
            if (this._language == null)
            {
                return;
            }

            // The limit is 10 changes in a list due to how the replacements are done
            if (changeList.Count > 10)
            {
                return;
            }

            // Sort the list by size and then reverse it so that the longest 
            changeList.Sort(new stringTupleLengthComp());
            changeList.Reverse();

            SoundMapListEditor soundMapListEditor = new SoundMapListEditor();
            List<SoundMap> soundMapList = Language.sound_map_list.GetRange(0, Language.sound_map_list.Count);
            soundMapListEditor.SoundMapList = Language.sound_map_list;
            soundMapListEditor.PhonemeReplacementPairs.AddRange(changeList);
            soundMapListEditor.UpdatePhonemeReplacements();
            soundMapListEditor.ShowDialog();
            // ShowDialog is modal
            if (soundMapListEditor.SoundMapSaved)
            {
                Language.sound_map_list = soundMapListEditor.SoundMapList;
            }

            string changeHistoryTimestamp = string.Format("{0:yyyyMMdd.hhmmssfffffff}", DateTime.Now);


            Dictionary<string, (string oldPhoneme, string newPhoneme)> interimReplacementMap = new Dictionary<string, (string oldPhoneme, string newPhoneme)>();

            int interimReplacementSymbolInt = 0;
            foreach ((string oldPhoneme, string newPhoneme) in changeList)
            {
                string interimReplacementSymbol = string.Format("{0}", interimReplacementSymbolInt++);
                interimReplacementMap.Add(interimReplacementSymbol, (oldPhoneme, newPhoneme));
                string replacementPattern = oldPhoneme + @"(?!" + IpaUtilities.DiacriticPattern + @")";
                // Update the Lexicon phase 1
                foreach (LexiconEntry word in Language.lexicon)
                {
                    // Preserve vowel diphthongs before doing the main replacement
                    Dictionary<string, string> diphthongReplacementMap = new Dictionary<string, string>();
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
                    LexiconEntry oldVersion = word.copy();
                    word.phonetic = Regex.Replace(word.phonetic, replacementPattern, interimReplacementSymbol);
                    // Put the replaced diphthongs back
                    foreach (string diphthong in diphthongReplacementMap.Keys)
                    {
                        word.phonetic = word.phonetic.Replace(diphthongReplacementMap[diphthong], diphthong);
                    }
                    if (word.metadata == null)
                    {
                        word.metadata = new System.Text.Json.Nodes.JsonObject();
                    }
                    Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    if (phoneticChangeHistories == null)
                    {
                        phoneticChangeHistories = new Dictionary<string, PhoneticChangeHistory>();
                    }
                    if (!phoneticChangeHistories.ContainsKey(changeHistoryTimestamp))
                    {
                        PhoneticChangeHistory pch = new PhoneticChangeHistory();
                        pch.OldPhoneme = oldPhoneme;
                        pch.NewPhoneme = newPhoneme;
                        pch.OldVersion = oldVersion;
                        phoneticChangeHistories.Add(changeHistoryTimestamp, pch);
                    }
                    string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);

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
                    oldVersion.phonetic.Replace(interimReplacementSymbol, oldPhoneme2);
                    word.phonetic = word.phonetic.Replace(interimReplacementSymbol, newPhoneme2);
                }
                if (!oldVersion.phonetic.Equals(word.phonetic))
                {
                    string oldSpelled = word.spelled;
                    word.spelled = ConLangUtilities.SpellWord(word.phonetic, Language.sound_map_list);
                    string wordPattern = @"(\s+)" + oldSpelled + @"([.,?!]?\s+)";
                    SampleText = Regex.Replace(SampleText, wordPattern, "$1"+word.spelled+"$2");
                    wordPattern = "^" + oldSpelled + @"([.,?!]?\s+)";
                    SampleText = Regex.Replace(SampleText, wordPattern, word.spelled + "$1");
                    wordPattern = @"(\s+)" + oldSpelled + "$";
                    SampleText = Regex.Replace(SampleText, wordPattern, "$1" + word.spelled);
                }
            }
            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
        }

        private class stringTupleLengthComp : IComparer<(string,string)>
        {
            private int Compare((string, string) x, (string, string) y)
            {
                (string x1, string x2) = x;
                (string y1, string y2) = y;
                return x1.Length.CompareTo(y1.Length);
            }

            int IComparer<(string, string)>.Compare((string, string) x, (string, string) y)
            {
                return Compare(x,y);
            }
        }

        public struct PhoneticChangeHistory
        {
            private string _oldPhoneme;
            private string _newPhoneme;
            private LexiconEntry _oldVersion;

            public string OldPhoneme
            {
                get => this._oldPhoneme;
                set => this._oldPhoneme = value;
            }
            public string NewPhoneme
            {
                get => this._newPhoneme;
                set => this._newPhoneme = value;
            }
            public LexiconEntry OldVersion
            {
                get => this._oldVersion;
                set => this._oldVersion = value;
            }
        }
    }
}
