namespace ConlangAudioHoning
{
    partial class CustomPatternForm
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
            txt_pattern = new TextBox();
            btn_OK = new Button();
            menus = new MenuStrip();
            SuspendLayout();
            // 
            // txt_pattern
            // 
            txt_pattern.Location = new Point(12, 27);
            txt_pattern.Name = "txt_pattern";
            txt_pattern.Size = new Size(759, 23);
            txt_pattern.TabIndex = 0;
            // 
            // btn_OK
            // 
            btn_OK.Location = new Point(696, 56);
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
            // CustomPatternForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(783, 84);
            Controls.Add(btn_OK);
            Controls.Add(txt_pattern);
            Controls.Add(menus);
            MainMenuStrip = menus;
            Name = "CustomPatternForm";
            Text = "Consonant Replacement Limit Pattern";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txt_pattern;
        private Button btn_OK;
        private MenuStrip menus;
    }
}