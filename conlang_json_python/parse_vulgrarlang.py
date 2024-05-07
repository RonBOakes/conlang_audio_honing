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
# This program is used to parse the file saved from Vulgarlang
# https://www.vulgarlang.com/, into the Conlang JSON format.  It has been tested to
# work with version 12.7.1 of the Vulgarlang output file, and Settings version version
# 12.0.
# 
# There are no guaranties that this tool will continue to work properly with any future updates
# to the Vulgarlang tool, or that all available Vulgarlang configurations will be properly parsed.
#
# 
import getopt
import json
import pdb
import re
import copy
import sys
import re
from argparse import ArgumentParser
from lexicon_entry import LEXICON_ENTRY
from conlang_lib import spell_word, derive_words, dedup_lexicon, decline_word, get_number_word, get_ipa_symbol_map

# Define the global patterns for matching consonants and vowels.
IPA_VOWELS_PATTERN = "[aioeu\u032f\u02d0]"
IPA_CONSONANT_PATTERN = "[^aioeuːaioeu\u032f\u02d0]"
IPA_VOWEL_SET = set()
SPELLING_VOWEL_PATTERN = "[aeiou\u0304]"
SPELLING_CONSONANT_PATTERN = "[^aeiou\u0304]"
SPELLING_VOWEL_SET = set()

def main(argv):
    
    # Define and parse the command line arguments
    cli = ArgumentParser(description="Parse a Vulgarlang save file")
    cli.add_argument("-i","--inputfile", type=str, metavar="FILE_PATH", required=True, dest="inputfile",
        help='Vulgarlang save file to be parsed')
    cli.add_argument("-v","--voice", type=str, required=False, dest="voice", 
        help='Amazon Polly Voice preferred for speaking this conlang')
    cli.add_argument("--espeak-language", type=str, required=False, dest="espeak_language",
        help='espeak-ng "voice" which is actually the language code')
    cli.add_argument("-l","--language", type=str, required=False, dest="language",
        help='Language which should be used to select the phonetics when speaking this language')
    cli.add_argument("-o","--output", type=str, required=True, metavar="FILE_PATH", dest="outputfile",
        help='File where the conlang JSON object should be placed')
    cli.add_argument("--derive", action="store_true", default=True, dest="derive",
        help='Indicates that the JSON object should contain derived words in addition to root words.  Default is to derive words')
    cli.add_argument("--decline", action="store_true", default=False, dest="decline",
        help='Indicates that the JSON object should contain the decilend form of all the words.  Default is to not decline words.  Using this option will produce a large JSON object')
    arguments = cli.parse_args()

    inputfile = arguments.inputfile
    outputfile = arguments.outputfile

    # Load the vulgarlang JSON save structure inconsantListuding the spelling (Romanization) rules and 
    # word list
    with open(inputfile,"rt", encoding="utf-8") as ifp:
        vulgarlang = json.load(ifp)
    
    comma_replace = re.compile(r'(\(\s*\w+\s*),(\s*\w+\s*\))')
    
    word_list = comma_replace.sub(r'\1;\2',vulgarlang.get("words").get("value")).split("\n")
    derived_word_list = comma_replace.sub(r'\1;\2',vulgarlang.get("derivedWords").get("value")).split("\n")
    spelling_rule_list = vulgarlang.get("spellingRules").get("value").split("\n")
    grammar = vulgarlang.get("grammarEditor").get('ops')
    lexical_order = vulgarlang.get("customAlphabetOrder").get("value").strip()
    derivational_affix_list = vulgarlang.get("derivationalAffixes").get("value").split("\n")
    noun_gender_list = vulgarlang.get("nounGenders").get("value").split("\n")
    
    # Go through the word list and extract the parts of speech.
    part_of_speech_set = set()
    for word in word_list:
        word_parts = word.split(':')
        left_part = word_parts[1].strip()
        left_parts = left_part.split('=')
        part_of_speech = left_parts[0].strip()
        part_of_speech_set.add(part_of_speech)
    
    # Set the lexical order.
    if lexical_order == '':
        lexical_order = 'abcdefghijklmnoöpqrstuvwxyz'
    lexical_order_list = list(lexical_order)
    lexical_order_list.append('\u2060')
    lexical_order_list.append(' ')
    LEXICON_ENTRY.set_lexical_order_list(lexical_order_list)
    
    # Remove the attributes from the grammar list
    for line in grammar:
        if 'attributes' in line.keys():
            grammar.remove(line)
    
    # Get the IPA patterns
    get_IPA_patterns(vulgarlang)
    
    # Parse the spelling rules
    sound_map_list = parse_spelling_rules(spelling_rule_list)
    
    # Parse the grammar rules.  This will build the affix_map and part of the lexicon
    affix_map,lexicon_fragment1 = parse_grammar_rules(grammar,part_of_speech_set,sound_map_list,
        {
            'IPA_VOWELS_PATTERN':IPA_VOWELS_PATTERN,
            'IPA_CONSONANT_PATTERN':IPA_CONSONANT_PATTERN,
            'SPELLING_VOWEL_PATTERN':SPELLING_VOWEL_PATTERN,
            'SPELLING_CONSONANT_PATTERN':SPELLING_CONSONANT_PATTERN
        })
    
    # Parse the derivation affixes.
    derivational_affix_map = parse_derivational_affix_list(derivational_affix_list,
        sound_map_list,
        {
            'IPA_VOWELS_PATTERN':IPA_VOWELS_PATTERN,
            'IPA_CONSONANT_PATTERN':IPA_CONSONANT_PATTERN,
            'SPELLING_VOWEL_PATTERN':SPELLING_VOWEL_PATTERN,
            'SPELLING_CONSONANT_PATTERN':SPELLING_CONSONANT_PATTERN
        })

    # Parse the word list to build the main part of the lexicon
    lexicon_fragment2 = parse_word_list(word_list,affix_map,sound_map_list)

    # Merge the two parts of the lexicon we have so far.
    lexicon = lexicon_fragment1 + lexicon_fragment2
    
    # Derive words if requested.
    if arguments.derive:
        add_lexicon = derive_words(derived_word_list,
                            derivational_affix_map,
                            lexicon,
                            affix_map,
                            sound_map_list,
                            arguments.decline)
    
        lexicon += add_lexicon
    
    # Decline the lexicon if requested
    if arguments.decline:
        add_lexicon = []
        for word in lexicon:
            add_lexicon += decline_word(word,affix_map,sound_map_list)
        lexicon += add_lexicon
        
    # Attempt to remove duplicate entries in the lexicon.
    clean_lexicon = dedup_lexicon(lexicon)
    if len(clean_lexicon) < len(lexicon):
        lexicon = clean_lexicon
    
    # Put the lexicon into order.
    lexicon.sort()
    
    # Convert the lexicon into a list
    lexicon_list = []
    for entry in lexicon:
        lexicon_list.append(entry.as_map())
    
    # Get the phoneme inventory
    phoneme_inventory = get_phoneme_inventory(vulgarlang)
    
    # Get the phonetic inventory
    phonetic_inventory = parse_phonetic_inventory(vulgarlang)

    # Get the word_order, adjective_position, and pre_post_position
    if "wordOrder" in vulgarlang:
        word_order = vulgarlang["wordOrder"]["radioID"]
    else:
        word_order = "SVO"
        
    if "adjectiveOrder" in vulgarlang:
        if vulgarlang["adjectiveOrder"]["radioID"] == "beforeAdjOrder":
            adjective_position = "Before"
        else:
            adjective_position = "After"
    else:
        adjective_position = "Before"
   
    if "prePostPosition" in vulgarlang:
        pre_post_position = vulgarlang["prePostPosition"]["radioID"]
    else:
        pre_post_position = "preposition"
            

    # Build the basic language structure.
    language_structure = {
                            'version':1.0,
                            'english_name':vulgarlang['anglicizedName']['value'].strip(),
                            'phonetic_characters':'ipa',
                            'native_name_phonetic':vulgarlang['ipaLangName']['value'].strip(),
                            'native_name_english':spell_word(vulgarlang['ipaLangName']['value'].strip(), sound_map_list).capitalize(),
                         }
    if arguments.voice:
        language_structure['preferred_voices'] = {'Polly':arguments.voice,'espeak-ng':arguments.espeak_language }
    if arguments.language:
        language_structure['preferred_language'] = arguments.language
    if arguments.derive:
        language_structure['derived'] = True;
    else:
        language_structure['derived'] = False;
    if arguments.decline:
        language_structure['declined'] = True;
    else:
        language_structure['declined'] = False;
    language_structure['noun_gender_list'] = noun_gender_list
    language_structure['part_of_speech_list'] = sorted(list(part_of_speech_set))
    language_structure['phoneme_inventory'] = phoneme_inventory
    language_structure['phonetic_inventory'] = phonetic_inventory
    language_structure['word_order'] = word_order
    language_structure['adjective_position'] = adjective_position
    language_structure['pre_post_position'] = pre_post_position
    language_structure['sound_map_list'] = sound_map_list
    language_structure['lexical_order_list'] = lexical_order_list
    language_structure['affix_map'] = affix_map
    language_structure['derivational_affix_map'] = derivational_affix_map
    language_structure['lexicon'] = lexicon_list
    language_structure['derived_word_list'] = derived_word_list
    language_structure['metadata'] = {'source':[{'vulgarlang':vulgarlang}]}

    # Save the file into a UTF-8 Byte Order Mark signed file to ensure that 
    # other tools can properly read it.
    with open(outputfile, 'wt', encoding="utf-8-sig") as ofp:
        json.dump(language_structure, ofp, ensure_ascii=False, indent=4)

#end def main(argv)

# This function is used during setup on at this point since it is used 
# for building a lexical order when one doesn't exist.
def get_spelling_symbol_list(vulgarlang,spelling_rule_list):
    sound_list = []
    sound_list += vulgarlang['customConsonants']['value'].split()
    sound_list += vulgarlang['customVowels']['value'].split()
    sound_list += vulgarlang['wordInitialConsonants']['value'].split()
    sound_list += vulgarlang['midWordConsonants']['value'].split()
    sound_list += vulgarlang['wordFinalConsonants']['value'].split()
    sound_list += vulgarlang['bwsVowels']['value'].split()
    sound_set = set(sound_list)
    
    spelling_symbol_list1 = []
    for sound in sound_set:
        spelling_symbol = spell_word(sound,spelling_rule_list)
        spelling_symbol_list1.append(spelling_symbol)
  
    spelling_symbol_set1 = set(spelling_symbol_list1)
    spelling_symbol_list2 = []
    nextchar = ''
    for char in reversed(''.join(spelling_symbol_set1)):
        charint = int(char.encode('utf-16-be').hex(),base=16) & 0x0000ffff
        if (charint >= 0x0300) and (charint <= 0x036f):
            nextchar = char
        elif nextchar != '':
            spelling_symbol_list2.append(char + nextchar)
            nextchar = ''
        else:
            spelling_symbol_list2.append(char)
    
    return list(set(spelling_symbol_list2))
#end get_spelling_symbol_list

# Extract the IPA patterns from the customVowels and bwsVowles fields of the
# Vulgarlang input.
def get_IPA_patterns(vulgarlang):
    sound_list = []
    sound_list += vulgarlang['customVowels']['value'].split()
    sound_list += vulgarlang['bwsVowels']['value'].split()
    sound_set = set(sound_list)
    
    ipa_symbol_list2 = []
    nextchar = ''
    # This block of code is used to tie diacritics to the proceeding character 
    for char in reversed(''.join(sound_set)):
        charint = int(char.encode('utf-16-be').hex(),base=16) & 0x0000ffff
        if (charint >= 0x0300) and (charint <= 0x036f):
            nextchar = char
        elif nextchar != '':
            ipa_symbol_list2.append(char + nextchar)
            nextchar = ''
        else:
            ipa_symbol_list2.append(char)
    # Turn the set back into a list.
    ipa_vowel_list = list(set(ipa_symbol_list2))
    
    global IPA_VOWELS_PATTERN
    global IPA_CONSONANT_PATTERN
    global IPA_VOWEL_SET
    
    # Finally rebuild the regex patterns.  Consonant is just defined as
    # the absence of a vowel since this pattern only will be used to match
    # strings that are known to only contain IPA.
    IPA_VOWELS_PATTERN = '['+ ''.join(ipa_vowel_list) + ']'
    IPA_CONSONANT_PATTERN = '[^'+ ''.join(ipa_vowel_list) + ']'
    IPA_VOWEL_SET = set(ipa_symbol_list2)

#end def get_IPA_patterns

def parse_spelling_rules(spelling_rule_list):
    global IPA_VOWELS_PATTERN
    global IPA_CONSONANT_PATTERN
    global IPA_VOWEL_SET
    global SPELLING_VOWEL_PATTERN
    global SPELLING_CONSONANT_PATTERN
    global SPELLING_VOWEL_SET

    # Initialize the sound map list with two default entries. 
    # These are the syllable separators.
    sound_map_list = []
    sound_map_list.append({'phoneme':'ˈ','romanization':'','spelling_regex':'ˈ','pronunciation_regex':'&&&&&&'})
    sound_map_list.append({'phoneme':'ˌ','romanization':'','spelling_regex':'ˌ','pronunciation_regex':'&&&&&&'})
    
    # Parse each spelling rule in turn and add it to the list.  
    # It is important to preserve the order since both Vulgarlang and the 
    # Conlang JSON format utilize the same order.
    for spelling_rule in spelling_rule_list:
        sound_map_list_addition = parse_spelling_rule(spelling_rule)
        if sound_map_list_addition:
            sound_map_list += sound_map_list_addition
    
    # Update the global spelling patterns which are now known thanks to
    # information gleened as a side effect of parsing the spelling rules.
    SPELLING_VOWEL_PATTERN = '[' + ''.join(SPELLING_VOWEL_SET) + ']'
    SPELLING_CONSONANT_PATTERN = '[^' + ''.join(SPELLING_VOWEL_SET) + ']'
    
    # Perform a number of global substitutions on the rules to replace 'C' and 'V' with the actual patterns.
    for sound_map in sound_map_list:
        sound_map['pronunciation_regex'] = re.sub('C',SPELLING_CONSONANT_PATTERN, sound_map['pronunciation_regex'])
        sound_map['pronunciation_regex'] = re.sub('V',SPELLING_VOWEL_PATTERN, sound_map['pronunciation_regex'])
        sound_map['romanization'] = re.sub('C',SPELLING_CONSONANT_PATTERN, sound_map['romanization'])
        sound_map['romanization'] = re.sub('V',SPELLING_VOWEL_PATTERN, sound_map['romanization'])
        sound_map['spelling_regex'] = re.sub('C',IPA_CONSONANT_PATTERN, sound_map['spelling_regex'])
        sound_map['spelling_regex'] = re.sub('V',IPA_VOWELS_PATTERN, sound_map['spelling_regex'])
        sound_map['phoneme'] = re.sub('C',IPA_CONSONANT_PATTERN, sound_map['phoneme'])
        sound_map['phoneme'] = re.sub('V',IPA_VOWELS_PATTERN, sound_map['phoneme'])
    
    return sound_map_list
#end def parse_spelling_rules(spelling_rule_list)

# Parse an individual spelling rule from Vulgarlang and build the Conlang JSON
# Regular expression based sound map entry.
def parse_spelling_rule(spelling_rule):
    global IPA_VOWELS_PATTERN
    global IPA_CONSONANT_PATTERN
    global IPA_VOWEL_SET
    global SPELLING_VOWEL_PATTERN
    global SPELLING_CONSONANT_PATTERN
    global SPELLING_VOWEL_SET

# Precompile two patterns that will be needed for matching common 
# Vulgarlang spelling rules
    group_repl_pattern1 = re.compile(r'([VC])(.+?)\s*>\s*\1\1')
    group_repl_pattern2 = re.compile(r'([VC])(.+?)\s*>\s*\1(.+?)')

    sound_map_list = []
    sound_map = {}

    if not re.match(r'^\s*/.*',spelling_rule):
        # Perform the matches on the two precompiled patterns, and if they match
        # Build the appropriate entry 
        group_repl_match1 = group_repl_pattern1.match(spelling_rule)
        group_repl_match2 = group_repl_pattern2.match(spelling_rule)
        if group_repl_match1:
            sound_map = {
                'phoneme':'$1'+group_repl_match1.group(2),
                'romanization':'$1$1',
                'spelling_regex':r'('+group_repl_match1.group(1)+r')'+group_repl_match1.group(2),
                'pronunciation_regex':r'('+group_repl_match1.group(1)+r')\1'
            }
            sound_map_list.append(sound_map)
        elif group_repl_match2:
            sound_map = {
                'phoneme':'$1'+group_repl_match2.group(2),
                'romanization':'$1'+group_repl_match2.group(3),
                'spelling_regex':r'('+group_repl_match2.group(1)+r')'+group_repl_match2.group(2),
                'pronunciation_regex':r'('+group_repl_match2.group(1)+r')'+group_repl_match2.group(3)
            }
            sound_map_list.append(sound_map)
        else:
            # Split the rule into parts based on the > used to split the Vulgarlang spelling rule
            # into its pattern and replacement sections.  Then process these as needed.
            rule_parts = spelling_rule.split('>')
            phonemes = rule_parts[0].strip()
            
            # Get the individual phonemes in the pattern part.
            match = re.match(r'(\S+)\{(.*)\}',phonemes)
            if match:
                phoneme_list = []
                for part in match.group(2).split(','):
                    phoneme_list.append(match.group(1).strip() + part)
            else:
                match = re.match(r'^\s*\{(.*)\}',phonemes)
                if match:
                    phoneme_list = []
                    for part in match.group(1).split(','):
                        phoneme_list.append(part)
                else:
                    match = re.match(r'^\s*(\S+)\s*\(\s*(\S+)\s*\)\s*$',phonemes)
                    if match:
                        phoneme_list = [
                            match.group(1)+match.group(2),  # Long pattern must be first.
                            match.group(1),
                        ]
                    else:
                        phoneme_list = [phonemes]
            # Iterate over the phoneme list determined in the previous block...
            for phoneme in phoneme_list:
                roman = rule_parts[1].strip()
                # If the sound change part indicates an option call parse_sound_change to process it.
                if '/' in roman:
                    roman_list = roman.split('/')
                    roman = roman_list[0].strip()
                    sound_map['romanization'] = roman_list[0].strip()
                    sound_map = parse_sound_change(phoneme,roman_list[0].strip(),roman_list[1].strip())
                    #sound_map['sound_change'] = roman_list[1].strip()
                # Otherwise it can be directly added.
                else:
                    sound_map = {'phoneme':phoneme,'romanization':roman}
                    sound_map['spelling_regex']=sound_map['phoneme']
                    sound_map['pronunciation_regex']=sound_map['romanization']
                #print ("\t",end="")
                #print(sound_map)
                sound_map_list.append(sound_map)
                # If we have discovered a new vowel in our romanization, add it to the set 
                if phoneme in IPA_VOWEL_SET and roman not in SPELLING_VOWEL_SET:
                    SPELLING_VOWEL_SET.add(roman)
        return sound_map_list
#end def parse_spelling_rule

# Parse the Vulgarlang Sound Change Notation for spelling rules.
def parse_sound_change(phoneme,roman,sound_change):
    # This functions looks for specific patterns and then
    # applies them individually.  The order of matching needs to
    # ensure that the correct pattern matches in cases where a latter
    # pattern might otherwise trigger a false match.

    match1 = re.match(r'(\S+)([CV])',phoneme)
    match2 = re.match(r'(\S+)([CV])(\S+)',roman)
    if sound_change == '_#' and match1 and match2:
        phoneme = match1.group(1) + r'$1'
        roman = match2.group(1) + r'$1' + match2.group(3)
        spelling_regex = match1.group(1) + '(' + match1.group(2) + ')'
        pronunciation_regex = match2.group(1) + '(' + match2.group(2) + ')' + match2.group(3)
        return {
                'phoneme':phoneme,
                'romanization':roman,
                'spelling_regex':spelling_regex,
                'pronunciation_regex':pronunciation_regex
               }

    if sound_change == '#_':
        return {
                'phoneme':phoneme,
                'romanization':roman,
                'spelling_regex':'^'+phoneme,
                'pronunciation_regex':'^'+roman
               }
               
    if sound_change == '_#':
        return {
                'phoneme':phoneme,
                'romanization':roman,
                'spelling_regex':phoneme+'$',
                'pronunciation_regex':roman+'$'
               }

    if sound_change == '#_#':
        return {
                'phoneme':phoneme,
                'romanization':roman,
                'spelling_regex':'^'+phoneme+'$',
                'pronunciation_regex':'^'+roman+'$'
               }

    if sound_change == 'C_':
        return {
                'phoneme':'$1'+phoneme,
                'romanization':'$1'+roman,
                'spelling_regex':'(C)'+phoneme,
                'pronunciation_regex':'(C)'+roman
               }

    if sound_change == 'C_#':
        return {
                'phoneme':phoneme,
                'romanization':roman,
                'spelling_regex':'C'+phoneme+'$',
                'pronunciation_regex':'C'+roman+'$'
               }
               
    if sound_change == 'VV':
        return {
                'phoneme':'$1',
                'romanization':'$1$1',
                'spelling_regex':r'(V)',
                'pronunciation_regex':r'(V)\1'
               }


    match = re.match(r'_(\S+)',sound_change)
    if match:
        spelling_regex = phoneme + match.group(1)
        pronunciation_regex = roman + match.group(1)
        return {
                'phoneme':phoneme,
                'romanization':roman,
                'spelling_regex':spelling_regex,
                'pronunciation_regex':pronunciation_regex
               }
        
    match = re.match(r'!_{(\S+)}',sound_change)
    if match:
        sounds = match.group(1).split(',')
        spelling_regex = phoneme + '([^' + ''.join(sounds) + '])'
        pronunciation_regex = roman + '([^' + ''.join(sounds) + '])'
        return {
                'phoneme':phoneme+'$1',
                'romanization':roman+'$1',
                'spelling_regex':spelling_regex,
                'pronunciation_regex':pronunciation_regex
               }
        
    match = re.match(r'(\S+)_#',sound_change)
    if match:
        spelling_regex = match.group(1) + phoneme + '$'
        pronunciation_regex = match.group(1) + roman + '$'
        return {
                'phoneme':match.group(1) + phoneme,
                'romanization':match.group(1) + roman,
                'spelling_regex':spelling_regex,
                'pronunciation_regex':pronunciation_regex
               }
        
    print("Unmatched sound_change pattern: " + sound_change)
    exit()
    # TODO: add other patterns as needed.
#end def parse_sound_change(phoneme,sound_change)

# Parses the Vulgarlang word list and uses it to build a partial
# lexicon in the format to be put into the Conlang JSON object format.
def parse_word_list(word_list,affix_map,sound_map_list):
    lexicon_fragment = []
    for word in word_list:
        word = word.replace('\u2060','') # Remove the Word Joiners that have a pernicious habit of sneaking into words.
        word_parts = word.split(':')
        left_part = word_parts[1].strip()
        left_parts = left_part.split('=')
        part_of_speech = left_parts[0].strip()
        phonetic = left_parts[1].strip()
        if '<' in phonetic:
            phonetic_parts = phonetic.split('<')
            phonetic = phonetic_parts[0].strip()
        english_parts = word_parts[0].strip()
        english_list = english_parts.split(',')
        for english in english_list:
            lexicon_fragment.append(LEXICON_ENTRY(phonetic.strip(),spell_word(phonetic,sound_map_list),english.strip(),part_of_speech.strip(),'root'))
    return lexicon_fragment
                 
#end def parse_word_list(word_list,affix_map,sound_map_list)

# Extracts the phonetic inventory from the Vulgarlang fields that contain 
# this information.
def get_phoneme_inventory(vulgarlang):
    phoneme_inventory_set = set()
    
    for phoneme in vulgarlang['wordInitialConsonants']['value'].strip().split():
        phoneme_inventory_set.add(phoneme)
    for phoneme in vulgarlang['midWordConsonants']['value'].strip().split():
        phoneme_inventory_set.add(phoneme)
    for phoneme in vulgarlang['wordFinalConsonants']['value'].strip().split():
        phoneme_inventory_set.add(phoneme)
    for phoneme in vulgarlang['bwsVowels']['value'].strip().split():
        phoneme_inventory_set.add(phoneme)
    for phoneme in vulgarlang['bws2ndVowels']['value'].strip().split():
        phoneme_inventory_set.add(phoneme)
    for phoneme in vulgarlang['customConsonants']['value'].strip().split():
        phoneme_inventory_set.add(phoneme)
    for phoneme in vulgarlang['customVowels']['value'].strip().split():
        phoneme_inventory_set.add(phoneme)
        
    return sorted(list(phoneme_inventory_set))
#end get_phoneme_inventory 

# Parse the derived word affix rules in a Vulgarlang output.
def parse_derivational_affix_list(derivational_affix_list,sound_map_list,patterns):
    IPA_VOWELS_PATTERN        = patterns['IPA_VOWELS_PATTERN']
    IPA_CONSONANT_PATTERN     = patterns['IPA_CONSONANT_PATTERN']
    SPELLING_VOWEL_PATTERN     = patterns['SPELLING_VOWEL_PATTERN']
    SPELLING_CONSONANT_PATTERN = patterns['SPELLING_CONSONANT_PATTERN']
    
    derivational_affix_map = {}
    
    # Precompile the patterns.
    vowel_prefix_pattern = re.compile(r'^\s*IF\s*#V\s*THEN\s*(\S+)-\s*ELSE\s*(\S+)-\s*$')
    vowel_suffix_pattern = re.compile(r'^\s*IF\s*V#\s*THEN\s*-(\S+)\s*ELSE\s*-(\S+)\s*$')
    consonant_prefix_pattern = re.compile(r'^\s*IF\s*#V\s*THEN\s*(\S+)-\s*ELSE\s*(\S+)-\s*$')
    consonant_suffix_pattern = re.compile(r'^\s*IF\s*V#\s*THEN\s*-(\S+)\s*ELSE\s*-(\S+)\s*$')
    prefix_pattern = re.compile(r'^\s*(\S+)-\s*$')
    suffix_pattern = re.compile(r'^\s*-(\S+)\s*$')
    
    
    for derivational_affix in derivational_affix_list:
        # split the Vulgarlang rule on the =.
        parts = derivational_affix.split('=')
        derivation_type = parts[0].strip()
        rule_part = parts[1].strip()
        
        # Perform the matches on the precompiled patterns.
        vowel_prefix_match = vowel_prefix_pattern.match(rule_part)
        vowel_suffix_match = vowel_suffix_pattern.match(rule_part)
        consonant_prefix_match = consonant_prefix_pattern.match(rule_part)
        consonant_suffix_match = consonant_suffix_pattern.match(rule_part)
        prefix_match = prefix_pattern.match(rule_part)
        suffix_match = suffix_pattern.match(rule_part)
        
        # Apply each match and build the appropriate map entry.
        if vowel_prefix_match:
            t_spelling_add = spell_word(vowel_prefix_match.group(1),sound_map_list)
            f_spelling_add = spell_word(vowel_prefix_match.group(2),sound_map_list)
            map_entry = {
                'type':'PREFIX',
                'pronunciation_regex':'^'+IPA_VOWELS_PATTERN,
                'spelling_regex':'^'+SPELLING_VOWEL_PATTERN,
                't_pronunciation_add':vowel_prefix_match.group(1),
                't_spelling_add':t_spelling_add,
                'f_pronunciation_add':vowel_prefix_match.group(2),
                'f_spelling_add':f_spelling_add
            }
        elif vowel_suffix_match:
            t_spelling_add = spell_word(vowel_suffix_match.group(1),sound_map_list)
            f_spelling_add = spell_word(vowel_suffix_match.group(2),sound_map_list)
            map_entry = {
                'type':'SUFFIX',
                'pronunciation_regex':IPA_VOWELS_PATTERN+'$',
                'spelling_regex':SPELLING_VOWEL_PATTERN+'$',
                't_pronunciation_add':vowel_suffix_match.group(1),
                't_spelling_add':t_spelling_add,
                'f_pronunciation_add':vowel_suffix_match.group(2),
                'f_spelling_add':f_spelling_add
            }
        elif consonant_prefix_match:
            t_spelling_add = spell_word(consonant_prefix_match.group(1),sound_map_list)
            f_spelling_add = spell_word(consonant_prefix_match.group(2),sound_map_list)
            map_entry = {
                'type':'PREFIX',
                'pronunciation_regex':'^'+IPA_CONSONANT_PATTERN,
                'spelling_regex':'^'+SPELLING_CONSONANT_PATTERN,
                't_pronunciation_add':consonant_prefix_match.group(1),
                't_spelling_add':t_spelling_add,
                'f_pronunciation_add':consonant_prefix_match.group(2),
                'f_spelling_add':f_spelling_add
            }
        elif consonant_suffix_match:
            t_spelling_add = spell_word(consonant_suffix_match.group(1),sound_map_list)
            f_spelling_add = spell_word(consonant_suffix_match.group(2),sound_map_list)
            map_entry = {
                'type':'SUFFIX',
                'pronunciation_regex':IPA_CONSONANT_PATTERN+'$',
                'spelling_regex':SPELLING_CONSONANT_PATTERN+'$',
                't_pronunciation_add':consonant_suffix_match.group(1),
                't_spelling_add':t_spelling_add,
                'f_pronunciation_add':consonant_suffix_match.group(2),
                'f_spelling_add':f_spelling_add
            }
        elif prefix_match:
            map_entry = {
                'type':'PREFIX',
                'pronunciation_add':prefix_match.group(1),
                'spelling_add':spell_word(prefix_match.group(1),sound_map_list),
            }
        elif suffix_match:
            map_entry = {
                'type':'SUFFIX',
                'pronunciation_add':suffix_match.group(1),
                'spelling_add':spell_word(suffix_match.group(1),sound_map_list),
            }
        else:
            print("Invalid derivation rule: " + derivational_affix)
            exit()
        derivational_affix_map[derivation_type] = map_entry

    return derivational_affix_map
    
#end def parse_derivational_affix_list

# Parse the grammar rules section of a Vulgarlang save file.
def parse_grammar_rules(grammar,part_of_speech_set,sound_map_list,patterns):
    IPA_VOWELS_PATTERN        = patterns['IPA_VOWELS_PATTERN']
    IPA_CONSONANT_PATTERN     = patterns['IPA_CONSONANT_PATTERN']
    SPELLING_VOWEL_PATTERN     = patterns['SPELLING_VOWEL_PATTERN']
    SPELLING_CONSONANT_PATTERN = patterns['SPELLING_CONSONANT_PATTERN']

    # Precompile some patterns.
    num_pattern1 = re.compile(r'^\s*(\d)\w+\s*$')
    num_pattern2 = re.compile(r'^\s*(\d)\w+\s+(\w+.*)\s*$')

    # Get the noun parts of speech - that is nouns with their genders.
    noun_type_list = []
    for part_of_speech in part_of_speech_set:
        if part_of_speech != 'num' and part_of_speech.startswith('n'):
            noun_type_list.append(part_of_speech)

    working_affix_map = {}
    affix_map = {}
    lexicon_fragment = []
    
    table_type = ''
    part_of_speech = ''
    declension_map = {}
    
    tbd_entries = []
    
    # The order of the grammar section is important, so it must be traversed as seen.
    for entry in grammar:
        if 'insert' in entry:
            line = entry['insert'].strip()
            if line.startswith('TABLE TYPE'):
                if len(working_affix_map) > 0:
                    if part_of_speech == 'n':
                        for noun in noun_type_list:
                            if noun not in affix_map:
                                affix_map[noun] = []
                            for affix in working_affix_map:
                                if affix != 'particle':
                                    affix_map[noun].append({affix:working_affix_map[affix]})
                                else:
                                    for entry in working_affix_map[affix]:
                                        #entry is a map with a single key
                                        particle_declension = list(entry.keys())[0]
                                        if 'pronunciation_regex' in entry[particle_declension]:
                                            if IPA_VOWELS_PATTERN in entry[particle_declension]['pronunciation_regex']:
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                      entry[particle_declension]['t_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                      entry[particle_declension]['f_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                            else:
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                      entry[particle_declension]['t_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                      entry[particle_declension]['f_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                        else:
                                            lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['pronunciation_add'],
                                                                                  entry[particle_declension]['spelling_add'],
                                                                                  "<"+particle_declension+" Particle>","Special",[particle_declension]))
                    else:
                        if part_of_speech not in affix_map:
                            affix_map[part_of_speech] = []
                        for affix in working_affix_map:
                            if affix != 'particle':
                                affix_map[part_of_speech].append({affix:working_affix_map[affix]})
                            else:
                                for entry in working_affix_map[affix]:
                                    #entry is a map with a single key
                                    particle_declension = list(entry.keys())[0]
                                    if (entry[particle_declension]): # Test to see if there is anything in the map, otherwise don't add a particle
                                        if 'pronunciation_regex' in entry[particle_declension]:
                                            if IPA_VOWELS_PATTERN in entry[particle_declension]['pronunciation_regex']:
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                    entry[particle_declension]['t_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                    entry[particle_declension]['f_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                            else:
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                    entry[particle_declension]['t_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                    entry[particle_declension]['f_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                        else:
                                            lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['pronunciation_add'],
                                                                                entry[particle_declension]['spelling_add'],
                                                                                "<"+particle_declension+" Particle>","Special",[particle_declension]))
                working_affix_map = {}
                line_parts = line.split('=')
                table_type = line_parts[1].strip()
                part_of_speech = ''
                declension_map = {}
                last_affix_type = 'TBD'
                affix_type = ''
            elif line.startswith('rows') or line.startswith('cols') or line.startswith('blocks'):
                line_parts = line.split('=')
                row_list = line_parts[1].split('/')
                for row in row_list:
                    num_match1 = num_pattern1.match(row)
                    num_match2 = num_pattern2.match(row)
                    if num_match1:
                        declension_abrv = num_match1.group(1)
                        declension = get_number_word(num_match2.group(1))
                        declension_map[declension_abrv.strip()] = declension
                    elif num_match2:
                        declension_abrv = num_match2.group(1)
                        declension = get_number_word(num_match2.group(1))
                        declension_map[declension_abrv.strip()] = declension
                        declension_abrv = ''
                        declension = num_match2.group(2).strip().capitalize()
                        for let in num_match2.group(2).strip():
                            if let == let.upper():
                                if let == ' ':
                                    declension_abrv += '.'
                                else:
                                    declension_abrv += let
                        declension_map[declension_abrv.strip()] = declension
                    else:
                        declension_abrv = ''
                        for let in row.strip():
                            if let == let.upper():
                                if let == ' ':
                                    declension_abrv += '.'
                                else:
                                    declension_abrv += let
                        declension = row.strip().capitalize()
                        if declension_abrv.endswith('.'):
                            declension_abrv = declension_abrv[0:-1]
                        declension_map[declension_abrv.strip()] = declension
            elif line.startswith('part-of-speech'):
                # If a part-of-speech is encountered after building part of 
                # an affix table, then a new table is being started even if
                # a new table directive is not encountered.
                if len(working_affix_map) > 0:
                    if part_of_speech == 'n':
                        for noun in noun_type_list:
                            if noun not in affix_map:
                                affix_map[noun] = []
                            for affix in working_affix_map:
                                if affix != 'particle':
                                    affix_map[noun].append({affix:working_affix_map[affix]})
                                else:
                                    for entry in working_affix_map[affix]:
                                        #entry is a map with a single key
                                        particle_declension = list(entry.keys())[0]
                                        if 'pronunciation_regex' in entry[particle_declension]:
                                            if IPA_VOWELS_PATTERN in entry[particle_declension]['pronunciation_regex']:
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                      entry[particle_declension]['t_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                      entry[particle_declension]['f_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                            else:
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                      entry[particle_declension]['t_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                      entry[particle_declension]['f_spelling_add'],
                                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                        else:
                                            lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['pronunciation_add'],
                                                                                  entry[particle_declension]['spelling_add'],
                                                                                  "<"+particle_declension+" Particle>","Special",[particle_declension]))
                    else:
                        if part_of_speech not in affix_map:
                            affix_map[part_of_speech] = []
                        for affix in working_affix_map:
                            if affix != 'particle':
                                affix_map[part_of_speech].append({affix:working_affix_map[affix]})
                            else:
                                for entry in working_affix_map[affix]:
                                    #entry is a map with a single key
                                    particle_declension = list(entry.keys())[0]
                                    if(entry[particle_declension]):
                                        if 'pronunciation_regex' in entry[particle_declension]:
                                            if IPA_VOWELS_PATTERN in entry[particle_declension]['pronunciation_regex']:
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                    entry[particle_declension]['t_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                    entry[particle_declension]['f_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                            else:
                                                particle_declension = "Consonant " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                                    entry[particle_declension]['t_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                                particle_declension = "Vowel " + particle_declension
                                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                                    entry[particle_declension]['f_spelling_add'],
                                                                                    "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                        else:
                                            lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['pronunciation_add'],
                                                                                entry[particle_declension]['spelling_add'],
                                                                                "<"+particle_declension+" Particle>","Special",[particle_declension]))
                    working_affix_map = {}
                    declension_map = {}
                    last_affix_type = 'TBD'
                    affix_type = ''
                line_parts = line.split('=')
                part_of_speech = line_parts[1].strip()
                if part_of_speech == 'pron':
                    # Vulgarlang tends to not include the person declension for pronouns, put it in
                    declension_map['1'] = '1st Person'
                    declension_map['2'] = '2nd Person'
                    declension_map['3'] = '3rd Person'
            elif re.match(r'^\s*[0-9A-Z.]+\s+~\s+.*$',line):
                line_parts = line.split('~')
                declension = ''
                abbrv_list = []
                abbrv_try = ''
                apbrv_part_list = line_parts[0].strip().split('.')
                for abbrv_part in apbrv_part_list:
                    if abbrv_part in declension_map:
                        abbrv_list.append(abbrv_part)
                        abbrv_try = abbrv_part
                    else:
                        if abbrv_try != '':
                            abbrv_try += '.' + abbrv_part
                            if abbrv_try in declension_map:
                                abbrv_list.append(abbrv_try)
                                abbrv_try = ''
                if abbrv_try != '' and abbrv_try not in abbrv_list:
                    print("Unmapped declension abbreviation: " + abbrv_try + "(" + line + ")")
                    print(abbrv_list)
                    print(declension_map)
                    exit()
                for abbrv_part in abbrv_list:
                    if declension != '':
                        declension += ' '
                    declension += declension_map[abbrv_part]
                if (table_type == 'affix') or (table_type == 'prefix') or (table_type == 'postfix'):
                    if '=' in line_parts[1]:
                        rule_list = line_parts[1].split('=')
                        affix_rule = rule_list[1].strip()
                    else:
                        affix_rule = line_parts[1].strip()
                    if affix_type != '':
                        last_affix_type = affix_type
                    affix_type, map_entry = parse_affix_rule(affix_rule,sound_map_list,patterns)
                    if affix_type == '':
                        affix_type = last_affix_type
                    elif last_affix_type == 'TBD':
                        if 'TBD' in working_affix_map:
                            working_affix_map[affix_type] = working_affix_map['TBD']
                            del working_affix_map['TBD']
                    if affix_type not in working_affix_map:
                        working_affix_map[affix_type] = []
                    working_affix_map[affix_type].append({declension:map_entry})
                elif table_type == 'word':
                    word_parts = line_parts[1].split('=')
                    english = word_parts[0].strip()
                    phonetic = word_parts[1].strip()
                    spelled = spell_word(phonetic,sound_map_list)
                    if part_of_speech == '':
                        print("ERROR: rule found without part of speech set")
                        exit()
                    lexicon_fragment.append(LEXICON_ENTRY(phonetic,spelled,english,part_of_speech,[declension]))

    if len(working_affix_map) > 0:
        if part_of_speech == 'n':
            for noun in noun_type_list:
                if noun not in affix_map:
                    affix_map[noun] = []
                for affix in working_affix_map:
                    if affix != 'particle':
                        affix_map[noun].append({affix:working_affix_map[affix]})
                    else:
                        for entry in working_affix_map[affix]:
                            #entry is a map with a single key
                            particle_declension = list(entry.keys())[0]
                            if 'pronunciation_regex' in entry[particle_declension]:
                                if IPA_VOWELS_PATTERN in entry[particle_declension]['pronunciation_regex']:
                                    particle_declension = "Vowel " + particle_declension
                                    lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                          entry[particle_declension]['t_spelling_add'],
                                                                          "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                    particle_declension = "Consonant " + particle_declension
                                    lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                          entry[particle_declension]['f_spelling_add'],
                                                                          "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                else:
                                    particle_declension = "Consonant " + particle_declension
                                    lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                          entry[particle_declension]['t_spelling_add'],
                                                                          "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                    particle_declension = "Vowel " + particle_declension
                                    lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                          entry[particle_declension]['f_spelling_add'],
                                                                          "<"+particle_declension+" Particle>","Special",[particle_declension]))
                            else:
                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['pronunciation_add'],
                                                                      entry[particle_declension]['spelling_add'],
                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
        else:
            if part_of_speech not in affix_map:
                affix_map[part_of_speech] = []
            for affix in working_affix_map:
                if affix != 'particle':
                    affix_map[part_of_speech].append({affix:working_affix_map[affix]})
                else:
                    for entry in working_affix_map[affix]:
                        #entry is a map with a single key
                        particle_declension = list(entry.keys())[0]
                        if 'pronunciation_regex' in entry[particle_declension]:
                            if IPA_VOWELS_PATTERN in entry[particle_declension]['pronunciation_regex']:
                                particle_declension = "Vowel " + particle_declension
                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                      entry[particle_declension]['t_spelling_add'],
                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                particle_declension = "Consonant " + particle_declension
                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                      entry[particle_declension]['f_spelling_add'],
                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                            else:
                                particle_declension = "Consonant " + particle_declension
                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['t_pronunciation_add'],
                                                                      entry[particle_declension]['t_spelling_add'],
                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                                particle_declension = "Vowel " + particle_declension
                                lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['f_pronunciation_add'],
                                                                      entry[particle_declension]['f_spelling_add'],
                                                                      "<"+particle_declension+" Particle>","Special",[particle_declension]))
                        else:
                            lexicon_fragment.append(LEXICON_ENTRY(entry[particle_declension]['pronunciation_add'],
                                                                  entry[particle_declension]['spelling_add'],
                                                                  "<"+particle_declension+" Particle>","Special",[particle_declension]))

    return affix_map, lexicon_fragment

#end def parse_grammar_rules

# Precompile a number of patterns used below.
vowel_prefix_pattern       = re.compile(r'^\s*IF\s*#V\s*THEN\s*(\S+)-\s*ELSE\s*(\S+\s*)-\s*$')
vowel_particle_pattern     = re.compile(r'^\s*IF\s*#V\s*THEN\s*(\S+\s+)-\s*ELSE\s*(\S+\s*)-\s*$')
vowel_suffix_pattern       = re.compile(r'^\s*IF\s*V#\s*THEN\s*-(\S+)\s*ELSE\s*-(\s*\S+)\s*$')
consonant_prefix_pattern   = re.compile(r'^\s*IF\s*#V\s*THEN\s*(\S+)-\s*ELSE\s*(\S+\s*)-\s*$')
consonant_particle_pattern = re.compile(r'^\s*IF\s*#V\s*THEN\s*(\S+\s+)-\s*ELSE\s*(\S+\s*)-\s*$')
consonant_suffix_pattern   = re.compile(r'^\s*IF\s*V#\s*THEN\s*-(\S+)\s*ELSE\s*-(\s*\S+)\s*$')
prefix_pattern             = re.compile(r'^\s*(\S+)-\s*$')
particle_pattern           = re.compile(r'^\s*(\S+\s+)-\s*$')
suffix_pattern             = re.compile(r'^\s*-(\S+)\s*$')

def parse_affix_rule(affix_rule,sound_map_list,patterns):
    IPA_VOWELS_PATTERN        = patterns['IPA_VOWELS_PATTERN']
    IPA_CONSONANT_PATTERN     = patterns['IPA_CONSONANT_PATTERN']
    SPELLING_VOWEL_PATTERN     = patterns['SPELLING_VOWEL_PATTERN']
    SPELLING_CONSONANT_PATTERN = patterns['SPELLING_CONSONANT_PATTERN']

    global vowel_prefix_pattern    
    global vowel_suffix_pattern    
    global consonant_prefix_pattern
    global consonant_suffix_pattern
    global prefix_pattern          
    global suffix_pattern          

    vowel_prefix_match = vowel_prefix_pattern.match(affix_rule)
    vowel_particle_match = vowel_particle_pattern.match(affix_rule)
    vowel_suffix_match = vowel_suffix_pattern.match(affix_rule)
    consonant_prefix_match = consonant_prefix_pattern.match(affix_rule)
    consonant_particle_match = consonant_particle_pattern.match(affix_rule)
    consonant_suffix_match = consonant_suffix_pattern.match(affix_rule)
    prefix_match = prefix_pattern.match(affix_rule)
    particle_match = particle_pattern.match(affix_rule)
    suffix_match = suffix_pattern.match(affix_rule)
    
    if vowel_prefix_match:
        t_spelling_add = spell_word(vowel_prefix_match.group(1),sound_map_list)
        f_spelling_add = spell_word(vowel_prefix_match.group(2),sound_map_list)
        affix_type = 'prefix'
        map_entry = {
            'pronunciation_regex':'^'+IPA_VOWELS_PATTERN,
            'spelling_regex':'^'+SPELLING_VOWEL_PATTERN,
            't_pronunciation_add':vowel_prefix_match.group(1),
            't_spelling_add':t_spelling_add,
            'f_pronunciation_add':vowel_prefix_match.group(2),
            'f_spelling_add':f_spelling_add
        }
    elif vowel_particle_match:
        t_spelling_add = spell_word(vowel_particle_match.group(1).strip(),sound_map_list)
        f_spelling_add = spell_word(vowel_particle_match.group(2).strip(),sound_map_list)
        affix_type = 'particle'
        map_entry = {
            'pronunciation_regex':'^'+IPA_VOWELS_PATTERN,
            'spelling_regex':'^'+SPELLING_VOWEL_PATTERN,
            't_pronunciation_add':vowel_particle_match.group(1),
            't_spelling_add':t_spelling_add,
            'f_pronunciation_add':vowel_particle_match.group(2),
            'f_spelling_add':f_spelling_add
        }
    elif vowel_suffix_match:
        t_spelling_add = spell_word(vowel_suffix_match.group(1).strip(),sound_map_list)
        f_spelling_add = spell_word(vowel_suffix_match.group(2).strip(),sound_map_list)
        affix_type = 'suffix'
        map_entry = {
            'pronunciation_regex':IPA_VOWELS_PATTERN+'$',
            'spelling_regex':SPELLING_VOWEL_PATTERN+'$',
            't_pronunciation_add':vowel_suffix_match.group(1),
            't_spelling_add':t_spelling_add,
            'f_pronunciation_add':vowel_suffix_match.group(2),
            'f_spelling_add':f_spelling_add
        }
    elif consonant_prefix_match:
        t_spelling_add = spell_word(consonant_prefix_match.group(1).strip(),sound_map_list)
        f_spelling_add = spell_word(consonant_prefix_match.group(2).strip(),sound_map_list)
        affix_type = 'prefix'
        map_entry = {
            'pronunciation_regex':'^'+IPA_CONSONANT_PATTERN,
            'spelling_regex':'^'+SPELLING_CONSONANT_PATTERN,
            't_pronunciation_add':consonant_prefix_match.group(1),
            't_spelling_add':t_spelling_add,
            'f_pronunciation_add':consonant_prefix_match.group(2),
            'f_spelling_add':f_spelling_add
        }
    elif consonant_particle_match:
        t_spelling_add = spell_word(consonant_particle_match.group(1).strip(),sound_map_list)
        f_spelling_add = spell_word(consonant_particle_match.group(2).strip(),sound_map_list)
        affix_type = 'particle'
        map_entry = {
            'pronunciation_regex':'^'+IPA_CONSONANT_PATTERN,
            'spelling_regex':'^'+SPELLING_CONSONANT_PATTERN,
            't_pronunciation_add':consonant_particle_match.group(1),
            't_spelling_add':t_spelling_add,
            'f_pronunciation_add':consonant_particle_match.group(2),
            'f_spelling_add':f_spelling_add
        }
    elif consonant_suffix_match:
        t_spelling_add = spell_word(consonant_suffix_match.group(1).strip(),sound_map_list)
        f_spelling_add = spell_word(consonant_suffix_match.group(2).strip(),sound_map_list)
        affix_type = 'suffix'
        map_entry = {
            'pronunciation_regex':IPA_CONSONANT_PATTERN+'$',
            'spelling_regex':SPELLING_CONSONANT_PATTERN+'$',
            't_pronunciation_add':consonant_suffix_match.group(1),
            't_spelling_add':t_spelling_add,
            'f_pronunciation_add':consonant_suffix_match.group(2),
            'f_spelling_add':f_spelling_add
        }
    elif prefix_match:
        affix_type = 'prefix'
        map_entry = {
            'pronunciation_add':prefix_match.group(1),
            'spelling_add':spell_word(prefix_match.group(1).strip(),sound_map_list),
        }
    elif particle_match:
        affix_type = 'particle'
        map_entry = {
            'pronunciation_add':particle_match.group(1),
            'spelling_add':spell_word(particle_match.group(1).strip(),sound_map_list),
        }
    elif suffix_match:
        affix_type = 'suffix'
        map_entry = {
            'pronunciation_add':suffix_match.group(1),
            'spelling_add':spell_word(suffix_match.group(1).strip(),sound_map_list),
        }
    elif re.match(r'V\(C\)\*#\s+>\s+__',affix_rule):
        affix_type = 'replacement'
        map_entry = {
            'pronunciation_regex':r'('+IPA_VOWELS_PATTERN+r')('+IPA_CONSONANT_PATTERN+r'+)(\S*)\s*$',
            'spelling_regex':r'('+SPELLING_VOWEL_PATTERN+r')('+SPELLING_CONSONANT_PATTERN+r'+)(\S*)\s*$',
            'pronunciation_replacement':'$1$1$2$3',
            'spelling_replacement':'$1$1$2$3'
        }
    elif affix_rule == '' or affix_rule.strip() == '-':
        affix_type = ''
        map_entry = {}
    else:
        print("Invalid affix rule: " + affix_rule)
        exit()

    return affix_type, map_entry

#end def parse_affix_rules

# Parse the customConsonants, customVowels, wordInitialConsonants, midWordConsonants, wordFinalConsonants, bwsVowels, and bws2ndVowels
# into the Conlang JSON structure phonetic_inventory structure.
def parse_phonetic_inventory(vulgarlang):
    ipa_symbol_map = get_ipa_symbol_map()

    p_consonants = []
    np_consonants = []
    vowels = []
    v_diphthongs = []
    
    for consonant in vulgarlang['customConsonants']['value'].split():
        if consonant in ipa_symbol_map['p_consonants']:
            p_consonants.append(consonant)
        elif consonant in ipa_symbol_map['np_consonants']:
            np_consonants.append(consonant)
        else:
            print("Warning consonant " + consonant + " is not pulmonic or non-plumonic, and is not getting inventoried")
            
    for vowel in vulgarlang['customVowels']['value'].split():
        if len(vowel) == 1:
            if vowel in ipa_symbol_map['vowels']:
                vowels.append(vowel)
            else:
                print("Warning vowel " + vowel + " is not in a valid IPA vowel, and is not getting inventoried");
        else:
            if (len(vowel) == 2) and ((vowel[1] == '\u02d0') or (vowel[1] == '\u02d1') or (vowel[1] == '\u02de') or (vowel[1] == '\u032f')):
                # A single vowel followed by a long, half-long, rhotacized, or short (Vulgarlang)
                if vowel[0] in ipa_symbol_map['vowels']:
                    vowels.append(vowel)
                else:
                    print("Warning vowel " + vowel + " is not in a valid IPA vowel, and is not getting inventoried");
            # For now, put everything else into v_diphthongs -- probably naive.
            else:
                v_diphthongs.append(vowel)
                
    phonetic_inventory = {
        'p_consonants':p_consonants,
        'np_consonants':np_consonants,
        'vowels':vowels,
        'v_diphthongs':v_diphthongs,
    }
    
    print(phonetic_inventory)
    
    return phonetic_inventory

#end parse_phonetic_inventory
        
# Call the main function.
if __name__ == "__main__":
   main(sys.argv[1:])
