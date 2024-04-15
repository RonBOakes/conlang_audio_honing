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
using ConlangJson;
using System.Text;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Class used for interacting with espeak-ng for speech generation.
    /// </summary>
    internal class NoEngineSpeak : SpeechEngine
    {
        public NoEngineSpeak() : base()
        {
            Description = "None";
        }

        /// <summary>
        /// Constructor for the NoEngineSpeak class.
        /// </summary>
        public NoEngineSpeak(LanguageDescription languageDescription) : base(languageDescription)
        {
            Description = "None";
        }

        public static string ESpeakNGPath { get; set; } = "";

        /// <summary>
        /// Key used to access the LanguageDescription preferred_voices dictionary.
        /// </summary>
        public override string PreferredVoiceKey
        {
            get => "None";
        }

        /// <summary>
        /// Generate the phonetic text and SSML text.
        /// </summary>
        /// <param name="speed">SSML &lt;prosody&gt; speed value to be used in the generated SSML.</param>
        /// <param name="caller">Optional link to the LanguageHoningForm that called this method.  If not
        /// null, the Decline method from that form will be used, allowing progress to be displayed.</param>
        /// <exception cref="ConlangAudioHoningException"></exception>
        public override void Generate(string speed, LanguageHoningForm? caller = null, VoiceData? voiceData = null)
        {
            if (LanguageDescription == null)
            {
                throw new ConlangAudioHoningException("Cannot Generate Polly Speech without a language description");
            }
            if (string.IsNullOrEmpty(SampleText))
            {
                throw new ConlangAudioHoningException("Cannot Generate polly Speech without sample text");
            }
            bool removeDerivedWords = false;
            if (!LanguageDescription.derived)
            {
                ConlangUtilities.DeriveLexicon(LanguageDescription);
                removeDerivedWords = true;
            }

            bool removeDeclinedWord = false;
            if (!LanguageDescription.declined)
            {
                if (caller != null)
                {
                    caller.DeclineLexicon(LanguageDescription);
                }
                else
                {
                    ConlangUtilities.DeclineLexicon(LanguageDescription);
                }
                removeDeclinedWord = true;
            }

            // Build a lookup dictionary from the lexicon - spelled words to their lexicon entries
            Dictionary<string, LexiconEntry> wordMap = [];
            foreach (LexiconEntry entry in LanguageDescription.lexicon)
            {
                wordMap[entry.spelled.Trim().ToLower()] = entry;
            }

            // Get the phonetic representations of the text - ported from Python code.
            List<List<Dictionary<string, string>>> pronounceMapList = [];
            pronounceMapList.Clear();
            using (StringReader sampleTextReader = new(SampleText.ToLower()))
            {
                string? line;
                do
                {
                    line = sampleTextReader.ReadLine();
                    if ((line != null) && (!line.Trim().Equals(string.Empty)))
                    {
                        List<Dictionary<string, string>> lineMapList = [];
                        foreach (string word in line.Split())
                        {
                            Dictionary<string, string>? pronounceMap = PronounceWord(word, wordMap);
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

            PhoneticText = string.Empty;
            int wordWrap = 0;

            foreach (List<Dictionary<string, string>> lineMapList in pronounceMapList)
            {
                foreach (Dictionary<string, string> pronounceMap in lineMapList)
                {
                    // Build the text entry
                    PhoneticText += pronounceMap["phonetic"];
                    wordWrap += pronounceMap["phonetic"].Length;
                    if (pronounceMap["punctuation"] != "")
                    {
                        PhoneticText += pronounceMap["punctuation"];
                        wordWrap += pronounceMap["punctuation"].Length;
                    }
                    PhoneticText += " ";
                    wordWrap += 1;
                    if (wordWrap >= 80)
                    {
                        PhoneticText += "\n";
                        wordWrap = 0;
                    }
                }
            }

            SsmlText = string.Empty;

            if (removeDeclinedWord)
            {
                ConlangUtilities.RemoveDeclinedEntries(LanguageDescription);
            }
            if (removeDerivedWords)
            {
                ConlangUtilities.RemoveDerivedEntries(LanguageDescription);
            }
        }

        /// <summary>
        /// Generate speech and save it in the specified Target File.  
        /// </summary>
        /// <param name="targetFile">String containing the name of the file where the speech audio will be saved.</param>
        /// <param name="voice">Optional: voice to be used.  If empty, either the preferred voice from
        /// the language description or an engine specific default will be used.</param>
        /// <param name="speed">Optional: SSML &lt;prosody&gt; speed value to be used in the generated SSML.
        /// If empty, "slow" will be used.</param>
        /// <param name="caller">LanguageHoningForm used to call this method.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public override bool GenerateSpeech(string targetFile, string? voice = null, string? speed = null, LanguageHoningForm? caller = null)
        {
            return false;
        }


        public override Dictionary<string, VoiceData> GetVoices()
        {
            Dictionary<string, VoiceData> voices = [];
            return voices;
        }

    }
}
