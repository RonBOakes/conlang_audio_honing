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
            utilitiesToolStripMenuItem = new ToolStripMenuItem();
            displayGlossOfSampleTextToolStripMenuItem = new ToolStripMenuItem();
            printSampleTextSummaryToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            setAmazonPollyURIToolStripMenuItem = new ToolStripMenuItem();
            setESpeakNgLocationToolStripMenuItem = new ToolStripMenuItem();
            debugToolStripMenuItem = new ToolStripMenuItem();
            displayPulmonicConsonantsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            printIPAMapToolStripMenuItem = new ToolStripMenuItem();
            printKiToolStripMenuItem = new ToolStripMenuItem();
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
            lbl_choiceCbx = new Label();
            lbl_replacementCbx = new Label();
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
            txt_changeList = new TextBox();
            label8 = new Label();
            btn_addCurrentChangeToList = new Button();
            btn_applyListOfChanges = new Button();
            label9 = new Label();
            cbx_speechEngine = new ComboBox();
            label10 = new Label();
            tabPhoneticAlterations = new TabControl();
            tabPageConsonants = new TabPage();
            gbx_replacementPhomeDepth = new GroupBox();
            rbn_allPhonemes = new RadioButton();
            rbn_l3 = new RadioButton();
            rbn_l2 = new RadioButton();
            rbn_l1 = new RadioButton();
            tabPageVowels = new TabPage();
            gbx_vowelReplacements = new GroupBox();
            rbn_longVowels = new RadioButton();
            rbn_semivowelDiphthong = new RadioButton();
            rbn_halfLongVowels = new RadioButton();
            rbn_normalVowel = new RadioButton();
            tabPageVowelDiphthongs = new TabPage();
            lbl_DiphthongEndVowel = new Label();
            cbx_dipthongEndVowel = new ComboBox();
            lbl_diphthongStartVowel = new Label();
            cbx_dipthongStartVowel = new ComboBox();
            rbn_diphthongReplacement = new RadioButton();
            rbn_changeEndVowel = new RadioButton();
            rbn_changeStartVowel = new RadioButton();
            rbn_vowelToDipthongEnd = new RadioButton();
            rbn_vowelToDiphthongStart = new RadioButton();
            tabPageRhoticity = new TabPage();
            rbn_replaceRSpelling = new RadioButton();
            rbn_replaceRhotacized = new RadioButton();
            rbn_removeRhoticity = new RadioButton();
            rbn_longToRhotacized = new RadioButton();
            rbn_addRhoticityRegular = new RadioButton();
            tabPageSpecialOperations = new TabPage();
            btn_updateSoundMapList = new Button();
            menuStrip1.SuspendLayout();
            tabPhoneticAlterations.SuspendLayout();
            tabPageConsonants.SuspendLayout();
            gbx_replacementPhomeDepth.SuspendLayout();
            tabPageVowels.SuspendLayout();
            gbx_vowelReplacements.SuspendLayout();
            tabPageVowelDiphthongs.SuspendLayout();
            tabPageRhoticity.SuspendLayout();
            tabPageSpecialOperations.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, languageToolStripMenuItem, utilitiesToolStripMenuItem, debugToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1000, 24);
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
            loadToolStripMenuItem.Click += LoadToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(227, 22);
            saveToolStripMenuItem.Text = "Save Language File";
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
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
            toolStripMenuItem1.Click += LoadSampleTextFile_Click;
            // 
            // saveSampleMenu
            // 
            saveSampleMenu.Name = "saveSampleMenu";
            saveSampleMenu.ShortcutKeys = Keys.Control | Keys.Alt | Keys.S;
            saveSampleMenu.Size = new Size(227, 22);
            saveSampleMenu.Text = "Save Sample Text";
            saveSampleMenu.Click += SaveSampleMenu_Click;
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
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
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
            declineToolStripMenuItem.Click += DeclineToolStripMenuItem_Click;
            // 
            // deriveToolStripMenuItem
            // 
            deriveToolStripMenuItem.Name = "deriveToolStripMenuItem";
            deriveToolStripMenuItem.Size = new Size(168, 22);
            deriveToolStripMenuItem.Text = "Derive Words";
            // 
            // utilitiesToolStripMenuItem
            // 
            utilitiesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { displayGlossOfSampleTextToolStripMenuItem, printSampleTextSummaryToolStripMenuItem, toolStripSeparator3, setAmazonPollyURIToolStripMenuItem, setESpeakNgLocationToolStripMenuItem });
            utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            utilitiesToolStripMenuItem.Size = new Size(58, 20);
            utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // displayGlossOfSampleTextToolStripMenuItem
            // 
            displayGlossOfSampleTextToolStripMenuItem.Name = "displayGlossOfSampleTextToolStripMenuItem";
            displayGlossOfSampleTextToolStripMenuItem.Size = new Size(223, 22);
            displayGlossOfSampleTextToolStripMenuItem.Text = "Display Gloss of Sample Text";
            displayGlossOfSampleTextToolStripMenuItem.Click += DisplayGlossOfSampleTextToolStripMenuItem_Click;
            // 
            // printSampleTextSummaryToolStripMenuItem
            // 
            printSampleTextSummaryToolStripMenuItem.Name = "printSampleTextSummaryToolStripMenuItem";
            printSampleTextSummaryToolStripMenuItem.Size = new Size(223, 22);
            printSampleTextSummaryToolStripMenuItem.Text = "Print Sample Text Summary";
            printSampleTextSummaryToolStripMenuItem.Click += PrintSampleTextSummaryToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(220, 6);
            // 
            // setAmazonPollyURIToolStripMenuItem
            // 
            setAmazonPollyURIToolStripMenuItem.Name = "setAmazonPollyURIToolStripMenuItem";
            setAmazonPollyURIToolStripMenuItem.Size = new Size(223, 22);
            setAmazonPollyURIToolStripMenuItem.Text = "Set Amazon Polly URI";
            setAmazonPollyURIToolStripMenuItem.Click += SetAmazonPollyURIToolStripMenuItem_Click;
            // 
            // setESpeakNgLocationToolStripMenuItem
            // 
            setESpeakNgLocationToolStripMenuItem.Name = "setESpeakNgLocationToolStripMenuItem";
            setESpeakNgLocationToolStripMenuItem.Size = new Size(223, 22);
            setESpeakNgLocationToolStripMenuItem.Text = "Set eSpeak-ng location";
            setESpeakNgLocationToolStripMenuItem.Click += SetESpeakNgLocationToolStripMenuItem_Click;
            // 
            // debugToolStripMenuItem
            // 
            debugToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { displayPulmonicConsonantsToolStripMenuItem, toolStripMenuItem2, printIPAMapToolStripMenuItem, printKiToolStripMenuItem });
            debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            debugToolStripMenuItem.Size = new Size(54, 20);
            debugToolStripMenuItem.Text = "Debug";
            // 
            // displayPulmonicConsonantsToolStripMenuItem
            // 
            displayPulmonicConsonantsToolStripMenuItem.Name = "displayPulmonicConsonantsToolStripMenuItem";
            displayPulmonicConsonantsToolStripMenuItem.Size = new Size(240, 22);
            displayPulmonicConsonantsToolStripMenuItem.Text = "Display Pulmonic Consonants";
            displayPulmonicConsonantsToolStripMenuItem.Click += DisplayPulmonicConsonantsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(240, 22);
            toolStripMenuItem2.Text = "Display Supersegmental";
            toolStripMenuItem2.Click += DisplaySupersegmentals_Click;
            // 
            // printIPAMapToolStripMenuItem
            // 
            printIPAMapToolStripMenuItem.Name = "printIPAMapToolStripMenuItem";
            printIPAMapToolStripMenuItem.Size = new Size(240, 22);
            printIPAMapToolStripMenuItem.Text = "Print IPA Map";
            printIPAMapToolStripMenuItem.Click += PrintIPAMapToolStripMenuItem_Click;
            // 
            // printKiToolStripMenuItem
            // 
            printKiToolStripMenuItem.Name = "printKiToolStripMenuItem";
            printKiToolStripMenuItem.Size = new Size(240, 22);
            printKiToolStripMenuItem.Text = "Print Kirshenbaum Missing Info";
            printKiToolStripMenuItem.Click += PrintKiToolStripMenuItem_Click;
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
            txt_SampleText.TextChanged += Txt_SampleText_TextChanged;
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
            txt_phonetic.Font = new Font("Charis SIL", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_phonetic.Location = new Point(513, 47);
            txt_phonetic.Multiline = true;
            txt_phonetic.Name = "txt_phonetic";
            txt_phonetic.ReadOnly = true;
            txt_phonetic.ScrollBars = ScrollBars.Vertical;
            txt_phonetic.Size = new Size(478, 109);
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
            btn_generate.Click += Btn_generate_Click;
            // 
            // btn_generateSpeech
            // 
            btn_generateSpeech.Location = new Point(15, 195);
            btn_generateSpeech.Name = "btn_generateSpeech";
            btn_generateSpeech.Size = new Size(136, 23);
            btn_generateSpeech.TabIndex = 6;
            btn_generateSpeech.Text = "Generate Speech";
            btn_generateSpeech.UseVisualStyleBackColor = true;
            btn_generateSpeech.Click += Btn_generateSpeech_Click;
            // 
            // cbx_recordings
            // 
            cbx_recordings.DropDownStyle = ComboBoxStyle.DropDownList;
            cbx_recordings.FormattingEnabled = true;
            cbx_recordings.ItemHeight = 15;
            cbx_recordings.Location = new Point(157, 195);
            cbx_recordings.Name = "cbx_recordings";
            cbx_recordings.Size = new Size(350, 23);
            cbx_recordings.Sorted = true;
            cbx_recordings.TabIndex = 7;
            // 
            // btn_replaySpeech
            // 
            btn_replaySpeech.Location = new Point(283, 168);
            btn_replaySpeech.Name = "btn_replaySpeech";
            btn_replaySpeech.Size = new Size(224, 23);
            btn_replaySpeech.TabIndex = 8;
            btn_replaySpeech.Text = "Replay Selected Recording";
            btn_replaySpeech.UseVisualStyleBackColor = true;
            btn_replaySpeech.Click += Btn_replaySpeech_Click;
            // 
            // pb_status
            // 
            pb_status.Location = new Point(12, 22);
            pb_status.Name = "pb_status";
            pb_status.Size = new Size(976, 23);
            pb_status.TabIndex = 9;
            pb_status.Visible = false;
            // 
            // pbTimer
            // 
            pbTimer.Tick += PbTimer_Tick;
            // 
            // lbl_choiceCbx
            // 
            lbl_choiceCbx.AutoSize = true;
            lbl_choiceCbx.Location = new Point(15, 269);
            lbl_choiceCbx.Name = "lbl_choiceCbx";
            lbl_choiceCbx.Size = new Size(114, 15);
            lbl_choiceCbx.TabIndex = 11;
            lbl_choiceCbx.Text = "Phoneme to change";
            // 
            // lbl_replacementCbx
            // 
            lbl_replacementCbx.AutoSize = true;
            lbl_replacementCbx.Location = new Point(15, 319);
            lbl_replacementCbx.Name = "lbl_replacementCbx";
            lbl_replacementCbx.Size = new Size(130, 15);
            lbl_replacementCbx.TabIndex = 12;
            lbl_replacementCbx.Text = "Replacement Phoneme";
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
            cbx_phonemeToChange.SelectedIndexChanged += Cbx_phonemeToChange_SelectedIndexChanged;
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
            btn_applyChangeToLanguage.Location = new Point(513, 422);
            btn_applyChangeToLanguage.Name = "btn_applyChangeToLanguage";
            btn_applyChangeToLanguage.Size = new Size(246, 23);
            btn_applyChangeToLanguage.TabIndex = 15;
            btn_applyChangeToLanguage.Text = "Apply Current Change to the Language";
            btn_applyChangeToLanguage.UseVisualStyleBackColor = true;
            btn_applyChangeToLanguage.Click += Btn_applyChangeToLanguage_Click;
            // 
            // btn_revertLastChange
            // 
            btn_revertLastChange.Location = new Point(763, 451);
            btn_revertLastChange.Name = "btn_revertLastChange";
            btn_revertLastChange.Size = new Size(226, 23);
            btn_revertLastChange.TabIndex = 16;
            btn_revertLastChange.Text = "Revert the last change";
            btn_revertLastChange.UseVisualStyleBackColor = true;
            btn_revertLastChange.Click += Btn_revertLastChange_Click;
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
            cbx_voice.Location = new Point(15, 224);
            cbx_voice.Name = "cbx_voice";
            cbx_voice.Size = new Size(246, 23);
            cbx_voice.Sorted = true;
            cbx_voice.TabIndex = 20;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(15, 250);
            label6.Name = "label6";
            label6.Size = new Size(70, 15);
            label6.TabIndex = 21;
            label6.Text = "Voice to use";
            // 
            // cbx_speed
            // 
            cbx_speed.FormattingEnabled = true;
            cbx_speed.Location = new Point(264, 224);
            cbx_speed.Name = "cbx_speed";
            cbx_speed.Size = new Size(243, 23);
            cbx_speed.TabIndex = 22;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(264, 250);
            label7.Name = "label7";
            label7.Size = new Size(76, 15);
            label7.TabIndex = 23;
            label7.Text = "Speed of text";
            // 
            // txt_changeList
            // 
            txt_changeList.Enabled = false;
            txt_changeList.Location = new Point(109, 423);
            txt_changeList.Multiline = true;
            txt_changeList.Name = "txt_changeList";
            txt_changeList.ScrollBars = ScrollBars.Vertical;
            txt_changeList.Size = new Size(398, 51);
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
            btn_addCurrentChangeToList.Location = new Point(765, 422);
            btn_addCurrentChangeToList.Name = "btn_addCurrentChangeToList";
            btn_addCurrentChangeToList.Size = new Size(226, 23);
            btn_addCurrentChangeToList.TabIndex = 28;
            btn_addCurrentChangeToList.Text = "Add Current Change to List of Changes";
            btn_addCurrentChangeToList.UseVisualStyleBackColor = true;
            btn_addCurrentChangeToList.Click += Btn_addCurrentChangeToList_Click;
            // 
            // btn_applyListOfChanges
            // 
            btn_applyListOfChanges.Location = new Point(513, 451);
            btn_applyListOfChanges.Name = "btn_applyListOfChanges";
            btn_applyListOfChanges.Size = new Size(244, 23);
            btn_applyListOfChanges.TabIndex = 29;
            btn_applyListOfChanges.Text = "Apply All Listed Changes to the Language (Simultaneously)";
            btn_applyListOfChanges.UseVisualStyleBackColor = true;
            btn_applyListOfChanges.Click += Btn_applyListOfChanges_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(160, 176);
            label9.Name = "label9";
            label9.Size = new Size(117, 15);
            label9.TabIndex = 30;
            label9.Text = "Available Recordings";
            // 
            // cbx_speechEngine
            // 
            cbx_speechEngine.FormattingEnabled = true;
            cbx_speechEngine.Location = new Point(515, 169);
            cbx_speechEngine.Name = "cbx_speechEngine";
            cbx_speechEngine.Size = new Size(161, 23);
            cbx_speechEngine.TabIndex = 31;
            cbx_speechEngine.SelectedIndexChanged += Cbx_speechEngine_SelectedIndexChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(515, 199);
            label10.Name = "label10";
            label10.Size = new Size(145, 15);
            label10.TabIndex = 32;
            label10.Text = "Speech Generation Engine";
            // 
            // tabPhoneticAlterations
            // 
            tabPhoneticAlterations.Controls.Add(tabPageConsonants);
            tabPhoneticAlterations.Controls.Add(tabPageVowels);
            tabPhoneticAlterations.Controls.Add(tabPageVowelDiphthongs);
            tabPhoneticAlterations.Controls.Add(tabPageRhoticity);
            tabPhoneticAlterations.Controls.Add(tabPageSpecialOperations);
            tabPhoneticAlterations.Location = new Point(515, 224);
            tabPhoneticAlterations.Name = "tabPhoneticAlterations";
            tabPhoneticAlterations.SelectedIndex = 0;
            tabPhoneticAlterations.Size = new Size(473, 192);
            tabPhoneticAlterations.TabIndex = 33;
            // 
            // tabPageConsonants
            // 
            tabPageConsonants.Controls.Add(gbx_replacementPhomeDepth);
            tabPageConsonants.Location = new Point(4, 24);
            tabPageConsonants.Name = "tabPageConsonants";
            tabPageConsonants.Padding = new Padding(3);
            tabPageConsonants.Size = new Size(465, 164);
            tabPageConsonants.TabIndex = 0;
            tabPageConsonants.Text = "Consonants";
            tabPageConsonants.UseVisualStyleBackColor = true;
            // 
            // gbx_replacementPhomeDepth
            // 
            gbx_replacementPhomeDepth.Controls.Add(rbn_allPhonemes);
            gbx_replacementPhomeDepth.Controls.Add(rbn_l3);
            gbx_replacementPhomeDepth.Controls.Add(rbn_l2);
            gbx_replacementPhomeDepth.Controls.Add(rbn_l1);
            gbx_replacementPhomeDepth.Location = new Point(6, 6);
            gbx_replacementPhomeDepth.Name = "gbx_replacementPhomeDepth";
            gbx_replacementPhomeDepth.Size = new Size(407, 42);
            gbx_replacementPhomeDepth.TabIndex = 25;
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
            rbn_allPhonemes.CheckedChanged += Rbn_allPhonemes_CheckedChanged;
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
            rbn_l3.CheckedChanged += Rbn_l3_CheckedChanged;
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
            rbn_l2.CheckedChanged += Rbn_l2_CheckedChanged;
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
            rbn_l1.CheckedChanged += Rbn_l1_CheckedChanged;
            // 
            // tabPageVowels
            // 
            tabPageVowels.Controls.Add(gbx_vowelReplacements);
            tabPageVowels.Location = new Point(4, 24);
            tabPageVowels.Name = "tabPageVowels";
            tabPageVowels.Padding = new Padding(3);
            tabPageVowels.Size = new Size(465, 164);
            tabPageVowels.TabIndex = 1;
            tabPageVowels.Text = "Vowels (Single)";
            tabPageVowels.UseVisualStyleBackColor = true;
            // 
            // gbx_vowelReplacements
            // 
            gbx_vowelReplacements.Controls.Add(rbn_longVowels);
            gbx_vowelReplacements.Controls.Add(rbn_semivowelDiphthong);
            gbx_vowelReplacements.Controls.Add(rbn_halfLongVowels);
            gbx_vowelReplacements.Controls.Add(rbn_normalVowel);
            gbx_vowelReplacements.Location = new Point(6, 10);
            gbx_vowelReplacements.Name = "gbx_vowelReplacements";
            gbx_vowelReplacements.Size = new Size(453, 76);
            gbx_vowelReplacements.TabIndex = 26;
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
            rbn_longVowels.CheckedChanged += Rbn_longVowels_CheckedChanged;
            // 
            // rbn_semivowelDiphthong
            // 
            rbn_semivowelDiphthong.AutoSize = true;
            rbn_semivowelDiphthong.Location = new Point(6, 34);
            rbn_semivowelDiphthong.Name = "rbn_semivowelDiphthong";
            rbn_semivowelDiphthong.Size = new Size(182, 19);
            rbn_semivowelDiphthong.TabIndex = 3;
            rbn_semivowelDiphthong.TabStop = true;
            rbn_semivowelDiphthong.Text = "Create semi-vowel diphthong";
            rbn_semivowelDiphthong.UseVisualStyleBackColor = true;
            rbn_semivowelDiphthong.CheckedChanged += Rbn_semivowelDiphthong_CheckedChanged;
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
            rbn_halfLongVowels.CheckedChanged += Rbn_halfLongVowels_CheckedChanged;
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
            rbn_normalVowel.CheckedChanged += Rbn_normalVowel_CheckedChanged;
            // 
            // tabPageVowelDiphthongs
            // 
            tabPageVowelDiphthongs.Controls.Add(lbl_DiphthongEndVowel);
            tabPageVowelDiphthongs.Controls.Add(cbx_dipthongEndVowel);
            tabPageVowelDiphthongs.Controls.Add(lbl_diphthongStartVowel);
            tabPageVowelDiphthongs.Controls.Add(cbx_dipthongStartVowel);
            tabPageVowelDiphthongs.Controls.Add(rbn_diphthongReplacement);
            tabPageVowelDiphthongs.Controls.Add(rbn_changeEndVowel);
            tabPageVowelDiphthongs.Controls.Add(rbn_changeStartVowel);
            tabPageVowelDiphthongs.Controls.Add(rbn_vowelToDipthongEnd);
            tabPageVowelDiphthongs.Controls.Add(rbn_vowelToDiphthongStart);
            tabPageVowelDiphthongs.Location = new Point(4, 24);
            tabPageVowelDiphthongs.Name = "tabPageVowelDiphthongs";
            tabPageVowelDiphthongs.Padding = new Padding(3);
            tabPageVowelDiphthongs.Size = new Size(465, 164);
            tabPageVowelDiphthongs.TabIndex = 2;
            tabPageVowelDiphthongs.Text = "Vowel Diphthongs";
            tabPageVowelDiphthongs.UseVisualStyleBackColor = true;
            // 
            // lbl_DiphthongEndVowel
            // 
            lbl_DiphthongEndVowel.AutoSize = true;
            lbl_DiphthongEndVowel.Location = new Point(234, 76);
            lbl_DiphthongEndVowel.Name = "lbl_DiphthongEndVowel";
            lbl_DiphthongEndVowel.Size = new Size(114, 15);
            lbl_DiphthongEndVowel.TabIndex = 8;
            lbl_DiphthongEndVowel.Text = "Dipthong End Vowel";
            // 
            // cbx_dipthongEndVowel
            // 
            cbx_dipthongEndVowel.FormattingEnabled = true;
            cbx_dipthongEndVowel.Location = new Point(234, 93);
            cbx_dipthongEndVowel.Name = "cbx_dipthongEndVowel";
            cbx_dipthongEndVowel.Size = new Size(225, 23);
            cbx_dipthongEndVowel.TabIndex = 7;
            // 
            // lbl_diphthongStartVowel
            // 
            lbl_diphthongStartVowel.AutoSize = true;
            lbl_diphthongStartVowel.Location = new Point(6, 76);
            lbl_diphthongStartVowel.Name = "lbl_diphthongStartVowel";
            lbl_diphthongStartVowel.Size = new Size(118, 15);
            lbl_diphthongStartVowel.TabIndex = 6;
            lbl_diphthongStartVowel.Text = "Dipthong Start Vowel";
            // 
            // cbx_dipthongStartVowel
            // 
            cbx_dipthongStartVowel.FormattingEnabled = true;
            cbx_dipthongStartVowel.Location = new Point(6, 94);
            cbx_dipthongStartVowel.Name = "cbx_dipthongStartVowel";
            cbx_dipthongStartVowel.Size = new Size(222, 23);
            cbx_dipthongStartVowel.TabIndex = 5;
            // 
            // rbn_diphthongReplacement
            // 
            rbn_diphthongReplacement.AutoSize = true;
            rbn_diphthongReplacement.Location = new Point(6, 56);
            rbn_diphthongReplacement.Name = "rbn_diphthongReplacement";
            rbn_diphthongReplacement.Size = new Size(152, 19);
            rbn_diphthongReplacement.TabIndex = 4;
            rbn_diphthongReplacement.Text = "Dipththong Replacemnt";
            rbn_diphthongReplacement.UseVisualStyleBackColor = true;
            rbn_diphthongReplacement.CheckedChanged += Rbn_diphthongReplacement_CheckedChanged;
            // 
            // rbn_changeEndVowel
            // 
            rbn_changeEndVowel.AutoSize = true;
            rbn_changeEndVowel.Location = new Point(139, 31);
            rbn_changeEndVowel.Name = "rbn_changeEndVowel";
            rbn_changeEndVowel.Size = new Size(140, 19);
            rbn_changeEndVowel.TabIndex = 3;
            rbn_changeEndVowel.Text = "Change Ending Vowel";
            rbn_changeEndVowel.UseVisualStyleBackColor = true;
            rbn_changeEndVowel.CheckedChanged += Rbn_ChangeEndVowel_CheckedChanged;
            // 
            // rbn_changeStartVowel
            // 
            rbn_changeStartVowel.AutoSize = true;
            rbn_changeStartVowel.Location = new Point(6, 31);
            rbn_changeStartVowel.Name = "rbn_changeStartVowel";
            rbn_changeStartVowel.Size = new Size(127, 19);
            rbn_changeStartVowel.TabIndex = 2;
            rbn_changeStartVowel.Text = "Change Start Vowel";
            rbn_changeStartVowel.UseVisualStyleBackColor = true;
            rbn_changeStartVowel.CheckedChanged += Rbn_changeStartVowel_CheckedChanged;
            // 
            // rbn_vowelToDipthongEnd
            // 
            rbn_vowelToDipthongEnd.AutoSize = true;
            rbn_vowelToDipthongEnd.Location = new Point(190, 6);
            rbn_vowelToDipthongEnd.Name = "rbn_vowelToDipthongEnd";
            rbn_vowelToDipthongEnd.Size = new Size(160, 19);
            rbn_vowelToDipthongEnd.TabIndex = 1;
            rbn_vowelToDipthongEnd.Text = "Vowel to End of Dipthong";
            rbn_vowelToDipthongEnd.UseVisualStyleBackColor = true;
            rbn_vowelToDipthongEnd.CheckedChanged += Rbn_vowelToDiphthongEnd_CheckedChanged;
            // 
            // rbn_vowelToDiphthongStart
            // 
            rbn_vowelToDiphthongStart.AutoSize = true;
            rbn_vowelToDiphthongStart.Checked = true;
            rbn_vowelToDiphthongStart.Location = new Point(6, 6);
            rbn_vowelToDiphthongStart.Name = "rbn_vowelToDiphthongStart";
            rbn_vowelToDiphthongStart.Size = new Size(164, 19);
            rbn_vowelToDiphthongStart.TabIndex = 0;
            rbn_vowelToDiphthongStart.TabStop = true;
            rbn_vowelToDiphthongStart.Text = "Vowel to Start of Dipthong";
            rbn_vowelToDiphthongStart.UseVisualStyleBackColor = true;
            rbn_vowelToDiphthongStart.CheckedChanged += Rbn_vowelToDiphthongStart_CheckedChanged;
            // 
            // tabPageRhoticity
            // 
            tabPageRhoticity.Controls.Add(rbn_replaceRSpelling);
            tabPageRhoticity.Controls.Add(rbn_replaceRhotacized);
            tabPageRhoticity.Controls.Add(rbn_removeRhoticity);
            tabPageRhoticity.Controls.Add(rbn_longToRhotacized);
            tabPageRhoticity.Controls.Add(rbn_addRhoticityRegular);
            tabPageRhoticity.Location = new Point(4, 24);
            tabPageRhoticity.Name = "tabPageRhoticity";
            tabPageRhoticity.Padding = new Padding(3);
            tabPageRhoticity.Size = new Size(465, 164);
            tabPageRhoticity.TabIndex = 4;
            tabPageRhoticity.Text = "Rhoticity";
            tabPageRhoticity.UseVisualStyleBackColor = true;
            // 
            // rbn_replaceRSpelling
            // 
            rbn_replaceRSpelling.AutoSize = true;
            rbn_replaceRSpelling.Location = new Point(212, 31);
            rbn_replaceRSpelling.Name = "rbn_replaceRSpelling";
            rbn_replaceRSpelling.Size = new Size(171, 19);
            rbn_replaceRSpelling.TabIndex = 4;
            rbn_replaceRSpelling.TabStop = true;
            rbn_replaceRSpelling.Text = "Rhotacize based on spelling";
            rbn_replaceRSpelling.UseVisualStyleBackColor = true;
            rbn_replaceRSpelling.CheckedChanged += Rbn_replaceRSpelling_CheckedChanged;
            // 
            // rbn_replaceRhotacized
            // 
            rbn_replaceRhotacized.AutoSize = true;
            rbn_replaceRhotacized.Location = new Point(6, 31);
            rbn_replaceRhotacized.Name = "rbn_replaceRhotacized";
            rbn_replaceRhotacized.Size = new Size(204, 19);
            rbn_replaceRhotacized.TabIndex = 3;
            rbn_replaceRhotacized.Text = "Replace Rhoticized with Phoneme";
            rbn_replaceRhotacized.UseVisualStyleBackColor = true;
            rbn_replaceRhotacized.CheckedChanged += Rbn_replaceRhotacized_CheckedChanged;
            // 
            // rbn_removeRhoticity
            // 
            rbn_removeRhoticity.AutoSize = true;
            rbn_removeRhoticity.Location = new Point(312, 6);
            rbn_removeRhoticity.Name = "rbn_removeRhoticity";
            rbn_removeRhoticity.Size = new Size(118, 19);
            rbn_removeRhoticity.TabIndex = 2;
            rbn_removeRhoticity.Text = "Remove Rhoticity";
            rbn_removeRhoticity.UseVisualStyleBackColor = true;
            rbn_removeRhoticity.CheckedChanged += Rbn_removeRhoticity_CheckedChanged;
            // 
            // rbn_longToRhotacized
            // 
            rbn_longToRhotacized.AutoSize = true;
            rbn_longToRhotacized.Location = new Point(157, 6);
            rbn_longToRhotacized.Name = "rbn_longToRhotacized";
            rbn_longToRhotacized.Size = new Size(149, 19);
            rbn_longToRhotacized.TabIndex = 1;
            rbn_longToRhotacized.Text = "Rhotacized long vowels";
            rbn_longToRhotacized.UseVisualStyleBackColor = true;
            rbn_longToRhotacized.CheckedChanged += Rbn_longToRhotacized_CheckedChanged;
            // 
            // rbn_addRhoticityRegular
            // 
            rbn_addRhoticityRegular.AutoSize = true;
            rbn_addRhoticityRegular.Checked = true;
            rbn_addRhoticityRegular.Location = new Point(6, 6);
            rbn_addRhoticityRegular.Name = "rbn_addRhoticityRegular";
            rbn_addRhoticityRegular.Size = new Size(145, 19);
            rbn_addRhoticityRegular.TabIndex = 0;
            rbn_addRhoticityRegular.TabStop = true;
            rbn_addRhoticityRegular.Text = "Add Rhoticity (regular)";
            rbn_addRhoticityRegular.UseVisualStyleBackColor = true;
            rbn_addRhoticityRegular.CheckedChanged += Rbn_addRhoticityRegular_CheckedChanged;
            // 
            // tabPageSpecialOperations
            // 
            tabPageSpecialOperations.Controls.Add(btn_updateSoundMapList);
            tabPageSpecialOperations.Location = new Point(4, 24);
            tabPageSpecialOperations.Name = "tabPageSpecialOperations";
            tabPageSpecialOperations.Padding = new Padding(3);
            tabPageSpecialOperations.Size = new Size(465, 164);
            tabPageSpecialOperations.TabIndex = 3;
            tabPageSpecialOperations.Text = "Special Operations";
            tabPageSpecialOperations.UseVisualStyleBackColor = true;
            // 
            // btn_updateSoundMapList
            // 
            btn_updateSoundMapList.Location = new Point(6, 13);
            btn_updateSoundMapList.Name = "btn_updateSoundMapList";
            btn_updateSoundMapList.Size = new Size(188, 23);
            btn_updateSoundMapList.TabIndex = 0;
            btn_updateSoundMapList.Text = "Update Spelling/Pronunciation";
            btn_updateSoundMapList.UseVisualStyleBackColor = true;
            btn_updateSoundMapList.Click += Btn_updateSoundMapList_Click;
            // 
            // LanguageHoningForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 496);
            Controls.Add(tabPhoneticAlterations);
            Controls.Add(label10);
            Controls.Add(cbx_speechEngine);
            Controls.Add(label9);
            Controls.Add(btn_applyListOfChanges);
            Controls.Add(btn_addCurrentChangeToList);
            Controls.Add(label8);
            Controls.Add(txt_changeList);
            Controls.Add(label7);
            Controls.Add(cbx_speed);
            Controls.Add(label6);
            Controls.Add(cbx_voice);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btn_revertLastChange);
            Controls.Add(btn_applyChangeToLanguage);
            Controls.Add(cbx_replacementPhoneme);
            Controls.Add(cbx_phonemeToChange);
            Controls.Add(lbl_replacementCbx);
            Controls.Add(lbl_choiceCbx);
            Controls.Add(btn_replaySpeech);
            Controls.Add(cbx_recordings);
            Controls.Add(btn_generateSpeech);
            Controls.Add(btn_generate);
            Controls.Add(lbl_phonetic);
            Controls.Add(txt_phonetic);
            Controls.Add(lbl_SampleText);
            Controls.Add(txt_SampleText);
            Controls.Add(menuStrip1);
            Controls.Add(pb_status);
            MainMenuStrip = menuStrip1;
            Name = "LanguageHoningForm";
            Text = "LanguageHoningForm";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabPhoneticAlterations.ResumeLayout(false);
            tabPageConsonants.ResumeLayout(false);
            gbx_replacementPhomeDepth.ResumeLayout(false);
            gbx_replacementPhomeDepth.PerformLayout();
            tabPageVowels.ResumeLayout(false);
            gbx_vowelReplacements.ResumeLayout(false);
            gbx_vowelReplacements.PerformLayout();
            tabPageVowelDiphthongs.ResumeLayout(false);
            tabPageVowelDiphthongs.PerformLayout();
            tabPageRhoticity.ResumeLayout(false);
            tabPageRhoticity.PerformLayout();
            tabPageSpecialOperations.ResumeLayout(false);
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
        private Label lbl_choiceCbx;
        private Label lbl_replacementCbx;
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
        private TextBox txt_changeList;
        private Label label8;
        private Button btn_addCurrentChangeToList;
        private Button btn_applyListOfChanges;
        private ToolStripMenuItem utilitiesToolStripMenuItem;
        private ToolStripMenuItem displayGlossOfSampleTextToolStripMenuItem;
        private ToolStripMenuItem printSampleTextSummaryToolStripMenuItem;
        private ToolStripMenuItem printKiToolStripMenuItem;
        private Label label9;
        private ComboBox cbx_speechEngine;
        private Label label10;
        private TabControl tabPhoneticAlterations;
        private TabPage tabPageConsonants;
        private GroupBox gbx_replacementPhomeDepth;
        private RadioButton rbn_allPhonemes;
        private RadioButton rbn_l3;
        private RadioButton rbn_l2;
        private RadioButton rbn_l1;
        private TabPage tabPageVowels;
        private GroupBox gbx_vowelReplacements;
        private RadioButton rbn_longVowels;
        private RadioButton rbn_semivowelDiphthong;
        private RadioButton rbn_halfLongVowels;
        private RadioButton rbn_normalVowel;
        private TabPage tabPageVowelDiphthongs;
        private TabPage tabPageSpecialOperations;
        private TabPage tabPageRhoticity;
        private Button btn_updateSoundMapList;
        private RadioButton rbn_replaceRhotacized;
        private RadioButton rbn_removeRhoticity;
        private RadioButton rbn_longToRhotacized;
        private RadioButton rbn_addRhoticityRegular;
        private RadioButton rbn_replaceRSpelling;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem setAmazonPollyURIToolStripMenuItem;
        private ToolStripMenuItem setESpeakNgLocationToolStripMenuItem;
        private RadioButton rbn_changeEndVowel;
        private RadioButton rbn_changeStartVowel;
        private RadioButton rbn_vowelToDipthongEnd;
        private RadioButton rbn_vowelToDiphthongStart;
        private Label lbl_DiphthongEndVowel;
        private ComboBox cbx_dipthongEndVowel;
        private Label lbl_diphthongStartVowel;
        private ComboBox cbx_dipthongStartVowel;
        private RadioButton rbn_diphthongReplacement;
    }
}