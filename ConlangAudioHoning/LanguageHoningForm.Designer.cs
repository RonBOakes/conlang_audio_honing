﻿/*
 * Language Honing Form designer code
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
            useCompactJsonToolStripItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripMenuItem1 = new ToolStripMenuItem();
            saveSampleMenu = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            languageToolStripMenuItem = new ToolStripMenuItem();
            editLexiconToolStripMenuItem = new ToolStripMenuItem();
            declineToolStripMenuItem = new ToolStripMenuItem();
            deriveToolStripMenuItem = new ToolStripMenuItem();
            setAndAdjustLexicalOrderToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            generatePhonemeClustersToolStripMenuItem = new ToolStripMenuItem();
            listNADAnalysisClustersToolStripMenuItem = new ToolStripMenuItem();
            exportNADClusterCountsToolStripMenuItem = new ToolStripMenuItem();
            utilitiesToolStripMenuItem = new ToolStripMenuItem();
            displayGlossOfSampleTextToolStripMenuItem = new ToolStripMenuItem();
            printSampleTextSummaryToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            useSharedAmazonPollyToolStripMenuItem = new ToolStripMenuItem();
            setAmazonPollyURIToolStripMenuItem = new ToolStripMenuItem();
            setAmazonPollyAuthorizationEmailToolStripMenuItem = new ToolStripMenuItem();
            setAmazonPollyAuthorizationPasswordToolStripMenuItem = new ToolStripMenuItem();
            setAmazonPollyProfileToolStripMenuItem = new ToolStripMenuItem();
            setAmazonPollyS3BucketNameToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            setESpeakNgLocationToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            setAzureSpeechKeyToolStripMenuItem = new ToolStripMenuItem();
            setAzureSpeechRegionToolStripMenuItem = new ToolStripMenuItem();
            debugToolStripMenuItem = new ToolStripMenuItem();
            displayPulmonicConsonantsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            printIPAMapToolStripMenuItem = new ToolStripMenuItem();
            printKiToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            viewHelpToolStripMenuItem = new ToolStripMenuItem();
            aboutConlangAudioHoningToolStripMenuItem = new ToolStripMenuItem();
            txt_SampleText = new TextBox();
            lbl_SampleText = new Label();
            txt_phonetic = new TextBox();
            lbl_phonetic = new Label();
            btn_generate = new Button();
            btn_generateSpeech = new Button();
            cbx_recordings = new ComboBox();
            btn_replaySpeech = new Button();
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
            lbl_customReplacementPattern = new Label();
            txt_customReplacementPattern = new TextBox();
            gbx_scopeOfConsonantReplacement = new GroupBox();
            chk_changeNotMatchLocations = new CheckBox();
            rbn_replaceConsonantsPattern = new RadioButton();
            rbn_replaceConsonantsEndOfWord = new RadioButton();
            rbn_replaceConsonantsStartOfWords = new RadioButton();
            rbn_replaceConsonantsGlobally = new RadioButton();
            gbx_replacementPhonemeDepth = new GroupBox();
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
            rbn_removeEndVowel = new RadioButton();
            rbn_removeStartVowel = new RadioButton();
            lbl_DiphthongEndVowel = new Label();
            cbx_diphthongEndVowel = new ComboBox();
            lbl_diphthongStartVowel = new Label();
            cbx_diphthongStartVowel = new ComboBox();
            rbn_diphthongReplacement = new RadioButton();
            rbn_changeEndVowel = new RadioButton();
            rbn_changeStartVowel = new RadioButton();
            rbn_vowelToDiphthongEnd = new RadioButton();
            rbn_vowelToDiphthongStart = new RadioButton();
            tabPageRhoticity = new TabPage();
            rbn_replaceRSpelling = new RadioButton();
            rbn_replaceRhotacized = new RadioButton();
            rbn_removeRhoticity = new RadioButton();
            rbn_longToRhotacized = new RadioButton();
            rbn_addRhoticityRegular = new RadioButton();
            tabPageSpecialOperations = new TabPage();
            btn_replaceCluster = new Button();
            btn_updateSoundMapList = new Button();
            tb_Volume = new TrackBar();
            label1 = new Label();
            exportBeatsAndBindingsClusterCountsToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            tabPhoneticAlterations.SuspendLayout();
            tabPageConsonants.SuspendLayout();
            gbx_scopeOfConsonantReplacement.SuspendLayout();
            gbx_replacementPhonemeDepth.SuspendLayout();
            tabPageVowels.SuspendLayout();
            gbx_vowelReplacements.SuspendLayout();
            tabPageVowelDiphthongs.SuspendLayout();
            tabPageRhoticity.SuspendLayout();
            tabPageSpecialOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tb_Volume).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, languageToolStripMenuItem, utilitiesToolStripMenuItem, debugToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1000, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadToolStripMenuItem, saveToolStripMenuItem, useCompactJsonToolStripItem, toolStripSeparator2, toolStripMenuItem1, saveSampleMenu, toolStripSeparator1, exitToolStripMenuItem });
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
            // useCompactJsonToolStripItem
            // 
            useCompactJsonToolStripItem.Name = "useCompactJsonToolStripItem";
            useCompactJsonToolStripItem.Size = new Size(227, 22);
            useCompactJsonToolStripItem.Text = "Use Compact JSON";
            useCompactJsonToolStripItem.Click += UseCompactJsonToolStripItem_Click;
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
            languageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { editLexiconToolStripMenuItem, declineToolStripMenuItem, deriveToolStripMenuItem, setAndAdjustLexicalOrderToolStripMenuItem, toolStripSeparator6, generatePhonemeClustersToolStripMenuItem, listNADAnalysisClustersToolStripMenuItem, exportNADClusterCountsToolStripMenuItem, exportBeatsAndBindingsClusterCountsToolStripMenuItem });
            languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            languageToolStripMenuItem.Size = new Size(71, 20);
            languageToolStripMenuItem.Text = "Language";
            // 
            // editLexiconToolStripMenuItem
            // 
            editLexiconToolStripMenuItem.Name = "editLexiconToolStripMenuItem";
            editLexiconToolStripMenuItem.Size = new Size(222, 22);
            editLexiconToolStripMenuItem.Text = "Edit Lexicon";
            editLexiconToolStripMenuItem.Click += EditLexiconToolStripMenuItem_Click;
            // 
            // declineToolStripMenuItem
            // 
            declineToolStripMenuItem.Name = "declineToolStripMenuItem";
            declineToolStripMenuItem.Size = new Size(222, 22);
            declineToolStripMenuItem.Text = "Decline Language";
            declineToolStripMenuItem.Click += DeclineToolStripMenuItem_Click;
            // 
            // deriveToolStripMenuItem
            // 
            deriveToolStripMenuItem.Name = "deriveToolStripMenuItem";
            deriveToolStripMenuItem.Size = new Size(222, 22);
            deriveToolStripMenuItem.Text = "Derive Words";
            deriveToolStripMenuItem.Click += DeriveToolStripMenuItem_Click;
            // 
            // setAndAdjustLexicalOrderToolStripMenuItem
            // 
            setAndAdjustLexicalOrderToolStripMenuItem.Name = "setAndAdjustLexicalOrderToolStripMenuItem";
            setAndAdjustLexicalOrderToolStripMenuItem.Size = new Size(222, 22);
            setAndAdjustLexicalOrderToolStripMenuItem.Text = "Set and Adjust Lexical Order";
            setAndAdjustLexicalOrderToolStripMenuItem.Click += SetAndAdjustLexicalOrderToolStripMenuItem_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(219, 6);
            // 
            // generatePhonemeClustersToolStripMenuItem
            // 
            generatePhonemeClustersToolStripMenuItem.Name = "generatePhonemeClustersToolStripMenuItem";
            generatePhonemeClustersToolStripMenuItem.Size = new Size(222, 22);
            generatePhonemeClustersToolStripMenuItem.Text = "Generate Phoneme Clusters";
            generatePhonemeClustersToolStripMenuItem.Click += generatePhonemeClustersToolStripMenuItem_Click;
            // 
            // listNADAnalysisClustersToolStripMenuItem
            // 
            listNADAnalysisClustersToolStripMenuItem.Name = "listNADAnalysisClustersToolStripMenuItem";
            listNADAnalysisClustersToolStripMenuItem.Size = new Size(222, 22);
            listNADAnalysisClustersToolStripMenuItem.Text = "List NAD Analysis Clusters";
            listNADAnalysisClustersToolStripMenuItem.Click += ListNADAnalysisClustersToolStripMenuItem_Click;
            // 
            // exportNADClusterCountsToolStripMenuItem
            // 
            exportNADClusterCountsToolStripMenuItem.Name = "exportNADClusterCountsToolStripMenuItem";
            exportNADClusterCountsToolStripMenuItem.Size = new Size(222, 22);
            exportNADClusterCountsToolStripMenuItem.Text = "Export NAD Cluster Counts";
            exportNADClusterCountsToolStripMenuItem.Click += ExportNADClusterCountsToolStripMenuItem_Click;
            // 
            // utilitiesToolStripMenuItem
            // 
            utilitiesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { displayGlossOfSampleTextToolStripMenuItem, printSampleTextSummaryToolStripMenuItem, toolStripSeparator3, useSharedAmazonPollyToolStripMenuItem, setAmazonPollyURIToolStripMenuItem, setAmazonPollyAuthorizationEmailToolStripMenuItem, setAmazonPollyAuthorizationPasswordToolStripMenuItem, setAmazonPollyProfileToolStripMenuItem, setAmazonPollyS3BucketNameToolStripMenuItem, toolStripSeparator4, setESpeakNgLocationToolStripMenuItem, toolStripSeparator5, setAzureSpeechKeyToolStripMenuItem, setAzureSpeechRegionToolStripMenuItem });
            utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            utilitiesToolStripMenuItem.Size = new Size(58, 20);
            utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // displayGlossOfSampleTextToolStripMenuItem
            // 
            displayGlossOfSampleTextToolStripMenuItem.Name = "displayGlossOfSampleTextToolStripMenuItem";
            displayGlossOfSampleTextToolStripMenuItem.Size = new Size(294, 22);
            displayGlossOfSampleTextToolStripMenuItem.Text = "Display Gloss of Sample Text";
            displayGlossOfSampleTextToolStripMenuItem.Click += DisplayGlossOfSampleTextToolStripMenuItem_Click;
            // 
            // printSampleTextSummaryToolStripMenuItem
            // 
            printSampleTextSummaryToolStripMenuItem.Name = "printSampleTextSummaryToolStripMenuItem";
            printSampleTextSummaryToolStripMenuItem.Size = new Size(294, 22);
            printSampleTextSummaryToolStripMenuItem.Text = "Print Sample Text Summary";
            printSampleTextSummaryToolStripMenuItem.Click += PrintSampleTextSummaryToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(291, 6);
            // 
            // useSharedAmazonPollyToolStripMenuItem
            // 
            useSharedAmazonPollyToolStripMenuItem.CheckOnClick = true;
            useSharedAmazonPollyToolStripMenuItem.Name = "useSharedAmazonPollyToolStripMenuItem";
            useSharedAmazonPollyToolStripMenuItem.Size = new Size(294, 22);
            useSharedAmazonPollyToolStripMenuItem.Text = "Use Shared Amazon Polly";
            useSharedAmazonPollyToolStripMenuItem.Click += UseSharedAmazonPollyToolStripMenuItem_Click;
            // 
            // setAmazonPollyURIToolStripMenuItem
            // 
            setAmazonPollyURIToolStripMenuItem.Name = "setAmazonPollyURIToolStripMenuItem";
            setAmazonPollyURIToolStripMenuItem.Size = new Size(294, 22);
            setAmazonPollyURIToolStripMenuItem.Text = "Set Amazon Polly URI";
            setAmazonPollyURIToolStripMenuItem.Click += SetAmazonPollyURIToolStripMenuItem_Click;
            // 
            // setAmazonPollyAuthorizationEmailToolStripMenuItem
            // 
            setAmazonPollyAuthorizationEmailToolStripMenuItem.Name = "setAmazonPollyAuthorizationEmailToolStripMenuItem";
            setAmazonPollyAuthorizationEmailToolStripMenuItem.Size = new Size(294, 22);
            setAmazonPollyAuthorizationEmailToolStripMenuItem.Text = "Set Amazon Polly Authorization email";
            setAmazonPollyAuthorizationEmailToolStripMenuItem.Click += SetAmazonPollyAuthorizationEmailToolStripMenuItem_Click;
            // 
            // setAmazonPollyAuthorizationPasswordToolStripMenuItem
            // 
            setAmazonPollyAuthorizationPasswordToolStripMenuItem.Name = "setAmazonPollyAuthorizationPasswordToolStripMenuItem";
            setAmazonPollyAuthorizationPasswordToolStripMenuItem.Size = new Size(294, 22);
            setAmazonPollyAuthorizationPasswordToolStripMenuItem.Text = "Set Amazon Polly Authorization Password";
            setAmazonPollyAuthorizationPasswordToolStripMenuItem.Click += SetAmazonPollyAuthorizationPasswordToolStripMenuItem_Click;
            // 
            // setAmazonPollyProfileToolStripMenuItem
            // 
            setAmazonPollyProfileToolStripMenuItem.Name = "setAmazonPollyProfileToolStripMenuItem";
            setAmazonPollyProfileToolStripMenuItem.Size = new Size(294, 22);
            setAmazonPollyProfileToolStripMenuItem.Text = "Set Amazon Polly Profile";
            setAmazonPollyProfileToolStripMenuItem.Click += SetAmazonPollyProfileToolStripMenuItem_Click;
            // 
            // setAmazonPollyS3BucketNameToolStripMenuItem
            // 
            setAmazonPollyS3BucketNameToolStripMenuItem.Name = "setAmazonPollyS3BucketNameToolStripMenuItem";
            setAmazonPollyS3BucketNameToolStripMenuItem.Size = new Size(294, 22);
            setAmazonPollyS3BucketNameToolStripMenuItem.Text = "Set Amazon Polly S3 Bucket Name";
            setAmazonPollyS3BucketNameToolStripMenuItem.Click += SetAmazonPollyS3BucketNameToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(291, 6);
            // 
            // setESpeakNgLocationToolStripMenuItem
            // 
            setESpeakNgLocationToolStripMenuItem.Name = "setESpeakNgLocationToolStripMenuItem";
            setESpeakNgLocationToolStripMenuItem.Size = new Size(294, 22);
            setESpeakNgLocationToolStripMenuItem.Text = "Set eSpeak-ng location";
            setESpeakNgLocationToolStripMenuItem.Click += SetESpeakNgLocationToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(291, 6);
            // 
            // setAzureSpeechKeyToolStripMenuItem
            // 
            setAzureSpeechKeyToolStripMenuItem.Name = "setAzureSpeechKeyToolStripMenuItem";
            setAzureSpeechKeyToolStripMenuItem.Size = new Size(294, 22);
            setAzureSpeechKeyToolStripMenuItem.Text = "Set Azure Speech Key";
            setAzureSpeechKeyToolStripMenuItem.Click += SetAzureSpeechKeyToolStripMenuItem_Click;
            // 
            // setAzureSpeechRegionToolStripMenuItem
            // 
            setAzureSpeechRegionToolStripMenuItem.Name = "setAzureSpeechRegionToolStripMenuItem";
            setAzureSpeechRegionToolStripMenuItem.Size = new Size(294, 22);
            setAzureSpeechRegionToolStripMenuItem.Text = "Set Azure Speech Region";
            setAzureSpeechRegionToolStripMenuItem.Click += SetAzureSpeechRegionToolStripMenuItem_Click;
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
            toolStripMenuItem2.Text = "Display Suprasegmental";
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
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { viewHelpToolStripMenuItem, aboutConlangAudioHoningToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.ShortcutKeys = Keys.F1;
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // viewHelpToolStripMenuItem
            // 
            viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            viewHelpToolStripMenuItem.ShortcutKeys = Keys.F1;
            viewHelpToolStripMenuItem.Size = new Size(233, 22);
            viewHelpToolStripMenuItem.Text = "View Help";
            viewHelpToolStripMenuItem.Click += ViewHelpToolStripMenuItem_Click;
            // 
            // aboutConlangAudioHoningToolStripMenuItem
            // 
            aboutConlangAudioHoningToolStripMenuItem.Name = "aboutConlangAudioHoningToolStripMenuItem";
            aboutConlangAudioHoningToolStripMenuItem.Size = new Size(233, 22);
            aboutConlangAudioHoningToolStripMenuItem.Text = "About Conlang Audio Honing";
            aboutConlangAudioHoningToolStripMenuItem.Click += AboutConlangAudioHoningToolStripMenuItem_Click;
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
            cbx_voice.SelectedIndexChanged += Cbx_voice_SelectedIndexChanged;
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
            tabPageConsonants.Controls.Add(lbl_customReplacementPattern);
            tabPageConsonants.Controls.Add(txt_customReplacementPattern);
            tabPageConsonants.Controls.Add(gbx_scopeOfConsonantReplacement);
            tabPageConsonants.Controls.Add(gbx_replacementPhonemeDepth);
            tabPageConsonants.Location = new Point(4, 24);
            tabPageConsonants.Name = "tabPageConsonants";
            tabPageConsonants.Padding = new Padding(3);
            tabPageConsonants.Size = new Size(465, 164);
            tabPageConsonants.TabIndex = 0;
            tabPageConsonants.Text = "Consonants";
            tabPageConsonants.UseVisualStyleBackColor = true;
            // 
            // lbl_customReplacementPattern
            // 
            lbl_customReplacementPattern.AutoSize = true;
            lbl_customReplacementPattern.Location = new Point(3, 144);
            lbl_customReplacementPattern.Name = "lbl_customReplacementPattern";
            lbl_customReplacementPattern.Size = new Size(215, 15);
            lbl_customReplacementPattern.TabIndex = 28;
            lbl_customReplacementPattern.Text = "Custom Replacement PatternToReplace";
            // 
            // txt_customReplacementPattern
            // 
            txt_customReplacementPattern.Location = new Point(3, 117);
            txt_customReplacementPattern.Name = "txt_customReplacementPattern";
            txt_customReplacementPattern.Size = new Size(453, 23);
            txt_customReplacementPattern.TabIndex = 27;
            // 
            // gbx_scopeOfConsonantReplacement
            // 
            gbx_scopeOfConsonantReplacement.Controls.Add(chk_changeNotMatchLocations);
            gbx_scopeOfConsonantReplacement.Controls.Add(rbn_replaceConsonantsPattern);
            gbx_scopeOfConsonantReplacement.Controls.Add(rbn_replaceConsonantsEndOfWord);
            gbx_scopeOfConsonantReplacement.Controls.Add(rbn_replaceConsonantsStartOfWords);
            gbx_scopeOfConsonantReplacement.Controls.Add(rbn_replaceConsonantsGlobally);
            gbx_scopeOfConsonantReplacement.Location = new Point(6, 50);
            gbx_scopeOfConsonantReplacement.Name = "gbx_scopeOfConsonantReplacement";
            gbx_scopeOfConsonantReplacement.Size = new Size(453, 67);
            gbx_scopeOfConsonantReplacement.TabIndex = 26;
            gbx_scopeOfConsonantReplacement.TabStop = false;
            gbx_scopeOfConsonantReplacement.Text = "Scope of Replacement";
            // 
            // chk_changeNotMatchLocations
            // 
            chk_changeNotMatchLocations.AutoSize = true;
            chk_changeNotMatchLocations.Location = new Point(6, 45);
            chk_changeNotMatchLocations.Name = "chk_changeNotMatchLocations";
            chk_changeNotMatchLocations.Size = new Size(229, 19);
            chk_changeNotMatchLocations.TabIndex = 4;
            chk_changeNotMatchLocations.Text = "Change where pattern does not match";
            chk_changeNotMatchLocations.UseVisualStyleBackColor = true;
            // 
            // rbn_replaceConsonantsPattern
            // 
            rbn_replaceConsonantsPattern.AutoSize = true;
            rbn_replaceConsonantsPattern.Location = new Point(290, 19);
            rbn_replaceConsonantsPattern.Name = "rbn_replaceConsonantsPattern";
            rbn_replaceConsonantsPattern.Size = new Size(116, 19);
            rbn_replaceConsonantsPattern.TabIndex = 3;
            rbn_replaceConsonantsPattern.TabStop = true;
            rbn_replaceConsonantsPattern.Text = "Custom (pattern)";
            rbn_replaceConsonantsPattern.UseVisualStyleBackColor = true;
            rbn_replaceConsonantsPattern.CheckedChanged += Rbn_replaceConsonantsPattern_CheckedChanged;
            // 
            // rbn_replaceConsonantsEndOfWord
            // 
            rbn_replaceConsonantsEndOfWord.AutoSize = true;
            rbn_replaceConsonantsEndOfWord.Location = new Point(186, 19);
            rbn_replaceConsonantsEndOfWord.Name = "rbn_replaceConsonantsEndOfWord";
            rbn_replaceConsonantsEndOfWord.Size = new Size(98, 19);
            rbn_replaceConsonantsEndOfWord.TabIndex = 2;
            rbn_replaceConsonantsEndOfWord.TabStop = true;
            rbn_replaceConsonantsEndOfWord.Text = "End Of Words";
            rbn_replaceConsonantsEndOfWord.UseVisualStyleBackColor = true;
            rbn_replaceConsonantsEndOfWord.CheckedChanged += Rbn_replaceConsonantsEndOfWord_CheckedChanged;
            // 
            // rbn_replaceConsonantsStartOfWords
            // 
            rbn_replaceConsonantsStartOfWords.AutoSize = true;
            rbn_replaceConsonantsStartOfWords.BackColor = Color.White;
            rbn_replaceConsonantsStartOfWords.Location = new Point(80, 19);
            rbn_replaceConsonantsStartOfWords.Name = "rbn_replaceConsonantsStartOfWords";
            rbn_replaceConsonantsStartOfWords.Size = new Size(100, 19);
            rbn_replaceConsonantsStartOfWords.TabIndex = 1;
            rbn_replaceConsonantsStartOfWords.TabStop = true;
            rbn_replaceConsonantsStartOfWords.Text = "Start of Words";
            rbn_replaceConsonantsStartOfWords.UseVisualStyleBackColor = false;
            rbn_replaceConsonantsStartOfWords.CheckedChanged += Rbn_replaceConsonantsStartOfWords_CheckedChanged;
            // 
            // rbn_replaceConsonantsGlobally
            // 
            rbn_replaceConsonantsGlobally.AutoSize = true;
            rbn_replaceConsonantsGlobally.Location = new Point(6, 19);
            rbn_replaceConsonantsGlobally.Name = "rbn_replaceConsonantsGlobally";
            rbn_replaceConsonantsGlobally.Size = new Size(68, 19);
            rbn_replaceConsonantsGlobally.TabIndex = 0;
            rbn_replaceConsonantsGlobally.TabStop = true;
            rbn_replaceConsonantsGlobally.Text = "Globally";
            rbn_replaceConsonantsGlobally.UseVisualStyleBackColor = true;
            rbn_replaceConsonantsGlobally.CheckedChanged += Rbn_replaceConsonantsGlobally_CheckedChanged;
            // 
            // gbx_replacementPhonemeDepth
            // 
            gbx_replacementPhonemeDepth.Controls.Add(rbn_allPhonemes);
            gbx_replacementPhonemeDepth.Controls.Add(rbn_l3);
            gbx_replacementPhonemeDepth.Controls.Add(rbn_l2);
            gbx_replacementPhonemeDepth.Controls.Add(rbn_l1);
            gbx_replacementPhonemeDepth.Location = new Point(6, 6);
            gbx_replacementPhonemeDepth.Name = "gbx_replacementPhonemeDepth";
            gbx_replacementPhonemeDepth.Size = new Size(453, 42);
            gbx_replacementPhonemeDepth.TabIndex = 25;
            gbx_replacementPhonemeDepth.TabStop = false;
            gbx_replacementPhonemeDepth.Text = "Consonant Depth of Replacement Phonemes";
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
            tabPageVowelDiphthongs.Controls.Add(rbn_removeEndVowel);
            tabPageVowelDiphthongs.Controls.Add(rbn_removeStartVowel);
            tabPageVowelDiphthongs.Controls.Add(lbl_DiphthongEndVowel);
            tabPageVowelDiphthongs.Controls.Add(cbx_diphthongEndVowel);
            tabPageVowelDiphthongs.Controls.Add(lbl_diphthongStartVowel);
            tabPageVowelDiphthongs.Controls.Add(cbx_diphthongStartVowel);
            tabPageVowelDiphthongs.Controls.Add(rbn_diphthongReplacement);
            tabPageVowelDiphthongs.Controls.Add(rbn_changeEndVowel);
            tabPageVowelDiphthongs.Controls.Add(rbn_changeStartVowel);
            tabPageVowelDiphthongs.Controls.Add(rbn_vowelToDiphthongEnd);
            tabPageVowelDiphthongs.Controls.Add(rbn_vowelToDiphthongStart);
            tabPageVowelDiphthongs.Location = new Point(4, 24);
            tabPageVowelDiphthongs.Name = "tabPageVowelDiphthongs";
            tabPageVowelDiphthongs.Padding = new Padding(3);
            tabPageVowelDiphthongs.Size = new Size(465, 164);
            tabPageVowelDiphthongs.TabIndex = 2;
            tabPageVowelDiphthongs.Text = "Vowel Diphthongs";
            tabPageVowelDiphthongs.UseVisualStyleBackColor = true;
            // 
            // rbn_removeEndVowel
            // 
            rbn_removeEndVowel.AutoSize = true;
            rbn_removeEndVowel.Location = new Point(190, 56);
            rbn_removeEndVowel.Name = "rbn_removeEndVowel";
            rbn_removeEndVowel.Size = new Size(125, 19);
            rbn_removeEndVowel.TabIndex = 10;
            rbn_removeEndVowel.TabStop = true;
            rbn_removeEndVowel.Text = "Remove End Vowel";
            rbn_removeEndVowel.UseVisualStyleBackColor = true;
            rbn_removeEndVowel.CheckedChanged += Rbn_removeEndVowel_CheckedChanged;
            // 
            // rbn_removeStartVowel
            // 
            rbn_removeStartVowel.AutoSize = true;
            rbn_removeStartVowel.Location = new Point(6, 56);
            rbn_removeStartVowel.Name = "rbn_removeStartVowel";
            rbn_removeStartVowel.Size = new Size(129, 19);
            rbn_removeStartVowel.TabIndex = 9;
            rbn_removeStartVowel.TabStop = true;
            rbn_removeStartVowel.Text = "Remove Start Vowel";
            rbn_removeStartVowel.UseVisualStyleBackColor = true;
            rbn_removeStartVowel.CheckedChanged += Rbn_removeStartVowel_CheckedChanged;
            // 
            // lbl_DiphthongEndVowel
            // 
            lbl_DiphthongEndVowel.AutoSize = true;
            lbl_DiphthongEndVowel.Location = new Point(236, 102);
            lbl_DiphthongEndVowel.Name = "lbl_DiphthongEndVowel";
            lbl_DiphthongEndVowel.Size = new Size(121, 15);
            lbl_DiphthongEndVowel.TabIndex = 8;
            lbl_DiphthongEndVowel.Text = "Diphthong End Vowel";
            // 
            // cbx_diphthongEndVowel
            // 
            cbx_diphthongEndVowel.FormattingEnabled = true;
            cbx_diphthongEndVowel.Location = new Point(236, 119);
            cbx_diphthongEndVowel.Name = "cbx_diphthongEndVowel";
            cbx_diphthongEndVowel.Size = new Size(225, 23);
            cbx_diphthongEndVowel.TabIndex = 7;
            // 
            // lbl_diphthongStartVowel
            // 
            lbl_diphthongStartVowel.AutoSize = true;
            lbl_diphthongStartVowel.Location = new Point(8, 102);
            lbl_diphthongStartVowel.Name = "lbl_diphthongStartVowel";
            lbl_diphthongStartVowel.Size = new Size(125, 15);
            lbl_diphthongStartVowel.TabIndex = 6;
            lbl_diphthongStartVowel.Text = "Diphthong Start Vowel";
            // 
            // cbx_diphthongStartVowel
            // 
            cbx_diphthongStartVowel.FormattingEnabled = true;
            cbx_diphthongStartVowel.Location = new Point(8, 120);
            cbx_diphthongStartVowel.Name = "cbx_diphthongStartVowel";
            cbx_diphthongStartVowel.Size = new Size(222, 23);
            cbx_diphthongStartVowel.TabIndex = 5;
            // 
            // rbn_diphthongReplacement
            // 
            rbn_diphthongReplacement.AutoSize = true;
            rbn_diphthongReplacement.Location = new Point(6, 81);
            rbn_diphthongReplacement.Name = "rbn_diphthongReplacement";
            rbn_diphthongReplacement.Size = new Size(154, 19);
            rbn_diphthongReplacement.TabIndex = 4;
            rbn_diphthongReplacement.Text = "Diphthong Replacement";
            rbn_diphthongReplacement.UseVisualStyleBackColor = true;
            rbn_diphthongReplacement.CheckedChanged += Rbn_diphthongReplacement_CheckedChanged;
            // 
            // rbn_changeEndVowel
            // 
            rbn_changeEndVowel.AutoSize = true;
            rbn_changeEndVowel.Location = new Point(190, 31);
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
            // rbn_vowelToDiphthongEnd
            // 
            rbn_vowelToDiphthongEnd.AutoSize = true;
            rbn_vowelToDiphthongEnd.Location = new Point(190, 6);
            rbn_vowelToDiphthongEnd.Name = "rbn_vowelToDiphthongEnd";
            rbn_vowelToDiphthongEnd.Size = new Size(167, 19);
            rbn_vowelToDiphthongEnd.TabIndex = 1;
            rbn_vowelToDiphthongEnd.Text = "Vowel to End of Diphthong";
            rbn_vowelToDiphthongEnd.UseVisualStyleBackColor = true;
            rbn_vowelToDiphthongEnd.CheckedChanged += Rbn_vowelToDiphthongEnd_CheckedChanged;
            // 
            // rbn_vowelToDiphthongStart
            // 
            rbn_vowelToDiphthongStart.AutoSize = true;
            rbn_vowelToDiphthongStart.Checked = true;
            rbn_vowelToDiphthongStart.Location = new Point(6, 6);
            rbn_vowelToDiphthongStart.Name = "rbn_vowelToDiphthongStart";
            rbn_vowelToDiphthongStart.Size = new Size(171, 19);
            rbn_vowelToDiphthongStart.TabIndex = 0;
            rbn_vowelToDiphthongStart.TabStop = true;
            rbn_vowelToDiphthongStart.Text = "Vowel to Start of Diphthong";
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
            rbn_replaceRhotacized.Size = new Size(207, 19);
            rbn_replaceRhotacized.TabIndex = 3;
            rbn_replaceRhotacized.Text = "Replace Rhotacized with Phoneme";
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
            tabPageSpecialOperations.Controls.Add(btn_replaceCluster);
            tabPageSpecialOperations.Controls.Add(btn_updateSoundMapList);
            tabPageSpecialOperations.Location = new Point(4, 24);
            tabPageSpecialOperations.Name = "tabPageSpecialOperations";
            tabPageSpecialOperations.Padding = new Padding(3);
            tabPageSpecialOperations.Size = new Size(465, 164);
            tabPageSpecialOperations.TabIndex = 3;
            tabPageSpecialOperations.Text = "Special Operations";
            tabPageSpecialOperations.UseVisualStyleBackColor = true;
            // 
            // btn_replaceCluster
            // 
            btn_replaceCluster.Location = new Point(200, 13);
            btn_replaceCluster.Name = "btn_replaceCluster";
            btn_replaceCluster.Size = new Size(110, 23);
            btn_replaceCluster.TabIndex = 1;
            btn_replaceCluster.Text = "Replace Cluster";
            btn_replaceCluster.UseVisualStyleBackColor = true;
            btn_replaceCluster.Click += btn_replaceCluster_Click;
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
            // tb_Volume
            // 
            tb_Volume.Location = new Point(692, 172);
            tb_Volume.Maximum = 100;
            tb_Volume.Name = "tb_Volume";
            tb_Volume.Size = new Size(292, 45);
            tb_Volume.TabIndex = 34;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(692, 203);
            label1.Name = "label1";
            label1.Size = new Size(47, 15);
            label1.TabIndex = 35;
            label1.Text = "Volume";
            // 
            // exportBeatsAndBindingsClusterCountsToolStripMenuItem
            // 
            exportBeatsAndBindingsClusterCountsToolStripMenuItem.Name = "exportBeatsAndBindingsClusterCountsToolStripMenuItem";
            exportBeatsAndBindingsClusterCountsToolStripMenuItem.Size = new Size(285, 22);
            exportBeatsAndBindingsClusterCountsToolStripMenuItem.Text = "Export Beats and Bindigs Cluster Counts";
            exportBeatsAndBindingsClusterCountsToolStripMenuItem.Click += ExportBBClusterCountsToolStripMenuItem_Click;
            // 
            // LanguageHoningForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 496);
            Controls.Add(label1);
            Controls.Add(tb_Volume);
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
            MainMenuStrip = menuStrip1;
            Name = "LanguageHoningForm";
            Text = "Conlang Audio Honing";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabPhoneticAlterations.ResumeLayout(false);
            tabPageConsonants.ResumeLayout(false);
            tabPageConsonants.PerformLayout();
            gbx_scopeOfConsonantReplacement.ResumeLayout(false);
            gbx_scopeOfConsonantReplacement.PerformLayout();
            gbx_replacementPhonemeDepth.ResumeLayout(false);
            gbx_replacementPhonemeDepth.PerformLayout();
            tabPageVowels.ResumeLayout(false);
            gbx_vowelReplacements.ResumeLayout(false);
            gbx_vowelReplacements.PerformLayout();
            tabPageVowelDiphthongs.ResumeLayout(false);
            tabPageVowelDiphthongs.PerformLayout();
            tabPageRhoticity.ResumeLayout(false);
            tabPageRhoticity.PerformLayout();
            tabPageSpecialOperations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tb_Volume).EndInit();
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
        private GroupBox gbx_replacementPhonemeDepth;
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
        private RadioButton rbn_vowelToDiphthongEnd;
        private RadioButton rbn_vowelToDiphthongStart;
        private Label lbl_DiphthongEndVowel;
        private ComboBox cbx_diphthongEndVowel;
        private Label lbl_diphthongStartVowel;
        private ComboBox cbx_diphthongStartVowel;
        private RadioButton rbn_diphthongReplacement;
        private ToolStripMenuItem editLexiconToolStripMenuItem;
        private ToolStripMenuItem setAndAdjustLexicalOrderToolStripMenuItem;
        private ToolStripMenuItem setAzureSpeechKeyToolStripMenuItem;
        private ToolStripMenuItem setAzureSpeechRegionToolStripMenuItem;
        private ToolStripMenuItem setAmazonPollyAuthorizationEmailToolStripMenuItem;
        private ToolStripMenuItem setAmazonPollyAuthorizationPasswordToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem viewHelpToolStripMenuItem;
        private ToolStripMenuItem aboutConlangAudioHoningToolStripMenuItem;
        private ToolStripMenuItem useSharedAmazonPollyToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem setAmazonPollyProfileToolStripMenuItem;
        private ToolStripMenuItem setAmazonPollyS3BucketNameToolStripMenuItem;
        private TrackBar tb_Volume;
        private Label label1;
        private ToolStripMenuItem useCompactJsonToolStripItem;
        private RadioButton rbn_removeStartVowel;
        private RadioButton rbn_removeEndVowel;
        private GroupBox gbx_scopeOfConsonantReplacement;
        private RadioButton rbn_replaceConsonantsStartOfWords;
        private RadioButton rbn_replaceConsonantsGlobally;
        private Label lbl_customReplacementPattern;
        private TextBox txt_customReplacementPattern;
        private RadioButton rbn_replaceConsonantsPattern;
        private RadioButton rbn_replaceConsonantsEndOfWord;
        private CheckBox chk_changeNotMatchLocations;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem generatePhonemeClustersToolStripMenuItem;
        private ToolStripMenuItem listNADAnalysisClustersToolStripMenuItem;
        private Button btn_replaceCluster;
        private ToolStripMenuItem exportNADClusterCountsToolStripMenuItem;
        private ToolStripMenuItem exportBeatsAndBindingsClusterCountsToolStripMenuItem;
    }
}
