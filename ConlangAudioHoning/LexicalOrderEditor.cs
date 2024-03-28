using ConlangJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConlangAudioHoning
{
    public partial class LexicalOrderEditor : Form
    {
        /// <summary>
        /// Language having its lexical order edited.
        /// </summary>
        public LanguageDescription Language
        {
            get; set;
        }

        private readonly List<string> charList;

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
            this.Close();
        }

        private void CloseWithoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
            if(lbxCharacters.SelectedIndex < (charList.Count - 1))
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
