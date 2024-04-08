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

            MenuStrip menuStrip1 = new MenuStrip();
            ToolStripMenuItem controlsToolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItem saveAndCloseToolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItem closeWithoutSavingToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            //
            // menuStrip1
            //
            menuStrip1.Items.AddRange(new ToolStripItem[] { controlsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1007, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            //
            // controlsToolStripMenuItem
            //
            controlsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAndCloseToolStripMenuItem, closeWithoutSavingToolStripMenuItem });
            controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            controlsToolStripMenuItem.Size = new Size(64, 20);
            controlsToolStripMenuItem.Text = "Controls";
            //
            // saveAndCloseToolStripMenuItem
            //
            saveAndCloseToolStripMenuItem.Name = "saveAndCloseToolStripMenuItem";
            saveAndCloseToolStripMenuItem.Size = new Size(184, 22);
            saveAndCloseToolStripMenuItem.Text = "Save and Close";
            saveAndCloseToolStripMenuItem.Click += SaveAndCloseToolStripMenuItem_Click;
            //
            // closeWithoutSavingToolStripMenuItem
            //
            closeWithoutSavingToolStripMenuItem.Name = "closeWithoutSavingToolStripMenuItem";
            closeWithoutSavingToolStripMenuItem.Size = new Size(184, 22);
            closeWithoutSavingToolStripMenuItem.Text = "Close without saving";
            closeWithoutSavingToolStripMenuItem.Click += CloseWithoutSavingToolStripMenuItem_Click;

            CharacterInsertToolStripMenuItem ciMenu = new();
            _ = menuStrip1.Items.Add(ciMenu);
            ciMenu.AddClickDelegate(SoundMapListEditor.CharInsetToolStripMenuItem_Click);

            ClientSize = new Size(1008, 425);

            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Controls.Add(SoundMapListEditor);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();

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

        private void SaveAndCloseToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            SoundMapSaved = true;
            this.Close();
        }

        private void CloseWithoutSavingToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            SoundMapSaved = false;
            this.Close();
        }


    }
}
