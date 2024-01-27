/*
 * Class containing utility functions for working with the C# version of the
 * Conlang structure.
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
using System.Text.RegularExpressions;

namespace ConlangJson
{
    public static class ConLangUtilities
    {
        public static string SpellWord(string phonetic, List<SoundMap>soundMapList)
        {
            string spelled = phonetic;
            foreach(SoundMap soundMap in soundMapList)
            {
                spelled = Regex.Replace(spelled, soundMap.spelling_regex, soundMap.romanization);
            }

            return spelled;
        }

        public static string SoundOutWord(string word,  List<SoundMap>soundMapList) 
        {
            string phonetic = word;
            foreach( SoundMap soundMap in soundMapList) 
            {
                phonetic = Regex.Replace(phonetic, soundMap.pronounciation_regex, soundMap.phoneme);
            }

            return phonetic;
        }
    }
}
