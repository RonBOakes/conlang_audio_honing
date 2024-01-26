/*
 * Custom code for the User Control to contain the entire lexicon for a conlang.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanguageEditor
{
    public partial class LexiconEditorPane : UserControl
    {
        private List<LexiconEntry>? _lexicon;
        private List<string>? _lexicalOrderList;

        private List<LexiconEntry>? entriesToReplace;

        internal List<LexiconEntry> Lexicon
        {
            get
            {
                return _lexicon ?? new List<LexiconEntry>();
            }
            set
            {
                _lexicon = value;
                createTabs();
                _lastTabIndex = 0;
                loadTab(0);
            }
        }

        internal List<string> LexicalOrderList
        {
            get
            {
                return _lexicalOrderList ?? new List<string>();
            }
            set
            {
                _lexicalOrderList = value;
            }
        }

        private int _lastTabIndex = 0;
        private int _startEntry = 0;
        private bool _dirty = false;

        public LexiconEditorPane()
        {
            InitializeComponent();
            rdo_english.Checked = true;
            btn_earlier.Enabled = false;
            btn_earlier.Visible = false;
            btn_later.Enabled = false;
            btn_later.Visible = false;
        }

        public void tpn_lexiconEditors_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadTab(tpn_lexiconEditors.SelectedIndex);
        }

        public void LexiconEditorPane_Load(object sender, EventArgs e)
        {
            rdo_english.Checked = true;
        }

        private void rdo_english_CheckedChanged(object sender, EventArgs e)
        {
            if (Lexicon != null)
            {
                if (rdo_english.Checked)
                {
                    Lexicon.Sort(new LexiconEntry.LexicalOrderCompEnglish());
                }
                else
                {
                    Lexicon.Sort(new LexiconEntry.LexicalOrderCompSpelling());
                }
                createTabs();
            }
        }

        private void createTabs()
        {
            if (Lexicon != null)
            {
                _lastTabIndex = 0;
                _startEntry = 0;
                this.SuspendLayout();
                this.tpn_lexiconEditors.SuspendLayout();
                this.tpn_lexiconEditors.TabPages.Clear();
                if (rdo_english.Checked)
                {
                    for (char let = 'a'; let <= 'z'; let++)
                    {
                        string title = Char.ToString(let);
                        TabPage tab = new(title);
                        tpn_lexiconEditors.TabPages.Add(tab);
                    }
                }
                else
                {
                    foreach (string ent in LexicalOrderList)
                    {
                        TabPage tab = new(ent);
                        tpn_lexiconEditors.TabPages.Add(tab);
                    }
                }
                loadTab(0);
                tpn_lexiconEditors.ResumeLayout(true);
                this.ResumeLayout(true);
            }
            else
            {
                this.SuspendLayout();
                this.tpn_lexiconEditors.SuspendLayout();
                tpn_lexiconEditors.TabPages.Clear();
                tpn_lexiconEditors.ResumeLayout(true);
                this.ResumeLayout(true);
            }
        }

        private void loadTab(int newTabIndex)
        {
            if (tpn_lexiconEditors.TabPages.Count < 1)
            {
                return;
            }
            TabPage oldTab = tpn_lexiconEditors.TabPages[_lastTabIndex];
            oldTab.SuspendLayout();
            if (entriesToReplace == null)
            {
                entriesToReplace = [];
            }
            // If we have entries to replace, they are on this tab
            if ((entriesToReplace != null) && (entriesToReplace.Count > 0) && (_dirty))
            {
                foreach (LexiconEntry entry in entriesToReplace)
                {
                    Lexicon.Remove(entry);
                }
                foreach (LexiconEditor editor in oldTab.Controls)
                {
                    LexiconEntry entry = editor.lexiconEntry;
                    if (entry != null)
                    {
                        Lexicon.Add(entry);
                    }
                }
                if (rdo_english.Checked)
                {
                    Lexicon.Sort(new LexiconEntry.LexicalOrderCompEnglish());
                }
                else
                {
                    Lexicon.Sort(new LexiconEntry.LexicalOrderCompSpelling());
                }
                _dirty = false;
            }
            oldTab.Controls.Clear();
            oldTab.ResumeLayout(true);
            if (TabIndex != newTabIndex)
            {
                _startEntry = 0;
            }

            TabPage tab = tpn_lexiconEditors.TabPages[newTabIndex];
            tab.SuspendLayout();
            tab.AutoScroll = true;

            this.btn_later.Enabled = false;
            this.btn_later.Visible = false;
            if (_startEntry == 0)
            {
                this.btn_earlier.Enabled = false;
                this.btn_earlier.Visible = false;
            }
            else
            {
                this.btn_earlier.Enabled = true;
                this.btn_earlier.Visible = true;
            }

            int xPos = 0, yPos = 0;
            tab.Controls.Clear();
            entriesToReplace?.Clear();
            int count = 0;
            foreach (LexiconEntry lexiconEntry in Lexicon)
            {
                if (((rdo_english.Checked) && (lexiconEntry.english.StartsWith(tab.Text, StringComparison.OrdinalIgnoreCase))) ||
                    ((rdo_spelled.Checked) && (lexiconEntry.spelled.StartsWith(tab.Text, StringComparison.OrdinalIgnoreCase))))
                {
                    if (count >= _startEntry)
                    {
                        LexiconEditor lexiconEditor = new();
                        lexiconEditor.lexiconEntry = lexiconEntry;
                        lexiconEditor.Location = new Point(xPos, yPos);
                        yPos += lexiconEditor.Height;
                        tab.Controls.Add(lexiconEditor);
                        lexiconEditor.Changed += LexiconEditor_Changed;
                        entriesToReplace?.Add(lexiconEntry);
                    }
                    if (++count > _startEntry + 200)
                    {
                        this.btn_later.Enabled = true;
                        this.btn_later.Visible = true;
                        break;
                    }
                }
            }
            tab.ResumeLayout(true);
            _lastTabIndex = newTabIndex;
        }

        private void LexiconEditor_Changed(object? sender, EventArgs e)
        {
            _dirty = true;
        }

        private void btn_earlier_Click(object sender, EventArgs e)
        {
            _startEntry = (_startEntry - 200) > 0 ? _startEntry - 200 : 0;
            loadTab(_lastTabIndex);
        }

        private void btn_later_Click(object sender, EventArgs e)
        {
            _startEntry += 200;
            loadTab(_lastTabIndex);
        }
    }
}
