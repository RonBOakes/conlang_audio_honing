﻿/*
 * User Control for editing the Declension Affixes.
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
using ConlangAudioHoning;
using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEditor
{
    internal class DeclensionAffixEditor : Panel
    {

        private Size controlSize = new(850, 270);

        private Affix? _affixRules;

        private bool declensionChanged = false;
        private bool affixRulesChanged = false;

        public string Declension
        {
            get
            {
                declensionChanged = false;
                return txt_declension.Text ??= string.Empty;
            }
            set
            {
                txt_declension.Text = value;
                declensionChanged = false;
            }
        }

        public Affix AffixRules
        {
            get
            {
                _affixRules = new Affix();
                if (rbn_fixed.Checked)
                {
                    if ((_affixRules.pronunciation_add == string.Empty) && (_affixRules.spelling_add == string.Empty))
                    {
                        _affixRules.pronunciation_add = null;
                        _affixRules.spelling_add = null;
                    }
                    else
                    {
                        _affixRules.pronunciation_add = txt_pronunciationAdd.Text.Trim();
                        _affixRules.spelling_add = txt_spellingAdd.Text.Trim();
                    }
                    _affixRules.pronunciation_regex = null;
                    _affixRules.spelling_regex = null;
                    _affixRules.t_pronunciation_add = null;
                    _affixRules.t_spelling_add = null;
                    _affixRules.f_pronunciation_add = null;
                    _affixRules.f_spelling_add = null;
                }
                else
                {
                    _affixRules.pronunciation_add = null;
                    _affixRules.spelling_add = null;
                    if ((_affixRules.pronunciation_regex == string.Empty) &&
                        (_affixRules.spelling_regex == string.Empty) &&
                        (_affixRules.t_pronunciation_add == string.Empty) &&
                        (_affixRules.t_spelling_add == string.Empty) &&
                        (_affixRules.f_pronunciation_add == string.Empty) &&
                        (_affixRules.f_spelling_add == string.Empty))
                    {
                        _affixRules.pronunciation_regex = null;
                        _affixRules.spelling_regex = null;
                        _affixRules.t_pronunciation_add = null;
                        _affixRules.t_spelling_add = null;
                        _affixRules.f_pronunciation_add = null;
                        _affixRules.f_spelling_add = null;
                    }
                    else
                    {
                        _affixRules.pronunciation_regex = txt_pronunciationRegex.Text.Trim();
                        _affixRules.spelling_regex = txt_spellingRegex.Text.Trim();
                        _affixRules.t_pronunciation_add = txt_tPronunciationAdd.Text.Trim();
                        _affixRules.t_spelling_add = txt_tSpellingAdd.Text.Trim();
                        _affixRules.f_pronunciation_add = txt_fPronunciationAdd.Text.Trim();
                        _affixRules.f_spelling_add = txt_fSpellingAdd.Text.Trim();
                    }
                }
                affixRulesChanged = false;
                return _affixRules;
            }
            set
            {
                _affixRules = value;
                if (value != null)
                {
                    if (value.pronunciation_add != null)
                    {
                        // Presume fixed rule
                        rbn_fixed.Checked = true;
                        rbn_conditional.Checked = false;
                        txt_pronunciationAdd.Text = value.pronunciation_add;
                        txt_spellingAdd.Text = value.spelling_add;
                        txt_pronunciationRegex.Text = "";
                        txt_spellingRegex.Text = "";
                        txt_tPronunciationAdd.Text = "";
                        txt_tSpellingAdd.Text = "";
                        txt_fPronunciationAdd.Text = "";
                        txt_fSpellingAdd.Text = "";
                    }
                    else
                    {
                        // Presume conditional rule
                        rbn_fixed.Checked = false;
                        rbn_conditional.Checked = true;
                        txt_pronunciationAdd.Text = "";
                        txt_spellingAdd.Text = "";
                        txt_pronunciationRegex.Text = value.pronunciation_regex;
                        txt_spellingRegex.Text = value.spelling_regex;
                        txt_tPronunciationAdd.Text = value.t_pronunciation_add;
                        txt_tSpellingAdd.Text = value.t_spelling_add;
                        txt_fPronunciationAdd.Text = value.f_pronunciation_add;
                        txt_fSpellingAdd.Text = value.f_spelling_add;
                    }
                }
                affixRulesChanged = false;
            }
        }

        public bool DeclensionChanged
        {
            get
            {
                return declensionChanged;
            }
            private set
            {
                declensionChanged = value;
            }
        }

        public bool AffixRulesChanged
        {
            get
            {
                return affixRulesChanged;
            }
            private set
            {
                affixRulesChanged = value;
            }
        }

#pragma warning disable S1450
        private Label lbl_declension;
        private Label lbl_pronunciationAdd;
        private Label lbl_spellingAdd;
        private Label lbl_pronunciationRegex;
        private Label lbl_spellingRegex;
        private Label lbl_tpronunciationAdd;
        private Label lbl_tSpellingAdd;
        private Label lbl_fPronunciationAdd;
        private Label lbl_fSpellingAdd;

        private GroupBox gb_ruleType;
        private RadioButton rbn_fixed;
        private RadioButton rbn_conditional;

        private TextBox txt_declension;
        private TextBox txt_pronunciationAdd;
        private TextBox txt_spellingAdd;
        private TextBox txt_pronunciationRegex;
        private TextBox txt_spellingRegex;
        private TextBox txt_tPronunciationAdd;
        private TextBox txt_tSpellingAdd;
        private TextBox txt_fPronunciationAdd;
        private TextBox txt_fSpellingAdd;

        private MenuStrip menuStrip1;

        private TextBox _lastFocused;

#pragma warning restore S1450

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DeclensionAffixEditor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
        }



        private void InitializeComponent()
        {
            this.Size = controlSize;
            this.BorderStyle = BorderStyle.None;

            lbl_declension = new Label
            {
                Text = "Declension:",
                Location = new Point(5, 25),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_declension);

            txt_declension = new TextBox
            {
                Location = new Point(205, 25),
                Size = new Size(200, 25)
            };
            txt_declension.GotFocus += Txt_GotFocus;
            Controls.Add(txt_declension);

            gb_ruleType = new GroupBox
            {
                Location = new Point(5, 50),
                Size = new Size(500, 50)
            };
            Controls.Add(gb_ruleType);

            rbn_fixed = new RadioButton
            {
                Text = "Fixed Rule",
                Location = new Point(5, 5),
                Size = new Size(200, 25)
            };
            gb_ruleType.Controls.Add(rbn_fixed);

            rbn_conditional = new RadioButton
            {
                Text = "Conditional Rule",
                Location = new Point(205, 5),
                Size = new Size(200, 25)
            };
            gb_ruleType.Controls.Add(rbn_conditional);

            rbn_fixed.Checked = true;
            rbn_conditional.Checked = false;

            lbl_pronunciationAdd = new Label
            {
                Text = "pronunciation Add:",
                Location = new Point(5, 110),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_pronunciationAdd);
            lbl_pronunciationAdd.Visible = true;

            txt_pronunciationAdd = new TextBox
            {
                Location = new Point(205, 110),
                Size = new Size(200, 25)
            };
            txt_pronunciationAdd.GotFocus += Txt_GotFocus;
            Controls.Add(txt_pronunciationAdd);
            txt_pronunciationAdd.Visible = true;
            txt_pronunciationAdd.Enabled = true;

            lbl_pronunciationRegex = new Label
            {
                Text = "pronunciation Regex:",
                Location = new Point(5, 110),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_pronunciationRegex);
            lbl_pronunciationRegex.Visible = false;

            txt_pronunciationRegex = new TextBox
            {
                Location = new Point(205, 110),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_pronunciationRegex);
            txt_pronunciationRegex.GotFocus += Txt_GotFocus;
            txt_pronunciationRegex.Visible = false;
            txt_pronunciationRegex.Enabled = false;

            lbl_spellingAdd = new Label
            {
                Text = "Spelling Add:",
                Location = new Point(5, 140),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_spellingAdd);
            lbl_spellingAdd.Visible = true;

            txt_spellingAdd = new TextBox
            {
                Location = new Point(205, 140),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_spellingAdd);
            txt_spellingAdd.GotFocus += Txt_GotFocus;
            txt_spellingAdd.Visible = true;
            txt_spellingAdd.Enabled = true;

            lbl_spellingRegex = new Label
            {
                Text = "Spelling Add:",
                Location = new Point(5, 140),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_spellingRegex);
            lbl_spellingRegex.Visible = false;

            txt_spellingRegex = new TextBox
            {
                Location = new Point(205, 140),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_spellingRegex);
            txt_spellingRegex.GotFocus += Txt_GotFocus;
            txt_spellingRegex.Visible = true;
            txt_spellingRegex.Enabled = true;

            lbl_tpronunciationAdd = new Label
            {
                Text = "True pronunciation Add:",
                Location = new Point(5, 170),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_tpronunciationAdd);
            lbl_tpronunciationAdd.Visible = false;

            txt_tPronunciationAdd = new TextBox
            {
                Location = new Point(205, 170),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_tPronunciationAdd);
            txt_tPronunciationAdd.GotFocus += Txt_GotFocus;
            txt_tPronunciationAdd.Visible = false;
            txt_tPronunciationAdd.Enabled = false;

            lbl_tSpellingAdd = new Label
            {
                Text = "True Spelling Add:",
                Location = new Point(5, 200),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_tSpellingAdd);
            lbl_tSpellingAdd.Visible = false;

            txt_tSpellingAdd = new TextBox
            {
                Location = new Point(205, 200),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_tSpellingAdd);
            txt_tSpellingAdd.GotFocus += Txt_GotFocus;
            txt_tSpellingAdd.Visible = false;
            txt_tSpellingAdd.Enabled = false;

            lbl_fPronunciationAdd = new Label
            {
                Text = "False pronunciation Add:",
                Location = new Point(5, 230),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_fPronunciationAdd);
            lbl_fPronunciationAdd.Visible = false;

            txt_fPronunciationAdd = new TextBox
            {
                Location = new Point(205, 230),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_fPronunciationAdd);
            txt_fPronunciationAdd.GotFocus += Txt_GotFocus;
            txt_fPronunciationAdd.Visible = false;
            txt_fPronunciationAdd.Enabled = false;

            lbl_fSpellingAdd = new Label
            {
                Text = "False Spelling Add:",
                Location = new Point(5, 270),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_fSpellingAdd);
            lbl_fSpellingAdd.Visible = false;

            txt_fSpellingAdd = new TextBox
            {
                Location = new Point(205, 270),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_fSpellingAdd);
            txt_fSpellingAdd.GotFocus += Txt_GotFocus;
            txt_fSpellingAdd.Visible = false;
            txt_fSpellingAdd.Enabled = false;

            rbn_fixed.CheckedChanged += Rbn_fixed_CheckedChanged;
            txt_declension.TextChanged += Txt_declension_TextChanged;
            txt_pronunciationAdd.TextChanged += Txt_TextChanged;
            txt_pronunciationRegex.TextChanged += Txt_TextChanged;
            txt_spellingAdd.TextChanged += Txt_TextChanged;
            txt_spellingRegex.TextChanged += Txt_TextChanged;
            txt_tPronunciationAdd.TextChanged += Txt_TextChanged;
            txt_tSpellingAdd.TextChanged += Txt_TextChanged;
            txt_fPronunciationAdd.TextChanged += Txt_TextChanged;
            txt_fSpellingAdd.TextChanged += Txt_TextChanged;

            menuStrip1 = new MenuStrip();
            menuStrip1.SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1007, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";

            CharacterInsertToolStripMenuItem ciMenu = new();
            _ = menuStrip1.Items.Add(ciMenu);
            menuStrip1.ResumeLayout(true);
            ciMenu.AddClickDelegate(CharInsetToolStripMenuItem_Click);

            Controls.Add(menuStrip1);
        }

        private void Txt_declension_TextChanged(object? sender, EventArgs e)
        {
            DeclensionChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Rbn_fixed_CheckedChanged(object? sender, EventArgs e)
        {
            if (rbn_fixed.Checked)
            {
                this.SuspendLayout();
                lbl_pronunciationAdd.Visible = true;
                txt_pronunciationAdd.Visible = true;
                txt_pronunciationAdd.Enabled = true;
                lbl_spellingAdd.Visible = true;
                txt_spellingAdd.Visible = true;
                lbl_pronunciationRegex.Visible = false;
                txt_pronunciationRegex.Visible = false;
                txt_pronunciationRegex.Enabled = false;
                lbl_spellingRegex.Visible = false;
                txt_spellingRegex.Visible = false;
                txt_spellingRegex.Enabled = false;
                lbl_tpronunciationAdd.Visible = false;
                txt_tPronunciationAdd.Visible = false;
                txt_tPronunciationAdd.Enabled = false;
                lbl_tSpellingAdd.Visible = false;
                txt_tSpellingAdd.Visible = false;
                txt_tSpellingAdd.Enabled = false;
                lbl_fPronunciationAdd.Visible = false;
                txt_fPronunciationAdd.Visible = false;
                txt_fPronunciationAdd.Enabled = false;
                lbl_fSpellingAdd.Visible = false;
                txt_fSpellingAdd.Visible = false;
                txt_fSpellingAdd.Enabled = false;
                this.controlSize.Height = 180;
                this.Size = this.controlSize;
                this.ResumeLayout(true);
            }
            else
            {
                this.SuspendLayout();
                lbl_pronunciationAdd.Visible = false;
                txt_pronunciationAdd.Visible = false;
                txt_pronunciationAdd.Enabled = false;
                lbl_spellingAdd.Visible = false;
                txt_spellingAdd.Visible = false;
                lbl_pronunciationRegex.Visible = true;
                txt_pronunciationRegex.Visible = true;
                txt_pronunciationRegex.Enabled = true;
                lbl_spellingRegex.Visible = true;
                txt_spellingRegex.Visible = true;
                txt_spellingRegex.Enabled = true;
                lbl_tpronunciationAdd.Visible = true;
                txt_tPronunciationAdd.Visible = true;
                txt_tPronunciationAdd.Enabled = true;
                lbl_tSpellingAdd.Visible = true;
                txt_tSpellingAdd.Visible = true;
                txt_tSpellingAdd.Enabled = true;
                lbl_fPronunciationAdd.Visible = true;
                txt_fPronunciationAdd.Visible = true;
                txt_fPronunciationAdd.Enabled = true;
                lbl_fSpellingAdd.Visible = true;
                txt_fSpellingAdd.Visible = true;
                txt_fSpellingAdd.Enabled = true;
                this.controlSize.Height = 300;
                this.Size = this.controlSize;
                this.ResumeLayout(true);
            }
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
            PasteIntoFocusedBox(charToInsert);
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

        private void Txt_GotFocus(Object? sender, EventArgs e)
        {
            if (sender is TextBox box)
            {
                _lastFocused = box;
            }
        }

        public event EventHandler Changed;
    }
}
