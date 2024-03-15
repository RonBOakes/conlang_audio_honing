namespace ConlangAudioHoning
{
    partial class SoundMapListEditor
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
            ConlangJson.SoundMap soundMap2 = new ConlangJson.SoundMap();
            btnAddAbove = new Button();
            btnAddBelow = new Button();
            lbx_soundMapListEntries = new ListBox();
            soundMapEditor = new SoundMapEditor();
            menuStrip1 = new MenuStrip();
            controlsToolStripMenuItem = new ToolStripMenuItem();
            saveAndCloseToolStripMenuItem = new ToolStripMenuItem();
            closeWithoutSavingToolStripMenuItem = new ToolStripMenuItem();
            btnReplaceSelected = new Button();
            btnEditSelected = new Button();
            btnDeleteSelected = new Button();
            txtPhonemeReplacements = new TextBox();
            label1 = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnAddAbove
            // 
            btnAddAbove.Location = new Point(868, 82);
            btnAddAbove.Name = "btnAddAbove";
            btnAddAbove.Size = new Size(127, 23);
            btnAddAbove.TabIndex = 3;
            btnAddAbove.Text = "Add Above Selected";
            btnAddAbove.UseVisualStyleBackColor = true;
            btnAddAbove.Click += BtnAddAbove_Click;
            // 
            // btnAddBelow
            // 
            btnAddBelow.Location = new Point(868, 140);
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
            lbx_soundMapListEntries.Size = new Size(850, 199);
            lbx_soundMapListEntries.TabIndex = 5;
            // 
            // soundMapEditor
            // 
            soundMapEditor.BorderStyle = BorderStyle.FixedSingle;
            soundMapEditor.Location = new Point(12, 82);
            soundMapEditor.Name = "soundMapEditor";
            soundMapEditor.Size = new Size(850, 80);
            soundMap2.phoneme = "";
            soundMap2.pronunciation_regex = "";
            soundMap2.romanization = "";
            soundMap2.spelling_regex = "";
            soundMapEditor.SoundMapData = soundMap2;
            soundMapEditor.TabIndex = 6;
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
            // 
            // btnReplaceSelected
            // 
            btnReplaceSelected.Location = new Point(868, 111);
            btnReplaceSelected.Name = "btnReplaceSelected";
            btnReplaceSelected.Size = new Size(127, 23);
            btnReplaceSelected.TabIndex = 8;
            btnReplaceSelected.Text = "Replace Selected";
            btnReplaceSelected.UseVisualStyleBackColor = true;
            btnReplaceSelected.Click += BtnReplaceSelected_Click;
            // 
            // btnEditSelected
            // 
            btnEditSelected.Location = new Point(868, 187);
            btnEditSelected.Name = "btnEditSelected";
            btnEditSelected.Size = new Size(127, 42);
            btnEditSelected.TabIndex = 9;
            btnEditSelected.Text = "Load Selected Into Editor";
            btnEditSelected.UseVisualStyleBackColor = true;
            btnEditSelected.Click += BtnEditSelected_Click;
            // 
            // btnDeleteSelected
            // 
            btnDeleteSelected.Location = new Point(868, 235);
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
            txtPhonemeReplacements.Size = new Size(699, 34);
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
            // SoundMapListEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1007, 382);
            Controls.Add(label1);
            Controls.Add(txtPhonemeReplacements);
            Controls.Add(btnDeleteSelected);
            Controls.Add(btnEditSelected);
            Controls.Add(btnReplaceSelected);
            Controls.Add(soundMapEditor);
            Controls.Add(lbx_soundMapListEntries);
            Controls.Add(btnAddBelow);
            Controls.Add(btnAddAbove);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "SoundMapListEditor";
            Text = "Spelling/Pronounciation List Editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnAddAbove;
        private Button btnAddBelow;
        private ListBox lbx_soundMapListEntries;
        private SoundMapEditor soundMapEditor;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem controlsToolStripMenuItem;
        private ToolStripMenuItem saveAndCloseToolStripMenuItem;
        private ToolStripMenuItem closeWithoutSavingToolStripMenuItem;
        private Button btnReplaceSelected;
        private Button btnEditSelected;
        private Button btnDeleteSelected;
        private TextBox txtPhonemeReplacements;
        private Label label1;
    }
}