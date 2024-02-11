/*
 * C# Structure of the Conlang JSON structure top level.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure for the top level of the Conlang JSON structure as described in
    /// https://github.com/RonBOakes/conlang_json/blob/main/doc/conlang_json_spec.pdf
    /// </summary>
    public class LanguageDescription
    {
        private string? _english_name;
        private string? _phonetic_characters;
        private string? _native_name_phonetic;
        private string? _native_name_english;
        private string? _preferred_voice;
        private Dictionary<string, string>? _preferred_voices;
        private string? _preferred_language;
        private bool? _derived;
        private bool? _declined;
        private List<string>? _noun_gender_list;
        private List<string>? _part_of_speech_list;
        private List<string>? _phoneme_inventory;
        private Dictionary<string, string[]>? _phonetic_inventory;
        private List<SoundMap>? _sound_map_list;
        private List<string>? _lexical_order_list;
        private Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>>? _affix_map;
        private Dictionary<string, DerivationalAffix>? _derivational_affix_map;
        private List<LexiconEntry>? _lexicon;
        private List<string>? _derived_word_list;
        private JsonObject? _metadata;

        /// <summary>
        /// Constructor used to build an empty LanguageDescription object.  All of the members are set to the default values. 
        /// </summary>
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

        /// <summary>
        /// Constructor used to build an LanguageDescription object by passing it values for the entries with parameters.
        /// </summary>
        /// <param name="english_name">This specifies the name of the conlang in English or another natural language.<br/>
        /// Required.</param>
        /// <param name="phonetic_characters">This specifies the symbols being used to express the phonetics for the conlang.  
        /// The valid values for this are ipa, x-sampa, and sampa.  The default value if this field is not present is ipa.<br/>
        /// Optional.<br/>Set to null if not present.</param>
        /// <param name="native_name_phonetic">This specifies the name of the conlang phonetically.  This will be expressed using 
        /// the symbology specified in the phonetic_characters field.<br/>Required.</param>
        /// <param name="native_name_english">This species is the Romanized (Latinized) name of the conlang, which is how the 
        /// conlang is expressed using characters in the Latin alphabet, possibly including diacritics.<br/>Optional.<br/>Set to 
        /// null if not present.</param>
        /// <param name="noun_gender_list"> This array contains a list of strings defining the noun genders in the language.  
        /// It is also recommended that two to three letters in this list be capitalized to be used as a short form for the 
        /// gender in the affix_map.<br/>Optional, Recommended.<br/>Set to null if not present.</param>
        /// <param name="preferred_voice">This specifies the preferred voice to be used when speaking text from the language.  
        /// This is used primarily by the Constructed Language (Conlang) Audio Sampling program when generating speech via 
        /// Amazon Polly, so it should specify an Amazon Polly voice.  This field is capital sensitive and should reflect 
        /// the Amazon Polly voice as listed by Amazon Web Services, normally using English proper noun style with the 
        /// first letter capitalized.<br/>Optional.<br/>Set to null if not present.</param>
        /// <param name="preferred_voices">This field contains a JSON object that has a map of speech-to-text applications 
        /// and their preferred voices.  Applications recognized at this time are: "Polly," "espeak-ng," and "Azure."  
        /// Both the key and resulting field are capital sensitive due to the underlying programs.<br/>Optional.<br/>
        /// Set to null if not present.</param>
        /// <param name="preferred_language">This specifies the preferred natural language for selecting phonemes for 
        /// speaking this conlang.  This should be specified using the XML:Lang specification from 
        /// https://www.ietf.org/rfc/rfc4267.txt<br/>Optional.<br/>Set to null if not present.</param>
        /// <param name="derived">This boolean specifies if the lexicon contains the results of processing the 
        /// derived_word_list (below).  If set to "true," the lexicon contains these words, and any application should 
        /// refrain from further attempts to derive these words.  If set to false, these words may need to be derived 
        /// before processing any conlang text.  The default value is "false."</param>
        /// <param name="declined">This boolean specifies if the lexicon contains the results of processing the 
        /// affix_map (below). If set to "true," the lexicon contains all words' declensions based upon the affix map.  
        /// It is application-dependent if words are sensibly declined.  For example, a declined lexicon may contain 
        /// declensions for nouns that do not properly match their gender.  The default value is "false."</param>
        /// <param name="part_of_speech_list">This array contains a list of strings with abbreviations for the parts 
        /// of speech that are contained within the lexicon.  Applications can use this to help index or search the 
        /// lexicon for key parts of speech.<br/>Optional.<br/>Set to null if not present.</param>
        /// <param name="phoneme_inventory">This array contains a list of strings containing the allowed or expected 
        /// phonemes in the conlang.  These strings will be expressed using the symbology specified in the phonetic_characters
        /// field.<br/>Optional.<br/>Set to null if not present.</param>
        /// <param name="sound_map_list">This array of objects contains the objects described below that are used to map between 
        /// the phonetic representation of the conlang and its Romanized or Latinized representation using the Latin alphabet.  
        /// Without this entry, tools cannot translate between phonetic and Latin formats.  This list is traversed in the order 
        /// presented when generating the Latin alphabet version of the word from the phonetic version.  When generating the 
        /// phonetic version of the word from the Latin alphabet version, it is traversed in the reverse of the order presented.
        /// <br/>Optional, Recommended.<br/>Set to null if not present.</param>
        /// <param name="lexical_order_list">This array of strings is used to sort the words in the conlang into a lexical order 
        /// after they have been converted into the Latin alphabet.  If not provided, then the natural lexical order provided by 
        /// the strings will be used for sorting.<br/>Optional. Recommended.<br/>Set to null if not present.</param>
        /// <param name="affix_map"> This object maps parts of speech, usually their abbreviations as listed in part_of_speech_list,
        /// to a list of objects described below that are used to decline root words of that part of speech.<br/>Optional, Recommended.
        /// <br/>Set to null if not present.</param>
        /// <param name="derivational_affix_map">This object contains keys that are used to aid in creating derived words.<br/>Optional. Recommended.
        /// <br/>Set to null if not present.</param>
        /// <param name="lexicon">This list of objects contains the lexicon, or dictionary, of the conlang.<br/>Required.</param>
        /// <param name="derived_word_list">This array of strings contains words that will be derived to add to the lexicon.  The 
        /// format for these strings is borrowed from Vulgarlang (https://www.vulgarlang.com/), 
        /// and may be updated later.<br/>Optional, Recommended.<br/>Set to null if not present.</param>
        /// <param name="metadata">This object contains an object where any program that edits the conlang object may add information 
        /// regarding the history of the conlang and its previous processing.  There is no exact format specified for this.  Programs 
        /// should not delete or alter metadata created by other programs but may add their own metadata or alter their metadata to 
        /// update their content<br/>Optional, Recommended.<br/>Set to null if not present.</param>
        public LanguageDescription(string? english_name, string? phonetic_characters, string? native_name_phonetic, string? native_name_english, List<string>? noun_gender_list, string? preferred_voice,
            Dictionary<string,string>? preferred_voices, string? preferred_language, bool? derived, bool? declined, List<string>? part_of_speech_list, List<string>? phoneme_inventory, List<SoundMap>? sound_map_list,
            List<string>? lexical_order_list, Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>>? affix_map, Dictionary<string, DerivationalAffix>? derivational_affix_map,
            List<LexiconEntry> lexicon, List<string>? derived_word_list, JsonObject metadata)
        {
            _english_name = english_name;
            _phonetic_characters = phonetic_characters;
            _native_name_phonetic = native_name_phonetic;
            _native_name_english = native_name_english;
            _preferred_voice = preferred_voice;
            _preferred_voices = preferred_voices;
            _preferred_language = preferred_language;
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

        /// <summary>
        /// This specifies the name of the conlang in English or another natural language.<br/>
        /// Required.
        /// </summary>
        public string english_name
        {
            get => _english_name ?? string.Empty;
            set => _english_name = value;
        }

        /// <summary>
        /// This specifies the symbols being used to express the phonetics for the conlang.  
        /// The valid values for this are ipa, x-sampa, and sampa.  The default value if this field is not present is ipa.<br/>
        /// </summary>
        public string phonetic_characters
        {
            get => _phonetic_characters ?? "ipa";
            set => _phonetic_characters = value;
        }

        /// <summary>
        /// This specifies the name of the conlang phonetically.  This will be expressed using the symbology specified in the phonetic_characters field.
        /// </summary>
        public string native_name_phonetic
        {
            get => _native_name_phonetic ?? string.Empty;
            set => _native_name_phonetic = value;
        }

        /// <summary>
        /// This species is the Romanized (Latinized) name of the conlang, which is how the 
        /// conlang is expressed using characters in the Latin alphabet, possibly including diacritics.<br/>Optional.
        /// </summary>
        public string native_name_english
        {
            get => _native_name_english ?? string.Empty;
            set => _native_name_english = value;
        }

        /// <summary>
        /// This specifies the preferred voice to be used when speaking text from the language.  
        /// This is used primarily by the Constructed Language (Conlang) Audio Sampling program when generating speech via 
        /// Amazon Polly, so it should specify an Amazon Polly voice.  This field is capital sensitive and should reflect 
        /// the Amazon Polly voice as listed by Amazon Web Services, normally using English proper noun style with the 
        /// first letter capitalized.<br/>Optional.
        /// </summary>
        public string preferred_voice
        {
            get => _preferred_voice ?? string.Empty;
            set => _preferred_voice = value;
        }

        /// <summary>
        /// This field contains a JSON object that has a map of speech-to-text applications and their preferred voices.  
        /// Applications recognized at this time are: "Polly," "espeak-ng," and "Azure."  Both the key and resulting field 
        /// are capital sensitive due to the underlying programs.
        /// </summary>
        public Dictionary<string,string> preferred_voices
        {
            get => _preferred_voices ?? new Dictionary<string, string>();
            set => _preferred_voices = value;
        }

        /// <summary>
        /// This specifies the preferred natural language for selecting phonemes for 
        /// speaking this conlang.  This should be specified using the XML:Lang specification from 
        /// https://www.ietf.org/rfc/rfc4267.txt<br/>Optional.
        /// </summary>
        public string preferred_language
        {
            get => _preferred_language ?? string.Empty;
            set => _preferred_language = value;
        }

        /// <summary>
        /// This boolean specifies if the lexicon contains the results of processing the 
        /// derived_word_list (below).  If set to "true," the lexicon contains these words, and any application should 
        /// refrain from further attempts to derive these words.  If set to false, these words may need to be derived 
        /// before processing any conlang text.  The default value is "false."
        /// </summary>
        public bool derived
        {
            get => _derived ?? false;
            set => _derived = value;
        }

        /// <summary>
        /// This boolean specifies if the lexicon contains the results of processing the 
        /// affix_map (below). If set to "true," the lexicon contains all words' declensions based upon the affix map.  
        /// It is application-dependent if words are sensibly declined.  For example, a declined lexicon may contain 
        /// declensions for nouns that do not properly match their gender.  The default value is "false."
        /// </summary>
        public bool declined
        {
            get => _declined ?? false;
            set => _declined = value;
        }

        /// <summary>
        /// This array contains a list of strings defining the noun genders in the language.  It is also recommended 
        /// that two to three letters in this list be capitalized to be used as a short form for the gender in the affix_map.
        /// <br/>Optional. Recommended.
        /// </summary>
        public List<string> noun_gender_list
        {
            get => _noun_gender_list ?? [];
            set => _noun_gender_list = value;
        }

        /// <summary>
        /// This array contains a list of strings with abbreviations for the parts 
        /// of speech that are contained within the lexicon.  Applications can use this to help index or search the 
        /// lexicon for key parts of speech.<br/>Optional.
        /// </summary>
        public List<string> part_of_speech_list
        {
            get => _part_of_speech_list ?? [];
            set => _part_of_speech_list = value;
        }

        /// <summary>
        /// This array contains a list of strings containing the allowed or expected 
        /// phonemes in the conlang.  These strings will be expressed using the symbology specified in the phonetic_characters
        /// field.<br/>Optional.
        /// </summary>
        public List<string> phoneme_inventory
        {
            get => _phoneme_inventory ?? [];
            set => _phoneme_inventory = value;
        }

        /// <summary>
        /// This object will contain the individual phonetic sounds (phones) and diphthongs found in the language.  The keys of this 
        /// object (all optional, but recommended) are "p_consonants" for the pulmonic consonants, "np_consonants" for the non-pulmonic 
        /// consonants, "vowels," "v\_diphthongs."  Other keys can be added if needed.  Each key will reference an array of strings 
        /// containing either a single phone, represented by a single phonetic character, a character followed by a diacritic, or a 
        /// pair of characters that may or may not be linked.  These strings will be expressed using the symbology specified in the 
        /// phonetic_characters field.
        /// </summary>
        public Dictionary<string, string[]> phonetic_inventory
        {
            get => _phonetic_inventory ?? new Dictionary<string, string[]>();
            set => _phonetic_inventory = value;
        }

        /// <summary>
        /// This array of objects contains the objects described below that are used to map between 
        /// the phonetic representation of the conlang and its Romanized or Latinized representation using the Latin alphabet.  
        /// Without this entry, tools cannot translate between phonetic and Latin formats.  This list is traversed in the order 
        /// presented when generating the Latin alphabet version of the word from the phonetic version.  When generating the 
        /// phonetic version of the word from the Latin alphabet version, it is traversed in the reverse of the order presented.
        /// <br/>Optional, Recommended.
        /// </summary>
        public List<SoundMap> sound_map_list
        {
            get => _sound_map_list ?? [];
            set => _sound_map_list = value;
        }

        /// <summary>
        /// This array of strings is used to sort the words in the conlang into a lexical order 
        /// after they have been converted into the Latin alphabet.  If not provided, then the natural lexical order provided by 
        /// the strings will be used for sorting.<br/>Optional. Recommended.
        /// </summary>
        public List<string> lexical_order_list
        {
            get => _lexical_order_list ?? []; 
            set 
            {
                _lexical_order_list = value;
                LexiconEntry.LexicalOrderList = value;
            }
        }

        /// <summary>
        /// This object maps parts of speech, usually their abbreviations as listed in part_of_speech_list,
        /// to a list of objects described below that are used to decline root words of that part of speech.<br/>Optional, Recommended.
        /// </summary>
        public Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>> affix_map
        {
            get => _affix_map ?? [];
            set => _affix_map = value;
        }

        /// <summary>
        /// This object contains keys that are used to aid in creating derived words.<br/>Optional. Recommended.
        /// </summary>
        public Dictionary<string, DerivationalAffix> derivational_affix_map
        {
            get => _derivational_affix_map ?? [];
            set =>_derivational_affix_map = value;
        }

        /// <summary>
        /// This list of objects contains the lexicon, or dictionary, of the conlang.<br/>Required.
        /// </summary>
        public List<LexiconEntry> lexicon
        {
            get => _lexicon ?? []; 
            set => _lexicon = value;
        }

        /// <summary>
        /// This array of strings contains words that will be derived to add to the lexicon.  The 
        /// format for these strings is borrowed from Vulgarlang (https://www.vulgarlang.com/), 
        /// and may be updated later.<br/>Optional, Recommended.
        /// </summary>
        public List<string> derived_word_list
        {
            get => _derived_word_list ?? []; 
            set => _derived_word_list = value;
        }

        /// <summary>
        /// This object contains an object where any program that edits the conlang object may add information 
        /// regarding the history of the conlang and its previous processing.  There is no exact format specified for this.  Programs 
        /// should not delete or alter metadata created by other programs but may add their own metadata or alter their metadata to 
        /// update their content<br/>Optional, Recommended.
        /// </summary>
        public JsonObject metadata
        { 
            get => _metadata ?? []; 
            set => _metadata = value;
        }

    }
}
