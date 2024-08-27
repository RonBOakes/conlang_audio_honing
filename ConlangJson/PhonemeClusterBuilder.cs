using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConlangJson
{
    internal class PhonemeClusterBuilder
    {
        private readonly LanguageDescription languageDescription;

        private readonly Regex CsVMatch;
        private readonly Regex CsVvMatch;
        private readonly Regex CsVCsMatch;
        private readonly Regex CsVvCsMatch;
        private readonly Regex VCsMatch;
        private readonly Regex VvCsMatch;
        private readonly Regex VVMatch;
        private readonly Regex VvVMatch;
        private readonly Regex VVvMatch;

        private PhonemeClusterBuilder(LanguageDescription languageDescription)
        {
            this.languageDescription = languageDescription;
            // At this point, languageDescription is assumed to have a current phonetic_inventory.
            string vowelPatternFragment = buildVowelPatternFragment();
            string consonantPatternFragment = buildConsonantPatternFragment();
            string vowelDiphthongPatternFragment = buildVowelDiphthongPatternFragment();

            CsVMatch = new Regex(string.Format("{0}+{1}", consonantPatternFragment, vowelPatternFragment), RegexOptions.Compiled);
            CsVvMatch = new Regex(string.Format("{0}+{1}", consonantPatternFragment, vowelDiphthongPatternFragment), RegexOptions.Compiled);
            CsVCsMatch = new Regex(string.Format("{0}+{1}{0}+", consonantPatternFragment, vowelPatternFragment), RegexOptions.Compiled);
            CsVvCsMatch = new Regex(string.Format("{0}+{1}{0}+", consonantPatternFragment, vowelDiphthongPatternFragment), RegexOptions.Compiled);
            VCsMatch = new Regex(string.Format("{0}{1}+", vowelPatternFragment, consonantPatternFragment), RegexOptions.Compiled);
            VvCsMatch = new Regex(string.Format("{0}{1}+", vowelDiphthongPatternFragment, consonantPatternFragment), RegexOptions.Compiled);
            VVMatch = new Regex(string.Format("{0}{{2}}", vowelPatternFragment), RegexOptions.Compiled);
            VvVMatch = new Regex(string.Format("{0}{1}", vowelPatternFragment, vowelDiphthongPatternFragment), RegexOptions.Compiled);
            VVvMatch = new Regex(string.Format("{0}{1}", vowelDiphthongPatternFragment, vowelPatternFragment), RegexOptions.Compiled);
        }

        public static void RebuildPhonemeClusters(LanguageDescription languageDescription, bool rebuildPhoneticInventory = false)
        {
            if (languageDescription == null)
            {
                return;
            }

            if (!languageDescription.phonetic_characters.Equals("ipa"))
            {
                return;
            }

            if (rebuildPhoneticInventory)
            {
                IpaUtilities.BuildPhoneticInventory(languageDescription);
            }

            // Remove any existing content in the phoneme_cluster dictionary.
            languageDescription.phoneme_clusters.Clear();

            PhonemeClusterBuilder clusterBuilder = new(languageDescription);
            clusterBuilder.BuildPhonemeClusters();

        }

        private void BuildPhonemeClusters()
        {
            bool removeDerived = false;
            bool removeDeclined = false;
            if (!languageDescription.derived)
            {
                ConlangUtilities.DeriveLexicon(languageDescription);
                removeDerived = true;
            }
            if (!languageDescription.declined)
            {
                ConlangUtilities.DeclineLexicon(languageDescription);
                removeDeclined = true;
            }

            foreach (LexiconEntry word in languageDescription.lexicon)
            {
                GetClusters(word.phonetic);
            }

            if (removeDeclined)
            {
                ConlangUtilities.RemoveDeclinedEntries(languageDescription);
            }
            if (removeDerived)
            {
                ConlangUtilities.RemoveDerivedEntries(languageDescription);
            }
        }

        private string buildVowelPatternFragment()
        {
            HashSet<char> vowels = [];
            HashSet<char> diacritics = [];
            foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
            {
                // Vowel should be length 1 or length 2
                vowels.Add(vowel[0]);
                if (vowel.Length > 1)
                {
                    diacritics.Add(vowel[1]);
                }
            }

            StringBuilder patternFragmentBuilder = new();
            patternFragmentBuilder.Append("([");
            foreach (char vowel in vowels)
            {
                patternFragmentBuilder.Append(vowel);
            }
            patternFragmentBuilder.Append("][");
            foreach (char diacritic in diacritics)
            {
                patternFragmentBuilder.Append(diacritic);
            }
            patternFragmentBuilder.Append("]?)");

            return patternFragmentBuilder.ToString();
        }

        private string buildVowelDiphthongPatternFragment()
        {
            StringBuilder patternFragmentBuilder = new();
            patternFragmentBuilder.Append('(');
            foreach (string vowel in languageDescription.phonetic_inventory["v_diphthongs"])
            {
                patternFragmentBuilder.Append(vowel);
                patternFragmentBuilder.Append('|');
            }
            patternFragmentBuilder.Remove(patternFragmentBuilder.Length - 1, 1);
            patternFragmentBuilder.Append(')');
            return patternFragmentBuilder.ToString();
        }

        private string buildConsonantPatternFragment()
        {
            HashSet<char> consonant = [];
            HashSet<char> diacritics = [];
            foreach (string vowel in languageDescription.phonetic_inventory["p_consonants"])
            {
                // Vowel should be length 1 or length 2
                consonant.Add(vowel[0]);
                if (vowel.Length > 1)
                {
                    diacritics.Add(vowel[1]);
                }
            }
            foreach (string vowel in languageDescription.phonetic_inventory["np_consonants"])
            {
                // Vowel should be length 1 or length 2
                consonant.Add(vowel[0]);
                if (vowel.Length > 1)
                {
                    diacritics.Add(vowel[1]);
                }
            }

            StringBuilder patternFragmentBuilder = new();
            patternFragmentBuilder.Append("([");
            foreach (char vowel in consonant)
            {
                patternFragmentBuilder.Append(vowel);
            }
            patternFragmentBuilder.Append("][");
            foreach (char diacritic in diacritics)
            {
                patternFragmentBuilder.Append(diacritic);
            }
            patternFragmentBuilder.Append("]?)");

            return patternFragmentBuilder.ToString();
        }

        private void GetClusters(string stringToParse)
        {

            MatchCollection CsVMatches = CsVMatch.Matches(stringToParse);
            foreach (Match C in CsVMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "(C)V"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection CsVvMatches = CsVvMatch.Matches(stringToParse);
            foreach (Match C in CsVvMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "(C)Vv"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection CsVCsMatches = CsVCsMatch.Matches(stringToParse);
            foreach (Match C in CsVCsMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "(C)V(C)"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection CsVvCsMatches = CsVvCsMatch.Matches(stringToParse);
            foreach (Match C in CsVvCsMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "(C)Vv(C)"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection VCsMatches = VCsMatch.Matches(stringToParse);
            foreach (Match C in VCsMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "V(C)"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection VvCsMatches = VvCsMatch.Matches(stringToParse);
            foreach (Match C in VvCsMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "Vv(C)"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection VVMatches = VVMatch.Matches(stringToParse);
            foreach (Match C in VVMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "VV"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection VvVMatches = VvVMatch.Matches(stringToParse);
            foreach (Match C in VvVMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "VvV"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
            MatchCollection VVvMatches = VVvMatch.Matches(stringToParse);
            foreach (Match C in VVvMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[0].Value;
                    languageDescription.phoneme_clusters.TryAdd(cluster, "VVv"); // TODO: provide better descriptions
                    stringToParse = stringToParse.Replace(cluster, "*");
                }
            }
        }
    }
}
