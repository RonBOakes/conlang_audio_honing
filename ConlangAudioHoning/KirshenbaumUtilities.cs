/*
* Tools and utilities for working with X-SAMPA phonetic coding.
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

namespace ConlangAudioHoning
{
    /// <summary>
    /// A collection of static properties and methods for working with the Kirshenbaum
    /// ASCII-based phonetics.  These are similar to SAMPA and X-SAMPA, but differ in 
    /// places.  The espeak-ng program uses Kirshenbaum for phonetic input from the
    /// command line.
    /// </summary>
    internal static class KirshenbaumUtilities
    {
        // Based on https://web.archive.org/web/20160312220739/http://www.kirshenbaum.net/IPA/faq.html
        // and https://github.com/espeak-ng/espeak-ng/blob/master/docs/phonemes/kirshenbaum.md as of 
        // commit cd17e7c.
        private static Dictionary<string, string> _ipa_kirshenbaum_map = new Dictionary<string, string>()
        {
            { "a", "a" }, { "b", "b" }, { "ɓ", "b`" },{ "c", "c" }, { "d", "d" }, { "ɖ", "d." },
            { "ɗ", "d`" }, { "e", "e" }, { "f", "f" }, { "ɡ", "g" }, { "ɠ", "g`" }, { "h", "h" },
            { "ɦ", @"h<?>" }, { "i", "i" }, { "j", "j" }, { "k", "k" }, { "l", "l" },
            { "ɭ", "l." }, { "ɺ", @"*<lat>" }, { "m", "m" }, { "n", "n" }, { "ɳ", "n." }, { "o", "o" },
            { "p", "p" }, { "ɸ", @"P" }, { "q", "q" }, { "r", "r<trl>" }, { "ɽ", "r." }, { "ɹ", @"r" },
            // Stopping point 2024-02-14 16:55 ^^^
            { "ɻ", @"r\`" }, { "s", "s" }, { "ʂ", "s`" }, { "ɕ", @"s\" }, { "t", "t" }, { "ʈ", "t`" },
            { "u", "u" }, { "v", "v" }, { "ʋ", @"v\" }, { "w", "w" }, { "x", "x" }, { "ɧ", @"x\" },
            { "y", "y" }, { "z", "z" }, { "ʐ", "z`" }, { "ʑ", @"z\" }, { "ɑ", "A" }, { "β", "B" },
            { "B", @"B\" }, { "ç", "C" }, { "ð", "D" }, { "ɛ", "E" }, { "ɱ", "F" }, { "ɣ", "G" },
            { "ɢ", @"G\" }, { "ʛ", @"G\<" }, { "ɥ", "H" }, { "ʜ", @"H\" }, { "ɪ", "I" }, { "ᵻ", @"I\" },
            { "ɲ", "J" }, { "ɟ", @"J\" }, { "ʄ", @"J\_<" }, { "ɬ", "K" }, { "ɮ", @"K\" }, { "ʎ", "L" },
            { "ʟ", @"L\" }, { "ɯ", "M" }, { "ɰ", @"M\" }, { "ŋ", "N" }, { "ɴ", @"N\" }, { "ɔ", "O" },
            { "ʘ", @"O\" }, { "ɒ", "Q" }, { "ʁ", "R" }, { "ʀ", @"R\" }, { "ʃ", "S" }, { "θ", "T" },
            { "ʊ", "U" }, { "ᵿ", @"U\" }, { "ʌ", "V" }, { "ʍ", "W" }, { "χ", "X" }, { "ħ", @"X\" },
            { "ʏ", "Y" }, { "ʒ", "Z" }, { ".", "." }, { "ˈ", "\"" }, { "ˌ", "%" }, { "ʲ", "'" },
            { "ː", ":" }, { "ˑ", @":\" }, { "ə", "@" }, { "ɘ", @"@\" }, { "ɚ", "@`" }, { "æ", "{" },
            { "ʉ", "}" }, { "ɨ", "1" }, { "ø", "2" }, { "ɜ", "3" }, { "ɞ", @"3\" }, { "ɾ", "4" },
            { "ɫ", "5" }, { "ɐ", "6" }, { "ɤ", "7" }, { "ɵ", "8" }, { "œ", "9" }, { "ɶ", "&" },
            { "ʔ", "?" }, { "ʕ", @"?\"}, { "ʢ", @"<\" }, { "ʡ", @">\"}, { "ꜛ", "^" }, { "ꜜ", "!" },
            { "ǃ", @"!\" }, { "|", "|" }, { "ǀ", @"|\" }, { "‖", "||" }, { "ǁ", @"|\|\" },
            { "ǂ", @"=\" }, { "‿", @"-\" }, { "\u0308", "_\"" }, { "\u031f", "_+" }, { "\u0320", "_-" },
            { "\u030c", @"_/" }, { "\u0325", "_0" }, { "\u0329", "_<" }, { "\u02bc", "_>" },
            { "\u0e24", @"_?\" }, { "\u0302", @"_\" }, { "\u032f", "_^" }, { "\u031a", "_}" },
            { "\u02de", "`" }, { "\u0303", "~" }, { "\u0318", "_A" }, { "\u033a", "_a" },
            { "\u031c", "_c" }, { "\u032a", "_d" }, { "\u02e0", "_G" }, { "\u02b0", "_h" },
            { "\u033b", "_m"}, { "\u033c", "_N" }, { "\u0339", "_O" }, { "\u031e", "_o" }, 
            { "\u0319", "_q" }, { "\u0330", "_k" }, { "\u031d", "_r" }, { "\u0324", "_t" }, 
            { "\u02b7", "_w" },
        };

        private static List<String> _unmapped_phonemes = new List<string>()
        {
            "ʝ", 
        };

        private static Dictionary<string, string>? _kirshenbaum_ipa_map = null;

        /// <summary>
        /// The dictionary used to map IPA characters to their (preferred)
        /// corresponding X-SAMPA character.
        /// </summary>
        public static Dictionary<string, string> IpaKirshenbaumMap
        {
            get => _ipa_kirshenbaum_map;
        }

        /// <summary>
        /// The Dictionary used to map X-SAMPA characters to their (preferred) 
        /// corresponding IPA character.
        /// </summary>
        public static Dictionary<string,string> KirshenbaumIpaMap
        {
            get
            {
                if(_kirshenbaum_ipa_map == null)
                {
                    _kirshenbaum_ipa_map= new Dictionary<string, string>();
                    foreach(string ipa in IpaKirshenbaumMap.Keys)
                    {
                        _kirshenbaum_ipa_map[IpaKirshenbaumMap[ipa]] = ipa;
                    }
                }
                return _kirshenbaum_ipa_map;
            }
        }
    }
}
