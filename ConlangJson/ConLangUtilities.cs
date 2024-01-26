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
