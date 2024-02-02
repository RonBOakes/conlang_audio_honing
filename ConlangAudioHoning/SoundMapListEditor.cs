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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConlangJson;

namespace ConlangAudioHoning
{
    public partial class SoundMapListEditor : Form
    {
        private List<SoundMap> _soundMapList;
        private bool _soundMapSaved = false;

        public List<SoundMap> SoundMapList
        {
            get
            {
                // TODO: extract data from form.
                return _soundMapList;
            }
            set
            {
                _soundMapList = value;
                loadForm();
            }
        }

        public bool SoundMapSaved
        {
            get => _soundMapSaved;
        }

        public SoundMapListEditor()
        {
            InitializeComponent();
            _soundMapList = new List<SoundMap>();
            List<ToolStripMenuItem> ipaMenuItems = new List<ToolStripMenuItem>()
            {
                pToolStripMenuItem,
                bToolStripMenuItem,
                tToolStripMenuItem,
                dToolStripMenuItem,
                ʈToolStripMenuItem,
                ɖToolStripMenuItem,
                cToolStripMenuItem,
                ɟToolStripMenuItem,
                kToolStripMenuItem,
                ɡToolStripMenuItem,
                qToolStripMenuItem,
                ɢToolStripMenuItem,
                ʔToolStripMenuItem,
                mToolStripMenuItem,
                ɱToolStripMenuItem,
                nToolStripMenuItem,
                ɳToolStripMenuItem,
                ɲToolStripMenuItem,
                ŋToolStripMenuItem,
                ɴToolStripMenuItem,
                ʙToolStripMenuItem,
                rToolStripMenuItem,
                ʀToolStripMenuItem,
                ⱱToolStripMenuItem,
                ɾToolStripMenuItem,
                ɽToolStripMenuItem,
                ɸToolStripMenuItem,
                βToolStripMenuItem,
                fToolStripMenuItem,
                vToolStripMenuItem,
                θToolStripMenuItem,
                ðToolStripMenuItem,
                sToolStripMenuItem,
                zToolStripMenuItem,
                ʃToolStripMenuItem,
                ʒToolStripMenuItem,
                ʂToolStripMenuItem,
                ʂToolStripMenuItem1,
                çToolStripMenuItem,
                ʝToolStripMenuItem,
                xToolStripMenuItem,
                ɣToolStripMenuItem,
                χToolStripMenuItem,
                ʁToolStripMenuItem,
                ʁToolStripMenuItem1,
                ʁToolStripMenuItem2,
                ʜToolStripMenuItem,
                ʢToolStripMenuItem,
                ɦToolStripMenuItem,
                ɬToolStripMenuItem,
                ɮToolStripMenuItem,
                ʋToolStripMenuItem,
                ɹToolStripMenuItem,
                ɻToolStripMenuItem,
                ɰToolStripMenuItem,
                ɭToolStripMenuItem,
                ʎToolStripMenuItem,
                ʟToolStripMenuItem,
                ʘToolStripMenuItem,
                ǀDentalClickToolStripMenuItem,
                ǃAlveolarClickToolStripMenuItem,
                ǂClickToolStripMenuItem,
                ǁAlveolarLateralClickToolStripMenuItem,
                ɓBilabialVoicedImplosiveToolStripMenuItem,
                ɗAlveolarVoicedImplosiveToolStripMenuItem,
                ʄPalatalVoicedImplosiveToolStripMenuItem,
                ɠValarVoicedImplosiveToolStripMenuItem,
                ʛUvularVoicedImplosiveToolStripMenuItem,
                ʍVoicelessLabialvelarApproximateToolStripMenuItem,
                ɥVoicedLabialpalatalApproximateToolStripMenuItem,
                iToolStripMenuItem,
                yToolStripMenuItem,
                ɨToolStripMenuItem,
                ʉToolStripMenuItem,
                ɯToolStripMenuItem,
                uToolStripMenuItem,
                ɪToolStripMenuItem,
                ʏToolStripMenuItem,
                ɪToolStripMenuItem1,
                ʊToolStripMenuItem,
                ʊToolStripMenuItem1,
                eToolStripMenuItem,
                øToolStripMenuItem,
                əToolStripMenuItem,
                ɤToolStripMenuItem,
                oToolStripMenuItem,
                eToolStripMenuItem1,
                øToolStripMenuItem1,
                ɘToolStripMenuItem,
                ɵToolStripMenuItem,
                ɤToolStripMenuItem1,
                oToolStripMenuItem1,
                ɛToolStripMenuItem,
                œToolStripMenuItem,
                ɜToolStripMenuItem,
                ɞToolStripMenuItem,
                ʌToolStripMenuItem,
                ɔToolStripMenuItem,
                æToolStripMenuItem,
                ɐToolStripMenuItem,
                aToolStripMenuItem,
                ɶToolStripMenuItem,
                äToolStripMenuItem,
                ɑToolStripMenuItem,
                ɒToolStripMenuItem,
                lengthenedToolStripMenuItem,
                halfLengthenedToolStripMenuItem,
                shortenedToolStripMenuItem,
                rhoticityToolStripMenuItem,
            };
            foreach (ToolStripMenuItem menuItem in ipaMenuItems)
            {
                if (!String.IsNullOrEmpty(menuItem.Text))
                {
                    string menuChar = menuItem.Text.Trim().Split()[0];
                    menuItem.Text = string.Format("{0} -- {1}", menuChar, IpaUtilities.IpaPhonemesMap[menuChar]);
                }
            }
        }

        private void loadForm()
        {
            foreach (SoundMap map in _soundMapList)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Phonetic Regex: {0}, ", map.pronounciation_regex);
                sb.AppendFormat("Phoneme: {0}, ", map.phoneme);
                sb.AppendFormat("Spelling Regex: {0}, ", map.spelling_regex);
                sb.AppendFormat("Spelling: {0}", map.romanization);
                lbx_soundMapListEntries.Items.Add(sb.ToString());
            }
        }

        private void btnAddAbove_Click(object sender, EventArgs e)
        {
            if (!soundMapEditor2.ValidMap)
            {
                return;
            }
            int index = lbx_soundMapListEntries.SelectedIndex; // Index will be the index to _soundMapList
        }

        private void btnAddBelow_Click(object sender, EventArgs e)
        {
            if (!soundMapEditor2.ValidMap)
            {
                return;
            }
            int index = lbx_soundMapListEntries.SelectedIndex; // Index will be the index to _soundMapList
        }

        private void addCharToSoundMap(string charToAdd)
        {
            soundMapEditor2.AppendToFocusedBox(charToAdd);
            soundMapEditor2.Focus();
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

        private void CharInsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            if (String.IsNullOrEmpty(menuItem.Text))
            {
                return;
            }
            string charToInsert = menuItem.Text.Split()[0];
            soundMapEditor2.AppendToFocusedBox(charToInsert);
        }
    }
}
