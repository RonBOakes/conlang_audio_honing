/*
 * C# Representation of the Conlang JSON object Sound Map List Entry Objects
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
namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure that corresponds to the Sound Map Entries.  Each object of this class
    /// will encapsulate one entry in the higher-level Sound Map.
    /// </summary>
    public class SoundMap
    {
        private string _phoneme;
        private string _romanization;
        private string _spelling_regex;
        private string _pronounciation_regex;

        /// <summary>
        /// Constructor used to build an empty SoundMap object.  All of the members are set to the default values.
        /// </summary>
        public SoundMap()
        {
            this._phoneme = string.Empty;
            this._romanization = string.Empty;
            this._spelling_regex = string.Empty;
            this._pronounciation_regex = string.Empty;
        }

        /// <summary>
        /// Constructor used to build an SoundMap object by passing it values for all of the entries.<br/>
        /// Note that this constructor method does not prevent invalid configurations based on the 
        /// parameter listings below.
        /// </summary>
        /// <param name="phoneme">This string contains the values that will be substituted for text matched 
        /// by the pronounciation_regex when converting text from Romanized or Latinized to phonetic representation.  
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.  
        /// The Perl standard of $n is used if group substitutions are included.<br/>Optional, Recommended, Required 
        /// if pronounciation_regex is present.<br/>Set to an empty string if not present.</param>
        /// <param name="romanization">This string contains the value that will be substituted for the text matched 
        /// by the spelling_regex when converting text from phonetic representation.  The Perl standard of $n is used 
        /// if group substitutions are included.<br/>Optional, Recommended, Required if spelling_regex is present.
        /// <br/>Set to an empty string if not present.</param>
        /// <param name="spelling_regex">This string contains a generalized regular expression used to match a portion 
        /// of the phonetic representation of a word in the conlang with a specific Romanization or Latinization.  The 
        /// matched text will then be replaced with the value from the romanization.  This will be expressed using the 
        /// symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required 
        /// if romanization is present.<br/>Set to an empty string if not present.</param>
        /// <param name="pronounciation_regex">This string contains a generalized regular expression that matches a 
        /// portion of a word's Romanized or Latinized version in the conlang with a specific phonetic representation.  
        /// The matched text will then be replaced with the value from the phoneme below.<br/> Optional, Recommended, 
        /// Required if phoneme is present.<br/>Set to an empty string if not present.</param>
        public SoundMap(string phoneme, string romanization, string spelling_regex, string pronounciation_regex)
        {
            this._phoneme = phoneme;
            this._romanization = romanization;
            this._spelling_regex = spelling_regex;
            this._pronounciation_regex = pronounciation_regex;
        }

        /// <summary>
        /// This string contains the values that will be substituted for text matched 
        /// by the pronounciation_regex when converting text from Romanized or Latinized to phonetic representation.  
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.  
        /// The Perl standard of $n is used if group substitutions are included.<br/>Optional, Recommended, Required 
        /// if pronounciation_regex is present.<br/>Set to an empty string if not present.
        /// </summary>
        public string phoneme
        {
            get { return _phoneme; }
            set { _phoneme = value; }
        }

        /// <summary>
        /// This string contains the value that will be substituted for the text matched 
        /// by the spelling_regex when converting text from phonetic representation.  The Perl standard of $n is used 
        /// if group substitutions are included.<br/>Optional, Recommended, Required if spelling_regex is present.
        /// <br/>Set to an empty string if not present.
        /// </summary>
        public string romanization
        {
            get { return _romanization; }
            set { _romanization = value; }
        }

        /// <summary>
        /// This string contains a generalized regular expression used to match a portion 
        /// of the phonetic representation of a word in the conlang with a specific Romanization or Latinization.  The 
        /// matched text will then be replaced with the value from the romanization.  This will be expressed using the 
        /// symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required 
        /// if romanization is present.<br/>Set to an empty string if not present.
        /// </summary>
        public string spelling_regex
        {
            get { return _spelling_regex; }
            set { _spelling_regex = value;}
        }

        /// <summary>
        /// This string contains a generalized regular expression that matches a 
        /// portion of a word's Romanized or Latinized version in the conlang with a specific phonetic representation.  
        /// The matched text will then be replaced with the value from the phoneme below.<br/> Optional, Recommended, 
        /// Required if phoneme is present.<br/>Set to an empty string if not present.
        /// </summary>
        public string pronounciation_regex
        {
            get { return _pronounciation_regex; }
            set { _pronounciation_regex = value; }
        }
    }
}