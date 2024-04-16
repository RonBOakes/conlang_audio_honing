using ConlangJson;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    internal class AzureSpeak : SpeechEngine
    {
        private string speechKey;
        private string speechRegion;
        private Dictionary<string, VoiceData> voices = [];

        public AzureSpeak() : base()
        {
            Description = "azure";

            speechKey = UserConfiguration.LoadCurrent().AzureSpeechKey;
            speechRegion = UserConfiguration.LoadCurrent().AzureSpeechRegion;
        }

        /// <summary>
        /// Constructor for the ESpeakNGSpeak class.
        /// </summary>
        public AzureSpeak(LanguageDescription languageDescription) : base(languageDescription)
        {
            Description = "azure";

            speechKey = UserConfiguration.LoadCurrent().AzureSpeechKey;
            speechRegion = UserConfiguration.LoadCurrent().AzureSpeechRegion;
        }

        /// <summary>
        /// Key used to access the LanguageDescription preferred_voices dictionary.
        /// </summary>
        public override string PreferredVoiceKey
        {
            get => "azure";
        }


        /// <summary>
        /// Generate the phonetic text and SSML text.
        /// </summary>
        /// <param name="speed">SSML &lt;prosody&gt; speed value to be used in the generated SSML.</param>
        /// <param name="caller">Optional link to the LanguageHoningForm that called this method.  If not
        /// null, the Decline method from that form will be used, allowing progress to be displayed.</param>
        /// <exception cref="ConlangAudioHoningException"></exception>
        /// <param name="voiceData">Option reference to the voiceData for the preferred voice (if set) </param>
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

            SsmlText = "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xmlns:emo=\"http://www.w3.org/2009/10/emotionml\" version=\"1.0\" xml:lang=\"en-US\">\n";
            if (voiceData == null)
            {
                SsmlText += "\t<voice name=\"en-US-RyanMultilingualNeural\">\n";
            }
            else
            {
                SsmlText += "\t<voice name=\"" + voiceData?.Id + "\">\n";
            }
            SsmlText += "\t\t<prosody rate=\"" + speed + "\">\n";
            SsmlText += "\t\t\t<lang xml:lang=\"" + voiceData?.LanguageCode + "\">\n";

            // Write an English introduction paragraph since reading of a book worked OK (this will get read by the language - maybe mangled)
            //SsmlText += "\t\t\t\t<p>The following is read in the users constructed language</p>\n";

            PhoneticText = string.Empty;
            int wordWrap = 0;

            foreach (List<Dictionary<string, string>> lineMapList in pronounceMapList)
            {
                foreach (Dictionary<string, string> pronounceMap in lineMapList)
                {
                    // Build the phoneme tag
                    if ((!pronounceMap["phonetic"].Trim().Equals(string.Empty)) && (pronounceMap["phonetic"] != ".") && (pronounceMap["phonetic"] != ","))
                    {
                        SsmlText += "\t\t\t\t<phoneme alphabet=\"ipa\" ph=\"" + pronounceMap["phonetic"] + "\">" + pronounceMap["word"] + "</phoneme>\n";
                        PhoneticText += pronounceMap["phonetic"];
                        wordWrap += pronounceMap["phonetic"].Length;
                        if (pronounceMap["punctuation"] != "")
                        {
                            PhoneticText += pronounceMap["punctuation"];
                            wordWrap += pronounceMap["punctuation"].Length;
                        }
                        PhoneticText += " ";
                        wordWrap += 1;
                    }
                    else if (pronounceMap["punctuation"] == ".")
                    {
                        SsmlText += "\t\t\t\t<break strength=\"strong\"/>\n";
                    }
                    else if (pronounceMap["punctuation"] == ",")
                    {
                        SsmlText += "\t\t\t\t<break strength=\"weak\"/>\n";
                    }
                    if (wordWrap >= 80)
                    {
                        PhoneticText += "\n";
                        wordWrap = 0;
                    }
                }
            }
            SsmlText += "\t\t\t</lang>";
            SsmlText += "\t\t</prosody>\n";
            SsmlText += "\t</voice>\n";
            SsmlText += "</speak>\n";

            if (removeDeclinedWord)
            {
                ConlangUtilities.RemoveDeclinedEntries(LanguageDescription);
            }
            if (removeDerivedWords)
            {
                ConlangUtilities.RemoveDerivedEntries(LanguageDescription);
            }
        }

        public override bool GenerateSpeech(string targetFile, string? voice = null, string? speed = null, LanguageHoningForm? caller = null)
        {
            if (LanguageDescription == null)
            {
                return false;
            }
            if ((SampleText == null) || (SampleText.Length == 0))
            {
                return false;
            }
            if (string.IsNullOrEmpty(speechKey) || string.IsNullOrEmpty(speechRegion))
            {
                return false;
            }
            bool ok = true;

            if (string.IsNullOrEmpty(voice))
            {
                voice = LanguageDescription.preferred_voices["azure"] ?? "en-US-AndrewNeural";
            }
            if (string.IsNullOrEmpty(speed))
            {
                speed = "slow";
            }

            if ((SsmlText == null) || (SsmlText.Trim().Equals(string.Empty)))
            {
                Generate(speed, caller, voices[voice]);
            }

            SpeechConfig speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            AudioConfig audioConfig = AudioConfig.FromDefaultSpeakerOutput();

            // Start configuring the speech engine
            speechConfig.SpeechSynthesisVoiceName = voice;
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(speechConfig, audioConfig);

            SpeechSynthesisResult speechSynthesisResult = speechSynthesizer.SpeakSsmlAsync(SsmlText).GetAwaiter().GetResult();

            if ((speechSynthesisResult != null) && (speechSynthesisResult.Reason == ResultReason.SynthesizingAudioCompleted))
            {
                AudioDataStream stream = AudioDataStream.FromResult(speechSynthesisResult);
                stream.SaveToWaveFileAsync(targetFile);
            }
            else
            {
                ok = false;
            }

            return ok;
        }

        public override Dictionary<string, VoiceData> GetVoices()
        {
            voices = [];
            HttpHandler httpHandler = HttpHandler.Instance;
            HttpClient httpClient = httpHandler.HttpClient;

            if (string.IsNullOrEmpty(speechKey))
            {
                speechKey = UserConfiguration.LoadCurrent().AzureSpeechKey;
            }
            if (string.IsNullOrEmpty(speechRegion))
            {
                speechRegion = UserConfiguration.LoadCurrent().AzureSpeechRegion;
            }

            string sURI = string.Format("https://{0}.tts.speech.microsoft.com/cognitiveservices/voices/list", speechRegion);
            JsonArray? responseData = null;
            Uri uri = new Uri(string.Format("{0}?Host={1}.tts.speech.microsoft.com&Ocp-Apim-Subscription-Key={2}",
                sURI, speechRegion, speechKey));
            HttpResponseMessage result = httpClient.GetAsync(uri).Result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return voices;
            }
            string data = result.Content.ReadAsStringAsync().Result;
            if (!String.IsNullOrEmpty(data))
            {
                responseData = JsonSerializer.Deserialize<JsonArray>(data);
            }

            if (responseData == null)
            {
                return voices;
            }

            foreach (JsonNode? voiceNode in responseData)
            {
                if (voiceNode is JsonObject)
                {
                    JsonObject voice = (JsonObject)voiceNode;
                    VoiceData voiceData = new VoiceData();
                    voiceData.Name = (string)voice["Name"];
                    voiceData.Gender = (string)voice["Gender"];
                    voiceData.Id = (string)voice["ShortName"];
                    voiceData.LanguageCode = (string)voice["LocaleName"];
                    voiceData.LanguageName = (string)voice["LocaleName"];
                    if (voice.ContainsKey("SecondaryLocaleList"))
                    {
                        voiceData.AdditionalLanguageCodes = new string[((JsonArray)voice["SecondaryLocaleList"]).Count];
                        for (int i = 0; i < ((JsonArray)voice["SecondaryLocaleList"]).Count; i++)
                        {
                            voiceData.AdditionalLanguageCodes[i] = (string)((JsonArray)voice["SecondaryLocaleList"])[i];
                        }
                    }
                    voices.Add(voiceData.Id, voiceData);
                }
            }

            return voices;
        }
    }
}
