/*
 * C# Structure for Conlang JSON Lexicon Entry Objects
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
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure that corresponds to the individual lexicon entries.  Each object
    /// will encapsulate a single word in the lexicon.
    /// </summary>
    public class LexiconEntry : IEquatable<LexiconEntry?>
    {
        private string _phonetic;
        private string _spelled;
        private string _english;
        private string _part_of_speech;
        private List<string> _declensions = [];
        private bool? _derived_word;
        private bool? _declined_word;
        private JsonObject? _metadata;

        private static List<string>? _lexicalOrderList;

        /// <summary>
        /// Constructor used to build an LexiconEntry object by passing it values for all of the entries.<br/>
        /// Note that this constructor method does not prevent invalid configurations based on the 
        /// parameter listings below.
        /// </summary>
        /// <param name="phonetic">Required if spelled is not present. This string contains the phonetic 
        /// representation of the word.  This will be expressed using the symbology specified in the phonetic_characters
        /// field at the Top Level.<br/> Optional.  Required if spelled is not present.</param>
        /// <param name="spelled">This string contains the Romanized or Latinized representation of the word.<br/> Optional. 
        /// Required if phonetic is not present.</param>
        /// <param name="english">This string contains the English or other natural language equivalent to the word or its 
        /// definition in English or another natural language.<br/>Required.</param>
        /// <param name="part_of_speech">This string contains the part of speech for this word.  This can, and should, be 
        /// one of the abbreviations found in the part_of_speech_list field at the Top Level.<br/>Required.</param>
        /// <param name="declensions">This array of strings contains the declensions used when declining this word from 
        /// its root form.  If the word is not declined, that is, if it is the root word, this array must contain a single 
        /// entry, the string "root."<br/>Required.</param>
        /// <param name="derived_word">This boolean is set to true if this word was created by deriving a root word using 
        /// words in the derived_word_list with the aid of the affixes in the derivation_affix_map}.<br/>Required.</param>
        /// <param name="declined_word">This boolean is set to true if the word was created by declining a root word using 
        /// the rules in the affix_map.<br/>Required.</param>
        /// <param name="metadata">This object contains an object where any program that edits the conlang object may add 
        /// information regarding the word's history and it's source.  There is no exact format specified for this.  Programs 
        /// should not delete or alter metadata created by other programs but may add their own metadata or alter their metadata 
        /// to update their content.<br/>Optional, Recommended.</param>
        public LexiconEntry(string phonetic, string spelled, string english, string part_of_speech, List<string> declensions, bool? derived_word, bool? declined_word, JsonObject? metadata)
        {
            this._phonetic = phonetic;
            this._spelled = spelled;
            this._english = english;
            this._part_of_speech = part_of_speech;
            this._declensions = declensions;
            this._derived_word = derived_word;
            this._declined_word = declined_word;
            this._metadata = metadata;
        }

        /// <summary>
        /// Constructor used to build an empty LexiconEntry object.  All of the members are set to the default values.
        /// </summary>
        public LexiconEntry()
        {
            _phonetic = "";
            _spelled = "";
            _english = "";
            _part_of_speech = "";
            _declensions = [];
            _derived_word = false;
            _declined_word = false;
        }

        /// <summary>
        /// Required if spelled is not present. This string contains the phonetic 
        /// representation of the word.  This will be expressed using the symbology specified in the phonetic_characters
        /// field at the Top Level.<br/> Optional.  Required if spelled is not present.
        /// </summary>
        public string phonetic
        {
            get { return _phonetic; }
            set { _phonetic = value; }
        }

        /// <summary>
        /// This string contains the Romanized or Latinized representation of the word.<br/> Optional. 
        /// Required if phonetic is not present.
        /// </summary>
        public string spelled
        { 
            get { return _spelled; } 
            set { _spelled = value; } 
        }

        /// <summary>
        /// This string contains the English or other natural language equivalent to the word or its 
        /// definition in English or another natural language.<br/>Required.
        /// </summary>
        public string english
        { 
            get { return _english; } 
            set { _english = value; } 
        }

        /// <summary>
        /// This string contains the part of speech for this word.  This can, and should, be 
        /// one of the abbreviations found in the part_of_speech_list field at the Top Level.<br/>Required.
        /// </summary>
        public string part_of_speech
        {
            get { return _part_of_speech; }
            set { _part_of_speech = value; }
        }

        /// <summary>
        /// This array of strings contains the declensions used when declining this word from 
        /// its root form.  If the word is not declined, that is, if it is the root word, this array must contain a single 
        /// entry, the string "root."<br/>Required.
        /// </summary>
        public List<string> declensions
        { 
            get { return _declensions; } 
            set { _declensions = value; } 
        }

        /// <summary>
        /// This boolean is set to true if this word was created by deriving a root word using 
        /// words in the derived_word_list with the aid of the affixes in the derivation_affix_map}.<br/>Required.
        /// </summary>
        public bool? derived_word 
        { 
            get { return _derived_word; } 
            set { _derived_word = value; } 
        }

        /// <summary>
        /// This boolean is set to true if the word was created by declining a root word using 
        /// the rules in the affix_map.<br/>Required.
        /// </summary>
        public bool? declined_word
        {
            get { return _declined_word; }
            set { _declined_word = value; }
        }

        /// <summary>
        /// This object contains an object where any program that edits the conlang object may 
        /// add information regarding the word's history and its source.  There is no exact format specified 
        /// for this.  Programs should not delete or alter metadata created by other programs but may add their 
        /// own metadata or alter their metadata to update their content.<br/>Optional, Recommended.
        /// </summary>
        public JsonObject metadata
        {
            get
            {
                return _metadata ??= [];
            }
            set
            {
                _metadata = value;
            }
        }

        /// <summary>
        /// Lexical order for sorting LexiconEntries by their spelled values.
        /// </summary>
        internal static List<string> LexicalOrderList
        {
            get
            {
                return _lexicalOrderList ?? [];
            }
            set
            {
                _lexicalOrderList = value;
            }
        }

        /// <summary>
        /// Performs a shallow copy of the LexiconEntry object.
        /// </summary>
        /// <returns></returns>
        public LexiconEntry copy()
        {
            LexiconEntry copy = new LexiconEntry(phonetic, spelled, english, part_of_speech, declensions, derived_word, declined_word, metadata);
            if (metadata != null)
            {
                string metadataString = JsonSerializer.Serialize<JsonObject>(metadata);
#pragma warning disable CS8601 // Possible null reference assignment.
                copy.metadata = JsonSerializer.Deserialize<JsonObject>(metadataString);
#pragma warning restore CS8601 // Possible null reference assignment.
            }
            return copy;
        }

        /// <summary>
        /// Determines if this LexiconEntry is the same as another object.
        /// </summary>
        /// <param name="obj">Object to be compared to this LexiconEntry.</param>
        /// <returns>true if the objects are the same, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as LexiconEntry);
        }

        /// <summary>
        /// Determines if this LexiconEntry is the same as another LexiconEntry.  This comparison is done
        /// based on the contents of the objects, not the object's identities.
        /// </summary>
        /// <param name="other">LexiconEntry object to be compared to this LexiconEntry.</param>
        /// <returns>true if the LexiconEntry objects represent the same data.</returns>
        public bool Equals(LexiconEntry? other)
        {
            return other is not null &&
                   _phonetic == other._phonetic &&
                   _spelled == other._spelled &&
                   _english == other._english &&
                   _part_of_speech == other._part_of_speech &&
                   EqualityComparer<List<string>>.Default.Equals(_declensions, other._declensions) &&
                   _derived_word == other._derived_word &&
                   _declined_word == other._declined_word &&
                   EqualityComparer<JsonObject?>.Default.Equals(_metadata, other._metadata);
        }

        /// <summary>
        /// Generates a hash code for a LexiconEntry object based on its data.
        /// </summary>
        /// <returns>Hash Code for this LexiconEntry object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(_phonetic, _spelled, _english, _part_of_speech, _declensions, _derived_word, _declined_word, _metadata);
        }

        /// <summary>
        /// Class to be used when sorting a list of LexiconEntry objects by their spelling.
        /// </summary>
        public class LexicalOrderCompSpelling : IComparer<LexiconEntry>
        {
            /// <summary>
            /// Determines the ordering of the two supplied LexiconEntry objects using their spelled
            /// property and the static LexicalOrderList property.
            /// </summary>
            /// <param name="x">First LexiconEntry object for the comparison</param>
            /// <param name="y">Second LexiconEntry object for the comparison</param>
            /// <returns>Per IComparer&lt;T&gt;.Compare.</returns>
            /// <exception cref="NotSupportedException">If either supplied object is null.</exception>
            public int Compare(LexiconEntry? x, LexiconEntry? y)
            {
                if(x == null || y == null)
                {
                    throw new NotSupportedException();
                }
                double xVal = lexicalIndex(x.spelled);
                double yVal = lexicalIndex(y.spelled);

                if (xVal < yVal)
                {
                    return -1;
                }
                else if (xVal == yVal)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            private double lexicalIndex(string item)
            {
                item = item.ToLower();

                double retVal = 0.0;
                int charPos = 0;
                while (charPos < item.Length)
                {
                    string evalChar;
                    int charIndex = -charPos;
                    if (charPos < item.Length - 2)
                    {
                        string nextCharStr = item.Substring(charPos + 1, 1);
                        char nextChar = nextCharStr[0];
                        if ((nextChar >= '\u0300') && (nextChar <= '\u036f'))
                        {
                            evalChar = item.Substring(charPos, 2);
                            charPos += 2;
                        }
                        else
                        {
                            evalChar = item.Substring(charPos, 1);
                            charPos += 1;
                        }
                    }
                    else
                    {
                        evalChar = item.Substring(charPos, 1);
                        charPos += 1;
                    }

                    if ((evalChar != "ˈ") && ((evalChar != " ")))
                    {
                        retVal += lexicalValue(evalChar) * Math.Pow(10.0, (double)charIndex);
                    }
                }


                return retVal;
            }

            private double lexicalValue(string value)
            {
                string charBase = value.Substring(0, 1);
                double lexVal;
                if(LexicalOrderList.Contains(value))
                {
                    int index = 0;
                    foreach (string s in LexicalOrderList)
                    {
                        if (s == charBase)
                        {
                            break;
                        }
                        index++;
                    }
                    lexVal = index * 100.0 ;
                    if (value.Length > 1)
                    {
                        string diacritic = value.Substring(1);
                        lexVal += Math.Round(((double)(diacritic[0] - '\u0300')) / (double)('\u036f' - '\u0300')); ;
                    }
                }
                else
                {
                    lexVal = LexicalOrderList.Count + 1.0;
                }

                return lexVal;
            }
        }

        /// <summary>
        /// Class to be used when sorting a list of LexiconEntry objects by their English equivalent.
        /// </summary>
        public class LexicalOrderCompEnglish : IComparer<LexiconEntry>
        {
            /// <summary>
            /// Determines the ordering of the two supplied LexiconEntry objects using their english
            /// property.
            /// </summary>
            /// <param name="x">First LexiconEntry object for the comparison</param>
            /// <param name="y">Second LexiconEntry object for the comparison</param>
            /// <returns>Per IComparer&lt;T&gt;.Compare.</returns>
            /// <exception cref="NotSupportedException">If either supplied object is null.</exception>
            int IComparer<LexiconEntry>.Compare(LexiconEntry? x, LexiconEntry? y)
            {
                if(x == null || y == null)
                {
                    throw new NotSupportedException();
                }
                StringComparer strComp = StringComparer.Create(System.Globalization.CultureInfo.CurrentCulture, true);

                return strComp.Compare(x.english, y.english);
            }
        }

        /// <summary>
        /// Operator for comparing two LexiconEntry objects.  Only know to work for comparing
        /// with null.
        /// </summary>
        /// <param name="left">Left hand side</param>
        /// <param name="right">Right hand side</param>
        /// <returns>true if they represent the same object.</returns>
        public static bool operator ==(LexiconEntry? left, LexiconEntry? right)
        {
            return EqualityComparer<LexiconEntry>.Default.Equals(left, right);
        }

        /// <summary>
        /// Operator for comparing two LexiconEntry objects.  Only know to work for comparing
        /// with null.
        /// </summary>
        /// <param name="left">Left hand side</param>
        /// <param name="right">Right hand side</param>
        /// <returns>true if they represent different same objects.</returns>
        public static bool operator !=(LexiconEntry? left, LexiconEntry? right)
        {
            return !(left == right);
        }
    }
}
