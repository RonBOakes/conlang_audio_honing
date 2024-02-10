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
            gbx_phonetics = new GroupBox();
            rbn_vowels = new RadioButton();
            rbn_consonants = new RadioButton();
            label1 = new Label();
            label2 = new Label();
            cbx_phonemeToChange = new ComboBox();
            cbx_replacementPhoneme = new ComboBox();
            btn_applyChangeToLanguage = new Button();
            btn_revertLastChange = new Button();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            cbx_voice = new ComboBox();
            label6 = new Label();
            cbx_speed = new ComboBox();
            label7 = new Label();
            gbx_replacementPhomeDepth = new GroupBox();
            rbn_allPhonemes = new RadioButton();
            rbn_l3 = new RadioButton();
            rbn_l2 = new RadioButton();
            rbn_l1 = new RadioButton();
            gbx_vowelReplacements = new GroupBox();
            rbn_longVowels = new RadioButton();
            rbn_semivowelDiphthong = new RadioButton();
            rbn_halfLongVowels = new RadioButton();
            rbn_normalVowel = new RadioButton();
            rbn_normalDiphthong = new RadioButton();
            txt_changeList = new TextBox();
            label8 = new Label();
            btn_addCurrentChangeToList = new Button();
            btn_applyListOfChanges = new Button();
            menuStrip1.SuspendLayout();
            gbx_phonetics.SuspendLayout();
            gbx_replacementPhomeDepth.SuspendLayout();
            gbx_vowelReplacements.SuspendLayout();
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
            txt_phonetic.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
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
            btn_generate.Location = new Point(15, 166);
            btn_generate.Name = "btn_generate";
            btn_generate.Size = new Size(136, 23);
            btn_generate.TabIndex = 5;
            btn_generate.Text = "Generate Phonetic Text";
            btn_generate.UseVisualStyleBackColor = true;
            btn_generate.Click += btn_generate_Click;
            // 
            // btn_generateSpeech
            // 
            btn_generateSpeech.Location = new Point(157, 166);
            btn_generateSpeech.Name = "btn_generateSpeech";
            btn_generateSpeech.Size = new Size(130, 23);
            btn_generateSpeech.TabIndex = 6;
            btn_generateSpeech.Text = "Generate Speech";
            btn_generateSpeech.UseVisualStyleBackColor = true;
            btn_generateSpeech.Click += btn_generateSpeech_Click;
            // 
            // cbx_recordings
            // 
            cbx_recordings.DropDownStyle = ComboBoxStyle.DropDownList;
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
            btn_replaySpeech.Location = new Point(293, 195);
            btn_replaySpeech.Name = "btn_replaySpeech";
            btn_replaySpeech.Size = new Size(145, 23);
            btn_replaySpeech.TabIndex = 8;
            btn_replaySpeech.Text = "Replay Selected Speech";
            btn_replaySpeech.UseVisualStyleBackColor = true;
            btn_replaySpeech.Click += btn_replaySpeech_Click;
            // 
            // pb_status
            // 
            pb_status.Location = new Point(15, 224);
            pb_status.Name = "pb_status";
            pb_status.Size = new Size(993, 34);
            pb_status.TabIndex = 9;
            pb_status.Visible = false;
            // 
            // pbTimer
            // 
            pbTimer.Tick += pbTimer_Tick;
            // 
            // gbx_phonetics
            // 
            gbx_phonetics.Controls.Add(rbn_vowels);
            gbx_phonetics.Controls.Add(rbn_consonants);
            gbx_phonetics.Location = new Point(15, 224);
            gbx_phonetics.Name = "gbx_phonetics";
            gbx_phonetics.Size = new Size(492, 42);
            gbx_phonetics.TabIndex = 10;
            gbx_phonetics.TabStop = false;
            gbx_phonetics.Text = "Phonetics to Alter";
            // 
            // rbn_vowels
            // 
            rbn_vowels.AutoSize = true;
            rbn_vowels.Location = new Point(106, 15);
            rbn_vowels.Name = "rbn_vowels";
            rbn_vowels.Size = new Size(103, 19);
            rbn_vowels.TabIndex = 1;
            rbn_vowels.TabStop = true;
            rbn_vowels.Text = "Vowels (single)";
            rbn_vowels.UseVisualStyleBackColor = true;
            rbn_vowels.CheckedChanged += rbn_vowels_CheckedChanged;
            // 
            // rbn_consonants
            // 
            rbn_consonants.AutoSize = true;
            rbn_consonants.Location = new Point(12, 16);
            rbn_consonants.Name = "rbn_consonants";
            rbn_consonants.Size = new Size(88, 19);
            rbn_consonants.TabIndex = 0;
            rbn_consonants.TabStop = true;
            rbn_consonants.Text = "Consonants";
            rbn_consonants.UseVisualStyleBackColor = true;
            rbn_consonants.CheckedChanged += rbn_consonants_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 269);
            label1.Name = "label1";
            label1.Size = new Size(114, 15);
            label1.TabIndex = 11;
            label1.Text = "Phoneme to change";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 319);
            label2.Name = "label2";
            label2.Size = new Size(130, 15);
            label2.TabIndex = 12;
            label2.Text = "Replacement Phoneme";
            // 
            // cbx_phonemeToChange
            // 
            cbx_phonemeToChange.DropDownStyle = ComboBoxStyle.DropDownList;
            cbx_phonemeToChange.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbx_phonemeToChange.FormattingEnabled = true;
            cbx_phonemeToChange.Location = new Point(15, 287);
            cbx_phonemeToChange.Name = "cbx_phonemeToChange";
            cbx_phonemeToChange.Size = new Size(492, 28);
            cbx_phonemeToChange.TabIndex = 13;
            cbx_phonemeToChange.SelectedIndexChanged += cbx_phonemeToChange_SelectedIndexChanged;
            // 
            // cbx_replacementPhoneme
            // 
            cbx_replacementPhoneme.DropDownStyle = ComboBoxStyle.DropDownList;
            cbx_replacementPhoneme.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbx_replacementPhoneme.FormattingEnabled = true;
            cbx_replacementPhoneme.Location = new Point(15, 337);
            cbx_replacementPhoneme.Name = "cbx_replacementPhoneme";
            cbx_replacementPhoneme.Size = new Size(492, 28);
            cbx_replacementPhoneme.TabIndex = 14;
            // 
            // btn_applyChangeToLanguage
            // 
            btn_applyChangeToLanguage.Location = new Point(515, 364);
            btn_applyChangeToLanguage.Name = "btn_applyChangeToLanguage";
            btn_applyChangeToLanguage.Size = new Size(493, 23);
            btn_applyChangeToLanguage.TabIndex = 15;
            btn_applyChangeToLanguage.Text = "Apply Current Change to the Language";
            btn_applyChangeToLanguage.UseVisualStyleBackColor = true;
            btn_applyChangeToLanguage.Click += btn_applyChangeToLanguage_Click;
            // 
            // btn_revertLastChange
            // 
            btn_revertLastChange.Location = new Point(515, 451);
            btn_revertLastChange.Name = "btn_revertLastChange";
            btn_revertLastChange.Size = new Size(493, 23);
            btn_revertLastChange.TabIndex = 16;
            btn_revertLastChange.Text = "Revert the last change";
            btn_revertLastChange.UseVisualStyleBackColor = true;
            btn_revertLastChange.Click += btn_revertLastChange_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Red;
            label3.Location = new Point(15, 368);
            label3.Name = "label3";
            label3.Size = new Size(174, 15);
            label3.TabIndex = 17;
            label3.Text = "Red - Phoneme in Spelling Map";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Green;
            label4.Location = new Point(15, 383);
            label4.Name = "label4";
            label4.Size = new Size(216, 15);
            label4.TabIndex = 18;
            label4.Text = "Green - Phoneme in phonetic inventory";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Blue;
            label5.Location = new Point(15, 398);
            label5.Name = "label5";
            label5.Size = new Size(302, 15);
            label5.TabIndex = 19;
            label5.Text = "Blue - Phoneme in spelling map and phonetic inventory";
            // 
            // cbx_voice
            // 
            cbx_voice.FormattingEnabled = true;
            cbx_voice.Location = new Point(513, 166);
            cbx_voice.Name = "cbx_voice";
            cbx_voice.Size = new Size(246, 23);
            cbx_voice.Sorted = true;
            cbx_voice.TabIndex = 20;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(513, 192);
            label6.Name = "label6";
            label6.Size = new Size(35, 15);
            label6.TabIndex = 21;
            label6.Text = "Voice";
            // 
            // cbx_speed
            // 
            cbx_speed.FormattingEnabled = true;
            cbx_speed.Location = new Point(765, 166);
            cbx_speed.Name = "cbx_speed";
            cbx_speed.Size = new Size(239, 23);
            cbx_speed.TabIndex = 22;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(765, 192);
            label7.Name = "label7";
            label7.Size = new Size(39, 15);
            label7.TabIndex = 23;
            label7.Text = "Speed";
            // 
            // gbx_replacementPhomeDepth
            // 
            gbx_replacementPhomeDepth.Controls.Add(rbn_allPhonemes);
            gbx_replacementPhomeDepth.Controls.Add(rbn_l3);
            gbx_replacementPhomeDepth.Controls.Add(rbn_l2);
            gbx_replacementPhomeDepth.Controls.Add(rbn_l1);
            gbx_replacementPhomeDepth.Location = new Point(513, 224);
            gbx_replacementPhomeDepth.Name = "gbx_replacementPhomeDepth";
            gbx_replacementPhomeDepth.Size = new Size(495, 42);
            gbx_replacementPhomeDepth.TabIndex = 24;
            gbx_replacementPhomeDepth.TabStop = false;
            gbx_replacementPhomeDepth.Text = "Consonant Depth of Replacement Phonemes";
            // 
            // rbn_allPhonemes
            // 
            rbn_allPhonemes.AutoSize = true;
            rbn_allPhonemes.Location = new Point(240, 15);
            rbn_allPhonemes.Name = "rbn_allPhonemes";
            rbn_allPhonemes.Size = new Size(105, 19);
            rbn_allPhonemes.TabIndex = 3;
            rbn_allPhonemes.TabStop = true;
            rbn_allPhonemes.Text = "All Consonants";
            rbn_allPhonemes.UseVisualStyleBackColor = true;
            // 
            // rbn_l3
            // 
            rbn_l3.AutoSize = true;
            rbn_l3.Location = new Point(162, 16);
            rbn_l3.Name = "rbn_l3";
            rbn_l3.Size = new Size(72, 19);
            rbn_l3.TabIndex = 2;
            rbn_l3.TabStop = true;
            rbn_l3.Text = "3-degree";
            rbn_l3.UseVisualStyleBackColor = true;
            // 
            // rbn_l2
            // 
            rbn_l2.AutoSize = true;
            rbn_l2.Location = new Point(84, 15);
            rbn_l2.Name = "rbn_l2";
            rbn_l2.Size = new Size(72, 19);
            rbn_l2.TabIndex = 1;
            rbn_l2.TabStop = true;
            rbn_l2.Text = "2-degree";
            rbn_l2.UseVisualStyleBackColor = true;
            // 
            // rbn_l1
            // 
            rbn_l1.AutoSize = true;
            rbn_l1.Location = new Point(6, 15);
            rbn_l1.Name = "rbn_l1";
            rbn_l1.Size = new Size(72, 19);
            rbn_l1.TabIndex = 0;
            rbn_l1.TabStop = true;
            rbn_l1.Text = "1-degree";
            rbn_l1.UseVisualStyleBackColor = true;
            // 
            // gbx_vowelReplacements
            // 
            gbx_vowelReplacements.Controls.Add(rbn_longVowels);
            gbx_vowelReplacements.Controls.Add(rbn_semivowelDiphthong);
            gbx_vowelReplacements.Controls.Add(rbn_halfLongVowels);
            gbx_vowelReplacements.Controls.Add(rbn_normalVowel);
            gbx_vowelReplacements.Controls.Add(rbn_normalDiphthong);
            gbx_vowelReplacements.Location = new Point(515, 272);
            gbx_vowelReplacements.Name = "gbx_vowelReplacements";
            gbx_vowelReplacements.Size = new Size(493, 76);
            gbx_vowelReplacements.TabIndex = 25;
            gbx_vowelReplacements.TabStop = false;
            gbx_vowelReplacements.Text = "Vowel Replacement Settings";
            // 
            // rbn_longVowels
            // 
            rbn_longVowels.AutoSize = true;
            rbn_longVowels.Location = new Point(235, 16);
            rbn_longVowels.Name = "rbn_longVowels";
            rbn_longVowels.Size = new Size(91, 19);
            rbn_longVowels.TabIndex = 2;
            rbn_longVowels.TabStop = true;
            rbn_longVowels.Text = "Long Vowels";
            rbn_longVowels.UseVisualStyleBackColor = true;
            // 
            // rbn_semivowelDiphthong
            // 
            rbn_semivowelDiphthong.AutoSize = true;
            rbn_semivowelDiphthong.Location = new Point(209, 40);
            rbn_semivowelDiphthong.Name = "rbn_semivowelDiphthong";
            rbn_semivowelDiphthong.Size = new Size(182, 19);
            rbn_semivowelDiphthong.TabIndex = 3;
            rbn_semivowelDiphthong.TabStop = true;
            rbn_semivowelDiphthong.Text = "Create semi-vowel diphthong";
            rbn_semivowelDiphthong.UseVisualStyleBackColor = true;
            // 
            // rbn_halfLongVowels
            // 
            rbn_halfLongVowels.AutoSize = true;
            rbn_halfLongVowels.Location = new Point(111, 15);
            rbn_halfLongVowels.Name = "rbn_halfLongVowels";
            rbn_halfLongVowels.Size = new Size(118, 19);
            rbn_halfLongVowels.TabIndex = 1;
            rbn_halfLongVowels.TabStop = true;
            rbn_halfLongVowels.Text = "Half-Long Vowels";
            rbn_halfLongVowels.UseVisualStyleBackColor = true;
            // 
            // rbn_normalVowel
            // 
            rbn_normalVowel.AutoSize = true;
            rbn_normalVowel.Location = new Point(6, 16);
            rbn_normalVowel.Name = "rbn_normalVowel";
            rbn_normalVowel.Size = new Size(99, 19);
            rbn_normalVowel.TabIndex = 0;
            rbn_normalVowel.TabStop = true;
            rbn_normalVowel.Text = "Normal Vowel";
            rbn_normalVowel.UseVisualStyleBackColor = true;
            // 
            // rbn_normalDiphthong
            // 
            rbn_normalDiphthong.AutoSize = true;
            rbn_normalDiphthong.Location = new Point(6, 41);
            rbn_normalDiphthong.Name = "rbn_normalDiphthong";
            rbn_normalDiphthong.Size = new Size(197, 19);
            rbn_normalDiphthong.TabIndex = 3;
            rbn_normalDiphthong.TabStop = true;
            rbn_normalDiphthong.Text = "Add Vowel to create Diphtohong";
            rbn_normalDiphthong.UseVisualStyleBackColor = true;
            // 
            // txt_changeList
            // 
            txt_changeList.Enabled = false;
            txt_changeList.Location = new Point(109, 423);
            txt_changeList.Multiline = true;
            txt_changeList.Name = "txt_changeList";
            txt_changeList.ScrollBars = ScrollBars.Vertical;
            txt_changeList.Size = new Size(398, 33);
            txt_changeList.TabIndex = 26;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 423);
            label8.Name = "label8";
            label8.Size = new Size(88, 15);
            label8.TabIndex = 27;
            label8.Text = "List of Changes";
            // 
            // btn_addCurrentChangeToList
            // 
            btn_addCurrentChangeToList.Location = new Point(515, 393);
            btn_addCurrentChangeToList.Name = "btn_addCurrentChangeToList";
            btn_addCurrentChangeToList.Size = new Size(493, 23);
            btn_addCurrentChangeToList.TabIndex = 28;
            btn_addCurrentChangeToList.Text = "Add Current Change to List of Changes";
            btn_addCurrentChangeToList.UseVisualStyleBackColor = true;
            btn_addCurrentChangeToList.Click += btn_addCurrentChangeToList_Click;
            // 
            // btn_applyListOfChanges
            // 
            btn_applyListOfChanges.Location = new Point(515, 422);
            btn_applyListOfChanges.Name = "btn_applyListOfChanges";
            btn_applyListOfChanges.Size = new Size(493, 23);
            btn_applyListOfChanges.TabIndex = 29;
            btn_applyListOfChanges.Text = "Apply All Listed Changes to the Language (Simultaneously)";
            btn_applyListOfChanges.UseVisualStyleBackColor = true;
            btn_applyListOfChanges.Click += btn_applyListOfChanges_Click;
            // 
            // LanguageHoningForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 496);
            Controls.Add(btn_applyListOfChanges);
            Controls.Add(btn_addCurrentChangeToList);
            Controls.Add(label8);
            Controls.Add(txt_changeList);
            Controls.Add(gbx_replacementPhomeDepth);
            Controls.Add(label7);
            Controls.Add(cbx_speed);
            Controls.Add(label6);
            Controls.Add(gbx_vowelReplacements);
            Controls.Add(cbx_voice);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btn_revertLastChange);
            Controls.Add(btn_applyChangeToLanguage);
            Controls.Add(cbx_replacementPhoneme);
            Controls.Add(cbx_phonemeToChange);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btn_replaySpeech);
            Controls.Add(cbx_recordings);
            Controls.Add(btn_generateSpeech);
            Controls.Add(btn_generate);
            Controls.Add(lbl_phonetic);
            Controls.Add(txt_phonetic);
            Controls.Add(lbl_SampleText);
            Controls.Add(txt_SampleText);
            Controls.Add(menuStrip1);
            Controls.Add(gbx_phonetics);
            Controls.Add(pb_status);
            MainMenuStrip = menuStrip1;
            Name = "LanguageHoningForm";
            Text = "LanguageHoningForm";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            gbx_phonetics.ResumeLayout(false);
            gbx_phonetics.PerformLayout();
            gbx_replacementPhomeDepth.ResumeLayout(false);
            gbx_replacementPhomeDepth.PerformLayout();
            gbx_vowelReplacements.ResumeLayout(false);
            gbx_vowelReplacements.PerformLayout();
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
        private GroupBox gbx_phonetics;
        private RadioButton rbn_consonants;
        private Label label1;
        private Label label2;
        private ComboBox cbx_phonemeToChange;
        private ComboBox cbx_replacementPhoneme;
        private Button btn_applyChangeToLanguage;
        private Button btn_revertLastChange;
        private Label label3;
        private Label label4;
        private Label label5;
        private ComboBox cbx_voice;
        private Label label6;
        private ComboBox cbx_speed;
        private Label label7;
        private GroupBox gbx_replacementPhomeDepth;
        private RadioButton rbn_l3;
        private RadioButton rbn_l2;
        private RadioButton rbn_l1;
        private RadioButton rbn_allPhonemes;
        private GroupBox gbx_vowelReplacements;
        private RadioButton rbn_halfLongVowels;
        private RadioButton rbn_normalVowel;
        private RadioButton rbn_longVowels;
        private RadioButton rbn_normalDiphthong;
        private RadioButton rbn_semivowelDiphthong;
        private RadioButton rbn_vowels;
        private TextBox txt_changeList;
        private Label label8;
        private Button btn_addCurrentChangeToList;
        private Button btn_applyListOfChanges;
    }
}