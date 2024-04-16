using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Form version of the Lexicon Editor.  This wraps around the LexiconEditor to allow for its use as an independent form.
    /// </summary>
    public class LexiconEditorForm : Form
    {
        private readonly LexiconEditor editor;

        /// <summary>
        /// Constructs a LexiconEditorForm.
        /// </summary>
        /// <param name="lexicon">Lexicon to be edited (empty by default)</param>
        /// <param name="partOfSpeech">Part of Speech list (empty by default)</param>
        /// <param name="soundMapList">Sound map list (empty by default)</param>
        public LexiconEditorForm(SortedSet<LexiconEntry>? lexicon = null, List<string>? partOfSpeech = null, List<SoundMap>? soundMapList = null)
        {
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 470);

            editor = new LexiconEditor(lexicon, partOfSpeech,soundMapList);

            editor.SaveAndCloseToolStripMenuItem.Click += SaveAndCloseToolStripMenuItem_Click;
            editor.CloseWithoutSavingToolStripMenuItem.Click += CloseWithoutSavingToolStripMenuItem_Click;

            Controls.Add(editor);
        }

        private void CloseWithoutSavingToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveAndCloseToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Indicates that the subordinate LexiconEditor has saved its changes.
        /// </summary>
        public bool Saved
        {
            get => editor.Saved;
        }

        /// <summary>
        /// Lexicon being edited.
        /// </summary>
        public SortedSet<LexiconEntry> Lexicon
        {
            get => editor.Lexicon;
            set => editor.Lexicon = value;
        }
    }
}
