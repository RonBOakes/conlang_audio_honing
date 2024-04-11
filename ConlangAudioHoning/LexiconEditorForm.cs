using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    public class LexiconEditorForm : Form
    {
        private LexiconEditor editor;
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

        public bool Saved
        {
            get => editor.Saved;
        }

        public SortedSet<LexiconEntry> Lexicon
        {
            get => editor.Lexicon;
            set => editor.Lexicon = value;
        }
    }
}
