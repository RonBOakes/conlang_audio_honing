/*
 * Form for wrapping around the Lexicon Editor control
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
    /// Form version of the Lexicon Editor.  This wraps around the LexiconEditor to allow for its use as an independent form.
    /// </summary>
    public class LexicalOrderEditorForm : Form
    {
        private readonly LexicalOrderEditor editor;

        /// <summary>
        /// Constructs a LexiconEditorForm.
        /// </summary>
        public LexicalOrderEditorForm(LanguageDescription language, bool regenerate = false)
        {
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 470);

            editor = new LexicalOrderEditor(language, regenerate);

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
    }
}
