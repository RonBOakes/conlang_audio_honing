/*
 * Class containing utility functions for dealing with Latin characters.
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
using System.Text;
using System.Text.RegularExpressions;

namespace ConlangAudioHoning
{
    /// <summary>
    /// A collection of static properties and methods for working with Latin, 
    /// characters, specifically using Unicode.
    /// </summary>
    internal static class LatinUtilities
    {
        /// <summary>
        /// Returns an array containing strings with all of the commonly used
        /// Latin diacritics.
        /// </summary>
        public static Dictionary<string, string> DiacriticsMap { get; } = new()
        {
            { "\u0300", "combining grave accent" },
            { "\u0301", "combining acute accent" },
            { "\u0302", "combining circumflex accent" },
            { "\u0303", "combining tilde" },
            { "\u0304", "combining macron" },
            { "\u0305", "combining overline" },
            { "\u0306", "combining breve" },
            { "\u0307", "combining dot above" },
            { "\u0308", "combining diaresis" },
            { "\u0309", "combining hook above" },
            { "\u030a", "combining ring above" },
            { "\u030b", "combining double acute accent" },
            { "\u030c", "combining caron" },
            { "\u030d", "combining vertical line above" },
            { "\u030e", "combining double vertical line above" },
            { "\u030f", "combining double grave accent" },
            { "\u0310", "combining candrabindu" },
            { "\u0311", "combining inverted breve" },
            { "\u0312", "combining turned comma above" },
            { "\u0313", "combining comma above" },
            { "\u0314", "combining reversed comma above" },
            { "\u0315", "combining comma above right" },
            { "\u0316", "combining grave accent below" },
            { "\u0317", "combining acute accent below" },
            { "\u0318", "combining left tack below" },
            { "\u0319", "combining right tack below" },
            { "\u031a", "combining left angle above" },
            { "\u031b", "combining horn" },
            { "\u031c", "combining left half ring below" },
            { "\u031d", "combining down tack below" },
            { "\u031e", "combining plus sign below" },
            { "\u0320", "combining minus sign below" },
            { "\u0321", "combining palatized hook below" },
            { "\u0322", "combining retroflex hook below" },
            { "\u0323", "combining dot below" },
            { "\u0324", "combining dieresis below" },
            { "\u0325", "combining ring below" },
            { "\u0326", "combining comma below" },
            { "\u0327", "combining cedilla" },
            { "\u0328", "combining ogonek" },
            { "\u0329", "combining vertical line below" },
            { "\u032a", "combining bridge below" },
            { "\u032b", "combining inverted double arch below" },
            { "\u032c", "combining caron below" },
            { "\u032d", "combining circumflex below" },
            { "\u032e", "combining breve below" },
            { "\u032f", "combining inverted breve below" },
            { "\u0330", "combining tilde below" },
            { "\u0331", "combining macron below" },
            { "\u0332", "combining low line" },
            { "\u0333", "combining double low line" },
            { "\u0334", "combining tilde overlay" },
            { "\u0335", "combining short stroke overlay" },
            { "\u0336", "combining long stroke overlay" },
            { "\u0337", "combining short solidus overlay" },
            { "\u0338", "combining long solidus overlay" },
            { "\u0339", "combining right half ring below" },
            { "\u033a", "combining inverted bridge below" },
            { "\u033b", "combining square below" },
            { "\u033c", "combining seagull below" },
            { "\u033d", "combining x above" },
            { "\u033e", "combining vertical tilde" },
            { "\u033f", "combining double overline" },
            { "\u0340", "combining grave tone mark" },
            { "\u0341", "combining acute tone mark" },
            { "\u0346", "combining bridge above" },
            { "\u0347", "combining equals sign below" },
            { "\u0348", "combining vertical line below" },
            { "\u0349", "combining left angle below" },
            { "\u034a", "combining not tilde above" },
            { "\u034b", "combining homothetic above" },
            { "\u034c", "combining almost equal to above" },
            { "\u034d", "combining left right arrow below"},
            { "\u034e", "combining upwards arrow below" },
            { "\u0350", "combining right arrowhead above" },
            { "\u0351", "combining left half ring above" },
            { "\u0352", "combining fermata" },
            { "\u0353", "combining x below" },
            { "\u0354", "combining left arrowhead below" },
            { "\u0355", "combining right arrow head below" },
            { "\u0356", "combining right arrowhead and up arrowhead below" },
            { "\u0357", "combining right half ring above" },
            { "\u0358", "combining dot above right" },
            { "\u0359", "combining asterisk below" },
            { "\u035a", "combining double ring below" },
            { "\u035b", "combining zigzag above" },
            // Stopping the list here - at least for now.
        };

        /// <summary>
        /// Generate a gloss of the supplied text using the supplied language.<br/>
        /// A gloss is a translation into a common (natural) language in the same order
        /// as the original text, including information about the part of speech and
        /// declension of the word.
        /// </summary>
        /// <param name="sampleText">Text to be glossed</param>
        /// <param name="language">LanguageDescription of the language of the text being glossed.</param>
        /// <param name="caller">Optional link to the LanguageHoningForm that called this method.  If not
        /// null, the Decline method from that form will be used, allowing progress to be displayed.</param>
        /// <returns>A string containing the glossed text.</returns>
        public static string GlossText(string sampleText, ConlangJson.LanguageDescription language, LanguageHoningForm? caller = null)
        {
            StringBuilder sb = new();

            bool removeDeclinedWord = false;
            if (!language.declined)
            {
                if (caller != null)
                {
                    caller.DeclineLexicon(language);
                }
                else
                {
                    ConlangUtilities.declineLexicon(language);
                }
                removeDeclinedWord = true;
            }

            // Build a lookup dictionary from the lexicon - spelled words to their lexicon entries
            Dictionary<string, LexiconEntry> wordMap = [];
            foreach (LexiconEntry entry in language.lexicon)
            {
                wordMap[entry.spelled.Trim().ToLower()] = entry;
            }

            using (StringReader sampleTextReader = new(sampleText))
            {
                string? line;
                do
                {
                    line = sampleTextReader.ReadLine();
                    if ((line != null) && (!line.Trim().Equals(string.Empty)))
                    {
                        foreach (string wordIterator in line.Split(null))
                        {
                            string word = wordIterator.Trim();
                            Match wordMatch = Regex.Match(word, @"(\w+)[.,?!]");
                            if (wordMatch.Success)
                            {
                                word = wordMatch.Groups[1].Value;
                            }
                            if (wordMap.TryGetValue(word, out LexiconEntry? value))
                            {
                                LexiconEntry lexiconEntry = value;
                                _ = sb.AppendFormat("{0}-({1} ", lexiconEntry.english, lexiconEntry.part_of_speech);
                                foreach (string declension in lexiconEntry.declensions)
                                {
                                    _ = sb.AppendFormat("[{0}]", declension);
                                }
                                _ = sb.Append(") ");
                            }
                            else
                            {
                                _ = sb.AppendFormat("Unknown Word ({0}) ", word);
                            }
                        }
                    }
                }
                while (line != null);
            }

            if (removeDeclinedWord)
            {
                ConlangUtilities.removeDeclinedEntries(language);
            }

            return sb.ToString();
        }
    }
}
