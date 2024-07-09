/*
 * Class with IPA reference structures etc.
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
using ConlangJson;

namespace ConlangAudioHoning
{
    /// <summary>
    /// A collection of static properties and methods for working with IPA, 
    /// The International Phonetic Alphabet, specifically using Unicode.
    /// </summary>
    public static class IpaUtilities
    {
        private static readonly string[] _suprasegmentals =
        [
            "\u02d0",
            "\u02d1",
            "\u02c8",
            "\u02cc",
            "\u035c",
            "\u0361"
        ];
        private static readonly Dictionary<string, List<string>> _consonant_changes = new()
        {
            // Plosive
            { "p", new List<string>(){ "b","m","t","d","\u0271", "\u0253" } },
            { "b", new List<string>(){ "p","m","t","d","\u0271", "\u0253" } },
            { "t", new List<string>(){ "p","b","d","\u0288","\u0256","\u0271","n","\u0273"} },
            { "d", new List<string>(){ "p","b","t", "\u0288", "\u0256", "\u0271", "n", "\u0273" } },
            { "\u0288", new List<string>(){ "t","d","\u0256","c","\u025f","n","\u0273","\u0272" } },
            { "\u0256", new List<string>(){ "t","d","\u0256","c","\u025f","n","\u0273","\u0272" } },
            { "c", new List<string>(){ "\u0288","\u0256","\u025f","k","\u0261","\u0273","\u0272","\u014b"} },
            { "\u025f", new List<string>(){ "\u0288", "\u0256", "c", "k", "\u0261", "\u0273", "\u0272", "\u014b" } },
            { "k", new List<string>(){"c","\u025f","\u0261","q","\u0262","\u0272","\u014b","\u0274"} },
            { "\u0261", new List<string>(){"c","\u025f","k","q","\u0262","\u0272","\u014b","\u0274"} },
            { "q", new List<string>(){"k","\u0261","\u0262","\u0294","\u014b","\u0274"} },
            { "\u0262", new List<string>(){"k","\u0261","q","\u0294","\u014b","\u0274"} },
            { "\u0294", new List<string>(){"q","\u0262","\u0274"} },
            // Nasal
            { "m",new List<string>(){"p","b","\u0271","\u0299"} },
            { "\u0271",new List<string>(){"p","b","t","d","m","n","\u0299","r"} },
            { "n", new List<string>(){"\u0271","t","d","\u0288","\u0256","\u0273","r"}},
            { "\u0273", new List<string>(){"n","\u0272","t","d","\u0288","\u0256","c","\u025f","r"} },
            { "\u0272", new List<string>(){"\u0273","\u014b","\u0274","\u0288","\u0256","c","\u025f","k","\u0261","\u0280"} },
            { "\u014b", new List<string>(){"\u0272","\u0274","c","\u025f","k","\u0261","q","\u0262"} },
            { "\u0274", new List<string>(){"\u014b","k","\u0261","q","\u0262","\u0294"} },
            // Trill
            { "\u0299", new List<string>(){"r","m","\u0271","\u2c71" } },
            { "r", new List<string>(){"\u0299","\u0280","\u0271","n","\u0273","\u2c71","\u027e","\u027d"} },
            { "\u0280", new List<string>(){"r","\u014b"} },
            // Tap or Flap
            { "\u2c71", new List<string>(){"\u027e","\u0299","r","\u0278","\u03b2","f","v","\u03b8","\u00f0" } },
            { "\u027e", new List<string>(){"\u2c71","\u027d","r","\u03b8","\u00f0","s","z","\u0283","\u0292"} },
            { "\u027d", new List<string>(){"\u027e","r","\u0283","\u0292","\u0282","\u0290","\u00e7","\u029d"} },
            // Affricate
            { "\u02a6", new List<string>(){"\u02a3", "\u02a7", "\u02a4","n","\u03b8","\u00f0","s","z","\u0283","\u0292", } },
            { "\u02a3", new List<string>(){"\u02a6", "\u02a7", "\u02a4","n","\u03b8","\u00f0","s","z","\u0283","\u0292", } },
            { "\u02a7", new List<string>(){"\u02a4", "\u02a3", "\u02a6","n", "\u0273","s","z","\u0283","\u0292","\u0282","\u0290", } },
            { "\u02a4", new List<string>(){"\u02a7", "\u02a3", "\u02a6","n", "\u0273","s","z","\u0283","\u0292","\u0282","\u0290", } },
            { "\u02a8", new List<string>(){"\u02a5", "\u0273", "\u0283", "\u0292", "\u0255","\u0291", } },
            { "\u02a5", new List<string>(){"\u02a8", "\u0273", "\u0283", "\u0292", "\u0255","\u0291", } },
            // Fricative
            { "\u0278", new List<string>(){"\u03b2","f","v","\u2c71"} },
            { "\u03b2", new List<string>(){"\u0278","f","v","\u2c71"} },
            { "f", new List<string>(){"v","\u0278","\u03b2","\u03b8","\u00f0","\u2c71","\u027e","\u026c","\u026e" } },
            { "v", new List<string>(){"f","\u0278","\u03b2","\u03b8","\u00f0","\u2c71","\u027e", "\u026c", "\u026e" } },
            { "\u03b8", new List<string>(){"\u00f0","f","v","s","z","\u2c71","\u027e", "\u026c", "\u026e" } },
            { "\u00f0", new List<string>(){"\u03b8","f","v","s","z","\u2c71","\u027e", "\u026c", "\u026e" } },
            { "s", new List<string>(){"z","\u03b8","\u00f0","\u0283","\u0292","\u027e","\u027d","\u026c","\u026e"} },
            { "z", new List<string>(){"s","\u03b8","\u00f0","\u0283","\u0292","\u027e","\u027d","\u026c","\u026e"} },
            { "\u0283", new List<string>(){"\u0292","s","z","\u0282","\u0290","\u027e","\u027d","\u026c","\u026e"} },
            { "\u0292", new List<string>(){"\u0283","s","z","\u0282","\u0290","\u027e","\u027d","\u026c","\u026e"} },
            { "\u0282", new List<string>(){"\u0290","\u0283","\u0292","\u00e7","\u029d","\u027e","\u027d" } },
            { "\u0290", new List<string>(){"\u0282","\u0283","\u0292","\u00e7","\u029d", "\u027e", "\u027d" } },
            { "\u0255", new List<string>(){"\u0291","\u0282","\u0290","\u00e7","\u029d","\u02a6","\u02a5","\u026d","j",} },
            { "\u0291", new List<string>(){"\u0255","\u0282","\u0290","\u00e7","\u029d","\u02a6","\u02a5","\u026d","j",} },
            { "\u00e7", new List<string>(){"\u029d","\u0282","\u0290","x","\u0263","\u027d"} },
            { "\u029d", new List<string>(){"\u00e7","\u0282","\u0290","x","\u0263","\u027d"} },
            { "x", new List<string>(){"\u0263","\u00e7","\u029d","\u03c7","\u0281"} },
            { "\u0263", new List<string>(){"x","\u00e7","\u029d","\u03c7","\u0281"} },
            { "\u03c7", new List<string>(){"\u0281","x","\u0263","\u0127","\u0295"} },
            { "\u0281", new List<string>(){"\u03c7","x","\u0263","\u0127","\u0295"} },
            { "\u0127", new List<string>(){"\u0295","\u03c7","\u0281","h","\u0266" } },
            { "\u0295", new List<string>(){"\u0127","\u03c7","\u0281","h","\u0266" } },
            { "h", new List<string>(){"\u0266","\u0127","\u0295"} },
            { "\u0266", new List<string>(){"h","\u0127","\u0295"} },
            // Lateral fricative
            { "\u026c", new List<string>(){"\u026e","\u03b8","\u00f0","s","z","\u0283","\u0292","\u028b","\u0279","\u027b"} },
            { "\u026e", new List<string>(){"\u026c","\u03b8","\u00f0","s","z","\u0283","\u0292","\u028b","\u0279","\u027b"} },
            // approximant
            { "\u028b", new List<string>(){"\u0279","\u026c","\u026e","l"} },
            { "\u0279", new List<string>(){"\u028b","\u027b","\u026c","\u026e","l","\u026d"} },
            { "\u027b", new List<string>(){"\u0279","j","\u026c","\u026e","l","\u026d","\u028e"} },
            { "j", new List<string>(){"\u027b","\u0270","\u026d","\u028e","\u029f","\u028d","w",} },
            { "\u0270", new List<string>(){"j","\u028e","\u029f", "\u028d", "w", } },
            // Lateral approximant
            { "l", new List<string>(){"\u026d","\u028b","\u0279","\u027b"} },
            { "\u026d", new List<string>(){"l","\u028e","\u0279","\u027b","j"} },
            { "\u028e", new List<string>(){"\u026d","\u029f","\u027b","j","\u0270", "\u028d", "w", } },
            { "\u029f", new List<string>(){"\u028e","j","\u0270", "\u028d", "w", } },
            // Clicks
            { "\u0298", new List<string>(){"\u01c0","\u01c3","\u01c2","\u01c1"} },
            { "\u01c0", new List<string>(){"\u0298","\u01c3","\u01c2","\u01c1"} },
            { "\u01c3", new List<string>(){"\u0298","\u01c0","\u01c2","\u01c1"} },
            { "\u01c2", new List<string>(){"\u0298","\u01c0","\u01c3","\u01c1"} },
            { "\u01c1", new List<string>(){"\u0298","\u01c0","\u01c3","\u01c2"} },
            // Voiced Implosives
            { "\u0253", new List<string>(){"b","p","m","\u0257","t","d"} },
            { "\u0257", new List<string>(){"t","d","n","\u0273","\u0253","b","p","\u0288","\u0256" } },
            { "\u0284", new List<string>(){"c", "\u025f", "\u0288", "\u0256", "k", "\u0261","n", "\u0273", "\u0272", } },
            { "\u0260", new List<string>(){"k","\u0261","c","\u025f","q","\u0262", "\u0273", "\u0272", "\u0274", } },
            { "\u029b", new List<string>(){"q","\u0262", "\u0272", "\u0274", } },
            // others
            { "\u028d", new List<string>(){"w","\u028b","\u0279","\u027b","\u006a","\u0270","\u029d","x"} },
            { "w", new List<string>(){ "ʍ", "\u028b","\u0279","\u027b","\u006a","\u0270","\u029d","x"} },
        };

        private static Dictionary<string, List<string>>? _consonant_changes_l2 = null;

        private static Dictionary<string, List<string>>? _consonant_changes_l3 = null;
        private static string? _consonant_pattern = null;
        private static string? _vowel_pattern = null;
        private static string? _diacritic_pattern = null;

        /// <summary>
        /// Returns an array containing strings with all of the identified Pulmonic Consonants.
        /// </summary>
        public static String[] PConsonants { get; } = [
            "b",
            "\u03b2",
            "\u0299",
            "c",
            "\u00e7",
            "d",
            "\u0256",
            "\u1d91",
            "\u02a3",
            "\u02a5",
            "\u02a4",
            "\uab66",
            "f",
            "\u0278",
            "g",
            "\u0262",
            "\u0270",
            "h",
            "\u0266",
            "\u0127",
            "\u0267",
            "j",
            "\u029d",
            "\u025f",
            "k",
            "l",
            "\u026c",
            "\u026e",
            "\u026d",
            "\ua78e",
            "\u029f",
            "m",
            "\u0271",
            "n",
            "\u0273",
            "\u0272",
            "\u014b",
            "\u0274",
            "p",
            "q",
            "r",
            "\u0279",
            "\u027e",
            "\u027d",
            "\u027b",
            "\u027a",
            "\u0281",
            "\u0280",
            "s",
            "\u0282",
            "\u0283",
            "t",
            "\u0288",
            "\u02a6",
            "\u02a8",
            "\u02a7",
            "\uab67",
            "v",
            "\u2c71",
            "\u028b",
            "x",
            "\u0263",
            "\u03c7",
            "\u028e",
            "z",
            "\u0290",
            "\u0292",
            "\u03b8",
            "\u00f0",
            "\u0294",
            "\u0295",
            "ɕ",
            "ʑ",
        ];

        /// <summary>
        /// Returns an array containing strings with all of the identified Nonpulmonic Consonants.
        /// </summary>
        public static string[] NpConsonants { get; } = [
            "\u0253",
            "\u0257",
            "\u0284",
            "\u0260",
            "\u029b",
            "w",
            "\u028d",
            "\u0265",
            "\u02a1",
            "\u02a2",
            "\u0255",
            "\u0291",
            "\u029c",
            "\u0298",
            "\u01c0",
            "\u01c3",
            "\u01c2",
            "\u01c1",
            "\ud837\udf0a"
        ];

        /// <summary>
        /// Returns an array containing strings with all of the vowels.
        /// </summary>
        public static string[] Vowels { get; } = [
            "a",
            "\u00e6",
            "\u0251",
            "\u0252",
            "\u0250",
            "e",
            "\u025b",
            "\u025c",
            "\u025e",
            "\u0259",
            "i",
            "\u0268",
            "\u026a",
            "y",
            "\u028f",
            "\u00f8",
            "\u0258",
            "\u0275",
            "\u0153",
            "\u0276",
            "\u0264",
            "o",
            "\u0254",
            "u",
            "\u0289",
            "\u028a",
            "\u026f",
            "\u028c",
            "\u025a",
        ];

        /// <summary>
        /// Returns an array containing strings with the suprasegmental symbols 
        /// (i.e. stress marks, length marks, and tie bars).
        /// </summary>
        public static string[] Suprasegmentals
        {
            get => _suprasegmentals;
        }

        /// <summary>
        /// Returns an array of strings with the modifiers that can be applied to vowels.
        /// </summary>
        public static string[] Vowel_modifiers { get; } = [
            "\u02d0",
            "\u02d1",
            "\u032f",
        ];

        /// <summary>
        /// Returns an array of strings with the diacritics valid for use with IPA.
        /// </summary>
        public static string[] Diacritics { get; } = [
            "\u02f3",
            "\u0325",
            "\u030a",
            "\u0324",
            "\u032a",
            "\u0329",
            "\u0c3c",
            "\u032c",
            "\u02f7",
            "\u0330",
            "\u02f7",
            "\u0330",
            "\u02fd",
            "\u033a",
            "\u032f",
            "\u02b0",
            "\u033c",
            "\u033b",
            "\u02d2",
            "\u0339",
            "\u20b7",
            "\u0303",
            "\u02b2",
            "\u02d3",
            "\u031c",
            "\u02d6",
            "\u031f",
            "\u207f",
            "\u00a8",
            "\u0308",
            "\u02e0",
            "\u02cd",
            "\u0320",
            "\u20e1",
            "\u02df",
            "\u033d",
            "\u02e4",
            "\uab68",
            "\u0319",
            "\u02de"
        ];

        /// <summary>
        /// Returns an array of strings that represent (Ron Oakes') selection of
        /// phonemes that could be used in place of rhoticity.  
        /// </summary>
        public static string[] RPhonemes { get; } = [
            "ɹ",
            "ɾ",
            "ɺ",
            "ɽ",
            "ɻ",
            "r"
        ];

        /// <summary>
        /// Returns a string that can be used within a generalized regular expression to match
        /// all IPA consonants. This pattern will only match the actual phone/phoneme character, 
        /// not any diacritics or related markings that follow it.
        /// </summary>
        public static string ConsonantPattern
        {
            get
            {
                if (_consonant_pattern == null)
                {
                    StringBuilder sb = new();
                    _ = sb.Append('[');
                    foreach (string consonant in PConsonants)
                    {
                        _ = sb.Append(consonant);
                    }
                    foreach (string consonant in NpConsonants)
                    {
                        _ = sb.Append(consonant);
                    }
                    _ = sb.Append(']');
                    _consonant_pattern = sb.ToString();
                }
                return _consonant_pattern;
            }
        }

        /// <summary>
        /// Returns a string that can be used within a generalized regular expression to match
        /// all IPA vowels. This pattern will only match the actual phone/phoneme character, 
        /// not any diacritics or related markings that follow it.
        /// </summary>
        public static string VowelPattern
        {
            get
            {
                if (_vowel_pattern == null)
                {
                    StringBuilder sb = new();
                    _ = sb.Append('[');
                    foreach (string vowel in Vowels)
                    {
                        _ = sb.Append(vowel);
                    }
                    _ = sb.Append(']');
                    _vowel_pattern = sb.ToString();
                }
                return _vowel_pattern;
            }
        }

        /// <summary>
        /// Returns a string that can be used within a generalized regular expression to match
        /// all IPA Diacritics.
        /// </summary>
        public static string DiacriticPattern
        {
            get
            {
                if (_diacritic_pattern == null)
                {
                    StringBuilder sb = new();
                    _ = sb.Append('[');
                    foreach (string diacritic in Vowel_modifiers)
                    {
                        _ = sb.Append(diacritic);
                    }
                    foreach (string diacritic in Diacritics)
                    {
                        _ = sb.Append(diacritic);
                    }
                    _ = sb.Append(']');
                    _diacritic_pattern = sb.ToString();
                }
                return _diacritic_pattern;
            }
        }

        /// <summary>
        /// Returns a Dictionary that maps an IPA phoneme or diacritic to a textual
        /// description of that symbol.
        /// </summary>
        public static Dictionary<string, string> IpaPhonemesMap { get; } = new()
        {
			// Plosives
			{ "\u0070", new string("unvoiced bilabial plosive") },
            { "\u0062", new string("voiced bilabial plosive") },
            { "\u0070\u032a", new string("unvoiced labiodental plosive") },
            { "\u0062\u032a", new string("voiced labiodental plosive") },
            { "\u0074\u032a", new string("unvoiced dental plosive") },
            { "\u0064\u032a", new string("voiced dental plosive") },
            { "\u0074", new string("unvoiced alveolar plosive") },
            { "\u0064", new string("voiced alveolar plosive") },
            { "\u0288", new string("unvoiced retroflex plosive") },
            { "\u0256", new string("voiced retroflex plosive") },
            { "\u0236", new string("unvoiced alveolo-palatal plosive") },
            { "\u0221", new string("voiced alveolo-palatal plosive") },
            { "\u0063", new string("unvoiced palatal plosive") },
            { "\u025f", new string("voiced palatal plosive") },
            { "\u006b\u0361\u0070", new string("unvoiced labial-velar") },
            { "\u0261\u0361\u0062", new string("voiced labial-velar") },
            { "\u006b", new string("unvoiced velar plosive") },
            { "\u0261", new string("voiced velar plosive") },
            { "\u0071", new string("unvoiced uvular plosive") },
            { "\u0262", new string("voiced uvular plosive") },
            { "\u02a1", new string("unvoiced epiglottal plosive") },
            { "\u0294", new string("unvoiced glottal plosive") },
			// Implosives
			{ "\u0253\u0325", new string("unvoiced bilabial implosive") },
            { "\u0253", new string("voiced bilabial implosive") },
            { "\u0257\u032a", new string("voiced dental implosive") },
            { "\u0257", new string("voiced alveolar implosive") },
            { "\u1d91", new string("voiced retroflex implosive") },
            { "\u0284", new string("voiced palatal implosive") },
            { "\u0260", new string("voiced velar implosive") },
            { "\u029b", new string("voiced uvular implosive") },
			// Ejectives
			{ "\u0070\u02bc", new string("unvoiced bilabial ejective") },
            { "\u0074\u032a\u02bc", new string("unvoiced dental ejective") },
            { "\u0074\u02bc", new string("unvoiced alveolar ejective") },
            { "\u0288\u02bc", new string("unvoiced retroflex ejective") },
            { "\u0063\u02bc", new string("unvoiced palatal ejective") },
            { "\u006b\u02bc", new string("unvoiced velar ejective") },
            { "\u0071\u02bc", new string("unvoiced uvular ejective") },
			// Nasals
			{ "\u006d\u0325", new string("unvoiced bilabial nasal") },
            { "\u006d", new string("voiced bilabial nasal") },
            { "\u0271\u030a", new string("unvoiced labiodental nasal") },
            { "\u0271", new string("voiced bilabial nasal") },
            { "\u006e\u032a\u030a", new string("unvoiced dental nasal") },
            { "\u006e\u032a", new string("voiced dental nasal") },
            { "\u006e\u0325", new string("unvoiced alveolar nasal") },
            { "\u006e", new string("voiced alveolar nasal")},
            { "\u0273\u030a", new string("unvoiced retroflex nasal") },
            { "\u0273", new string("voiced retroflex nasal") },
            { "\u0235", new string("voiced alveolo-palatal nasal") },
            { "\u0272", new string("voiced palatal nasal") },
            { "\u014b\u0361\u006d", new string("voiced labial-velar") },
            { "\u014b", new string("voiced velar nasal") },
            { "\u0274", new string("voiced uvular nasal") },
			// Trills
			{ "\u0299", new string("voiced bilabial trill") },
            { "\u0072\u0325", new string("unvoiced alveolar trill") },
            { "\u0072", new string("voiced alveolar trill") },
            { "\u0280", new string("voiced uvular trill") },
			// Taps or Flaps
			{ "\u2c71\u031f", new string("voiced bilabial tap or flap") },
            { "\u2c71", new string("voiced labiodental tap or flap") },
            { "\u027e", new string("voiced alveolar tap or flap") },
            { "\u027d", new string("voiced retroflex tap or flap") },
			// Lateral flaps
			{ "\u027a", new string("voiced alveolar lateral flap") },
            // Wikipedia has a voiced retroflex lateral flap with an invalid unicode (or at least UTF8)\
            // Affricate (Only in Vulgarlang, but needed to support conlangs coming from Vulgarlang)
            { "\u02a6", new string("unvoiced alveolar affricate") },
            { "\u02a3", new string("voiced alveolar affricate") },
            { "\u02a7", new string("unvoiced postalveolar affricate") },
            { "\u02a4", new string("voiced postalveolar affricate") },
            { "\u02a8", new string("unvoiced alveolo-palatal affricate") },
            { "\u02a5", new string("voiced alveolo-palatal affricate") },
			// Fricatives
			{ "\u0278", new string("unvoiced bilabial fricative") },
            { "\u03b2", new string("voiced bilabial fricative") },
            { "\u0066", new string("unvoiced labiodental fricative") },
            { "\u0076", new string("voiced labiodental fricative") },
            { "\u03b8", new string("unvoiced dental fricative") },
            { "\u00f0", new string("voiced dental fricative") },
            { "\u0073", new string("unvoiced alveolar fricative") },
            { "\u007a", new string("voiced alveolar fricative") },
            { "\u0283", new string("unvoiced postalveolar fricative") },
            { "\u0292", new string("voiced postalveolar fricative") },
            { "\u0282", new string("unvoiced retroflex fricative") },
            { "\u0290", new string("voiced retroflex fricative") },
            { "\u0267", new string("unvoiced postalveolar-velar fricative") },
            { "\u0255", new string("unvoiced alveolo-palatal fricative") },
            { "\u0291", new string("voiced alveolo-palatal fricative") },
            { "\u00e7", new string("unvoiced palatal fricative") },
            { "\u029d", new string("voiced palatal fricative") },
            { "\u0078", new string("unvoiced velar fricative") },
            { "\u0263", new string("voiced velar fricative") },
            { "\u03c7", new string("unvoiced uvular fricative") },
            { "\u0281", new string("voiced uvular fricative") },
            { "\u0127", new string("unvoiced pharyngeal fricative") },
            { "\u0295", new string("voiced pharyngeal fricative") },
            { "\u029c", new string("unvoiced epiglottal fricative") },
            { "\u02a2", new string("voiced epiglottal fricative") },
            { "\u0068", new string("unvoiced glottal fricative") },
            { "\u0266", new string("voiced glottal fricative") },
			// Lateral fricatives
			{ "\u026c", new string("unvoiced alveolar lateral fricative") },
            { "\u026e", new string("voiced alveolar lateral fricative") },
            { "\ua78e", new string("unvoiced retroflex lateral fricative") },
			// Ejective fricatives
			{ "\u0073\u02bc", new string("unvoiced alveolar ejective fricative") },
            { "\u0283\u02bc", new string("unvoiced retroflex ejective fricative") },
            { "\u026c\u02bc", new string("unvoiced alveolar ejective fricative") },
			// percussive (Category only seen on Wikipedia)
			{ "\u02ac", new string("bilabial percussive") },
            { "\u02ad", new string("dental percussive") },
			// approximant
			{ "\u03b2\u031e\u030a", new string("unvoiced bilabial approximant") },
            { "\u03b2\u031e", new string("voiced bilabial approximant") },
            { "\u028b\u0325", new string("unvoiced labiodental approximant") },
            { "\u028b", new string("voiced labiodental approximant") },
            { "\u00f0\u031e", new string("voiced dental approximant") },
            { "\u0279\u0325", new string("unvoiced alveolar approximant") },
            { "\u0279", new string("voiced alveolar approximant") },
            { "\u027b\u030a", new string("unvoiced retroflex approximant") },
            { "\u027b", new string("voiced retroflex approximant") },
            { "\u0265\u0308", new string("unvoiced labialized palatial approximant") },
            { "\u0265", new string("voiced labialized palatial approximant") },
            { "\u006a", new string("voiced palatal approximant") },
            { "\u028d", new string("unvoiced labial-velar approximant (fricative)") },
            { "\u0077", new string("voiced labial-velar approximant") },
            { "\u0270", new string("voiced velar approximant") },
			// Lateral approximant
			{ "\u006c\u0325", new string("unvoiced alveolar lateral approximant") },
            { "\u006c", new string("voiced alveolar lateral approximant") },
            { "\u026d", new string("voiced retroflex lateral approximant") },
            { "\u0234", new string("voiced alveolo-palatal lateral approximant") },
            { "\u028e", new string("voiced palatal lateral approximant") },
            { "\u029f", new string("voiced velar lateral approximant") },
			// click consonants
			{ "\u0298", new string("bilabial click") },
            { "\u01c0", new string("dental click") },
            { "\u01c3", new string("alveolar click") },
            { "\u01c2", new string("postalveolar click") },
            { "\u01c1", new string("alveolar lateral click") },
			// Vowels close->open
			{ "\u0069", new string("close front unrounded vowel") },
            { "\u0079", new string("close front rounded vowel") },
            { "\u0268", new string("close central unrounded vowel") },
            { "\u0289", new string("close central rounded vowel") },
            { "\u026f", new string("close back rounded vowel") },
            { "\u0075", new string("close back unrounded vowel") },
            { "\u026a", new string("near-close front unrounded vowel") },
            { "\u028f", new string("near-close front rounded vowel")},
            { "\u026a\u0308", new string("near-close central unrounded vowel") },
            { "\u028a\u0308", new string("near-close central rounded vowel") },
            { "\u028a", new string("near-close back rounded vowel") },
            { "\u0065", new string("close-mid front unrounded vowel") },
            { "\u00f8", new string("close-mid front rounded vowel") },
            { "\u0258", new string("close-mid central unrounded vowel") },
            { "\u0275", new string("close-mid central rounded vowel") },
            { "\u0264", new string("close-mid back unrounded vowel") },
            { "\u006f", new string("close-mid back rounded vowel") },
            { "\u0065\u031e", new string("mid front unrounded vowel") },
            { "\u00f8\u031e", new string("mid front rounded vowel") },
            { "\u0259", new string("mid central vowel") },
            { "\u025a", new string("mid central vowel rhoticity") },
            { "\u0264\u031e", new string("mid back unrounded vowel") },
            { "\u006f\u031e", new string("mid back rounded vowel") },
            { "\u025b", new string("open-mid front unrounded vowel") },
            { "\u0153", new string("open-mid front rounded vowel") },
            { "\u025c", new string("open-mid central unrounded vowel") },
            { "\u025e", new string("open-mid central rounded vowel") },
            { "\u028c", new string("open-mid back unrounded vowel") },
            { "\u0254", new string("open-mid back rounded vowel") },
            { "\u00e6", new string("near-open front unrounded vowel") },
            { "\u0250", new string("near-open central unrounded vowel") },
            { "\u0061", new string("open front unrounded vowel") },
            { "\u0276", new string("open front rounded vowel") },
            { "\u0061\u0308", new string("open central unrounded vowel") },
            { "\u0251", new string("open back unrounded vowel") },
            { "\u0252", new string("open back rounded vowel") },
			// Diacritics etc.
			{ "\u02f3", new string("voiceless") },
            { "\u0325", new string("voiceless") },
            { "\u0324", new string("breathy voiced") },
            { "\u032a", new string("dental") },
            { "\u02cc", new string("syllabic") },
            { "\u0329", new string("syllabic") },
            { "\u02ec", new string("voiced") },
            { "\u032c", new string("voiced") },
            { "\u02f7", new string("creaky voiced") },
            { "\u0330", new string("creaky voiced") },
            { "\u02fd", new string("apical") },
            { "\u033a", new string("apical") },
            { "\u032f", new string("non-syllabic") },
            { "\u02b0", new string("aspirated") },
            { "\u033c", new string("linguolabial") },
            { "\u033b", new string("laminal") },
            { "\u02d2", new string("more rounded") },
            { "\u0339", new string("more rounded") },
            { "\u03b7", new string("labialized") },
            { "\u0303", new string("nasalized") },
            { "\u02b2", new string("palatalized") },
            { "\u02d3", new string("less rounded") },
            { "\u031c", new string("less rounded") },
            { "\u02d6", new string("advanced") },
            { "\u031f", new string("advanced") },
            { "\u027f", new string("nasal release") },
            { "\u00a8", new string("centralized") },
            { "\u0308", new string("centralized") },
            { "\u02e0", new string("velarized") },
            { "\u02cd", new string("retracted") },
            { "\u0320", new string("retracted") },
            { "\u02e1", new string("lateral release") },
            { "\u02df", new string("mid-centralized") },
            { "\u033d", new string("mid-centralized") },
            { "\u02e4", new string("pharyngealized") },
            { "\uab6a", new string("advance tongue root") },
            { "\u0318", new string("advance tongue root") },
            { "\u02fa", new string("no audible release") },
            { "\u031a", new string("no audible release") },
            { "\u02d4", new string("raised") },
            { "\u031d", new string("raised") },
            { "\u0334", new string("velarized or pharyngealized") },
            { "\uab6b", new string("retracted tongue root") },
            { "\u0319", new string("retracted tongue root") },
            { "\u02de", new string("rhoticity") },
            { "\u02d5", new string("lowered") },
            { "\u031e", new string("lowered") },
            { "\u02d0", new string("lengthened, length mark") },
			// From here down this is from https://www.phon.ucl.ac.uk/home/wells/ipa-unicode.htm, January 29, 2024
			{ "\u02d1", new string("half-length") },
            { "\u02c8", new string("primary stress mark") },
        };

        /// <summary>
        /// Returns a Dictionary that can be used to replace latin characters
        /// that closely resemble IPA characters with the IPA character they can
        /// easily be confused with.  
        /// </summary>
        public static Dictionary<string, string> LatinIpaReplacements { get; } = new()
        {
            { "g", "\u0261" },
            { "G", "\u0262" },
            { "N", "\u0274" },
            { "B", "\u0299" },
            { "R", "\u0280" },
            { "H", "\u029c" },
            { "L", "\u029f" },
            { "I", "ɪ"}
        };

        /// <summary>
        /// Returns a Dictionary that maps the IPA consonants to a list of other consonants that
        /// are considered to be "1 degree" away and can be used for a Level 1 substitution.
        /// </summary>
        public static Dictionary<string, List<string>> Consonant_changes
        {
            get => _consonant_changes;
        }

        /// <summary>
        /// Returns a Dictionary that maps the IPA consonants to a list of other consonants that
        /// are considered to be "2 degrees" away and can be used for a Level 2 substitution.
        /// </summary>
        public static Dictionary<string, List<string>> Consonant_changes_l2
        {
            get
            {
                _consonant_changes_l2 ??= PopulateL2ConantChanges();
                return _consonant_changes_l2;
            }
        }

        /// <summary>
        /// Returns a Dictionary that maps the IPA consonants to a list of other consonants that
        /// are considered to be "3 degrees" away and can be used for a Level 3 substitution.
        /// </summary>
        public static Dictionary<string, List<string>> Consonant_changes_l3
        {
            get
            {
                _consonant_changes_l3 ??= PopulateL3ConantChanges();
                return _consonant_changes_l3;
            }
        }

        /// <summary>
        /// Returns a list of Unicode symbols that can be utilized within an IPA string
        /// as a temporary placeholder during a multi-phase replacement operation.  These
        /// are Unicode symbols that are not part of IPA and are distinct from any IPA character
        /// so that if an error occurs and they remain in place the error will be noticeable.
        /// </summary>
        /// <returns></returns>
        public static List<string> Ipa_replacements { get; } = [
            "!",
            "@",
            "#",
            "$",
            "%",
            "&",
            "*",
            "+",
            "=",
            "<",
            ">",
            "~",
            "\u00a2",
            "\u00a3",
            "\u00a4",
            "\u00a5",
            "\u00a7",
            "\u00a9",
            "\u00ae",
            "\u0394",
            "\u039e",
            "\u03a6",
            "\u03a8",
        ];

        /// <summary>
        /// Returns a single string that contains all of the IPA symbols along
        /// with their description, one symbol per line.  This listing will also
        /// include the Unicode U+xxxx code for any non-ascii characters within
        /// the symbol. 
        /// </summary>
        /// <returns>String with the list of IPA symbols for printing/debugging purposes.</returns>
        public static string IpaPhonemes()
        {
            StringBuilder sb = new();
            foreach (string phoneme in IpaPhonemesMap.Keys)
            {
                string description = IpaPhonemesMap[phoneme];
                StringBuilder sb2 = new();
                foreach (char c in phoneme)
                {
                    if (Char.IsAsciiLetterOrDigit(c))
                    {
                        _ = sb2.Append(c);
                    }
                    else
                    {
                        int cInt = c;
                        _ = sb2.AppendFormat("U+{0,4:x4}", cInt);
                    }
                }
                _ = sb.AppendFormat("{0} ({2}):\t{1}\n", phoneme, description, sb2.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Searches an entire LanguageDescription object and replaces any characters within a
        /// field that should contain IPA that are Latin characters easily used in places of similar-
        /// looking IPA characters with the correct (or suspected to be correct) IPA character.
        /// </summary>
        /// <param name="language">LanguageDescription object to be updated.</param>
        public static void SubstituteLatinIpaReplacements(LanguageDescription language)
        {
            language.native_name_phonetic = SubstituteLatinIpaReplacements(language.native_name_phonetic);

            if (language.phoneme_inventory != null)
            {
                List<string> newPhonemeInventory = [];
                foreach (string phoneme in language.phoneme_inventory)
                {
                    newPhonemeInventory.Add(SubstituteLatinIpaReplacements(phoneme));
                }
                language.phoneme_inventory = newPhonemeInventory;
            }

            if (language.phonetic_inventory != null)
            {
                foreach (string piKey in language.phonetic_inventory.Keys)
                {
                    if (language.phonetic_inventory[piKey] != null)
                    {
                        for (int i = 0; i < language.phonetic_inventory[piKey].Length; i++)
                        {
                            language.phonetic_inventory[piKey][i] = SubstituteLatinIpaReplacements(language.phonetic_inventory[piKey][i]);
                        }
                    }
                }
            }

            if (language.spelling_pronunciation_rules != null)
            {
                foreach (SpellingPronunciationRules soundMap in language.spelling_pronunciation_rules)
                {
                    soundMap.phoneme = SubstituteLatinIpaReplacements(soundMap.phoneme);
                    soundMap.spelling_regex = SubstituteLatinIpaReplacements(soundMap.spelling_regex);
                }
            }

            if (language.affix_map != null)
            {
                foreach (string partOfSpeech in language.affix_map.Keys)
                {
                    List<Dictionary<string, List<Dictionary<string, Affix>>>> affixList = language.affix_map[partOfSpeech];
                    foreach (Dictionary<string, List<Dictionary<string, Affix>>> affixTypeMap in affixList)
                    {
                        foreach (string affixType in affixTypeMap.Keys)
                        {
                            List<Dictionary<string, Affix>> declensionList = affixTypeMap[affixType];
                            foreach (Dictionary<string, Affix> affixMap in declensionList)
                            {
                                foreach (string declension in affixMap.Keys)
                                {
                                    Affix affix = affixMap[declension];
                                    if (affix.pronunciation_add != null)
                                    {
                                        affix.pronunciation_add = SubstituteLatinIpaReplacements(affix.pronunciation_add);
                                    }
                                    if (affix.pronunciation_regex != null)
                                    {
                                        affix.pronunciation_regex = SubstituteLatinIpaReplacements(affix.pronunciation_regex);
                                    }
                                    if (affix.t_pronunciation_add != null)
                                    {
                                        affix.t_pronunciation_add = SubstituteLatinIpaReplacements(affix.t_pronunciation_add);
                                    }
                                    if (affix.f_pronunciation_add != null)
                                    {
                                        affix.f_pronunciation_add = SubstituteLatinIpaReplacements(affix.f_pronunciation_add);
                                    }
                                    if (affix.pronunciation_replacement != null)
                                    {
                                        affix.pronunciation_replacement = SubstituteLatinIpaReplacements(affix.pronunciation_replacement);
                                    }
                                }
                            }

                        }
                    }

                }
            }

            if (language.derivational_affix_map != null)
            {
                foreach (string derivationKey in language.derivational_affix_map.Keys)
                {
                    DerivationalAffix derivationalAffix = language.derivational_affix_map[derivationKey];
                    if (derivationalAffix.pronunciation_regex != null)
                    {
                        derivationalAffix.pronunciation_regex = SubstituteLatinIpaReplacements(derivationalAffix.pronunciation_regex);
                    }
                    if (derivationalAffix.pronunciation_add != null)
                    {
                        derivationalAffix.pronunciation_add = SubstituteLatinIpaReplacements(derivationalAffix.pronunciation_add);
                    }
                    if (derivationalAffix.t_pronunciation_add != null)
                    {
                        derivationalAffix.t_pronunciation_add = SubstituteLatinIpaReplacements(derivationalAffix.t_pronunciation_add);
                    }
                    if (derivationalAffix.f_pronunciation_add != null)
                    {
                        derivationalAffix.f_pronunciation_add = SubstituteLatinIpaReplacements(derivationalAffix.f_pronunciation_add);
                    }
                }
            }

            if (language.lexicon != null)
            {
                foreach (LexiconEntry entry in language.lexicon)
                {
                    if (entry.phonetic != null)
                    {
                        entry.phonetic = SubstituteLatinIpaReplacements(entry.phonetic);
                    }
                }
            }
        }

        /// <summary>
        /// Replace all of the Latin characters that closely resemble IPA characters
        /// with their matching IPA characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SubstituteLatinIpaReplacements(string text)
        {
            string fixedText = text;
            foreach (string bad in LatinIpaReplacements.Keys)
            {
                fixedText = fixedText.Replace(bad, LatinIpaReplacements[bad]);
            }
            return fixedText;
        }

        /// <summary>
        /// Build or rebuild the phonetic_inventory section of the Language 
        /// structure
        /// </summary>
        /// <param name="languageDescription">Conlang JSON structure read into .NET</param>
        public static void BuildPhoneticInventory(LanguageDescription languageDescription)
        {
            SortedSet<string> pConsonants = [];
            SortedSet<string> npConsonants = [];
            SortedSet<string> vowels = [];
            SortedSet<string> vDiphthongs = [];

            // vDiphthongs will be trusted if it exists
            if ((languageDescription.phoneme_inventory != null) && (languageDescription.phoneme_inventory.Contains("v_diphthongs")))
            {
                foreach (string diphthong in languageDescription.phonetic_inventory["v_diphthongs"])
                {
                    _ = vDiphthongs.Add(diphthong);
                }
            }
            // Add any potential vowel diphthongs found in the phoneme inventory
            if (languageDescription.phoneme_inventory != null)
            {
                foreach (string phoneme in languageDescription.phoneme_inventory)
                {
                    if (phoneme.Length > 1)
                    {
                        int vowelCount = 0;
                        int symbolCount = 0;
                        bool isPossibleDiphthong = true;
                        foreach (char letter in phoneme)
                        {
                            if (Vowels.Contains(letter.ToString()))
                            {
                                vowelCount += 1;
                            }
                            else if (Vowel_modifiers.Contains(letter.ToString()))
                            {
                                symbolCount += 1;
                            }
                            else
                            {
                                isPossibleDiphthong = false;
                                break;
                            }
                        }
                        if (isPossibleDiphthong && (vowelCount == 2) && (symbolCount <= vowelCount))
                        {
                            _ = vDiphthongs.Add(phoneme);
                        }
                    }
                }
            }

            // Iterate over the lexicon looking at the phonetic representation of each word
            foreach (LexiconEntry entry in languageDescription.lexicon)
            {
                StringBuilder priorChar = new();
                // Iterate over the characters in the phonetic representation
                foreach (char letter in entry.phonetic)
                {
                    if (string.IsNullOrEmpty(priorChar.ToString()))
                    {
                        _ = priorChar.Clear();
                        _ = priorChar.Append(letter);
                    }
                    else
                    {
                        if ((Diacritics.Contains(letter.ToString())) || (letter == '\u02d0') || (letter == '\u02d1'))
                        {
                            string newChar = priorChar + letter.ToString();
                            if (PConsonants.Contains(priorChar.ToString()))
                            {
                                _ = pConsonants.Add(newChar);
                                _ = priorChar.Clear();
                            }
                            else if (NpConsonants.Contains(priorChar.ToString()))
                            {
                                _ = npConsonants.Add(newChar);
                                _ = priorChar.Clear();
                            }
                            else if (ContainsVowels(priorChar.ToString()))
                            {
                                _ = priorChar.Append(letter);
                            }
                        }
                        else if ((letter == '\u035c') || (letter == '\u0361'))
                        {
                            if (ContainsVowels(priorChar.ToString()))
                            {
                                _ = priorChar.Append(letter);
                            }
                        }
                        else if (PConsonants.Contains(priorChar.ToString()))
                        {
                            _ = pConsonants.Add(priorChar.ToString());
                            _ = priorChar.Clear();
                            _ = priorChar.Append(letter);
                        }
                        else if (NpConsonants.Contains(priorChar.ToString()))
                        {
                            _ = npConsonants.Add(priorChar.ToString());
                            _ = priorChar.Clear();
                            _ = priorChar.Append(letter);
                        }
                        else if (ContainsVowels(priorChar.ToString()))
                        {
                            if (Vowels.Contains(letter.ToString()))
                            {
                                if (priorChar.Length >= 2)
                                {
                                    // First add this as a potential diphthong since that is probably how it will be treated 
                                    // by the text-to-speech engines.
                                    int vCount = CountVowels(priorChar.ToString());
                                    int dCount = CountDiacritics(priorChar.ToString());
                                    if ((vCount >= 2) || (dCount >= 2))
                                    {
                                        if (!vDiphthongs.Contains(priorChar.ToString()))
                                        {
                                            int vowelCount = 0;
                                            int symbolCount = 0;
                                            bool isPossibleDiphthong = true;
                                            foreach (char letter2 in priorChar.ToString())
                                            {
                                                if (Vowels.Contains(letter.ToString()))
                                                {
                                                    vowelCount += 1;
                                                }
                                                else if (Vowel_modifiers.Contains(letter.ToString()))
                                                {
                                                    symbolCount += 1;
                                                }
                                                else
                                                {
                                                    isPossibleDiphthong = false;
                                                    break;
                                                }
                                            }
                                            if (isPossibleDiphthong && (vowelCount == 2) && (symbolCount <= vowelCount))
                                            {
                                                _ = vDiphthongs.Add(priorChar.ToString());
                                            }

                                            // Also consider adding it as individual vowels for editing purposes.
                                            StringBuilder workingChar = new();
                                            foreach (char letter2 in priorChar.ToString())
                                            {
                                                if (ContainsVowels(letter2.ToString()))
                                                {
                                                    if (workingChar.Length > 0)
                                                    {
                                                        _ = vowels.Add(workingChar.ToString());
                                                        _ = workingChar.Clear();
                                                        _ = workingChar.Append(letter2);
                                                    }
                                                    else
                                                    {
                                                        _ = workingChar.Append(letter2);
                                                    }
                                                }
                                                else
                                                {
                                                    _ = workingChar.Append(letter2);
                                                }
                                            }
                                            if (!(string.IsNullOrEmpty(workingChar.ToString())))
                                            {
                                                _ = vowels.Add(workingChar.ToString());
                                            }
                                        }
                                        _ = priorChar.Clear();
                                        _ = priorChar.Append(letter);
                                    }
                                    else
                                    {
                                        _ = priorChar.Append(letter);
                                    }
                                }
                                else
                                {
                                    _ = priorChar.Append(letter); // Keep building the possible diphthong
                                }
                            }
                            else
                            {
                                string lastPriorChar = priorChar.ToString().Last().ToString();
                                if ((priorChar.Length == 1) ||
                                    ((priorChar.Length == 2) && (Vowel_modifiers.Contains(lastPriorChar))))
                                {
                                    _ = vowels.Add(priorChar.ToString());
                                }
                                else
                                {
                                    if (!vDiphthongs.Contains(priorChar.ToString()))
                                    {
                                        StringBuilder workingChar = new();
                                        foreach (char letter2 in priorChar.ToString())
                                        {
                                            if (ContainsVowels(letter2.ToString()))
                                            {
                                                if (workingChar.Length > 0)
                                                {
                                                    _ = vowels.Add(workingChar.ToString());
                                                    _ = workingChar.Clear();
                                                    _ = workingChar.Append(letter2);
                                                }
                                                else
                                                {
                                                    _ = workingChar.Append(letter2);
                                                }
                                            }
                                            else
                                            {
                                                _ = workingChar.Append(letter2);
                                            }
                                        }
                                        if (!(string.IsNullOrEmpty(workingChar.ToString())))
                                        {
                                            _ = vowels.Add(workingChar.ToString());
                                        }
                                    }
                                }
                                _ = priorChar.Clear();
                                _ = priorChar.Append(letter);
                            }
                        }
                    }
                    // treat stress like new word.
                    if ((priorChar.Equals("\u02c8")) || (priorChar.Equals("\u02cc")))
                    {
                        _ = priorChar.Clear();
                    }
                }
            }

            Dictionary<string, string[]> phonetic_inventory = new()
            {
                ["p_consonants"] = [.. pConsonants],
                ["np_consonants"] = [.. npConsonants],
                ["vowels"] = [.. vowels],
                ["v_diphthongs"] = [.. vDiphthongs]
            };
            languageDescription.phonetic_inventory = phonetic_inventory;
        }

        private static bool ContainsVowels(string priorChars)
        {
            bool containsVowels = false;
            foreach (var _ in from char c in priorChars.ToString()
                              where Vowels.Contains(c.ToString())
                              select new { })
            {
                containsVowels = true;
            }

            return containsVowels;
        }

        private static int CountVowels(string priorChars)
        {
            return (from char c in priorChars
                    where Vowels.Contains(c.ToString())
                    select c).Count();
        }

        private static int CountDiacritics(string priorChars)
        {
            int count = 0;
            foreach (char c in priorChars)
            {
                if (Diacritics.Contains(c.ToString()))
                {
                    count++;
                }
                else if ((c == '\u02D0') || (c == '\u02D1'))
                {
                    count++;
                }
            }
            return count;
        }

        private static Dictionary<string, List<string>> PopulateL2ConantChanges()
        {
            Dictionary<string, List<string>> pConsonantChangesL2 = [];

            foreach (string pConsonant in Consonant_changes.Keys)
            {
                SortedSet<string> addedChanges = [];
                foreach (string oldChange in Consonant_changes[pConsonant])
                {
                    // Iterate over the changes for the existing changes in this consonant and add
                    // them to the candidate set of changes if, and only if, they are not already
                    // changes
                    foreach (string candidateChange in Consonant_changes[oldChange])
                    {
                        if ((!candidateChange.Equals(pConsonant)) && (!Consonant_changes[pConsonant].Contains(candidateChange)))
                        {
                            _ = addedChanges.Add(candidateChange);
                        }
                    }
                }
                List<string> l2changes = [.. Consonant_changes[pConsonant], .. addedChanges];
                pConsonantChangesL2.Add(pConsonant, l2changes);
            }

            return pConsonantChangesL2;
        }
        private static Dictionary<string, List<string>> PopulateL3ConantChanges()
        {
            Dictionary<string, List<string>> pConsonantChangesL3 = [];

            foreach (string pConsonant in Consonant_changes_l2.Keys)
            {
                SortedSet<string> addedChanges = [];
                foreach (string oldChange in Consonant_changes_l2[pConsonant])
                {
                    // Iterate over the changes for the existing changes in this consonant and add
                    // them to the candidate set of changes if, and only if, they are not already
                    // changes
                    foreach (string candidateChange in Consonant_changes_l2[oldChange])
                    {
                        if ((!candidateChange.Equals(pConsonant)) && (!Consonant_changes_l2[pConsonant].Contains(candidateChange)))
                        {
                            _ = addedChanges.Add(candidateChange);
                        }
                    }
                }
                List<string> l3changes = [.. Consonant_changes[pConsonant], .. addedChanges];
                pConsonantChangesL3.Add(pConsonant, l3changes);
            }

            return pConsonantChangesL3;
        }

    }
}
