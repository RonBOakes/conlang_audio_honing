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
# Definition of the LEXICON_ENTRY used throughout the Python code for working with
# the Conlang JSON object.
#
import pdb

# LEXICON_ENTRY Class
class LEXICON_ENTRY:
    lexical_order_list = ['a b c d e f g h i j k l m n o p q r s t u v w k y z'.split()]
    def __init__(self, phonetic, spelled, english='', part_of_speech='', declension=[], derived_word=False, declined_word=False, metadata={}):
        self.phonetic = phonetic
        self.spelled = spelled
        self.english = english
        self.part_of_speech = part_of_speech
        if isinstance(declension,list):
            self.declension = declension
        elif isinstance(declension,str):
            self.declension = [declension]
        else:
            self.declension = [str(declension)]
        self.derived_word = derived_word
        self.declined_word = declined_word
        self.metadata = metadata
        
    def __lt__(self, obj):
        return (LEXICON_ENTRY.lexical_index(self.spelled) < LEXICON_ENTRY.lexical_index(obj.spelled))
        
    def __gt__(self, obj):
        return (LEXICON_ENTRY.lexical_index(self.spelled) > LEXICON_ENTRY.lexical_index(obj.spelled))
        
    def __eq__(self, obj):
        if(self.spelled != obj.spelled):
            return False
        elif (self.phonetic == obj.phonetic) and (self.english == obj.english) and (self.part_of_speech == obj.part_of_speech) and (''.join(self.declension) == ''.join(obj.declension)):
            return True
        else:
            return False
        
    def __le__(self, obj):
        return (self < obj) or (self == obj)
        
    def __ge__(self, obj):
        return (self > obj) or (self == obj)
        
    def __hash__(self):
        return hash((self.phonetic, self.english, self.part_of_speech, ''.join(self.declension)))
        
    def __repr__(self):
        return str(self.as_map())
        
    def as_map(self):
        my_map = {
                    'phonetic':self.phonetic.strip(),
                    'spelled':self.spelled.strip(),
                    'english':self.english.strip(),
                    'part_of_speech':self.part_of_speech.strip(),
                    'declensions':self.declension,
                    'derived_word':self.derived_word,
                    'declined_word':self.declined_word,
                    'metadata':self.metadata,
                 }
        return my_map
    
    @staticmethod
    def set_lexical_order_list(in_lexical_order_list):
        LEXICON_ENTRY.lexical_order_list = in_lexical_order_list
    
    @staticmethod
    def lexical_index(in_item):
        item = in_item.lower()
        lexical_inx = 0
        char_pos = 0
        while char_pos < len(item):
            char_inx = -char_pos
            if char_pos < len(item) - 2:
                nextchar = item[char_pos+1:char_pos+2]
                charint = int(nextchar.encode('utf-16-be').hex(),base=16) & 0x0000ffff
                if (charint >= 0x0300) and (charint <= 0x036f):
                    char = item[char_pos:char_pos+2]
                    char_pos += 2
                else:
                    char = item[char_pos:char_pos+1]
                    char_pos += 1
            else:
                char = item[char_pos:char_pos+1]
                char_pos += 1
                
            if char != 'ˈ' and char != ' ':
                lexical_inx += LEXICON_ENTRY.lexical_value(char) * (100 ** char_inx)
        return lexical_inx
    #end def lexical_index

    @staticmethod
    def lexical_value(char):
        char_base = char[0:1]
        
        if char_base in LEXICON_ENTRY.lexical_order_list:
            lexval = LEXICON_ENTRY.lexical_order_list.index(char_base) * 100
            if len(char) > 1:
                diacritic = char[1:]
                lexval += round((float(int(diacritic.encode('utf-16-be').hex(),base=16) & 0x0000ffff) - 768.0))
        else:
            lexval = len(LEXICON_ENTRY.lexical_order_list) + 1
            
        return lexval

# End of LEXICON_ENTRY