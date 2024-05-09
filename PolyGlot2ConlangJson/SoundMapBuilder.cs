﻿/*
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
