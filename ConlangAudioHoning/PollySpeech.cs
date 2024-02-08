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
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ConlangAudioHoning
{
    internal class PollySpeech
    {
        LanguageDescription? _languageDescription;
        private string? _sampleText = null;
        private string? _ssmlText = null;
        private string? _phoneticText = null;

        // URI for the Amazon REST URI for processing the Polly messages.  This needs beefing up
        // before this project can be made public since it will allow any SSML to be turned to 
        // speech on my dime.
        private static string _polyURI = "https://9ggv18yii2.execute-api.us-east-1.amazonaws.com/general_speak2";

        public PollySpeech(LanguageDescription languageDescription)
        {
            _languageDescription = languageDescription;
        }

        public LanguageDescription LanguageDescription
        {
            get => _languageDescription ?? new LanguageDescription();
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

            _ssmlText = "<speak>\n";
            _ssmlText += "\t<prosody rate =\"" + speed + "\">\n";

            _phoneticText = string.Empty;
            int wordWrap = 0;

            foreach (List<Dictionary<string, string>> lineMapList in pronounceMapList)
            {
                foreach (Dictionary<string, string> pronounceMap in lineMapList)
                {
                    // Build the phoneme tag
                    if ((!pronounceMap["phonetic"].Trim().Equals(string.Empty)) && (pronounceMap["phonetic"] != ".") && (pronounceMap["phonetic"] != ","))
                    {
                        _ssmlText += "\t\t<phoneme alphabet=\"ipa\" ph=\"" + pronounceMap["phonetic"] + "\">" + pronounceMap["word"] + "</phoneme>\n";
                        _phoneticText += pronounceMap["phonetic"];
                        wordWrap += pronounceMap["phonetic"].Length;
                        if (pronounceMap["punctuation"] != "")
                        {
                            _phoneticText += pronounceMap["punctuation"];
                            wordWrap += pronounceMap["punctuation"].Length;
                        }
                        _phoneticText += " ";
                        wordWrap += 1;
                    }
                    else if (pronounceMap["punctuation"] == ".")
                    {
                        _ssmlText += "\t\t<break strength=\"strong\"/>\n";
                    }
                    else if (pronounceMap["punctuation"] == ",")
                    {
                        _ssmlText += "\t\t<break strength=\"weak\"/>\n";
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

            if (removeDeclinedWord)
            {
                ConLangUtilities.removeDeclinedEntries(_languageDescription);
            }
        }

        public struct VoiceData
        {
            private string _name;
            private string _gender;
            private string _id;
            private string _languageCode;
            private string _languageName;
            private string[] _additionalLanguageCodes;
            private string[] _supportedEngines;
            public string Name
            {
                get => _name;
                set => _name = value;
            }
            public string Gender
            {
                get => _gender;
                set => _gender = value;
            }
            public string Id
            {
                get => _id; 
                set => _id = value;
            }
            public string LanguageCode
            {
                get => _languageCode; 
                set => _languageCode = value;
            }
            public string LanguageName
            {
                get => _languageName; 
                set => _languageName = value;
            }
            public string[] AdditionalLanguageCodes
            {
                get => _additionalLanguageCodes; 
                set => _additionalLanguageCodes = value;
            }
            public string[] SupportedEngines
            {
                get => _supportedEngines; 
                set => _supportedEngines = value;
            }
        }

        public static Dictionary<string, PollySpeech.VoiceData> getAmazonPollyVoices()
        {
            Dictionary<string, PollySpeech.VoiceData> voices = new Dictionary<string, PollySpeech.VoiceData>();
            HttpHandler httpHandler = HttpHandler.Instance;
            HttpClient httpClient = httpHandler.httpClient;
            StringContent content;

            Dictionary<string, string> requestDict = new Dictionary<string, string>();
            requestDict["systemStatus"] = string.Empty;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = httpClient.PostAsync(_polyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return voices;
                }
            }
            requestDict.Clear();
            requestDict["requestVoices"] = string.Empty;
            JsonObject? responseData = null;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = httpClient.PostAsync(_polyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return voices;
                }
                string data = result.Content.ReadAsStringAsync().Result;
                if (!String.IsNullOrEmpty(data))
                {
                    responseData = JsonSerializer.Deserialize<JsonObject>(data);
                }
            }
            if(responseData == null)
            {
                return voices;
            }
           
            if(responseData.ContainsKey("Voices"))
            {
                JsonNode? voiceDataNode;
                if ((responseData.TryGetPropertyValue("Voices", out voiceDataNode)) && (voiceDataNode != null))
                {
                    JsonArray voiceData = voiceDataNode.AsArray();
                    foreach(JsonNode? voiceDatum in voiceData)
                    {
                        if(voiceDatum != null)
                        {
                            // Run the AWS output through JSON serializer to convert into the structure
                            VoiceData data = JsonSerializer.Deserialize<VoiceData>(voiceDatum);
                            voices.Add(data.Name, data);
                        }
                    }
                }
            }
           


             return voices;
        }

        public bool GenerateSpeech(string targetFile, string? voice = null, string? speed = null, LanguageHoningForm? caller = null)
        {
            if (_languageDescription == null)
            {
                return false;
            }
            if ((_sampleText == null) || (_sampleText.Length == 0))
            {
                return false;
            }

            if(string.IsNullOrEmpty(voice))
            {
                voice = _languageDescription.preferred_voice ?? "Brian";
            }
            if(string.IsNullOrEmpty(speed)) 
            {
                speed = "slow";
            }

            if ((_ssmlText == null) || (_ssmlText.Trim().Equals(string.Empty)))
            {
                Generate(speed,caller);
            }

            bool generated = false;
            HttpHandler httpHandler = HttpHandler.Instance;
            HttpClient httpClient = httpHandler.httpClient;
            StringContent content;

            ProgressBar? progressBar;
            if(caller != null)
            {
                progressBar = caller.ProgressBar;
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Minimum = 0;
                progressBar.Maximum = 5;
                progressBar.Value = 0;
                progressBar.Step = 1;
                progressBar.Visible = true;
                progressBar.BringToFront();
            }
            else
            {
                progressBar = null;
            }


            Dictionary<string, string> requestDict = new Dictionary<string, string>();
            requestDict["systemStatus"] = string.Empty;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = httpClient.PostAsync(_polyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }
            }
            if(progressBar != null)
            {
                progressBar.Value = 1;
            }

            requestDict.Clear();
            requestDict["start"] = string.Empty;
#pragma warning disable CS8601 // Possible null reference assignment.
            requestDict["ssml"] = _ssmlText;
#pragma warning restore CS8601 // Possible null reference assignment.            }
            requestDict["voice"] = voice;
            requestDict["filetype"] = "mp3";
            string taskId;
            string taskURI;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = httpClient.PostAsync(_polyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }
                string data = result.Content.ReadAsStringAsync().Result;
#pragma warning disable CS8600 // Possible null reference assignment.
#pragma warning disable CS8602 // Possible null reference assignment.
                Dictionary<string, string> resultData = JsonSerializer.Deserialize<Dictionary<string, string>>(data);
                taskId = resultData["task_id"];
                taskURI = resultData["uri"];
#pragma warning restore CS8602 // Possible null reference assignment.            }
#pragma warning restore CS8600 // Possible null reference assignment.
            }
            if(progressBar != null)
            {
                progressBar.Value = 2;
            }

            requestDict.Clear();
            requestDict["taskStatus"] = string.Empty;
            requestDict["task_id"] = taskId;
            bool complete = false;
            do
            {
                using(content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage result = httpClient.PostAsync(_polyURI, content).Result;
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }
                    string data = result.Content.ReadAsStringAsync().Result;
                    if(data.Contains("FAILED"))
                    {
                        return false;
                    }
                    if(data.Contains("COMPLETE"))
                    {
                        complete = true;
                    }
                    if(progressBar != null)
                    {
                        progressBar.Value = 3;
                    }
                }
                Thread.Sleep(5000);

            }
            while(!complete);

            requestDict.Clear();
            requestDict["downloadFile"] = string.Empty;
            requestDict["uri"] = taskURI;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = httpClient.PostAsync(_polyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }
                byte[] data = result.Content.ReadAsByteArrayAsync().Result;
                File.WriteAllBytes(targetFile, data);
                generated = true;
            }
            if(progressBar != null)
            {
                progressBar.Value = 4;
            }

            requestDict.Clear();
            requestDict["delete"] = string.Empty;
            requestDict["uri"] = taskURI;
            // requestDict["uri"] = string.Empty; // Remove or comment out to just delete the newly created file.
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = httpClient.PostAsync(_polyURI, content).Result;
            }
            if(progressBar != null)
            {
                progressBar.Value = 5;
                progressBar.Visible = false;
                progressBar.SendToBack();
            }

            return generated;
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
    }
}
