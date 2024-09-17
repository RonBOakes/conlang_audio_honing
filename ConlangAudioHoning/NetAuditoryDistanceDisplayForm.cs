/*
 * Form for displaying the NAD Clusters
 * Copyright (C) 2024 Ronald B. Oakes
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
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Form for displaying Net Auditory Distance (NAD) Clusters along with the link to the online calculator
    /// for performing phonotactic analysis using this information.
    /// </summary>
    internal class NetAuditoryDistanceDisplayForm : Form
    {
        private readonly LinkLabel _nadPhonotacticCalculatorSiteLabel;

        public NetAuditoryDistanceDisplayForm(string nadString)
        {
            TextBox _nadClusterListBox;
            Button _okButton;
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
