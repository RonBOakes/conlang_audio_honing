using ConlangJson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Form/Dialog to allow editing of the entire Lexicon.
    /// </summary>
    public partial class LexiconEditor : Form
    {
        private List<LexiconEntry> lexiconEntries;

        private readonly Dictionary<(string,string,string), LexiconEntry> lexiconMap;

        /// <summary>
        /// The lexicon being edited in this form.
        /// </summary>
        public List<LexiconEntry> Lexicon
        {
            get => lexiconEntries;
            set
            {
                lexiconEntries = value;
                lbxLexicon.BeginUpdate();
                lbxLexicon.Items.Clear();
                lexiconMap.Clear();
                foreach (LexiconEntry entry in lexiconEntries)
                {
                    lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                    lexiconMap.Add((entry.spelled, entry.english, entry.part_of_speech), entry);
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
        public List<SoundMap> SoundMapList
        {
            get; set;
        }

        /// <summary>
        /// Construct the Lexicon Editor
        /// </summary>
        /// <param name="lexicon">Lexicon to be edited (empty by default)</param>
        /// <param name="partOfSpeech">Part of Speech list (empty by default)</param>
        /// <param name="soundMapList">Sound map list (empty by default)</param>
        public LexiconEditor(List<LexiconEntry>? lexicon = null, List<string>? partOfSpeech = null, List<SoundMap>? soundMapList = null)
        {
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
        }

        private void SaveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Saved = true;
            this.Close();
        }

        private void CloseWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Saved = false;
            this.Close();
        }

        private void LbxLexicon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxLexicon.SelectedItem != null)
            {
                string? selectedWord = lbxLexicon.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(selectedWord))
                {
                    Match wordMatch = EditorEntryRegex().Match(selectedWord);
                    if (wordMatch.Success)
                    {
                        string conlang = wordMatch.Groups[1].Value;
                        string english = wordMatch.Groups[2].Value;
                        string partOfSpeech = wordMatch.Groups[3].Value;

                        LexiconEntry word = lexiconMap[(conlang, english, partOfSpeech)];
                        
                        LexiconEntryEditor editor = new(word,PartOfSpeechList,SoundMapList);
                        _ = editor.ShowDialog();
                        if(editor.LexiconEntrySaved)
                        {
                            lexiconEntries.Remove(word);
                            lexiconMap.Remove((conlang, english, partOfSpeech));
                            word = editor.LexiconEntry;
                            lexiconEntries.Add(word);
                            lexiconMap.TryAdd((word.spelled, word.english, word.part_of_speech),word);
                            lbxLexicon.BeginUpdate();
                            lbxLexicon.Items.Clear();
                            foreach (LexiconEntry entry in lexiconEntries)
                            {
                                lbxLexicon.Items.Add(string.Format("{0} ({1}: {2})", entry.spelled, entry.english, entry.part_of_speech));
                            }
                            lbxLexicon.EndUpdate();
                        }
                    }
                }
            }
        }

        [GeneratedRegex(@"^\s*(\S+)\s+\((\S+):\s+(\S+)\)\s*$")]
        private static partial Regex EditorEntryRegex();
    }
}
