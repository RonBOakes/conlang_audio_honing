/*
 * Program for populating TestEnglish's lexicon from Dwarven's Lexicon
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
using System.Runtime.CompilerServices;
using ConlangJson;
using System.Text.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using System.Text.Unicode;

namespace AddTestEnglishLexicon
{
    public static class AddTestEnglishLexicon
    {
        private static readonly string _testEnglishLocation = @"C:\Users\ron\source\repos\conlang_audio_honing\TestData\TestEnglish.json";
        private static readonly string _dwarvenLocation = @"C:\Users\ron\Dropbox\FantasyStory\Conlang\Dwarven2\dwarven_language.json";
        private static readonly string _espeakNgLocation = @"C:\Program Files\eSpeak NG\espeak-ng.exe";

        public static void Main()
        {
            string jsonString = File.ReadAllText(_dwarvenLocation);
            LanguageDescription? dwarven;
            try
            {
                dwarven = JsonSerializer.Deserialize<LanguageDescription>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Could not load Dwarven: {0}", ex.Message));
                return;
            }
            if (dwarven == null)
            {
                Console.WriteLine("Failed to read Dwarven");
                return;
            }

            jsonString = File.ReadAllText(_testEnglishLocation);
            LanguageDescription? testEnglish;
            try
            {
                testEnglish = JsonSerializer.Deserialize<LanguageDescription>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Could not load Test English: {0}", ex.Message));
                return;
            }
            if (testEnglish == null)
            {
                Console.WriteLine("Failed to read Test English");
                return;
            }

            foreach (LexiconEntry dwarvenEntry in dwarven.lexicon)
            {
                LexiconEntry englishEntry = new LexiconEntry()
                {
                    english = dwarvenEntry.english,
                    spelled = dwarvenEntry.english,
                    phonetic = getWordPhonetic(dwarvenEntry.english),
                    part_of_speech = dwarvenEntry.part_of_speech,
                    declensions = [],
                    declined_word = false,
                    derived_word = dwarvenEntry.derived_word,
                    metadata = []
                };
                testEnglish.lexicon.Add(englishEntry);
            }
            DateTime now = DateTime.UtcNow;
            string timestamp = now.ToString("o");
            string history = "Dwarven Lexicon added, saved at " + timestamp;

            // Set the version based on the presence of phoneme_clusters.  Even if empty, 
            // the version needs to be updated because the field will be in the saved JSON.
            testEnglish.version = 1.1;

            if (!testEnglish.metadata.ContainsKey("history"))
            {
                testEnglish.metadata["history"] = new JsonArray();
            }
#pragma warning disable CS8600
            JsonArray historyEntries = (JsonArray)testEnglish.metadata["history"] ?? [];
#pragma warning restore CS8600
            historyEntries.Add(history);

#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
            JsonSerializerOptions jsonSerializerOptions = new(DefaultJsonSerializerOptions);
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
            JavaScriptEncoder encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            jsonSerializerOptions.Encoder = encoder;

            jsonString = JsonSerializer.Serialize<LanguageDescription>(testEnglish, jsonSerializerOptions);

            File.WriteAllText(_testEnglishLocation, jsonString, System.Text.Encoding.UTF8);

        }

        private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };



        private static string getWordPhonetic(string word)
        {
            StringBuilder cmdSb = new();

            cmdSb.Append("-v en-us --ipa ");
            cmdSb.Append(string.Format(@"""{0}""", word));

            string response = string.Empty;
            _ = RunConsoleCommand(cmdSb.ToString().Trim(), ref response);

            return response;
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
            startInfo.FileName = _espeakNgLocation;

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
