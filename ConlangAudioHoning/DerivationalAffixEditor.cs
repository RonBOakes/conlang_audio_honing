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
    internal class DerivationalAffixEditor : UserControl
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
                if (rbn_fixed.Checked == true)
                {
                    if ((_affixRules.pronounciation_add == string.Empty) && (_affixRules.spelling_add == string.Empty))
                    {
                        _affixRules.pronounciation_add = null;
                        _affixRules.spelling_add = null;
                    }
                    else
                    {
                        _affixRules.pronounciation_add = txt_pronounciationAdd.Text.Trim();
                        _affixRules.spelling_add = txt_spellingAdd.Text.Trim();
                        _affixRules.pronounciation_regex = null;
                        _affixRules.spelling_regex = null;
                        _affixRules.t_pronounciation_add = null;
                        _affixRules.t_spelling_add = null;
                        _affixRules.f_pronounciation_add = null;
                        _affixRules.f_spelling_add = null;
                    }
                }
                else
                {
                    _affixRules.pronounciation_add = null;
                    _affixRules.spelling_add = null;
                    if ((_affixRules.pronounciation_regex == string.Empty) &&
                        (_affixRules.spelling_regex == string.Empty) &&
                        (_affixRules.t_pronounciation_add == string.Empty) &&
                        (_affixRules.t_spelling_add == string.Empty) &&
                        (_affixRules.f_pronounciation_add == string.Empty) &&
                        (_affixRules.f_spelling_add == string.Empty))
                    {
                        _affixRules.pronounciation_regex = null;
                        _affixRules.spelling_regex = null;
                        _affixRules.t_pronounciation_add = null;
                        _affixRules.t_spelling_add = null;
                        _affixRules.f_pronounciation_add = null;
                        _affixRules.f_spelling_add = null;
                    }
                    else
                    {
                        _affixRules.pronounciation_regex = txt_pronounciationRegex.Text.Trim();
                        _affixRules.spelling_regex = txt_spellingRegex.Text.Trim();
                        _affixRules.t_pronounciation_add = txt_tPronounciationAdd.Text.Trim();
                        _affixRules.t_spelling_add = txt_tSpellingAdd.Text.Trim();
                        _affixRules.f_pronounciation_add = txt_fPronounciationAdd.Text.Trim();
                        _affixRules.f_spelling_add = txt_fSpellingAdd.Text.Trim();
                    }
                }
                return _affixRules; ;
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
                    if (value.pronounciation_add != null)
                    {
                        // Presume fixed rule
                        rbn_fixed.Checked = true;
                        rbn_conditional.Checked = false;
                        txt_pronounciationAdd.Text = value.pronounciation_add;
                        txt_spellingAdd.Text = value.spelling_add;
                        txt_pronounciationRegex.Text = "";
                        txt_spellingRegex.Text = "";
                        txt_tPronounciationAdd.Text = "";
                        txt_tSpellingAdd.Text = "";
                        txt_fPronounciationAdd.Text = "";
                        txt_fSpellingAdd.Text = "";
                    }
                    else
                    {
                        // Presume conditional rule
                        rbn_fixed.Checked = false;
                        rbn_conditional.Checked = true;
                        txt_pronounciationAdd.Text = "";
                        txt_spellingAdd.Text = "";
                        txt_pronounciationRegex.Text = value.pronounciation_regex;
                        txt_spellingRegex.Text = value.spelling_regex;
                        txt_tPronounciationAdd.Text = value.t_pronounciation_add;
                        txt_tSpellingAdd.Text = value.t_spelling_add;
                        txt_fPronounciationAdd.Text = value.f_pronounciation_add;
                        txt_fSpellingAdd.Text = value.f_spelling_add;
                    }
                }
            }
        }

        private static readonly string[] _affixTypes =
        {
            "PREFIX",
            "SUFFIX",
        };

        private Label lbl_affixType;
        private Label lbl_pronounciationAdd;
        private Label lbl_spellingAdd;
        private Label lbl_pronounciationRegex;
        private Label lbl_spellingRegex;
        private Label lbl_tPronounciationAdd;
        private Label lbl_tSpellingAdd;
        private Label lbl_fPronounciationAdd;
        private Label lbl_fSpellingAdd;

        private ComboBox cmb_affixType;

        private GroupBox gb_ruleType;
        private RadioButton rbn_fixed;
        private RadioButton rbn_conditional;

        private TextBox txt_pronounciationAdd;
        private TextBox txt_spellingAdd;
        private TextBox txt_pronounciationRegex;
        private TextBox txt_spellingRegex;
        private TextBox txt_tPronounciationAdd;
        private TextBox txt_tSpellingAdd;
        private TextBox txt_fPronounciationAdd;
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

            lbl_affixType = new Label();
            lbl_affixType.Text = "Affix Type:";
            lbl_affixType.Location = new Point(5, 5);
            lbl_affixType.Size = new Size(200, 25);
            lbl_affixType.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_affixType);

            cmb_affixType = new ComboBox();
            cmb_affixType.Location = new Point(205, 5);
            cmb_affixType.Size = new Size(200, 25);
            cmb_affixType.Items.AddRange(_affixTypes);
            Controls.Add(cmb_affixType);

            gb_ruleType = new GroupBox();
            gb_ruleType.Location = new Point(5, 30);
            gb_ruleType.Size = new Size(500, 50);
            Controls.Add(gb_ruleType);

            rbn_fixed = new RadioButton();
            rbn_fixed.Text = "Fixed Rule";
            rbn_fixed.Location = new Point(5, 5);
            rbn_fixed.Size = new Size(200, 25);
            gb_ruleType.Controls.Add(rbn_fixed);

            rbn_conditional = new RadioButton();
            rbn_conditional.Text = "Conditional Rule";
            rbn_conditional.Location = new Point(205, 5);
            rbn_conditional.Size = new Size(200, 25);
            gb_ruleType.Controls.Add(rbn_conditional);

            rbn_fixed.Checked = true;
            rbn_conditional.Checked = false;

            lbl_pronounciationAdd = new Label();
            lbl_pronounciationAdd.Text = "Pronounciation Add:";
            lbl_pronounciationAdd.Location = new Point(5, 90);
            lbl_pronounciationAdd.Size = new Size(200, 25);
            lbl_pronounciationAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_pronounciationAdd);
            lbl_pronounciationAdd.Visible = true;

            txt_pronounciationAdd = new TextBox();
            txt_pronounciationAdd.Location = new Point(205, 90);
            txt_pronounciationAdd.Size = new Size(200, 25);
            Controls.Add(txt_pronounciationAdd);
            txt_pronounciationAdd.Visible = true;
            txt_pronounciationAdd.Enabled = true;

            lbl_pronounciationRegex = new Label();
            lbl_pronounciationRegex.Text = "Pronounciation Regex:";
            lbl_pronounciationRegex.Location = new Point(5, 90);
            lbl_pronounciationRegex.Size = new Size(200, 25);
            lbl_pronounciationRegex.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_pronounciationRegex);
            lbl_pronounciationRegex.Visible = false;

            txt_pronounciationRegex = new TextBox();
            txt_pronounciationRegex.Location = new Point(205, 90);
            txt_pronounciationRegex.Size = new Size(200, 25);
            Controls.Add(txt_pronounciationRegex);
            txt_pronounciationRegex.Visible = false;
            txt_pronounciationRegex.Enabled = false;

            lbl_spellingAdd = new Label();
            lbl_spellingAdd.Text = "Spelling Add:";
            lbl_spellingAdd.Location = new Point(5, 120);
            lbl_spellingAdd.Size = new Size(200, 25);
            lbl_spellingAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_spellingAdd);
            lbl_spellingAdd.Visible = true;

            txt_spellingAdd = new TextBox();
            txt_spellingAdd.Location = new Point(205, 120);
            txt_spellingAdd.Size = new Size(200, 25);
            Controls.Add(txt_spellingAdd);
            txt_spellingAdd.Visible = true;
            txt_spellingAdd.Enabled = true;

            lbl_spellingRegex = new Label();
            lbl_spellingRegex.Text = "Spelling Add:";
            lbl_spellingRegex.Location = new Point(5, 120);
            lbl_spellingRegex.Size = new Size(200, 25);
            lbl_spellingRegex.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_spellingRegex);
            lbl_spellingRegex.Visible = false;

            txt_spellingRegex = new TextBox();
            txt_spellingRegex.Location = new Point(205, 120);
            txt_spellingRegex.Size = new Size(200, 25);
            Controls.Add(txt_spellingRegex);
            txt_spellingRegex.Visible = true;
            txt_spellingRegex.Enabled = true;

            lbl_tPronounciationAdd = new Label();
            lbl_tPronounciationAdd.Text = "True Pronounciation Add:";
            lbl_tPronounciationAdd.Location = new Point(5, 150);
            lbl_tPronounciationAdd.Size = new Size(200, 25);
            lbl_tPronounciationAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_tPronounciationAdd);
            lbl_tPronounciationAdd.Visible = false;

            txt_tPronounciationAdd = new TextBox();
            txt_tPronounciationAdd.Location = new Point(205, 150);
            txt_tPronounciationAdd.Size = new Size(200, 25);
            Controls.Add(txt_tPronounciationAdd);
            txt_tPronounciationAdd.Visible = false;
            txt_tPronounciationAdd.Enabled = false;

            lbl_tSpellingAdd = new Label();
            lbl_tSpellingAdd.Text = "True Spelling Add:";
            lbl_tSpellingAdd.Location = new Point(5, 180);
            lbl_tSpellingAdd.Size = new Size(200, 25);
            lbl_tSpellingAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_tSpellingAdd);
            lbl_tSpellingAdd.Visible = false;

            txt_tSpellingAdd = new TextBox();
            txt_tSpellingAdd.Location = new Point(205, 180);
            txt_tSpellingAdd.Size = new Size(200, 25);
            Controls.Add(txt_tSpellingAdd);
            txt_tSpellingAdd.Visible = false;
            txt_tSpellingAdd.Enabled = false;

            lbl_fPronounciationAdd = new Label();
            lbl_fPronounciationAdd.Text = "False Pronounciation Add:";
            lbl_fPronounciationAdd.Location = new Point(5, 210);
            lbl_fPronounciationAdd.Size = new Size(200, 25);
            lbl_fPronounciationAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_fPronounciationAdd);
            lbl_fPronounciationAdd.Visible = false;

            txt_fPronounciationAdd = new TextBox();
            txt_fPronounciationAdd.Location = new Point(205, 210);
            txt_fPronounciationAdd.Size = new Size(200, 25);
            Controls.Add(txt_fPronounciationAdd);
            txt_fPronounciationAdd.Visible = false;
            txt_fPronounciationAdd.Enabled = false;

            lbl_fSpellingAdd = new Label();
            lbl_fSpellingAdd.Text = "False Spelling Add:";
            lbl_fSpellingAdd.Location = new Point(5, 240);
            lbl_fSpellingAdd.Size = new Size(200, 25);
            lbl_fSpellingAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_fSpellingAdd);
            lbl_fSpellingAdd.Visible = false;

            txt_fSpellingAdd = new TextBox();
            txt_fSpellingAdd.Location = new Point(205, 240);
            txt_fSpellingAdd.Size = new Size(200, 25);
            Controls.Add(txt_fSpellingAdd);
            txt_fSpellingAdd.Visible = false;
            txt_fSpellingAdd.Enabled = false;

            rbn_fixed.CheckedChanged += Rbn_fixed_CheckedChanged;
        }

        private void Rbn_fixed_CheckedChanged(object? sender, EventArgs e)
        {
            if (rbn_fixed.Checked == true)
            {
                this.SuspendLayout();
                lbl_pronounciationAdd.Visible = true;
                txt_pronounciationAdd.Visible = true;
                txt_pronounciationAdd.Enabled = true;
                lbl_spellingAdd.Visible = true;
                txt_spellingAdd.Visible = true;
                lbl_pronounciationRegex.Visible = false;
                txt_pronounciationRegex.Visible = false;
                txt_pronounciationRegex.Enabled = false;
                lbl_spellingRegex.Visible = false;
                txt_spellingRegex.Visible = false;
                txt_spellingRegex.Enabled = false;
                lbl_tPronounciationAdd.Visible = false;
                txt_tPronounciationAdd.Visible = false;
                txt_tPronounciationAdd.Enabled = false;
                lbl_tSpellingAdd.Visible = false;
                txt_tSpellingAdd.Visible = false;
                txt_tSpellingAdd.Enabled = false;
                lbl_fPronounciationAdd.Visible = false;
                txt_fPronounciationAdd.Visible = false;
                txt_fPronounciationAdd.Enabled = false;
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
                lbl_pronounciationAdd.Visible = false;
                txt_pronounciationAdd.Visible = false;
                txt_pronounciationAdd.Enabled = false;
                lbl_spellingAdd.Visible = false;
                txt_spellingAdd.Visible = false;
                lbl_pronounciationRegex.Visible = true;
                txt_pronounciationRegex.Visible = true;
                txt_pronounciationRegex.Enabled = true;
                lbl_spellingRegex.Visible = true;
                txt_spellingRegex.Visible = true;
                txt_spellingRegex.Enabled = true;
                lbl_tPronounciationAdd.Visible = true;
                txt_tPronounciationAdd.Visible = true;
                txt_tPronounciationAdd.Enabled = true;
                lbl_tSpellingAdd.Visible = true;
                txt_tSpellingAdd.Visible = true;
                txt_tSpellingAdd.Enabled = true;
                lbl_fPronounciationAdd.Visible = true;
                txt_fPronounciationAdd.Visible = true;
                txt_fPronounciationAdd.Enabled = true;
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
