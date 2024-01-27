/*
* Class for Processing speach using Amazon Polly.
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

namespace ConlangAudioHoning
{
    internal class PollySpeech
    {
        LanguageDescription? _languageDescription;
        string? _sampleText = null;
        string? _ssmlText = null;
        string? _phoneticText = null;

        public PollySpeech(LanguageDescription languageDescription)
        {
            _languageDescription = languageDescription;
        }

        public LanguageDescription LanguageDescription
        {
            get => _languageDescription ??  new LanguageDescription();
            set => _languageDescription = value;
        }

        public string sampleText
        {
            get => _sampleText ?? string.Empty;
            set => _sampleText = value;
        }

        public string ssmlText
        {
            get => _ssmlText ?? string.Empty;
        }

        public string phoneticText
        {
            get => _phoneticText ?? string.Empty;
        }

        public void generate(string voice, string speed)
        {
            if (_languageDescription == null)
            {
                throw new ConlangAudioHoningException("Cannot generate Polly Speech without a language description"); 
            }
            if((_sampleText == null) || (sampleText.Trim().Equals(string.Empty)))
            {
                throw new ConlangAudioHoningException("Cannot generate polly Speech without sample text");
            }

            // Build a lookup dictionary from the lexicon - spelled words to their lexicon entries
            Dictionary<string, LexiconEntry> wordMap = new Dictionary<string, LexiconEntry>();
            foreach (LexiconEntry entry in _languageDescription.lexicon)
            {
                wordMap[entry.spelled.Trim().ToLower()] = entry;
            }

            // Get the phonetic representations of the text - ported from Python code.
            List<List<Dictionary<string, string>>> pronounceMapList = new List<List<Dictionary<string, string>>>();
            using (StringReader sampleTextReader = new StringReader(sampleText))
            {
                string? line;
                do
                {
                    line = sampleTextReader.ReadLine();
                    if(line != null)
                    {
                        List<Dictionary<string,string>> lineMapList = new List<Dictionary<string,string>>();
                        foreach (string word in line.Split(null))
                        {
                            Dictionary<string, string>? pronoucneMap = pronounceWord(word, wordMap);
                            if (pronoucneMap != null)
                            {
                                lineMapList.Add(pronoucneMap);
                            }
                        }
                        pronounceMapList.Add(lineMapList);
                    }
                }
                while (line != null);
            }

            _ssmlText = "<speak>\n";
            _ssmlText = "\t<prosody rate =\"" + speed + "\">\n";

            _phoneticText = string.Empty;
            int wordWrap = 0;

            foreach (List<Dictionary<string,string>> lineMapList in pronounceMapList) 
            {
                foreach(Dictionary<string,string> pronounceMap in lineMapList)
                {
                    // Build the phoneme tag
                    if (!pronounceMap["phonetic"].Trim().Equals(string.Empty))
                    {
                        _ssmlText += "\t\t<phoneme alphabet=\"ipa\" ph=\"" + pronounceMap["phonetic"] + "\">" + pronounceMap["word"] + "</phoneme>\n";
                        _phoneticText += pronounceMap["phonetic"];
                        wordWrap += pronounceMap["phonetic"].Length + 1;
                    }
                    if (pronounceMap["punctuation"] == ".")
                    {
                        _ssmlText += "\t\t<break strength=\"strong\"/>\n";
                        _phoneticText += ". ";
                        wordWrap += 2;
                    }
                    else if (pronounceMap["punctuation"] == ",")
                    {
                        _ssmlText += "\t\t<break strength=\"weak\"/>\n";
                        _phoneticText += ", ";
                        wordWrap += 2;
                    }
                    else
                    {
                        _phoneticText += " ";
                        wordWrap += 1;
                    }
                    if (wordWrap >= 80)
                    {
                        _phoneticText += "\n";
                        wordWrap = 0;
                    }
                }
            }
            _ssmlText += "\t</prosody>\n";
            _ssmlText += "</speak>\n";
        }

        private Dictionary<string,string>? pronounceWord(string word, Dictionary<string, LexiconEntry> wordMap)
        {
            if(word.Trim().Equals(string.Empty))
            {
                return null;
            }
            Dictionary<string, string> pw = new Dictionary<string, string>();
            if (word.Trim().Equals("."))
            {
                
                pw.Add("phonetic", string.Empty);
                pw.Add("punctuation", ".");
                pw.Add("word", word);
            }
            if (word.Trim().Equals(","))
            {
                pw.Add("phonetic", string.Empty);
                pw.Add("punctuation", ",");
                pw.Add("word", word);
            }

            string phonetic = string.Empty;
            string punctuation = string.Empty;

            word = word.Trim();
            word = word.ToLower();

            int wordlen = word.Length;
            string lastchar = word.Substring(wordlen - 1);
            if((lastchar == ".") || (lastchar == ","))
            {
                punctuation = lastchar;
                word = word.Substring((wordlen - 1));
            }
            if(wordMap.ContainsKey(word))
            {
                phonetic = wordMap[word].phonetic;
            }
            else
            {
                phonetic = ConLangUtilities.SoundOutWord(word, LanguageDescription.sound_map_list);
                LexiconEntry entry = new LexiconEntry();
                entry.phonetic = phonetic;
                entry.spelled = word;
                wordMap[word] = entry;
            }

            pw.Add("phonetic", phonetic);
            pw.Add("punctuation", punctuation);
            pw.Add("word", word);
            return pw;

         }
    }
}
