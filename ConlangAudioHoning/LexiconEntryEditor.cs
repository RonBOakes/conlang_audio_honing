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
using ConlangAudioHoning;
using ConlangJson;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConlangAudioHoning
{
    internal class LexiconEntryEditor : Form
    {
        private static Size controlSize = new(850, 280);

        private List<SoundMap>? _soundMapList;
        private List<string>? _partOfSpeechList;

        private LexiconEntry? _lexiconEntry;
        private bool dirty = false;
        public bool LexiconEntrySaved
        {
            get; set;
        }

        public LexiconEntry LexiconEntry
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
                return _lexiconEntry;
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

        private TextBox txt_phonetic;
        private TextBox txt_spelled;
        private TextBox txt_english;

        private ComboBox cmb_partOfSpeech;

        private Panel pnl_declensionList;

        private CheckBox ckb_derivedWord;
        private CheckBox ckb_declinedWord;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public LexiconEntryEditor(LexiconEntry? lexiconEntry = null, List<string>? partOfSpeechList = null, List<SoundMap>? soundMapList = null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();

            if (lexiconEntry == null)
            {
                LexiconEntry = new LexiconEntry();
            }
            else
            { 
                LexiconEntry = lexiconEntry;
            }

            if(partOfSpeechList == null)
            {
                _partOfSpeechList = [];
            }
            else
            {
                _partOfSpeechList = partOfSpeechList;
                if (cmb_partOfSpeech != null)
                {
                    cmb_partOfSpeech.Items.AddRange(_partOfSpeechList.ToArray());
                    cmb_partOfSpeech.SelectedIndex = 0;
                }
            }

            if (soundMapList == null)
            {
                _soundMapList = [];
            }
            else
            {
                _soundMapList = soundMapList;
            }

            dirty = false;
            LexiconEntrySaved = false;
        }

        internal List<string> PartOfSpeechList
        {
            set
            {
                _partOfSpeechList = value;
                cmb_partOfSpeech.Items.AddRange(_partOfSpeechList.ToArray());
                cmb_partOfSpeech.SelectedIndex = 0;
            }
            private get
            {
                return [.. _partOfSpeechList];
            }
        }

        internal List<SoundMap> SoundMapList
        {
            set
            {
                _soundMapList = value;
            }
            private get
            {
                return _soundMapList ?? [];
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
                        TextBox txt_declension = new()
                        {
                            Text = declension,
                            Location = new Point(xPos, yPos),
                            Size = new Size(605, 15)
                        };
                        pnl_declensionList.Controls.Add(txt_declension);
                        txt_declension.TextChanged += Txt_any_TextChanged;
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
            Label lbl_phonetic;
            Label lbl_spelled;
            Label lbl_english;
            Label lbl_partOfSpeech;
            Label lbl_declensions;
            Label lbl_derivedWord;
            Label lbl_declinedWord;
            MenuStrip menuStrip1;
            ToolStripMenuItem controlsToolStripMenuItem;
            ToolStripMenuItem saveAndCloseToolStripMenuItem;
            ToolStripMenuItem closeWithoutSavingToolStripMenuItem;

            this.Size = controlSize;

            lbl_phonetic = new Label
            {
                Text = "Phonetic Representation:",
                Location = new Point(5, 25),
                Size = new Size(200, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_phonetic);

            txt_phonetic = new TextBox
            {
                Location = new Point(205, 25),
                Size = new Size(200, 15)
            };
            txt_phonetic.TextChanged += Txt_phonetic_TextChanged;
            Controls.Add(txt_phonetic);

            lbl_spelled = new Label
            {
                Text = "Spelled/Romanized:",
                Location = new Point(410, 25),
                Size = new Size(200, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_spelled);

            txt_spelled = new TextBox
            {
                Location = new Point(615, 25),
                Size = new Size(200, 15)
            };
            txt_spelled.TextChanged += Txt_spelled_TextChanged;
            Controls.Add(txt_spelled);

            lbl_english = new Label
            {
                Text = "English Equivalent:",
                Location = new Point(5, 50),
                Size = new Size(200, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_english);

            txt_english = new TextBox
            {
                Location = new Point(205, 50),
                Size = new Size(200, 15)
            };
            txt_english.TextChanged += Txt_any_TextChanged;
            Controls.Add(txt_english);

            lbl_partOfSpeech = new Label
            {
                Text = "Part of Speech:",
                Location = new Point(410, 50),
                Size = new Size(200, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_partOfSpeech);

            cmb_partOfSpeech = new ComboBox
            {
                Location = new Point(615, 50),
                Size = new Size(200, 15)
            };

            if (_partOfSpeechList != null)
            {
                cmb_partOfSpeech.Items.AddRange(_partOfSpeechList.ToArray());
                cmb_partOfSpeech.SelectedIndex = 0;
            }
            else
            {
                cmb_partOfSpeech.SelectedIndex = -1;
            }
            Controls.Add(cmb_partOfSpeech);

            lbl_declensions = new Label
            {
                Text = "Declensions",
                Location = new Point(5, 80),
                Size = new Size(200, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_declensions);

            pnl_declensionList = new Panel
            {
                Location = new Point(205, 80),
                Size = new Size(610, 60),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(pnl_declensionList);

            lbl_derivedWord = new Label
            {
                Text = "Derived Word:",
                Location = new Point(5, 140),
                Size = new Size(200, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_derivedWord);

            ckb_derivedWord = new CheckBox
            {
                Location = new Point(205, 140),
                Enabled = false
            };
            Controls.Add(ckb_derivedWord);

            lbl_declinedWord = new Label
            {
                Text = "Declined Word:",
                Location = new Point(410, 140),
                Size = new Size(200, 15),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_declinedWord);

            ckb_declinedWord = new CheckBox
            {
                Location = new Point(615, 140),
                Enabled = false
            };
            Controls.Add(ckb_declinedWord);

            menuStrip1 = new MenuStrip();
            controlsToolStripMenuItem = new ToolStripMenuItem();
            saveAndCloseToolStripMenuItem = new ToolStripMenuItem();
            closeWithoutSavingToolStripMenuItem = new ToolStripMenuItem();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { controlsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1007, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // controlsToolStripMenuItem
            // 
            controlsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAndCloseToolStripMenuItem, closeWithoutSavingToolStripMenuItem });
            controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            controlsToolStripMenuItem.Size = new Size(64, 20);
            controlsToolStripMenuItem.Text = "Controls";
            // 
            // saveAndCloseToolStripMenuItem
            // 
            saveAndCloseToolStripMenuItem.Name = "saveAndCloseToolStripMenuItem";
            saveAndCloseToolStripMenuItem.Size = new Size(184, 22);
            saveAndCloseToolStripMenuItem.Text = "Save and Close";
            saveAndCloseToolStripMenuItem.Click += SaveAndCloseToolStripMenuItem_Click;
            // 
            // closeWithoutSavingToolStripMenuItem
            // 
            closeWithoutSavingToolStripMenuItem.Name = "closeWithoutSavingToolStripMenuItem";
            closeWithoutSavingToolStripMenuItem.Size = new Size(184, 22);
            closeWithoutSavingToolStripMenuItem.Text = "Close without saving";
            closeWithoutSavingToolStripMenuItem.Click += CloseWithoutSavingToolStripMenuItem_Click;

            menuStrip1.Items.Add(controlsToolStripMenuItem);

            CharacterInsertToolStripMenuItem ciMenu = new();
            _ = menuStrip1.Items.Add(ciMenu);
            
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "SoundMapListEditor";
            Text = "Lexicon Entry Editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void Txt_any_TextChanged(object? sender, EventArgs e)
        {
            dirty = true;
        }

        private void Txt_spelled_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                txt_spelled.TextChanged -= Txt_spelled_TextChanged;
                txt_phonetic.TextChanged -= Txt_phonetic_TextChanged;

                if (_soundMapList != null)
                {
                    txt_phonetic.Text = ConlangUtilities.SoundOutWord(txt_spelled.Text.Trim(), _soundMapList);
                }
            }
            finally
            {
                txt_spelled.TextChanged += Txt_spelled_TextChanged;
                txt_phonetic.TextChanged += Txt_phonetic_TextChanged;
                dirty = true;
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
                    txt_spelled.Text = ConlangUtilities.SpellWord(txt_phonetic.Text.Trim(), _soundMapList);
                }
            }
            finally
            {
                txt_spelled.TextChanged += Txt_spelled_TextChanged;
                txt_phonetic.TextChanged += Txt_phonetic_TextChanged;
                dirty = true;
            }
        }

        private void SaveAndCloseToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            LexiconEntrySaved = true;
            this.Close();
        }

        private void CloseWithoutSavingToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            LexiconEntrySaved = false;
            this.Close();
        }
    }
}
