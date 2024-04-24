using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConlangJson;

namespace PolyGlot2ConlangJson
{
    internal static class SoundMapBuilder
    {
        public static List<SoundMap> BuildSoundMap(XElement polyGlotLanguage)
        {
            List<SoundMap> soundMapList = new();

            IEnumerable<XElement> proCollection = polyGlotLanguage.Descendants("pronunciationCollection");
            foreach(XElement p in proCollection) 
            {
                IEnumerable<XElement> proGuides = p.Descendants("proGuide");
                foreach (XElement proGuide in proGuides)
                {
                    XElement? proGuideBase = proGuide.Element("proGuideBase");
                    XElement? proGuidePhon = proGuide.Element("proGuidePhon");
                    SoundMap soundMap = new SoundMap();
                    soundMap.phoneme = proGuidePhon?.Value ?? string.Empty;
                    soundMap.pronunciation_regex = proGuideBase?.Value ?? string.Empty;
                    soundMap.romanization = proGuideBase?.Value ?? string.Empty;
                    soundMap.spelling_regex = proGuidePhon?.Value ?? string.Empty;
                    soundMapList.Add(soundMap);
                }
            }


            return soundMapList;
        }
    }
}
