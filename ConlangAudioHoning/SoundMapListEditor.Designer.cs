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
            soundMapEditor2 = new SoundMapEditor();
            menuStrip1 = new MenuStrip();
            controlsToolStripMenuItem = new ToolStripMenuItem();
            saveAndCloseToolStripMenuItem = new ToolStripMenuItem();
            closeWithoutSavingToolStripMenuItem = new ToolStripMenuItem();
            btnReplaceSelected = new Button();
            btnEditSelected = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnAddAbove
            // 
            btnAddAbove.Location = new Point(868, 27);
            btnAddAbove.Name = "btnAddAbove";
            btnAddAbove.Size = new Size(127, 23);
            btnAddAbove.TabIndex = 3;
            btnAddAbove.Text = "Add Above Selected";
            btnAddAbove.UseVisualStyleBackColor = true;
            btnAddAbove.Click += btnAddAbove_Click;
            // 
            // btnAddBelow
            // 
            btnAddBelow.Location = new Point(868, 85);
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
            lbx_soundMapListEntries.Location = new Point(12, 113);
            lbx_soundMapListEntries.Name = "lbx_soundMapListEntries";
            lbx_soundMapListEntries.Size = new Size(850, 349);
            lbx_soundMapListEntries.TabIndex = 5;
            // 
            // soundMapEditor2
            // 
            soundMapEditor2.BorderStyle = BorderStyle.FixedSingle;
            soundMapEditor2.Location = new Point(12, 27);
            soundMapEditor2.Name = "soundMapEditor2";
            soundMapEditor2.Size = new Size(850, 80);
            soundMap1.phoneme = "";
            soundMap1.pronounciation_regex = "";
            soundMap1.romanization = "";
            soundMap1.spelling_regex = "";
            soundMapEditor2.SoundMapData = soundMap1;
            soundMapEditor2.TabIndex = 6;
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
            btnReplaceSelected.Location = new Point(868, 56);
            btnReplaceSelected.Name = "btnReplaceSelected";
            btnReplaceSelected.Size = new Size(127, 23);
            btnReplaceSelected.TabIndex = 8;
            btnReplaceSelected.Text = "Replace Selected";
            btnReplaceSelected.UseVisualStyleBackColor = true;
            // 
            // btnEditSelected
            // 
            btnEditSelected.Location = new Point(868, 138);
            btnEditSelected.Name = "btnEditSelected";
            btnEditSelected.Size = new Size(127, 42);
            btnEditSelected.TabIndex = 9;
            btnEditSelected.Text = "Load Selected Into Editor";
            btnEditSelected.UseVisualStyleBackColor = true;
            // 
            // SoundMapListEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1007, 474);
            Controls.Add(btnEditSelected);
            Controls.Add(btnReplaceSelected);
            Controls.Add(soundMapEditor2);
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
        private SoundMapEditor soundMapEditor2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem controlsToolStripMenuItem;
        private ToolStripMenuItem saveAndCloseToolStripMenuItem;
        private ToolStripMenuItem closeWithoutSavingToolStripMenuItem;
        private Button btnReplaceSelected;
        private Button btnEditSelected;
    }
}