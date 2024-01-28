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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangJson
{
    public class Affix
    {
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

        public Affix() { }

        public string? pronounciation_add
        {
            get { return _pronounciation_add; }
            set { _pronounciation_add = value; }
        }

        public string? spelling_add
        {
            get { return _spelling_add; }
            set { _spelling_add = value; }
        }

        public string? pronounciation_regex
        {
            get { return _pronounciation_regex; }
            set { _pronounciation_regex = value; }
        }

        public string? spelling_regex
        {
            get { return _spelling_regex; }
            set { _spelling_regex = value; }
        }

        public string? t_pronounciation_add
        {
            get { return _t_pronounciation_add; }
            set { _t_pronounciation_add = value; }
        }

        public string? t_spelling_add
        {
            get { return _t_spelling_add; }
            set { _t_spelling_add = value; }
        }

        public string? f_pronounciation_add
        {
            get { return _f_pronounciation_add; }
            set { _f_pronounciation_add = value; }
        }

        public string? f_spelling_add
        {
            get { return _f_spelling_add; }
            set { _f_spelling_add = value; }
        }

        public string? pronounciation_repl
        {
            get { return _pronounciation_repl; }
            set { _pronounciation_repl = value; }
        }

        public string? spelling_repl
        {
            get { return _spelling_repl; }
            set { _spelling_repl = value; }
        }
    }
}
