namespace ConlangAudioHoning
{
    partial class PatternReplacementForm
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
            txt_patternToReplace = new TextBox();
            btn_OK = new Button();
            menus = new MenuStrip();
            txt_usageText = new TextBox();
            label1 = new Label();
            label2 = new Label();
            txt_replacement = new TextBox();
            SuspendLayout();
            // 
            // txt_patternToReplace
            // 
            txt_patternToReplace.Location = new Point(12, 48);
            txt_patternToReplace.Name = "txt_patternToReplace";
            txt_patternToReplace.Size = new Size(759, 23);
            txt_patternToReplace.TabIndex = 0;
            // 
            // btn_OK
            // 
            btn_OK.Location = new Point(696, 121);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(75, 23);
            btn_OK.TabIndex = 1;
            btn_OK.Text = "OK";
            btn_OK.UseVisualStyleBackColor = true;
            // 
            // menus
            // 
            menus.Location = new Point(0, 0);
            menus.Name = "menus";
            menus.Size = new Size(783, 24);
            menus.TabIndex = 2;
            menus.Text = "menuStrip1";
            // 
            // txt_usageText
            // 
            txt_usageText.Location = new Point(12, 150);
            txt_usageText.Multiline = true;
            txt_usageText.Name = "txt_usageText";
            txt_usageText.ReadOnly = true;
            txt_usageText.Size = new Size(759, 115);
            txt_usageText.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(127, 15);
            label1.TabIndex = 4;
            label1.Text = "Pattern To Be Replaced";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 74);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 5;
            label2.Text = "Replacement";
            // 
            // txt_replacement
            // 
            txt_replacement.Location = new Point(12, 92);
            txt_replacement.Name = "txt_replacement";
            txt_replacement.Size = new Size(759, 23);
            txt_replacement.TabIndex = 6;
            // 
            // PatternReplacementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(783, 277);
            Controls.Add(txt_replacement);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txt_usageText);
            Controls.Add(btn_OK);
            Controls.Add(txt_patternToReplace);
            Controls.Add(menus);
            MainMenuStrip = menus;
            Name = "PatternReplacementForm";
            Text = "Cluster Replacement PatternToReplace";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txt_patternToReplace;
        private Button btn_OK;
        private MenuStrip menus;
        private TextBox txt_usageText;
        private Label label1;
        private Label label2;
        private TextBox txt_replacement;
    }
}