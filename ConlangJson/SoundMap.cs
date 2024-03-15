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
using System.Text;

namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure that corresponds to the Sound Map Entries.  Each object of this class
    /// will encapsulate one entry in the higher-level Sound Map.
    /// </summary>
    public sealed class SoundMap : IEquatable<SoundMap?>
    {

        /// <summary>
        /// Constructor used to build an empty SoundMap object.  All of the members are set to the default values.
        /// </summary>
        public SoundMap()
        {
            this.phoneme = string.Empty;
            this.romanization = string.Empty;
            this.spelling_regex = string.Empty;
            this.pronunciation_regex = string.Empty;
        }

        /// <summary>
        /// Constructor used to build an SoundMap object by passing it values for all of the entries.<br/>
        /// Note that this constructor method does not prevent invalid configurations based on the 
        /// parameter listings below.
        /// </summary>
        /// <param name="phoneme">This string contains the values that will be substituted for text matched 
        /// by the pronunciation_regex when converting text from Romanized or Latinized to phonetic representation.  
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.  
        /// The Perl standard of $n is used if group substitutions are included.<br/>Optional, Recommended, Required 
        /// if pronunciation_regex is present.<br/>Set to an empty string if not present.</param>
        /// <param name="romanization">This string contains the value that will be substituted for the text matched 
        /// by the spelling_regex when converting text from phonetic representation.  The Perl standard of $n is used 
        /// if group substitutions are included.<br/>Optional, Recommended, Required if spelling_regex is present.
        /// <br/>Set to an empty string if not present.</param>
        /// <param name="spelling_regex">This string contains a generalized regular expression used to match a portion 
        /// of the phonetic representation of a word in the conlang with a specific Romanization or Latinization.  The 
        /// matched text will then be replaced with the value from the romanization.  This will be expressed using the 
        /// symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required 
        /// if romanization is present.<br/>Set to an empty string if not present.</param>
        /// <param name="pronunciation_regex">This string contains a generalized regular expression that matches a 
        /// portion of a word's Romanized or Latinized version in the conlang with a specific phonetic representation.  
        /// The matched text will then be replaced with the value from the phoneme below.<br/> Optional, Recommended, 
        /// Required if phoneme is present.<br/>Set to an empty string if not present.</param>
        public SoundMap(string phoneme, string romanization, string spelling_regex, string pronunciation_regex)
        {
            this.phoneme = phoneme;
            this.romanization = romanization;
            this.spelling_regex = spelling_regex;
            this.pronunciation_regex = pronunciation_regex;
        }

        /// <summary>
        /// This string contains the values that will be substituted for text matched 
        /// by the pronunciation_regex when converting text from Romanized or Latinized to phonetic representation.  
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.  
        /// The Perl standard of $n is used if group substitutions are included.<br/>Optional, Recommended, Required 
        /// if pronunciation_regex is present.<br/>Set to an empty string if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string phoneme { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This string contains the value that will be substituted for the text matched 
        /// by the spelling_regex when converting text from phonetic representation.  The Perl standard of $n is used 
        /// if group substitutions are included.<br/>Optional, Recommended, Required if spelling_regex is present.
        /// <br/>Set to an empty string if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string romanization { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This string contains a generalized regular expression used to match a portion 
        /// of the phonetic representation of a word in the conlang with a specific Romanization or Latinization.  The 
        /// matched text will then be replaced with the value from the romanization.  This will be expressed using the 
        /// symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required 
        /// if romanization is present.<br/>Set to an empty string if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string spelling_regex { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This string contains a generalized regular expression that matches a 
        /// portion of a word's Romanized or Latinized version in the conlang with a specific phonetic representation.  
        /// The matched text will then be replaced with the value from the phoneme below.<br/> Optional, Recommended, 
        /// Required if phoneme is present.<br/>Set to an empty string if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string pronunciation_regex { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Performs a shallow copy of the SoundMap object.
        /// </summary>
        /// <returns>A new SoundMap with the same data.</returns>
#pragma warning disable IDE1006 // Naming Styles
        public SoundMap copy()
#pragma warning restore IDE1006 // Naming Styles
        {
            SoundMap copy = new(this.phoneme, this.romanization, this.spelling_regex, this.pronunciation_regex);
            return copy;
        }

        /// <summary>
        /// Determines if this LexiconEntry is the same as another object.
        /// </summary>
        /// <param name="obj">Object to be compared to this LexiconEntry.</param>
        /// <returns>true if the objects are the same, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return obj is SoundMap map &&
                phoneme == map.phoneme &&
                   romanization == map.romanization &&
                   spelling_regex == map.spelling_regex &&
                   pronunciation_regex == map.pronunciation_regex;
        }

        /// <summary>
        /// Determines if this SoundMap is the same as another SoundMap.  This comparison is done
        /// based on the contents of the objects, not the object's identities.
        /// </summary>
        /// <param name="other">SoundMap object to be compared to this SoundMap.</param>
        /// <returns>true if the SoundMap objects represent the same data.</returns>
        public bool Equals(SoundMap? other)
        {
            return other is not null &&
                   phoneme == other.phoneme &&
                   romanization == other.romanization &&
                   spelling_regex == other.spelling_regex &&
                   pronunciation_regex == other.pronunciation_regex;
        }

        /// <summary>
        /// Generates a hash code for a LexiconEntry object based on its data.
        /// </summary>
        /// <returns>Hash Code for this LexiconEntry object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(phoneme, romanization, spelling_regex, pronunciation_regex);
        }

        /// <summary>
        /// Gets a string description of the SoundMap object
        /// </summary>
        /// <returns>String Description showing two groupings separated by a semicolon.  
        /// The first grouping is pronunciation regex -> phoneme.  The second is 
        /// spelling regex -> romanization.</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if ((this.phoneme != null) && (this.romanization != null))
            {
                _ = sb.AppendFormat("{0} -> {1}; {2} -> {3}", this.pronunciation_regex, this.phoneme, this.spelling_regex, this.romanization);
            }
            else if (this.phoneme != null)
            {
                _ = sb.AppendFormat("{0} -> {1}; <NA> -> <NA>", this.pronunciation_regex, this.phoneme);
            }
            else // Should be a romanization only entry
            {
                _ = sb.AppendFormat("<NA> -> <NA>; {0} -> {1}", this.spelling_regex, this.romanization);
            }
            return sb.ToString();
        }
    }
}