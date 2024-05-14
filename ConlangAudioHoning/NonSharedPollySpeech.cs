/*
* Class for Processing speech using Amazon Polly via the non-shared direct interface.
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
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SecurityToken.Model;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.SecurityToken;
using Amazon;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace ConlangAudioHoning
{
    internal class NonSharedPollySpeech : SpeechEngine
    {
        AmazonPollyClient pollyClient;
        AmazonSecurityTokenServiceClient ssoProfileClient;

        /// <summary>
        /// Constructor for the Amazon Polly interface.
        /// </summary>
        public NonSharedPollySpeech() : base()
        {
            Description = "Amazon Polly";
            if (string.IsNullOrEmpty(PollySSOProfile))
            {
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object: No SSO Profile available");
            }
            SSOAWSCredentials credentials;
            try
            {
                credentials = LoadSsoCredentials();
            }
            catch (NonSharedPollyException ex)
            {
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object:", ex);
            }

            // Log into the SSO
            ssoProfileClient = new(credentials);

            pollyClient = new AmazonPollyClient(credentials,RegionEndpoint.GetBySystemName(credentials.Region));
        }

        /// <summary>
        /// Constructor for the Amazon Polly interface.
        /// </summary>
        /// <param name="languageDescription">LanguageDescription object for the language to work with.</param>
        public NonSharedPollySpeech(LanguageDescription languageDescription) : base(languageDescription)
        {
            Description = "Amazon Polly";
            if (string.IsNullOrEmpty(PollySSOProfile))
            {
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object: No SSO Profile available");
            }
            SSOAWSCredentials credentials;
            try
            {
                credentials = LoadSsoCredentials();
            }
            catch (NonSharedPollyException ex)
            {
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object:", ex);
            }

            // Log into the SSO
            ssoProfileClient = new(credentials);
            pollyClient = new AmazonPollyClient(credentials, RegionEndpoint.GetBySystemName(credentials.Region));
        }

        /// <summary>
        /// Key used to access the LanguageDescription preferred_voices dictionary.
        /// </summary>
        public override string PreferredVoiceKey
        {
            get => "Polly";
        }

        /// <summary>
        /// Amazon SSO Profile to use for Amazon Polly.
        /// </summary>
        public static string? PollySSOProfile
        {
            get; set;
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
        }

        public override bool GenerateSpeech(string targetFile, string? voice = null, string? speed = null, LanguageHoningForm? caller = null)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, VoiceData> GetVoices()
        {
            Dictionary<string, SharedPollySpeech.VoiceData> voices = [];

            DescribeVoicesRequest request = new DescribeVoicesRequest
            {
                Engine = Amazon.Polly.Engine.Neural,
                IncludeAdditionalLanguageCodes = true
            };

            DescribeVoicesResponse response = pollyClient.DescribeVoicesAsync(request).Result;
            if (response != null)
            {
                foreach (Amazon.Polly.Model.Voice voice in response.Voices)
                {
                    SharedPollySpeech.VoiceData voiceData = new()
                    {
                        Name = voice.Name,
                        Gender = voice.Gender,
                        Id = voice.Id,
                        LanguageCode = voice.LanguageCode,
                        LanguageName = voice.LanguageName,
                        SupportedEngines = voice.SupportedEngines.ToArray()
                    };
                    voices.Add(voiceData.Name, voiceData);
                }
            }

            return voices;
        }

        /// <summary>
        /// Attempt to load the SSO credentials for the profile.  Will open a browser window for
        /// AWS Login if needed
        /// </summary>
        /// <returns>the AWS credentials</returns>
        /// <exception cref="NonSharedPollyException">If an exception occurs in the process.</exception>
        private SSOAWSCredentials LoadSsoCredentials()
        {
            CredentialProfileStoreChain chain = new();
            if (!chain.TryGetAWSCredentials(PollySSOProfile, out AWSCredentials credentials))
            {
                throw new NonSharedPollyException($"Failed to find the {PollySSOProfile} profile");
            }
            if(credentials == null )
            {
                throw new NonSharedPollyException("Unable to acquire credentials");
            }
            SSOAWSCredentials ssoCredentials = (SSOAWSCredentials)credentials;

            ssoCredentials.Options.ClientName = "ConlangAudioHoning-Polly-App";
            ssoCredentials.Options.SsoVerificationCallback = args =>
            {
                // Launch a browser window that prompts the SSO user to complete an SSO sign-in.
                // This method is only invoked if the session doesn't already have a valid SSO token.
                // NOTE: Process.Start might not support launching a browser on macOS or Linux. If not,
                //       use an appropriate mechanism on those systems instead.
                Process.Start(new ProcessStartInfo
                {
                    FileName = args.VerificationUriComplete,
                    UseShellExecute = true
                });
            };
            return ssoCredentials;
        }

    }

    /// <summary>
    /// Exceptions thrown from the NonSharedPollySpeech class.
    /// </summary>
    public class NonSharedPollyException : Exception
    {
        /// <summary>
        /// Create an exception containing a message.
        /// </summary>
        /// <param name="message"></param>
        public NonSharedPollyException(string message) : base(message) { }

        /// <summary>
        /// Create an exception containing a message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NonSharedPollyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
