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
        private bool _soundMapSaved = false;

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
                loadForm();
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
        public bool SoundMapSaved
        {
            get => _soundMapSaved;
        }

        /// <summary>
        /// Update the header text.  This will override any automatically generated text from the 
        /// PhonemeReplacementPairs.  However, updating them will override this text.
        /// </summary>
        public string headerText
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

            CharacterInsertToolStripMenuItem ciMenu = new CharacterInsertToolStripMenuItem();
            menuStrip1.Items.Add(ciMenu);
            _soundMapList = [];
            ciMenu.AddClickDelegate(CharInsetToolStripMenuItem_Click);
        }

        /// <summary>
        /// Update the display showing the Phoneme Replacement List.
        /// </summary>
        public void UpdatePhonemeReplacements()
        {
            StringBuilder sb = new StringBuilder();
            foreach ((string phonemeToBeReplaced, string replacementPhoneme) in PhonemeReplacementPairs)
            {
                sb.AppendFormat("{0} -> {1},", phonemeToBeReplaced, replacementPhoneme);
            }
            txtPhonemeReplacements.Text = sb.ToString();
        }

        private void loadForm()
        {
            UpdateSoundMapEntries();
        }

        private void btnAddAbove_Click(object sender, EventArgs e)
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

        private void btnAddBelow_Click(object sender, EventArgs e)
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

        private void addCharToSoundMap(string charToAdd)
        {
            soundMapEditor.AppendToFocusedBox(charToAdd);
            soundMapEditor.Focus();
        }

        private void saveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _soundMapSaved = true;
            this.Close();
        }

        private void closeWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _soundMapSaved = false;
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

        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            int index = lbx_soundMapListEntries.SelectedIndex;
            if (index >= 0)
            {
                SoundMap soundMapToEdit = SoundMapList[index];
                this.soundMapEditor.SoundMapData = soundMapToEdit.copy();
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
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
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Phonetic Regex: {0}, ", map.pronunciation_regex);
                sb.AppendFormat("Phoneme: {0}, ", map.phoneme);
                sb.AppendFormat("Spelling Regex: {0}, ", map.spelling_regex);
                sb.AppendFormat("Spelling: {0}", map.romanization);
                lbx_soundMapListEntries.Items.Add(sb.ToString());
            }
            lbx_soundMapListEntries.ResumeLayout();
        }

        private void btnReplaceSelected_Click(object sender, EventArgs e)
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
