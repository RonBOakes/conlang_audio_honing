/*
* Class for Processing speech using Amazon Polly.
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
using System.Text.RegularExpressions;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Abstract Class used for defining the speech engine interface.<br/>
    /// This is an abstract class to allow for sharing methods.
    /// </summary>
    internal abstract class SpeechEngine
    {
        private LanguageDescription? _languageDescription;
        private string? _sampleText = null;
        private string? _ssmlText = null;
        private string? _phoneticText = null;
        private string _description = "";


        /// <summary>
        /// Constructor for the Speech Engine interface.
        /// </summary>
        protected SpeechEngine()
        {
            _languageDescription = null;
        }

        /// <summary>
        /// Constructor for the Speech Engine interface.
        /// </summary>
        /// <param name="languageDescription">LanguageDescription object for the language to work with.</param>
        protected SpeechEngine(LanguageDescription? languageDescription)
        {
            _languageDescription = languageDescription;
        }

        /// <summary>
        /// Gets the description of the speech engine.
        /// </summary>
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_description))
                {
                    throw new NotSupportedException();
                }
                return _description;
            }

            protected set { _description = value; }
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
        public string SampleText
        {
            get => _sampleText ?? string.Empty;
            set => _sampleText = value;
        }

        /// <summary>
        /// The SSML text generated to be sent to Amazon Polly.
        /// </summary>
        public string SsmlText
        {
            get => _ssmlText ?? string.Empty;
            protected set => _ssmlText = value;
        }

        /// <summary>
        /// The Phonetic text produced from the SampleText and the LanguageDescription.
        /// </summary>
        public string PhoneticText
        {
            get => _phoneticText ?? string.Empty;
            protected set => _phoneticText = value;
        }

        /// <summary>
        /// Key used to access the LanguageDescription preferred_voices dictionary.
        /// </summary>
        public abstract string PreferredVoiceKey
        {
            get;
        }

        /// <summary>
        /// Key used in the Conlang JSON structure to index the preferred voice.
        /// </summary>
        public abstract string PreferredVoiceJsonKey
        {
            get;
        }

        /// <summary>
        /// Generate the phonetic text and SSML text.
        /// </summary>
        /// <param name="speed">SSML &lt;prosody&gt; speed value to be used in the generated SSML.</param>
        /// <param name="caller">Optional link to the LanguageHoningForm that called this method.  If not
        /// null, the Decline method from that form will be used, allowing progress to be displayed.</param>
        /// <param name="voiceData">Optional voice data.  Required for some speech engines.</param>
        /// <exception cref="ConlangAudioHoningException"></exception>
        public abstract void Generate(string speed, LanguageHoningForm? caller = null, VoiceData? voiceData = null);

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
        public abstract bool GenerateSpeech(string targetFile, string? voice = null, string? speed = null, LanguageHoningForm? caller = null);

        public abstract Dictionary<string, VoiceData> GetVoices();

        protected Dictionary<string, string>? PronounceWord(string word, Dictionary<string, LexiconEntry> wordMap)
        {
            if (word.Trim().Equals(string.Empty))
            {
                return null;
            }
            Dictionary<string, string> pw = [];
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

            string punctuation = string.Empty;

            word = word.Trim();
            word = word.ToLower();

            int wordLen = word.Length;
            string lastChar = word[(wordLen - 1)..];
            if ((lastChar == ".") || (lastChar == ","))
            {
                punctuation = lastChar;
                word = word[..(wordLen - 1)];
            }

            word = Regex.Replace(word, @"\W+", "");

            string phonetic;
            if (wordMap.TryGetValue(word, out LexiconEntry? value))
            {
                phonetic = value.phonetic;
            }
            else
            {
                phonetic = ConlangUtilities.SoundOutWord(word, LanguageDescription.spelling_pronunciation_rules);
                LexiconEntry entry = new()
                {
                    phonetic = phonetic,
                    spelled = word,
                    english = "<<unknown/undefined word sounded out by the Language Honing Application>>",
                    part_of_speech = "unk",
                    declensions = ["root"],
                    declined_word = false,
                    derived_word = false
                };
                wordMap[word] = entry;
                // Add this word to the lexicon so that it is available for sound changes later.
                LanguageDescription.lexicon.Add(entry);
            }

            pw.Add("phonetic", phonetic);
            pw.Add("punctuation", punctuation);
            pw.Add("word", word);
            return pw;
        }

        /// <summary>
        /// Represents the JSON structure returned by Amazon Web Services when the list of 
        /// Polly voices are requested.  Being used as the generic description of voices.
        /// Unused fields will be set to string.Empty ("").
        /// </summary>
        public struct VoiceData
        {

            /// <summary>
            /// Name of the voice. This provides a human readable voice name that you might display in your application.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gender of the voice.
            /// </summary>
            public string Gender { get; set; }

            /// <summary>
            /// Amazon Polly assigned voice ID. This is the ID that you specify when calling the SynthesizeSpeech operation.
            /// Set to string.Empty for other engines.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Language code of the voice.
            /// </summary>
            public string LanguageCode { get; set; }

            /// <summary>
            /// Human readable name of the language in English.
            /// </summary>
            public string LanguageName { get; set; }

            /// <summary>
            /// Additional codes for languages available for the specified voice in addition to its default language.<br/>
            /// For example, the default language for Aditi is Indian English(en-IN) because it was first used for that 
            /// language.Since Aditi is bilingual and fluent in both Indian English and Hindi, this parameter would show 
            /// the code hi-IN.
            /// </summary>
            public string[] AdditionalLanguageCodes { get; set; }

            /// <summary>
            /// Specifies which engines (standard, neural or long-form) are supported by a given voice.
            /// </summary>
            public string[] SupportedEngines { get; set; }
        }
    }
}
