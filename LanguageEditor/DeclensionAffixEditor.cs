/*
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
using ConlangJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEditor
{
    internal class DeclensionAffixEditor : UserControl
    {
        private static List<SoundMap>? _soundMapList;

        private Size controlSize = new(850, 200);

        private string? _declension;
        private Affix? _affixRules;

        private bool declensionChanged = false;
        private bool affixRulesChanged = false;

        public string Declension
        {
            get
            {
                _declension = txt_declension.Text;
                declensionChanged = false;
                return _declension ??= string.Empty;
            }
            set
            {
                _declension = value;
                txt_declension.Text = value;
                declensionChanged = false;
            }
        }

        internal static List<SoundMap> SoundMapList
        {
            set
            {
                _soundMapList = value;
            }
        }

        public Affix AffixRules
        {
            get
            {
                _affixRules = new Affix();
                if (rbn_fixed.Checked == true)
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
                        _affixRules.t_pronunciation_add = txt_tpronunciationAdd.Text.Trim();
                        _affixRules.t_spelling_add = txt_tSpellingAdd.Text.Trim();
                        _affixRules.f_pronunciation_add = txt_fpronunciationAdd.Text.Trim();
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

        private Label lbl_declension;
        private Label lbl_pronunciationAdd;
        private Label lbl_spellingAdd;
        private Label lbl_pronunciationRegex;
        private Label lbl_spellingRegex;
        private Label lbl_tpronunciationAdd;
        private Label lbl_tSpellingAdd;
        private Label lbl_fpronunciationAdd;
        private Label lbl_fSpellingAdd;

        private GroupBox gb_ruleType;
        private RadioButton rbn_fixed;
        private RadioButton rbn_conditional;

        private TextBox txt_declension;
        private TextBox txt_pronunciationAdd;
        private TextBox txt_spellingAdd;
        private TextBox txt_pronunciationRegex;
        private TextBox txt_spellingRegex;
        private TextBox txt_tpronunciationAdd;
        private TextBox txt_tSpellingAdd;
        private TextBox txt_fpronunciationAdd;
        private TextBox txt_fSpellingAdd;

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

            lbl_declension = new Label();
            lbl_declension.Text = "Declension:";
            lbl_declension.Location = new Point(5, 5);
            lbl_declension.Size = new Size(200, 25);
            lbl_declension.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_declension);

            txt_declension = new TextBox();
            txt_declension.Location = new Point(205, 5);
            txt_declension.Size = new Size(200, 25);
            Controls.Add(txt_declension);

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

            lbl_pronunciationAdd = new Label();
            lbl_pronunciationAdd.Text = "pronunciation Add:";
            lbl_pronunciationAdd.Location = new Point(5, 90);
            lbl_pronunciationAdd.Size = new Size(200, 25);
            lbl_pronunciationAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_pronunciationAdd);
            lbl_pronunciationAdd.Visible = true;

            txt_pronunciationAdd = new TextBox();
            txt_pronunciationAdd.Location = new Point(205, 90);
            txt_pronunciationAdd.Size = new Size(200, 25);
            Controls.Add(txt_pronunciationAdd);
            txt_pronunciationAdd.Visible = true;
            txt_pronunciationAdd.Enabled = true;

            lbl_pronunciationRegex = new Label();
            lbl_pronunciationRegex.Text = "pronunciation Regex:";
            lbl_pronunciationRegex.Location = new Point(5, 90);
            lbl_pronunciationRegex.Size = new Size(200, 25);
            lbl_pronunciationRegex.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_pronunciationRegex);
            lbl_pronunciationRegex.Visible = false;

            txt_pronunciationRegex = new TextBox();
            txt_pronunciationRegex.Location = new Point(205, 90);
            txt_pronunciationRegex.Size = new Size(200, 25);
            Controls.Add(txt_pronunciationRegex);
            txt_pronunciationRegex.Visible = false;
            txt_pronunciationRegex.Enabled = false;

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

            lbl_tpronunciationAdd = new Label();
            lbl_tpronunciationAdd.Text = "True pronunciation Add:";
            lbl_tpronunciationAdd.Location = new Point(5, 150);
            lbl_tpronunciationAdd.Size = new Size(200, 25);
            lbl_tpronunciationAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_tpronunciationAdd);
            lbl_tpronunciationAdd.Visible = false;

            txt_tpronunciationAdd = new TextBox();
            txt_tpronunciationAdd.Location = new Point(205, 150);
            txt_tpronunciationAdd.Size = new Size(200, 25);
            Controls.Add(txt_tpronunciationAdd);
            txt_tpronunciationAdd.Visible = false;
            txt_tpronunciationAdd.Enabled = false;

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

            lbl_fpronunciationAdd = new Label();
            lbl_fpronunciationAdd.Text = "False pronunciation Add:";
            lbl_fpronunciationAdd.Location = new Point(5, 210);
            lbl_fpronunciationAdd.Size = new Size(200, 25);
            lbl_fpronunciationAdd.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(lbl_fpronunciationAdd);
            lbl_fpronunciationAdd.Visible = false;

            txt_fpronunciationAdd = new TextBox();
            txt_fpronunciationAdd.Location = new Point(205, 210);
            txt_fpronunciationAdd.Size = new Size(200, 25);
            Controls.Add(txt_fpronunciationAdd);
            txt_fpronunciationAdd.Visible = false;
            txt_fpronunciationAdd.Enabled = false;

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
            txt_declension.TextChanged += Txt_declension_TextChanged;
            txt_pronunciationAdd.TextChanged += Txt_pronunciationAdd_TextChanged;
            txt_pronunciationRegex.TextChanged += Txt_pronunciationRegex_TextChanged;
            txt_spellingAdd.TextChanged += Txt_spellingAdd_TextChanged;
            txt_spellingRegex.TextChanged += Txt_spellingRegex_TextChanged;
            txt_tpronunciationAdd.TextChanged += Txt_tpronunciationAdd_TextChanged;
            txt_tSpellingAdd.TextChanged += Txt_tSpellingAdd_TextChanged;
            txt_fpronunciationAdd.TextChanged += Txt_fpronunciationAdd_TextChanged;
            txt_fSpellingAdd.TextChanged += Txt_fSpellingAdd_TextChanged;

        }

        private void Txt_declension_TextChanged(object? sender, EventArgs e)
        {
            DeclensionChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_pronunciationRegex_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_pronunciationAdd_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_spellingRegex_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_spellingAdd_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_tpronunciationAdd_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_tSpellingAdd_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_fpronunciationAdd_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Txt_fSpellingAdd_TextChanged(object? sender, EventArgs e)
        {
            AffixRulesChanged = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Rbn_fixed_CheckedChanged(object? sender, EventArgs e)
        {
            if (rbn_fixed.Checked == true)
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

        public event EventHandler Changed;
    }
}
