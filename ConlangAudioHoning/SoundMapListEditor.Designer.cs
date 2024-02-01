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
            btnAbbove = new Button();
            btnAddBelow = new Button();
            lbx_soundMapListEntries = new ListBox();
            soundMapEditor2 = new SoundMapEditor();
            SuspendLayout();
            // 
            // btnAbbove
            // 
            btnAbbove.Location = new Point(868, 12);
            btnAbbove.Name = "btnAbbove";
            btnAbbove.Size = new Size(75, 23);
            btnAbbove.TabIndex = 3;
            btnAbbove.Text = "Add Above";
            btnAbbove.UseVisualStyleBackColor = true;
            // 
            // btnAddBelow
            // 
            btnAddBelow.Location = new Point(868, 34);
            btnAddBelow.Name = "btnAddBelow";
            btnAddBelow.Size = new Size(75, 23);
            btnAddBelow.TabIndex = 4;
            btnAddBelow.Text = "Add Below";
            btnAddBelow.UseVisualStyleBackColor = true;
            // 
            // lbx_soundMapListEntries
            // 
            lbx_soundMapListEntries.FormattingEnabled = true;
            lbx_soundMapListEntries.ItemHeight = 15;
            lbx_soundMapListEntries.Location = new Point(4, 74);
            lbx_soundMapListEntries.Name = "lbx_soundMapListEntries";
            lbx_soundMapListEntries.Size = new Size(939, 364);
            lbx_soundMapListEntries.TabIndex = 5;
            // 
            // soundMapEditor2
            // 
            soundMapEditor2.BorderStyle = BorderStyle.FixedSingle;
            soundMapEditor2.Location = new Point(12, 12);
            soundMapEditor2.Name = "soundMapEditor2";
            soundMapEditor2.Size = new Size(850, 56);
            soundMap1.phoneme = "";
            soundMap1.pronounciation_regex = "";
            soundMap1.romanization = "";
            soundMap1.spelling_regex = "";
            soundMapEditor2.SoundMapData = soundMap1;
            soundMapEditor2.TabIndex = 6;
            // 
            // SoundMapListEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(945, 450);
            Controls.Add(soundMapEditor2);
            Controls.Add(lbx_soundMapListEntries);
            Controls.Add(btnAddBelow);
            Controls.Add(btnAbbove);
            Name = "SoundMapListEditor";
            Text = "Spelling/Pronounciation List Editor";
            ResumeLayout(false);
        }

        #endregion
        private Button btnAbbove;
        private Button btnAddBelow;
        private ListBox lbx_soundMapListEntries;
        private SoundMapEditor soundMapEditor2;
    }
}