using System.Text.RegularExpressions;
/*
 * Lexicon Editor form
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
    /// <summary>
    /// Panel/Dialog to allow editing of the entire Lexicon.
    /// </summary>
    public partial class LexiconEditor : Panel
    {
        /// <summary>
        /// Which criteria is used to sort the lexicon for display.
        /// </summary>
        public enum SortByCriteriaTypes
        {
            /// <summary>
            /// Sort by the Conlang's lexical order
            /// </summary>
            CONLANG,
            /// <summary>
            /// Sort by the English (or other native language) order.
            /// </summary>
            ENGLISH
        };

        private SortedSet<LexiconEntry> lexiconEntries;

        private readonly Dictionary<(string, string, string), LexiconEntry> lexiconMap;

        /// <summary>
        /// Selects the sorting criteria
        /// </summary>
        public SortByCriteriaTypes SortByCriteria
        {
            get; set;
        }

        /// <summary>
        /// The lexicon being edited in this form.
        /// </summary>
        public SortedSet<LexiconEntry> Lexicon
        {
            get
            {
                SortedSet<LexiconEntry> lexicon = [];
                foreach (LexiconEntry entry in lexiconEntries)
                {
                    lexicon.Add(entry);
                }

                return lexicon;
            }
            set
            {
                lexiconEntries = new SortedSet<LexiconEntry>(SortByCriteria == SortByCriteriaTypes.CONLANG ? new LexiconEntry.LexicalOrderCompSpelling() :
                    new LexiconEntry.LexicalOrderCompEnglish());
                foreach (LexiconEntry entry in value)
                {
                    lexiconEntries.Add(entry);
                }
                lbxLexicon.BeginUpdate();
                lbxLexicon.Items.Clear();
                lexiconMap.Clear();
                foreach (LexiconEntry entry in lexiconEntries)
                {
                    if (SortByCriteria == SortByCriteriaTypes.CONLANG)
                    {
                        lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                    }
                    else
                    {
                        lbxLexicon.Items.Add(string.Format("{1} ({0}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                    }
                    _ = lexiconMap.TryAdd((entry.spelled, entry.english, entry.part_of_speech), entry);
                }
                lbxLexicon.EndUpdate();
            }
        }

        /// <summary>
        /// This is set to true if the user has requested saving the edited lexicon.
        /// </summary>
        public bool Saved
        { get; private set; } = false;

        /// <summary>
        /// Holds the list of valid parts of speech for passing to the LexiconEntryEditor.
        /// </summary>
        public List<string> PartOfSpeechList
        { get; set; }

        /// <summary>
        /// Holds the soundMapList for passing to the LexiconEntryEditor.
        /// </summary>
        public List<SpellingPronunciationRules> SoundMapList
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
            get => closeWithoutSavingToolStripMenuItem;
        }

        /// <summary>
        /// Construct the Lexicon Editor
        /// </summary>
        /// <param name="lexicon">Lexicon to be edited (empty by default)</param>
        /// <param name="partOfSpeech">Part of Speech list (empty by default)</param>
        /// <param name="soundMapList">Sound map list (empty by default)</param>
        public LexiconEditor(SortedSet<LexiconEntry>? lexicon = null, List<string>? partOfSpeech = null, List<SpellingPronunciationRules>? soundMapList = null)
        {
            SortByCriteria = SortByCriteriaTypes.CONLANG;

            if (lexicon != null)
            {
                lexiconEntries = lexicon;
            }
            else
            {
                lexiconEntries = [];
            }

            if (partOfSpeech != null)
            {
                PartOfSpeechList = partOfSpeech;
            }
            else
            {
                PartOfSpeechList = [];
            }

            if (soundMapList != null)
            {
                SoundMapList = soundMapList;
            }
            else
            {
                SoundMapList = [];
            }

            Saved = false;

            lexiconMap = [];

            InitializeComponent();

            lbxLexicon.BeginUpdate();
            lbxLexicon.Items.Clear();
            lexiconMap.Clear();
            foreach (LexiconEntry entry in lexiconEntries)
            {
                lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                lexiconMap.TryAdd((entry.spelled, entry.english, entry.part_of_speech), entry);
            }
            lbxLexicon.EndUpdate();

            this.SizeChanged += LexiconEditor_SizeChanged;
        }

        private void LexiconEditor_SizeChanged(object? sender, EventArgs e)
        {
            this.SuspendLayout();
            ClientSize = this.Size;
            lbxLexicon.Size = new Size(this.Width - 20, this.Height - 20);
            ReSort();
            this.ResumeLayout();
        }

        private void ReSort()
        {
            SortedSet<LexiconEntry> oldLexiconEntries = lexiconEntries;
            lexiconEntries = new SortedSet<LexiconEntry>(SortByCriteria == SortByCriteriaTypes.CONLANG ? new LexiconEntry.LexicalOrderCompSpelling() :
    new LexiconEntry.LexicalOrderCompEnglish());
            foreach (LexiconEntry entry in oldLexiconEntries)
            {
                lexiconEntries.Add(entry);
            }
            lbxLexicon.BeginUpdate();
            lbxLexicon.Items.Clear();
            lexiconMap.Clear();
            foreach (LexiconEntry entry in lexiconEntries)
            {
                if (SortByCriteria == SortByCriteriaTypes.CONLANG)
                {
                    lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                }
                else
                {
                    lbxLexicon.Items.Add(string.Format("{1} ({0}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                }
                lexiconMap.Add((entry.spelled, entry.english, entry.part_of_speech), entry);
            }
            lbxLexicon.EndUpdate();

        }
        private void SaveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Saved = true;
        }

        private void CloseWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Saved = false;
        }


        private void LbxLexicon_DoubleClick(object sender, EventArgs e)
        {
            if (lbxLexicon.SelectedItem != null)
            {
                string? selectedWord = lbxLexicon.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(selectedWord))
                {
                    Match wordMatch = EditorEntryRegex().Match(selectedWord);
                    if (wordMatch.Success)
                    {
                        string conlang = (SortByCriteria == SortByCriteriaTypes.CONLANG ? wordMatch.Groups[1].Value : wordMatch.Groups[2].Value);
                        string english = (SortByCriteria == SortByCriteriaTypes.CONLANG ? wordMatch.Groups[2].Value : wordMatch.Groups[1].Value);
                        string partOfSpeech = wordMatch.Groups[3].Value;

                        LexiconEntry word = lexiconMap[(conlang, english, partOfSpeech)];

                        LexiconEntryEditor editor = new(word, PartOfSpeechList, SoundMapList);
                        _ = editor.ShowDialog();
                        if (editor.LexiconEntrySaved)
                        {
                            lexiconEntries.Remove(word);
                            lexiconMap.Remove((conlang, english, partOfSpeech));
                            word = editor.LexiconEntry;
                            lexiconEntries.Add(word);
                            lexiconMap.TryAdd((word.spelled, word.english, word.part_of_speech), word);
                            lbxLexicon.BeginUpdate();
                            lbxLexicon.Items.Clear();
                            foreach (LexiconEntry entry in lexiconEntries)
                            {
                                if (SortByCriteria == SortByCriteriaTypes.CONLANG)
                                {
                                    lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                                }
                                else
                                {
                                    lbxLexicon.Items.Add(string.Format("{1} ({0}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                                }
                            }
                            lbxLexicon.EndUpdate();
                        }
                    }
                }
            }
        }

        [GeneratedRegex(@"^\s*((?:\S+\(?.*?\)?)|\<.+\>)\s+\((\S+|\<.+\>):\s+(\S+)\)\s*$")]
        private static partial Regex EditorEntryRegex();

        private void AddEntryMenuItem_Click(object sender, EventArgs e)
        {
            LexiconEntry word = new();
            LexiconEntryEditor editor = new(word, PartOfSpeechList, SoundMapList);
            _ = editor.ShowDialog();
            if (editor.LexiconEntrySaved)
            {
                word = editor.LexiconEntry;
                lexiconEntries.Add(word);
                lexiconMap.TryAdd((word.spelled, word.english, word.part_of_speech), word);
                lbxLexicon.BeginUpdate();
                lbxLexicon.Items.Clear();
                foreach (LexiconEntry entry in lexiconEntries)
                {
                    if (SortByCriteria == SortByCriteriaTypes.CONLANG)
                    {
                        lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                    }
                    else
                    {
                        lbxLexicon.Items.Add(string.Format("{1} ({0}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                    }
                }
                lbxLexicon.EndUpdate();
            }
        }

        private void ConlangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortByCriteria = SortByCriteriaTypes.CONLANG;
            ReSort();
        }

        private void EnglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortByCriteria = SortByCriteriaTypes.ENGLISH;
            ReSort();
        }

        private void FindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDialog findDialog = new(this);
            findDialog.Show(this);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbxLexicon.SelectedItem == null)
            {
                return;
            }
            string? selectedWord = lbxLexicon.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(selectedWord))
            {
                Match wordMatch = EditorEntryRegex().Match(selectedWord);
                if (wordMatch.Success)
                {
                    string conlang = (SortByCriteria == SortByCriteriaTypes.CONLANG ? wordMatch.Groups[1].Value : wordMatch.Groups[2].Value);
                    string english = (SortByCriteria == SortByCriteriaTypes.CONLANG ? wordMatch.Groups[2].Value : wordMatch.Groups[1].Value);
                    string partOfSpeech = wordMatch.Groups[3].Value;

                    LexiconEntry word = lexiconMap[(conlang, english, partOfSpeech)];
                    lexiconEntries.Remove(word);
                    lbxLexicon.BeginUpdate();
                    lbxLexicon.Items.Clear();
                    foreach (LexiconEntry entry in lexiconEntries)
                    {
                        if (SortByCriteria == SortByCriteriaTypes.CONLANG)
                        {
                            lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                        }
                        else
                        {
                            lbxLexicon.Items.Add(string.Format("{1} ({0}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                        }
                    }
                    lbxLexicon.EndUpdate();
                }
            }
        }

        private sealed class FindDialog : Form
        {
            private readonly TextBox tbxFindString;
            private readonly LexiconEditor parent;

            public FindDialog(LexiconEditor parent)
            {
                this.parent = parent;
                this.Text = "Find Word";
                this.Size = new Size(220, 75);
                tbxFindString = new TextBox
                {
                    Location = new Point(5, 5),
                    Size = new Size(200, 15)
                };
                this.Controls.Add(tbxFindString);
                tbxFindString.TextChanged += TbxFindString_TextChanged;
                tbxFindString.LostFocus += FindDialog_LostFocusEtc;
                this.LostFocus += FindDialog_LostFocusEtc;
                this.Leave += FindDialog_LostFocusEtc;
            }

            private void FindDialog_LostFocusEtc(object? sender, EventArgs e)
            {
                string findText = tbxFindString.Text;
                parent.lbxLexicon.FindString(findText);
                this.Close();
            }

            private void TbxFindString_TextChanged(object? sender, EventArgs e)
            {
                string findText = tbxFindString.Text;
                int locIndex = parent.lbxLexicon.FindString(findText);
                parent.lbxLexicon.SelectedIndex = locIndex;
            }
        }

    }
}
