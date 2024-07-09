/*
 * Form to wrap around the Sound Map Editor control
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
 */using ConlangJson;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Form for editing the spelling and pronunciation rules.
    /// </summary>
    public class SpellingPronunciationRuleListEditorForm : Form
    {
        private readonly SpellingPronunciationRuleListEditor SoundMapListEditor;

        /// <summary>
        /// Constructor for constructing the spelling and pronunciation rules editor form.
        /// </summary>
        /// <exception cref="ConlangAudioHoningException"></exception>
        public SpellingPronunciationRuleListEditorForm()
        {
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            SoundMapListEditor = new SpellingPronunciationRuleListEditor();

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
        public List<SpellingPronunciationRules> SoundMapList
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

        /// <summary>
        /// Custom dialog that prompts the user about mass changes to the lexicon.  
        /// </summary>
        /// <param name="title">Optional string to change the title</param>
        /// <param name="promptText">Optional string to change the dialog box text</param>
        /// <param name="button1">Optional text to change the first button - returns DialogResult.Yes</param>
        /// <param name="button2">Optional text to change the second button - returns DialogResult.No</param>
        /// <returns></returns>
        public static DialogResult SpellingPronunciationDialogBox(string title = "Spelling or Pronunciation",
            string promptText = "Preserve the spelling or pronunciation?",
            string button1 = "Spelling", 
            string button2 = "Pronunciation")
        {
            Form form = new();
            Label label = new();
            Button button_1 = new();
            Button button_2 = new();

            int buttonStartPos = 130; //Standard two button position

            form.Text = title;

            // Label
            label.Text = promptText;
            label.SetBounds(9, 20, 372, 13);
            label.Font = new Font("Microsoft Tai Le", 10, FontStyle.Regular);

            button_1.Text = button1;
            button_2.Text = button2;
            button_1.DialogResult = DialogResult.Yes;
            button_2.DialogResult = DialogResult.No;


            button_1.SetBounds(buttonStartPos, 72, 100, 23);
            button_2.SetBounds(buttonStartPos + 100, 72, 100, 23);

            label.AutoSize = true;
            button_1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange([label, button_1, button_2]);

            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = button_1;
            form.CancelButton = button_2;

            DialogResult dialogResult = form.ShowDialog();
            return dialogResult;
        }
    }
}
