/*
 * Automatically generated code for the top level conlang editor
 
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
    partial class LanguageEditorForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbl_languageNameEnglish = new Label();
            txt_languageNameEnglish = new TextBox();
            lbl_langageNativePhonetic = new Label();
            txt_languageNativePhonetic = new TextBox();
            lbl_languageNameNativeEnglish = new Label();
            txt_languageNameNativeEnglish = new TextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            panel_nounGender = new Panel();
            vScrollBar1 = new VScrollBar();
            lbl_nounGenderList = new Label();
            tpn_Main = new TabControl();
            tab_misc = new TabPage();
            panel_phonemeInventory = new Panel();
            lbl_phonemeInventory = new Label();
            panel_partsOfSpeechList = new Panel();
            lbl_partsOfSpeach = new Label();
            tab_soundMapList = new TabPage();
            tab_derivationalAffixMap = new TabPage();
            tpn_DerivationalAffixMap = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tab_derivedWordList = new TabPage();
            tab_declinsionAffixes = new TabPage();
            tab_lexicon = new TabPage();
            menuStrip1.SuspendLayout();
            panel_nounGender.SuspendLayout();
            tpn_Main.SuspendLayout();
            tab_misc.SuspendLayout();
            tab_derivationalAffixMap.SuspendLayout();
            tpn_DerivationalAffixMap.SuspendLayout();
            SuspendLayout();
            // 
            // lbl_languageNameEnglish
            // 
            lbl_languageNameEnglish.AutoSize = true;
            lbl_languageNameEnglish.Location = new Point(14, 35);
            lbl_languageNameEnglish.Margin = new Padding(4, 0, 4, 0);
            lbl_languageNameEnglish.Name = "lbl_languageNameEnglish";
            lbl_languageNameEnglish.Size = new Size(143, 15);
            lbl_languageNameEnglish.TabIndex = 0;
            lbl_languageNameEnglish.Text = "Language Name (English)";
            // 
            // txt_languageNameEnglish
            // 
            txt_languageNameEnglish.Location = new Point(197, 31);
            txt_languageNameEnglish.Margin = new Padding(4, 3, 4, 3);
            txt_languageNameEnglish.Name = "txt_languageNameEnglish";
            txt_languageNameEnglish.Size = new Size(363, 23);
            txt_languageNameEnglish.TabIndex = 1;
            // 
            // lbl_langageNativePhonetic
            // 
            lbl_langageNativePhonetic.AutoSize = true;
            lbl_langageNativePhonetic.Location = new Point(14, 66);
            lbl_langageNativePhonetic.Margin = new Padding(4, 0, 4, 0);
            lbl_langageNativePhonetic.Name = "lbl_langageNativePhonetic";
            lbl_langageNativePhonetic.Size = new Size(152, 15);
            lbl_langageNativePhonetic.TabIndex = 2;
            lbl_langageNativePhonetic.Text = "Language Name (Phonetic)";
            // 
            // txt_languageNativePhonetic
            // 
            txt_languageNativePhonetic.Location = new Point(197, 62);
            txt_languageNativePhonetic.Margin = new Padding(4, 3, 4, 3);
            txt_languageNativePhonetic.Name = "txt_languageNativePhonetic";
            txt_languageNativePhonetic.Size = new Size(363, 23);
            txt_languageNativePhonetic.TabIndex = 3;
            // 
            // lbl_languageNameNativeEnglish
            // 
            lbl_languageNameNativeEnglish.AutoSize = true;
            lbl_languageNameNativeEnglish.Location = new Point(13, 98);
            lbl_languageNameNativeEnglish.Margin = new Padding(4, 0, 4, 0);
            lbl_languageNameNativeEnglish.Name = "lbl_languageNameNativeEnglish";
            lbl_languageNameNativeEnglish.Size = new Size(164, 15);
            lbl_languageNameNativeEnglish.TabIndex = 4;
            lbl_languageNameNativeEnglish.Text = "Language Name (Romanized)";
            // 
            // txt_languageNameNativeEnglish
            // 
            txt_languageNameNativeEnglish.Location = new Point(197, 95);
            txt_languageNameNativeEnglish.Margin = new Padding(4, 3, 4, 3);
            txt_languageNameNativeEnglish.Name = "txt_languageNameNativeEnglish";
            txt_languageNameNativeEnglish.Size = new Size(363, 23);
            txt_languageNameNativeEnglish.TabIndex = 5;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(933, 24);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadToolStripMenuItem, saveToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(25, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(180, 22);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // panel_nounGender
            // 
            panel_nounGender.AutoScroll = true;
            panel_nounGender.Controls.Add(vScrollBar1);
            panel_nounGender.Location = new Point(567, 51);
            panel_nounGender.Name = "panel_nounGender";
            panel_nounGender.Size = new Size(345, 67);
            panel_nounGender.TabIndex = 7;
            // 
            // vScrollBar1
            // 
            vScrollBar1.Dock = DockStyle.Right;
            vScrollBar1.Location = new Point(328, 0);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(17, 67);
            vScrollBar1.TabIndex = 0;
            // 
            // lbl_nounGenderList
            // 
            lbl_nounGenderList.AutoSize = true;
            lbl_nounGenderList.Location = new Point(567, 31);
            lbl_nounGenderList.Name = "lbl_nounGenderList";
            lbl_nounGenderList.Size = new Size(83, 15);
            lbl_nounGenderList.TabIndex = 8;
            lbl_nounGenderList.Text = "Noun Genders";
            // 
            // tpn_Main
            // 
            tpn_Main.Controls.Add(tab_misc);
            tpn_Main.Controls.Add(tab_soundMapList);
            tpn_Main.Controls.Add(tab_derivationalAffixMap);
            tpn_Main.Controls.Add(tab_derivedWordList);
            tpn_Main.Controls.Add(tab_declinsionAffixes);
            tpn_Main.Controls.Add(tab_lexicon);
            tpn_Main.Location = new Point(18, 124);
            tpn_Main.Name = "tpn_Main";
            tpn_Main.SelectedIndex = 0;
            tpn_Main.Size = new Size(903, 383);
            tpn_Main.TabIndex = 9;
            // 
            // tab_misc
            // 
            tab_misc.Controls.Add(panel_phonemeInventory);
            tab_misc.Controls.Add(lbl_phonemeInventory);
            tab_misc.Controls.Add(panel_partsOfSpeechList);
            tab_misc.Controls.Add(lbl_partsOfSpeach);
            tab_misc.Location = new Point(4, 24);
            tab_misc.Name = "tab_misc";
            tab_misc.Padding = new Padding(3);
            tab_misc.Size = new Size(895, 355);
            tab_misc.TabIndex = 1;
            tab_misc.Text = "Misc.";
            tab_misc.UseVisualStyleBackColor = true;
            // 
            // panel_phonemeInventory
            // 
            panel_phonemeInventory.AutoScroll = true;
            panel_phonemeInventory.Location = new Point(429, 22);
            panel_phonemeInventory.Name = "panel_phonemeInventory";
            panel_phonemeInventory.Size = new Size(460, 328);
            panel_phonemeInventory.TabIndex = 7;
            // 
            // lbl_phonemeInventory
            // 
            lbl_phonemeInventory.AutoSize = true;
            lbl_phonemeInventory.Location = new Point(429, 4);
            lbl_phonemeInventory.Name = "lbl_phonemeInventory";
            lbl_phonemeInventory.Size = new Size(111, 15);
            lbl_phonemeInventory.TabIndex = 6;
            lbl_phonemeInventory.Text = "Phoneme Inventory";
            // 
            // panel_partsOfSpeechList
            // 
            panel_partsOfSpeechList.AutoScroll = true;
            panel_partsOfSpeechList.Location = new Point(6, 22);
            panel_partsOfSpeechList.Name = "panel_partsOfSpeechList";
            panel_partsOfSpeechList.Size = new Size(417, 328);
            panel_partsOfSpeechList.TabIndex = 5;
            // 
            // lbl_partsOfSpeach
            // 
            lbl_partsOfSpeach.AutoSize = true;
            lbl_partsOfSpeach.Location = new Point(6, 4);
            lbl_partsOfSpeach.Name = "lbl_partsOfSpeach";
            lbl_partsOfSpeach.Size = new Size(88, 15);
            lbl_partsOfSpeach.TabIndex = 4;
            lbl_partsOfSpeach.Text = "Parts of Speech";
            // 
            // tab_soundMapList
            // 
            tab_soundMapList.AutoScroll = true;
            tab_soundMapList.Location = new Point(4, 24);
            tab_soundMapList.Name = "tab_soundMapList";
            tab_soundMapList.Padding = new Padding(3);
            tab_soundMapList.Size = new Size(895, 355);
            tab_soundMapList.TabIndex = 0;
            tab_soundMapList.Text = "Sound Map (Spelling)";
            tab_soundMapList.UseVisualStyleBackColor = true;
            // 
            // tab_derivationalAffixMap
            // 
            tab_derivationalAffixMap.Controls.Add(tpn_DerivationalAffixMap);
            tab_derivationalAffixMap.Location = new Point(4, 24);
            tab_derivationalAffixMap.Name = "tab_derivationalAffixMap";
            tab_derivationalAffixMap.Padding = new Padding(3);
            tab_derivationalAffixMap.Size = new Size(895, 355);
            tab_derivationalAffixMap.TabIndex = 3;
            tab_derivationalAffixMap.Text = "Derivational Affixes";
            tab_derivationalAffixMap.UseVisualStyleBackColor = true;
            // 
            // tpn_DerivationalAffixMap
            // 
            tpn_DerivationalAffixMap.Controls.Add(tabPage1);
            tpn_DerivationalAffixMap.Controls.Add(tabPage2);
            tpn_DerivationalAffixMap.Location = new Point(3, 6);
            tpn_DerivationalAffixMap.Name = "tpn_DerivationalAffixMap";
            tpn_DerivationalAffixMap.SelectedIndex = 0;
            tpn_DerivationalAffixMap.Size = new Size(889, 346);
            tpn_DerivationalAffixMap.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(881, 318);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(881, 318);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tab_derivedWordList
            // 
            tab_derivedWordList.Location = new Point(4, 24);
            tab_derivedWordList.Name = "tab_derivedWordList";
            tab_derivedWordList.Padding = new Padding(3);
            tab_derivedWordList.Size = new Size(895, 355);
            tab_derivedWordList.TabIndex = 5;
            tab_derivedWordList.Text = "Derived Word Sources";
            tab_derivedWordList.UseVisualStyleBackColor = true;
            // 
            // tab_declinsionAffixes
            // 
            tab_declinsionAffixes.Location = new Point(4, 24);
            tab_declinsionAffixes.Name = "tab_declinsionAffixes";
            tab_declinsionAffixes.Padding = new Padding(3);
            tab_declinsionAffixes.Size = new Size(895, 355);
            tab_declinsionAffixes.TabIndex = 4;
            tab_declinsionAffixes.Text = "Declension Affixes";
            tab_declinsionAffixes.UseVisualStyleBackColor = true;
            // 
            // tab_lexicon
            // 
            tab_lexicon.AutoScroll = true;
            tab_lexicon.Location = new Point(4, 24);
            tab_lexicon.Name = "tab_lexicon";
            tab_lexicon.Padding = new Padding(3);
            tab_lexicon.Size = new Size(895, 355);
            tab_lexicon.TabIndex = 2;
            tab_lexicon.Text = "Lexicon";
            tab_lexicon.UseVisualStyleBackColor = true;
            // 
            // LanguageEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 519);
            Controls.Add(tpn_Main);
            Controls.Add(lbl_nounGenderList);
            Controls.Add(panel_nounGender);
            Controls.Add(txt_languageNameNativeEnglish);
            Controls.Add(lbl_languageNameNativeEnglish);
            Controls.Add(txt_languageNativePhonetic);
            Controls.Add(lbl_langageNativePhonetic);
            Controls.Add(txt_languageNameEnglish);
            Controls.Add(lbl_languageNameEnglish);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            Name = "LanguageEditorForm";
            Text = "Conlang Editor";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel_nounGender.ResumeLayout(false);
            tpn_Main.ResumeLayout(false);
            tab_misc.ResumeLayout(false);
            tab_misc.PerformLayout();
            tab_derivationalAffixMap.ResumeLayout(false);
            tpn_DerivationalAffixMap.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbl_languageNameEnglish;
        private System.Windows.Forms.TextBox txt_languageNameEnglish;
        private System.Windows.Forms.Label lbl_langageNativePhonetic;
        private System.Windows.Forms.TextBox txt_languageNativePhonetic;
        private System.Windows.Forms.Label lbl_languageNameNativeEnglish;
        private System.Windows.Forms.TextBox txt_languageNameNativeEnglish;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private Panel panel_nounGender;
        private VScrollBar vScrollBar1;
        private Label lbl_nounGenderList;
        private TabControl tpn_Main;
        private TabPage tab_soundMapList;
        private TabPage tab_misc;
        private Panel panel_phonemeInventory;
        private Label lbl_phonemeInventory;
        private Panel panel_partsOfSpeechList;
        private Label lbl_partsOfSpeach;
        private TabPage tab_lexicon;
        private TabPage tab_derivationalAffixMap;
        private TabControl tpn_DerivationalAffixMap;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tab_declinsionAffixes;
        private TabPage tab_derivedWordList;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}
