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
using System.Text;
using ConlangJson;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Class used for interacting with espeak-ng for speech generation.
    /// </summary>
    internal class ESpeakNGSpeak : SpeechEngine
    {
        public ESpeakNGSpeak() : base()
        {
            Description = "eSpeak-ng";
        }

        /// <summary>
        /// Constructor for the ESpeakNGSpeak class.
        /// </summary>
        public ESpeakNGSpeak(LanguageDescription languageDescription) : base(languageDescription)
        {
            Description = "eSpeak-ng";
        }

        public static string ESpeakNGPath { get; set; } = "";

        /// <summary>
        /// Key used to access the LanguageDescription preferred_voices dictionary.
        /// </summary>
        public override string PreferredVoiceKey
        {
            get => "eSpeak-ng";
        }

        /// <summary>
        /// Key used in the Conlang JSON structure to index the preferred voice.
        /// </summary>
        public override string PreferredVoiceJsonKey
        {
            get => "espeak-ng";
        }

        /// <summary>
        /// Generate the phonetic text and SSML text.
        /// </summary>
        /// <param name="speed">SSML &lt;prosody&gt; speed value to be used in the generated SSML.</param>
        /// <param name="caller">Optional link to the LanguageHoningForm that called this method.  If not
        /// null, the Decline method from that form will be used, allowing progress to be displayed.</param>
        /// <param name="voiceData"> Optional voice data. Unused.</param>
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

            StringBuilder eSpeakText = new();

            PhoneticText = string.Empty;
            int wordWrap = 0;

            foreach (List<Dictionary<string, string>> lineMapList in pronounceMapList)
            {
                foreach (Dictionary<string, string> pronounceMap in lineMapList)
                {
                    // Build the text entry
                    eSpeakText.Append(KirshenbaumUtilities.IpaWordToKirshenbaum(pronounceMap["phonetic"]));
                    PhoneticText += pronounceMap["phonetic"];
                    wordWrap += pronounceMap["phonetic"].Length;
                    if (pronounceMap["punctuation"] != "")
                    {
                        eSpeakText.Append(pronounceMap["punctuation"]);
                        PhoneticText += pronounceMap["punctuation"];
                        wordWrap += pronounceMap["punctuation"].Length;
                    }
                    PhoneticText += " ";
                    eSpeakText.Append(' ');
                    wordWrap += 1;
                    if (wordWrap >= 80)
                    {
                        eSpeakText.Append('\n');
                        PhoneticText += "\n";
                        wordWrap = 0;
                    }
                }
            }

            SsmlText = eSpeakText.ToString().Trim();

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
            if (LanguageDescription == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SampleText))
            {
                return false;
            }

            if (string.IsNullOrEmpty(SsmlText))
            {
                this.Generate(speed ?? "medium", caller);
            }

            Cursor.Current = Cursors.WaitCursor;

            if (string.IsNullOrEmpty(voice))
            {
                voice = "en-us";
            }
            int speedInt = -1;
            if (!string.IsNullOrEmpty(speed))
            {
                speedInt = speed.ToLower() switch
                {
                    "x-slow" => 75,
                    "slow" => 125,
                    "medium" or "default" => 175,
                    "fast" => 225,
                    "x-fast" => 275,
                    _ => 175,
                };
            }

            bool result = Speak(text: SsmlText, waveFile: targetFile, voiceLanguage: voice, speed: speedInt);
            Cursor.Current = Cursors.Default;
            return result;
        }


        public override Dictionary<string, VoiceData> GetVoices()
        {
            Dictionary<string, VoiceData> voices = [];

            char[] whiteSpaces = [' ', '\t',];

            string voiceList = string.Empty;
            RunConsoleCommand("--voices", ref voiceList);
            /* Output looks like
             * Pty Language       Age/Gender VoiceName          File                 Other Languages
             * 5  af              --/M      Afrikaans          gmw\af
             * 7  af              --/M      afrikaans-mbrola-1 mb\mb-af1
             */
            using (StringReader sampleTextReader = new(voiceList))
            {
                string? line;
                do
                {
                    line = sampleTextReader.ReadLine();
                    if (!string.IsNullOrEmpty(line) && (!line.Contains("Language")))
                    {

                        string[] fields = line.Split(whiteSpaces, StringSplitOptions.RemoveEmptyEntries); // Split on whitespace
                        VoiceData voiceData = new()
                        {
                            Name = fields[3],
                            LanguageCode = fields[1],
                            LanguageName = fields[1], // espeak-ng just uses code
                            Id = fields[4]
                        };
                        string[] ageGender = fields[2].Split("/");
                        voiceData.Gender = ageGender[1];
                        voices.TryAdd(voiceData.LanguageCode, voiceData);
                    }
                }
                while (line != null);
            }


            return voices;
        }

        public static void Test()
        {
            // Initialize the eSpeak-ng library via the wrapper
            List<string> ipaText =
            [
                "ðɪs",
                "ɪz",
                "ɐ",
                "tˈɛst"
            ];
            StringBuilder sb = new();
            foreach (string word in ipaText)
            {
                sb.Append(KirshenbaumUtilities.IpaWordToKirshenbaum(word));
                sb.Append(' ');
            }
            string text = "[[h@'loU]]. This is a test. ";
            text += sb.ToString();
            Speak(text: text, voiceLanguage: "en-us");
            return;
        }

        internal static bool Speak(string text, string waveFile = "", string voiceLanguage = "", int speed = -1)
        {
            StringBuilder cmdSb = new();

            // Write the text to a temporary file
            DateTime now = DateTime.Now;
            string targetFileBaseName = string.Format("speech_{0:s}.txt", now);
            targetFileBaseName = targetFileBaseName.Replace(":", "_");
            string targetFileName = Path.GetTempPath() + targetFileBaseName;
            FileInfo targetFile = new(targetFileName);
            StreamWriter fileWriter = targetFile.CreateText();
            fileWriter.WriteLine(text);
            fileWriter.Flush();
            fileWriter.Close();

            if (!string.IsNullOrEmpty(voiceLanguage))
            {
                cmdSb.AppendFormat("-v{0} ", voiceLanguage);
            }
            if (!string.IsNullOrEmpty(waveFile))
            {
                cmdSb.AppendFormat("-w \"{0}\" ", waveFile);
            }
            if (speed > 0)
            {
                cmdSb.AppendFormat("-s{0} ", speed);
            }
            cmdSb.AppendFormat("-f \"{0}\" ", targetFile.FullName);

            string response = string.Empty;
            bool result = RunConsoleCommand(cmdSb.ToString().Trim(), ref response);

            targetFile.Delete(); // Remove temporary file.

            return result;
        }

        private static bool RunConsoleCommand(string cmd, ref string response)
        {
            bool processRunning;
            System.Diagnostics.Process process = new();
            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            process.Exited += new EventHandler((sender, e) =>
            {
                processRunning = false;
            }
            );
            startInfo.FileName = ESpeakNGPath;

            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            processRunning = true;
            process.Start();
            while (processRunning)
            {
                if (!process.HasExited)
                {
                    processRunning = false;
                }
                if (!process.Responding)
                {
                    processRunning = false;
                    process.Kill();
                }
            }

            string stdOut = process.StandardOutput.ReadToEnd();
            string stdErr = process.StandardError.ReadToEnd();

            bool noError = false;

            if (process.ExitCode == 0)
            {
                noError = true;
                response = stdOut;
            }
            else if (stdErr.Length > 0)
            {
                noError = false;
                response = stdErr;
            }
            else
            {
                noError = false;
                response = stdOut;
            }
            process.Close();
            process.Dispose();

            return noError;
        }
    }
}
