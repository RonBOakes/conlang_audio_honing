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
namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure that corresponds to the Affix Map Entries.  Each object of this class
    /// will encapsulate one entry in the higher-level Affix Map.
    /// </summary>
    public class Affix
    {

        /// <summary>
        /// Constructor used to build an Affix object by passing it values for all of the entries.<br/>
        /// Note that this constructor method does not prevent invalid configurations based on the 
        /// parameter listings below.
        /// </summary>
        /// <param name="pronunciation_add">This string contains the value that will be added to 
        /// the beginning of a word for a prefix or the end of a word for a suffix in its phonetic 
        /// representation.This will be expressed using the symbology specified in the phonetic_characters 
        /// field at the Top Level.<br/>Optional, Recommended, Required if spelling_add is present, Not allowed 
        /// if pronunciation_regex is present.<br/>Set to null if not present.</param>
        /// <param name="spelling_add">This string contains the value that will be added to the beginning of 
        /// a word for a prefix or the end of a word for a suffix in its Romanized or Latinized representation.<br/>
        /// Optional, Recommended, Required if pronunciation_add is present, Not allowed if spelling_regex is 
        /// present.<br/>Set to null if not present.</param>
        /// <param name="pronunciation_regex">This string contains the generalized regular expression used to 
        /// match against the phonetic representation of the word to determine which phonetic string to add to 
        /// the beginning or end of that phonetic representation.If the phonetic representation of the word matches 
        /// this pattern, then the t_pronunciation_add string should be added to the appropriate end of the phonetic 
        /// representation. The f_pronunciation_add string should be added instead if it does not match. This pattern 
        /// should be constructed to match the beginning or end of the string as appropriate for the type of affix 
        /// being constructed.<br/>Optional, Recommended, Required if t_pronunciation_add, f_pronunciation_add, 
        /// or spelling_regex are present, Not allowed if pronunciation_add is present.<br/>Set to null if not present.</param>
        /// <param name="spelling_regex">This string contains the generalized regular expression used to match against 
        /// the Romanized or Latinized representation of the word to determine which Romanized or Latinized string to 
        /// add to the beginning or end of that Romanized or Latinized representation.If the Romanized or Latinized 
        /// representation of the word matches this pattern, then the t_spelling_add string should be added to the 
        /// appropriate end of the phonetic representation. The f_spelling_add string should be added if it does not 
        /// match. This pattern should be constructed to match the beginning or end of the string as appropriate for 
        /// the type of affix being constructed.<br/>Optional, Recommended, Required if t_spelling_add, f_spelling_add, 
        /// or pronunciation_regex are present, Not allowed if spelling_add is present.<br/>Set to null if not 
        /// present.</param>
        /// <param name="t_pronunciation_add">The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the phonetic representation for a suffix, when phonetic representation matches 
        /// pronunciation_regex.This will be expressed using the symbology specified in the phonetic_characters field 
        /// at the Top Level.<br/>Optional, Recommended, Required if pronunciation_regex is present, Not allowed if 
        /// pronunciation_add is present.<br/>Set to null if not present.</param>
        /// <param name="t_spelling_add">The string to add to the beginning of the Romanized or Latinized representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when the Romanized or Latinized 
        /// representation matches spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present,
        /// Not allowed if spelling_add is present.<br/>Set to null if not present.</param>
        /// <param name="f_pronunciation_add">The string to add to the beginning of the phonetic representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when phonetic representation does 
        /// not match pronunciation_regex.This will be expressed using the symbology specified in the 
        /// phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required if pronunciation_regex
        /// is present, Not allowed if pronunciation_add is present.<br/>Set to null if not present.</param>
        /// <param name="f_spelling_add">The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the Romanized or Latinized representation for a suffix, when the Romanized or 
        /// Latinized representation does not match spelling_regex.<br/>Optional, Recommended, Required if 
        /// spelling_regex is present, Not allowed if spelling_add is present.<br/>Set to null if not
        /// present.</param>
        /// <param name="pronunciation_replacement">This is the pattern that will be used to replace the pattern 
        /// matched by pronunciation_regex in the phonetic representation of the word when a match occurs.  
        /// Replacement groups are represented using the Perl standard of $n.  This will be expressed using 
        /// the symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, 
        /// Required if pronunciation_regex is present.<br/>Set to null if not present.</param>
        /// <param name="spelling_replacement"> This is the pattern that will be used to replace the pattern matched 
        /// by spelling_regex in the Romanized or Latinized representation of the word when a match 
        /// occurs.  Replacement groups are represented using the Perl standard of \$n.<br/>Optional, Recommended, 
        /// Required if spelling_regex is present.<br/>Set to null if not present.</param>
        public Affix(string? pronunciation_add, string? spelling_add, string? pronunciation_regex, string? spelling_regex, string? t_pronunciation_add,
            string? t_spelling_add, string? f_pronunciation_add, string? f_spelling_add, string? pronunciation_replacement, string? spelling_replacement)
        {
            this.pronunciation_add = pronunciation_add;
            this.spelling_add = spelling_add;
            this.pronunciation_regex = pronunciation_regex;
            this.spelling_regex = spelling_regex;
            this.t_pronunciation_add = t_pronunciation_add;
            this.t_spelling_add = t_spelling_add;
            this.f_pronunciation_add = f_pronunciation_add;
            this.f_spelling_add = f_spelling_add;
            this.pronunciation_replacement = pronunciation_replacement;
            this.spelling_replacement = spelling_replacement;
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
        /// if pronunciation_regex is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? pronunciation_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This string contains the value that will be added to the beginning of 
        /// a word for a prefix or the end of a word for a suffix in its Romanized or Latinized representation.<br/>
        /// Optional, Recommended, Required if pronunciation_add is present, Not allowed if spelling_regex is 
        /// present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? spelling_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This string contains the generalized regular expression used to 
        /// match against the phonetic representation of the word to determine which phonetic string to add to 
        /// the beginning or end of that phonetic representation.If the phonetic representation of the word matches 
        /// this pattern, then the t_pronunciation_add string should be added to the appropriate end of the phonetic 
        /// representation. The f_pronunciation_add string should be added instead if it does not match. This pattern 
        /// should be constructed to match the beginning or end of the string as appropriate for the type of affix 
        /// being constructed.<br/>Optional, Recommended, Required if t_pronunciation_add, f_pronunciation_add, 
        /// or spelling_regex are present, Not allowed if pronunciation_add is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? pronunciation_regex { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This string contains the generalized regular expression used to match against 
        /// the Romanized or Latinized representation of the word to determine which Romanized or Latinized string to 
        /// add to the beginning or end of that Romanized or Latinized representation.If the Romanized or Latinized 
        /// representation of the word matches this pattern, then the t_spelling_add string should be added to the 
        /// appropriate end of the phonetic representation. The f_spelling_add string should be added if it does not 
        /// match. This pattern should be constructed to match the beginning or end of the string as appropriate for 
        /// the type of affix being constructed.<br/>Optional, Recommended, Required if t_spelling_add, f_spelling_add, 
        /// or pronunciation_regex are present, Not allowed if spelling_add is present.<br/>Set to null if not 
        /// present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? spelling_regex { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the phonetic representation for a suffix, when phonetic representation matches 
        /// pronunciation_regex.This will be expressed using the symbology specified in the phonetic_characters field 
        /// at the Top Level.<br/>Optional, Recommended, Required if pronunciation_regex is present, Not allowed if 
        /// pronunciation_add is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? t_pronunciation_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The string to add to the beginning of the Romanized or Latinized representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when the Romanized or Latinized 
        /// representation matches spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present,
        /// Not allowed if spelling_add is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? t_spelling_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The string to add to the beginning of the phonetic representation 
        /// for a prefix, or the end of the phonetic representation for a suffix, when phonetic representation does 
        /// not match pronunciation_regex.This will be expressed using the symbology specified in the 
        /// phonetic_characters field at the Top Level.<br/>Optional, Recommended, Required if pronunciation_regex
        /// is present, Not allowed if pronunciation_add is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? f_pronunciation_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// The string to add to the beginning of the phonetic representation for a 
        /// prefix, or the end of the Romanized or Latinized representation for a suffix, when the Romanized or 
        /// Latinized representation does not match spelling_regex.<br/>Optional, Recommended, Required if 
        /// spelling_regex is present, Not allowed if spelling_add is present.<br/>Set to null if not
        /// present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? f_spelling_add
#pragma warning restore IDE1006 // Naming Styles
        { get; set; } = null;

        /// <summary>
        /// This is the pattern that will be used to replace the pattern 
        /// matched by pronunciation_regex in the phonetic representation of the word when a match occurs.  
        /// Replacement groups are represented using the Perl standard of $n.  This will be expressed using 
        /// the symbology specified in the phonetic_characters field at the Top Level.<br/>Optional, Recommended, 
        /// Required if pronunciation_regex is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? pronunciation_replacement { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the pattern that will be used to replace the pattern matched 
        /// by spelling_regex in the Romanized or Latinized representation of the word when a match 
        /// occurs.  Replacement groups are represented using the Perl standard of \$n.<br/>Optional, Recommended, 
        /// Required if spelling_regex is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? spelling_replacement
#pragma warning restore IDE1006 // Naming Styles
        { get; set; } = null;
    }
}
