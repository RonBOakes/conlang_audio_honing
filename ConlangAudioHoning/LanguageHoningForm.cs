/*
* User code for the top-level form for the Conlang Audio Honing program.
* 
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConlangJson;
using LanguageEditor;

namespace ConlangAudioHoning
{
    public partial class LanguageHoningForm : Form
    {
        private LanguageDescription? languageDescription = null;
        private FileInfo? languageFileInfo = null;
        private string? sampleText = null;
        private PollySpeech? pollySpeech = null;

        public LanguageHoningForm()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    openFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    openFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                openFileDialog.Filter = "JSON file (*.json)|*.json|txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadLanguage(openFileDialog.FileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    saveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    saveFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                saveFileDialog.Filter = "JSON file (*.json)|*.json|txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.OverwritePrompt = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveLanguage(saveFileDialog.FileName);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadLanguage(string filename)
        {
            string jsonString = File.ReadAllText(filename);

            languageDescription = JsonSerializer.Deserialize<LanguageDescription>(jsonString);

            if (languageDescription == null)
            {
                MessageBox.Show("Unable to decode Language file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // TODO: Populate form
            if (pollySpeech == null)
            {
                pollySpeech = new PollySpeech(languageDescription);
            }
            else
            {
                pollySpeech.LanguageDescription = languageDescription;
            }
        }

        private void SaveLanguage(string filename)
        {
            if (languageDescription == null)
            {
                MessageBox.Show("Unable to save language File, no language information present", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // TODO: Pull the form data into the structure to allow for saving.

                DateTime now = DateTime.UtcNow;
                string timestamp = now.ToString("o");
                string history = "Edited in LanguageEditor, saved at " + timestamp;

                if (!languageDescription.metadata.ContainsKey("history"))
                {
                    languageDescription.metadata["history"] = new JsonArray();
                }
#pragma warning disable CS8600
                JsonArray historyEntries = (JsonArray)languageDescription.metadata["history"] ?? [];
#pragma warning restore CS8600
                historyEntries.Add(history);

                JsonSerializerOptions jsonSerializerOptions = new();
                jsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                JavaScriptEncoder encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                jsonSerializerOptions.Encoder = encoder;
                jsonSerializerOptions.WriteIndented = true; // TODO: make an option

                string jsonString = JsonSerializer.Serialize<LanguageDescription>(languageDescription, jsonSerializerOptions);
                File.WriteAllText(filename, jsonString, System.Text.Encoding.UTF8);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    openFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    openFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                openFileDialog.Filter = "txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.CheckFileExists = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadSampleText(openFileDialog.FileName);
                }
            }
        }

        private void LoadSampleText(string fileName)
        {
            string fileText = File.ReadAllText(fileName);

            sampleText = fileText;
            txt_SampleText.Text = sampleText;
            if(pollySpeech != null)
            {
                pollySpeech.sampleText = sampleText;
            }
        }

        private void saveSampleMenu_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new())
            {
                if (languageFileInfo == null)
                {
                    saveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
                }
                else
                {
                    saveFileDialog.InitialDirectory = languageFileInfo.DirectoryName;
                }
                saveFileDialog.Filter = "txt file (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.OverwritePrompt = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveSampleText(saveFileDialog.FileName);
                }
            }
        }

        private void SaveSampleText(string filename)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
            File.WriteAllText(filename, sampleText);
        }

        private void txt_SampleText_TextChanged(object sender, EventArgs e)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            if((languageDescription != null) && (pollySpeech != null) && (sampleText != null) && (!sampleText.Trim().Equals(string.Empty)))
            {
                pollySpeech.generate(languageDescription.preferred_voice ?? "Brian","slow");
                txt_phonetic.Text = pollySpeech.phoneticText;
            }
        }
    }
}

