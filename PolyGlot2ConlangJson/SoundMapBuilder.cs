/*
 * Static class for converting PolyGlot XML structures into Sound Maps
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
using ConlangJson;
using System.Xml.Linq;

namespace PolyGlot2ConlangJson
{
    internal static class SoundMapBuilder
    {
        public static List<SpellingPronunciationRules> BuildSoundMap(XElement polyGlotLanguage)
        {
            List<SpellingPronunciationRules> soundMapList = new();

            IEnumerable<XElement> proCollection = polyGlotLanguage.Descendants("pronunciationCollection");
            foreach (XElement p in proCollection)
            {
                IEnumerable<XElement> proGuides = p.Descendants("proGuide");
                foreach (XElement proGuide in proGuides)
                {
                    XElement? proGuideBase = proGuide.Element("proGuideBase");
                    XElement? proGuidePhon = proGuide.Element("proGuidePhon");
                    SpellingPronunciationRules soundMap = new SpellingPronunciationRules
                    {
                        phoneme = proGuidePhon?.Value ?? string.Empty,
                        pronunciation_regex = proGuideBase?.Value ?? string.Empty,
                        romanization = proGuideBase?.Value ?? string.Empty,
                        spelling_regex = proGuidePhon?.Value ?? string.Empty
                    };
                    soundMapList.Add(soundMap);
                }
            }


            return soundMapList;
        }
    }
}
