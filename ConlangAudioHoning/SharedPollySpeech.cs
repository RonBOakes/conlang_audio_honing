﻿/*
* Class for Processing speech using Amazon Polly via the shared REST interface.
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
using System.Text.Json;
using System.Text.Json.Nodes;
using ConlangJson;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Class used for interacting with Amazon Polly for speech generation and related operations.
    /// </summary>
    internal class SharedPollySpeech : SpeechEngine
    {

        /// <summary>
        /// Constructor for the Amazon Polly interface.
        /// </summary>
        public SharedPollySpeech() : base()
        {
            Description = "Amazon Polly";
        }

        /// <summary>
        /// Constructor for the Amazon Polly interface.
        /// </summary>
        /// <param name="languageDescription">LanguageDescription object for the language to work with.</param>
        public SharedPollySpeech(LanguageDescription languageDescription) : base(languageDescription)
        {
            Description = "Amazon Polly";
        }

        public static string PollyURI { get; set; } = "";

        public static string PollyEmail { get; set; } = "";

        public static string PollyPassword { get; set; } = "";

        /// <summary>
        /// Key used to access the LanguageDescription preferred_voices dictionary.
        /// </summary>
        public override string PreferredVoiceKey
        {
            get => "Polly";
        }

        /// <summary>
        /// Key used in the Conlang JSON structure to index the preferred voice.
        /// </summary>
        public override string PreferredVoiceJsonKey
        {
            get => "Polly";
        }

        /// <summary>
        /// Generate the phonetic text and SSML text.
        /// </summary>
        /// <param name="speed">SSML &lt;prosody&gt; speed value to be used in the generated SSML.</param>
        /// <param name="caller">Optional link to the LanguageHoningForm that called this method.  If not
        /// null, the Decline method from that form will be used, allowing progress to be displayed.</param>
        /// <param name="voiceData">Optional Voice Data, unused.</param>
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
            Cursor.Current = Cursors.WaitCursor;
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
            using (StringReader sampleTextReader = new(IpaUtilities.IpaSafeLowerCase(SampleText)))
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

            SsmlText = "<speak>\n";
            SsmlText += "\t<prosody rate =\"" + speed + "\">\n";

            PhoneticText = string.Empty;
            int wordWrap = 0;

            foreach (List<Dictionary<string, string>> lineMapList in pronounceMapList)
            {
                foreach (Dictionary<string, string> pronounceMap in lineMapList)
                {
                    // Build the phoneme tag
                    if ((!pronounceMap["phonetic"].Trim().Equals(string.Empty)) && (pronounceMap["phonetic"] != ".") && (pronounceMap["phonetic"] != ","))
                    {
                        SsmlText += "\t\t<phoneme alphabet=\"ipa\" ph=\"" + pronounceMap["phonetic"] + "\">" + pronounceMap["word"] + "</phoneme>\n";
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
                        SsmlText += "\t\t<break strength=\"strong\"/>\n";
                    }
                    else if (pronounceMap["punctuation"] == ",")
                    {
                        SsmlText += "\t\t<break strength=\"weak\"/>\n";
                    }
                    if (wordWrap >= 80)
                    {
                        PhoneticText += "\n";
                        wordWrap = 0;
                    }
                }
            }
            SsmlText += "\t</prosody>\n";
            SsmlText += "</speak>\n";

            if (removeDeclinedWord)
            {
                ConlangUtilities.RemoveDeclinedEntries(LanguageDescription);
            }
            if (removeDerivedWords)
            {
                ConlangUtilities.RemoveDerivedEntries(LanguageDescription);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Get all of the Amazon Polly voices from Amazon Web Services.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, SharedPollySpeech.VoiceData> GetVoices()
        {
            Dictionary<string, SharedPollySpeech.VoiceData> voices = [];
            HttpHandler httpHandler = HttpHandler.Instance;
            HttpClient httpClient = httpHandler.HttpClient;
            StringContent content;

            Dictionary<string, string> requestDict = new()
            {
                ["systemStatus"] = string.Empty
            };
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                content.Headers.Add("authorization_email", PollyEmail);
                content.Headers.Add("authorization_password", PollyPassword);
                HttpResponseMessage result = httpClient.PostAsync(PollyURI, content).Result;
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
                content.Headers.Add("authorization_email", PollyEmail);
                content.Headers.Add("authorization_password", PollyPassword);
                HttpResponseMessage result = httpClient.PostAsync(PollyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return voices;
                }
                string data = result.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(data))
                {
                    responseData = JsonSerializer.Deserialize<JsonObject>(data);
                }
            }
            if (responseData == null)
            {
                return voices;
            }

            if (responseData.ContainsKey("Voices") && (responseData.TryGetPropertyValue("Voices", out JsonNode? voiceDataNode)) && (voiceDataNode != null))
            {
                JsonArray voiceData = voiceDataNode.AsArray();
                foreach (JsonNode? voiceDatum in voiceData)
                {
                    if (voiceDatum != null)
                    {
                        // Run the AWS output through JSON serializer to convert into the structure
                        VoiceData data = JsonSerializer.Deserialize<VoiceData>(voiceDatum);
                        voices.Add(data.Name, data);
                    }
                }
            }

            return voices;
        }

        /// <summary>
        /// Generate Amazon Polly Speech and save it in the specified Target File.  
        /// </summary>
        /// <param name="targetFile">String containing the name of the file where the speech audio will be saved.</param>
        /// <param name="voice">Optional: Amazon Polly Voice to be used.  If empty, either the preferred voice from
        /// the language description or "Brian" will be used.</param>
        /// <param name="speed">Optional: SSML &lt;prosody&gt; speed value to be used in the generated SSML.
        /// If empty, "slow" will be used.</param>
        /// <param name="caller">LanguageHoningForm used to call this method.  If not null, this method
        /// will display the progress of getting the speech from Amazon Web Services using that form's 
        /// ProgressBar.</param>
        /// <returns>true if successful, false otherwise.</returns>
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

            if (string.IsNullOrEmpty(voice))
            {
                voice = LanguageDescription.preferred_voices["Polly"] ?? "Brian";
            }
            if (string.IsNullOrEmpty(speed))
            {
                speed = "slow";
            }

            if ((SsmlText == null) || (SsmlText.Trim().Equals(string.Empty)))
            {
                Generate(speed, caller);
            }

            Cursor.Current = Cursors.WaitCursor;

            bool generated = false;
            HttpHandler httpHandler = HttpHandler.Instance;
            HttpClient httpClient = httpHandler.HttpClient;
            StringContent content;

            ProgressBar? progressBar;
            if (caller != null)
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


            Dictionary<string, string> requestDict = new()
            {
                ["systemStatus"] = string.Empty
            };
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                content.Headers.Add("authorization_email", PollyEmail);
                content.Headers.Add("authorization_password", PollyPassword);
                HttpResponseMessage result = httpClient.PostAsync(PollyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Cursor.Current = Cursors.Default;
                    return false;
                }
            }
            if (progressBar != null)
            {
                progressBar.Value = 1;
            }

            requestDict.Clear();
            requestDict["start"] = string.Empty;
#pragma warning disable CS8601 // Possible null reference assignment.
            requestDict["ssml"] = SsmlText;
#pragma warning restore CS8601 // Possible null reference assignment.            }
            requestDict["voice"] = voice;
            requestDict["filetype"] = "mp3";
            string taskId;
            string taskURI;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                content.Headers.Add("authorization_email", PollyEmail);
                content.Headers.Add("authorization_password", PollyPassword);
                HttpResponseMessage result = httpClient.PostAsync(PollyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Cursor.Current = Cursors.Default;
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
            if (progressBar != null)
            {
                progressBar.Value = 2;
            }

            requestDict.Clear();
            requestDict["taskStatus"] = string.Empty;
            requestDict["task_id"] = taskId;
            bool complete = false;
            do
            {
                using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
                {
                    content.Headers.Add("authorization_email", PollyEmail);
                    content.Headers.Add("authorization_password", PollyPassword);
                    HttpResponseMessage result = httpClient.PostAsync(PollyURI, content).Result;
                    if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    string data = result.Content.ReadAsStringAsync().Result;
                    if (data.Contains("FAILED"))
                    {
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    if (data.Contains("COMPLETE"))
                    {
                        complete = true;
                    }
                    if (progressBar != null)
                    {
                        progressBar.Value = 3;
                    }
                }
                Thread.Sleep(5000);

            }
            while (!complete);

            requestDict.Clear();
            requestDict["downloadFile"] = string.Empty;
            requestDict["uri"] = taskURI;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                content.Headers.Add("authorization_email", PollyEmail);
                content.Headers.Add("authorization_password", PollyPassword);
                HttpResponseMessage result = httpClient.PostAsync(PollyURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                byte[] data = result.Content.ReadAsByteArrayAsync().Result;
                File.WriteAllBytes(targetFile, data);
                generated = true;
            }
            if (progressBar != null)
            {
                progressBar.Value = 4;
            }

            requestDict.Clear();
            requestDict["delete"] = string.Empty;
            requestDict["uri"] = taskURI;
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                content.Headers.Add("authorization_email", PollyEmail);
                content.Headers.Add("authorization_password", PollyPassword);
                _ = httpClient.PostAsync(PollyURI, content).Result;
            }
            if (progressBar != null)
            {
                progressBar.Value = 5;
                progressBar.Visible = false;
                progressBar.SendToBack();
            }

            Cursor.Current = Cursors.Default;
            return generated;
        }

        /// <summary>
        /// Test to see if the supplied URI is a valid Amazon Polly language honing URI.
        /// </summary>
        /// <param name="newURI"></param>
        /// <returns></returns>
        public static bool TestURI(string newURI)
        {
            HttpHandler httpHandler = HttpHandler.Instance;
            HttpClient httpClient = httpHandler.HttpClient;
            StringContent content;
            Dictionary<string, string> requestDict = new()
            {
                ["systemStatus"] = string.Empty
            };
            using (content = new StringContent(JsonSerializer.Serialize<Dictionary<string, string>>(requestDict), Encoding.UTF8, "application/json"))
            {
                content.Headers.Add("authorization_email", PollyEmail);
                content.Headers.Add("authorization_password", PollyPassword);
                HttpResponseMessage result = httpClient.PostAsync(newURI, content).Result;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
