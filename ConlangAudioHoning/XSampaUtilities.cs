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
namespace ConlangAudioHoning
{
    /// <summary>
    /// A collection of static properties and methods for working with X-SAMPA.
    /// </summary>
    internal static class XSampaUtilities
    {
        // Based on https://en.wikipedia.org/wiki/X-SAMPA as of 2024-02-08
        private static readonly Dictionary<string, string> _ipa_x_sampa_map = new()
        {
            { "a", "a" }, { "b", "b" }, { "ɓ", "b_<" }, { "c", "c" }, { "d", "d" }, { "ɖ", "d`" },
            { "ɗ", "d_<" }, { "e", "e" }, { "f", "f" }, { "ɡ", "g" }, { "ɠ", "g_<" }, { "h", "h" },
            { "ɦ", @"h\" }, { "i", "i" }, { "j", "j" }, { "ʝ", @"j\" }, { "k", "k" }, { "l", "l" },
            { "ɭ", "l`" }, { "ɺ", @"l\" }, { "m", "m" }, { "n", "n" }, { "ɳ", "n`" }, { "o", "o" },
            { "p", "p" }, { "ɸ", @"p\" }, { "q", "q" }, { "r", "r" }, { "ɽ", "r`" }, { "ɹ", @"r\" },
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

        private static Dictionary<string, string>? _x_sampa_ipa_map = null;

        /// <summary>
        /// The dictionary used to map IPA characters to their (preferred)
        /// corresponding X-SAMPA character.
        /// </summary>
        public static Dictionary<string, string> IpaXSampaMap
        {
            get => _ipa_x_sampa_map;
        }

        /// <summary>
        /// The Dictionary used to map X-SAMPA characters to their (preferred) 
        /// corresponding IPA character.
        /// </summary>
        public static Dictionary<string, string> XSampaIpaMap
        {
            get
            {
                if (_x_sampa_ipa_map == null)
                {
                    _x_sampa_ipa_map = [];
                    foreach (string ipa in IpaXSampaMap.Keys)
                    {
                        _x_sampa_ipa_map[IpaXSampaMap[ipa]] = ipa;
                    }
                }
                return _x_sampa_ipa_map;
            }
        }
    }
}
