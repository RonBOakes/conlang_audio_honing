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
using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    internal class UserConfiguration
    {
        private static string? _pollyURI;
        private static string? _eSpeakNgPath;

        // TODO: replace these with empty strings or something to 
        //       indicate when the related option is not present.
        private static string DefaultPollyURI = "https://9ggv18yii2.execute-api.us-east-1.amazonaws.com/general_speak2";
        private static string DefalultESpeakNGPath = @"C:\Program Files\eSpeak NG\espeak-ng.exe";

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
        /// File Path contining the locally installed version of eSpeak-ng.
        /// </summary>
        public string ESpeakNgPath
        {
            get => _eSpeakNgPath ?? string.Empty;
            set => _eSpeakNgPath = value;
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

        /// <summary>
        /// Write this configuration out as a JSON file.
        /// </summary>
        /// <param name="filepath">Path to write the configuration file.  By default this will create the 
        /// configuration file in the user's application data directory using a default name.</param>
        public void SaveConfiguration(string filepath = "")
        {
            if (string.IsNullOrEmpty(filepath))
            {
                filepath = Application.UserAppDataPath.Trim() + @"\LanguageHoningConfig.json";
            }

            JsonSerializerOptions jsonSerializerOptions = new();
            jsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            JavaScriptEncoder encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            jsonSerializerOptions.Encoder = encoder;
            jsonSerializerOptions.WriteIndented = true; // TODO: make an option

            string jsonString = JsonSerializer.Serialize<UserConfiguration>(this, jsonSerializerOptions);
            File.WriteAllText(filepath, jsonString, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Load a configuration from the specified file.
        /// </summary>
        /// <param name="filepath">Path to write the configuration file.  By default this will use the 
        /// configuration file in the user's application data directory using a default name if it exists.</param>
        /// <returns>The configuration as loaded, or a default configuration if no configuration can be loaded.</returns>
        public static UserConfiguration LoadFromFile(string filepath = "")
        {
            if(string.IsNullOrEmpty(filepath)) 
            { 
                filepath = Application.UserAppDataPath.Trim() + @"\LanguageHoningConfig.json";
            }

            UserConfiguration configuration;
            if (File.Exists(filepath)) 
            {
                string jsonString = File.ReadAllText(filepath);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                configuration = JsonSerializer.Deserialize<UserConfiguration>(jsonString);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if(configuration == null)
                {
                    configuration = new UserConfiguration();
                }
            }
            else
            {
                configuration = new UserConfiguration();
            }
            return configuration;
        }

        /// <summary>
        /// Load the current configuration 
        /// </summary>
        /// <returns>The current version of the User Configuration.</returns>
        public static UserConfiguration LoadCurrent()
        {
            UserConfiguration config = new UserConfiguration();
            return config;
        }

        private static UserConfiguration LoadDefault()
        {
            UserConfiguration config = new UserConfiguration();
            config.PollyURI = DefaultPollyURI;
            config.ESpeakNgPath = DefalultESpeakNGPath;
            return config;
        }
    }
}
