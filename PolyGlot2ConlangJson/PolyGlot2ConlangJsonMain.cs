/*
 * Main program for the tool to convert PolyGlot save files into the Conlang JSON object format
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
// See https://aka.ms/new-console-template for more information

using ConlangJson;
using Mono.Options;
using PolyGlot2ConlangJson;
using System.IO.Compression;
using System.Xml.Linq;

string? inputFile = null;
string? outputFile = null;

Mono.Options.OptionSet options = new()
{
    { "i|input=", "The PolyGlot .pgd file", v => inputFile = v},
    { "o|output=", "The Conlang JSON file", v => outputFile = v }
};

try
{
    options.Parse(args);
}
catch (OptionException ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine("Try `PolyGlot2Json --help' for more information");
}

if (string.IsNullOrEmpty(inputFile))
{
    Console.WriteLine("--input is a required argument");
    Environment.Exit(1);
}
if (string.IsNullOrEmpty(outputFile))
{
    Console.WriteLine("--output is a required argument");
    Environment.Exit(1);
}

FileInfo inputFileInfo = new(inputFile);
FileInfo outputFileInfo = new(outputFile);

if (inputFileInfo.Exists)
{
    using FileStream zipToOpen = inputFileInfo.Open(FileMode.Open);
    using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Read);
    ZipArchiveEntry? entry = archive.GetEntry(@"PGDictionary.xml");
    if (entry != null)
    {
        using StreamReader reader = new(entry.Open());
        XElement polyGlotLanguage = XElement.Load(reader);

        LanguageDescription language = new();

        language.spelling_pronunciation_rules = SoundMapBuilder.BuildSoundMap(polyGlotLanguage);
    }
}