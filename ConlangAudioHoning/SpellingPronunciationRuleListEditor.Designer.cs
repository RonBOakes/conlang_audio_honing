/*
 * Designer code for the Sound Map Editor control
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
 */namespace ConlangAudioHoning
{
    partial class SpellingPronunciationRuleListEditor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ConlangJson.SpellingPronunciationRules soundMap2 = new ConlangJson.SpellingPronunciationRules();
            btnAddAbove = new Button();
            btnAddBelow = new Button();
            lbx_soundMapListEntries = new ListBox();
            soundMapEditor = new SpellingPronunciationRuleEditor();
            btnReplaceSelected = new Button();
            btnEditSelected = new Button();
            btnDeleteSelected = new Button();
            txtPhonemeReplacements = new TextBox();
            label1 = new Label();
            SuspendLayout();

            menuStrip1 = new MenuStrip();
            controlsToolStripMenuItem = new ToolStripMenuItem();
            saveAndCloseToolStripMenuItem = new ToolStripMenuItem();
            closeWithoutSavingToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
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

            CharacterInsertToolStripMenuItem ciMenu = new();
            _ = menuStrip1.Items.Add(ciMenu);
            ciMenu.AddClickDelegate(CharInsetToolStripMenuItem_Click);

            // 
            // btnAddAbove
            // 
            btnAddAbove.Location = new Point(718, 82);
            btnAddAbove.Name = "btnAddAbove";
            btnAddAbove.Size = new Size(127, 23);
            btnAddAbove.TabIndex = 3;
            btnAddAbove.Text = "Add Above Selected";
            btnAddAbove.UseVisualStyleBackColor = true;
            btnAddAbove.Click += BtnAddAbove_Click;
            // 
            // btnAddBelow
            // 
            btnAddBelow.Location = new Point(718, 140);
            btnAddBelow.Name = "btnAddBelow";
            btnAddBelow.Size = new Size(127, 23);
            btnAddBelow.TabIndex = 4;
            btnAddBelow.Text = "Add Below Selected";
            btnAddBelow.UseVisualStyleBackColor = true;
            btnAddBelow.Click += BtnAddBelow_Click;
            // 
            // lbx_soundMapListEntries
            // 
            lbx_soundMapListEntries.FormattingEnabled = true;
            lbx_soundMapListEntries.ItemHeight = 15;
            lbx_soundMapListEntries.Location = new Point(12, 168);
            lbx_soundMapListEntries.Name = "lbx_soundMapListEntries";
            lbx_soundMapListEntries.Size = new Size(700, 175);
            lbx_soundMapListEntries.ScrollAlwaysVisible = true;
            lbx_soundMapListEntries.TabIndex = 5;
            // 
            // soundMapEditor
            // 
            soundMapEditor.BorderStyle = BorderStyle.FixedSingle;
            soundMapEditor.Location = new Point(12, 82);
            soundMapEditor.Name = "soundMapEditor";
            soundMapEditor.Size = new Size(700, 80);
            soundMap2.phoneme = "";
            soundMap2.pronunciation_regex = "";
            soundMap2.romanization = "";
            soundMap2.spelling_regex = "";
            soundMapEditor.SoundMapData = soundMap2;
            soundMapEditor.TabIndex = 6;
            // 
            // btnReplaceSelected
            // 
            btnReplaceSelected.Location = new Point(718, 111);
            btnReplaceSelected.Name = "btnReplaceSelected";
            btnReplaceSelected.Size = new Size(127, 23);
            btnReplaceSelected.TabIndex = 8;
            btnReplaceSelected.Text = "Replace Selected";
            btnReplaceSelected.UseVisualStyleBackColor = true;
            btnReplaceSelected.Click += BtnReplaceSelected_Click;
            // 
            // btnEditSelected
            // 
            btnEditSelected.Location = new Point(718, 187);
            btnEditSelected.Name = "btnEditSelected";
            btnEditSelected.Size = new Size(127, 42);
            btnEditSelected.TabIndex = 9;
            btnEditSelected.Text = "Load Selected Into Editor";
            btnEditSelected.UseVisualStyleBackColor = true;
            btnEditSelected.Click += BtnEditSelected_Click;
            // 
            // btnDeleteSelected
            // 
            btnDeleteSelected.Location = new Point(718, 235);
            btnDeleteSelected.Name = "btnDeleteSelected";
            btnDeleteSelected.Size = new Size(127, 23);
            btnDeleteSelected.TabIndex = 10;
            btnDeleteSelected.Text = "Delete Selected Entry";
            btnDeleteSelected.UseVisualStyleBackColor = true;
            btnDeleteSelected.Click += BtnDeleteSelected_Click;
            // 
            // txtPhonemeReplacements
            // 
            txtPhonemeReplacements.Font = new Font("Charis SIL", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPhonemeReplacements.Location = new Point(163, 33);
            txtPhonemeReplacements.Name = "txtPhonemeReplacements";
            txtPhonemeReplacements.ReadOnly = true;
            txtPhonemeReplacements.Size = new Size(650, 34);
            txtPhonemeReplacements.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 42);
            label1.Name = "label1";
            label1.Size = new Size(145, 15);
            label1.TabIndex = 13;
            label1.Text = "Phoneme Being Replaced:";
            // 
            // SpellingPronounciationRuleListEditor
            // 
            Controls.Add(menuStrip1);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();

            ClientSize = new Size(895, 355);
            Controls.Add(label1);
            Controls.Add(txtPhonemeReplacements);
            Controls.Add(btnDeleteSelected);
            Controls.Add(btnEditSelected);
            Controls.Add(btnReplaceSelected);
            Controls.Add(soundMapEditor);
            Controls.Add(lbx_soundMapListEntries);
            Controls.Add(btnAddBelow);
            Controls.Add(btnAddAbove);
            Name = "SpellingPronounciationRuleListEditor";
            Text = "Spelling/Pronunciation List Editor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem controlsToolStripMenuItem;
        private ToolStripMenuItem saveAndCloseToolStripMenuItem;
        private ToolStripMenuItem closeWithoutSavingToolStripMenuItem;
        private Button btnAddAbove;
        private Button btnAddBelow;
        private ListBox lbx_soundMapListEntries;
        private SpellingPronunciationRuleEditor soundMapEditor;
        private Button btnReplaceSelected;
        private Button btnEditSelected;
        private Button btnDeleteSelected;
        private TextBox txtPhonemeReplacements;
        private Label label1;
    }
}