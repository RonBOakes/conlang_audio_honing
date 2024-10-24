﻿using System.Text;
/*
 * Lexical Order Editor Form
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

namespace ConlangAudioHoning
{
    public partial class LexicalOrderEditor : Panel
    {
        /// <summary>
        /// Language having its lexical order edited.
        /// </summary>
        public LanguageDescription Language
        {
            get; set;
        }

        /// <summary>
        /// Exports the SaveAndClose menu for access by parent forms.
        /// </summary>
        public ToolStripMenuItem SaveAndCloseToolStripMenuItem
        {
            get => saveAndCloseToolStripMenuItem;
        }

        /// <summary>
        /// Exports the close without saving menu for access by parent forms.
        /// </summary>
        public ToolStripMenuItem CloseWithoutSavingToolStripMenuItem
        {
            get => closeWithoutToolStripMenuItem;
        }

        /// <summary>
        /// This is set to true if the user has requested saving the edited lexicon.
        /// </summary>
        public bool Saved
        { get; private set; } = false;

        private List<string> charList;

        /// <summary>
        /// Construct a Lexical Order Editor
        /// </summary>
        /// <param name="language">Language having its lexical order edited</param>
        /// <param name="regenerate">Set to true to get a fresh set of characters from the language</param>
        public LexicalOrderEditor(LanguageDescription language, bool regenerate = false)
        {
            Language = language;
            InitializeComponent();

            if (regenerate)
            {
                charList = RegenerateLexicalOrder(Language);
            }
            else
            {
                charList = Language.lexical_order_list.GetRange(0, Language.lexical_order_list.Count);
            }

            lbxCharacters.BeginUpdate();
            lbxCharacters.Items.Clear();
            foreach (string str in charList)
            {
                lbxCharacters.Items.Add(str);
            }
            lbxCharacters.EndUpdate();
        }

        /// <summary>
        /// Regenerate the characters, usually due to an update of the loaded language.
        /// </summary>
        public void RegenerateLexicalOrder()
        {
            charList = RegenerateLexicalOrder(Language);
            lbxCharacters.BeginUpdate();
            lbxCharacters.Items.Clear();
            foreach (string str in charList)
            {
                lbxCharacters.Items.Add(str);
            }
            lbxCharacters.EndUpdate();
        }

        private static List<string> RegenerateLexicalOrder(LanguageDescription language)
        {
            SortedSet<string> chars = [];
            foreach (LexiconEntry word in language.lexicon)
            {
                StringBuilder currentChar = new();
                foreach (char letter in word.spelled)
                {
                    if (LatinUtilities.DiacriticsMap.ContainsKey(letter.ToString()))
                    {
                        currentChar.Append(letter);
                    }
                    else
                    {
                        if (currentChar.Length > 0)
                        {
                            chars.Add(currentChar.ToString());
                        }
                        currentChar.Clear();
                        currentChar.Append(letter);
                    }
                }
            }

            return [.. chars];
        }

        private void SaveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Language.lexical_order_list = charList;
            LexiconEntry.LexicalOrderList = charList;
            Language.RefreshLexiconOrder();
            Saved = true;
        }

        private void CloseWithoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Saved = false;
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            if (lbxCharacters.SelectedIndex > 0)
            {
                int index = lbxCharacters.SelectedIndex;
                (charList[index - 1], charList[index]) = (charList[index], charList[index - 1]);
                lbxCharacters.BeginUpdate();
                lbxCharacters.Items.Clear();
                foreach (string str in charList)
                {
                    lbxCharacters.Items.Add(str);
                }
                lbxCharacters.EndUpdate();
                lbxCharacters.SelectedIndex = index - 1;
            }
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            if (lbxCharacters.SelectedIndex < (charList.Count - 1))
            {
                int index = lbxCharacters.SelectedIndex;
                (charList[index], charList[index + 1]) = (charList[index + 1], charList[index]);
                lbxCharacters.BeginUpdate();
                lbxCharacters.Items.Clear();
                foreach (string str in charList)
                {
                    lbxCharacters.Items.Add(str);
                }
                lbxCharacters.EndUpdate();
                lbxCharacters.SelectedIndex = index + 1;
            }
        }
    }
}
