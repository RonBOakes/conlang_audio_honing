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
# This file contains a collection of library files for use with the Conlang JSON
# structure in the Python language.
#
from lexicon_entry import LEXICON_ENTRY
import traceback
import re
import itertools
import functools
import pdb

# This function attempts to remove duplicate entries in a Conlang JSON object
# phonetic list.
def dedup_phonetic_list(phonetic_list):
    new_phonetic_map = {}
    
    for entry in phonetic_list:
        if entry[0] not in new_phonetic_map:
            new_phonetic_map[entry[0]] = []
            new_phonetic_map[entry[0]].append(entry)
        else:
            match = False
            for entry2 in new_phonetic_map[entry[0]]:
                decl_comp = functools.reduce(lambda x, y : x and y, map(lambda p, q: p == q,entry[1],entry2[1]), True)
                if decl_comp and entry[2] == entry2[2]:
                    match = True
                    break
            if not match:
                new_phonetic_map[entry[0]].append(entry)
   
    new_phonetic_list = []
    for entrykey in new_phonetic_map:
        new_phonetic_list += new_phonetic_map[entrykey]
        
    return new_phonetic_list

#end dedup_phonetic_list

# This function attempts to remove duplicate entries in a conlang JSON
# Object's Lexicon list.
def dedup_lexicon(lexicon):
    for ent in lexicon:
        if not isinstance(ent,LEXICON_ENTRY):
            print("ERROR: Entry is not a LEXICON_ENTRY: ",end="")
            print(ent)
            exit()

    lexicon_set = set(lexicon)
    
    return list(lexicon_set)

#end dedup_lexicon

# This function is part of the declension process, and is used to process 
# the affix_map_tuple generated during declining a word based on its
# part of speech.
def process_affix_map_tuple(affix_map_tuple,phonetic,part_of_speech,prior_declensions=[]):
    phonetic_list = []
    
    if len(affix_map_tuple) == 0:
        return phonetic_list
    
    affix_map = affix_map_tuple[0]
    # affix_map will have only one entry, the key is the affix type, the value is a list of rules.
    affix = list(affix_map.keys())[0]
    
    # If the affix is listed as a particle, then just return the empty phonetic list
    if affix == 'particle':
        return phonetic_list
        
    for entry in affix_map[affix]:
        # Strip emphisys marks off the beginning of phonetic strings.
        if phonetic[0:1] == 'ˈ':
            phonetic2 = phonetic[1:]
        else:
            phonetic2 = phonetic
            
        # At this point, the entry should be a dictionary with one key and a dictionary as its only value
        declension = list(entry.keys())[0]
        rules = entry[declension]
        
        # Perform the substitution if there is a regular expression in the affix rule.
        if 'pronunciation_regex' in rules.keys():
            if affix == 'prefix':
                if re.match(rules['pronunciation_regex'],phonetic):
                    new_word = rules['t_pronunciation_add'] + phonetic2
                else:
                    new_word = rules['f_pronunciation_add'] + phonetic2
            elif affix == 'suffix':
                if re.match(rules['pronunciation_regex'],phonetic):
                    new_word = phonetic2 + rules['t_pronunciation_add']
                else:
                    new_word = phonetic2 +rules['f_pronunciation_add']
            elif affix == 'replacement':
                replacement = rules['pronunciation_replacement'].replace('$','\\')
                match = re.match(rules['pronunciation_regex'],phonetic2)
                new_word = re.sub(rules['pronunciation_regex'],replacement,phonetic2)
        # If no regex, stick the new text on the correct end.
        elif 'pronunciation_add' in rules.keys():
            if affix == 'prefix':
                new_word = rules['pronunciation_add'] + phonetic2
            else:
                    new_word = phonetic2 +rules['pronunciation_add']
        # If we get here, we should be a particle which we punted out above, but put the new word in anyway.
        elif not bool(rules):
            new_word = phonetic
        
        # Recurse!
        next_map_tuple = affix_map_tuple[1:]
        phonetic_list += process_affix_map_tuple(next_map_tuple,new_word,part_of_speech,prior_declensions+[declension])
        phonetic_list.append([new_word,prior_declensions+[declension],part_of_speech,phonetic])

    return phonetic_list
#end def process_afix_map_list_tuple

# This function is part of the declension process, and is used to process 
# a single layer of the affix map list.
def process_affix_list_layer(affix_map_list,phonetic,part_of_speech):

    phonetic_list = []
    
    affix_map_combos = []
    for i in range(len(affix_map_list)):
        affix_map_combos += itertools.combinations(affix_map_list, i+1)
        
    for affix_map_tupple in affix_map_combos:
        phonetic_list += process_affix_map_tuple(affix_map_tupple,phonetic,part_of_speech)

    phonetic_list = dedup_phonetic_list(phonetic_list)
                
    return phonetic_list

#end def process_affix_list_layer

# Decline a word.
def decline_word(word,affix_map,sound_map_list,derived_word=False):

    phonetic_list = []
    
    # The process of declining a word is dependent on its format.  
    
    if isinstance(word,str):
        # Word strings are presumed to be in the Vulgarlang format. Split it accordingly and turn it into a LEXICON_ENTRY
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
        word_source_metatdata = LEXICON_ENTRY(phonetic=phonetic,spelled=spell_word(phonetic, sound_map_list),english=english_list[0],part_of_speech=part_of_speech,declension=[]).as_map()
    elif isinstance(word,dict):
        # Words as dictionaries are expected to have the parts below.  Extract these then turn it into a LEXICON_ENTRY
        phonetic = word['phonetic']
        part_of_speech = word['part_of_speech']
        english_list = [word['english']]
        word_source_metatdata = LEXICON_ENTRY(phonetic=phonetic,spelled=spell_word(phonetic, sound_map_list),english=english_list[0],part_of_speech=part_of_speech,declension=[]).as_map()
    elif isinstance(word,LEXICON_ENTRY):
        # Extract the needed parts from any LEXICON_ENTRY
        phonetic = word.phonetic
        part_of_speech = word.part_of_speech
        english_list = [word.english]
        derived_word = word.derived_word
        word_source_metatdata = word.as_map()
    else:
        print("ERROR invalid input to decline_word")
        print([word,type(word)])
        traceback.print_stack()
        exit()
        
    
    # Search the affix_map for a matching part of speech.  If one is found then
    # There are rules for declining this part of speech, so apply them to this word,
    # using its phonetic representation.
    if part_of_speech in affix_map.keys():
        affix_map_list = sorted(affix_map[part_of_speech],key=lambda x: list(x)[0])
        phonetic_list += process_affix_list_layer(affix_map_list,phonetic,part_of_speech)
        
    lexicon_fragment = []
        
    # build the pronunciation lexicon entries
    for phonetic_entry in phonetic_list:
        phonetic = phonetic_entry[0]
        declensions = phonetic_entry[1]
        part_of_speech = phonetic_entry[2]
        root = phonetic_entry[3]
        spelled = spell_word(phonetic, sound_map_list)
        for english in english_list:
            lexent = LEXICON_ENTRY(phonetic,spelled,english.strip(),part_of_speech,declensions,derived_word=derived_word,declined_word=True,metadata={'source':{'declined_word':word_source_metatdata}})
            lexicon_fragment.append(lexent)

    return lexicon_fragment

#end decline_word

# Derive words based on the Vulgarlang format still used by the Conlang JSON objects.
def derive_words(derived_word_list,derivational_affix_map,lexicon,affix_map,sound_map_list,decline=True):

    word_map = {}
    word_map_tupple = {}

    # Build the word map and word map tuple from the passed in lexicon.
    for raw_entry in lexicon:
        # Ensure that the entry is a LEXICON_ENTRY
        if isinstance(raw_entry,dict):
            entry = LEXICON_ENTRY(raw_entry['phonetic'],raw_entry['spelled'],raw_entry['english'],raw_entry['part_of_speech'],raw_entry['declensions'])
        elif isinstance(raw_entry,LEXICON_ENTRY):
            entry = raw_entry
        else:
            print("ERROR invalid input to derive_words")
            print([raw_entry,type(raw_entry)])
            traceback.print_stack()
            exit()
        
        # If the word is a root word, then put it in the word map.  
        # In order to match with words in the derived_word_list, spaces need to be
        # replaced with underscores.
        if 'root' in entry.declension:
            wm_english = entry.english.replace(' ','_')
            word_map[wm_english] = entry
            part_of_speech = entry.part_of_speech
            if part_of_speech.startswith('n'):
                part_of_speech = 'n'
            word_map_tupple[(wm_english,part_of_speech)] = entry
    
    lexicon_fragment = []
    
    # Iterate over the derived_word_list.
    for words in derived_word_list:
        # Partition the word into parts based on the Vulgarlang format.
        parts1 = words.split("=")
        parts2 = parts1[0].split(":")
        english = parts2[0].strip()
        part_of_speech = parts2[1].strip()
        rule_text = parts1[1].strip()
        rule_text = re.sub(r'([a-z]+)-([a-z]+)',r'\1 \2',rule_text)
        rule_list = rule_text.split()
        
        
        phonetic = ""
        
        for rule in rule_list:
            rule_affix_data={}

            # Look for derivation affixes and add them to the rule_affix_data if present.
            if '-' in rule:
                rule_split = rule.split('-')
                rule_affix_data = derivational_affix_map[rule_split[1].strip()]
                rule = rule_split[0].strip()

            rule_part_of_speech = ''
            if ':' in rule:
                rule_split = rule.split(':')
                rule_part_of_speech = rule_split[1].strip()
                rule = rule_split[0].strip()
                # Turn all gendered nouns into just 'n'
                if rule_part_of_speech != 'num':
                    rule_part_of_speech = re.sub('\s*n\w*\s*','n',rule_part_of_speech)

            # look for the root word
            word = rule
            # If we are looking to match a specific part of speech
            if rule_part_of_speech:
                if (word, rule_part_of_speech) not in word_map_tupple:
                    # Search for matching entries that start and use the first one.
                    found = False
                    for pair in word_map_tupple.keys():
                        if pair[0].startswith(word) and pair[1] == rule_part_of_speech:
                            lex_entry = word_map_tupple[pair]
                            found = True
                            break
                    if not found:
                        print("ERROR: unable to locate " + word +" with part of speech " + rule_part_of_speech)
                        exit()
                else:
                    lex_entry = word_map_tupple[(word, rule_part_of_speech)]
            else:
                if word not in word_map:
                    # Search for matching entries that start and use the first one.
                    found = False
                    for search_word in word_map.keys():
                        if search_word.startswith(word):
                            lex_entry = word_map[search_word]
                            found = True
                            break
                    if not found:
                        print("ERROR: unable to locate " + word)
                        exit()
                else:
                    lex_entry = word_map[word]
                
            # Build the derived word's phonetic representation.
            phonetic_part = lex_entry.phonetic
            if rule_affix_data:
                if 'pronunciation_regex' in rule_affix_data.keys():
                    if re.match(rule_affix_data['pronunciation_regex'],phonetic_part):
                        if rule_affix_data['type'] == 'PREFIX':
                            phonetic_part = rule_affix_data['t_pronunciation_add'] + phonetic_part
                        else:
                            phonetic_part = phonetic_part + rule_affix_data['t_pronunciation_add']
                    else:
                        if rule_affix_data['type'] == 'PREFIX':
                            phonetic_part = rule_affix_data['f_pronunciation_add'] + phonetic_part
                        else:
                            phonetic_part = phonetic_part + rule_affix_data['f_pronunciation_add']
                elif 'pronunciation_add' in rule_affix_data.keys():
                    if rule_affix_data['type'] == 'PREFIX':
                        phonetic_part = rule_affix_data['pronunciation_add'] + phonetic_part
                    else:
                        phonetic_part = phonetic_part + rule_affix_data['pronunciation_add']
                    

            phonetic += phonetic_part

        # Build all of the LEXICON_ENTRYs for this word - each English word or defination gets its own entry.
        for eng in english.split(','):
            eng = eng.strip()
            entry = LEXICON_ENTRY(phonetic,spell_word(phonetic,sound_map_list),eng, part_of_speech, ['root'],derived_word=True,declined_word=False,metadata={'source':{'derrived_word':words}})
            wm_english = eng.replace(' ','_')
            word_map[wm_english] = entry
            part_of_speech = entry.part_of_speech
            if part_of_speech.startswith('n'):
                part_of_speech = 'n'
            word_map_tupple[(wm_english,part_of_speech)] = entry
            word_lexicon_fragment = [entry]
            new_word_line = eng + " : " + part_of_speech +" =" + phonetic
            if decline:
                word_lexicon_fragment = decline_word(new_word_line,affix_map,sound_map_list,derived_word=True)
            lexicon_fragment += word_lexicon_fragment
                
    return lexicon_fragment

#end def derive_words

# Convert a word from phonetic representation into romanized representation.
def spell_word(phonetic, sound_map_list):
    spelled = phonetic

    for sound_map in sound_map_list:
        if 'romanization' in sound_map:
            # Change the regular expression replace/substitute from PERL to Python.
            romanization = sound_map['romanization'].replace('$','\\')
            spelled = re.sub(sound_map['spelling_regex'],romanization,spelled)
       
    return spelled.strip()
#end def spell_word

# Quick utility function to get the English number word short form.
def get_number_word(num):
    num = num.strip()
    if int(num) < 0 or int(num) > 9:
        return num
    elif num == '1':
        return "1st"
    elif num == '2':
        return "2nd"
    elif num == '3':
        return "3rd"
    else:
        return num + "th"
    
#end get_number_word

# Returns the map containing the lists of IPA symbols broken down by category
# This is based on the symbols used by Vulgarlang, with some additions.
def get_ipa_symbol_map():
    ipa_symbol_map = {}
    ipa_symbol_map['p_consonants'] = "b \u03b2 \u0299 c \u00e7 d \u0256 \u1d91 \u02a3 \u02a5 \u02a4 \uab66 f \u0278 g \u0262 \u0270 h \u0266 \u0127 \u0267 j \u029d \u025f k l \u026b \u026c \u026e \ud837\udf05 \u026d \ua78e \u029f \ud837\udf04 m \u0271 n \u0273 \u0272 \u014b \u0274 p q r \u0279 \u027e \u027d \u027b \u027a \ud837\udf08 \u0281 \u0280 s \u0282 \u0283 t \u0288 \u02a6 \u02a8 \u02a7 \uab67 v \u2c71 \u028b x \u0263 \u03c7 \u028e \ud837\udf06 z \u0290 \u0292 \u03b8 \u00f0 \u0294 \u0295 R".split()
    ipa_symbol_map['np_consonants'] = "\u0253 \u0257 \u0284 \u0260 \u029b w \u028d \u0265 \u02a1 \u02a2 \u0255 \u0291 \u029c \u0298 \u01c0 \u01c3 \u01c2 \u01c1 \ud837\udf0a".split()
    ipa_symbol_map['vowels'] = "a \u00e6 \u0251 \u0252 \u0250 e \u025b \u025c \u025e \u0259 i \u0268 \u026a y \u028f \u00f8 \u0258 \u0275 \u0153 \u0276 \u0264 o \u0254 u \u0289 \u028a \u026f \u028c \u025a \u02de".split()
    ipa_symbol_map['suprasegmentals'] = "\u02d0 \u02d1 \u02c8 \02cc \u035c \u0361".split()
    ipa_symbol_map['diacritics'] = "\u02f3 \u0325 \u030a \u0324 \u032a \u02cc \u0329 \u0c3c \u032c \u02f7 \u0330 \u02f7 \u0330 \u02fd \u033a \u032f \u02b0 \u033c \u033b \u02d2 \u0339 \u20b7 \u0303 \u02b2 \u02d3 \u031c \u02d6 \u031f \u207f \u00a8 \u0308 \u02e0 \u02cd \u0320 \u20e1 \u02df \u033d \u02e4 \uab68 \u0319 \u02de".split()
    return ipa_symbol_map

#end get_ipa_symbol_map