/*
 * Designer code for the Lexical Editor
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
    /// <summary>
    /// Dialog/form that allows the user to edit the lexical order used when sorting the language's 
    /// spelled (Romanized or Latinized) form.
    /// </summary>
    partial class LexicalOrderEditor
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveAndCloseToolStripMenuItem = new ToolStripMenuItem();
            closeWithoutToolStripMenuItem = new ToolStripMenuItem();
            lbxCharacters = new ListBox();
            btnMoveUp = new Button();
            btnMoveDown = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(237, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAndCloseToolStripMenuItem, closeWithoutToolStripMenuItem });
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
            // closeWithoutToolStripMenuItem
            // 
            closeWithoutToolStripMenuItem.Name = "closeWithoutToolStripMenuItem";
            closeWithoutToolStripMenuItem.Size = new Size(187, 22);
            closeWithoutToolStripMenuItem.Text = "Close Without Saving";
            closeWithoutToolStripMenuItem.Click += CloseWithoutToolStripMenuItem_Click;
            // 
            // lbxCharacters
            // 
            lbxCharacters.FormattingEnabled = true;
            lbxCharacters.ItemHeight = 15;
            lbxCharacters.Location = new Point(12, 27);
            lbxCharacters.Name = "lbxCharacters";
            lbxCharacters.Size = new Size(86, 289);
            lbxCharacters.TabIndex = 1;
            // 
            // btnMoveUp
            // 
            btnMoveUp.Location = new Point(108, 35);
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(116, 23);
            btnMoveUp.TabIndex = 2;
            btnMoveUp.Text = "Move Up ↑";
            btnMoveUp.UseVisualStyleBackColor = true;
            btnMoveUp.Click += BtnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.Location = new Point(108, 64);
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(116, 23);
            btnMoveDown.TabIndex = 3;
            btnMoveDown.Text = "Move Down ↓";
            btnMoveDown.UseVisualStyleBackColor = true;
            btnMoveDown.Click += BtnMoveDown_Click;
            // 
            // LexicalOrderEditor
            // 
            ClientSize = new Size(237, 322);
            Controls.Add(btnMoveDown);
            Controls.Add(btnMoveUp);
            Controls.Add(lbxCharacters);
            Controls.Add(menuStrip1);
            Name = "LexicalOrderEditor";
            Text = "LexicalOrderEditor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveAndCloseToolStripMenuItem;
        private ToolStripMenuItem closeWithoutToolStripMenuItem;
        private ListBox lbxCharacters;
        private Button btnMoveUp;
        private Button btnMoveDown;
    }
}