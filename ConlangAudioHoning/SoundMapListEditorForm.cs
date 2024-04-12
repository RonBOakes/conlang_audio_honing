using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace ConlangAudioHoning
{
    internal class SoundMapListEditorForm : Form
    {
        private readonly SoundMapListEditor SoundMapListEditor;

        public SoundMapListEditorForm()
        {
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            SoundMapListEditor = new SoundMapListEditor();

            SuspendLayout();

            ClientSize = new Size(915, 375);
            Controls.Add(SoundMapListEditor);

            SoundMapListEditor.SaveAndCloseToolStripItem.Click += SaveAndCloseToolStripItem_Click;
            SoundMapListEditor.CloseWithoutSavingToolStripMenuItem.Click += CloseWithoutSavingToolStripMenuItem_Click;

            // Checks to ensure that the form from the designer doesn't exceed 1024x768
            if (this.Width > 1024)
            {
                throw new ConlangAudioHoningException("The default/design width of LanguageHoningForm exceeds the 1024 small screen size limit");
            }
            if (this.Height > 768)
            {
                throw new ConlangAudioHoningException("The default/design height of LanguageHoningForm exceeds the 768 small screen size limit");
            }

        }

        private void CloseWithoutSavingToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveAndCloseToolStripItem_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Update the display showing the Phoneme Replacement List.
        /// </summary>
        public void UpdatePhonemeReplacements()
        {
            SoundMapListEditor.UpdatePhonemeReplacements();
        }


        /// <summary>
        /// List of SoundMap objects being edited.  The order of this list is important.
        /// </summary>
        public List<SoundMap> SoundMapList
        {
            get => SoundMapListEditor.SoundMapList;
            set => SoundMapListEditor.SoundMapList = value;
        }

        /// <summary>
        /// List of Phoneme Replacement Pairs to display at the top of the editor
        /// screen.
        /// </summary>
        public List<(string, string)> PhonemeReplacementPairs
        {
            get => SoundMapListEditor.PhonemeReplacementPairs;
            set => SoundMapListEditor.PhonemeReplacementPairs = value;
        }

        /// <summary>
        /// If this is set to true, then the user has indicated that they have "saved" the changes,
        /// if false, then the user has not saved the changes and they should not be applied.
        /// </summary>
        public bool SoundMapSaved
        {
            get => SoundMapListEditor.SoundMapSaved;
            set => SoundMapListEditor.SoundMapSaved = value;
        }

        /// <summary>
        /// Update the header text.  This will override any automatically generated text from the 
        /// PhonemeReplacementPairs.  However, updating them will override this text.
        /// </summary>
        public string HeaderText
        {
            get => SoundMapListEditor.HeaderText;
            set => SoundMapListEditor.HeaderText = value;
        }
    }
}
