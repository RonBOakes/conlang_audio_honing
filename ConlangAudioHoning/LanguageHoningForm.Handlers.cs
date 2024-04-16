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
using ConlangJson;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConlangAudioHoning
{
    public partial class LanguageHoningForm
    {
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
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

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using SaveFileDialog saveFileDialog = new();
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadSampleTextFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
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
                // Empty the speech files and list box
                speechFiles.Clear();
                cbx_recordings.Items.Clear();
            }
        }

        private void SaveSampleMenu_Click(object sender, EventArgs e)
        {
            using SaveFileDialog saveFileDialog = new();
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

        private void This_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (languageDirty)
            {
                DialogResult dialogResult = MessageBox.Show("Language has been altered, save?", "Language Altered", MessageBoxButtons.YesNoCancel);
                if (dialogResult.Equals(DialogResult.Yes))
                {
                    using SaveFileDialog saveFileDialog = new();
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
                else if (dialogResult.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (sampleDirty)
            {
                DialogResult dialogResult = MessageBox.Show("Sample Text has been altered, save?", "Sample Text Altered", MessageBoxButtons.YesNoCancel);
                if (dialogResult.Equals(DialogResult.Yes))
                {
                    using SaveFileDialog saveFileDialog = new();
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
                else if (dialogResult.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void Txt_SampleText_TextChanged(object sender, EventArgs e)
        {
            sampleText = txt_SampleText.Text;
            sampleText = sampleText.Trim();
            sampleDirty = true;
        }

        private void Btn_generate_Click(object sender, EventArgs e)
        {
            if ((languageDescription != null) && (!string.IsNullOrEmpty(sampleText)))
            {
                string engineName;
                if (cbx_speechEngine.SelectedIndex == -1)
                {
                    engineName = "None";
                }
                else
                {
                    engineName = cbx_speechEngine.Text.Trim();
                }
                SpeechEngine engine = speechEngines[engineName];
                string voiceName;
                if (cbx_voice.SelectedIndex == -1)
                {
                    voiceName = string.Empty;
                }
                else
                {
                    voiceName = cbx_voice.Text.Trim().Split()[0];
                }
                SpeechEngine.VoiceData? voiceData = string.IsNullOrEmpty(voiceName) ? null : voices[engineName][voiceName];
                string speed = cbx_speed.Text.Trim();
                engine.Generate(speed, this, voiceData);
                txt_phonetic.Text = engine.PhoneticText;
            }
        }

        private void DeclineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if (!languageDescription.declined)
            {
                DeclineLexicon(languageDescription);
                declineToolStripMenuItem.Text = "Remove Declensions";
                languageDirty = true;
            }
            else
            {
                ConlangUtilities.RemoveDeclinedEntries(languageDescription);
                declineToolStripMenuItem.Text = "Decline Language";
                languageDirty = true;
            }

        }

        private void Btn_generateSpeech_Click(object sender, EventArgs e)
        {
            if ((languageDescription == null) || (string.IsNullOrEmpty(sampleText)))
            {
                return;
            }
            string engineName = cbx_speechEngine.Text.Trim();
            if ((engineName.Equals("Amazon Polly")) && (UserConfiguration.IsPollySupported))
            {
                PollySpeech pollySpeech = (PollySpeech)speechEngines[engineName];
                DateTime now = DateTime.Now;
                string targetFileBaseName = string.Format("speech_{0:s}.mp3", now);
                targetFileBaseName = targetFileBaseName.Replace(":", "_");

                string targetFileName;
                if (languageFileInfo != null)
                {
                    targetFileName = languageFileInfo.Directory + "\\" + targetFileBaseName;
                }
                else
                {
                    targetFileName = Path.GetTempPath() + targetFileBaseName;
                }

                string voice = cbx_voice.Text.Trim().Split()[0];
                string speed = cbx_speed.Text.Trim();

                bool ok = pollySpeech.GenerateSpeech(targetFileName, voice, speed, this);
                if (!ok)
                {
                    _ = MessageBox.Show("Unable to generate speech file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txt_phonetic.Text = pollySpeech.PhoneticText;
                    // Play the audio (MP3) file with the Windows Media Player
                    WMPLib.WindowsMediaPlayer player = new()
                    {
                        URL = targetFileName
                    };
                    player.controls.play();

                    FileInfo fileInfo = new(targetFileName);
                    speechFiles.Add(fileInfo.Name, fileInfo);
                    _ = cbx_recordings.Items.Add(fileInfo.Name);
                    cbx_recordings.SelectedText = fileInfo.Name;
                }
            }
            else if ((engineName.Equals("espeak-ng")) && (UserConfiguration.IsESpeakNGSupported))
            {
                ESpeakNGSpeak speechEngine = (ESpeakNGSpeak)speechEngines[engineName];
                DateTime now = DateTime.Now;
                string targetFileBaseName = string.Format("speech_{0:s}.wav", now);
                targetFileBaseName = targetFileBaseName.Replace(":", "_");

                string targetFileName;
                if (languageFileInfo != null)
                {
                    targetFileName = languageFileInfo.Directory + "\\" + targetFileBaseName;
                }
                else
                {
                    targetFileName = Path.GetTempPath() + targetFileBaseName;
                }
                string voiceKey = cbx_voice.Text.Trim().Split()[0];
                string voice = string.Empty;
                if (voices[engineName].TryGetValue(voiceKey, out SpeechEngine.VoiceData value))
                {
                    SpeechEngine.VoiceData voiceData = value;
                    voice = voiceData.LanguageCode;
                }
                string speed = cbx_speed.Text.Trim();
                speechEngine.SampleText = sampleText;
                bool ok = speechEngine.GenerateSpeech(targetFileName, voice, speed, this);
                if (!ok)
                {
                    _ = MessageBox.Show("Unable to generate speech file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txt_phonetic.Text = speechEngine.PhoneticText;
                    // Play the audio (wav) file with the Windows Media Player
                    WMPLib.WindowsMediaPlayer player = new()
                    {
                        URL = targetFileName
                    };
                    player.controls.play();

                    FileInfo fileInfo = new(targetFileName);
                    speechFiles.Add(fileInfo.Name, fileInfo);
                    _ = cbx_recordings.Items.Add(fileInfo.Name);
                    cbx_recordings.SelectedText = fileInfo.Name;
                }
            }
            else if ((engineName.Equals("azure")) && UserConfiguration.IsAzureSupported)
            {
                AzureSpeak speechEngine = (AzureSpeak)speechEngines[engineName];
                DateTime now = DateTime.Now;
                string targetFileBaseName = string.Format("speech_{0:s}.wav", now);
                targetFileBaseName = targetFileBaseName.Replace(":", "_");

                string targetFileName;
                if (languageFileInfo != null)
                {
                    targetFileName = languageFileInfo.Directory + "\\" + targetFileBaseName;
                }
                else
                {
                    targetFileName = Path.GetTempPath() + targetFileBaseName;
                }
                string voiceKey = cbx_voice.Text.Trim().Split()[0];
                string voice = string.Empty;
                if (voices[engineName].TryGetValue(voiceKey, out SpeechEngine.VoiceData value))
                {
                    SpeechEngine.VoiceData voiceData = value;
                    voice = voiceData.LanguageCode;
                }
                string speed = cbx_speed.Text.Trim();
                speechEngine.SampleText = sampleText;
                bool ok = speechEngine.GenerateSpeech(targetFileName, voice, speed, this);
                if (!ok)
                {
                    _ = MessageBox.Show("Unable to generate speech file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txt_phonetic.Text = speechEngine.PhoneticText;
                    // Play the audio (wav) file with the Windows Media Player
                    WMPLib.WindowsMediaPlayer player = new()
                    {
                        URL = targetFileName
                    };
                    player.controls.play();

                    FileInfo fileInfo = new(targetFileName);
                    speechFiles.Add(fileInfo.Name, fileInfo);
                    _ = cbx_recordings.Items.Add(fileInfo.Name);
                    cbx_recordings.SelectedText = fileInfo.Name;
                }
            }
        }

        private void DisplayPulmonicConsonantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new();
            foreach (string c in IpaUtilities.PConsonants)
            {
                _ = sb.Append(c);
                _ = sb.Append(' ');
            }
            txt_SampleText.Text = sb.ToString();
            txt_phonetic.Text = sb.ToString();
        }

        // The PrintPage event is raised for each page to be printed.
        private void Pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            if ((readerToPrint == null) || (printFont == null) || (ev == null))
            {
                return;
            }

            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string? line = string.Empty;

            // Calculate the number of lines per page.
#pragma warning disable CS8604 // Possible null reference argument.
            float linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
#pragma warning restore CS8604 // Possible null reference argument.

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = readerToPrint.ReadLine()) != null))
            {
                float yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
            {
                ev.HasMorePages = true;
            }
            else
            {
                ev.HasMorePages = false;
            }
        }

        private void PrintIPAMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ipaPhonemes = IpaUtilities.IpaPhonemes();
            StringReader ipaPhonemeReader = new(ipaPhonemes);
            readerToPrint = ipaPhonemeReader;
            try
            {
                PrintDialog printDialog = new();
                printFont = new Font("Charis SIL", 12f);
                PrintDocument pd = new();
                pd.PrintPage += new PrintPageEventHandler(this.Pd_PrintPage);
                printDialog.Document = pd;
                DialogResult result = printDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pd.Print();
                }
            }
            finally
            {
                readerToPrint.Close();
            }
        }

        private void DisplaySupersegmentals_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new();
            foreach (string c in IpaUtilities.Suprasegmentals)
            {
                _ = sb.Append(c);
                _ = sb.Append(' ');
            }
            txt_SampleText.Text = sb.ToString();
            txt_phonetic.Text = sb.ToString();
        }

        private void PbTimer_Tick(object sender, EventArgs e)
        {
            ProgressBar.PerformStep();
        }

        private void Btn_replaySpeech_Click(object sender, EventArgs e)
        {
            string fileName = cbx_recordings.Text;
            string targetFileName = speechFiles[fileName].FullName;
            // Play the audio (OGG) file with the default application
            WMPLib.WindowsMediaPlayer player = new()
            {
                URL = targetFileName
            };
            player.controls.play();
        }

        private void Cbx_phonemeToChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReplacementPhonemeCbx();
        }

        private void Btn_applyChangeToLanguage_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex != 0) &&
                (tabPhoneticAlterations.SelectedIndex != 1) &&
                (tabPhoneticAlterations.SelectedIndex != 2) &&
                (tabPhoneticAlterations.SelectedIndex != 3))
            {
                return;
            }

            if ((tabPhoneticAlterations.SelectedIndex == 0) ||
                (tabPhoneticAlterations.SelectedIndex == 1) ||
                ((tabPhoneticAlterations.SelectedIndex == 3) && (!rbn_replaceRSpelling.Checked)) ||
                ((tabPhoneticAlterations.SelectedIndex == 2) && (!rbn_diphthongReplacement.Checked)))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
                {
                    return;
                }
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = cbx_replacementPhoneme.Text.Split()[0];
                // Make the change
                phoneticChanger.PhoneticChange(oldPhoneme, newPhoneme);
                // Update sample text
                if (sampleText != string.Empty)
                {
                    sampleText = phoneticChanger.SampleText;
                    txt_SampleText.Text = sampleText;
                    txt_phonetic.Text = string.Empty;
                    foreach (string engineName in speechEngines.Keys)
                    {
                        SpeechEngine speechEngine = speechEngines[engineName];
                        speechEngine.SampleText = sampleText;
                    }
                }
                // Clear the combo boxes
                cbx_phonemeToChange.Items.Clear();
                cbx_replacementPhoneme.Items.Clear();
                tabPhoneticAlterations.SelectedIndex = -1;
            }
            else if ((tabPhoneticAlterations.SelectedIndex == 3) && rbn_replaceRSpelling.Checked && cbx_phonemeToChange.SelectedItem != null)
            {
                SoundMap mapToReplace = (SoundMap)cbx_phonemeToChange.SelectedItem;
                SoundMap replacementMap = mapToReplace.copy();
                string phonemeToAdd = cbx_replacementPhoneme.Text.Split()[0];
                string diacriticAtEnd = string.Format("{0}$", IpaUtilities.DiacriticPattern);
                if (!string.IsNullOrEmpty(replacementMap.spelling_regex))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(replacementMap.spelling_regex, diacriticAtEnd))
                    {
                        replacementMap.spelling_regex = replacementMap.spelling_regex.Remove(replacementMap.spelling_regex.Length - 1, 1);
                    }
                    replacementMap.spelling_regex += phonemeToAdd;
                }
                if (!string.IsNullOrEmpty(replacementMap.phoneme))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(replacementMap.phoneme, diacriticAtEnd))
                    {
                        replacementMap.phoneme = replacementMap.phoneme.Remove(replacementMap.phoneme.Length - 1, 1);
                    }
                    replacementMap.phoneme += phonemeToAdd;
                }
                PhoneticChanger.ReplaceSoundMapEntry(mapToReplace, replacementMap, languageDescription.sound_map_list);
                phoneticChanger.UpdatePronunciation();
                if (sampleText != string.Empty)
                {
                    sampleText = phoneticChanger.SampleText;
                    txt_SampleText.Text = sampleText;
                    txt_phonetic.Text = string.Empty;
                    foreach (string engineName in speechEngines.Keys)
                    {
                        SpeechEngine speech = speechEngines[engineName];
                        speech.SampleText = sampleText;
                    }
                }
                // Clear the combo boxes
                cbx_phonemeToChange.Items.Clear();
                cbx_replacementPhoneme.Items.Clear();
                tabPhoneticAlterations.SelectedIndex = -1;
            }
            else if ((tabPhoneticAlterations.SelectedIndex == 2) && (rbn_diphthongReplacement.Checked))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_dipthongStartVowel.SelectedIndex == -1) || (cbx_dipthongEndVowel.SelectedIndex == -1))
                {
                    return;
                }
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = "";
                if (BuildNewDiphthong(ref newPhoneme))
                {
                    phoneticChanger.PhoneticChange(oldPhoneme, newPhoneme);
                    // Update sample text
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speechEngine = speechEngines[engineName];
                            speechEngine.SampleText = sampleText;
                        }
                    }
                    // Clear the combo boxes
                    cbx_phonemeToChange.Items.Clear();
                    cbx_replacementPhoneme.Items.Clear();
                    tabPhoneticAlterations.SelectedIndex = -1;
                }
            }
            // Add other options as needed.

            // Mark language and sample dirty
            languageDirty = true;
            sampleDirty = true;
        }

        private void Cbx_replacementPhoneme_MeasureItem(object? sender, System.Windows.Forms.MeasureItemEventArgs e)
        {
            e.ItemWidth = cbx_phonemeToChange.DropDownWidth;
            e.ItemHeight = cbx_replacementPhoneme.Font.Height + 10;
        }

        private void Cbx_replacementPhoneme_DrawItem(object? sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();
                return;
            }
            Brush comboBrush = Brushes.Black; // Set the default color to black

            string cbxEntry = string.Empty;
            if (cbx_replacementPhoneme.Items[e.Index] is string replacementPhoneme)
            {
                cbxEntry = replacementPhoneme;
            }
            bool inInventory = IsInInventory(cbxEntry);
            bool hasSpellingMap = HasSpellingMap(cbxEntry);
            if (inInventory && hasSpellingMap)
            {
                comboBrush = Brushes.Blue;
            }
            else if (inInventory)
            {
                comboBrush = Brushes.Green;
            }
            else if (hasSpellingMap)
            {
                comboBrush = Brushes.Red;
            }

            e.DrawBackground();
            Rectangle rectangle = new(2, e.Bounds.Top + 2, e.Bounds.Height, e.Bounds.Height - 4);
            e.Graphics.DrawString(cbxEntry, cbx_replacementPhoneme.Font, comboBrush,
                new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
        }

        private void Btn_addCurrentChangeToList_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex != 0) &&
                (tabPhoneticAlterations.SelectedIndex != 1) &&
                (tabPhoneticAlterations.SelectedIndex != 2) &&
                (tabPhoneticAlterations.SelectedIndex != 3))
            {
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex == 3) && (rbn_replaceRSpelling.Checked))
            {
                _ = MessageBox.Show("Rhotacize based on spelling can only be performed singly", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (changesToBeMade.Count >= 10)
            {
                _ = MessageBox.Show("No more than 10 changes can be grouped for simultaneous update.  Apply existing updates before adding additional changes",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((tabPhoneticAlterations.SelectedIndex == 0) ||
                (tabPhoneticAlterations.SelectedIndex == 1) ||
                ((tabPhoneticAlterations.SelectedIndex == 3) && (!rbn_replaceRSpelling.Checked)) ||
                ((tabPhoneticAlterations.SelectedIndex == 2) && (!rbn_diphthongReplacement.Checked)))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_replacementPhoneme.SelectedIndex == -1))
                {
                    return;
                }
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = cbx_replacementPhoneme.Text.Split()[0];

                changesToBeMade.Add((oldPhoneme, newPhoneme));
                StringBuilder sb = new();
                foreach ((string oph, string nph) in changesToBeMade)
                {
                    _ = sb.Append(oph);
                    _ = sb.Append(" -> ");
                    _ = sb.Append(nph);
                    _ = sb.Append(", ");
                }
                txt_changeList.Text = sb.ToString();
            }
            else if ((tabPhoneticAlterations.SelectedIndex == 2) && (rbn_diphthongReplacement.Checked))
            {
                if ((cbx_phonemeToChange.SelectedIndex == -1) || (cbx_dipthongStartVowel.SelectedIndex == -1) || (cbx_dipthongEndVowel.SelectedIndex == -1))
                {
                    return;
                }
                string oldPhoneme = cbx_phonemeToChange.Text.Split()[0];
                string newPhoneme = "";
                if (BuildNewDiphthong(ref newPhoneme))
                {
                    phoneticChanger.PhoneticChange(oldPhoneme, newPhoneme);
                    // Update sample text
                    if (sampleText != string.Empty)
                    {
                        changesToBeMade.Add((oldPhoneme, newPhoneme));
                        StringBuilder sb = new();
                        foreach ((string oph, string nph) in changesToBeMade)
                        {
                            _ = sb.Append(oph);
                            _ = sb.Append(" -> ");
                            _ = sb.Append(nph);
                            _ = sb.Append(", ");
                        }
                        txt_changeList.Text = sb.ToString();
                    }
                }
            }

        }

        private void Btn_applyListOfChanges_Click(object sender, EventArgs e)
        {
            phoneticChanger.PhoneticChange(changesToBeMade);
            if (sampleText != string.Empty)
            {
                sampleText = phoneticChanger.SampleText;
                txt_SampleText.Text = sampleText;
                txt_phonetic.Text = string.Empty;
                foreach (string engineName in speechEngines.Keys)
                {
                    SpeechEngine speech = speechEngines[engineName];
                    speech.SampleText = sampleText;
                }
            }
            // Clear the combo boxes
            cbx_phonemeToChange.Items.Clear();
            cbx_replacementPhoneme.Items.Clear();
            changesToBeMade.Clear();
            txt_changeList.Text = string.Empty;
            cbx_phonemeToChange.SelectedIndex = -1;
            cbx_replacementPhoneme.SelectedIndex = -1;
            tabPhoneticAlterations.SelectedIndex = -1;

            // Mark language and sample dirty
            languageDirty = true;
            sampleDirty = true;
        }

        private void Btn_revertLastChange_Click(object sender, EventArgs e)
        {
            phoneticChanger.RevertMostRecentChange();
            txt_phonetic.Text = string.Empty;
            if (!string.IsNullOrEmpty(phoneticChanger.SampleText))
            {
                sampleText = phoneticChanger.SampleText;
                txt_SampleText.Text = sampleText;
            }
            else
            {
                txt_SampleText.Text = string.Empty;
            }

            // Mark language and sample dirty
            languageDirty = true;
            sampleDirty = true;
        }

        private void Cbx_speechEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVoices();
        }

        private void Btn_updateSoundMapList_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            SoundMapListEditorForm soundMapListEditorForm = new()
            {
                SoundMapList = languageDescription.sound_map_list,
                HeaderText = "No specific changes - editing the entire list"
            };
            soundMapListEditorForm.UpdatePhonemeReplacements();
            _ = soundMapListEditorForm.ShowDialog();
            // ShowDialog is modal
            if (soundMapListEditorForm.SoundMapSaved)
            {
                DialogResult result = MessageBox.Show("Preserve the spelling (Yes)?\nNo preserves the pronunciation.", "Spelling or pronunciation", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    languageDescription.sound_map_list = soundMapListEditorForm.SoundMapList;
                    phoneticChanger.UpdatePronunciation();
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speech = speechEngines[engineName];
                            speech.SampleText = sampleText;
                        }
                    }
                }
                else if (result == DialogResult.No)
                {
                    phoneticChanger.UpdateSpelling();
                    languageDescription.sound_map_list = soundMapListEditorForm.SoundMapList;
                    if (sampleText != string.Empty)
                    {
                        sampleText = phoneticChanger.SampleText;
                        txt_SampleText.Text = sampleText;
                        txt_phonetic.Text = string.Empty;
                        foreach (string engineName in speechEngines.Keys)
                        {
                            SpeechEngine speech = speechEngines[engineName];
                            speech.SampleText = sampleText;
                        }
                    }
                }
            }

        }

        private void Rbn_l1_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_normalVowel_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_addRhoticityRegular_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_l2_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_l3_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_allPhonemes_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_halfLongVowels_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_longVowels_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_normalDiphthong_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_semivowelDiphthong_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_longToRhotacized_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_removeRhoticity_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_replaceRhotacized_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_replaceRSpelling_CheckedChanged(object sender, EventArgs e)
        {
            if (rbn_replaceRSpelling.Checked)
            {
                lbl_choiceCbx.Text = "Pronunciation Patter to Update";
                lbl_replacementCbx.Text = "Phoneme to add/replace";
                UpdatePhonemeToChangeCbx();
            }
            else
            {
                lbl_choiceCbx.Text = "Phoneme to change";
                lbl_replacementCbx.Text = "Replacement Phoneme";
            }
        }

        private void SetAmazonPollyURIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pollyURI = Microsoft.VisualBasic.Interaction.InputBox("Enter the URI for Amazon Polly", "Amazon Polly URI", PollySpeech.PollyURI ?? "");
            if (PollySpeech.TestURI(pollyURI))
            {
                PollySpeech.PollyURI = pollyURI;
                PollySpeech pollySpeech = new();
                Dictionary<string, SpeechEngine.VoiceData> amazonPollyVoices = pollySpeech.GetVoices();
                if (speechEngines.ContainsKey(pollySpeech.Description))
                {
                    speechEngines[pollySpeech.Description] = pollySpeech;
                }
                else
                {
                    speechEngines.Add(pollySpeech.Description, pollySpeech);
                }
                if (!voices.TryAdd(pollySpeech.Description, amazonPollyVoices))
                {
                    voices[pollySpeech.Description] = amazonPollyVoices;
                }

                UserConfiguration.PollyURI = pollyURI;
            }
        }

        private void SetESpeakNgLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
            if (string.IsNullOrEmpty(ESpeakNGSpeak.ESpeakNGPath))
            {
                openFileDialog.InitialDirectory = Environment.SpecialFolder.ProgramFiles.ToString();

            }
            else
            {
                FileInfo fi = new(ESpeakNGSpeak.ESpeakNGPath);
                openFileDialog.InitialDirectory = fi.DirectoryName;
            }
            openFileDialog.Filter = "Executable file (*.exe)|*.exe|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ESpeakNGSpeak.ESpeakNGPath = openFileDialog.FileName;
                ESpeakNGSpeak eSpeakNGSpeak = new();
                Dictionary<string, SpeechEngine.VoiceData> espeakVoices = eSpeakNGSpeak.GetVoices();
                if (speechEngines.ContainsKey(eSpeakNGSpeak.Description))
                {
                    speechEngines[eSpeakNGSpeak.Description] = eSpeakNGSpeak;
                }
                else
                {
                    speechEngines.Add(eSpeakNGSpeak.Description, eSpeakNGSpeak);
                }
                if (!voices.TryAdd(eSpeakNGSpeak.Description, espeakVoices))
                {
                    voices[eSpeakNGSpeak.Description] = espeakVoices;
                }

                UserConfiguration.ESpeakNgPath = ESpeakNGSpeak.ESpeakNGPath;
            }

        }


        private void setAzureSpeechKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentKey = UserConfiguration.AzureSpeechKey;
            string azureSpeechKey = Microsoft.VisualBasic.Interaction.InputBox("Enter the Speech (Resource) key for Microsoft Azure", "AzureSpeechKey", currentKey ?? string.Empty);
            if(!string.IsNullOrEmpty(azureSpeechKey))
            {
                UserConfiguration.AzureSpeechKey = azureSpeechKey;
            }
            if (UserConfiguration.IsAzureSupported)
            {
                AzureSpeak azureSpeak = new();
                Dictionary<string, SpeechEngine.VoiceData> azureVoices = azureSpeak.GetVoices();
                speechEngines.Add(azureSpeak.Description, azureSpeak);
                voices.Add(azureSpeak.Description, azureVoices);
            }
        }

        private void setAzureSpeechRegionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentRegion = UserConfiguration.AzureSpeechRegion;
            string azureSpeechRegion = Microsoft.VisualBasic.Interaction.InputBox("Enter the Speech region for Microsoft Azure", "Azure Speech region", currentRegion ?? string.Empty);
            if (!string.IsNullOrEmpty(azureSpeechRegion))
            {
                UserConfiguration.AzureSpeechRegion = azureSpeechRegion;
            }
            if (UserConfiguration.IsAzureSupported)
            {
                AzureSpeak azureSpeak = new();
                Dictionary<string, SpeechEngine.VoiceData> azureVoices = azureSpeak.GetVoices();
                speechEngines.Add(azureSpeak.Description, azureSpeak);
                voices.Add(azureSpeak.Description, azureVoices);
            }
        }

        private void Rbn_vowelToDiphthongStart_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_vowelToDiphthongEnd_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_changeStartVowel_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_ChangeEndVowel_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void Rbn_diphthongReplacement_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePhonemeToChangeCbx();
        }

        private void DeriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }

            if (languageDescription.derived)
            {
                ConlangUtilities.RemoveDerivedEntries(languageDescription);
                deriveToolStripMenuItem.Text = "Derive Words";
            }
            else
            {
                ConlangUtilities.DeriveLexicon(languageDescription);
                deriveToolStripMenuItem.Text = "Remove Derived Words";
            }
        }

        private void EditLexiconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            LexiconEditorForm lexiconEditorForm = new(languageDescription.lexicon, languageDescription.part_of_speech_list, languageDescription.sound_map_list);
            _ = lexiconEditorForm.ShowDialog();
            if (lexiconEditorForm.Saved)
            {
                languageDescription.lexicon = lexiconEditorForm.Lexicon;
            }
        }

        private void SetAndAdjustLexicalOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (languageDescription == null)
            {
                return;
            }
            LexicalOrderEditor editor = new(languageDescription, true);
            _ = editor.ShowDialog();
        }
    }
}
