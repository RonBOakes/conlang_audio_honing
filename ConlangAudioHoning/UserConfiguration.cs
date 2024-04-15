/*
* Class for holding the user configuration.
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
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace ConlangAudioHoning
{
    internal partial class UserConfiguration
    {
        private string? _pollyURI;
        private string? _eSpeakNgPath;
        private string? _azureSpeechKey;
        private string? _azureSpeechRegion;

        private static UserConfiguration? config = null;

        public UserConfiguration()
        { }

        /// <summary>
        /// The URI to be used to access Amazon Polly.  
        /// </summary>
        public string PollyURI
        {
            get => _pollyURI ?? string.Empty;
            set => _pollyURI = value;
        }

        /// <summary>
        /// File Path containing the locally installed version of eSpeak-ng.
        /// </summary>
        public string ESpeakNgPath
        {
            get => _eSpeakNgPath ?? string.Empty;
            set => _eSpeakNgPath = value;
        }

        public string AzureSpeechKey
        {
            get => _azureSpeechKey ?? string.Empty;
            set => _azureSpeechKey = value;
        }

        public string AzureSpeechRegion
        {
            get => _azureSpeechRegion ?? string.Empty;
            set => _azureSpeechRegion = value;
        }

        /// <summary>
        /// True if this configuration has settings to load and support Amazon Polly.
        /// </summary>
        public bool IsPollySupported
        {
            get => (!string.IsNullOrEmpty(PollyURI));
        }


        /// <summary>
        /// True if this configuration has settings to utilize a local copy of eSpeak-ng
        /// </summary>
        public bool IsESpeakNGSupported
        {
            get => (!string.IsNullOrEmpty(ESpeakNgPath));
        }

        public bool IsAzureSupported
        {
            get => (!string.IsNullOrEmpty(AzureSpeechKey) && !string.IsNullOrEmpty(AzureSpeechRegion));
        }

        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new()
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        /// <summary>
        /// Write this configuration out as a JSON file.
        /// </summary>
        /// <param name="jsonSerializerOptions">Options for serializing the JSON when writing out</param>
        /// <param name="filePath">Path to write the configuration file.  By default this will create the 
        /// configuration file in the user's application data directory using a default name.</param>
        public void SaveConfiguration(JsonSerializerOptions jsonSerializerOptions, string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Application.UserAppDataPath.Trim() + @"\LanguageHoningConfig.json";
            }

            Match commitInPathMatch = MyRegex().Match(filePath);
            if (commitInPathMatch.Success)
            {
                filePath = filePath.Replace(commitInPathMatch.Groups[1].ToString(), commitInPathMatch.Groups[2].ToString());
            }

            FileInfo fi = new(filePath);

            DirectoryInfo di = fi.Directory ?? new DirectoryInfo(Application.UserAppDataPath.Trim());
            if (!di.Exists)
            {
                di.Create();
            }

            JavaScriptEncoder encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            jsonSerializerOptions.Encoder = encoder;
            jsonSerializerOptions.WriteIndented = true;

            string jsonString = JsonSerializer.Serialize<UserConfiguration>(this, jsonSerializerOptions);
            File.WriteAllText(filePath, jsonString, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Load a configuration from the specified file.
        /// </summary>
        /// <param name="filePath">Path to write the configuration file.  By default this will use the 
        /// configuration file in the user's application data directory using a default name if it exists.</param>
        /// <returns>The configuration as loaded, or a default configuration if no configuration can be loaded.</returns>
        public static UserConfiguration LoadFromFile(string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Application.UserAppDataPath.Trim() + @"\LanguageHoningConfig.json";
            }

            Match commitInPathMatch = MyRegex().Match(filePath);
            if (commitInPathMatch.Success)
            {
                filePath = filePath.Replace(commitInPathMatch.Groups[1].ToString(), commitInPathMatch.Groups[2].ToString());
            }

            UserConfiguration configuration;
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                configuration = JsonSerializer.Deserialize<UserConfiguration>(jsonString);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                configuration ??= new UserConfiguration();
            }
            else
            {
                configuration = new UserConfiguration();
            }
            config = configuration;
            return configuration;
        }

        /// <summary>
        /// Load the current configuration 
        /// </summary>
        /// <returns>The current version of the User Configuration.</returns>
        public static UserConfiguration LoadCurrent()
        {
            if (config == null)
            {
                config = new();
            }
            return config;
        }

        [GeneratedRegex(@"\\((\d+\.\d+\.\d+)\+[0-9a-f]+)\\")]
        private static partial Regex MyRegex();
    }
}
