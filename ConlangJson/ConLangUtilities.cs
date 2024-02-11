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
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ConlangJson
{
    /// <summary>
    /// A collection of static utility methods used with the ConlangJson objects.
    /// </summary>
    public static class ConLangUtilities
    {
        /// <summary>
        /// Spell a word - Convert its phonetic representation into a Romanized or Latinized
        /// representation.
        /// </summary>
        /// <param name="phonetic">Phonetic representation of a word using IPA.</param>
        /// <param name="soundMapList">The SoundMapList (sound_map_list) from the Language.  
        /// This must be ordered so that processing from the first to the last element will 
        /// produce the correct spelling.</param>
        /// <returns>The Romanized or Latinized version of the word.</returns>
        public static string SpellWord(string phonetic, List<SoundMap> soundMapList)
        {
            string spelled = phonetic;
            foreach (SoundMap soundMap in soundMapList)
            {
                spelled = Regex.Replace(spelled, soundMap.spelling_regex, soundMap.romanization);
            }

            return spelled;
        }

        /// <summary>
        /// Sound out a word - Convert its Romanized or Latinized form into a phonetic
        /// form in IPA.
        /// </summary>
        /// <param name="word">The Romanized or Latinized version of the word.</param>
        /// <param name="soundMapList">The SoundMapList (sound_map_list) from the Language.  
        /// This must be ordered so that processing from the last to the first element will 
        /// produce the correct phonetic representation.</param>
        /// <returns>The phonetic representation of the word in IPA.</returns>
        public static string SoundOutWord(string word, List<SoundMap> soundMapList)
        {
            string phonetic = word;
            foreach (SoundMap soundMap in soundMapList)
            {
                phonetic = Regex.Replace(phonetic, soundMap.pronounciation_regex, soundMap.phoneme);
            }

            return phonetic;
        }

        /// <summary>
        /// Decline a single word
        /// </summary>
        /// <param name="word">LexiconEntry for the word to be declined</param>
        /// <param name="affixMap">AffixMap (affix_map) from the language containing the word 
        /// to be declined</param>
        /// <param name="soundMapList">The SoundMapList (sound_map_list) from the Language.  
        /// This must be ordered so that processing from the first to the last element will 
        /// produce the correct spelling, and processing in the reverse order will produce
        /// the correct pronunciation.</param>
        /// <param name="derivedWord">Optional.  Set to true to indicate that the word being
        /// declined is a derived word.</param>
        /// <returns>A List of LexiconEntry objects containing the new words created by 
        /// declining the word parameter according to the supplied AffixMap's rules.</returns>
        public static List<LexiconEntry> DeclineWord(LexiconEntry word, Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>> affixMap, 
            List<SoundMap> soundMapList, bool derivedWord = false)
        {
            // Safety check - never decline a word already marked as declined, or a word with
            // a source metadata entry
            if ((word.declined_word != null) && ((bool)word.declined_word))
            {
                return new List<LexiconEntry>();
            }
            if(word.metadata != null)
            {
                if(word.metadata.ContainsKey("Source"))
                {
                    return new List<LexiconEntry>();
                }
            }

            List<NewWordData> phoneticList = new List<NewWordData>();

            string phonetic = word.phonetic;
            string partOfSpeech = word.part_of_speech;
            string english =  word.english;
            if (word.derived_word != null)
            {
                derivedWord = (bool)word.derived_word;
            }
            LexiconEntry wordSourceData = word.copy();

            // Search the affixMap for a matching part of speech.  If one is found then
            // there are rules for declining this part of speech, so apply them to this word,
            // using its phonetic representation.
            if (affixMap.Keys.Contains(partOfSpeech))
            {
                List<Dictionary<string, List<Dictionary<string, Affix>>>> affixMapList = affixMap[partOfSpeech];
                affixMapList.Sort(AffixListComparison);
                phoneticList.AddRange(ProcessAffixListLayer(affixMapList, phonetic, partOfSpeech));
            }

            // Build the pronunciation lexicon entries
            List<LexiconEntry> lexiconFragment = new List<LexiconEntry>();

            foreach(NewWordData phoneticEntry in phoneticList)
            {
                string spelled = SpellWord(phoneticEntry.Phonetic, soundMapList);
                JsonObject newMetadata;
                if (word.metadata != null)
                {
                    string metadataString = JsonSerializer.Serialize<JsonObject>(word.metadata);
#pragma warning disable CS8600 // Possible null reference assignment.
                    newMetadata = JsonSerializer.Deserialize<JsonObject>(metadataString);
#pragma warning restore CS8600 // Possible null reference assignment.
                }
                else
                {
                    newMetadata = new JsonObject();
                }
                JsonObject declinedWordData = new JsonObject();
                string declinedWordDataString = JsonSerializer.Serialize<LexiconEntry>(wordSourceData);
                declinedWordData.Add("declined_word", JsonSerializer.Deserialize<JsonObject>(declinedWordDataString));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (newMetadata.ContainsKey("Source"))
                {
                    newMetadata.Remove("Source");
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                newMetadata.Add("Source", declinedWordData);
                LexiconEntry entry = new LexiconEntry(phoneticEntry.Phonetic, spelled,english,phoneticEntry.PartOfSpeech,phoneticEntry.Declensions,derivedWord,true,newMetadata);
                entry.declined_word = true;
                lexiconFragment.Add(entry);
            }

            return lexiconFragment;
        }

        /// <summary>
        /// Decline the complete Lexicon of the supplied language.
        /// </summary>
        /// <param name="language">Language to be declined.</param>
        public static void declineLexicon(LanguageDescription language)
        {
            List<LexiconEntry> addLexicon = new List<LexiconEntry>();
            foreach(LexiconEntry word in language.lexicon)
            {
                addLexicon.AddRange(DeclineWord(word, language.affix_map, language.sound_map_list));
            }
            language.lexicon.AddRange(addLexicon);
            List<LexiconEntry> cleanLexicon = deDuplicateLexicon(language.lexicon);
            language.declined = true;
        }

        /// <summary>
        /// Remove all of the entries in the supplied language's lexicon that were created
        /// as the result of a programmatic declension of the language.  
        /// </summary>
        /// <param name="language">Language to have its declined words removed from the lexicon</param>
        public static void removeDeclinedEntries(LanguageDescription language) 
        {
            List<LexiconEntry> cleanLexicon = new List<LexiconEntry>();
            foreach( LexiconEntry word in language.lexicon )
            {
                if(word.declined_word == null)
                {
                    cleanLexicon.Add(word);
                }
                else if(!(bool)word.declined_word)
                {
                    cleanLexicon.Add(word);
                }
            }
            language.lexicon = cleanLexicon;
            language.lexicon.Sort(new LexiconEntry.LexicalOrderCompSpelling());
            language.declined = false;
        }

        /// <summary>
        /// Remove any duplicate entries in the supplied Lexicon.<br/>This method is slow.
        /// </summary>
        /// <param name="lexicon">List of LexiconEntry objects.</param>
        /// <returns>A list of LexiconEntry objects with any duplicates removed.</returns>
        public static List<LexiconEntry> deDuplicateLexicon(List<LexiconEntry> lexicon)
        {
            List<LexiconEntry> newLexicon = new List<LexiconEntry>();
            foreach(LexiconEntry lexiconEntry in lexicon) 
            { 
                if(!newLexicon.Contains(lexiconEntry))
                {
                    newLexicon.Add(lexiconEntry);
                }
            }
            return newLexicon;
        }

        private struct NewWordData : IEquatable<NewWordData>
        {
            public string NewWord;
            public List<string> Declensions;
            public string PartOfSpeech;
            public string Phonetic;

            public override bool Equals([NotNullWhen(true)] object? obj)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException();
                }
                if (obj is NewWordData)
                {
                    NewWordData nwObj = (NewWordData)obj;
                    bool comp = this.NewWord.Equals(nwObj.NewWord) &&
                        this.PartOfSpeech.Equals(nwObj.PartOfSpeech) &&
                        this.Phonetic.Equals(nwObj.Phonetic);
                    comp &= (sameContent(this.Declensions, nwObj.Declensions));
                    return comp;
                }
                else
                {
                    throw new ArgumentException("Invalid Argument in NewWordData.Equals");
                }
            }

            public override int GetHashCode()
            {
                return Phonetic.GetHashCode() & NewWord.GetHashCode() & PartOfSpeech.GetHashCode();
            }

            public override string? ToString()
            {
                return NewWord;
            }

            bool IEquatable<NewWordData>.Equals(NewWordData other)
            {
                bool comp = this.NewWord.Equals(other.NewWord) &&
                    this.PartOfSpeech.Equals(other.PartOfSpeech) &&
                    this.Phonetic.Equals(other.Phonetic);
                comp &= (sameContent(this.Declensions, other.Declensions));
                return comp;
            }
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

            // Remove the emphasis mark off the beginning of the phonetic string.
            string phonetic2;
            if (phonetic.Substring(0, 1).Equals("ˈ"))
            {
                phonetic2 = phonetic.Substring(1, phonetic.Length - 1);
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
                // Perform the substitution if there is a regular expression in the affix rule
                if (rules.pronounciation_regex != null)
                {
                    if (affix.Equals("prefix"))
                    {
                        if (Regex.IsMatch(phonetic, rules.pronounciation_regex))
                        {
                            newWord = rules.t_pronounciation_add + phonetic2;
                        }
                        else
                        {
                            newWord = rules.f_pronounciation_add + phonetic2;
                        }
                    }
                    else if (affix.Equals("suffix"))
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
                    else if ((affix.Equals("replacement")) && (rules.pronounciation_repl != null))
                    {
                        newWord = Regex.Replace(phonetic, rules.pronounciation_regex, rules.pronounciation_repl);
                    }
                }
                else if (rules.pronounciation_add != null)
                {
                    if (affix.Equals("prefix"))
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
                    List<string> declensions = priorDeclensions.GetRange(0, priorDeclensions.Count);
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

        /**
         * This function is part of the declension process, and is used to process 
         * a single layer of the affix map list.
         */
        private static List<NewWordData> ProcessAffixListLayer(List<Dictionary<string, List<Dictionary<string, Affix>>>> affixMapList, string phonetic, string partOfSpeech)
        {
            List<NewWordData> phoneticList = new List<NewWordData>();

            List<List<Dictionary<string, List<Dictionary<string, Affix>>>>> affixMapCombos = allCombinations(affixMapList);
            foreach (List<Dictionary<string, List<Dictionary<string, Affix>>>> affixMapTuple in affixMapCombos)
            {
                phoneticList.AddRange(ProcessAffixMapTuple(affixMapTuple, phonetic, partOfSpeech));
            }

            phoneticList = deDuplicatePhoneticList(phoneticList);

            return phoneticList;
        }


        /*
         * This function is part of the declension process, and is used to process
         * a single layer of the affix map list.
         */
        private static List<List<T>> allCombinations<T>(List<T> list)
        {
            List<List<T>> allCombos = new List<List<T>>();
            for (int i = 0; i < list.Count; i++)
            {
                List<List<T>> combos = combinations(list, i + 1);
                allCombos.AddRange(combos);
            }
            return allCombos;
        }

        private static List<NewWordData> deDuplicatePhoneticList(List<NewWordData> phoneticList)
        {
            List<NewWordData> newPhoneticList = new List<NewWordData>();
            foreach (NewWordData entry in phoneticList)
            {
                if (!newPhoneticList.Contains(entry))
                {
                    newPhoneticList.Add(entry);
                }
            }

            return newPhoneticList;
        }

        private static List<List<T>> combinations<T>(List<T> list, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            List<List<T>> permutationList = permutations<T>(list, count);
            List<List<T>> combos = new List<List<T>>();
            foreach (List<T> permutation in permutationList)
            {
                bool newCombo = true;
                foreach (List<T> combo in combos)
                {
                    if (sameContent(combo, permutation))
                    {
                        newCombo = false;
                    }
                }
                if (newCombo)
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

        private static List<List<T>> permutations<T>(List<T> list, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            List<List<T>> permutationList = new List<List<T>>();

            if (count == 1)
            {
                foreach (T t in list)
                {
                    List<T> combo = new List<T>() { t };
                    permutationList.Add(combo);
                }
            }
            else
            {
                List<List<T>> partialCombos = permutations<T>(list, count - 1);
                foreach (List<T> combo in partialCombos)
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
            if (one.Count != two.Count)
            {
                return false;
            }
            // Assumption: neither list will have duplicate entries - safe for our needs here.
            int matchCount = 0;
            foreach (T t in one)
            {
                if (two.Contains(t))
                {
                    matchCount += 1;
                    continue;
                }
            }
            return matchCount == one.Count;
        }

        private static int AffixListComparison(Dictionary<string, List<Dictionary<string, Affix>>> x, Dictionary<string, List<Dictionary<string, Affix>>> y)
        {
            string xKey = x.Keys.First();
            string yKey = y.Keys.First();
            return xKey.CompareTo(yKey);
        }
    }
}
