/*
 * C# structure for Conlang JSON structure Prefix or Suffix objects
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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure that corresponds to the Affix Map Entries.  Each object of this class
    /// will encapsulate one entry in the higher-level Affix Map.
    /// </summary>
    public class Affix
    {
        /* Private members that represent the individual strings that may be placed
         * into the Affix structure
         */
        private string? _pronounciation_add = null;
        private string? _spelling_add = null;
        private string? _pronounciation_regex = null;
        private string? _spelling_regex = null;
        private string? _t_pronounciation_add = null;
        private string? _t_spelling_add = null;
        private string? _f_pronounciation_add = null;
        private string? _f_spelling_add = null;
        private string? _pronounciation_repl = null;
        private string? _spelling_repl = null;

        /// <summary>
        /// Constructor used to build an Affix object by passing it values for all of the entries.<br/>
        /// Note that this constructor method does not prevent invalid configurations based on the 
        /// parameter listings below.
        /// </summary>
        /// <param name="pronounciation_add">This string contains the value that will be added to 
        /// the beginning of a word for a prefix or the end of a word for a suffix in its phonetic 
        /// representation.This will be expressed using the symbology specified in the phonetic_characters 
        /// field at the Top Level.<br/>Optional, Recommended, Required if spelling_add is present, Not allowed 
        /// if pronnounciation_regex is present.<br/>Set to null if not present.</param>
        /// <param name="spelling_add">This string contains the value that will be added to the beginning of 
        /// a word for a prefix or the end of a word for a suffix in its Romanized or Latinized representation.<br/>
        /// Optional, Recommended, Required if pronounciation_add is present, Not allowed if spelling_regex is 
        /// present.<br/>Set to null if not present.</param>
        /// <param name="pronounciation_regex">This string contains the generalized regular expression used to 
        /// match against the phonetic representation of the word to determine which phonetic string to add to 
        /// the beginning or end of that phonetic representation.If the phonetic representation of the word matches 
        /// this pattern, then the t_pronounciation_add string should be added to the appropriate end of the phonetic 
        /// representation. The f_pronounciation_add string should be added instead if it does not match. This pattern 
        /// should be constructed to match the beginning or end of the string as appropriate for the type of affix 
        /// being constructed.<br/>Optional, Recommended, Required if t_pronounciation_add, f_pronounciation_add, 
        /// or spelling_regex are present, Not allowed if pronunciation_add is present.<br/>Set to null if not present.</param>
        /// <param name="spelling_regex">This string contains the generalized regular expression used to match against 
        /// the Romanized or Latinized representation of the word to determine which Romanized or Latinized string to 
        /// add to the beginning or end of that Romanized or Latinized representation.If the Romanized or Latinized 
        /// representation of the word matches this pattern, then the t_spelling_add string should be added to the 
        /// appropriate end of the phonetic representation. The f_spelling_add string should be added if it does not 
        /// match. This pattern should be constructed to match the beginning or end of the string as appropriate for 
        /// the type of affix being constructed.<br/>Optional, Recommended, Required if t_spelling_add, f_spelling_add, 
        /// or pronounciation_regex are present, Not allowed if spelling_add is present.<br/>Set to null if not 
        /// present.</param>
        /// <param name="t_pronounciation_add">The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the phonetic representation for a suffix, when phonetic representation matches 
        /// pronounciation_regex.This will be expressed using the symbology specified in the phonetic_characters field 
        /// at the Top Level.<br/>Optional, Recommended, Required if pronouncation_regex is present, Not allowed if 
        /// pronuncation_add is present.<br/>Set to null if not present.</param>
        /// <param name="t_spelling_add">The string to add to the beginning of the Romanized or Latinized representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when the Romanized or Latinized 
        /// representation matches spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present,
        /// Not allowed if spelling_add is present.<br/>Set to null if not present.</param>
        /// <param name="f_pronounciation_add">The string to add to the beginning of the phonetic representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when phonetic representation does 
        /// not match pronounciation_regex.This will be expressed using the symbology specified in the 
        /// phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required if pronouncation_regex
        /// is present, Not allowed if pronuncation_add is present.<br/>Set to null if not present.</param>
        /// <param name="f_spelling_add">The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the Romanized or Latinized representation for a suffix, when the Romanized or 
        /// Latinized representation does not match spelling_regex.<br/>Optional, Recommended, Required if 
        /// spelling_regex is present, Not allowed if spelling_add is present.<br/>Set to null if not
        /// present.</param>
        /// <param name="pronounciation_repl">This is the pattern that will be used to replace the pattern 
        /// matched by pronounciation_regex in the phonetic representation of the word when a match occurs.  
        /// Replacement groups are represented using the Perl standard of $n.  This will be expressed using 
        /// the symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, 
        /// Required if pronounciation_regex is present.<br/>Set to null if not present.</param>
        /// <param name="spelling_repl"> This is the pattern that will be used to replace the pattern matched 
        /// by \textbf{spelling\_regex} in the Romanized or Latinized representation of the word when a match 
        /// occurs.  Replacement groups are represented using the Perl standard of \$n.<br/>Optional, Recommended, 
        /// Required if spelling_regex is present.<br/>Set to null if not present.</param>
        public Affix(string? pronounciation_add, string? spelling_add, string? pronounciation_regex, string? spelling_regex, string? t_pronounciation_add, 
            string? t_spelling_add, string? f_pronounciation_add, string? f_spelling_add, string? pronounciation_repl, string? spelling_repl)
        {
            this._pronounciation_add = pronounciation_add;
            this._spelling_add = spelling_add;
            this._pronounciation_regex = pronounciation_regex;
            this._spelling_regex = spelling_regex;
            this._t_pronounciation_add = t_pronounciation_add;
            this._t_spelling_add = t_spelling_add;
            this._f_pronounciation_add = f_pronounciation_add;
            this._f_spelling_add = f_spelling_add;
            _pronounciation_repl = pronounciation_repl;
            _spelling_repl = spelling_repl;
        }

        /// <summary>
        /// Constructor used to build an empty Affix object.  All of the members are set to the default null values.
        /// </summary>
        public Affix() { }

        /// <summary>
        /// This string contains the value that will be added to 
        /// the beginning of a word for a prefix or the end of a word for a suffix in its phonetic 
        /// representation.This will be expressed using the symbology specified in the phonetic_characters 
        /// field at the Top Level.<br/>Optional, Recommended, Required if spelling_add is present, Not allowed 
        /// if pronnounciation_regex is present.<br/>Set to null if not present.
        /// </summary>
        public string? pronounciation_add
        {
            get { return _pronounciation_add; }
            set { _pronounciation_add = value; }
        }

        /// <summary>
        /// This string contains the value that will be added to the beginning of 
        /// a word for a prefix or the end of a word for a suffix in its Romanized or Latinized representation.<br/>
        /// Optional, Recommended, Required if pronounciation_add is present, Not allowed if spelling_regex is 
        /// present.<br/>Set to null if not present.
        /// </summary>
        public string? spelling_add
        {
            get { return _spelling_add; }
            set { _spelling_add = value; }
        }

        /// <summary>
        /// This string contains the generalized regular expression used to 
        /// match against the phonetic representation of the word to determine which phonetic string to add to 
        /// the beginning or end of that phonetic representation.If the phonetic representation of the word matches 
        /// this pattern, then the t_pronounciation_add string should be added to the appropriate end of the phonetic 
        /// representation. The f_pronounciation_add string should be added instead if it does not match. This pattern 
        /// should be constructed to match the beginning or end of the string as appropriate for the type of affix 
        /// being constructed.<br/>Optional, Recommended, Required if t_pronounciation_add, f_pronounciation_add, 
        /// or spelling_regex are present, Not allowed if pronunciation_add is present.<br/>Set to null if not present.
        /// </summary>
        public string? pronounciation_regex
        {
            get { return _pronounciation_regex; }
            set { _pronounciation_regex = value; }
        }

        /// <summary>
        /// This string contains the generalized regular expression used to match against 
        /// the Romanized or Latinized representation of the word to determine which Romanized or Latinized string to 
        /// add to the beginning or end of that Romanized or Latinized representation.If the Romanized or Latinized 
        /// representation of the word matches this pattern, then the t_spelling_add string should be added to the 
        /// appropriate end of the phonetic representation. The f_spelling_add string should be added if it does not 
        /// match. This pattern should be constructed to match the beginning or end of the string as appropriate for 
        /// the type of affix being constructed.<br/>Optional, Recommended, Required if t_spelling_add, f_spelling_add, 
        /// or pronounciation_regex are present, Not allowed if spelling_add is present.<br/>Set to null if not 
        /// present.
        /// </summary>
        public string? spelling_regex
        {
            get { return _spelling_regex; }
            set { _spelling_regex = value; }
        }

        /// <summary>
        /// The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the phonetic representation for a suffix, when phonetic representation matches 
        /// pronounciation_regex.This will be expressed using the symbology specified in the phonetic_characters field 
        /// at the Top Level.<br/>Optional, Recommended, Required if pronouncation_regex is present, Not allowed if 
        /// pronuncation_add is present.<br/>Set to null if not present.
        /// </summary>
        public string? t_pronounciation_add
        {
            get { return _t_pronounciation_add; }
            set { _t_pronounciation_add = value; }
        }

        /// <summary>
        /// The string to add to the beginning of the Romanized or Latinized representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when the Romanized or Latinized 
        /// representation matches spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present,
        /// Not allowed if spelling_add is present.<br/>Set to null if not present.
        /// </summary>
        public string? t_spelling_add
        {
            get { return _t_spelling_add; }
            set { _t_spelling_add = value; }
        }

        /// <summary>
        /// The string to add to the beginning of the phonetic representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when phonetic representation does 
        /// not match pronounciation_regex.This will be expressed using the symbology specified in the 
        /// phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required if pronouncation_regex
        /// is present, Not allowed if pronuncation_add is present.<br/>Set to null if not present.
        /// </summary>
        public string? f_pronounciation_add
        {
            get { return _f_pronounciation_add; }
            set { _f_pronounciation_add = value; }
        }

        /// <summary>
        /// The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the Romanized or Latinized representation for a suffix, when the Romanized or 
        /// Latinized representation does not match spelling_regex.<br/>Optional, Recommended, Required if 
        /// spelling_regex is present, Not allowed if spelling_add is present.<br/>Set to null if not
        /// present.
        /// </summary>
        public string? f_spelling_add
        {
            get { return _f_spelling_add; }
            set { _f_spelling_add = value; }
        }

        /// <summary>
        /// This is the pattern that will be used to replace the pattern 
        /// matched by pronounciation_regex in the phonetic representation of the word when a match occurs.  
        /// Replacement groups are represented using the Perl standard of $n.  This will be expressed using 
        /// the symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, 
        /// Required if pronounciation_regex is present.<br/>Set to null if not present.
        /// </summary>
        public string? pronounciation_repl
        {
            get { return _pronounciation_repl; }
            set { _pronounciation_repl = value; }
        }

        /// <summary>
        /// This is the pattern that will be used to replace the pattern matched 
        /// by \textbf{spelling\_regex} in the Romanized or Latinized representation of the word when a match 
        /// occurs.  Replacement groups are represented using the Perl standard of \$n.<br/>Optional, Recommended, 
        /// Required if spelling_regex is present.<br/>Set to null if not present.
        /// </summary>
        public string? spelling_repl
        {
            get { return _spelling_repl; }
            set { _spelling_repl = value; }
        }
    }
}
