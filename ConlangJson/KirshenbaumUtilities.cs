﻿/*
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
using System.Text;

namespace ConlangJson
{
    /// <summary>
    /// A collection of static properties and methods for working with the Kirshenbaum
    /// ASCII-based phonetics.  These are similar to SAMPA and X-SAMPA, but differ in 
    /// places.  The espeak-ng program uses Kirshenbaum for phonetic input from the
    /// command line.
    /// </summary>
    public static class KirshenbaumUtilities
    {

        /// <summary>
        /// The dictionary used to map IPA characters to their (preferred)
        /// corresponding X-SAMPA character.
        /// </summary>
        public static Dictionary<string, string> IpaKirshenbaumMap { get; } = new()
        {
            { "a", "a" }, { "b", "b" }, { "ɓ", "b`" },{ "c", "c" }, { "d", "d" }, { "ɖ", "d." },
            { "ɗ", "d`" }, { "e", "e" }, { "f", "f" }, { "ɡ", "g" }, { "ɠ", "g`" }, { "h", "h" },
            { "ɦ", @"h<?>" }, { "i", "i" }, { "j", "j" }, { "k", "k" }, { "l", "l" },
            { "ɭ", "l." }, { "ɺ", @"*<lat>" }, { "m", "m" }, { "n", "n" }, { "ɳ", "n." }, { "o", "o" },
            { "p", "p" }, { "ɸ", @"P" }, { "q", "q" }, { "r", "r<trl>" }, { "ɽ", "r." }, { "ɹ", @"r" },
            { "ɻ", @"r." }, { "s", "s" }, { "t", "t" }, { "ʈ", "t." },
            { "u", "u" }, { "v", "v" }, { "ʋ", @"r<lbd>" }, { "w", "w" }, { "x", "x" },
            { "y", "y" }, { "z", "z" }, { "ʐ", "z." }, { "ɑ", "A" }, { "β", "B" },
            { "B", @"b<trl>" }, { "ç", "C" }, { "ð", "D" }, { "ɛ", "E" }, { "ɱ", "M" }, { "ɣ", "Q" },
            { "ɢ", "G" }, { "ʜ", "H" }, { "ɪ", "I" },
            { "ɲ", "n^" }, { "ɟ", "J" }, { "ɬ", "s<lat>" }, { "ɮ", @"z<lat>" }, { "ʎ", "l[" },
            { "ʟ", "L" }, { "ɯ", "u-" }, { "ɰ", @"j<vel>" }, { "ŋ", "N" }, { "ɴ", @"n""" }, { "ɔ", "o-" },
            { "ʁ", "g\"" }, { "ʀ", @"r""" }, { "ʃ", "S" }, { "θ", "T" },
            { "ʊ", "U" }, { "ʌ", "V" }, { "ʍ", "w<vls>" }, { "χ", "g\"" }, { "ħ", "X" },
            { "ʏ", "I." }, { "ʒ", "Z" }, { "ˈ", "'" }, { "ˌ", "," },
            { "ː", "{lng}" }, { "ə", "@" }, { "ɘ", @"@<umd>" }, { "æ", "&" },
            { "ʉ", "u\"" }, { "ɨ", "i\"" }, { "ø", "Y" }, { "ɜ", "V\"" }, { "ɞ", @"O""" }, { "ɾ", "*" },
            { "ɫ", "l~" }, { "ɤ", "o-" }, { "œ", "Y" }, { "ɶ", "a." },
            { "ʔ", "?" }, { "ʕ", @"H<vcd>"},
            { "ǃ", "!" }, { "\u0308", "\"" }, { "\u0329", "`" }, { "\u02bc", "`" }, { "\u02de", "<r>" },
            { "\u0303", "~" }, { "\u032a", "[" }, { "\u02e0", "~" }, { "\u02b0", "<h>" }, { "\u031e", "_o" },
            { "\u0324", "<?>" }, {"ʝ","Z"}, {"ʂ","s."}, {"ɕ","s;" }, {"ɧ", "z;"}, {"ʑ", "z;"},
            { "ɥ", "r<lbd>" }, {"ʄ", "J`"}, {"ʘ", "!"}, {"ɒ","A."}, {"ɚ","@<r>"}, {"ɐ","V\""},
            { "ɵ", "@." }, {"ǀ","!["}, {"‖","!"}, {"ǂ","!"}
        };

        /// <summary>
        /// IPA Phonemes that are not (currently) mapped in Kirshenbaum 
        /// </summary>
        public static List<string> UnmappedPhonemes { get; } = [
              "ʛ",
            "ᵻ",
            "ᵿ",
            ".",
            "ʲ",
            "ˑ",
            "ʢ",
            "ʡ",
            "ꜛ",
            "ꜜ",
            "|",
            "ǁ",
            "‿",
            "\u031f",
            "\u0320",
            "\u030c",
            "\u0325",
            "\u0e24",
            "\u0302",
            "\u032f",
            "\u031a",
            "\u0318",
            "\u033a",
            "\u031c",
            "\u033b",
            "\u033c",
            "\u0339",
            "\u0319",
            "\u0330",
            "\u031d",
            "\u02b7",
        ];

        /// <summary>
        /// Convert a string consisting of a single word represented in IPA into its Kirshenbaum equivalent.
        /// </summary>
        /// <param name="word">Word in IPA representation</param>
        /// <returns>Kirshenbaum equivalent</returns>
        public static string IpaWordToKirshenbaum(string word)
        {
            StringBuilder sb = new();
            _ = sb.Append("[[");

            // All of the keys in both Kirshenbaum maps are 1 character
            foreach (char c in word)
            {
                if (!UnmappedPhonemes.Contains(c.ToString()))
                {
                    string kirshenbaumPhoneme = IpaKirshenbaumMap[c.ToString()];
                    _ = sb.Append(kirshenbaumPhoneme);
                }
            }
            _ = sb.Append("]]");
            return sb.ToString();
        }

    }
}
