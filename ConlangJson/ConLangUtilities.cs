/*
 * Class containing utility functions for working with the C# version of the
 * Conlang structure.
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.AccessControl;

namespace ConlangJson
{
    public static class ConLangUtilities
    {
        public static string SpellWord(string phonetic, List<SoundMap>soundMapList)
        {
            string spelled = phonetic;
            foreach(SoundMap soundMap in soundMapList)
            {
                spelled = Regex.Replace(spelled, soundMap.spelling_regex, soundMap.romanization);
            }

            return spelled;
        }

        public static string SoundOutWord(string word,  List<SoundMap>soundMapList) 
        {
            string phonetic = word;
            foreach( SoundMap soundMap in soundMapList) 
            {
                phonetic = Regex.Replace(phonetic, soundMap.pronounciation_regex, soundMap.phoneme);
            }

            return phonetic;
        }

        private struct NewWordData
        {
            public string NewWord;
            public List<string> Declensions;
            public string PartOfSpeech;
            public string Phonetic;
        }

        /**
         * This function is part of the priorDeclensions process, and is used to process 
         * the affix_map_tuple generated during declining a word based on its
         * part of speech.
         */
        private static List<NewWordData> ProcessAffixMapTuple(List<Dictionary<string, List<Dictionary<string, Affix>>>> affixMapTupple, string phonetic, string partOfSpeech, List<string>? priorDeclensions = null)
        {
            List<NewWordData> phoneticList = new List<NewWordData>();
            if (affixMapTupple.Count == 0)
            {
                return phoneticList;
            }

            if (priorDeclensions == null)
            {
                priorDeclensions = new List<string>();
            }

            Dictionary<string, List<Dictionary<string, Affix>>> affixMap = affixMapTupple[0];
            // affixMap will have only one entry, the key is the affix type, the value is the list of rules.
            string affix = affixMap.Keys.First();

            // If the affix is a particle, then just return the empty phonetic list
            if (affix.Equals("particle"))
            {
                return phoneticList;
            }

            // Remove the emphisys mark off the beginning of the phonetic string.
            string phonetic2;
            if(phonetic.Substring(0,1).Equals("ˈ"))
            {
                phonetic2 = phonetic.Substring(1,phonetic.Length - 1);
            }
            else
            {
                phonetic2 = phonetic;
            }

            foreach (Dictionary<string, Affix> entry in affixMap[affix])
            {
                // At this point, the entry should be a dictionary with one key and an Affix as its value
                string declension = entry.Keys.First();
                Affix rules = entry[declension];

                string? newWord = null;
                // Perform the subsitution if there is a regular expression in the affix rule
                if(rules.pronounciation_regex != null)
                {
                    if(affix.Equals("prefix"))
                    {
                        if(Regex.IsMatch(phonetic, rules.pronounciation_regex))
                        {
                            newWord = rules.t_pronounciation_add + phonetic2;
                        }
                        else
                        {
                            newWord = rules.f_pronounciation_add + phonetic2;
                        }
                    }
                    else if(affix.Equals("suffix"))
                    {
                        if (Regex.IsMatch(phonetic, rules.pronounciation_regex))
                        {
                            newWord = phonetic2 + rules.t_pronounciation_add;
                        }
                        else
                        {
                            newWord = phonetic2 + rules.f_pronounciation_add;
                        }
                    }
                    else if((affix.Equals("replacement")) && (rules.pronounciation_repl != null))
                    {
                        newWord = Regex.Replace(phonetic, rules.pronounciation_regex, rules.pronounciation_repl);
                    }
                }
                else if (rules.pronounciation_add != null)
                {
                    if(affix.Equals("prefix"))
                    {
                        newWord = rules.pronounciation_add + phonetic2;
                    }
                    else
                    {
                        newWord = phonetic2 + rules.pronounciation_add;
                    }
                }
                else
                {
                    newWord = phonetic;
                }
                // Recurse (yes, we are in the for loop but this is where we recurse)
                if (newWord != null)
                {
                    List<Dictionary<string, List<Dictionary<string, Affix>>>> nextMapTuple = affixMapTupple.GetRange(1, affixMapTupple.Count - 1);
                    List<string> declensions = priorDeclensions.GetRange(0,priorDeclensions.Count);
                    declensions.Add(declension);
                    phoneticList.AddRange(ProcessAffixMapTuple(nextMapTuple, newWord, partOfSpeech, declensions));
                    NewWordData newWordData = new NewWordData();
                    newWordData.NewWord = newWord;
                    newWordData.Declensions = declensions;
                    newWordData.PartOfSpeech = partOfSpeech;
                    newWordData.Phonetic = phonetic;
                    phoneticList.Add(newWordData);
                }
            }

            return phoneticList;
        }

        public static List<List<T>> allCombinations<T>(List<T> list)
        {
            List<List<T>> allCombos = new List<List<T>>();
            for (int i = 0; i <  list.Count; i++)
            {
                List<List<T>> combos = combinations(list, i + 1);
                allCombos.AddRange(combos);
            }
            return allCombos;
        }

        public static List<List<T>> combinations<T>(List<T> list, int count)
        {
            if(list == null)
            {
                throw new ArgumentNullException("list");
            }
            List<List<T>> permutationList = permutations<T>(list, count);
            List<List<T>> combos = new List<List<T>>();
            foreach (List<T> permutation in permutationList)
            {
                bool newCombo = true;
                foreach(List<T> combo in combos)
                {
                    if(sameContent(combo, permutation))
                    {
                        newCombo = false;
                    }
                }
                if(newCombo)
                {
                    combos.Add(permutation);
                }
            }
            foreach (List<T> combo in combos)
            {
                combo.Reverse();
            }
            return combos;
        }

        public static List<List<T>> permutations<T>(List<T> list, int count)
        {
            if(list == null)
            {
                throw new ArgumentNullException("list");
            }
            List<List<T>> permutationList = new List<List<T>>();

            if (count == 1)
            {
                foreach(T t in list)
                {
                    List<T> combo = new List<T>() { t };
                    permutationList.Add(combo);
                }
            }
            else
            {
                List<List<T>> partialCombos = permutations<T>(list, count - 1);
                foreach(List<T> combo in partialCombos)
                {
                    foreach (T t in list)
                    {
                        if (!(combo.Contains(t)))
                        {
                            List<T> newCombo = new List<T>();
                            newCombo.Add(t);
                            newCombo.AddRange(combo);
                            permutationList.Add(newCombo);
                        }
                    }
                }
            }
            return permutationList;
        }

        private static bool sameContent<T>(List<T> one, List<T> two)
        {
            if(one.Count != two.Count)
            {
                return false;
            }
            // Asumption: neither list will have duplicate entries - safe for our needs here.
            int matchCount = 0;
            foreach(T t in one)
            {
                if(two.Contains(t))
                {
                    matchCount += 1;
                    continue;
                }
            }
            return matchCount == one.Count;
        }
    }
}
