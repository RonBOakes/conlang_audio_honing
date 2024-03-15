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
namespace ConlangJson
{
    /// <summary>
    /// Defines the .NET/C# structure that corresponds to Derivation Affix Entries of the 
    /// Conlang JSON structure.
    /// </summary>
    public class DerivationalAffix
    {

        /// <summary>
        /// Constructor used to build an DerivationalAffix object by passing it values for all of the entries.<br/>
        /// Note that this constructor method does not prevent invalid configurations based on the 
        /// parameter listings below.
        /// </summary>
        /// <param name="type">This string has two valid values: “PREFIX” or “SUFFIX”. This indicates if the affix 
        /// placed on the root word when deriving the new word is a prefix or suffix.<br/>.Required.</param>
        /// <param name="pronunciation_add">This is the string to be prepended or appended to the phonetic 
        /// representation of the root word to create the new derived word’s phonetic representation. This 
        /// will be expressed using the symbology specified in the phonetic_characters field at the Top Level.
        /// <br/>Optional, Recommended, Not allowed if pronunciation_regex or spelling_regex is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="spelling_add">This is the string to be prepended or appended to the Romanized or Latinized 
        /// representation of the root word to create the new derived word’s Romanized or Latinized representation.
        /// <br/>Optional, Recommended, Not allowed if spelling_regex or pronunciation_regex is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="pronunciation_regex">This is the generalized regular expression used to match on the 
        /// phonetic representation of the root word to determine which phonetic string to prepend or append to 
        /// that root word when creating the derived word.<br/>Optional, Recommended, Required if t_pronunciation_add
        /// or f_pronunciation_add are present, Not allowed if pronunciation_add or spelling_add is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="spelling_regex">This is the generalized regular expression used to match on the Romanized 
        /// or Latinized representation of the root word to determine which Romanized or Latinized string to prepend 
        /// or append to that root word when creating the derived word.<br/> Optional, Recommended, Required if 
        /// t_spelling_add or f_spelling_add are present, Not allowed if pronunciation_add or spelling_add is present.<br/>
        /// Set to null if not present.</param>
        /// <param name="t_pronunciation_add">This is the string to be prepended or appended to the phonetic representation 
        /// of the root word to create the new derived word's phonetic representation if the root word matches the pattern in 
        /// pronunciation_regex.  This will be expressed using the symbology specified in the phonetic_characters field at the 
        /// Top Level.<br/>Set to null if not present.</param>
        /// <param name="t_spelling_add"> This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word matches the pattern in 
        /// spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present, Not allowed if pronunciation_add or 
        /// spelling_add is present.<br/>Set to null if not present.</param>
        /// <param name="f_pronunciation_add">This is the string to be prepended or appended to the phonetic representation of the 
        /// root word to create the new derived word's phonetic representation if the root word does not match the pattern in pronunciation_regex.
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.<br/>
        /// Set to null if not present.</param>
        /// <param name="f_spelling_add">This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word does not match the pattern 
        /// in spelling_regex.<br/>Set to null if not present.</param>
        public DerivationalAffix(string? type, string? pronunciation_add, string? spelling_add, string? pronunciation_regex, string? spelling_regex, string? t_pronunciation_add, string? t_spelling_add, string? f_pronunciation_add, string? f_spelling_add)
        {
            this.type = type;
            this.pronunciation_add = pronunciation_add;
            this.spelling_add = spelling_add;
            this.pronunciation_regex = pronunciation_regex;
            this.spelling_regex = spelling_regex;
            this.t_pronunciation_add = t_pronunciation_add;
            this.t_spelling_add = t_spelling_add;
            this.f_pronunciation_add = f_pronunciation_add;
            this.f_spelling_add = f_spelling_add;
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
#pragma warning disable IDE1006 // Naming Styles
        public string? type { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the string to be prepended or appended to the phonetic 
        /// representation of the root word to create the new derived word’s phonetic representation. This 
        /// will be expressed using the symbology specified in the phonetic_characters field at the Top Level.
        /// <br/>Optional, Recommended, Not allowed if pronunciation_regex or spelling_regex is present.<br/>
        /// Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? pronunciation_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the string to be prepended or appended to the Romanized or Latinized 
        /// representation of the root word to create the new derived word’s Romanized or Latinized representation.
        /// <br/>Optional, Recommended, Not allowed if spelling_regex or pronunciation_regex is present.<br/>
        /// Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? spelling_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the generalized regular expression used to match on the 
        /// phonetic representation of the root word to determine which phonetic string to prepend or append to 
        /// that root word when creating the derived word.<br/>Optional, Recommended, Required if t_pronunciation_add
        /// or f_pronunciation_add are present, Not allowed if pronunciation_add or spelling_add is present.<br/>
        /// Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? pronunciation_regex { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the generalized regular expression used to match on the Romanized 
        /// or Latinized representation of the root word to determine which Romanized or Latinized string to prepend 
        /// or append to that root word when creating the derived word.<br/> Optional, Recommended, Required if 
        /// t_spelling_add or f_spelling_add are present, Not allowed if pronunciation_add or spelling_add is present.<br/>
        /// Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? spelling_regex { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the string to be prepended or appended to the phonetic representation 
        /// of the root word to create the new derived word's phonetic representation if the root word matches the pattern in 
        /// pronunciation_regex.  This will be expressed using the symbology specified in the phonetic_characters field at the 
        /// Top Level.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? t_pronunciation_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word matches the pattern in 
        /// spelling_regex.<br/>Optional, Recommended, Required if spelling_regex is present, Not allowed if pronunciation_add or 
        /// spelling_add is present.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? t_spelling_add
#pragma warning restore IDE1006 // Naming Styles
        { get; set; } = null;

        /// <summary>
        /// This is the string to be prepended or appended to the phonetic representation of the 
        /// root word to create the new derived word's phonetic representation if the root word does not match the pattern in pronunciation_regex.
        /// This will be expressed using the symbology specified in the phonetic_characters field at the Top Level.<br/>
        /// Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? f_pronunciation_add { get; set; } = null;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// This is the string to be prepended or appended to the Romanized or Latinized representation 
        /// of the root word to create the new word's Romanized or Latinized representation if the root word does not match the pattern 
        /// in spelling_regex.<br/>Set to null if not present.
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public string? f_spelling_add
#pragma warning restore IDE1006 // Naming Styles
        { get; set; } = null;

    }
}
