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
using System.Text.Json.Nodes;

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
        private Dictionary<string, string>? _preferred_voices;
        private string? _preferred_language;
        private bool? _derived;
        private bool? _declined;
        private List<string>? _noun_gender_list;
        private List<string>? _part_of_speech_list;
        private List<string>? _phoneme_inventory;
        private string? _word_order;
        private string? _adjective_position;
        private string? _pre_post_position;
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
            version = double.MinValue;
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
        /// The structure version read in from the JSON object.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public double version { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This specifies the name of the conlang in English or another natural language.<br/>
        /// Required.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string english_name
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _english_name ?? string.Empty;
            set => _english_name = value;
        }

        /// <summary>
        /// This specifies the symbols being used to express the phonetics for the conlang.  
        /// The valid values for this are ipa, x-sampa, and sampa.  The default value if this field is not present is ipa.<br/>
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string phonetic_characters
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _phonetic_characters ?? "ipa";
            set => _phonetic_characters = value;
        }

        /// <summary>
        /// This specifies the name of the conlang phonetically.  This will be expressed using the symbology specified in the phonetic_characters field.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string native_name_phonetic
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _native_name_phonetic ?? string.Empty;
            set => _native_name_phonetic = value;
        }

        /// <summary>
        /// This species is the Romanized (Latinized) name of the conlang, which is how the 
        /// conlang is expressed using characters in the Latin alphabet, possibly including diacritics.<br/>Optional.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string native_name_english
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _native_name_english ?? string.Empty;
            set => _native_name_english = value;
        }

        /// <summary>
        /// This field contains a JSON object that has a map of speech-to-text applications and their preferred voices.  
        /// Applications recognized at this time are: "Polly," "espeak-ng," and "Azure."  Both the key and resulting field 
        /// are capital sensitive due to the underlying programs.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public Dictionary<string, string> preferred_voices
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _preferred_voices ?? [];
            set => _preferred_voices = value;
        }

        /// <summary>
        /// This specifies the preferred natural language for selecting phonemes for 
        /// speaking this conlang.  This should be specified using the XML:Lang specification from 
        /// https://www.ietf.org/rfc/rfc4267.txt<br/>Optional.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string preferred_language
#pragma warning restore IDE1006 // Naming Styles
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
#pragma warning disable IDE1006 // Naming Styles
        public bool derived
#pragma warning restore IDE1006 // Naming Styles
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
#pragma warning disable IDE1006 // Naming Styles
        public bool declined
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _declined ?? false;
            set => _declined = value;
        }

        /// <summary>
        /// This array contains a list of strings defining the noun genders in the language.  It is also recommended 
        /// that two to three letters in this list be capitalized to be used as a short form for the gender in the affix_map.
        /// <br/>Optional. Recommended.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public List<string> noun_gender_list
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _noun_gender_list ?? [];
            set => _noun_gender_list = value;
        }

        /// <summary>
        /// This array contains a list of strings with abbreviations for the parts 
        /// of speech that are contained within the lexicon.  Applications can use this to help index or search the 
        /// lexicon for key parts of speech.<br/>Optional.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public List<string> part_of_speech_list
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _part_of_speech_list ?? [];
            set => _part_of_speech_list = value;
        }

        /// <summary>
        /// This array contains a list of strings containing the allowed or expected 
        /// phonemes in the conlang.  These strings will be expressed using the symbology specified in the phonetic_characters
        /// field.<br/>Optional.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public List<string> phoneme_inventory
#pragma warning restore IDE1006 // Naming Styles
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
#pragma warning disable IDE1006 // Naming Styles
        public Dictionary<string, string[]> phonetic_inventory
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _phonetic_inventory ?? [];
            set => _phonetic_inventory = value;
        }

        /// <summary>
        /// This string will contain the language’s word order expressed using the letters “S” for subject, “V” for verb, and “O” 
        /// for object. The language may take on any of the six possible arrangements for these.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string word_order
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _word_order ?? "SVO";
            set => _word_order = value;
        }

        /// <summary>
        /// Defines the word orders for a language as an enum.
        /// </summary>
        public enum WordOrders
        {
            /// <summary>
            /// Subject, Verb, Object
            /// </summary>
            SVO,
            /// <summary>
            /// Subject, Object, Verb
            /// </summary>
            SOV,
            /// <summary>
            /// Verb, Subject, Object
            /// </summary>
            VSO,
            /// <summary>
            /// Verb, Object, Subject
            /// </summary>
            VOS,
            /// <summary>
            /// Object, Subject, Verb
            /// </summary>
            OSV,
            /// <summary>
            /// Object, Verb Subject.
            /// </summary>
            OVS,
        }

        /// <summary>
        /// Get the Word Order as an Enum.
        /// </summary>
        public LanguageDescription.WordOrders WordOrder()
        {
            return word_order switch
            {
                "SVO" => WordOrders.SVO,
                "SOV" => WordOrders.SOV,
                "VSO" => WordOrders.VSO,
                "VOS" => WordOrders.VOS,
                "OSV" => WordOrders.OSV,
                "OVS" => WordOrders.OVS,
                _ => WordOrders.SVO,
            };
        }

        /// <summary>
        /// This string indicates where adjectives are placed in this language in relationship to the noun that the adjective modifies. 
        /// “Before” indicates that the adjective precedes the noun, as occurs in English, and “After” indicates that the adjective 
        /// follows the noun it modifies, as occurs in French.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string adjective_position
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _adjective_position ?? "Before";
            set => _adjective_position = value;
        }

        /// <summary>
        /// Enumerated value for the Adjective Position
        /// </summary>
        public enum AdjectivePositions
        {
            /// <summary>
            /// Adjectives come before the noun
            /// </summary>
            BEFORE,
            /// <summary>
            /// Adjectives come after the noun
            /// </summary>
            AFTER,
        }

        /// <summary>
        /// Gets the adjective position as an Enum.
        /// </summary>
        /// <returns></returns>
        public LanguageDescription.AdjectivePositions AdjectivePosition()
        {
            return adjective_position.ToLower() switch
            {
                "before" => AdjectivePositions.BEFORE,
                "after" => AdjectivePositions.AFTER,
                _ => AdjectivePositions.BEFORE,
            };
        }


        /// <summary>
        /// This is used to specify if the language has prepositions or postpositions for its adposition.In languages with postpositions, 
        /// the adposition comes after the noun phrase, for example, “a key with.” In languages with prepositions, the adposition proceeds 
        /// the noun phrase, for example, “with a key.” The valid values are “preposition,” and “postposition.”
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string pre_post_position
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _pre_post_position ?? "preposition";
            set => _pre_post_position = value;
        }

        /// <summary>
        /// Enumerated Values for adposition.
        /// </summary>
        public enum PrePostPositions
        {
            /// <summary>
            /// Preposition
            /// </summary>
            PREPOSITION,
            /// <summary>
            /// Postposition.
            /// </summary>
            POSTPOSITION,
        }


        /// <summary>
        /// Return the adposition as an enumerated value.
        /// </summary>
        /// <returns></returns>
        public LanguageDescription.PrePostPositions PrePostPosition()
        {
            return pre_post_position.ToLower() switch
            {
                "preposition" => PrePostPositions.PREPOSITION,
                "postposition" => PrePostPositions.POSTPOSITION,
                _ => PrePostPositions.PREPOSITION,
            };
        }

        /// <summary>
        /// This array of objects contains the objects described below that are used to map between 
        /// the phonetic representation of the conlang and its Romanized or Latinized representation using the Latin alphabet.  
        /// Without this entry, tools cannot translate between phonetic and Latin formats.  This list is traversed in the order 
        /// presented when generating the Latin alphabet version of the word from the phonetic version.  When generating the 
        /// phonetic version of the word from the Latin alphabet version, it is traversed in the reverse of the order presented.
        /// <br/>Optional, Recommended.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public List<SoundMap> sound_map_list
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _sound_map_list ?? [];
            set => _sound_map_list = value;
        }

        /// <summary>
        /// This array of strings is used to sort the words in the conlang into a lexical order 
        /// after they have been converted into the Latin alphabet.  If not provided, then the natural lexical order provided by 
        /// the strings will be used for sorting.<br/>Optional. Recommended.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public List<string> lexical_order_list
#pragma warning restore IDE1006 // Naming Styles
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
#pragma warning disable IDE1006 // Naming Styles
        public Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>> affix_map
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _affix_map ?? [];
            set => _affix_map = value;
        }

        /// <summary>
        /// This object contains keys that are used to aid in creating derived words.<br/>Optional. Recommended.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public Dictionary<string, DerivationalAffix> derivational_affix_map
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _derivational_affix_map ?? [];
            set => _derivational_affix_map = value;
        }

        /// <summary>
        /// This list of objects contains the lexicon, or dictionary, of the conlang.<br/>Required.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public List<LexiconEntry> lexicon
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _lexicon ?? [];
            set => _lexicon = value;
        }

        /// <summary>
        /// This array of strings contains words that will be derived to add to the lexicon.  The 
        /// format for these strings is borrowed from Vulgarlang (https://www.vulgarlang.com/), 
        /// and may be updated later.<br/>Optional, Recommended.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public List<string> derived_word_list
#pragma warning restore IDE1006 // Naming Styles
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
#pragma warning disable IDE1006 // Naming Styles
        public JsonObject metadata
#pragma warning restore IDE1006 // Naming Styles
        {
            get => _metadata ?? [];
            set => _metadata = value;
        }

    }
}
