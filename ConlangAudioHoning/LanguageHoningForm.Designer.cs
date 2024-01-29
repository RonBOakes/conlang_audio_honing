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
            txt_SampleText = new TextBox();
            lbl_SampleText = new Label();
            txt_phonetic = new TextBox();
            lbl_phonetic = new Label();
            btn_generate = new Button();
            btn_generateSpeech = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, languageToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
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
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
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
            // txt_SampleText
            // 
            txt_SampleText.AccessibleDescription = "Sample Text";
            txt_SampleText.Location = new Point(12, 42);
            txt_SampleText.Multiline = true;
            txt_SampleText.Name = "txt_SampleText";
            txt_SampleText.ScrollBars = ScrollBars.Vertical;
            txt_SampleText.Size = new Size(769, 92);
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
            txt_phonetic.Location = new Point(12, 157);
            txt_phonetic.Multiline = true;
            txt_phonetic.Name = "txt_phonetic";
            txt_phonetic.ReadOnly = true;
            txt_phonetic.ScrollBars = ScrollBars.Vertical;
            txt_phonetic.Size = new Size(769, 111);
            txt_phonetic.TabIndex = 3;
            // 
            // lbl_phonetic
            // 
            lbl_phonetic.AutoSize = true;
            lbl_phonetic.Location = new Point(14, 142);
            lbl_phonetic.Name = "lbl_phonetic";
            lbl_phonetic.Size = new Size(136, 15);
            lbl_phonetic.TabIndex = 4;
            lbl_phonetic.Text = "Phonetic Representation";
            // 
            // btn_generate
            // 
            btn_generate.Location = new Point(14, 272);
            btn_generate.Name = "btn_generate";
            btn_generate.Size = new Size(136, 23);
            btn_generate.TabIndex = 5;
            btn_generate.Text = "Generate Phonetic Text";
            btn_generate.UseVisualStyleBackColor = true;
            btn_generate.Click += btn_generate_Click;
            // 
            // btn_generateSpeech
            // 
            btn_generateSpeech.Location = new Point(156, 272);
            btn_generateSpeech.Name = "btn_generateSpeech";
            btn_generateSpeech.Size = new Size(130, 23);
            btn_generateSpeech.TabIndex = 6;
            btn_generateSpeech.Text = "Generate Speech";
            btn_generateSpeech.UseVisualStyleBackColor = true;
            btn_generateSpeech.Click += btn_generateSpeech_Click;
            // 
            // LanguageHoningForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}