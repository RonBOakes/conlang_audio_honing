/*
* Class for Processing speech using espeak-ng.
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
using System.Runtime.InteropServices;
using ESpeakWrapper;
using ConlangJson;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Class used for interacting with espeak-ng for spech generation.
    /// </summary>
    internal class ESpeakNGSpeak
    {
        private static string ESpeakNGLibPath = @"C:\Program Files\eSpeak NG\libespeak-ng.dll";
        LanguageDescription? _languageDescription;
        private string? _sampleText = null;
        private string? _espeakNgText = null;
        private string? _phoneticText = null;

        /// <summary>
        /// Constructor for the ESpeakNGSpeak class.
        /// </summary>
        public ESpeakNGSpeak()
        {
            ESpeakWrapper.Client.Initialize(ESpeakNGSpeak.ESpeakNGLibPath);
            ESpeakWrapper.Client.SetPhonemeEvents(true, false);
        }

        /// <summary>
        /// The LanguageDescription object being worked on.
        /// </summary>
        public LanguageDescription LanguageDescription
        {
            get => _languageDescription ?? new LanguageDescription();
            set => _languageDescription = value;
        }

        /// <summary>
        /// The sample text being worked on.
        /// </summary>
        public string sampleText
        {
            get => _sampleText ?? string.Empty;
            set => _sampleText = value;
        }

        /// <summary>
        /// The SSML text generated to be sent to Amazon Polly.
        /// </summary>
        public string ssmlText
        {
            get => _espeakNgText ?? string.Empty;
        }

        /// <summary>
        /// The Phonetic text produced from the SampleText and the LanguageDescription.
        /// </summary>
        public string phoneticText
        {
            get => _phoneticText ?? string.Empty;
        }

        /// <summary>
        /// Generate the phonetic text and SSML text.
        /// </summary>
        /// <param name="speed">SSML &lt;prosody&gt; speed value to be used in the generated SSML.</param>
        /// <param name="caller">Optional link to the LanguageHoningForm that called this method.  If not
        /// null, the Decline method from that form will be used, allowing progress to be displayed.</param>
        /// <exception cref="ConlangAudioHoningException"></exception>
        public void Generate(string speed, LanguageHoningForm? caller = null)
        {
            if (_languageDescription == null)
            {
                throw new ConlangAudioHoningException("Cannot Generate Polly Speech without a language description");
            }
            if ((_sampleText == null) || (sampleText.Trim().Equals(string.Empty)))
            {
                throw new ConlangAudioHoningException("Cannot Generate polly Speech without sample text");
            }
            bool removeDeclinedWord = false;
            if (!_languageDescription.declined)
            {
                if (caller != null)
                {
                    caller.DeclineLexicon(_languageDescription);
                }
                else
                {
                    ConLangUtilities.declineLexicon(_languageDescription);
                }
                removeDeclinedWord = true;
            }

            // Build a lookup dictionary from the lexicon - spelled words to their lexicon entries
            Dictionary<string, LexiconEntry> wordMap = new Dictionary<string, LexiconEntry>();
            foreach (LexiconEntry entry in _languageDescription.lexicon)
            {
                wordMap[entry.spelled.Trim().ToLower()] = entry;
            }

            // Get the phonetic representations of the text - ported from Python code.
            List<List<Dictionary<string, string>>> pronounceMapList = new List<List<Dictionary<string, string>>>();
            pronounceMapList.Clear();
            using (StringReader sampleTextReader = new StringReader(sampleText))
            {
                string? line;
                do
                {
                    line = sampleTextReader.ReadLine();
                    if ((line != null) && (!line.Trim().Equals(string.Empty)))
                    {
                        List<Dictionary<string, string>> lineMapList = new List<Dictionary<string, string>>();
                        foreach (string word in line.Split(null))
                        {
                            Dictionary<string, string>? pronounceMap = pronounceWord(word, wordMap);
                            if (pronounceMap != null)
                            {
                                lineMapList.Add(pronounceMap);
                            }
                        }
                        pronounceMapList.Add(lineMapList);
                    }
                }
                while (line != null);
            }

            _espeakNgText = string.Empty;

            _phoneticText = string.Empty;
            int wordWrap = 0;

            foreach (List<Dictionary<string, string>> lineMapList in pronounceMapList)
            {
                foreach (Dictionary<string, string> pronounceMap in lineMapList)
                {
                    // Build the text entry
                    _espeakNgText += KirshenbaumUtilities.IpaWordToKirshenbaum(pronounceMap["phonetic"]);
                    _phoneticText += pronounceMap["phonetic"];
                    wordWrap += pronounceMap["phonetic"].Length;
                    if (pronounceMap["punctuation"] != "")
                    {
                        _espeakNgText += pronounceMap["punctuation"];
                        _phoneticText += pronounceMap["punctuation"];
                        wordWrap += pronounceMap["punctuation"].Length;
                    }
                    _phoneticText += " ";
                    wordWrap += 1;
                    if (wordWrap >= 80)
                    {
                        _espeakNgText += "\n";
                        _phoneticText += "\n";
                        wordWrap = 0;
                    }
                }
            }

            if (removeDeclinedWord)
            {
                ConLangUtilities.removeDeclinedEntries(_languageDescription);
            }
        }

        public List<ESpeakWrapper.ESpeakVoice> getVoices()
        {
            List<ESpeakWrapper.ESpeakVoice> voices = new List<ESpeakWrapper.ESpeakVoice>();
            List<IntPtr> voicePointers = new List<IntPtr>();

            unsafe
            {
                void** voicePointerArray = (void**)espeak_ListVoices(IntPtr.Zero);
                void* v;
                for (int ix = 0; (v = voicePointerArray[ix]) != null; ix++)
                {
                    voicePointers.Add((IntPtr)v);
                }
            }

            foreach (IntPtr voicePointer in voicePointers)
            {
                if (voicePointer != IntPtr.Zero)
                {
#pragma warning disable CS8605
                    ESpeakVoice espeakVoice = (ESpeakVoice)Marshal.PtrToStructure(voicePointer, typeof(ESpeakVoice));
#pragma warning restore CS8605
                    voices.Add(espeakVoice);
                }
            }

            return voices;
        }

        public void Test()
        {
            // Initialize the eSpeak-ng library via the wrapper
            if (!ESpeakWrapper.Client.SetVoiceByName("English (America)"))
            {
                return;
            }
            List<string> ipaText = new List<string>()
            {
                "ðɪs", "ɪz", "ɐ", "tˈɛst"
            };
            StringBuilder sb = new StringBuilder();
            foreach (string word in ipaText)
            {
                sb.Append(KirshenbaumUtilities.IpaWordToKirshenbaum(word));
                sb.Append(" ");
            }
            string text = "[[h@'loU]]. This is a test. ";
            text += sb.ToString();
            if (!ESpeakWrapper.Client.Speak(text))
            {
                return;
            }
        }

        private Dictionary<string, string>? pronounceWord(string word, Dictionary<string, LexiconEntry> wordMap)
        {
            if (word.Trim().Equals(string.Empty))
            {
                return null;
            }
            Dictionary<string, string> pw = new Dictionary<string, string>();
            pw.Clear();
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

            int wordLen = word.Length;
            string lastChar = word.Substring(wordLen - 1);
            if ((lastChar == ".") || (lastChar == ","))
            {
                punctuation = lastChar;
                word = word.Substring(0, (wordLen - 1));
            }
            if (wordMap.ContainsKey(word))
            {
                phonetic = wordMap[word].phonetic;
            }
            else
            {
                phonetic = ConLangUtilities.SoundOutWord(word, LanguageDescription.sound_map_list);
                LexiconEntry entry = new LexiconEntry();
                entry.phonetic = phonetic;
                entry.spelled = word;
                entry.english = "<<unknown/undefined word sounded out by the Language Honing Application>>";
                entry.part_of_speech = "unk";
                entry.declensions = new List<string>() { "root" };
                entry.declined_word = false;
                entry.derived_word = false;
                wordMap[word] = entry;
                // Add this word to the lexicon so that it is available for sound changes later.
                LanguageDescription.lexicon.Add(entry);
            }

            pw.Add("phonetic", phonetic);
            pw.Add("punctuation", punctuation);
            pw.Add("word", word);
            return pw;

        }

        // Extra espeakNG Library calls
        [DllImport("libespeak-ng.dll", CharSet = CharSet.Auto)]
        static extern IntPtr espeak_ListVoices(IntPtr voice_spec);

    }
}
