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

namespace ConlangAudioHoning
{
    internal class SoundMapEditor : UserControl
    {
        private static Size controlSize = new(850, 50);

        private SoundMap? _soundMapData;
        private bool _enabled;

        private TextBox? _lastFocused = null;

        public SoundMap SoundMapData
        {
            get 
            {
                if (_soundMapData == null)
                {
                    _soundMapData = new SoundMap();
                }
                _soundMapData.pronounciation_regex = txt_pronunciationRegex.Text;
                _soundMapData.phoneme = txt_phoneme.Text;
                _soundMapData.romanization = txt_romanization.Text;
                _soundMapData.spelling_regex = txt_spellingRegex.Text;
                return _soundMapData; 
            }

            set 
            { 
                _soundMapData = value; 
                txt_pronunciationRegex.Text = _soundMapData.pronounciation_regex;
                txt_phoneme.Text = _soundMapData.phoneme;
                txt_romanization.Text = _soundMapData.romanization;
                txt_spellingRegex.Text = _soundMapData.spelling_regex;
            }
        }

        public new bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                txt_phoneme.Enabled = _enabled;
                txt_pronunciationRegex.Enabled = _enabled;
                txt_romanization.Enabled = _enabled;
                txt_spellingRegex.Enabled = _enabled;
            }
        }

        private Label lbl_phoneme;
        private Label lbl_pronunciationRegex;
        private Label lbl_romanization;
        private Label lbl_spellingRegex;

        private TextBox txt_phoneme;
        private TextBox txt_pronunciationRegex;
        private TextBox txt_romanization;
        private TextBox txt_spellingRegex;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SoundMapEditor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
            Enabled = true;
        }

        public bool ValidMap
        {
            get
            {
                bool valid = false;
           
                if ((!String.IsNullOrEmpty(SoundMapData.pronounciation_regex.Trim())) && (!String.IsNullOrEmpty(SoundMapData.phoneme.Trim())))
                {
                    valid = true;
                }
                if ((!String.IsNullOrEmpty(SoundMapData.spelling_regex.Trim())) && (!String.IsNullOrEmpty(SoundMapData.romanization.Trim())))
                {
                    valid = true;
                }
                return valid;
            }
        }

        public void AppendToFocusedBox(string textToAppend)
        {
            if((_lastFocused != null) && (!String.IsNullOrEmpty(textToAppend)))
            {
                _lastFocused.Text += textToAppend;
                _lastFocused.Select(_lastFocused.Text.Length, 0);
                _lastFocused.Focus();
            }
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
            txt_phoneme.GotFocus += txt_phoneme_GotFocus;
            Controls.Add(txt_phoneme);

            lbl_pronunciationRegex = new Label();
            lbl_pronunciationRegex.Text = "Pronunciation RegEx:";
            lbl_pronunciationRegex.Location = new Point(410, 5);
            lbl_pronunciationRegex.Size = new Size(200, 15);
            lbl_pronunciationRegex.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_pronunciationRegex);

            txt_pronunciationRegex = new TextBox();
            txt_pronunciationRegex.Location = new Point(615, 5);
            txt_pronunciationRegex.Size = new Size(200, 15);
            txt_pronunciationRegex.GotFocus += txt_pronunciationRegex_GotFocus;
            Controls.Add(txt_pronunciationRegex);

            lbl_romanization = new Label();
            lbl_romanization.Text = "Spelling/Romanization:";
            lbl_romanization.Location = new Point(5, 30);
            lbl_romanization.Size = new Size(200, 15);
            lbl_romanization.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_romanization);

            txt_romanization = new TextBox();
            txt_romanization.Location = new Point(205, 30);
            txt_romanization.Size = new Size(200, 15);
            txt_romanization.GotFocus += txt_romanization_GotFocus;
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
            txt_spellingRegex.GotFocus += txt_spellingRegex_GotFocus;
            Controls.Add(txt_spellingRegex);
        }

        private void txt_phoneme_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_phoneme;
        }

        private void txt_pronunciationRegex_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_pronunciationRegex;
        }

        private void txt_romanization_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_romanization;
        }

        private void txt_spellingRegex_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_spellingRegex;
        }
    }
}
