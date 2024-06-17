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
using ConlangJson;

namespace ConlangAudioHoning
{
    /// <summary>
    /// User Control used to edit a single SoundMap entry.
    /// </summary>
    public class SpellingPronunciationRuleEditor : UserControl
    {
        private static Size controlSize = new(850, 50);

        private SpellingPronunciationRules? _soundMapData;
        private bool _enabled;

        private TextBox? _lastFocused = null;

        /// <summary>
        /// SoundMap object being edited in this editor.
        /// </summary>
        public SpellingPronunciationRules SoundMapData
        {
            get
            {
                _soundMapData ??= new SpellingPronunciationRules();
                _soundMapData.pronunciation_regex = txt_pronunciationRegex.Text;
                _soundMapData.phoneme = txt_phoneme.Text;
                _soundMapData.romanization = txt_romanization.Text;
                _soundMapData.spelling_regex = txt_spellingRegex.Text;
                return _soundMapData;
            }

            set
            {
                _soundMapData = value;
                txt_pronunciationRegex.Text = _soundMapData.pronunciation_regex;
                txt_phoneme.Text = _soundMapData.phoneme;
                txt_romanization.Text = _soundMapData.romanization;
                txt_spellingRegex.Text = _soundMapData.spelling_regex;
            }
        }

        /// <summary>
        /// Allows for enabling and disabling the controls within this editor.
        /// </summary>
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

        private TextBox txt_phoneme;
        private TextBox txt_pronunciationRegex;
        private TextBox txt_romanization;
        private TextBox txt_spellingRegex;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// Instantiates a SoundMapEditor panel.
        /// </summary>
        public SpellingPronunciationRuleEditor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
            Enabled = true;
        }

        /// <summary>
        /// Returns true if the SoundMap being edited is in a valid state.  That is
        /// if it satisfies both of the dependency requirements found in 
        /// https://github.com/RonBOakes/conlang_json/blob/main/doc/conlang_json_spec.pdf
        /// section on spelling_pronounciation_rules entries.
        /// </summary>
        public bool ValidMap
        {
            get
            {
                bool valid = false;

                if ((!String.IsNullOrEmpty(SoundMapData.pronunciation_regex.Trim())) && (!String.IsNullOrEmpty(SoundMapData.phoneme.Trim())))
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

        /// <summary>
        /// Append the supplied string to which of the four text boxes that most recently
        /// had focus.
        /// </summary>
        /// <param name="textToAppend">string containing the text to append.</param>
        public void PasteIntoFocusedBox(string textToAppend)
        {
            if ((_lastFocused != null) && (!String.IsNullOrEmpty(textToAppend)))
            {
                Clipboard.SetText(textToAppend);
                _lastFocused.Paste();
            }
        }

        private void InitializeComponent()
        {
            Label lbl_phoneme;
            Label lbl_pronunciationRegex;
            Label lbl_romanization;
            Label lbl_spellingRegex;

            this.Size = controlSize;
            this.BorderStyle = BorderStyle.FixedSingle;

            lbl_phoneme = new Label
            {
                Text = "Phoneme:",
                Location = new Point(0, 5),
                Size = new Size(130, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_phoneme);

            txt_phoneme = new TextBox
            {
                Location = new Point(135, 5),
                Size = new Size(150, 15)
            };
            txt_phoneme.GotFocus += Txt_phoneme_GotFocus;
            Controls.Add(txt_phoneme);

            lbl_pronunciationRegex = new Label
            {
                Text = "Pronunciation RegEx:",
                Location = new Point(265, 5),
                Size = new Size(150, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_pronunciationRegex);

            txt_pronunciationRegex = new TextBox
            {
                Location = new Point(415, 5),
                Size = new Size(150, 15)
            };
            txt_pronunciationRegex.GotFocus += Txt_pronunciationRegex_GotFocus;
            Controls.Add(txt_pronunciationRegex);

            lbl_romanization = new Label
            {
                Text = "Spelling/Romanization:",
                Location = new Point(0, 30),
                Size = new Size(130, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_romanization);

            txt_romanization = new TextBox
            {
                Location = new Point(135, 30),
                Size = new Size(150, 15)
            };
            txt_romanization.GotFocus += Txt_romanization_GotFocus;
            Controls.Add(txt_romanization);

            lbl_spellingRegex = new Label
            {
                Text = "Spelling RegEx:",
                Location = new Point(265, 30),
                Size = new Size(150, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_spellingRegex);

            txt_spellingRegex = new TextBox
            {
                Location = new Point(415, 30),
                Size = new Size(150, 15)
            };
            txt_spellingRegex.GotFocus += Txt_spellingRegex_GotFocus;
            Controls.Add(txt_spellingRegex);
        }

        private void Txt_phoneme_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_phoneme;
        }

        private void Txt_pronunciationRegex_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_pronunciationRegex;
        }

        private void Txt_romanization_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_romanization;
        }

        private void Txt_spellingRegex_GotFocus(Object? sender, EventArgs e)
        {
            _lastFocused = this.txt_spellingRegex;
        }
    }
}
