namespace ConlangAudioHoning
{
    partial class LanguageHoningForm
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
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripMenuItem1 = new ToolStripMenuItem();
            saveSampleMenu = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            languageToolStripMenuItem = new ToolStripMenuItem();
            declineToolStripMenuItem = new ToolStripMenuItem();
            deriveToolStripMenuItem = new ToolStripMenuItem();
            debugToolStripMenuItem = new ToolStripMenuItem();
            displayPulmonicConsonantsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            printIPAMapToolStripMenuItem = new ToolStripMenuItem();
            txt_SampleText = new TextBox();
            lbl_SampleText = new Label();
            txt_phonetic = new TextBox();
            lbl_phonetic = new Label();
            btn_generate = new Button();
            btn_generateSpeech = new Button();
            cbx_recordings = new ComboBox();
            btn_replaySpeech = new Button();
            pb_status = new ProgressBar();
            pbTimer = new System.Windows.Forms.Timer(components);
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, languageToolStripMenuItem, debugToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1024, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadToolStripMenuItem, saveToolStripMenuItem, toolStripSeparator2, toolStripMenuItem1, saveSampleMenu, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(227, 22);
            loadToolStripMenuItem.Text = "Load Language File";
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(227, 22);
            saveToolStripMenuItem.Text = "Save Language File";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(224, 6);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(227, 22);
            toolStripMenuItem1.Text = "Load Sample Text";
            toolStripMenuItem1.Click += loadSampleTextFile_Click;
            // 
            // saveSampleMenu
            // 
            saveSampleMenu.Name = "saveSampleMenu";
            saveSampleMenu.ShortcutKeys = Keys.Control | Keys.Alt | Keys.S;
            saveSampleMenu.Size = new Size(227, 22);
            saveSampleMenu.Text = "Save Sample Text";
            saveSampleMenu.Click += saveSampleMenu_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(224, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(227, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // languageToolStripMenuItem
            // 
            languageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { declineToolStripMenuItem, deriveToolStripMenuItem });
            languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            languageToolStripMenuItem.Size = new Size(71, 20);
            languageToolStripMenuItem.Text = "Language";
            // 
            // declineToolStripMenuItem
            // 
            declineToolStripMenuItem.Name = "declineToolStripMenuItem";
            declineToolStripMenuItem.Size = new Size(168, 22);
            declineToolStripMenuItem.Text = "Decline Language";
            declineToolStripMenuItem.Click += declineToolStripMenuItem_Click;
            // 
            // deriveToolStripMenuItem
            // 
            deriveToolStripMenuItem.Name = "deriveToolStripMenuItem";
            deriveToolStripMenuItem.Size = new Size(168, 22);
            deriveToolStripMenuItem.Text = "Derive Words";
            // 
            // debugToolStripMenuItem
            // 
            debugToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { displayPulmonicConsonantsToolStripMenuItem, toolStripMenuItem2, printIPAMapToolStripMenuItem });
            debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            debugToolStripMenuItem.Size = new Size(54, 20);
            debugToolStripMenuItem.Text = "Debug";
            // 
            // displayPulmonicConsonantsToolStripMenuItem
            // 
            displayPulmonicConsonantsToolStripMenuItem.Name = "displayPulmonicConsonantsToolStripMenuItem";
            displayPulmonicConsonantsToolStripMenuItem.Size = new Size(232, 22);
            displayPulmonicConsonantsToolStripMenuItem.Text = "Display Pulmonic Consonants";
            displayPulmonicConsonantsToolStripMenuItem.Click += displayPulmonicConsonantsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(232, 22);
            toolStripMenuItem2.Text = "Display Supersegmental";
            toolStripMenuItem2.Click += displaySupersegmentals_Click;
            // 
            // printIPAMapToolStripMenuItem
            // 
            printIPAMapToolStripMenuItem.Name = "printIPAMapToolStripMenuItem";
            printIPAMapToolStripMenuItem.Size = new Size(232, 22);
            printIPAMapToolStripMenuItem.Text = "Print IPA Map";
            printIPAMapToolStripMenuItem.Click += printIPAMapToolStripMenuItem_Click;
            // 
            // txt_SampleText
            // 
            txt_SampleText.AccessibleDescription = "Sample Text";
            txt_SampleText.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_SampleText.Location = new Point(15, 47);
            txt_SampleText.Multiline = true;
            txt_SampleText.Name = "txt_SampleText";
            txt_SampleText.ScrollBars = ScrollBars.Vertical;
            txt_SampleText.Size = new Size(492, 109);
            txt_SampleText.TabIndex = 1;
            txt_SampleText.TextChanged += txt_SampleText_TextChanged;
            // 
            // lbl_SampleText
            // 
            lbl_SampleText.AutoSize = true;
            lbl_SampleText.Location = new Point(15, 24);
            lbl_SampleText.Name = "lbl_SampleText";
            lbl_SampleText.Size = new Size(70, 15);
            lbl_SampleText.TabIndex = 2;
            lbl_SampleText.Text = "Sample Text";
            // 
            // txt_phonetic
            // 
            txt_phonetic.Font = new Font("Charis SIL", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_phonetic.Location = new Point(513, 47);
            txt_phonetic.Multiline = true;
            txt_phonetic.Name = "txt_phonetic";
            txt_phonetic.ReadOnly = true;
            txt_phonetic.ScrollBars = ScrollBars.Vertical;
            txt_phonetic.Size = new Size(495, 109);
            txt_phonetic.TabIndex = 3;
            // 
            // lbl_phonetic
            // 
            lbl_phonetic.AutoSize = true;
            lbl_phonetic.Location = new Point(515, 30);
            lbl_phonetic.Name = "lbl_phonetic";
            lbl_phonetic.Size = new Size(136, 15);
            lbl_phonetic.TabIndex = 4;
            lbl_phonetic.Text = "Phonetic Representation";
            // 
            // btn_generate
            // 
            btn_generate.Location = new Point(15, 162);
            btn_generate.Name = "btn_generate";
            btn_generate.Size = new Size(136, 23);
            btn_generate.TabIndex = 5;
            btn_generate.Text = "Generate Phonetic Text";
            btn_generate.UseVisualStyleBackColor = true;
            btn_generate.Click += btn_generate_Click;
            // 
            // btn_generateSpeech
            // 
            btn_generateSpeech.Location = new Point(157, 162);
            btn_generateSpeech.Name = "btn_generateSpeech";
            btn_generateSpeech.Size = new Size(130, 23);
            btn_generateSpeech.TabIndex = 6;
            btn_generateSpeech.Text = "Generate Speech";
            btn_generateSpeech.UseVisualStyleBackColor = true;
            btn_generateSpeech.Click += btn_generateSpeech_Click;
            // 
            // cbx_recordings
            // 
            cbx_recordings.FormattingEnabled = true;
            cbx_recordings.ItemHeight = 15;
            cbx_recordings.Location = new Point(293, 166);
            cbx_recordings.Name = "cbx_recordings";
            cbx_recordings.Size = new Size(214, 23);
            cbx_recordings.Sorted = true;
            cbx_recordings.TabIndex = 7;
            // 
            // btn_replaySpeech
            // 
            btn_replaySpeech.Location = new Point(515, 162);
            btn_replaySpeech.Name = "btn_replaySpeech";
            btn_replaySpeech.Size = new Size(145, 23);
            btn_replaySpeech.TabIndex = 8;
            btn_replaySpeech.Text = "Replay Selected Speech";
            btn_replaySpeech.UseVisualStyleBackColor = true;
            btn_replaySpeech.Click += btn_replaySpeech_Click;
            // 
            // pb_status
            // 
            pb_status.Location = new Point(21, 192);
            pb_status.Name = "pb_status";
            pb_status.Size = new Size(987, 34);
            pb_status.TabIndex = 9;
            pb_status.Visible = false;
            // 
            // pbTimer
            // 
            pbTimer.Tick += pbTimer_Tick;
            // 
            // LanguageHoningForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 496);
            Controls.Add(pb_status);
            Controls.Add(btn_replaySpeech);
            Controls.Add(cbx_recordings);
            Controls.Add(btn_generateSpeech);
            Controls.Add(btn_generate);
            Controls.Add(lbl_phonetic);
            Controls.Add(txt_phonetic);
            Controls.Add(lbl_SampleText);
            Controls.Add(txt_SampleText);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "LanguageHoningForm";
            Text = "LanguageHoningForm";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem toolStripMenuItem1;
        private TextBox txt_SampleText;
        private Label lbl_SampleText;
        private ToolStripMenuItem saveSampleMenu;
        private TextBox txt_phonetic;
        private Label lbl_phonetic;
        private Button btn_generate;
        private ToolStripMenuItem languageToolStripMenuItem;
        private ToolStripMenuItem declineToolStripMenuItem;
        private ToolStripMenuItem deriveToolStripMenuItem;
        private Button btn_generateSpeech;
        private ToolStripMenuItem debugToolStripMenuItem;
        private ToolStripMenuItem displayPulmonicConsonantsToolStripMenuItem;
        private ToolStripMenuItem printIPAMapToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ComboBox cbx_recordings;
        private Button btn_replaySpeech;
        private ProgressBar pb_status;
        private System.Windows.Forms.Timer pbTimer;
    }
}