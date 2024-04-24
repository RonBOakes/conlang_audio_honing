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

        language.sound_map_list = SoundMapBuilder.BuildSoundMap(polyGlotLanguage);
    }
}