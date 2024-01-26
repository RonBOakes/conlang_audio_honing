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
        private LanguageDescription? languageDescription;
        private FileInfo? languageFileInfo = null;

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

    }
}
