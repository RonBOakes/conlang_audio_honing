/*
 * User Control for editing the Derivational Affix Entries
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEditor
{
    internal class DerivationalAffixEditor : Panel
    {
        private Size controlSize = new(500, 200);

        private DerivationalAffix? _affixRules;

        public DerivationalAffix AffixRules
        {
            get
            {
                _affixRules = new DerivationalAffix();
                if (cmb_affixType.SelectedIndex == 0)
                {
                    _affixRules.type = "PREFIX";
                }
                else
                {
                    _affixRules.type = "SUFFIX";
                }
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
                        _affixRules.pronunciation_regex = null;
                        _affixRules.spelling_regex = null;
                        _affixRules.t_pronunciation_add = null;
                        _affixRules.t_spelling_add = null;
                        _affixRules.f_pronunciation_add = null;
                        _affixRules.f_spelling_add = null;
                    }
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
                        _affixRules.t_pronunciation_add = txt_tpronunciationAdd.Text.Trim();
                        _affixRules.t_spelling_add = txt_tSpellingAdd.Text.Trim();
                        _affixRules.f_pronunciation_add = txt_fpronunciationAdd.Text.Trim();
                        _affixRules.f_spelling_add = txt_fSpellingAdd.Text.Trim();
                    }
                }
                return _affixRules;
            }
            set
            {
                _affixRules = value;
                if (value != null)
                {
                    if (value.type == "PREFIX")
                    {
                        cmb_affixType.SelectedIndex = 0;
                    }
                    else
                    {
                        cmb_affixType.SelectedIndex = 1;
                    }
                    if (value.pronunciation_add != null)
                    {
                        // Presume fixed rule
                        rbn_fixed.Checked = true;
                        rbn_conditional.Checked = false;
                        txt_pronunciationAdd.Text = value.pronunciation_add;
                        txt_spellingAdd.Text = value.spelling_add;
                        txt_pronunciationRegex.Text = "";
                        txt_spellingRegex.Text = "";
                        txt_tpronunciationAdd.Text = "";
                        txt_tSpellingAdd.Text = "";
                        txt_fpronunciationAdd.Text = "";
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
                        txt_tpronunciationAdd.Text = value.t_pronunciation_add;
                        txt_tSpellingAdd.Text = value.t_spelling_add;
                        txt_fpronunciationAdd.Text = value.f_pronunciation_add;
                        txt_fSpellingAdd.Text = value.f_spelling_add;
                    }
                }
            }
        }

        private static readonly string[] _affixTypes =
        [
            "PREFIX",
            "SUFFIX",
        ];

        private Label lbl_affixType;
        private Label lbl_pronunciationAdd;
        private Label lbl_spellingAdd;
        private Label lbl_pronunciationRegex;
        private Label lbl_spellingRegex;
        private Label lbl_tpronunciationAdd;
        private Label lbl_tSpellingAdd;
        private Label lbl_fpronunciationAdd;
        private Label lbl_fSpellingAdd;

        private ComboBox cmb_affixType;

        private GroupBox gb_ruleType;
        private RadioButton rbn_fixed;
        private RadioButton rbn_conditional;

        private TextBox txt_pronunciationAdd;
        private TextBox txt_spellingAdd;
        private TextBox txt_pronunciationRegex;
        private TextBox txt_spellingRegex;
        private TextBox txt_tpronunciationAdd;
        private TextBox txt_tSpellingAdd;
        private TextBox txt_fpronunciationAdd;
        private TextBox txt_fSpellingAdd;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DerivationalAffixEditor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
        }



        private void InitializeComponent()
        {
            this.Size = controlSize;
            this.BorderStyle = BorderStyle.None;

            lbl_affixType = new Label
            {
                Text = "Affix Type:",
                Location = new Point(5, 5),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_affixType);

            cmb_affixType = new ComboBox
            {
                Location = new Point(205, 5),
                Size = new Size(200, 25)
            };
            cmb_affixType.Items.AddRange(_affixTypes);
            Controls.Add(cmb_affixType);

            gb_ruleType = new GroupBox
            {
                Location = new Point(5, 30),
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
                Location = new Point(5, 90),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_pronunciationAdd);
            lbl_pronunciationAdd.Visible = true;

            txt_pronunciationAdd = new TextBox
            {
                Location = new Point(205, 90),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_pronunciationAdd);
            txt_pronunciationAdd.Visible = true;
            txt_pronunciationAdd.Enabled = true;

            lbl_pronunciationRegex = new Label
            {
                Text = "pronunciation Regex:",
                Location = new Point(5, 90),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_pronunciationRegex);
            lbl_pronunciationRegex.Visible = false;

            txt_pronunciationRegex = new TextBox
            {
                Location = new Point(205, 90),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_pronunciationRegex);
            txt_pronunciationRegex.Visible = false;
            txt_pronunciationRegex.Enabled = false;

            lbl_spellingAdd = new Label
            {
                Text = "Spelling Add:",
                Location = new Point(5, 120),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_spellingAdd);
            lbl_spellingAdd.Visible = true;

            txt_spellingAdd = new TextBox
            {
                Location = new Point(205, 120),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_spellingAdd);
            txt_spellingAdd.Visible = true;
            txt_spellingAdd.Enabled = true;

            lbl_spellingRegex = new Label
            {
                Text = "Spelling Add:",
                Location = new Point(5, 120),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_spellingRegex);
            lbl_spellingRegex.Visible = false;

            txt_spellingRegex = new TextBox
            {
                Location = new Point(205, 120),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_spellingRegex);
            txt_spellingRegex.Visible = true;
            txt_spellingRegex.Enabled = true;

            lbl_tpronunciationAdd = new Label
            {
                Text = "True pronunciation Add:",
                Location = new Point(5, 150),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_tpronunciationAdd);
            lbl_tpronunciationAdd.Visible = false;

            txt_tpronunciationAdd = new TextBox
            {
                Location = new Point(205, 150),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_tpronunciationAdd);
            txt_tpronunciationAdd.Visible = false;
            txt_tpronunciationAdd.Enabled = false;

            lbl_tSpellingAdd = new Label
            {
                Text = "True Spelling Add:",
                Location = new Point(5, 180),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_tSpellingAdd);
            lbl_tSpellingAdd.Visible = false;

            txt_tSpellingAdd = new TextBox
            {
                Location = new Point(205, 180),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_tSpellingAdd);
            txt_tSpellingAdd.Visible = false;
            txt_tSpellingAdd.Enabled = false;

            lbl_fpronunciationAdd = new Label
            {
                Text = "False pronunciation Add:",
                Location = new Point(5, 210),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_fpronunciationAdd);
            lbl_fpronunciationAdd.Visible = false;

            txt_fpronunciationAdd = new TextBox
            {
                Location = new Point(205, 210),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_fpronunciationAdd);
            txt_fpronunciationAdd.Visible = false;
            txt_fpronunciationAdd.Enabled = false;

            lbl_fSpellingAdd = new Label
            {
                Text = "False Spelling Add:",
                Location = new Point(5, 240),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lbl_fSpellingAdd);
            lbl_fSpellingAdd.Visible = false;

            txt_fSpellingAdd = new TextBox
            {
                Location = new Point(205, 240),
                Size = new Size(200, 25)
            };
            Controls.Add(txt_fSpellingAdd);
            txt_fSpellingAdd.Visible = false;
            txt_fSpellingAdd.Enabled = false;

            rbn_fixed.CheckedChanged += Rbn_fixed_CheckedChanged;
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
                txt_tpronunciationAdd.Visible = false;
                txt_tpronunciationAdd.Enabled = false;
                lbl_tSpellingAdd.Visible = false;
                txt_tSpellingAdd.Visible = false;
                txt_tSpellingAdd.Enabled = false;
                lbl_fpronunciationAdd.Visible = false;
                txt_fpronunciationAdd.Visible = false;
                txt_fpronunciationAdd.Enabled = false;
                lbl_fSpellingAdd.Visible = false;
                txt_fSpellingAdd.Visible = false;
                txt_fSpellingAdd.Enabled = false;
                this.controlSize.Height = 150;
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
                txt_tpronunciationAdd.Visible = true;
                txt_tpronunciationAdd.Enabled = true;
                lbl_tSpellingAdd.Visible = true;
                txt_tSpellingAdd.Visible = true;
                txt_tSpellingAdd.Enabled = true;
                lbl_fpronunciationAdd.Visible = true;
                txt_fpronunciationAdd.Visible = true;
                txt_fpronunciationAdd.Enabled = true;
                lbl_fSpellingAdd.Visible = true;
                txt_fSpellingAdd.Visible = true;
                txt_fSpellingAdd.Enabled = true;
                this.controlSize.Height = 270;
                this.Size = this.controlSize;
                this.ResumeLayout(true);
            }

        }
    }
}
