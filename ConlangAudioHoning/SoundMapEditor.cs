/*
 * User Control for editing Sound Map Entry Objects.
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConlangJson;

namespace LanguageEditor
{
    internal class SoundMapEditor : UserControl
    {
        private static Size controlSize = new(850, 50);

        private SoundMap? _soundMapData;

        public SoundMap SoundMapData
        {
            get 
            {
                if (_soundMapData == null)
                {
                    _soundMapData = new SoundMap();
                }
                _soundMapData.pronounciation_regex = txt_pronounciationRegex.Text;
                _soundMapData.phoneme = txt_phoneme.Text;
                _soundMapData.romanization = txt_romanization.Text;
                _soundMapData.spelling_regex = txt_spellingRegex.Text;
                return _soundMapData; 
            }

            set 
            { 
                _soundMapData = value; 
                txt_pronounciationRegex.Text = _soundMapData.pronounciation_regex;
                txt_phoneme.Text = _soundMapData.phoneme;
                txt_romanization.Text = _soundMapData.romanization;
                txt_spellingRegex.Text = _soundMapData.spelling_regex;
            }
        }

        private Label lbl_phoneme;
        private Label lbl_pronounciationRegex;
        private Label lbl_romanization;
        private Label lbl_spellingRegex;

        private TextBox txt_phoneme;
        private TextBox txt_pronounciationRegex;
        private TextBox txt_romanization;
        private TextBox txt_spellingRegex;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SoundMapEditor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = controlSize;
            this.BorderStyle = BorderStyle.FixedSingle;

            lbl_phoneme = new Label();
            lbl_phoneme.Text = "Phoneme:";
            lbl_phoneme.Location = new Point(5, 5);
            lbl_phoneme.Size = new Size(200, 15);
            lbl_phoneme.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_phoneme);

            txt_phoneme = new TextBox();
            txt_phoneme.Location = new Point(205, 5);
            txt_phoneme.Size = new Size(200, 15);
            Controls.Add(txt_phoneme);

            lbl_pronounciationRegex = new Label();
            lbl_pronounciationRegex.Text = "Pronounciation RegEx:";
            lbl_pronounciationRegex.Location = new Point(410, 5);
            lbl_pronounciationRegex.Size = new Size(200, 15);
            lbl_pronounciationRegex.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_pronounciationRegex);

            txt_pronounciationRegex = new TextBox();
            txt_pronounciationRegex.Location = new Point(615, 5);
            txt_pronounciationRegex.Size = new Size(200, 15);
            Controls.Add(txt_pronounciationRegex);

            lbl_romanization = new Label();
            lbl_romanization.Text = "Spelling/Romanization:";
            lbl_romanization.Location = new Point(5, 30);
            lbl_romanization.Size = new Size(200, 15);
            lbl_romanization.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_romanization);

            txt_romanization = new TextBox();
            txt_romanization.Location = new Point(205, 30);
            txt_romanization.Size = new Size(200, 15);
            Controls.Add(txt_romanization);

            lbl_spellingRegex = new Label();
            lbl_spellingRegex.Text = "Spelling RegEx:";
            lbl_spellingRegex.Location = new Point(410, 30);
            lbl_spellingRegex.Size = new Size(200, 15);
            lbl_spellingRegex.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_spellingRegex);

            txt_spellingRegex = new TextBox();
            txt_spellingRegex.Location = new Point(615, 30);
            txt_spellingRegex.Size = new Size(200, 15);
            Controls.Add(txt_spellingRegex);
        }
    }
}
