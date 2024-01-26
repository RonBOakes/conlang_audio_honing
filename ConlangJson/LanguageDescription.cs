using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ConlangJson
{
    public class LanguageDescription
    {
        private string? _english_name;
        private string? _native_name_phonetic;
        private string? _native_name_english;
        private string? _preferred_voice;
        private bool? _derived;
        private bool? _declined;
        private List<string>? _noun_gender_list;
        private List<string>? _part_of_speech_list;
        private List<string>? _phoneme_inventory;
        private List<SoundMap>? _sound_map_list;
        private List<string>? _lexical_order_list;
        private Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>>? _affix_map;
        private Dictionary<string, DerivationalAffix>? _derivational_affix_map;
        private List<LexiconEntry>? _lexicon;
        private List<string>? _derived_word_list;
        private JsonObject? _metadata;

        public LanguageDescription()
        {
            _noun_gender_list = [];
            _part_of_speech_list = [];
            _phoneme_inventory = [];
            _sound_map_list = [];
            _lexical_order_list = [];
            _affix_map = [];
            _derivational_affix_map = [];
            _lexicon = [];
            _derived_word_list = [];
            _metadata = [];
        }

        public LanguageDescription(string? english_name, string? native_name_phonetic, string? native_name_english, List<string>? noun_gender_list, string? preferred_voice, 
            bool? derived, bool? declined, List<string>? part_of_speech_list, List<string>? phoneme_inventory, List<SoundMap>? sound_map_list, 
            List<string>? lexical_order_list, Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>>? affix_map, Dictionary<string, DerivationalAffix>? derivational_affix_map, 
            List<LexiconEntry> lexicon, List<string>? derived_word_list, JsonObject metadata)
        {
            _english_name = english_name;
            _native_name_phonetic = native_name_phonetic;
            _native_name_english = native_name_english;
            _noun_gender_list = noun_gender_list ?? [];
            _part_of_speech_list = part_of_speech_list ?? [];
            _phoneme_inventory = phoneme_inventory ?? [];
            _sound_map_list = sound_map_list ?? [];
            _lexical_order_list = lexical_order_list ?? [];
            _affix_map = affix_map ?? [];
            _derivational_affix_map = derivational_affix_map ?? [];
            _lexicon = lexicon ?? [];
            _derived_word_list = derived_word_list ?? [];
            _metadata = metadata ?? [];
        }

        public string? english_name
        {
            get => _english_name;
            set => _english_name = value;
        }
        
        public string? native_name_phonetic
        {
            get => _native_name_phonetic;
            set => _native_name_phonetic = value;
        }

        public string? native_name_english
        {
            get => _native_name_english;
            set => _native_name_english = value;
        }

        public string? preferred_voice
        {
            get => _preferred_voice;
            set => _preferred_voice = value;
        }

        public bool? derived
        {
            get => _derived; 
            set => _derived = value;
        }

        public bool? declined
        {
            get => _declined; 
            set => _declined = value;
        }

        public List<string> noun_gender_list
        {
            get => _noun_gender_list ?? [];
            set => _noun_gender_list = value;
        }

        public List<string> part_of_speech_list
        {
            get => _part_of_speech_list ?? [];
            set => _part_of_speech_list = value;
        }

        public List<string> phoneme_inventory
        {
            get => _phoneme_inventory ?? [];
            set => _phoneme_inventory = value;
        }

        public List<SoundMap> sound_map_list
        {
            get => _sound_map_list ?? [];
            set => _sound_map_list = value;
        }

        public List<string> lexical_order_list
        {
            get => _lexical_order_list ?? []; 
            set 
            {
                _lexical_order_list = value;
                LexiconEntry.LexicalOrderList = value;
            }
        }

        public Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>> affix_map
        {
            get => _affix_map ?? [];
            set => _affix_map = value;
        }

        public Dictionary<string, DerivationalAffix> derivational_affix_map
        {
            get => _derivational_affix_map ?? [];
            set =>_derivational_affix_map = value;
        }

        public List<LexiconEntry> lexicon
        {
            get => _lexicon ?? []; 
            set => _lexicon = value;
        }

        public List<string> derived_word_list
        {
            get => _derived_word_list ?? []; 
            set => _derived_word_list = value;
        }

        public JsonObject metadata
        { 
            get => _metadata ?? []; 
            set => _metadata = value;
        }

    }
}
