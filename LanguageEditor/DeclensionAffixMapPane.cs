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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEditor
{
    internal class DeclensionAffixMapPane : UserControl
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
                return _affix_map ??= [];
            }
            set
            {
                _affix_map = value;
                createPartOfSpeechTabs();
                partOfSpeechTabIndex = 0;
                loadPartOfSpeechTab(0);
                // TODO: update to populate contents
            }
        }

        private TabControl tpn_partOfSpeechLevel;

        private int partOfSpeechTabIndex = 0;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DeclensionAffixMapPane()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
        }

        private void createPartOfSpeechTabs()
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

        public void tpn_partOfSpeechLevel_SelectedIndexChanged(object? sender, EventArgs e)
        {
            loadPartOfSpeechTab(tpn_partOfSpeechLevel.SelectedIndex);
        }

        public void this_SizeChanged(object? sender, EventArgs e)
        {
            tpn_partOfSpeechLevel.Size = this.Size;
        }

        private void loadPartOfSpeechTab(int newTabIndex)
        {
            if ((_affix_map == null) || (tpn_partOfSpeechLevel == null) || (tpn_partOfSpeechLevel.TabPages.Count < 1))
            {
                return;
            }
            if (partOfSpeechTabIndex != newTabIndex)
            {
                TabPage oldTab = tpn_partOfSpeechLevel.TabPages[partOfSpeechTabIndex];
                oldTab.SuspendLayout();
                oldTab.Controls.Clear();
                oldTab.ResumeLayout(true);
            }

            TabPage tab = tpn_partOfSpeechLevel.TabPages[newTabIndex];
            tab.Size = this.Size;
            tab.SuspendLayout();
            tab.AutoScroll = true;
            int xPos = 0, yPos = 0;
            tab.Controls.Clear();
            foreach (Dictionary<string, List<Dictionary<string, Affix>>> entry in _affix_map[tab.Text])
            {
                PosSubPane posSubPane = new();
                posSubPane.PosSubMap = entry;
                posSubPane.Location = new Point(xPos, yPos);
                posSubPane.Size = new Size(tab.Size.Width, 350);
                posSubPane.BorderStyle = BorderStyle.FixedSingle;
                tab.Controls.Add(posSubPane);
                yPos += posSubPane.Height + 5;
            }
            tab.ResumeLayout(true);
        }

        private void InitializeComponent()
        {
            this.Size = InitialSize;
            tpn_partOfSpeechLevel = new TabControl();
            tpn_partOfSpeechLevel.Size = this.Size;
            tpn_partOfSpeechLevel.Location = new System.Drawing.Point(0, 0);
            this.Controls.Add(tpn_partOfSpeechLevel);
            tpn_partOfSpeechLevel.SelectedIndexChanged += tpn_partOfSpeechLevel_SelectedIndexChanged;
            this.SizeChanged += this_SizeChanged;
        }

        private class PosSubPane : UserControl
        {
            private Dictionary<string, List<Dictionary<string, Affix>>>? _posSubMap;
            private bool dataChanged;

            public Dictionary<string, List<Dictionary<string, Affix>>> PosSubMap
            {
                get
                {
                    updateData();
                    return _posSubMap ??= [];
                }
                set
                {
                    _posSubMap = value;
                    createAffixTabs();
                    affixTabIndex = 0;
                    loadAffixTab(0);
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

            private TabControl tpn_affixLevel = new();
            private int affixTabIndex = 0;

            public PosSubPane()
            {
                InitializeComponent();
            }

            public void tpn_affixLevel_SelectedIndexChanged(object? sender, EventArgs e)
            {
                loadAffixTab(tpn_affixLevel.SelectedIndex);
            }

            private void createAffixTabs()
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

            private void loadAffixTab(int newTabIndex)
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
                        DeclensionAffixEditor declensionAffixEditor = new();
                        declensionAffixEditor.Declension = key;
                        declensionAffixEditor.AffixRules = entry[key];
                        declensionAffixEditor.Location = new Point(xPos, yPos);
                        declensionAffixEditor.BorderStyle = BorderStyle.FixedSingle;
                        tab.Controls.Add(declensionAffixEditor);
                        yPos += declensionAffixEditor.Height + 5;
                    }
                }
                tab.ResumeLayout(true);
            }

            void updateData()
            {
                if (_posSubMap == null)
                {
                    _posSubMap = [];
                }

                DeclensionAffixEditor das = new();
                _posSubMap.Clear();
                foreach (TabPage tab in tpn_affixLevel.TabPages)
                {
                    string affix = tab.Text;
                    List<Dictionary<string, Affix>> entryList = [];
                    foreach (Control ctl in tab.Controls)
                    {
                        if (ctl.GetType().IsInstanceOfType(das))
                        {
                            DeclensionAffixEditor declensionAffixEditor = (DeclensionAffixEditor)ctl;
                            string declension = declensionAffixEditor.Declension;
                            Affix affixRules = declensionAffixEditor.AffixRules;
                            entryList.Add(new Dictionary<string, Affix> { { declension, affixRules } });
                        }
                    }
                    _posSubMap[affix] = entryList;
                }
            }

            public void this_SizeChanged(object? sender, EventArgs e)
            {
                tpn_affixLevel.Size = this.Size;
            }


            private void InitializeComponent()
            {
                tpn_affixLevel = new TabControl();
                tpn_affixLevel.Size = this.Size;
                tpn_affixLevel.Location = new System.Drawing.Point(0, 0);
                this.Controls.Add(tpn_affixLevel);
                tpn_affixLevel.SelectedIndexChanged += tpn_affixLevel_SelectedIndexChanged;
                this.SizeChanged += this_SizeChanged;
            }
        }
    }
}
