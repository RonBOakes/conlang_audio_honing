/*
 * Designer code for the Lexicon Editor
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
    partial class LexiconEditor
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
            lbxLexicon = new ListBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveAndCloseToolStripMenuItem = new ToolStripMenuItem();
            closeWithoutSavingToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            addEntryMenuItem = new ToolStripMenuItem();
            sortByToolStripMenuItem = new ToolStripMenuItem();
            conlangToolStripMenuItem = new ToolStripMenuItem();
            englishToolStripMenuItem = new ToolStripMenuItem();
            findToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lbxLexicon
            // 
            lbxLexicon.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbxLexicon.FormattingEnabled = true;
            lbxLexicon.ItemHeight = 21;
            lbxLexicon.Location = new Point(12, 34);
            lbxLexicon.Name = "lbxLexicon";
            lbxLexicon.Size = new Size(384, 403);
            lbxLexicon.TabIndex = 0;
            lbxLexicon.DoubleClick += LbxLexicon_DoubleClick;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(412, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAndCloseToolStripMenuItem, closeWithoutSavingToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveAndCloseToolStripMenuItem
            // 
            saveAndCloseToolStripMenuItem.Name = "saveAndCloseToolStripMenuItem";
            saveAndCloseToolStripMenuItem.Size = new Size(187, 22);
            saveAndCloseToolStripMenuItem.Text = "Save and Close";
            saveAndCloseToolStripMenuItem.Click += SaveAndCloseToolStripMenuItem_Click;
            // 
            // closeWithoutSavingToolStripMenuItem
            // 
            closeWithoutSavingToolStripMenuItem.Name = "closeWithoutSavingToolStripMenuItem";
            closeWithoutSavingToolStripMenuItem.Size = new Size(187, 22);
            closeWithoutSavingToolStripMenuItem.Text = "Close Without Saving";
            closeWithoutSavingToolStripMenuItem.Click += CloseWithoutSavingToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addEntryMenuItem, sortByToolStripMenuItem, findToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            //
            // addEntryMenuItem
            //
            addEntryMenuItem.Name = "addEntryMenuItem";
            addEntryMenuItem.Size = new Size(119, 22);
            addEntryMenuItem.Text = "Add Lexicon Entry";
            addEntryMenuItem.Click += AddEntryMenuItem_Click;
            // 
            // sortByToolStripMenuItem
            // 
            sortByToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { conlangToolStripMenuItem, englishToolStripMenuItem });
            sortByToolStripMenuItem.Name = "sortByToolStripMenuItem";
            sortByToolStripMenuItem.Size = new Size(180, 22);
            sortByToolStripMenuItem.Text = "Sort By";
            // 
            // conlangToolStripMenuItem
            // 
            conlangToolStripMenuItem.Name = "conlangToolStripMenuItem";
            conlangToolStripMenuItem.Size = new Size(119, 22);
            conlangToolStripMenuItem.Text = "Conlang";
            conlangToolStripMenuItem.Click += ConlangToolStripMenuItem_Click;
            // 
            // englishToolStripMenuItem
            // 
            englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            englishToolStripMenuItem.Size = new Size(119, 22);
            englishToolStripMenuItem.Text = "English";
            englishToolStripMenuItem.Click += EnglishToolStripMenuItem_Click;
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            findToolStripMenuItem.Size = new Size(180, 22);
            findToolStripMenuItem.Text = "Find";
            findToolStripMenuItem.Click += FindToolStripMenuItem_Click;
            // 
            // LexiconEditor
            // 
            ClientSize = new Size(412, 450);
            Controls.Add(lbxLexicon);
            Controls.Add(menuStrip1);
            Name = "LexiconEditor";
            Text = "Lexicon Editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private ListBox lbxLexicon;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveAndCloseToolStripMenuItem;
        private ToolStripMenuItem closeWithoutSavingToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem addEntryMenuItem;
        private ToolStripMenuItem sortByToolStripMenuItem;
        private ToolStripMenuItem conlangToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
    }
}