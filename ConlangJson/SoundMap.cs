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
    public class SoundMap
    {
        private string _phoneme;
        private string _romanization;
        private string _spelling_regex;
        private string _pronounciation_regex;

        public SoundMap()
        {
            this._phoneme = string.Empty;
            this._romanization = string.Empty;
            this._spelling_regex = string.Empty;
            this._pronounciation_regex = string.Empty;
        }

        public SoundMap(string phoneme, string romanization, string spelling_regex, string pronounciation_regex)
        {
            this._phoneme = phoneme;
            this._romanization = romanization;
            this._spelling_regex = spelling_regex;
            this._pronounciation_regex = pronounciation_regex;
        }

        public string phoneme
        {
            get { return _phoneme; }
            set { _phoneme = value; }
        }

        public string romanization
        {
            get { return _romanization; }
            set { _romanization = value; }
        }

        public string spelling_regex
        {
            get { return _spelling_regex; }
            set { _spelling_regex = value;}
        }

        public string pronounciation_regex
        {
            get { return _pronounciation_regex; }
            set { _pronounciation_regex = value; }
        }
    }
}