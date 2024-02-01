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

        public SoundMapListEditor()
        {
            InitializeComponent();
            _soundMapList = new List<SoundMap>();
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
    }
}
