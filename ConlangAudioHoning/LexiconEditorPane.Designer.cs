/*
 * Automatically generated code for the User Control to contain the entire lexicon 
 * for a conlang.
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
namespace LanguageEditor
{
    partial class LexiconEditorPane
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            grp_tabSelect = new GroupBox();
            rdo_spelled = new RadioButton();
            rdo_english = new RadioButton();
            tpn_lexiconEditors = new TabControl();
            btn_earlier = new Button();
            btn_later = new Button();
            grp_tabSelect.SuspendLayout();
            SuspendLayout();
            // 
            // grp_tabSelect
            // 
            grp_tabSelect.Controls.Add(rdo_spelled);
            grp_tabSelect.Controls.Add(rdo_english);
            grp_tabSelect.Location = new Point(42, 16);
            grp_tabSelect.Name = "grp_tabSelect";
            grp_tabSelect.Size = new Size(344, 46);
            grp_tabSelect.TabIndex = 0;
            grp_tabSelect.TabStop = false;
            grp_tabSelect.Text = "Sort/View By";
            // 
            // rdo_spelled
            // 
            rdo_spelled.AutoSize = true;
            rdo_spelled.Location = new Point(141, 20);
            rdo_spelled.Name = "rdo_spelled";
            rdo_spelled.Size = new Size(193, 19);
            rdo_spelled.TabIndex = 1;
            rdo_spelled.TabStop = true;
            rdo_spelled.Text = "Conlang Romanization/Spelling";
            rdo_spelled.UseVisualStyleBackColor = true;
            // 
            // rdo_english
            // 
            rdo_english.AutoSize = true;
            rdo_english.Location = new Point(17, 20);
            rdo_english.Name = "rdo_english";
            rdo_english.Size = new Size(118, 19);
            rdo_english.TabIndex = 0;
            rdo_english.TabStop = true;
            rdo_english.Text = "English Equivalent";
            rdo_english.UseVisualStyleBackColor = true;
            rdo_english.CheckedChanged += rdo_english_CheckedChanged;
            // 
            // tpn_lexiconEditors
            // 
            tpn_lexiconEditors.Location = new Point(3, 68);
            tpn_lexiconEditors.Name = "tpn_lexiconEditors";
            tpn_lexiconEditors.SelectedIndex = 0;
            tpn_lexiconEditors.Size = new Size(875, 279);
            tpn_lexiconEditors.TabIndex = 1;
            tpn_lexiconEditors.SelectedIndexChanged += tpn_lexiconEditors_SelectedIndexChanged;
            // 
            // btn_earlier
            // 
            btn_earlier.Location = new Point(403, 32);
            btn_earlier.Name = "btn_earlier";
            btn_earlier.Size = new Size(92, 23);
            btn_earlier.TabIndex = 2;
            btn_earlier.Text = "Earlier Entries";
            btn_earlier.UseVisualStyleBackColor = true;
            btn_earlier.Click += btn_earlier_Click;
            // 
            // btn_later
            // 
            btn_later.Location = new Point(501, 32);
            btn_later.Name = "btn_later";
            btn_later.Size = new Size(85, 23);
            btn_later.TabIndex = 3;
            btn_later.Text = "Later Entries";
            btn_later.UseVisualStyleBackColor = true;
            btn_later.Click += btn_later_Click;
            // 
            // LexiconEditorPane
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btn_later);
            Controls.Add(btn_earlier);
            Controls.Add(tpn_lexiconEditors);
            Controls.Add(grp_tabSelect);
            Name = "LexiconEditorPane";
            Size = new Size(890, 350);
            Load += LexiconEditorPane_Load;
            grp_tabSelect.ResumeLayout(false);
            grp_tabSelect.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grp_tabSelect;
        private RadioButton rdo_spelled;
        private RadioButton rdo_english;
        private TabControl tpn_lexiconEditors;
        private Button btn_earlier;
        private Button btn_later;
    }
}
