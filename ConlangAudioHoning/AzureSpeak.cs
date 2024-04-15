using ConlangJson;
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


        public override void Generate(string speed, LanguageHoningForm? caller = null)
        {
            throw new NotImplementedException();
        }

        public override bool GenerateSpeech(string targetFile, string? voice = null, string? speed = null, LanguageHoningForm? caller = null)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, VoiceData> GetVoices()
        {
            Dictionary<string, VoiceData> voices = [];
            HttpHandler httpHandler = HttpHandler.Instance;
            HttpClient httpClient = httpHandler.HttpClient;
            StringContent content;

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
                sURI,speechRegion,speechKey));
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

            foreach(JsonObject voice in responseData)
            {
                VoiceData voiceData = new VoiceData();
                voiceData.Name = (string)voice["Name"];
                voiceData.Gender = (string)voice["Gender"];
                voiceData.Id = (string)voice["ShortName"];
                voiceData.LanguageCode = (string)voice["LocaleName"];
                voiceData.LanguageName = (string)voice["LocaleName"];
                if (voice.ContainsKey("SecondaryLocaleList"))
                {
                    voiceData.AdditionalLanguageCodes = new string[((JsonArray)voice["SecondaryLocaleList"]).Count];
                    for(int i = 0; i < ((JsonArray)voice["SecondaryLocaleList"]).Count; i++)
                    {
                        voiceData.AdditionalLanguageCodes[i] = (string)((JsonArray)voice["SecondaryLocaleList"])[i];
                    }
                }
                voices.Add(voiceData.Id, voiceData);
            }

            return voices;
        }
    }
}
