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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Linq;

namespace ConlangJson
{
    /// <summary>
    /// A collection of static utility methods used with the ConlangJson objects.
    /// </summary>
    public static partial class ConlangUtilities
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
                if (!string.IsNullOrEmpty(soundMap.spelling_regex))
                {
                    spelled = Regex.Replace(spelled, soundMap.spelling_regex, soundMap.romanization);
                }
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
                if (!string.IsNullOrEmpty(soundMap.pronunciation_regex))
                {
                    phonetic = Regex.Replace(phonetic, soundMap.pronunciation_regex, soundMap.phoneme);
                }
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
                return [];
            }
            if (word.metadata != null && word.metadata.ContainsKey("Source"))
            {
                return [];
            }

            List<NewWordData> phoneticList = [];

            string phonetic = word.phonetic;
            string partOfSpeech = word.part_of_speech;
            string english = word.english;
            if (word.derived_word != null)
            {
                derivedWord = (bool)word.derived_word;
            }
            LexiconEntry wordSourceData = word.copy();

            // Search the affixMap for a matching part of speech.  If one is found then
            // there are rules for declining this part of speech, so apply them to this word,
            // using its phonetic representation.
            if (affixMap.TryGetValue(partOfSpeech, out List<Dictionary<string, List<Dictionary<string, Affix>>>>? value))
            {
                List<Dictionary<string, List<Dictionary<string, Affix>>>> affixMapList = value;
                affixMapList.Sort(AffixListComparison);
                phoneticList.AddRange(ProcessAffixListLayer(affixMapList, phonetic, partOfSpeech));
            }

            // Build the pronunciation lexicon entries
            List<LexiconEntry> lexiconFragment = [];

            foreach (NewWordData phoneticEntry in phoneticList)
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
                    newMetadata = [];
                }
                JsonObject declinedWordData = [];
                string declinedWordDataString = JsonSerializer.Serialize<LexiconEntry>(wordSourceData);
                declinedWordData.Add("declined_word", JsonSerializer.Deserialize<JsonObject>(declinedWordDataString));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (newMetadata.ContainsKey("Source"))
                {
                    _ = newMetadata.Remove("Source");
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                newMetadata.Add("Source", declinedWordData);
                LexiconEntry entry = new(phoneticEntry.Phonetic, spelled, english, phoneticEntry.PartOfSpeech, phoneticEntry.Declensions, derivedWord, true, newMetadata)
                {
                    declined_word = true
                };
                lexiconFragment.Add(entry);
            }

            return lexiconFragment;
        }

        /// <summary>
        /// Decline the complete Lexicon of the supplied language.
        /// </summary>
        /// <param name="language">Language to be declined.</param>
        public static void DeclineLexicon(LanguageDescription language)
        {
            List<LexiconEntry> addLexicon = [];
            foreach (LexiconEntry word in language.lexicon)
            {
                addLexicon.AddRange(DeclineWord(word, language.affix_map, language.sound_map_list));
            }
            language.lexicon.AddRange(addLexicon);
            language.declined = true;
        }

        /// <summary>
        /// Derive the words contained in the language
        /// </summary>
        /// <param name="language">Language needing words derived</param>
        public static bool DeriveLexicon(LanguageDescription language)
        {
            bool ok = false;
#pragma warning disable IDE0028 // Simplify collection initialization
            Dictionary<string, LexiconEntry> wordMap = new();
            Dictionary<(string, string), LexiconEntry> wordMapTuple = new();
            List<LexiconEntry> lexiconFragment = new();
#pragma warning restore IDE0028 // Simplify collection initialization

            // Build the word map and word map tuple from the lexicon in the language
            for (int i = 0; i < language.lexicon.Count; i++)
            {
                LexiconEntry entry = language.lexicon[i];
                // If the word is a root word, then put it in the word maps
                // In order to match with words in the derived_word_list, spaces need to be 
                // replaced with underscores.
                if (entry.declensions.Contains("root"))
                {
                    string wmEnglish = entry.english.Replace(' ', '_');
                    _ = wordMap.TryAdd(wmEnglish, entry);
                    string partOfSpeech = entry.part_of_speech;
                    if (partOfSpeech.StartsWith('n'))
                    {
                        partOfSpeech = "n";
                    }
                    _ = wordMapTuple.TryAdd((wmEnglish, partOfSpeech), entry);
                }
            }

            // Iterate over the derived_word_list
            foreach (string words in language.derived_word_list)
            {
                // Partition the word into parts based on the Vulgarlang format.
                string[] parts1 = words.Split("=");
                string[] parts2 = parts1[0].Split(":");
                string english = parts2[0].Trim();
                string partOfSpeech = parts2[1].Trim();
                string ruleText = parts1[1].Trim();
                // The following rule splits Vulgarlang compound words by putting a space.
                // This does not impact affix rules due to the letter case involved.
                ruleText = ConvertVulgarlangCompoundWordRegex().Replace(ruleText, @"$1 $2");
                string[] ruleList = ruleText.Split();

                StringBuilder phonetic = new();

                foreach (string ruleListEntry in ruleList)
                {
                    string rule = ruleListEntry;
                    DerivationalAffix? ruleAffixData = null;

                    if (rule.Contains('-'))
                    {
                        string[] ruleSplit = rule.Split('-');
                        ruleAffixData = language.derivational_affix_map[ruleSplit[1].Trim()];
                        rule = ruleSplit[0].Trim();
                    }
                    string rulePartOfSpeech = "";
                    if (rule.Contains(':'))
                    {
                        string[] ruleSplit = rule.Split(':');
                        rulePartOfSpeech = ruleSplit[1].Trim();
                        rule = ruleSplit[0].Trim();
                        // Turn all gendered nouns into "n"
                        if (rulePartOfSpeech != "num")
                        {
                            rulePartOfSpeech = RemoveNounGenderRegex().Replace(rulePartOfSpeech, "n");
                        }
                    }

                    // Look for the root word.
                    string word = rule;
                    LexiconEntry? lexEntry = null;
                    // If we are looking to match a specific part of speech
                    if (!string.IsNullOrEmpty(rulePartOfSpeech))
                    {
                        if (!wordMapTuple.ContainsKey((word, rulePartOfSpeech)))
                        {
                            // Searching for matching entries that start and use the first one.
                            bool found = false;
                            foreach ((string, string) pair in wordMapTuple.Keys)
                            {
                                if (pair.Item1.StartsWith(word) && pair.Item2.Equals(rulePartOfSpeech))
                                {
                                    lexEntry = wordMapTuple[pair];
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            lexEntry = wordMapTuple[(word, rulePartOfSpeech)];
                        }
                    }
                    else if (!wordMap.TryGetValue(word, out LexiconEntry? value))
                    {
                        // Searching for matching entries that start and use the first one
                        bool found = false;
                        foreach (var searchWord in from string searchWord in wordMap.Keys
                                                   where searchWord.StartsWith(word)
                                                   select searchWord)
                        {
                            lexEntry = wordMap[searchWord];
                            found = true;
                        }

                        if (!found)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        lexEntry = value;
                    }

                    if (lexEntry == null)
                    {
                        return false;
                    }

                    // build the derived word's phonetic representation
                    string phoneticPart = lexEntry.phonetic;
                    if (ruleAffixData != null)
                    {
                        if (!string.IsNullOrEmpty(ruleAffixData.pronunciation_regex))
                        {
                            if (Regex.IsMatch(ruleAffixData.pronunciation_regex, phoneticPart))
                            {
                                if (ruleAffixData.type == "PREFIX")
                                {
                                    phoneticPart = ruleAffixData.t_pronunciation_add + phoneticPart;
                                }
                                else
                                {
                                    phoneticPart += ruleAffixData.t_pronunciation_add;
                                }
                            }
                            else
                            {
                                if (ruleAffixData.type == "PREFIX")
                                {
                                    phoneticPart = ruleAffixData.f_pronunciation_add + phoneticPart;
                                }
                                else
                                {
                                    phoneticPart += ruleAffixData.f_pronunciation_add;
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(ruleAffixData.pronunciation_add))
                        {
                            if (ruleAffixData.type == "PREFIX")
                            {
                                phoneticPart = ruleAffixData.pronunciation_add + phoneticPart;
                            }
                            else
                            {
                                phoneticPart += ruleAffixData.pronunciation_add;
                            }

                        }
                    }
                    phonetic.Append(phoneticPart);
                }

                string spelled = SpellWord(phonetic.ToString(), language.sound_map_list);
                Dictionary<string, Dictionary<string, string>> metadataDict = new() { { "source", new Dictionary<string, string> { { "derived_word", words } } } };
                JsonObject? metadata = JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize<Dictionary<string, Dictionary<string, string>>>(metadataDict));


                // build all of the LexiconEntries for this word.  Each English word or definition gets its own entry
                foreach (string eng1 in english.Split(','))
                {
                    string eng = eng1.Trim();
#pragma warning disable IDE0028 // Simplify collection initialization
                    LexiconEntry entry = new(phonetic: phonetic.ToString(),
                        spelled: spelled,
                        english: eng,
                        part_of_speech: partOfSpeech,
                        declensions: new List<string>() { "root" },
                        derived_word: true,
                        declined_word: false,
                        metadata: metadata);
#pragma warning restore IDE0028 // Simplify collection initialization
                    string wm_english = eng.Replace(' ', '_');
                    _ = wordMap.TryAdd(wm_english, entry);
                    _ = wordMapTuple.TryAdd((wm_english, partOfSpeech), entry);
                    lexiconFragment.Add(entry);
                }
            }

            // Add the lexicon fragment into the existing lexicon
            language.lexicon.AddRange(lexiconFragment);
            language.derived = true;
            ok = true;

            return ok;
        }

        /// <summary>
        /// Remove all of the entries in the supplied language's lexicon that were created
        /// as the result of a programmatic declension of the language.  
        /// </summary>
        /// <param name="language">Language to have its declined words removed from the lexicon</param>
        public static void RemoveDeclinedEntries(LanguageDescription language)
        {
            List<LexiconEntry> cleanLexicon = [];
            foreach (LexiconEntry word in language.lexicon)
            {
                if (word.declined_word == null)
                {
                    cleanLexicon.Add(word);
                }
                else if (!(bool)word.declined_word)
                {
                    cleanLexicon.Add(word);
                }
            }
            language.lexicon = cleanLexicon;
            language.lexicon.Sort(new LexiconEntry.LexicalOrderCompSpelling());
            language.declined = false;
        }


        /// <summary>
        /// Remove all of the entries in the supplied language's lexicon that were created
        /// as the result of a programmatic derivation of the language.
        /// </summary>
        /// <param name="language">Language to have its derived words removed from the lexicon.</param>
        public static void RemoveDerivedEntries(LanguageDescription language)
        {
            List<LexiconEntry> cleanLexicon = [];
            foreach (LexiconEntry word in language.lexicon)
            {
                if (word.derived_word == null)
                {
                    cleanLexicon.Add(word);
                }
                else if (!(bool)word.derived_word)
                {
                    cleanLexicon.Add(word);
                }
            }
            language.lexicon = cleanLexicon;
            language.lexicon.Sort(new LexiconEntry.LexicalOrderCompSpelling());
            language.derived = false;
        }

        /// <summary>
        /// Remove any duplicate entries in the supplied Lexicon.<br/>This method is slow.
        /// </summary>
        /// <param name="lexicon">List of LexiconEntry objects.</param>
        /// <returns>A list of LexiconEntry objects with any duplicates removed.</returns>
        public static List<LexiconEntry> DeDuplicateLexicon(List<LexiconEntry> lexicon)
        {
            List<LexiconEntry> newLexicon = [];
            foreach (LexiconEntry lexiconEntry in lexicon)
            {
                if (!newLexicon.Contains(lexiconEntry))
                {
                    newLexicon.Add(lexiconEntry);
                }
            }
            return newLexicon;
        }

        private struct NewWordData(string NewWord, List<string> Declensions, string PartOfSpeech, string Phonetic) : IEquatable<NewWordData>
        {
            public string NewWord { get; private set; } = NewWord;
            public List<string> Declensions { get; private set; } = Declensions;
            public string PartOfSpeech { get; private set; } = PartOfSpeech;
            public string Phonetic { get; private set; } = Phonetic;

            public override readonly bool Equals([NotNullWhen(true)] object? obj)
            {
                if (obj is NewWordData nwObj)
                {
                    bool comp = this.NewWord.Equals(nwObj.NewWord) &&
                        this.PartOfSpeech.Equals(nwObj.PartOfSpeech) &&
                        this.Phonetic.Equals(nwObj.Phonetic);
                    comp &= SameContent(this.Declensions, nwObj.Declensions);
                    return comp;
                }
                else
                {
                    return false;
                }
            }

            public override readonly int GetHashCode()
            {
                return Phonetic.GetHashCode() & NewWord.GetHashCode() & PartOfSpeech.GetHashCode();
            }

            public override readonly string? ToString()
            {
                return NewWord;
            }

            readonly bool IEquatable<NewWordData>.Equals(NewWordData other)
            {
                bool comp = this.NewWord.Equals(other.NewWord) &&
                    this.PartOfSpeech.Equals(other.PartOfSpeech) &&
                    this.Phonetic.Equals(other.Phonetic);
                comp &= SameContent(this.Declensions, other.Declensions);
                return comp;
            }
        }

        /**
         * This function is part of the priorDeclensions process, and is used to process 
         * the affix_map_tuple generated during declining a word based on its
         * part of speech.
         */
        private static List<NewWordData> ProcessAffixMapTuple(List<Dictionary<string, List<Dictionary<string, Affix>>>> affixMapTuple, string phonetic, string partOfSpeech, List<string>? priorDeclensions = null)
        {
            List<NewWordData> phoneticList = [];
            if (affixMapTuple.Count == 0)
            {
                return phoneticList;
            }

            priorDeclensions ??= [];

            Dictionary<string, List<Dictionary<string, Affix>>> affixMap = affixMapTuple[0];
            // affixMap will have only one entry, the key is the affix type, the value is the list of rules.
            string affix = affixMap.Keys.First();

            // If the affix is a particle, then just return the empty phonetic list
            if (affix.Equals("particle"))
            {
                return phoneticList;
            }

            // Remove the emphasis mark off the beginning of the phonetic string.
            string phonetic2;
            if (phonetic[..1].Equals("ˈ"))
            {
                phonetic2 = phonetic[1..];
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
                if (rules.pronunciation_regex != null)
                {
                    if (affix.Equals("prefix"))
                    {
                        if (Regex.IsMatch(phonetic, rules.pronunciation_regex))
                        {
                            newWord = rules.t_pronunciation_add + phonetic2;
                        }
                        else
                        {
                            newWord = rules.f_pronunciation_add + phonetic2;
                        }
                    }
                    else if (affix.Equals("suffix"))
                    {
                        if (Regex.IsMatch(phonetic, rules.pronunciation_regex))
                        {
                            newWord = phonetic2 + rules.t_pronunciation_add;
                        }
                        else
                        {
                            newWord = phonetic2 + rules.f_pronunciation_add;
                        }
                    }
                    else if (affix.Equals("replacement") && (rules.pronunciation_replacement != null))
                    {
                        newWord = Regex.Replace(phonetic, rules.pronunciation_regex, rules.pronunciation_replacement);
                    }
                }
                else if (rules.pronunciation_add != null)
                {
                    if (affix.Equals("prefix"))
                    {
                        newWord = rules.pronunciation_add + phonetic2;
                    }
                    else
                    {
                        newWord = phonetic2 + rules.pronunciation_add;
                    }
                }
                else
                {
                    newWord = phonetic;
                }
                // Recurse (yes, we are in the for loop but this is where we recurse)
                if (newWord != null)
                {
                    List<Dictionary<string, List<Dictionary<string, Affix>>>> nextMapTuple = affixMapTuple.GetRange(1, affixMapTuple.Count - 1);
                    List<string> declensions = priorDeclensions.GetRange(0, priorDeclensions.Count);
                    declensions.Add(declension);
                    phoneticList.AddRange(ProcessAffixMapTuple(nextMapTuple, newWord, partOfSpeech, declensions));
                    NewWordData newWordData = new(
                        NewWord: newWord,
                        Declensions: declensions,
                        PartOfSpeech: partOfSpeech,
                        Phonetic: phonetic
                    );
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
            List<NewWordData> phoneticList = [];

            List<List<Dictionary<string, List<Dictionary<string, Affix>>>>> affixMapCombos = AllCombinations(affixMapList);
            foreach (List<Dictionary<string, List<Dictionary<string, Affix>>>> affixMapTuple in affixMapCombos)
            {
                phoneticList.AddRange(ProcessAffixMapTuple(affixMapTuple, phonetic, partOfSpeech));
            }

            phoneticList = DeDuplicatePhoneticList(phoneticList);

            return phoneticList;
        }


        /*
         * This function is part of the declension process, and is used to process
         * a single layer of the affix map list.
         */
        private static List<List<T>> AllCombinations<T>(List<T> list)
        {
            List<List<T>> allCombos = [];
            for (int i = 0; i < list.Count; i++)
            {
                List<List<T>> combos = Combinations(list, i + 1);
                allCombos.AddRange(combos);
            }
            return allCombos;
        }

        private static List<NewWordData> DeDuplicatePhoneticList(List<NewWordData> phoneticList)
        {
            List<NewWordData> newPhoneticList = [];
            foreach (NewWordData entry in phoneticList)
            {
                if (!newPhoneticList.Contains(entry))
                {
                    newPhoneticList.Add(entry);
                }
            }

            return newPhoneticList;
        }

        private static List<List<T>> Combinations<T>(List<T> list, int count)
        {
            ArgumentNullException.ThrowIfNull(list);
            List<List<T>> permutationList = Permutations<T>(list, count);
            List<List<T>> combos = [];
            foreach (List<T> permutation in permutationList)
            {
                bool newCombo = true;
                foreach (var _ in from List<T> combo in combos
                                  where SameContent(combo, permutation)
                                  select new { })
                {
                    newCombo = false;
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

        private static List<List<T>> Permutations<T>(List<T> list, int count)
        {
            ArgumentNullException.ThrowIfNull(list);
            List<List<T>> permutationList = [];

            if (count == 1)
            {
                foreach (T t in list)
                {
                    List<T> combo = [t];
                    permutationList.Add(combo);
                }
            }
            else
            {
                List<List<T>> partialCombos = Permutations<T>(list, count - 1);
                foreach (List<T> combo in partialCombos)
                {
                    foreach (T t in list)
                    {
                        if (!combo.Contains(t))
                        {
                            List<T> newCombo = [t, .. combo];
                            permutationList.Add(newCombo);
                        }
                    }
                }
            }
            return permutationList;
        }

        private static bool SameContent<T>(List<T> one, List<T> two)
        {
            if (one.Count != two.Count)
            {
                return false;
            }
            // Assumption: neither list will have duplicate entries - safe for our needs here.
            int matchCount = 0;
            foreach (var _ in from T t in one
                              where two.Contains(t)
                              select new { })
            {
                matchCount += 1;
            }

            return matchCount == one.Count;
        }

        private static int AffixListComparison(Dictionary<string, List<Dictionary<string, Affix>>> x, Dictionary<string, List<Dictionary<string, Affix>>> y)
        {
            string xKey = x.Keys.First();
            string yKey = y.Keys.First();
            return xKey.CompareTo(yKey);
        }

        [GeneratedRegex(@"([a-z]+)-([a-z]+)")]
        private static partial Regex ConvertVulgarlangCompoundWordRegex();
        [GeneratedRegex(@"\s*\n\w*\s*")]
        private static partial Regex RemoveNounGenderRegex();
    }
}
