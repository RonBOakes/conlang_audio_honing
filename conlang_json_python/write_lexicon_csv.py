#!/usr/bin/python3 
# Copyright (C) 2024 Ronald B. Oakes
#
# This program is free software: you can redistribute it and/or modify it under
# the terms of the GNU General Public License as published by the Free Software
# Foundation, either version 3 of the License, or (at your option) any later
# version.
#
# This program is distributed in the hope that it will be useful, but WITHOUT
# ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS
# FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License along with
# this program.  If not, see <http://www.gnu.org/licenses/>.
#
# This program is used to convert a Conlang JSON object into a CSV file that can
# be imported into a spreadsheet or other program for various uses.
#
import sys
import getopt
import json
import pdb
import re
import csv
import codecs
from argparse import ArgumentParser
from lexicon_entry import LEXICON_ENTRY
from conlang_lib import spell_word, decline_word, derive_words, dedup_lexicon

# Define and parse the command line arguments
cli = ArgumentParser(description="Build a CSV version of the lexicon")
cli.add_argument("-i","--input", type=str, metavar="FILE_PATH", required=True, dest="input",
    help="Conlang JSON file to be converted into a CSV file")
cli.add_argument("-o","--output", type=str, metavar="FILE_PATH", required=True, dest="output",
    help="CSV file where the conlang information will be placed")
arguments = cli.parse_args()

inputfile = arguments.input
outputfile = arguments.output

# Read the JSON language data
with open(inputfile,"r", encoding="utf-8-sig") as ifp:
    language_structure = json.load(ifp)

lexicon = language_structure["lexicon"]

# Derive words if needed.
if not language_structure["derived"]:
    add_lexicon = []
    add_lexicon += derive_words(language_structure['derived_word_list'],
                                language_structure['derivational_affix_map'],
                                language_structure['lexicon'],
                                language_structure['sound_map_list'],
                                false)
    clean_lexicon = dedup_lexicon(add_lexicon)
    if len(clean_lexicon) < len(add_lexicon):
        add_lexicon = clean_lexicon
    for lex_entry in add_lexicon:
        lexicon.append(lex_entry.as_map())

# Decline words if needed.
if not language_structure["declined"]:
    add_lexicon = []
    for word in lexicon:
        add_lexicon += decline_word(word,language_structure['affix_map'],language_structure['sound_map_list'])
    clean_lexicon = dedup_lexicon(add_lexicon)
    if len(clean_lexicon) < len(add_lexicon):
        add_lexicon = clean_lexicon
    for lex_entry in add_lexicon:
        lexicon.append(lex_entry.as_map())

# Sort the language on its English words.
lexicon = sorted(language_structure["lexicon"], key=lambda x: x['english'].lower())

# Write it out in CSV format.
with open(outputfile, "w", newline='', encoding="utf-8-sig") as ofp:
    lexcsvwriter = csv.writer(ofp, dialect='excel')
    
    lexcsvwriter.writerow(['English Word',language_structure['native_name_english']+' Word','Part of Speech','Declensions','Pronunciation'])
    for entry in lexicon:
        lexcsvwriter.writerow([entry['english'],entry['spelled'],entry['part_of_speech'],entry['declensions'],entry['phonetic']])
