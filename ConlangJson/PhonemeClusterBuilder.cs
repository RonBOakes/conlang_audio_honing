using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConlangJson
{
    /// <summary>
    /// Class that contains the static method for building Phoneme Clusters within a language description, as well as the 
    /// private structures used for this purpose.
    /// </summary>
    public class PhonemeClusterBuilder
    {
        private readonly LanguageDescription languageDescription;

        private readonly Regex CsVMatch;
        private readonly Regex VCsVMatch;
        private readonly Regex VCsMatch;

        private PhonemeClusterBuilder(LanguageDescription languageDescription)
        {
            this.languageDescription = languageDescription;
            // At this point, languageDescription is assumed to have a current phonetic_inventory.
            string vowelPatternFragment = buildVowelPatternFragment();
            string consonantPatternFragment = buildConsonantPatternFragment();

            CsVMatch = new Regex(string.Format(@"^\s*(({0}){1}).*$", consonantPatternFragment, vowelPatternFragment), RegexOptions.Compiled);
            VCsVMatch = new Regex(string.Format(@"(?=({1}({0}){1}))", consonantPatternFragment, vowelPatternFragment), RegexOptions.Compiled);
            VCsMatch = new Regex(string.Format(@"^.*({1}({0}))\s*$", consonantPatternFragment, vowelPatternFragment), RegexOptions.Compiled);
        }

        /// <summary>
        /// Rebuilds the phoneme_cluster section of the supplied LanguageDescription object based on its current lexicon.  
        /// </summary>
        /// <param name="languageDescription">LanguageDescription object that will have its phoneme_cluster rebuilt</param>
        /// <param name="rebuildPhoneticInventory">Optional: rebuild the phonetic_inventory before rebuilding the phoneme_cluster.
        /// Set to "true" if this is not currently up to date.</param>
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

        /// <summary>
        /// Returns a string containing the NAD clusters for the language passed in, one per line.<br/>
        /// Only clusters of more than two characters are provided, since the NAD calculator only produces
        /// results on clusters of that size.
        /// </summary>
        /// <param name="languageDescription">LanguageDescription to be analyzed.  The phoneme_clusters must 
        /// be populated and is presumed to be current.</param>
        /// <returns>A string containing the NAD analysis clusters, one per line.</returns>
        public static string GetNADClusters(LanguageDescription languageDescription)
        {
            if (languageDescription == null)
            {
                return string.Empty;
            }
            if ((languageDescription.phoneme_clusters == null) || (languageDescription.phoneme_clusters.Count < 1))
            {
                return string.Empty;
            }

            SortedSet<String> nadClusters = [];

            foreach (string cluster in languageDescription.phoneme_clusters.Keys)
            {
                string nadCluster = languageDescription.phoneme_clusters[cluster];
                string nadClusterLenCheck = nadCluster.Replace("V", "");
                if (nadClusterLenCheck.Length >= 2)
                {
                    nadClusters.Add(nadCluster);
                }
            }

            StringBuilder nadClusterBuilder = new();
            foreach (string nadCluster in nadClusters)
            {
                nadClusterBuilder.AppendLine(nadCluster);
            }

            return nadClusterBuilder.ToString();
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
            StringBuilder patternFragmentBuilder = new();
            patternFragmentBuilder.Append("(?:");
            foreach (string vowel in languageDescription.phonetic_inventory["v_diphthongs"])
            {
                patternFragmentBuilder.Append(vowel);
                patternFragmentBuilder.Append('|');
            }
            foreach (string vowel in languageDescription.phonetic_inventory["vowels"])
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
            StringBuilder patternFragmentBuilder = new();
            patternFragmentBuilder.Append("(?:");
            foreach (string vowel in languageDescription.phonetic_inventory["p_consonants"])
            {
                patternFragmentBuilder.Append(vowel);
                patternFragmentBuilder.Append('|');
            }
            foreach (string vowel in languageDescription.phonetic_inventory["np_consonants"])
            {
                patternFragmentBuilder.Append(vowel);
                patternFragmentBuilder.Append('|');
            }
            patternFragmentBuilder.Remove(patternFragmentBuilder.Length - 1, 1);
            patternFragmentBuilder.Append(")+");
            return patternFragmentBuilder.ToString();
        }

        private void GetClusters(string stringToParse)
        {
            stringToParse = stringToParse.Replace("ˈ", "");

            MatchCollection CsVMatches = CsVMatch.Matches(stringToParse);
            foreach (Match C in CsVMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[1].Value;
                    StringBuilder nadBuilder = new();
                    nadBuilder.Append(C.Groups[2].Value);
                    nadBuilder.Append('V');
                    string nad = nadBuilder.ToString();
                    languageDescription.phoneme_clusters.TryAdd(cluster, nad);
                    if (!languageDescription.phoneme_cluster_count.ContainsKey(nad))
                    {
                        languageDescription.phoneme_cluster_count[nad] = 0;
                    }
                    languageDescription.phoneme_cluster_count[nad] += 1;
                }
            }
            MatchCollection VCsVMatches = VCsVMatch.Matches(stringToParse);
            foreach (Match C in VCsVMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[1].Value;
                    StringBuilder nadBuilder = new();
                    nadBuilder.Append('V');
                    nadBuilder.Append(C.Groups[2].Value);
                    nadBuilder.Append('V');
                    string nad = nadBuilder.ToString();
                    languageDescription.phoneme_clusters.TryAdd(cluster, nad);
                    if (!languageDescription.phoneme_cluster_count.ContainsKey(nad))
                    {
                        languageDescription.phoneme_cluster_count[nad] = 0;
                    }
                    languageDescription.phoneme_cluster_count[nad] += 1;
                }
            }
            MatchCollection VCsMatches = VCsMatch.Matches(stringToParse);
            foreach (Match C in VCsMatches)
            {
                if (C.Success)
                {
                    string cluster = C.Groups[1].Value;
                    StringBuilder nadBuilder = new();
                    nadBuilder.Append('V');
                    nadBuilder.Append(C.Groups[2].Value);
                    string nad = nadBuilder.ToString();
                    languageDescription.phoneme_clusters.TryAdd(cluster, nad);
                    if (!languageDescription.phoneme_cluster_count.ContainsKey(nad))
                    {
                        languageDescription.phoneme_cluster_count[nad] = 0;
                    }
                    languageDescription.phoneme_cluster_count[nad] += 1;
                }
            }
        }
    }
}
