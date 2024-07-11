using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ConlangJson;
using static ConlangAudioHoning.PhoneticChanger;

namespace ConlangAudioHoning
{
    internal static class MetadataWasher
    {
        public static bool CleanLanguageMetadata(LanguageDescription languageDescription)
        {
            foreach (LexiconEntry word in languageDescription.lexicon)
            {
                CleanLexiconEntryMetadata(word);
            }

            return true;
        }

        public static bool CleanLexiconEntryMetadata(LexiconEntry word, bool aggressive = false)
        {
            word.metadata ??= [];
            Dictionary<string, PhoneticChangeHistory>? phoneticChangeHistories = null;
            if (word.metadata.ContainsKey("PhoneticChangeHistory"))
            {
                phoneticChangeHistories = JsonSerializer.Deserialize<Dictionary<string, PhoneticChangeHistory>>(word.metadata["PhoneticChangeHistory"]);
            }
            if (phoneticChangeHistories != null)
            {
                SortedSet<string> timeStamps = [.. phoneticChangeHistories.Keys];
                if (!aggressive)
                {
                    string lastTimestamp = timeStamps.Max ?? string.Empty;
                    timeStamps.Remove(lastTimestamp);
                    CleanLexiconEntryMetadata(phoneticChangeHistories[lastTimestamp].OldVersion, true);
                }
                foreach (string timestamp in timeStamps)
                {
                    phoneticChangeHistories.Remove(timestamp);
                }
                string pchString = JsonSerializer.Serialize<Dictionary<string, PhoneticChangeHistory>>(phoneticChangeHistories);
                word.metadata["PhoneticChangeHistory"] = JsonSerializer.Deserialize<JsonObject>(pchString);
            }


            return true;
        }
    }
}
