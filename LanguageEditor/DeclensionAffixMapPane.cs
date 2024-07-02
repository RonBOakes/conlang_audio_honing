/*
 * Creates the pane that contains a declension editor for each part of speech.
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
using System.Text.RegularExpressions;

namespace LanguageEditor
{
    internal partial class DeclensionAffixMapPane : Panel
    {
        private static Size InitialSize = new(895, 355);
        private Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>>? _affix_map;

        /*
         * The outer dictionary maps Parts of speech to the next level.
         *      The next level is a list of dictionaries which map "prefix," "suffix," and "replacement," 
         *          The next level is a list of dictionaries which map the declension to the Affix.  These will be
         *          represented by DeclensionAffixEditor UserControls
         */

        public Dictionary<string, List<Dictionary<string, List<Dictionary<string, Affix>>>>>? AffixMap
        {
            get
            {
                if ((_affix_map == null) || (tpn_partOfSpeechLevel == null))
                {
                    return [];
                }

                TabPage? tab = tpn_partOfSpeechLevel.SelectedTab;
                if (tab != null)
                {
                    string partOfSpeech = tab.Text;
                    Control control = tab.Controls[0];
                    if (control is not PosSubPane)
                    {
                        return _affix_map;
                    }
                    PosSubPane subPane = (PosSubPane)control;
                    if (subPane.DataChanged)
                    {
                        _affix_map[partOfSpeech].Clear();
                        _affix_map[partOfSpeech].Add(subPane.PosSubMap);
                    }

                    return _affix_map;
                }

                return _affix_map;
            }
            set
            {
                _affix_map = value;
                CreatePartOfSpeechTabs();
                partOfSpeechTabIndex = 0;
                LoadPartOfSpeechTab(0);
            }
        }

        public List<SpellingPronunciationRules> SpellingPronunciationRules
        {
            private get;
            set;
        }

        private TabControl tpn_partOfSpeechLevel;

        private int partOfSpeechTabIndex = 0;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DeclensionAffixMapPane()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
        }

        private void CreatePartOfSpeechTabs()
        {
            if (_affix_map != null)
            {
                partOfSpeechTabIndex = 0;
                this.SuspendLayout();
                tpn_partOfSpeechLevel.SuspendLayout();
                tpn_partOfSpeechLevel.TabPages.Clear();
                foreach (string partOfSpeech in _affix_map.Keys)
                {
                    TabPage tab = new(partOfSpeech);
                    tpn_partOfSpeechLevel.TabPages.Add(tab);
                }
                tpn_partOfSpeechLevel.ResumeLayout(true);
                this.ResumeLayout(true);
            }
        }

        public void Tpn_partOfSpeechLevel_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadPartOfSpeechTab(tpn_partOfSpeechLevel.SelectedIndex);
        }

        public void DeclensionAffixMapPane_SizeChanged(object? sender, EventArgs e)
        {
            tpn_partOfSpeechLevel.Size = new Size(this.Size.Width - 2, this.Size.Height - 2);
        }

        private void LoadPartOfSpeechTab(int newTabIndex)
        {
            if ((_affix_map == null) || (tpn_partOfSpeechLevel == null) || (tpn_partOfSpeechLevel.TabPages.Count < 1))
            {
                return;
            }
            if (partOfSpeechTabIndex != newTabIndex)
            {
                TabPage oldTab = tpn_partOfSpeechLevel.TabPages[partOfSpeechTabIndex];
                string oldPartOfSpeech = oldTab.Text;
                _affix_map[oldPartOfSpeech].Clear();
                _affix_map[oldPartOfSpeech].AddRange(from Control control1 in oldTab.Controls
                                                     where control1 is PosSubPane
                                                     let subPane = (PosSubPane)control1
                                                     let PosSubMap = subPane.PosSubMap
                                                     select PosSubMap);
                oldTab.SuspendLayout();
                oldTab.Controls.Clear();
                oldTab.ResumeLayout(true);
            }

            TabPage tab = tpn_partOfSpeechLevel.TabPages[newTabIndex];
            tab.Size = this.Size;
            tab.SuspendLayout();
            tab.AutoScroll = false;
            int xPos = 0, yPos = 0;
            tab.Controls.Clear();
            foreach (Dictionary<string, List<Dictionary<string, Affix>>> entry in _affix_map[tab.Text])
            {
                PosSubPane posSubPane = new()
                {
                    ParentPane = this,
                    PartOfSpeech = tab.Text,
                    Location = new Point(xPos, yPos),
                    Size = new Size(tab.Size.Width - 2, tab.Size.Height - 20),
                    BorderStyle = BorderStyle.FixedSingle,
                    SpellingPronunciationRules = SpellingPronunciationRules,
                    PosSubMap = entry,
                };
                tab.Controls.Add(posSubPane);
                yPos += posSubPane.Height + 5;
            }
            tab.ResumeLayout(true);
        }

        private void InitializeComponent()
        {
            this.Size = InitialSize;
            tpn_partOfSpeechLevel = new TabControl
            {
                // Size = this.size,
                Size = new Size(this.Size.Width - 2, this.Size.Height - 2),
                Location = new System.Drawing.Point(0, 0),
            };
            this.Controls.Add(tpn_partOfSpeechLevel);
            tpn_partOfSpeechLevel.SelectedIndexChanged += Tpn_partOfSpeechLevel_SelectedIndexChanged;
            this.SizeChanged += DeclensionAffixMapPane_SizeChanged;
        }


        private void AddButton_Click(object? sender, EventArgs e)
        {
            if ((sender == null) || (sender.GetType() != typeof(Button)))
            {
                return;
            }
            Button addButton = (Button)sender;
            string buttonText = addButton.Text;
            Match buttonMatch = ButtonTextRegex().Match(buttonText);
            if (buttonMatch.Success)
            {
                string affixType = buttonMatch.Groups[1].Value;
                string partOfSpeech = buttonMatch.Groups[2].Value;

                TabPage? posTab = null;
                foreach (var tabPage1 in from TabPage tabPage1 in tpn_partOfSpeechLevel.TabPages
                                         where tabPage1.Text.Trim().Equals(partOfSpeech.Trim())
                                         select tabPage1)
                {
                    posTab = tabPage1;
                }

                if (posTab == null)
                {
                    return;
                }
                Control posControl = posTab.Controls[0];
                if ((posControl is not PosSubPane))
                {
                    return;
                }
                PosSubPane posPane = (PosSubPane)posControl;

                TabPage? affixTab = null;
                foreach (var tabPage2 in from TabPage tabPage2 in posPane.tpn_affixLevel.TabPages
                                         where tabPage2.Text.Trim().Equals(affixType.Trim())
                                         select tabPage2)
                {
                    affixTab = tabPage2;
                }

                if (affixTab == null)
                {
                    return;
                }

                int controlInx = 0;
                int xPos = 0, yPos = 0;
                Control ctl;
#pragma warning disable S1121 // Assignments should not be made from within sub-expressions
                while ((ctl = affixTab.Controls[controlInx]).GetType() == typeof(DeclensionAffixEditor))
                {
                    yPos += ctl.Height + 5;
                    controlInx += 1;
                }
#pragma warning restore S1121 // Assignments should not be made from within sub-expressions

                affixTab.SuspendLayout();
                Affix rules = new();
                DeclensionAffixEditor editor = new()
                {
                    Declension = string.Empty,
                    AffixRules = rules,
                    PartOfSpeech = partOfSpeech,
                    AffixType = affixType,
                    Location = new Point(xPos, yPos),
                    BorderStyle = BorderStyle.FixedSingle,
                    SpellingPronunciationRules = SpellingPronunciationRules ?? [],
                };
                editor.Changed += posPane.DeclensionAffixEditor_Changed;
                editor.Delete += DeclensionAffixEditor_Delete;
                affixTab.Controls.Remove(ctl);
                affixTab.Controls.Add(editor);
                yPos += editor.Height + 5;
                ctl.Location = new Point(xPos, yPos);
                affixTab.Controls.Add(ctl);
                affixTab.ResumeLayout(true);

                if (_affix_map != null)
                {
                    _affix_map[partOfSpeech][0][affixType][0].TryAdd(string.Empty, rules);
                    posPane.PosSubMap = _affix_map[partOfSpeech][0];
                }
            }
        }


        internal void DeclensionAffixEditor_Delete(object? sender, EventArgs e)
        {
            if (_affix_map == null)
            {
                return;
            }
            if ((sender == null) || (sender.GetType() != typeof(DeclensionAffixEditor)))
            {
                return;
            }
            if (e.GetType() == typeof(DeclensionAffixEditor.DeclensionAffixEntryEventArgs))
            {
                DeclensionAffixEditor.DeclensionAffixEntryEventArgs e2 = (DeclensionAffixEditor.DeclensionAffixEntryEventArgs)e;
                if ((!string.IsNullOrEmpty(e2.AffixType)) && (!string.IsNullOrEmpty(e2.PartOfSpeech)) && (e2.Declension != null))
                {
                    TabPage? posTab = null;
                    foreach (var tabPage1 in from TabPage tabPage1 in tpn_partOfSpeechLevel.TabPages
                                             where tabPage1.Text.Trim().Equals(e2.PartOfSpeech.Trim())
                                             select tabPage1)
                    {
                        posTab = tabPage1;
                    }

                    if (posTab == null)
                    {
                        return;
                    }
                    Control posControl = posTab.Controls[0];
                    if ((posControl is not PosSubPane))
                    {
                        return;
                    }
                    PosSubPane posPane = (PosSubPane)posControl;

                    foreach (var subDict in from Dictionary<string, Affix> subDict in _affix_map[e2.PartOfSpeech][0][e2.AffixType]
                                            where subDict.ContainsKey(e2.Declension)
                                            select (subDict))
                    {
                        subDict.Remove(e2.Declension);
                    }
                    posPane.PosSubMap = _affix_map[e2.PartOfSpeech][0];
                }
            }
        }

        private sealed class PosSubPane : Panel
        {
            private Dictionary<string, List<Dictionary<string, Affix>>>? _posSubMap;
            private bool dataChanged;

            public Dictionary<string, List<Dictionary<string, Affix>>> PosSubMap
            {
                get
                {
                    UpdateData();
                    return _posSubMap ??= [];
                }
                set
                {
                    _posSubMap = value;
                    DataChanged = false;
                    CreateAffixTabs();
                    affixTabIndex = 0;
                    LoadAffixTab(0);
                }
            }

            public bool DataChanged
            {
                get
                {
                    return dataChanged;
                }
                private set
                {
                    dataChanged = value;
                }
            }

            public List<SpellingPronunciationRules>? SpellingPronunciationRules
            {
                private get;
                set;
            }

            public string? PartOfSpeech
            {
                private get;
                set;
            }

            public DeclensionAffixMapPane? ParentPane
            {
                private get;
                set;
            }

            internal TabControl tpn_affixLevel = new();
            private int affixTabIndex = 0;

            public PosSubPane()
            {
                InitializeComponent();
            }

            public void Tpn_affixLevel_SelectedIndexChanged(object? sender, EventArgs e)
            {
                LoadAffixTab(tpn_affixLevel.SelectedIndex);
            }

            private void CreateAffixTabs()
            {
                if (_posSubMap != null)
                {
                    affixTabIndex = 0;
                    this.SuspendLayout();
                    tpn_affixLevel.SuspendLayout();
                    tpn_affixLevel.TabPages.Clear();
                    foreach (string affix in _posSubMap.Keys)
                    {
                        TabPage tab = new(affix);
                        tpn_affixLevel.TabPages.Add(tab);
                    }
                    tpn_affixLevel.ResumeLayout(true);
                    this.ResumeLayout(true);
                }
            }

            private void LoadAffixTab(int newTabIndex)
            {
                if ((_posSubMap == null) || (tpn_affixLevel == null) || (tpn_affixLevel.TabPages.Count < 1))
                {
                    return;
                }

                TabPage oldTab = tpn_affixLevel.TabPages[affixTabIndex];
                if (DataChanged)
                {
                    _posSubMap[oldTab.Text.Trim()].Clear();
                    foreach (DeclensionAffixEditor editor in oldTab.Controls)
                    {
                        Dictionary<string, Affix> dict = [];
                        dict[editor.Declension] = editor.AffixRules;
                        _posSubMap[oldTab.Text.Trim()].Add(dict);
                    }
                    dataChanged = false;
                }
                if (affixTabIndex != newTabIndex)
                {
                    oldTab.SuspendLayout();
                    oldTab.Controls.Clear();
                    oldTab.ResumeLayout(true);
                }

                TabPage tab = tpn_affixLevel.TabPages[newTabIndex];
                tab.SuspendLayout();
                tab.AutoScroll = true;
                int xPos = 0, yPos = 0;
                tab.Controls.Clear();
                foreach (Dictionary<string, Affix> entry in _posSubMap[tab.Text])
                {
                    foreach (string key in entry.Keys) // Should only be one entry
                    {
                        DeclensionAffixEditor declensionAffixEditor = new()
                        {
                            Declension = key,
                            AffixRules = entry[key],
                            PartOfSpeech = PartOfSpeech,
                            AffixType = tab.Text,
                            Location = new Point(xPos, yPos),
                            BorderStyle = BorderStyle.FixedSingle,
                            SpellingPronunciationRules = SpellingPronunciationRules ?? [],
                        };
                        declensionAffixEditor.Changed += DeclensionAffixEditor_Changed;
                        if (ParentPane != null)
                        {
                            declensionAffixEditor.Delete += ParentPane.DeclensionAffixEditor_Delete;
                        }
                        tab.Controls.Add(declensionAffixEditor);
                        yPos += declensionAffixEditor.Height + 5;
                    }
                }
                string buttonText = string.Format("Add {0} Declension to {1}", tab.Text, PartOfSpeech);
                Button addButton = new()
                {
                    Text = buttonText,
                    Size = new Size(300, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(xPos, yPos)
                };
                if (ParentPane != null)
                {
                    addButton.Click += ParentPane.AddButton_Click;
                }
                tab.Controls.Add(addButton);

                tab.ResumeLayout(true);
            }
            internal void DeclensionAffixEditor_Changed(object? sender, EventArgs e)
            {
                if (_posSubMap == null)
                {
                    return;
                }
                if ((sender == null) || (sender.GetType() != typeof(DeclensionAffixEditor)))
                {
                    return;
                }
                DeclensionAffixEditor editor = (DeclensionAffixEditor)sender;
                if (e.GetType() == typeof(DeclensionAffixEditor.DeclensionAffixChangedEventArgs))
                {
                    DeclensionAffixEditor.DeclensionAffixChangedEventArgs e2 = (DeclensionAffixEditor.DeclensionAffixChangedEventArgs)e;
                    if ((e2.DeclensionTextChanged) && (!string.IsNullOrEmpty(e2.AffixType)) && (e2.Declension != null))
                    {
                        foreach (var (subDict, temp) in from Dictionary<string, Affix> subDict in _posSubMap[e2.AffixType]
                                                        where subDict.ContainsKey(e2.Declension)
                                                        let temp = subDict[e2.Declension]
                                                        select (subDict, temp))
                        {
                            subDict.Remove(e2.Declension);
                            subDict.Add(editor.Declension, temp);
                        }
                    }
                }
                dataChanged = true;
            }

            void UpdateData()
            {
                _posSubMap ??= [];

                DeclensionAffixEditor das = new();
                _posSubMap.Clear();
                foreach (TabPage tab in tpn_affixLevel.TabPages)
                {
                    string affix = tab.Text;
                    List<Dictionary<string, Affix>> entryList = [];
                    entryList.AddRange(from Control ctl in tab.Controls
                                       where ctl.GetType().IsInstanceOfType(das)
                                       let declensionAffixEditor = (DeclensionAffixEditor)ctl
                                       let declension = declensionAffixEditor.Declension
                                       let affixRules = declensionAffixEditor.AffixRules
                                       select new Dictionary<string, Affix> { { declension, affixRules } });
                    _posSubMap[affix] = entryList;
                }
            }

            public void PosSubPane_SizeChanged(object? sender, EventArgs e)
            {
                tpn_affixLevel.Size = new Size(this.Size.Width - 2, this.Size.Height - 2);
            }

            private void InitializeComponent()
            {
                tpn_affixLevel = new TabControl
                {
                    Size = new Size(this.Size.Width - 2, this.Size.Height - 2),
                    Location = new System.Drawing.Point(0, 0)
                };
                this.Controls.Add(tpn_affixLevel);
                tpn_affixLevel.SelectedIndexChanged += Tpn_affixLevel_SelectedIndexChanged;
                this.SizeChanged += PosSubPane_SizeChanged;
            }
        }

        [GeneratedRegex(@"^\s*Add\s+(\w+)\s+Declension\s+to\s+(\w+)\s*$")]
        private static partial Regex ButtonTextRegex();
    }
}
