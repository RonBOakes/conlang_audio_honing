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
            ConlangJson.SoundMap soundMap1 = new ConlangJson.SoundMap();
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
            txtPhonemeBeingReplaced = new TextBox();
            txtReplacementPhoneme = new TextBox();
            label1 = new Label();
            label2 = new Label();
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
            btnAddAbove.Click += btnAddAbove_Click;
            // 
            // btnAddBelow
            // 
            btnAddBelow.Location = new Point(868, 140);
            btnAddBelow.Name = "btnAddBelow";
            btnAddBelow.Size = new Size(127, 23);
            btnAddBelow.TabIndex = 4;
            btnAddBelow.Text = "Add Below Selected";
            btnAddBelow.UseVisualStyleBackColor = true;
            btnAddBelow.Click += btnAddBelow_Click;
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
            soundMap1.phoneme = "";
            soundMap1.pronounciation_regex = "";
            soundMap1.romanization = "";
            soundMap1.spelling_regex = "";
            soundMapEditor.SoundMapData = soundMap1;
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
            saveAndCloseToolStripMenuItem.Click += saveAndCloseToolStripMenuItem_Click;
            // 
            // closeWithoutSavingToolStripMenuItem
            // 
            closeWithoutSavingToolStripMenuItem.Name = "closeWithoutSavingToolStripMenuItem";
            closeWithoutSavingToolStripMenuItem.Size = new Size(184, 22);
            closeWithoutSavingToolStripMenuItem.Text = "Close without saving";
            closeWithoutSavingToolStripMenuItem.Click += closeWithoutSavingToolStripMenuItem_Click;
            // 
            // btnReplaceSelected
            // 
            btnReplaceSelected.Location = new Point(868, 111);
            btnReplaceSelected.Name = "btnReplaceSelected";
            btnReplaceSelected.Size = new Size(127, 23);
            btnReplaceSelected.TabIndex = 8;
            btnReplaceSelected.Text = "Replace Selected";
            btnReplaceSelected.UseVisualStyleBackColor = true;
            btnReplaceSelected.Click += btnReplaceSelected_Click;
            // 
            // btnEditSelected
            // 
            btnEditSelected.Location = new Point(868, 187);
            btnEditSelected.Name = "btnEditSelected";
            btnEditSelected.Size = new Size(127, 42);
            btnEditSelected.TabIndex = 9;
            btnEditSelected.Text = "Load Selected Into Editor";
            btnEditSelected.UseVisualStyleBackColor = true;
            btnEditSelected.Click += btnEditSelected_Click;
            // 
            // btnDeleteSelected
            // 
            btnDeleteSelected.Location = new Point(868, 235);
            btnDeleteSelected.Name = "btnDeleteSelected";
            btnDeleteSelected.Size = new Size(127, 23);
            btnDeleteSelected.TabIndex = 10;
            btnDeleteSelected.Text = "Delete Selected Entry";
            btnDeleteSelected.UseVisualStyleBackColor = true;
            btnDeleteSelected.Click += btnDeleteSelected_Click;
            // 
            // txtPhonemeBeingReplaced
            // 
            txtPhonemeBeingReplaced.Font = new Font("Charis SIL", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPhonemeBeingReplaced.Location = new Point(163, 33);
            txtPhonemeBeingReplaced.Name = "txtPhonemeBeingReplaced";
            txtPhonemeBeingReplaced.ReadOnly = true;
            txtPhonemeBeingReplaced.Size = new Size(118, 34);
            txtPhonemeBeingReplaced.TabIndex = 11;
            // 
            // txtReplacementPhoneme
            // 
            txtReplacementPhoneme.Font = new Font("Charis SIL", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtReplacementPhoneme.Location = new Point(426, 33);
            txtReplacementPhoneme.Name = "txtReplacementPhoneme";
            txtReplacementPhoneme.ReadOnly = true;
            txtReplacementPhoneme.Size = new Size(132, 32);
            txtReplacementPhoneme.TabIndex = 12;
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(287, 42);
            label2.Name = "label2";
            label2.Size = new Size(133, 15);
            label2.TabIndex = 14;
            label2.Text = "Replacement Phoneme:";
            // 
            // SoundMapListEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1007, 382);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtReplacementPhoneme);
            Controls.Add(txtPhonemeBeingReplaced);
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
        private TextBox txtPhonemeBeingReplaced;
        private TextBox txtReplacementPhoneme;
        private Label label1;
        private Label label2;
    }
}