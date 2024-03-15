/*
 * User Control for editing Sound Map Lists.
 *
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
using System.Text;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Form for editing the SoundMap List used for spelling and pronunciation of a 
    /// language.
    /// </summary>
    public partial class SoundMapListEditor : Form
    {
        private List<SoundMap> _soundMapList;
        private List<(string, string)> _phonemeReplacementPairs = [];

        /// <summary>
        /// List of SoundMap objects being edited.  The order of this list is important.
        /// </summary>
        public List<SoundMap> SoundMapList
        {
            get
            {
                return _soundMapList;
            }
            set
            {
                _soundMapList = value;
                LoadForm();
            }
        }

        /// <summary>
        /// List of Phoneme Replacement Pairs to display at the top of the editor
        /// screen.
        /// </summary>
        public List<(string, string)> PhonemeReplacementPairs
        {
            get => _phonemeReplacementPairs;
            set
            {
                _phonemeReplacementPairs = value;
                UpdatePhonemeReplacements();
            }
        }

        /// <summary>
        /// If this is set to true, then the user has indicated that they have "saved" the changes,
        /// if false, then the user has not saved the changes and they should not be applied.
        /// </summary>
        public bool SoundMapSaved { get; private set; } = false;

        /// <summary>
        /// Update the header text.  This will override any automatically generated text from the 
        /// PhonemeReplacementPairs.  However, updating them will override this text.
        /// </summary>
        public string HeaderText
        {
            get
            {
                string text = txtPhonemeReplacements.Text;
                return text;
            }
            set
            {
                txtPhonemeReplacements.Text = value;
            }
        }

        /// <summary>
        /// Constructor for a SoundMapListEditor.
        /// </summary>
        public SoundMapListEditor()
        {
            InitializeComponent();

            // Checks to ensure that the form from the designer doesn't exceed 1024x768
            if (this.Width > 1024)
            {
                throw new ConlangAudioHoningException("The default/design width of LanguageHoningForm exceeds the 1024 small screen size limit");
            }
            if (this.Height > 768)
            {
                throw new ConlangAudioHoningException("The default/design height of LanguageHoningForm exceeds the 768 small screen size limit");
            }

            CharacterInsertToolStripMenuItem ciMenu = new();
            _ = menuStrip1.Items.Add(ciMenu);
            _soundMapList = [];
            ciMenu.AddClickDelegate(CharInsetToolStripMenuItem_Click);
        }

        /// <summary>
        /// Update the display showing the Phoneme Replacement List.
        /// </summary>
        public void UpdatePhonemeReplacements()
        {
            StringBuilder sb = new();
            foreach ((string phonemeToBeReplaced, string replacementPhoneme) in PhonemeReplacementPairs)
            {
                _ = sb.AppendFormat("{0} -> {1},", phonemeToBeReplaced, replacementPhoneme);
            }
            txtPhonemeReplacements.Text = sb.ToString();
        }

        private void LoadForm()
        {
            UpdateSoundMapEntries();
        }

        private void BtnAddAbove_Click(object sender, EventArgs e)
        {
            if (!soundMapEditor.ValidMap)
            {
                return;
            }
            int index = lbx_soundMapListEntries.SelectedIndex; // Index will be the index to _soundMapList
            if (index >= 0)
            {
                SoundMapList.Insert(index, soundMapEditor.SoundMapData.copy());
                UpdateSoundMapEntries();
            }
        }

        private void BtnAddBelow_Click(object sender, EventArgs e)
        {
            if (!soundMapEditor.ValidMap)
            {
                return;
            }
            int index = lbx_soundMapListEntries.SelectedIndex; // Index will be the index to _soundMapList
            index += 1;
            if (index > 0)
            {
                SoundMapList.Insert(index, soundMapEditor.SoundMapData.copy());
                UpdateSoundMapEntries();
            }
        }

        private void SaveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundMapSaved = true;
            this.Close();
        }

        private void CloseWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundMapSaved = false;
            this.Close();
        }

        private void CharInsetToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            if (String.IsNullOrEmpty(menuItem.Text))
            {
                return;
            }
            string charToInsert = menuItem.Text.Split()[0];
            soundMapEditor.AppendToFocusedBox(charToInsert);
        }

        private void BtnEditSelected_Click(object sender, EventArgs e)
        {
            int index = lbx_soundMapListEntries.SelectedIndex;
            if (index >= 0)
            {
                SoundMap soundMapToEdit = SoundMapList[index];
                this.soundMapEditor.SoundMapData = soundMapToEdit.copy();
            }
        }

        private void BtnDeleteSelected_Click(object sender, EventArgs e)
        {
            int index = lbx_soundMapListEntries.SelectedIndex;
            if (index >= 0)
            {
                DialogResult result = MessageBox.Show("This action cannot be reversed", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    SoundMapList.RemoveAt(index);
                    UpdateSoundMapEntries();
                }
            }
        }

        private void UpdateSoundMapEntries()
        {
            lbx_soundMapListEntries.SuspendLayout();
            lbx_soundMapListEntries.Items.Clear();
            foreach (SoundMap map in _soundMapList)
            {
                StringBuilder sb = new();
                _ = sb.AppendFormat("Phonetic Regex: {0}, ", map.pronunciation_regex);
                _ = sb.AppendFormat("Phoneme: {0}, ", map.phoneme);
                _ = sb.AppendFormat("Spelling Regex: {0}, ", map.spelling_regex);
                _ = sb.AppendFormat("Spelling: {0}", map.romanization);
                _ = lbx_soundMapListEntries.Items.Add(sb.ToString());
            }
            lbx_soundMapListEntries.ResumeLayout();
        }

        private void BtnReplaceSelected_Click(object sender, EventArgs e)
        {
            if (!soundMapEditor.ValidMap)
            {
                return;
            }
            int index = lbx_soundMapListEntries.SelectedIndex; // Index will be the index to _soundMapList
            if (index >= 0)
            {
                SoundMapList[index] = soundMapEditor.SoundMapData.copy();
                UpdateSoundMapEntries();
            }

        }
    }
}
