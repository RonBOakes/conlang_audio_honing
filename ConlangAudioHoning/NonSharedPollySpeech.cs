﻿/*
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
using System.Diagnostics;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using ConlangJson;
using NLog;

namespace ConlangAudioHoning
{
    internal class NonSharedPollySpeech : SpeechEngine
    {
        private readonly AmazonPollyClient pollyClient;
        private readonly AmazonS3Client s3Client;
        private readonly Dictionary<string, Amazon.Polly.Model.Voice> voiceModels = [];

        private readonly NLog.Logger _logger;

        /// <summary>
        /// Constructor for the Amazon Polly interface.
        /// </summary>
        public NonSharedPollySpeech() : base()
        {
            Description = "Amazon Polly";
            _logger = LogManager.GetCurrentClassLogger();

            if (string.IsNullOrEmpty(PollySSOProfile))
            {
                _logger.Fatal("Unable to create NonSharedPollySpeech object: No SSO Profile available");
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object: No SSO Profile available");
            }
            SSOAWSCredentials credentials;
            try
            {
                credentials = LoadSsoCredentials();
                _logger.Trace("SSO Credentials created");
            }
            catch (NonSharedPollyException ex)
            {
                _logger.Fatal(ex, "Unable to create NonSharedPollySpeech object: {Exception}", ex.Message);
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object:", ex);
            }

            pollyClient = new AmazonPollyClient(credentials, RegionEndpoint.GetBySystemName(credentials.Region));
            s3Client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(credentials.Region));
            _logger.Trace("Polly and S3 Clients Created");
        }

        /// <summary>
        /// Constructor for the Amazon Polly interface.
        /// </summary>
        /// <param name="languageDescription">LanguageDescription object for the language to work with.</param>
        public NonSharedPollySpeech(LanguageDescription languageDescription) : base(languageDescription)
        {
            Description = "Amazon Polly";
            _logger = LogManager.GetCurrentClassLogger();

            if (string.IsNullOrEmpty(PollySSOProfile))
            {
                _logger.Fatal("Unable to create NonSharedPollySpeech object: No SSO Profile available");
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object: No SSO Profile available");
            }
            SSOAWSCredentials credentials;
            try
            {
                credentials = LoadSsoCredentials();
                _logger.Trace("SSO Credentials created");
            }
            catch (NonSharedPollyException ex)
            {
                _logger.Fatal(ex, "Unable to create NonSharedPollySpeech object: {Exception}", ex.Message);
                throw new NonSharedPollyException("Unable to create NonSharedPollySpeech object:", ex);
            }

            pollyClient = new AmazonPollyClient(credentials, RegionEndpoint.GetBySystemName(credentials.Region));
            s3Client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(credentials.Region));
            _logger.Trace("Polly and S3 Clients Created");
        }

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
        /// Amazon SSO Profile to use for Amazon Polly.
        /// </summary>
        public static string? PollySSOProfile
        {
            get; set;
        }

        /// <summary>
        /// Name of the AWS S3 bucket where this Polly nonshared (direct) instance
        /// will store its files.
        /// </summary>
        public static string? PollyS3Bucket
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
                _logger.Error("Cannot Generate Polly Speech without a language description");
                throw new ConlangAudioHoningException("Cannot Generate Polly Speech without a language description");
            }
            if (string.IsNullOrEmpty(SampleText))
            {
                _logger.Error("Cannot Generate polly Speech without sample text");
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
                _logger.Error("Cannot generate speech without a language description");
                return false;
            }
            if ((SampleText == null) || (SampleText.Length == 0))
            {
                _logger.Error("Cannot generate speech without sample text");
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

            Cursor.Current = Cursors.Default;

            if (voiceModels.Count == 0)
            {
                _ = GetVoices();
            }

            if (!CreateBucketIfNeeded())
            {
                _logger.Error("Unable to create an S3 Bucket");
                return false;
            }

            StartSpeechSynthesisTaskRequest request = new()
            {
                Engine = Engine.Neural,
                OutputFormat = OutputFormat.Mp3,
                OutputS3BucketName = PollyS3Bucket,
                TextType = TextType.Ssml,
                Text = SsmlText,
                VoiceId = voiceModels[voice].Id
            };

            try
            {
                // Start the speech generation.
                Task<StartSpeechSynthesisTaskResponse> responseTask = pollyClient.StartSpeechSynthesisTaskAsync(request);

                while (!responseTask.IsCompleted)
                {
                    Thread.Sleep(5000);
                }

                // Wait for it to finish.
                StartSpeechSynthesisTaskResponse response = responseTask.Result;
                if (response != null)
                {
                    SynthesisTask task = response.SynthesisTask;
                    string outputURIString = task.OutputUri;

                    // List all of the files currently in the S3 Bucket
                    ListObjectsV2Request listObjectsV2Request = new()
                    {
                        BucketName = PollyS3Bucket
                    };
                    int retryCount = 3;
                    bool done = false;
                    do
                    {
                        ListObjectsV2Response listObjectsV2Response = s3Client.ListObjectsV2Async(listObjectsV2Request).Result;
                        S3Object? s3AudioFile = null;
                        if (listObjectsV2Response != null)
                        {
                            foreach (S3Object s3file in from S3Object s3file in listObjectsV2Response.S3Objects
                                                        where outputURIString.Contains(s3file.Key)
                                                        select s3file)
                            {
                                s3AudioFile = s3file;
                            }

                            if (s3AudioFile == null)
                            {
                                Cursor.Current = Cursors.Default;
                                _logger.Error("Failed to get the name for an audio file from S3 after receiving a valid V2Response");
                                _logger.Debug("Output URI: {OutputURI}", outputURIString);
                                foreach (S3Object s3file in listObjectsV2Response.S3Objects)
                                {
                                    _logger.Debug("--> S3 Object Key {Key}", s3file.Key);
                                }
                                Thread.Sleep(5000);
                                continue;
                            }
                            // Get the S3 Object
                            GetObjectRequest getObjectRequest = new()
                            {
                                BucketName = PollyS3Bucket,
                                Key = s3AudioFile.Key
                            };
                            GetObjectResponse getObjectResponse = s3Client.GetObjectAsync(getObjectRequest).Result;

                            // Write the data out to the target file
                            CancellationToken cancellationToken = new();
                            getObjectResponse.WriteResponseStreamToFileAsync(targetFile, false, cancellationToken).Wait();

                            // Delete all files in the S3 Bucket (to clean up from past issues)
                            foreach (S3Object s3file in listObjectsV2Response.S3Objects)
                            {
                                DeleteObjectRequest deleteObjectRequest = new()
                                {
                                    BucketName = PollyS3Bucket,
                                    Key = s3file.Key
                                };
                                s3Client.DeleteObjectAsync(deleteObjectRequest).Wait();
                            }
                            done = true;
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            _logger.Error("Did not get a listObjectsV2Response");
                            return false;
                        }
                    }
                    while ((!done) && (--retryCount >= 0));
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    _logger.Error("Did not get a response to StartSpeechSynthesisTaskAsync");
                    if (responseTask.Exception != null)
                    {
                        _logger.Error(responseTask.Exception, "Exception in StartSpeechSynthesisTaskAsync: {ExceptionMessage}", responseTask.Exception.Message);
                    }
                    else
                    {
                        _logger.Info("The state of the task is {TaskStatus}", responseTask.Status.ToString());
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception generating speech: {ExceptionMessage}", ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }

            Cursor.Current = Cursors.Default;
            return true;
        }

        public override Dictionary<string, VoiceData> GetVoices()
        {
            Dictionary<string, SharedPollySpeech.VoiceData> voices = [];

            DescribeVoicesRequest request = new()
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
                        SupportedEngines = [.. voice.SupportedEngines]
                    };
                    voices.Add(voiceData.Name, voiceData);
                    voiceModels.Add(voice.Name, voice);
                }
            }

            return voices;
        }

        /// <summary>
        /// Create an Amazon S3 Bucket for the current PollyS3Bucket, if needed.
        /// </summary>
        private bool CreateBucketIfNeeded()
        {
            ListBucketsResponse response = s3Client.ListBucketsAsync().Result;
            foreach (S3Bucket bucket in response.Buckets)
            {
                if (bucket.BucketName == PollyS3Bucket)
                {
                    return true;
                }
            }

            PutBucketRequest request = new()
            {
                BucketName = PollyS3Bucket,
                UseClientRegion = true,

            };
            try
            {
                _ = s3Client.PutBucketAsync(request).Result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception when creating S3 bucket {BucketName}: {ExceptionMessage}", PollyS3Bucket, ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Attempt to load the SSO credentials for the profile.  Will open a browser window for
        /// AWS Login if needed
        /// </summary>
        /// <returns>the AWS credentials</returns>
        /// <exception cref="NonSharedPollyException">If an exception occurs in the process.</exception>
        private static SSOAWSCredentials LoadSsoCredentials()
        {
            CredentialProfileStoreChain chain = new();
            if (!chain.TryGetAWSCredentials(PollySSOProfile, out AWSCredentials credentials))
            {
                LogManager.GetCurrentClassLogger().Error("Failed to find the {PollySSOProfile} profile", PollySSOProfile);
                throw new NonSharedPollyException($"Failed to find the {PollySSOProfile} profile");
            }
            if (credentials == null)
            {
                LogManager.GetCurrentClassLogger().Error("Unable to acquire credentials");
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
