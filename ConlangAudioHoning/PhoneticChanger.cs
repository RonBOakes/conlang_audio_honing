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
using System.Text;
using System.Text.Json;
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

            // TODO: Update spelling/pronunciation rules (a.k.a. sound map list)
            bool spellingChange = false;
            foreach (SoundMap soundMap in Language.sound_map_list)
            {
                if ((soundMap.spelling_regex.Contains(newPhoneme)) || (soundMap.phoneme.Contains(newPhoneme)))
                {
                    spellingChange = true;
                    break;
                }
            }

            // Update the Lexicon
            foreach (LexiconEntry word in Language.lexicon)
            {
                // replace all occurrences of oldPhoneme in phonetic with newPhoneme
                string oldPhonetic = word.phonetic;
                word.phonetic = word.phonetic.Replace(oldPhoneme, newPhoneme);
                if (!oldPhonetic.Equals(word.phonetic))
                {
                    if (spellingChange)
                    {
                        string oldSpelled = word.spelled;
                        word.spelled = ConLangUtilities.SpellWord(word.phonetic, Language.sound_map_list);
                        SampleText = SampleText.Replace(oldSpelled, word.spelled);
                    }
                    if (word.metadata == null)
                    {
                        word.metadata = new System.Text.Json.Nodes.JsonObject();
                    }
                    List<PhoneticChangeHistory>? phoneticChangeHistories = null;
                    if (word.metadata.ContainsKey("PhoneticChangeHistory"))
                    {
                        phoneticChangeHistories = JsonSerializer.Deserialize<List<PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
                    }
                    if (phoneticChangeHistories == null)
                    {
                        phoneticChangeHistories = new List<PhoneticChangeHistory>();
                    }
                    PhoneticChangeHistory pch = new PhoneticChangeHistory();
                    pch.OldPhoneme = oldPhoneme;
                    pch.NewPhoneme = newPhoneme;
                    pch.ChangeTime = DateTime.Now;
                    phoneticChangeHistories.Add(pch);
                    word.metadata["PhoneticChangeHistory"] = JsonSerializer.Serialize<List<PhoneticChangeHistory>>(phoneticChangeHistories);
                }

            }

            // Update the phonetic inventory
            IpaUtilities.BuildPhoneticInventory(Language);
        }


        public struct PhoneticChangeHistory
        {
            private string _oldPhoneme;
            private string _newPhoneme;
            private DateTime _changeTime;

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
            public DateTime ChangeTime
            {
                get => this._changeTime;
                set => this._changeTime = value;
            }
        }
    }
}
