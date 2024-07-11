using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Class for displaying a form for entering a custom regular expression pattern.
    /// </summary>
    public partial class CustomPatternForm : Form
    {
        private string Pattern
        {
            get; set;
        }

        private CustomPatternForm(string? pattern = null)
        {
            Pattern = pattern ?? string.Empty;
            InitializeComponent();
            CharacterInsertToolStripMenuItem ciMenu = new();
            _ = menus.Items.Add(ciMenu);
            ciMenu.AddClickDelegate(CharInsetToolStripMenuItem_Click);
            btn_OK.Click += Btn_OK_Click;

            txt_pattern.Text = Pattern;
        }

        /// <summary>
        /// Display a custom dialog pattern and wait for it to be filled in and dismissed.
        /// </summary>
        /// <param name="pattern">Pattern provided by the user.</param>
        /// <param name="startingPattern">Optional starting pattern to be edited.</param>
        /// <returns>DialogResult.OK</returns>
        public static DialogResult Show(out string pattern, string? startingPattern = null)
        {

            using CustomPatternForm patternForm = new(startingPattern);
            patternForm.ShowDialog();
            pattern = patternForm.Pattern;

            return DialogResult.OK;
        }

        private void Btn_OK_Click(object? sender, EventArgs e)
        {
            Pattern = txt_pattern.Text.Trim();
            if (string.IsNullOrEmpty(Pattern))
            {
                return;
            }
            this.Close();
        }
        private void CharInsetToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }
#pragma warning disable IDE0007 // Use implicit type
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
#pragma warning restore IDE0007 // Use implicit type
            if (string.IsNullOrEmpty(menuItem.Text))
            {
                return;
            }
            Match menuTextMatch = CharacterInsertToolStripMenuItem.RegexMenuTextRegex().Match(menuItem.Text);
            string charToInsert;
            if (menuTextMatch.Success)
            {
                if (menuTextMatch.Groups[1].Value.Contains("Vowel"))
                {
                    charToInsert = CharacterInsertToolStripMenuItem.VowelMatchPattern;
                }
                else
                {
                    charToInsert = CharacterInsertToolStripMenuItem.ConsonantMatchPattern;
                }
            }
            else
            {
                charToInsert = menuItem.Text.Split()[0];
            }
            PasteIntoFocusedBox(charToInsert);
        }

        /// <summary>
        /// Append the supplied string to which of the four text boxes that most recently
        /// had focus.
        /// </summary>
        /// <param name="textToAppend">string containing the text to append.</param>
        public void PasteIntoFocusedBox(string textToAppend)
        {
            if (!string.IsNullOrEmpty(textToAppend))
            {
                Clipboard.SetText(textToAppend);
                txt_pattern.Paste();
            }
        }
    }
}
