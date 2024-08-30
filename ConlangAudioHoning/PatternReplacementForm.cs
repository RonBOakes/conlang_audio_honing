using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConlangAudioHoning
{
    /// <summary>
    /// Class for displaying a form for entering a custom regular expression patternToReplace.
    /// </summary>
    public partial class PatternReplacementForm : Form
    {
        private static readonly string s_usageText =
            "The Pattern to be Replaced defines the cluster to be replaced. This is not a regular expression, and " +
            "cannot include wildcards. Typically this should be one of the clusters from the NAD Analysis Clusters " +
            "generated from the Language menu.\n\n" +
            "The Replacement will define the new patternToReplace to be put in place.  And C or V (see below) in this patternToReplace " +
            "will be matched to the corresponding entry in the Pattern to be Replaced.  These must match in order to work\n\n" +
            "To match any vowel put in a capital V, and to match any consonant put in a capital C.  The tool will " +
            "use these as needed performing the updates.";

        private TextBox? _lastFocused;

        private string PatternToReplace
        {
            get; set;
        }

        private string Replacement
        {
            get; set;
        }

        private PatternReplacementForm(string? pattern = null)
        {
            PatternToReplace = pattern ?? string.Empty;
            Replacement = string.Empty;

            InitializeComponent();

            _lastFocused = null;

            CharacterInsertToolStripMenuItem ciMenu = new(false);
            _ = menus.Items.Add(ciMenu);
            ciMenu.AddClickDelegate(CharInsetToolStripMenuItem_Click);
            txt_patternToReplace.GotFocus += Txt_GotFocus;
            txt_replacement.GotFocus += Txt_GotFocus;

            btn_OK.Click += Btn_OK_Click;

            txt_patternToReplace.Text = PatternToReplace;
            txt_replacement.Text = Replacement;

            txt_usageText.Text = s_usageText;
        }

        /// <summary>
        /// Display a custom dialog patternToReplace and wait for it to be filled in and dismissed.
        /// </summary>
        /// <param name="patternToReplace">Pattern to be Replaced provided by the user.</param>
        /// <param name="replacement">Replacement for the Pattern to be replaced.</param>
        /// <returns>DialogResult.OK</returns>
        public static DialogResult Show(out string patternToReplace, out string replacement)
        {

            using PatternReplacementForm patternForm = new();
            patternForm.ShowDialog();
            patternToReplace = patternForm.PatternToReplace;
            replacement = patternForm.Replacement;

            return DialogResult.OK;
        }

        private void Btn_OK_Click(object? sender, EventArgs e)
        {
            PatternToReplace = txt_patternToReplace.Text.Trim();
            Replacement = txt_replacement.Text.Trim();
            if ((string.IsNullOrEmpty(PatternToReplace)) || (string.IsNullOrEmpty(Replacement)))
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
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
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
            if ((_lastFocused != null) && (!string.IsNullOrEmpty(textToAppend)))
            {
                _lastFocused.Text = _lastFocused.Text.Trim() + textToAppend;
            }
        }

        private void Txt_GotFocus(object? sender, EventArgs e)
        {
            if (sender is TextBox box)
            {
                _lastFocused = box;
            }
        }
    }
}
