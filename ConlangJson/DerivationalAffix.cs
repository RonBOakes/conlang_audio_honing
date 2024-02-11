/*
 * C# Structure for the Conlang JSON Derivational Affix Entry Objects
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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure that corresponds to Derivation Affix Entries of the 
    /// Conlang JSON structure.
    /// </summary>
    public class DerivationalAffix
    {
        private string? _type;
        private string? _pronounciation_add = null;
        private string? _spelling_add = null;
        private string? _pronounciation_regex = null;
        private string? _spelling_regex = null;
        private string? _t_pronounciation_add = null;
        private string? _t_spelling_add = null;    
        private string? _f_pronounciation_add = null;
        private string? _f_spelling_add = null;

        /// <summary>
        /// Constructor used to build an DerivationalAffix object by passing it values for all of the entries.<br/>
        /// Note that this constructor method does not prevent invalid configurations based on the 
        /// parameter listings below.
        /// </summary>
        /// <param name="type">This string has two valid values: “PREFIX” or “SUFFIX”. This indicates if the affix 
        /// placed on the root word when deriving the new word is a prefix or suffix.<br/>.Required.</param>
        /// <param name="pronounciation_add">This is the string to be prepended or appended to the phonetic 
        /// representation of the root word to create the new derived word’s phonetic representation. This 
        /// will be expressed using the symbology specified in the phonetic_characters field at the Top Level.
        /// <br/>Optional, Recommended, Not allowed if pronounciation_regex or spelling_regex is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="spelling_add">This is the string to be prepended or appended to the Romanized or Latinized 
        /// representation of the root word to create the new derived word’s Romanized or Latinized representation.
        /// <br/>Optional, Recommended, Not allowed if spelling_regex or pronounciation_regex is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="pronounciation_regex">This is the generalized regular expression used to match on the 
        /// phonetic representation of the root word to determine which phonetic string to prepend or append to 
        /// that root word when creating the derived word.<br/>Optional, Recommended, Required if t_pronounciation_add
        /// or f_pronounciation_add are present, Not allowed if pronounciation_add or spelling_add is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="spelling_regex">This is the generalized regular expression used to match on the Romanized 
        /// or Latinized representation of the root word to determine which Romanized or Latinized string to prepend 
        /// or append to that root word when creating the derived word.<br/> Optional, Recommended, Required if 
        /// t_spelling_add or f_spelling_add are present, Not allowed if pronuncation_add or spelling_add is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="t_pronounciation_add">This is the string to be prepended or appended to the phonetic representation 
        /// of the root word to create the new derived word's phonetic representation if the root word matches the pattern in 
        /// pronouncation_regex.  This will be expressed using the symbology specified in the phonetic_characters field at the 
        /// Top Level.<br/>Set to null if not present.</param>
        /// <param name="t_spelling_add"> This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word matches the pattern in 
        /// spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present, Not allowed if pronunciation_add or 
        /// spelling_add is present.<br/>Set to null if not present.</param>
        /// <param name="f_pronounciation_add">This is the string to be prepended or appended to the phonetic representation of the 
        /// root word to create the new derived word's phonetic representation if the root word does not match the pattern in pronouncation_regex.
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.<br/>
        /// Set to null if not present.</param>
        /// <param name="f_spelling_add">This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word does not match the pattern 
        /// in spelling_regex.<br/>Set to null if not present.</param>
        public DerivationalAffix(string? type, string? pronounciation_add, string? spelling_add, string? pronounciation_regex, string? spelling_regex, string? t_pronounciation_add, string? t_spelling_add, string? f_pronounciation_add, string? f_spelling_add)
        {
            this._type = type;
            this._pronounciation_add = pronounciation_add;
            this._spelling_add = spelling_add;
            this._pronounciation_regex = pronounciation_regex;
            this._spelling_regex = spelling_regex;
            this._t_pronounciation_add = t_pronounciation_add;
            this._t_spelling_add= t_spelling_add;
            this._f_pronounciation_add= f_pronounciation_add;
            this._f_spelling_add = f_spelling_add;
        }

        /// <summary>
        /// Constructor used to build an empty DerivationalAffix object.  All of the members are set to the default values.
        /// </summary>
        public DerivationalAffix()
        {
            type = "";
        }

        /// <summary>
        /// This string has two valid values: “PREFIX” or “SUFFIX”. This indicates if the affix 
        /// placed on the root word when deriving the new word is a prefix or suffix.<br/>.Required.
        /// </summary>
        public string? type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// This is the string to be prepended or appended to the phonetic 
        /// representation of the root word to create the new derived word’s phonetic representation. This 
        /// will be expressed using the symbology specified in the phonetic_characters field at the Top Level.
        /// <br/>Optional, Recommended, Not allowed if pronounciation_regex or spelling_regex is present.<br/>
        /// Set to null if not present.
        /// </summary>
        public string? pronounciation_add
        {
            get { return _pronounciation_add; }
            set { _pronounciation_add = value; }
        }

        /// <summary>
        /// This is the string to be prepended or appended to the Romanized or Latinized 
        /// representation of the root word to create the new derived word’s Romanized or Latinized representation.
        /// <br/>Optional, Recommended, Not allowed if spelling_regex or pronounciation_regex is present.<br/>
        /// Set to null if not present.
        /// </summary>
        public string? spelling_add
        {
            get { return _spelling_add; }
            set { _spelling_add = value; }
        }

        /// <summary>
        /// This is the generalized regular expression used to match on the 
        /// phonetic representation of the root word to determine which phonetic string to prepend or append to 
        /// that root word when creating the derived word.<br/>Optional, Recommended, Required if t_pronounciation_add
        /// or f_pronounciation_add are present, Not allowed if pronounciation_add or spelling_add is present.<br/>
        /// Set to null if not present.
        /// </summary>
        public string? pronounciation_regex
        {
            get { return _pronounciation_regex; }
            set { _pronounciation_regex = value; }
        }

        /// <summary>
        /// This is the generalized regular expression used to match on the Romanized 
        /// or Latinized representation of the root word to determine which Romanized or Latinized string to prepend 
        /// or append to that root word when creating the derived word.<br/> Optional, Recommended, Required if 
        /// t_spelling_add or f_spelling_add are present, Not allowed if pronuncation_add or spelling_add is present.<br/>
        /// Set to null if not present.
        /// </summary>
        public string? spelling_regex
        {
            get { return _spelling_regex; }
            set { _spelling_regex = value; }
        }

        /// <summary>
        /// This is the string to be prepended or appended to the phonetic representation 
        /// of the root word to create the new derived word's phonetic representation if the root word matches the pattern in 
        /// pronouncation_regex.  This will be expressed using the symbology specified in the phonetic_characters field at the 
        /// Top Level.<br/>Set to null if not present.
        /// </summary>
        public string? t_pronounciation_add
        {
            get { return _t_pronounciation_add; }
            set { _t_pronounciation_add = value; }
        }

        /// <summary>
        /// This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word matches the pattern in 
        /// spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present, Not allowed if pronunciation_add or 
        /// spelling_add is present.<br/>Set to null if not present.
        /// </summary>
        public string? t_spelling_add
        {
            get { return _t_spelling_add; }
            set { _t_spelling_add = value; }
        }

        /// <summary>
        /// This is the string to be prepended or appended to the phonetic representation of the 
        /// root word to create the new derived word's phonetic representation if the root word does not match the pattern in pronouncation_regex.
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.<br/>
        /// Set to null if not present.
        /// </summary>
        public string? f_pronounciation_add
        {
            get { return _f_pronounciation_add; }
            set { _f_pronounciation_add = value; }
        }

        /// <summary>
        /// This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word does not match the pattern 
        /// in spelling_regex.<br/>Set to null if not present.
        /// </summary>
        public string? f_spelling_add
        {
            get { return _f_spelling_add; }
            set { _f_spelling_add = value;}
        }

    }
}
