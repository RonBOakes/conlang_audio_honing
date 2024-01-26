/*
 * User Control for edit Conlang lexicon objects
 
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
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEditor
{
    internal class LexiconEditor : UserControl
    {
        private static Size controlSize = new(850, 140);
        private static string[]? _partOfSpeechList;

        private static List<SoundMap>? _soundMapList;

        private LexiconEntry? _lexiconEntry;
        private bool dirty = false;

        public LexiconEntry lexiconEntry
        {
            get
            {
                if (_lexiconEntry == null)
                {
                    _lexiconEntry = new LexiconEntry();
                }
                if (dirty)
                {
                    _lexiconEntry.phonetic = txt_phonetic.Text.Trim();
                    _lexiconEntry.spelled = txt_spelled.Text.Trim();
                    _lexiconEntry.english = txt_english.Text.Trim();
                    _lexiconEntry.part_of_speech = cmb_partOfSpeech.Text.Trim();
                    _lexiconEntry.declensions.Clear();
                    foreach (TextBox txt_declension in pnl_declensionList.Controls)
                    {
                        _lexiconEntry.declensions.Add(txt_declension.Text.Trim());
                    }
                    _lexiconEntry.derived_word = ckb_derivedWord.Checked;
                    _lexiconEntry.declined_word = ckb_declinedWord.Checked;
                }
                dirty = false;
                return _lexiconEntry ?? new LexiconEntry();
            }
            set
            {
                _lexiconEntry = value;
                txt_phonetic.Text = _lexiconEntry.phonetic;
                txt_spelled.Text = _lexiconEntry.spelled;
                txt_english.Text = _lexiconEntry.english;
                cmb_partOfSpeech.SelectedItem = _lexiconEntry.part_of_speech;
                LoadDeclensionList();
                ckb_derivedWord.Checked = _lexiconEntry.derived_word ?? false;
                ckb_declinedWord.Checked = _lexiconEntry.declined_word ?? false;
                dirty = false;
            }
        }

        private Label lbl_phonetic;
        private Label lbl_spelled;
        private Label lbl_english;
        private Label lbl_partOfSpeech;
        private Label lbl_declensions;
        private Label lbl_derivedWord;
        private Label lbl_declinedWord;

        private TextBox txt_phonetic;
        private TextBox txt_spelled;
        private TextBox txt_english;

        private ComboBox cmb_partOfSpeech;

        private Panel pnl_declensionList;

        private CheckBox ckb_derivedWord;
        private CheckBox ckb_declinedWord;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public LexiconEditor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
        }

        internal static List<string> PartOfSpeechList
        {
            set
            {
                _partOfSpeechList = new string[value.Count];
                value.CopyTo(_partOfSpeechList, 0);
                // TODO: combo boxes will need to be updated.
            }
        }

        internal static List<SoundMap> SoundMapList
        {
            set
            {
                _soundMapList = value;
            }
        }

        private void LoadDeclensionList()
        {
            if (_lexiconEntry != null)
            {
                int xPos = 0, yPos = 0;
                pnl_declensionList.SuspendLayout();
                try
                {
                    foreach (string declension in _lexiconEntry.declensions)
                    {
                        TextBox txt_declension = new();
                        txt_declension.Text = declension;
                        txt_declension.Location = new Point(xPos, yPos);
                        txt_declension.Size = new Size(605, 15);
                        pnl_declensionList.Controls.Add(txt_declension);
                        txt_declension.TextChanged += Txt_declensions_TextChanged;
                        yPos += txt_declension.Height + 5;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    MessageBox.Show("Exception Received: " + ex.Message);
                }
            }
            pnl_declensionList.ResumeLayout(true);
        }

        private void InitializeComponent()
        {
            this.Size = controlSize;
            this.BorderStyle = BorderStyle.FixedSingle;

            lbl_phonetic = new Label();
            lbl_phonetic.Text = "Phonetic Representation:";
            lbl_phonetic.Location = new Point(5, 5);
            lbl_phonetic.Size = new Size(200, 15);
            lbl_phonetic.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_phonetic);

            txt_phonetic = new TextBox();
            txt_phonetic.Location = new Point(205, 5);
            txt_phonetic.Size = new Size(200, 15);
            txt_phonetic.TextChanged += Txt_phonetic_TextChanged;
            Controls.Add(txt_phonetic);

            lbl_spelled = new Label();
            lbl_spelled.Text = "Spelled/Romanized:";
            lbl_spelled.Location = new Point(410, 5);
            lbl_spelled.Size = new Size(200, 15);
            lbl_spelled.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_spelled);

            txt_spelled = new TextBox();
            txt_spelled.Location = new Point(615, 5);
            txt_spelled.Size = new Size(200, 15);
            txt_spelled.TextChanged += Txt_spelled_TextChanged;
            Controls.Add(txt_spelled);

            lbl_english = new Label();
            lbl_english.Text = "English Equivalent:";
            lbl_english.Location = new Point(5, 30);
            lbl_english.Size = new Size(200, 15);
            lbl_english.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_english);

            txt_english = new TextBox();
            txt_english.Location = new Point(205, 30);
            txt_english.Size = new Size(200, 15);
            txt_english.TextChanged += Txt_english_TextChanged;
            Controls.Add(txt_english);

            lbl_partOfSpeech = new Label();
            lbl_partOfSpeech.Text = "Part of Speech:";
            lbl_partOfSpeech.Location = new Point(410, 30);
            lbl_partOfSpeech.Size = new Size(200, 15);
            lbl_partOfSpeech.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_partOfSpeech);

            cmb_partOfSpeech = new ComboBox();
            cmb_partOfSpeech.Location = new Point(615, 30);
            cmb_partOfSpeech.Size = new Size(200, 15);
#pragma warning disable CS8604 // Possible null reference argument.
            cmb_partOfSpeech.Items.AddRange(_partOfSpeechList);
#pragma warning restore CS8604 // Possible null reference argument.
            cmb_partOfSpeech.SelectedIndex = 0;
            Controls.Add(cmb_partOfSpeech);

            lbl_declensions = new Label();
            lbl_declensions.Text = "Declensions";
            lbl_declensions.Location = new Point(5, 60);
            lbl_declensions.Size = new Size(200, 15);
            lbl_declensions.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_declensions);

            pnl_declensionList = new Panel();
            pnl_declensionList.Location = new Point(205, 60);
            pnl_declensionList.Size = new Size(610, 60);
            pnl_declensionList.AutoScroll = true;
            pnl_declensionList.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(pnl_declensionList);

            lbl_derivedWord = new Label();
            lbl_derivedWord.Text = "Derived Word:";
            lbl_derivedWord.Location = new Point(5, 120);
            lbl_derivedWord.Size = new Size(200, 15);
            lbl_derivedWord.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_derivedWord);

            ckb_derivedWord = new CheckBox();
            ckb_derivedWord.Location = new Point(205, 120);
            ckb_derivedWord.Enabled = false;
            Controls.Add(ckb_derivedWord);

            lbl_declinedWord = new Label();
            lbl_declinedWord.Text = "Declined Word:";
            lbl_declinedWord.Location = new Point(410, 120);
            lbl_declinedWord.Size = new Size(200, 15);
            lbl_declinedWord.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_declinedWord);

            ckb_declinedWord = new CheckBox();
            ckb_declinedWord.Location = new Point(615, 120);
            ckb_declinedWord.Enabled = false;
            Controls.Add(ckb_declinedWord);
        }

        private void Txt_declensions_TextChanged(object? sender, EventArgs e)
        {
            dirty = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_english_TextChanged(object? sender, EventArgs e)
        {
            dirty = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_spelled_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                txt_spelled.TextChanged -= Txt_spelled_TextChanged;
                txt_phonetic.TextChanged -= Txt_phonetic_TextChanged;

                if (_soundMapList != null)
                {
                    txt_phonetic.Text = ConLangUtilities.SoundOutWord(txt_spelled.Text.Trim(), _soundMapList);
                }
            }
            finally
            {
                txt_spelled.TextChanged += Txt_spelled_TextChanged;
                txt_phonetic.TextChanged += Txt_phonetic_TextChanged;
                dirty = true;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Txt_phonetic_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                txt_spelled.TextChanged -= Txt_spelled_TextChanged;
                txt_phonetic.TextChanged -= Txt_phonetic_TextChanged;

                if (_soundMapList != null)
                {
                    txt_spelled.Text = ConLangUtilities.SpellWord(txt_phonetic.Text.Trim(), _soundMapList);
                }
            }
            finally
            {
                txt_spelled.TextChanged += Txt_spelled_TextChanged;
                txt_phonetic.TextChanged += Txt_phonetic_TextChanged;
                dirty = true;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler Changed;
    }
}
