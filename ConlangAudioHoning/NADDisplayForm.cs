using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Form for displaying Net Audio Distance (NAD) Clusters along with the link to the online calculator
    /// for performing phonotactic analysis using this information.
    /// </summary>
    internal class NADDisplayForm : Form
    {
        LinkLabel _nadPhonotacticCalculatorSiteLabel;
        TextBox _nadClusterListBox;
        Button _okButton;

        public NADDisplayForm(string nadString)
        {
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(160, 280);

            this.SuspendLayout();

            _nadPhonotacticCalculatorSiteLabel = new();
            _nadPhonotacticCalculatorSiteLabel.Text = "NAD Phonotactic Calculator";
            _nadPhonotacticCalculatorSiteLabel.Size = new Size(80, 25);
            _nadPhonotacticCalculatorSiteLabel.Location = new Point(5, 5);
            _nadPhonotacticCalculatorSiteLabel.LinkClicked += _nadPhonotacticCalculatorSiteLabel_LinkClicked;
            Controls.Add(_nadPhonotacticCalculatorSiteLabel);

            _nadClusterListBox = new();
            _nadClusterListBox.Size = new Size(150, 220);
            _nadClusterListBox.Location = new Point(5, 30);
            _nadClusterListBox.Multiline = true;
            _nadClusterListBox.Text = nadString;
            _nadClusterListBox.ScrollBars = ScrollBars.Vertical;
            _nadClusterListBox.ReadOnly = true;
            Controls.Add(_nadClusterListBox);

            _okButton = new();
            _okButton.Text = "OK";
            _okButton.UseVisualStyleBackColor = true;
            _okButton.Size = new Size(50, 30);
            _okButton.Location = new Point(5, 250);
            _okButton.Click += _okButton_Click;
            Controls.Add(_okButton);


            this.ResumeLayout(true);
        }

        private void _nadPhonotacticCalculatorSiteLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            this._nadPhonotacticCalculatorSiteLabel.LinkVisited = true;

            string target = "https://wa.amu.edu.pl/nadcalc/";

            try
            {
                Process.Start(new ProcessStartInfo() { FileName = target, UseShellExecute = true });
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                {
                    MessageBox.Show(noBrowser.Message);
                }
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void _okButton_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
