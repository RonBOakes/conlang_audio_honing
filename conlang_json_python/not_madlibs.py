#!/usr/bin/python3 
import sys
import getopt
import json
import pdb
import re
import random
from argparse import ArgumentParser
sys.path.insert(0, '../speak_general')
from lexicon_entry import LEXICON_ENTRY
from conlang_lib import spell_word, decline_word, derive_words

def main(argv):
    # Define and parse the command line arguments
    cli = ArgumentParser(description="Generate Nonsense Sentences")
    cli.add_argument("-l","--languagefile", type=str, required=True, metavar="FILE_PATH", dest="language_file")
    cli.add_argument("-o","--output", type=str, required=True, metavar="FILE_PATH", dest="output")
    cli.add_argument("-c","--count", type=int, required=False, dest="count")
    arguments = cli.parse_args()
    
    language_file = arguments.language_file
        
    if not arguments.count:
        count = 5
    else:
        count = arguments.count
    
    output_file = arguments.output
    
    # Read the JSON language data
    with open(language_file,"r", encoding="utf-8-sig") as ifp:
        language_structure = json.load(ifp)
        
    lexicon = language_structure["lexicon"]
    add_lexicon = []
    for word in lexicon:
        add_lexicon += decline_word(word,language_structure['affix_map'],language_structure['sound_map_list'])
    for lex_entry in add_lexicon:
        lexicon.append(lex_entry.as_map())
    language_structure['lexicon'] = lexicon
    
    language_map = build_parts_of_speech(language_structure)
    
    with open(output_file,"wt", encoding="utf-8-sig") as ofp:
        for i in range(count):
            sentence = generate_sentence(language_map)
            ofp.write(sentence)
            ofp.write(" ")
            print(sentence)

#end def main

def build_parts_of_speech(language_structure):
    nouns = {}
    verbs = {}

    for lexicon_entry in language_structure['lexicon']:
        if lexicon_entry['part_of_speech'] == 'v':
            if lexicon_entry['english'] not in verbs.keys():
                verbs[lexicon_entry['english']] = []
            verbs[lexicon_entry['english']].append(lexicon_entry)
        elif (lexicon_entry['part_of_speech'].startswith('n')) and (lexicon_entry['part_of_speech'] != 'num'):
            if lexicon_entry['english'] not in nouns.keys():
                nouns[lexicon_entry['english']] = []
            nouns[lexicon_entry['english']].append(lexicon_entry)
            
    return {'nouns':nouns, 'verbs':verbs}

#end build_parts_of_speech


def generate_sentence(language_map):
# Main word order: Subject Verb Object (Prepositional phrase). “Mary opened the door with a key” turns into Mary opened the door with a key.
# Adjective order: Adjectives are positioned before the noun.
# Adposition: prepositions

    sentence_subject_key = random.choices(list(language_map['nouns'].keys()))[0]
    sentence_subject = language_map['nouns'][sentence_subject_key]
    sentence_verb_key = random.choices(list(language_map['verbs'].keys()))[0]
    sentence_verb = language_map['verbs'][sentence_verb_key]
    sentence_object_key = random.choices(list(language_map['nouns'].keys()))[0]
    sentence_object = language_map['nouns'][sentence_object_key]

    definateness_choices = ['Definate','Indefinate']
    count_choices = ['Singular','Plural','Paucal']
    tense_choices = ['Present','Past','Remote past','Future','Remote Future']
    mood_choices = ['Indicative','Conditional','Subjunctive','Imperative']
    
    sentence = ""

    definateness = random.choices(definateness_choices)[0]
    count = random.choices(count_choices)[0]
    subject_word = ''
    for noun in sentence_subject:
        if 'Nominative' in noun['declensions']:
            if definateness in noun['declensions']:
                if count in noun['declensions']:
                    subject_word = noun['spelled'].strip()
                    break
            elif subject_word == '':
                subject_word = noun['spelled'].strip()
        elif subject_word == '':
            subject_word = noun['spelled'].strip()
    sentence += subject_word + ' '

    tense = random.choices(tense_choices)[0]
    mood = random.choices(mood_choices)[0]
    for verb in sentence_verb:
        if tense in verb['declensions'] and mood in verb['declensions']:
            sentence += verb['spelled'].strip() + ' '
            break

    definateness = random.choices(definateness_choices)[0]
    count = random.choices(count_choices)[0]
    object_word = ''
    for noun in sentence_object:
        if 'Accusative' in noun['declensions']:
            if definateness in noun['declensions']:
                if count in noun['declensions']:
                    object_word = noun['spelled'].strip()
                    break
            elif object_word == '':
                object_word = noun['spelled'].strip()
        elif object_word == '':
            object_word = noun['spelled'].strip()
    sentence += object_word + '.'
    return sentence

#end def generate_sentence

if __name__ == "__main__":
   main(sys.argv[1:])
