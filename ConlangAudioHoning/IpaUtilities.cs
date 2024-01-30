﻿/*
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
using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ConlangAudioHoning
{
    public static class IpaUtilities
    {
        private static string[] _p_consonants =
        {
            "b", "\u03b2", "\u0299", "c", "\u00e7", "d", "\u0256", "\u1d91", "\u02a3", "\u02a5", "\u02a4", "\uab66",
            "f", "\u0278", "g", "\u0262", "\u0270", "h", "\u0266", "\u0127", "\u0267", "j", "\u029d", "\u025f", "k",
            "l", "\u026b", "\u026c", "\u026e", "\u026d", "\ua78e", "\u029f", "m",
            "\u0271", "n", "\u0273", "\u0272", "\u014b", "\u0274", "p", "q", "r", "\u0279", "\u027e", "\u027d",
            "\u027b", "\u027a", "\u0281", "\u0280", "s", "\u0282", "\u0283", "t", "\u0288", "\u02a6",
            "\u02a8", "\u02a7", "\uab67", "v", "\u2c71", "\u028b", "x", "\u0263", "\u03c7", "\u028e",
            "z", "\u0290", "\u0292", "\u03b8", "\u00f0", "\u0294", "\u0295"
        };
        /*
		 * b β ʙ c ç d ɖ ᶑ ʣ ʥ ʤ ꭦ		"b", "\u03b2", "\u0299", "c", "\u00e7", "d", "\u0256", "\u1d91", "\u02a3", "\u02a5", "\u02a4", "\uab66",
		 * f ɸ g ɢ ɰ h ɦ ħ ɧ j ʝ ɟ k	"f", "\u0278", "g", "\u0262", "\u0270", "h", "\u0266", "\u0127", "\u0267", "j", "\u029d", "\u025f", "k",
		 * l ɫ ɬ ɮ ɭ ꞎ ʟ m				"l", "\u026b", "\u026c", "\u026e", "\u026d", "\ua78e", "\u029f", "m",
		 * ɱ n ɳ ɲ ŋ ɴ p q r ɹ ɾ ɽ		"\u0271", "n", "\u0273", "\u0272", "\u014b", "\u0274", "p", "q", "r", "\u0279", "\u027e", "\u027d",
		 * ɻ ɺ ʁ ʀ s ʂ ʃ t ʈ ʦ ʨ		"\u027b", "\u027a", "\u0281", "\u0280", "s", "\u0282", "\u0283", "t", "\u0288", "\u02a6",
		 * ʧ ꭧ v ⱱ ʋ x ɣ χ ʎ			"\u02a8", "\u02a7", "\uab67", "v", "\u2c71", "\u028b", "x", "\u0263", "\u03c7", "\u028e",
		 * z ʐ ʒ θ ð ʔ ʕ				"z", "\u0290", "\u0292", "\u03b8", "\u00f0", "\u0294", "\u0295"
		 */

        private static string[] _np_consonants =
        {
            "\u0253", "\u0257", "\u0284", "\u0260", "\u029b", "w", "\u028d", "\u0265", "\u02a1", "\u02a2", "\u0255",
            "\u0291", "\u029c", "\u0298", "\u01c0", "\u01c3", "\u01c2", "\u01c1", "\ud837\udf0a"
        };

        private static string[] _vowels =
        {
            "a", "\u00e6", "\u0251", "\u0252", "\u0250", "e", "\u025b", "\u025c", "\u025e", "\u0259", "i", "\u0268",
            "\u026a", "y", "\u028f", "\u00f8", "\u0258", "\u0275", "\u0153", "\u0276", "\u0264", "o", "\u0254", "u",
            "\u0289", "\u028a", "\u026f", "\u028c", "\u025a", "\u02de"
        };

        private static string[] _suprasegmentals =
        {
            "\u02d0", "\u02d1", "\u02c8", "\02cc", "\u035c", "\u0361"
        };

        private static string[] _diacritics =
        {
            "\u02f3", "\u0325", "\u030a", "\u0324", "\u032a", "\u02cc", "\u0329", "\u0c3c", "\u032c", "\u02f7", "\u0330",
            "\u02f7", "\u0330", "\u02fd", "\u033a", "\u032f", "\u02b0", "\u033c", "\u033b", "\u02d2", "\u0339", "\u20b7",
            "\u0303", "\u02b2", "\u02d3", "\u031c", "\u02d6", "\u031f", "\u207f", "\u00a8", "\u0308", "\u02e0", "\u02cd",
            "\u0320", "\u20e1", "\u02df", "\u033d", "\u02e4", "\uab68", "\u0319", "\u02de"
        };

        // The following is based on the chart found at https://en.wikipedia.org/wiki/Phonetic_symbols_in_Unicode
        // As of January 29, 2024
        private static Dictionary<string, string> _ipaPhonemesMap = new Dictionary<string, string>()
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
			// Wikipedia has a voiced retroflex lateral flap with an invalid unicode (or at least UTF8)
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
			// approximates
			{ "\u03b2\u031e\u030a", new string("unvoiced bilabial approximate") },
            { "\u03b2\u031e", new string("voiced bilabial approximate") },
            { "\u028b\u0325", new string("unvoiced labiodental approximate") },
            { "\u028b", new string("voiced labiodental approximate") },
            { "\u00f0\u031e", new string("voiced dental approximate") },
            { "\u0279\u0325", new string("unvoiced alveolar approximate") },
            { "\u0279", new string("voiced alveolar approximate") },
            { "\u027b\u030a", new string("unvoiced retroflex approximate") },
            { "\u027b", new string("voiced retroflex approximate") },
            { "\u0265\u0308", new string("unvoiced labialized palatial approximate") },
            { "\u0265", new string("voiced labialized palatial approximate") },
            { "\u006a", new string("voiced palatal approximate") },
            { "\u028d", new string("unvoiced labial-velar approximate") },
            { "\u0077", new string("voiced labial-velar approximate") },
            { "\u0270", new string("voiced velar approximate") },
			// Lateral approximates
			{ "\u006c\u0325", new string("unvoiced alveolar lateral approximate") },
            { "\u006c", new string("voiced alveolar lateral approximate") },
            { "\u026d", new string("voiced retroflex lateral approximate") },
            { "\u0234", new string("voiced alveolo-palatal lateral approximate") },
            { "\u028e", new string("voiced palatal lateral approximate") },
            { "\u029f", new string("voiced velar lateral approximate") },
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
            { "\u0264\u031e", new string("mid back unrounded vowel") },
            { "\u006f\u031e", new string("mid back rounded vowel") },
            { "\u025b", new string("open-mid front unrounded vowel") },
            { "\u015e", new string("open-mid front rounded vowel") },
            { "\u025c", new string("open-mid central unrounded vowel") },
            { "\u025e", new string("open-mid central rounded vowel") },
            { "\u028c", new string("open-mid back unrounded vowel") },
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

        private static Dictionary<string, string> _latinIpaReplacements = new Dictionary<string, string>()
        {
            { "g", "\u0261" },
            { "G", "\u0262" },
            { "N", "\u0274" },
            { "B", "\u0299" },
            { "R", "\u0280" },
            { "H", "\u029c" },
            { "L", "\u029f" },
        };

        private static Dictionary<string, List<string>> _p_consonant_changes = new Dictionary<string, List<string>>()
        {
            // Plosive
            { "p", new List<string>(){ "b","m","t","d","\u0271" } },
            { "b", new List<string>(){ "p","m","t","d","\u0271" } },
            { "t", new List<string>(){ "p","b","d","\u0288","\u0256","\u0271","n","\u0273"} },
            { "d", new List<string>(){ "p","b","t", "\u0288", "\u0256", "\u0271", "n", "\u0273" } },
            { "\u0288", new List<string>(){ "t","d","\u0256","c","\u025f","n","\u0273","\u0272" } },
            { "\u0256", new List<string>(){ "t","d","\u0256","c","\u025f","n","\u0273","\u0272" } },
            { "c", new List<string>(){ "\u0288","\u0256","\u025f","k","\u0261","\u0273","\u0272","\u014b"} },
            { "\u025f", new List<string>(){ "\u0288", "\u0256", "c", "k", "\u0261", "\u0273", "\u0272", "\u014b" } },
            { "k", new List<string>(){"c","\u025f","\u0261","q","\u0262","\u0272","\u014b","\u0274"} },
            { "\u0261", new List<string>(){"c","\u025f","k","q","\u0262","\u0272","\u041b","\u0274"} },
            { "q", new List<string>(){"k","\u0261","\u0262","\u0294","\u014b","\u0274"} },
            { "\u0262", new List<string>(){"k","\u0261","q","\u0294","\u014b","\u0274"} },
            { "\u0294", new List<string>(){"q","\u0262","\u0274"} },
            // Nasal
            { "m",new List<string>(){"p","b","\u0271","\u0299"} },
            { "\u0271",new List<string>(){"p","b","t","d","m","n","\u0299","r"} },
            { "n", new List<string>(){"\u0271","t","d","\u0288","\u0256","\u0273","r"}},
            { "\u0273", new List<string>(){"n","\u0272","t","d","\u0288","\u0256","c","\u025f","r"} },
            { "\u0272", new List<string>(){"\u0273","\u014b","\u0274","\u0288","\u0256","c","\u025f","k","\u0261","\u0280"} },
            { "\u0274", new List<string>(){"\u0272","k","\u0261","q","\u0262","\u0294"} },
            // Trill
            { "\u0299", new List<string>(){"r","m","\u0271","\u2c71" } },
            { "r", new List<string>(){"\u0299","\u0280","\u0271","n","\u0273","\u2c71","\u027e","\u027d"} },
            { "\u0280", new List<string>(){"r","\u014b"} },
            // Tap or Flap
            { "\u2c71", new List<string>(){"\u027e","\u0299","r","\u0278","\u03b2","f","v","\u03b8","\u00f0" } },
            { "\u027e", new List<string>(){"\u2c71","\u027d","r","\u03b8","\u00f0","s","z","\u0283","\u0292"} },
            { "\u027d", new List<string>(){"\u027e","r","\u0283","\u0292","\u0282","\u0290","\u00e7","\u0291"} },
            // Fricative
            { "\u0278", new List<string>(){"\u03b2","f","v","\u2c71"} },
            { "\u03b2", new List<string>(){"\u0278","f","v","\u2c71"} },
            { "f", new List<string>(){"v","\u0278","\u03b2","\u02b8","\u00f0","\u2c71","\u027e","\u026c","\u026e" } },
            { "v", new List<string>(){"f","\u0278","\u03b2","\u02b8","\u00f0","\u2c71","\u027e", "\u026c", "\u026e" } },
            { "\u02b8", new List<string>(){"\u00f0","f","v","s","z","\u2c71","\u027e", "\u026c", "\u026e" } },
            { "\u00f0", new List<string>(){"\u02b8","f","v","s","z","\u2c71","\u027e", "\u026c", "\u026e" } },
            { "s", new List<string>(){"z","\u02b8","\u00f0","\u0283","\u0292","\u027e","\u027d","\u026c","\u026e"} },
            { "z", new List<string>(){"s","\u02b8","\u00f0","\u0283","\u0292","\u027e","\u027d","\u026c","\u026e"} },
            { "\u0283", new List<string>(){"\u0292","s","z","\u0282","\u0290","\u027e","\u027d","\u026c","\u026e"} },
            { "\u0292", new List<string>(){"\u0283","s","z","\u0282","\u0290","\u027e","\u027d","\u026c","\u026e"} },
            { "\u0282", new List<string>(){"\u0290","\u0283","\u0292","\u00e7","\u029d","\u027e","\u027d" } },
            { "\u0290", new List<string>(){"\u0282","\u0283","\u0292","\u00e7","\u029d", "\u027e", "\u027d" } },
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

        };

        public static String[] PConsonants
        {
            get => _p_consonants;
        }

        public static string[] NpConsonants
        {
            get => _np_consonants;
        }

        public static string[] Vowels
        {
            get => _vowels;
        }

        public static string[] Suprasegmentals
        {
            get => _suprasegmentals;
        }

        public static string[] Diacritics
        {
            get => _diacritics;
        }

        public static Dictionary<string, string> IpaPhonemesMap
        {
            get => _ipaPhonemesMap;
        }

        public static Dictionary<string, string> LatinIpaReplacements
        {
            get => _latinIpaReplacements;
        }

        public static string IpaPhonemes()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string phoneme in IpaPhonemesMap.Keys)
            {
                string description = IpaPhonemesMap[phoneme];
                StringBuilder sb2 = new StringBuilder();
                foreach (char c in phoneme)
                {
                    if (Char.IsAsciiLetterOrDigit(c))
                    {
                        sb2.Append(c);
                    }
                    else
                    {
                        int cInt = (int)c;
                        sb2.AppendFormat("U+{0,4:x4}", cInt);
                    }
                }
                sb.AppendFormat("{0} ({2}):\t{1}\n", phoneme, description, sb2.ToString());
            }

            return sb.ToString();
        }

        public static void SubstituteLatinIpaReplacements(LanguageDescription language)
        {
            language.native_name_phonetic = SubstituteLatinIpaReplacements(language.native_name_phonetic);

            if (language.phoneme_inventory != null)
            {
                List<string> newPhonemeInventory = new List<string>();
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

            if (language.sound_map_list != null)
            {
                foreach (SoundMap soundMap in language.sound_map_list)
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
                                    if (affix.pronounciation_add != null)
                                    {
                                        affix.pronounciation_add = SubstituteLatinIpaReplacements(affix.pronounciation_add);
                                    }
                                    if (affix.pronounciation_regex != null)
                                    {
                                        affix.pronounciation_regex = SubstituteLatinIpaReplacements(affix.pronounciation_regex);
                                    }
                                    if (affix.t_pronounciation_add != null)
                                    {
                                        affix.t_pronounciation_add = SubstituteLatinIpaReplacements(affix.t_pronounciation_add);
                                    }
                                    if (affix.f_pronounciation_add != null)
                                    {
                                        affix.f_pronounciation_add = SubstituteLatinIpaReplacements(affix.f_pronounciation_add);
                                    }
                                    if (affix.pronounciation_repl != null)
                                    {
                                        affix.pronounciation_repl = SubstituteLatinIpaReplacements(affix.pronounciation_repl);
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
                    if (derivationalAffix.pronounciation_regex != null)
                    {
                        derivationalAffix.pronounciation_regex = SubstituteLatinIpaReplacements(derivationalAffix.pronounciation_regex);
                    }
                    if (derivationalAffix.pronounciation_add != null)
                    {
                        derivationalAffix.pronounciation_add = SubstituteLatinIpaReplacements(derivationalAffix.pronounciation_add);
                    }
                    if (derivationalAffix.t_pronounciation_add != null)
                    {
                        derivationalAffix.t_pronounciation_add = SubstituteLatinIpaReplacements(derivationalAffix.t_pronounciation_add);
                    }
                    if (derivationalAffix.f_pronounciation_add != null)
                    {
                        derivationalAffix.f_pronounciation_add = SubstituteLatinIpaReplacements(derivationalAffix.f_pronounciation_add);
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

        public static string SubstituteLatinIpaReplacements(string text)
        {
            string fixedText = text;
            foreach (string bad in LatinIpaReplacements.Keys)
            {
                fixedText = fixedText.Replace(bad, LatinIpaReplacements[bad]);
            }
            return fixedText;
        }

    }
}
