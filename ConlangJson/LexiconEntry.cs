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

        public string phonetic
        {
            get { return _phonetic; }
            set { _phonetic = value; }
        }

        public string spelled
        { 
            get { return _spelled; } 
            set { _spelled = value; } 
        }

        public string english
        { 
            get { return _english; } 
            set { _english = value; } 
        }

        public string part_of_speech
        {
            get { return _part_of_speech; }
            set { _part_of_speech = value; }
        }

        public List<string> declensions
        { 
            get { return _declensions; } 
            set { _declensions = value; } 
        }

        public bool? derived_word 
        { 
            get { return _derived_word; } 
            set { _derived_word = value; } 
        }

        public bool? declined_word
        {
            get { return _declined_word; }
            set { _declined_word = value; }
        }

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

        public LexiconEntry copy()
        {
            LexiconEntry copy = new LexiconEntry(phonetic, spelled, english, part_of_speech, declensions, derived_word, declined_word, metadata);
            if (metadata != null)
            {
                string metatdataString = JsonSerializer.Serialize<JsonObject>(metadata);
#pragma warning disable CS8601 // Possible null reference assignment.
                copy.metadata = JsonSerializer.Deserialize<JsonObject>(metatdataString);
#pragma warning restore CS8601 // Possible null reference assignment.
            }
            return copy;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LexiconEntry);
        }

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

        public override int GetHashCode()
        {
            return HashCode.Combine(_phonetic, _spelled, _english, _part_of_speech, _declensions, _derived_word, _declined_word, _metadata);
        }

        public class LexicalOrderCompSpelling : IComparer<LexiconEntry>
        {
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

        public class LexicalOrderCompEnglish : IComparer<LexiconEntry>
        {
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

        public static bool operator ==(LexiconEntry? left, LexiconEntry? right)
        {
            return EqualityComparer<LexiconEntry>.Default.Equals(left, right);
        }

        public static bool operator !=(LexiconEntry? left, LexiconEntry? right)
        {
            return !(left == right);
        }
    }
}
