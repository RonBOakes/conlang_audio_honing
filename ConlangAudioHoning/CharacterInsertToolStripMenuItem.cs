/*
 * (top level) menu item for inserting characters.
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
namespace ConlangAudioHoning
{
    /// <summary>
    /// Menu item to provide a top-level menu for inserting IPA characters and 
    /// latin diacritics into TextBoxes or other controls.
    /// </summary>
    internal class CharacterInsertToolStripMenuItem : ToolStripMenuItem
    {
        // First level menus
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
        private readonly ToolStripMenuItem pulmonicConsonantsToolStripMenuItem;
        private readonly ToolStripMenuItem nonPulmonicConsonantsToolStripMenuItem;
        private readonly ToolStripMenuItem vowelsToolStripMenuItem;
        private readonly ToolStripMenuItem symbolsToolStripMenuItem;
        private readonly ToolStripMenuItem latinTextDiacriticsToolStripMenuItem;
        // Pulmonic Consonant Submenu
        private readonly ToolStripMenuItem plosiveToolStripMenuItem;
        private readonly ToolStripMenuItem nasalToolStripMenuItem;
        private readonly ToolStripMenuItem trillStripMenuItem;
        private readonly ToolStripMenuItem tapOrFlapToolStripMenuItem;
        private readonly ToolStripMenuItem fricativeToolStripMenuItem;
        private readonly ToolStripMenuItem lateralFricativeToolStripMenuItem;
        private readonly ToolStripMenuItem approximateToolStripMenuItem;
        private readonly ToolStripMenuItem lateralApproximateToolStripMenuItem;
        // Other Consonants Submenu
        private readonly ToolStripMenuItem clicksToolStripMenuItem;
        private readonly ToolStripMenuItem alveolarLateralClickToolStripMenuItem;
        private readonly ToolStripMenuItem voicedImplosivesToolStripMenuItem;
        private readonly ToolStripMenuItem otherConsonantsToolStripMenuItem;
        // Plosive Consonants
        private readonly ToolStripMenuItem ʈToolStripMenuItem;
        private readonly ToolStripMenuItem ɖToolStripMenuItem;
        private readonly ToolStripMenuItem ɟToolStripMenuItem;
        private readonly ToolStripMenuItem ɡToolStripMenuItem;
        private readonly ToolStripMenuItem ɢToolStripMenuItem;
        private readonly ToolStripMenuItem ʔToolStripMenuItem;
        // Nasal Consonants
        private readonly ToolStripMenuItem ɱToolStripMenuItem;
        private readonly ToolStripMenuItem ɳToolStripMenuItem;
        private readonly ToolStripMenuItem ɲToolStripMenuItem;
        private readonly ToolStripMenuItem ŋToolStripMenuItem;
        private readonly ToolStripMenuItem ɴToolStripMenuItem;
        // Trill Consonants
        private readonly ToolStripMenuItem ʙToolStripMenuItem;
        private readonly ToolStripMenuItem rToolStripMenuItem;
        private readonly ToolStripMenuItem ʀToolStripMenuItem;
        private readonly ToolStripMenuItem fToolStripMenuItem;
        private readonly ToolStripMenuItem vToolStripMenuItem;
        private readonly ToolStripMenuItem sToolStripMenuItem;
        private readonly ToolStripMenuItem zToolStripMenuItem;
        private readonly ToolStripMenuItem xToolStripMenuItem;
        // Tap or Flap Consonants
        private readonly ToolStripMenuItem ⱱToolStripMenuItem;
        private readonly ToolStripMenuItem ɾToolStripMenuItem;
        private readonly ToolStripMenuItem ɽToolStripMenuItem;
        // Fricative Consonants
        private readonly ToolStripMenuItem ɸToolStripMenuItem;
        private readonly ToolStripMenuItem βToolStripMenuItem;
        private readonly ToolStripMenuItem θToolStripMenuItem;
        private readonly ToolStripMenuItem ðToolStripMenuItem;
        private readonly ToolStripMenuItem ʃToolStripMenuItem;
        private readonly ToolStripMenuItem ʒToolStripMenuItem;
        private readonly ToolStripMenuItem ʂToolStripMenuItem;
        private readonly ToolStripMenuItem ʂToolStripMenuItem1;
        private readonly ToolStripMenuItem çToolStripMenuItem;
        private readonly ToolStripMenuItem ʝToolStripMenuItem;
        private readonly ToolStripMenuItem ɣToolStripMenuItem;
        private readonly ToolStripMenuItem χToolStripMenuItem;
        private readonly ToolStripMenuItem ʁToolStripMenuItem;
        private readonly ToolStripMenuItem ħToolStripMenuItem;
        private readonly ToolStripMenuItem ʕToolStripMenuItem;
        private readonly ToolStripMenuItem ʜToolStripMenuItem;
        private readonly ToolStripMenuItem ʢToolStripMenuItem;
        private readonly ToolStripMenuItem ɦToolStripMenuItem;
        // Lateral Fricative Consonants
        private readonly ToolStripMenuItem ɬToolStripMenuItem;
        private readonly ToolStripMenuItem ɮToolStripMenuItem;
        // Approximant Consonants
        private readonly ToolStripMenuItem ʋToolStripMenuItem;
        private readonly ToolStripMenuItem ɹToolStripMenuItem;
        private readonly ToolStripMenuItem ɻToolStripMenuItem;
        private readonly ToolStripMenuItem ɰToolStripMenuItem;
        // Lateral Approximant Consonants
        private readonly ToolStripMenuItem ɭToolStripMenuItem;
        private readonly ToolStripMenuItem ʎToolStripMenuItem;
        private readonly ToolStripMenuItem ʟToolStripMenuItem;
        // Click Consonants
        private readonly ToolStripMenuItem ʘToolStripMenuItem;
        private readonly ToolStripMenuItem ǀDentalClickToolStripMenuItem;
        private readonly ToolStripMenuItem ǃAlveolarClickToolStripMenuItem;
        private readonly ToolStripMenuItem ǂClickToolStripMenuItem;
        // Voiced Implosives
        private readonly ToolStripMenuItem ɓBilabialVoicedImplosiveToolStripMenuItem;
        private readonly ToolStripMenuItem ɗAlveolarVoicedImplosiveToolStripMenuItem;
        private readonly ToolStripMenuItem ʄPalatalVoicedImplosiveToolStripMenuItem;
        private readonly ToolStripMenuItem ɠVelarVoicedImplosiveToolStripMenuItem;
        private readonly ToolStripMenuItem ʛUvularVoicedImplosiveToolStripMenuItem;
        // Other consonants
        private readonly ToolStripMenuItem ʍVoicelessLabialVelarApproximateToolStripMenuItem;
        private readonly ToolStripMenuItem ɥVoicedLabialPalatalApproximateToolStripMenuItem;
        //  Vowels including submenus
        private readonly ToolStripMenuItem closeToolStripMenuItem;
        private readonly ToolStripMenuItem iToolStripMenuItem;
        private readonly ToolStripMenuItem yToolStripMenuItem;
        private readonly ToolStripMenuItem ɨToolStripMenuItem;
        private readonly ToolStripMenuItem ʉToolStripMenuItem;
        private readonly ToolStripMenuItem ɯToolStripMenuItem;
        private readonly ToolStripMenuItem uToolStripMenuItem;
        private readonly ToolStripMenuItem nearCloseToolStripMenuItem;
        private readonly ToolStripMenuItem ɪToolStripMenuItem;
        private readonly ToolStripMenuItem ʏToolStripMenuItem;
        private readonly ToolStripMenuItem ɪToolStripMenuItem1;
        private readonly ToolStripMenuItem ʊToolStripMenuItem;
        private readonly ToolStripMenuItem ʊToolStripMenuItem1;
        private readonly ToolStripMenuItem closeMidToolStripMenuItem;
        private readonly ToolStripMenuItem midToolStripMenuItem;
        private readonly ToolStripMenuItem eToolStripMenuItem;
        private readonly ToolStripMenuItem øToolStripMenuItem;
        private readonly ToolStripMenuItem əToolStripMenuItem;
        private readonly ToolStripMenuItem ɤToolStripMenuItem;
        private readonly ToolStripMenuItem oToolStripMenuItem;
        private readonly ToolStripMenuItem openMidToolStripMenuItem;
        private readonly ToolStripMenuItem nearOpenToolStripMenuItem;
        private readonly ToolStripMenuItem openToolStripMenuItem;
        private readonly ToolStripMenuItem eToolStripMenuItem1;
        private readonly ToolStripMenuItem øToolStripMenuItem1;
        private readonly ToolStripMenuItem ɘToolStripMenuItem;
        private readonly ToolStripMenuItem ɵToolStripMenuItem;
        private readonly ToolStripMenuItem ɤToolStripMenuItem1;
        private readonly ToolStripMenuItem oToolStripMenuItem1;
        private readonly ToolStripMenuItem ɛToolStripMenuItem;
        private readonly ToolStripMenuItem œToolStripMenuItem;
        private readonly ToolStripMenuItem ɜToolStripMenuItem;
        private readonly ToolStripMenuItem ɞToolStripMenuItem;
        private readonly ToolStripMenuItem ʌToolStripMenuItem;
        private readonly ToolStripMenuItem ɔToolStripMenuItem;
        private readonly ToolStripMenuItem æToolStripMenuItem;
        private readonly ToolStripMenuItem ɐToolStripMenuItem;
        private readonly ToolStripMenuItem aToolStripMenuItem;
        private readonly ToolStripMenuItem ɶToolStripMenuItem;
        private readonly ToolStripMenuItem äToolStripMenuItem;
        private readonly ToolStripMenuItem ɑToolStripMenuItem;
        private readonly ToolStripMenuItem ɒToolStripMenuItem;
        private readonly ToolStripMenuItem pToolStripMenuItem;
        private readonly ToolStripMenuItem bToolStripMenuItem;
        private readonly ToolStripMenuItem dToolStripMenuItem;
        private readonly ToolStripMenuItem tToolStripMenuItem;
        private readonly ToolStripMenuItem cToolStripMenuItem;
        private readonly ToolStripMenuItem kToolStripMenuItem;
        private readonly ToolStripMenuItem qToolStripMenuItem;
        private readonly ToolStripMenuItem mToolStripMenuItem;
        private readonly ToolStripMenuItem nToolStripMenuItem;
        private readonly ToolStripMenuItem lengthenedToolStripMenuItem;
        private readonly ToolStripMenuItem halfLengthenedToolStripMenuItem;
        private readonly ToolStripMenuItem shortenedToolStripMenuItem;
        private readonly ToolStripMenuItem rhoticityToolStripMenuItem;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

        private readonly List<ToolStripMenuItem> latinDiacriticToolStripSubMenus = [];

        /// <summary>
        /// Constructor for a CharacterInsertToolStripMenuItem
        /// </summary>
        public CharacterInsertToolStripMenuItem()
        {
            // Instantiate the menu SpeakingSpeeds.
            pulmonicConsonantsToolStripMenuItem = new ToolStripMenuItem();
            nonPulmonicConsonantsToolStripMenuItem = new ToolStripMenuItem();
            vowelsToolStripMenuItem = new ToolStripMenuItem();
            symbolsToolStripMenuItem = new ToolStripMenuItem();
            latinTextDiacriticsToolStripMenuItem = new ToolStripMenuItem();

            plosiveToolStripMenuItem = new ToolStripMenuItem();
            nasalToolStripMenuItem = new ToolStripMenuItem();
            trillStripMenuItem = new ToolStripMenuItem();
            tapOrFlapToolStripMenuItem = new ToolStripMenuItem();
            fricativeToolStripMenuItem = new ToolStripMenuItem();
            lateralFricativeToolStripMenuItem = new ToolStripMenuItem();
            approximateToolStripMenuItem = new ToolStripMenuItem();
            lateralApproximateToolStripMenuItem = new ToolStripMenuItem();

            clicksToolStripMenuItem = new ToolStripMenuItem();
            alveolarLateralClickToolStripMenuItem = new ToolStripMenuItem();
            voicedImplosivesToolStripMenuItem = new ToolStripMenuItem();
            otherConsonantsToolStripMenuItem = new ToolStripMenuItem();

            pToolStripMenuItem = new ToolStripMenuItem();
            bToolStripMenuItem = new ToolStripMenuItem();
            tToolStripMenuItem = new ToolStripMenuItem();
            dToolStripMenuItem = new ToolStripMenuItem();
            ʈToolStripMenuItem = new ToolStripMenuItem();
            ɖToolStripMenuItem = new ToolStripMenuItem();
            cToolStripMenuItem = new ToolStripMenuItem();
            ɟToolStripMenuItem = new ToolStripMenuItem();
            kToolStripMenuItem = new ToolStripMenuItem();
            qToolStripMenuItem = new ToolStripMenuItem();
            ɡToolStripMenuItem = new ToolStripMenuItem();
            ɢToolStripMenuItem = new ToolStripMenuItem();
            ʔToolStripMenuItem = new ToolStripMenuItem();
            mToolStripMenuItem = new ToolStripMenuItem();
            ɱToolStripMenuItem = new ToolStripMenuItem();
            nToolStripMenuItem = new ToolStripMenuItem();
            ɳToolStripMenuItem = new ToolStripMenuItem();
            ɲToolStripMenuItem = new ToolStripMenuItem();
            ŋToolStripMenuItem = new ToolStripMenuItem();
            ɴToolStripMenuItem = new ToolStripMenuItem();
            ʙToolStripMenuItem = new ToolStripMenuItem();
            rToolStripMenuItem = new ToolStripMenuItem();
            ʀToolStripMenuItem = new ToolStripMenuItem();
            ⱱToolStripMenuItem = new ToolStripMenuItem();
            ɾToolStripMenuItem = new ToolStripMenuItem();
            ɽToolStripMenuItem = new ToolStripMenuItem();
            ɸToolStripMenuItem = new ToolStripMenuItem();
            βToolStripMenuItem = new ToolStripMenuItem();
            fToolStripMenuItem = new ToolStripMenuItem();
            vToolStripMenuItem = new ToolStripMenuItem();
            θToolStripMenuItem = new ToolStripMenuItem();
            ðToolStripMenuItem = new ToolStripMenuItem();
            sToolStripMenuItem = new ToolStripMenuItem();
            zToolStripMenuItem = new ToolStripMenuItem();
            ʃToolStripMenuItem = new ToolStripMenuItem();
            ʒToolStripMenuItem = new ToolStripMenuItem();
            ʂToolStripMenuItem = new ToolStripMenuItem();
            ʂToolStripMenuItem1 = new ToolStripMenuItem();
            çToolStripMenuItem = new ToolStripMenuItem();
            ʝToolStripMenuItem = new ToolStripMenuItem();
            xToolStripMenuItem = new ToolStripMenuItem();
            ɣToolStripMenuItem = new ToolStripMenuItem();
            χToolStripMenuItem = new ToolStripMenuItem();
            ʁToolStripMenuItem = new ToolStripMenuItem();
            ħToolStripMenuItem = new ToolStripMenuItem();
            ʕToolStripMenuItem = new ToolStripMenuItem();
            ʜToolStripMenuItem = new ToolStripMenuItem();
            ʢToolStripMenuItem = new ToolStripMenuItem();
            ɦToolStripMenuItem = new ToolStripMenuItem();
            ɬToolStripMenuItem = new ToolStripMenuItem();
            ɮToolStripMenuItem = new ToolStripMenuItem();
            ʋToolStripMenuItem = new ToolStripMenuItem();
            ɹToolStripMenuItem = new ToolStripMenuItem();
            ɻToolStripMenuItem = new ToolStripMenuItem();
            ɰToolStripMenuItem = new ToolStripMenuItem();
            ɭToolStripMenuItem = new ToolStripMenuItem();
            ʎToolStripMenuItem = new ToolStripMenuItem();
            ʟToolStripMenuItem = new ToolStripMenuItem();
            ʘToolStripMenuItem = new ToolStripMenuItem();
            ǀDentalClickToolStripMenuItem = new ToolStripMenuItem();
            ǃAlveolarClickToolStripMenuItem = new ToolStripMenuItem();
            ǂClickToolStripMenuItem = new ToolStripMenuItem();
            ɓBilabialVoicedImplosiveToolStripMenuItem = new ToolStripMenuItem();
            ɗAlveolarVoicedImplosiveToolStripMenuItem = new ToolStripMenuItem();
            ʄPalatalVoicedImplosiveToolStripMenuItem = new ToolStripMenuItem();
            ɠVelarVoicedImplosiveToolStripMenuItem = new ToolStripMenuItem();
            ʛUvularVoicedImplosiveToolStripMenuItem = new ToolStripMenuItem();
            ʍVoicelessLabialVelarApproximateToolStripMenuItem = new ToolStripMenuItem();
            ɥVoicedLabialPalatalApproximateToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            iToolStripMenuItem = new ToolStripMenuItem();
            yToolStripMenuItem = new ToolStripMenuItem();
            ɨToolStripMenuItem = new ToolStripMenuItem();
            ʉToolStripMenuItem = new ToolStripMenuItem();
            ɯToolStripMenuItem = new ToolStripMenuItem();
            uToolStripMenuItem = new ToolStripMenuItem();
            nearCloseToolStripMenuItem = new ToolStripMenuItem();
            ɪToolStripMenuItem = new ToolStripMenuItem();
            ʏToolStripMenuItem = new ToolStripMenuItem();
            ɪToolStripMenuItem1 = new ToolStripMenuItem();
            ʊToolStripMenuItem = new ToolStripMenuItem();
            ʊToolStripMenuItem1 = new ToolStripMenuItem();
            closeMidToolStripMenuItem = new ToolStripMenuItem();
            eToolStripMenuItem1 = new ToolStripMenuItem();
            øToolStripMenuItem1 = new ToolStripMenuItem();
            ɘToolStripMenuItem = new ToolStripMenuItem();
            ɵToolStripMenuItem = new ToolStripMenuItem();
            ɤToolStripMenuItem1 = new ToolStripMenuItem();
            oToolStripMenuItem1 = new ToolStripMenuItem();
            midToolStripMenuItem = new ToolStripMenuItem();
            eToolStripMenuItem = new ToolStripMenuItem();
            øToolStripMenuItem = new ToolStripMenuItem();
            əToolStripMenuItem = new ToolStripMenuItem();
            ɤToolStripMenuItem = new ToolStripMenuItem();
            oToolStripMenuItem = new ToolStripMenuItem();
            openMidToolStripMenuItem = new ToolStripMenuItem();
            ɛToolStripMenuItem = new ToolStripMenuItem();
            œToolStripMenuItem = new ToolStripMenuItem();
            ɜToolStripMenuItem = new ToolStripMenuItem();
            ɞToolStripMenuItem = new ToolStripMenuItem();
            ʌToolStripMenuItem = new ToolStripMenuItem();
            ɔToolStripMenuItem = new ToolStripMenuItem();
            nearOpenToolStripMenuItem = new ToolStripMenuItem();
            æToolStripMenuItem = new ToolStripMenuItem();
            ɐToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            aToolStripMenuItem = new ToolStripMenuItem();
            ɶToolStripMenuItem = new ToolStripMenuItem();
            äToolStripMenuItem = new ToolStripMenuItem();
            ɑToolStripMenuItem = new ToolStripMenuItem();
            ɒToolStripMenuItem = new ToolStripMenuItem();
            lengthenedToolStripMenuItem = new ToolStripMenuItem();
            halfLengthenedToolStripMenuItem = new ToolStripMenuItem();
            shortenedToolStripMenuItem = new ToolStripMenuItem();
            rhoticityToolStripMenuItem = new ToolStripMenuItem();

            // Start building the menus.  (Code initially copied from designer code then cleaned up)
            // 
            // pulmonicConsonantsToolStripMenuItem
            // 
            pulmonicConsonantsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { plosiveToolStripMenuItem, nasalToolStripMenuItem, trillStripMenuItem, tapOrFlapToolStripMenuItem, fricativeToolStripMenuItem, lateralFricativeToolStripMenuItem, approximateToolStripMenuItem, lateralApproximateToolStripMenuItem });
            pulmonicConsonantsToolStripMenuItem.Name = "pulmonicConsonantsToolStripMenuItem";
            pulmonicConsonantsToolStripMenuItem.Size = new Size(211, 22);
            pulmonicConsonantsToolStripMenuItem.Text = "IPA Pulmonic Consonants";
            // 
            // plosiveToolStripMenuItem
            // 
            plosiveToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pToolStripMenuItem, bToolStripMenuItem, tToolStripMenuItem, dToolStripMenuItem, ʈToolStripMenuItem, ɖToolStripMenuItem, cToolStripMenuItem, ɟToolStripMenuItem, kToolStripMenuItem, qToolStripMenuItem, ɡToolStripMenuItem, ɢToolStripMenuItem, ʔToolStripMenuItem });
            plosiveToolStripMenuItem.Name = "plosiveToolStripMenuItem";
            plosiveToolStripMenuItem.Size = new Size(181, 22);
            plosiveToolStripMenuItem.Text = "Plosive";
            // 
            // pToolStripMenuItem
            // 
            pToolStripMenuItem.Name = "pToolStripMenuItem";
            pToolStripMenuItem.Size = new Size(81, 22);
            pToolStripMenuItem.Text = "p";
            // 
            // bToolStripMenuItem
            // 
            bToolStripMenuItem.Name = "bToolStripMenuItem";
            bToolStripMenuItem.Size = new Size(81, 22);
            bToolStripMenuItem.Text = "b";
            // 
            // tToolStripMenuItem
            // 
            tToolStripMenuItem.Name = "tToolStripMenuItem";
            tToolStripMenuItem.Size = new Size(81, 22);
            tToolStripMenuItem.Text = "t";
            // 
            // dToolStripMenuItem
            // 
            dToolStripMenuItem.Name = "dToolStripMenuItem";
            dToolStripMenuItem.Size = new Size(81, 22);
            dToolStripMenuItem.Text = "d";
            // 
            // ʈToolStripMenuItem
            // 
            ʈToolStripMenuItem.Name = "ʈToolStripMenuItem";
            ʈToolStripMenuItem.Size = new Size(81, 22);
            ʈToolStripMenuItem.Text = "ʈ";
            // 
            // ɖToolStripMenuItem
            // 
            ɖToolStripMenuItem.Name = "ɖToolStripMenuItem";
            ɖToolStripMenuItem.Size = new Size(81, 22);
            ɖToolStripMenuItem.Text = "ɖ";
            // 
            // cToolStripMenuItem
            // 
            cToolStripMenuItem.Name = "cToolStripMenuItem";
            cToolStripMenuItem.Size = new Size(81, 22);
            cToolStripMenuItem.Text = "c";
            // 
            // ɟToolStripMenuItem
            // 
            ɟToolStripMenuItem.Name = "ɟToolStripMenuItem";
            ɟToolStripMenuItem.Size = new Size(81, 22);
            ɟToolStripMenuItem.Text = "ɟ";
            // 
            // kToolStripMenuItem
            // 
            kToolStripMenuItem.Name = "kToolStripMenuItem";
            kToolStripMenuItem.Size = new Size(81, 22);
            kToolStripMenuItem.Text = "k";
            // 
            // qToolStripMenuItem
            // 
            qToolStripMenuItem.Name = "qToolStripMenuItem";
            qToolStripMenuItem.Size = new Size(81, 22);
            qToolStripMenuItem.Text = "q";
            // 
            // ɡToolStripMenuItem
            // 
            ɡToolStripMenuItem.Name = "ɡToolStripMenuItem";
            ɡToolStripMenuItem.Size = new Size(81, 22);
            ɡToolStripMenuItem.Text = "ɡ";
            // 
            // ɢToolStripMenuItem
            // 
            ɢToolStripMenuItem.Name = "ɢToolStripMenuItem";
            ɢToolStripMenuItem.Size = new Size(81, 22);
            ɢToolStripMenuItem.Text = "ɢ";
            // 
            // ʔToolStripMenuItem
            // 
            ʔToolStripMenuItem.Name = "ʔToolStripMenuItem";
            ʔToolStripMenuItem.Size = new Size(81, 22);
            ʔToolStripMenuItem.Text = "ʔ";
            // 
            // nasalToolStripMenuItem
            // 
            nasalToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mToolStripMenuItem, ɱToolStripMenuItem, nToolStripMenuItem, ɳToolStripMenuItem, ɲToolStripMenuItem, ŋToolStripMenuItem, ɴToolStripMenuItem });
            nasalToolStripMenuItem.Name = "nasalToolStripMenuItem";
            nasalToolStripMenuItem.Size = new Size(181, 22);
            nasalToolStripMenuItem.Text = "Nasal";
            // 
            // mToolStripMenuItem
            // 
            mToolStripMenuItem.Name = "mToolStripMenuItem";
            mToolStripMenuItem.Size = new Size(85, 22);
            mToolStripMenuItem.Text = "m";
            // 
            // ɱToolStripMenuItem
            // 
            ɱToolStripMenuItem.Name = "ɱToolStripMenuItem";
            ɱToolStripMenuItem.Size = new Size(85, 22);
            ɱToolStripMenuItem.Text = "ɱ";
            // 
            // nToolStripMenuItem
            // 
            nToolStripMenuItem.Name = "nToolStripMenuItem";
            nToolStripMenuItem.Size = new Size(85, 22);
            nToolStripMenuItem.Text = "n";
            // 
            // ɳToolStripMenuItem
            // 
            ɳToolStripMenuItem.Name = "ɳToolStripMenuItem";
            ɳToolStripMenuItem.Size = new Size(85, 22);
            ɳToolStripMenuItem.Text = "ɳ";
            // 
            // ɲToolStripMenuItem
            // 
            ɲToolStripMenuItem.Name = "ɲToolStripMenuItem";
            ɲToolStripMenuItem.Size = new Size(85, 22);
            ɲToolStripMenuItem.Text = "ɲ";
            // 
            // ŋToolStripMenuItem
            // 
            ŋToolStripMenuItem.Name = "ŋToolStripMenuItem";
            ŋToolStripMenuItem.Size = new Size(85, 22);
            ŋToolStripMenuItem.Text = "ŋ";
            // 
            // ɴToolStripMenuItem
            // 
            ɴToolStripMenuItem.Name = "ɴToolStripMenuItem";
            ɴToolStripMenuItem.Size = new Size(85, 22);
            ɴToolStripMenuItem.Text = "ɴ";
            // 
            // trillStripMenuItem
            // 
            trillStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ʙToolStripMenuItem, rToolStripMenuItem, ʀToolStripMenuItem });
            trillStripMenuItem.Name = "trillStripMenuItem";
            trillStripMenuItem.Size = new Size(181, 22);
            trillStripMenuItem.Text = "Trill";
            // 
            // ʙToolStripMenuItem
            // 
            ʙToolStripMenuItem.Name = "ʙToolStripMenuItem";
            ʙToolStripMenuItem.Size = new Size(80, 22);
            ʙToolStripMenuItem.Text = "ʙ";
            // 
            // rToolStripMenuItem
            // 
            rToolStripMenuItem.Name = "rToolStripMenuItem";
            rToolStripMenuItem.Size = new Size(80, 22);
            rToolStripMenuItem.Text = "r";
            // 
            // ʀToolStripMenuItem
            // 
            ʀToolStripMenuItem.Name = "ʀToolStripMenuItem";
            ʀToolStripMenuItem.Size = new Size(80, 22);
            ʀToolStripMenuItem.Text = "ʀ";
            // 
            // tapOrFlapToolStripMenuItem
            // 
            tapOrFlapToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ⱱToolStripMenuItem, ɾToolStripMenuItem, ɽToolStripMenuItem });
            tapOrFlapToolStripMenuItem.Name = "tapOrFlapToolStripMenuItem";
            tapOrFlapToolStripMenuItem.Size = new Size(181, 22);
            tapOrFlapToolStripMenuItem.Text = "Tap or Flap";
            // 
            // ⱱToolStripMenuItem
            // 
            ⱱToolStripMenuItem.Name = "ⱱToolStripMenuItem";
            ⱱToolStripMenuItem.Size = new Size(81, 22);
            ⱱToolStripMenuItem.Text = "ⱱ";
            // 
            // ɾToolStripMenuItem
            // 
            ɾToolStripMenuItem.Name = "ɾToolStripMenuItem";
            ɾToolStripMenuItem.Size = new Size(81, 22);
            ɾToolStripMenuItem.Text = "ɾ";
            // 
            // ɽToolStripMenuItem
            // 
            ɽToolStripMenuItem.Name = "ɽToolStripMenuItem";
            ɽToolStripMenuItem.Size = new Size(81, 22);
            ɽToolStripMenuItem.Text = "ɽ";
            // 
            // fricativeToolStripMenuItem
            // 
            fricativeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ɸToolStripMenuItem, βToolStripMenuItem, fToolStripMenuItem, vToolStripMenuItem, θToolStripMenuItem, ðToolStripMenuItem, sToolStripMenuItem, zToolStripMenuItem, ʃToolStripMenuItem, ʒToolStripMenuItem, ʂToolStripMenuItem, ʂToolStripMenuItem1, çToolStripMenuItem, ʝToolStripMenuItem, xToolStripMenuItem, ɣToolStripMenuItem, χToolStripMenuItem, ʁToolStripMenuItem, ħToolStripMenuItem, ʕToolStripMenuItem, ʜToolStripMenuItem, ʢToolStripMenuItem, ɦToolStripMenuItem });
            fricativeToolStripMenuItem.Name = "fricativeToolStripMenuItem";
            fricativeToolStripMenuItem.Size = new Size(181, 22);
            fricativeToolStripMenuItem.Text = "Fricative";
            // 
            // ɸToolStripMenuItem
            // 
            ɸToolStripMenuItem.Name = "ɸToolStripMenuItem";
            ɸToolStripMenuItem.Size = new Size(82, 22);
            ɸToolStripMenuItem.Text = "ɸ";
            // 
            // βToolStripMenuItem
            // 
            βToolStripMenuItem.Name = "βToolStripMenuItem";
            βToolStripMenuItem.Size = new Size(82, 22);
            βToolStripMenuItem.Text = "β";
            // 
            // fToolStripMenuItem
            // 
            fToolStripMenuItem.Name = "fToolStripMenuItem";
            fToolStripMenuItem.Size = new Size(82, 22);
            fToolStripMenuItem.Text = "f";
            // 
            // vToolStripMenuItem
            // 
            vToolStripMenuItem.Name = "vToolStripMenuItem";
            vToolStripMenuItem.Size = new Size(82, 22);
            vToolStripMenuItem.Text = "v";
            // 
            // θToolStripMenuItem
            // 
            θToolStripMenuItem.Name = "θToolStripMenuItem";
            θToolStripMenuItem.Size = new Size(82, 22);
            θToolStripMenuItem.Text = "θ";
            // 
            // ðToolStripMenuItem
            // 
            ðToolStripMenuItem.Name = "ðToolStripMenuItem";
            ðToolStripMenuItem.Size = new Size(82, 22);
            ðToolStripMenuItem.Text = "ð";
            // 
            // sToolStripMenuItem
            // 
            sToolStripMenuItem.Name = "sToolStripMenuItem";
            sToolStripMenuItem.Size = new Size(82, 22);
            sToolStripMenuItem.Text = "s";
            // 
            // zToolStripMenuItem
            // 
            zToolStripMenuItem.Name = "zToolStripMenuItem";
            zToolStripMenuItem.Size = new Size(82, 22);
            zToolStripMenuItem.Text = "z";
            // 
            // ʃToolStripMenuItem
            // 
            ʃToolStripMenuItem.Name = "ʃToolStripMenuItem";
            ʃToolStripMenuItem.Size = new Size(82, 22);
            ʃToolStripMenuItem.Text = "ʃ";
            // 
            // ʒToolStripMenuItem
            // 
            ʒToolStripMenuItem.Name = "ʒToolStripMenuItem";
            ʒToolStripMenuItem.Size = new Size(82, 22);
            ʒToolStripMenuItem.Text = "ʒ";
            // 
            // ʂToolStripMenuItem
            // 
            ʂToolStripMenuItem.Name = "ʂToolStripMenuItem";
            ʂToolStripMenuItem.Size = new Size(82, 22);
            ʂToolStripMenuItem.Text = "ʂ";
            // 
            // ʂToolStripMenuItem1
            // 
            ʂToolStripMenuItem1.Name = "ʂToolStripMenuItem1";
            ʂToolStripMenuItem1.Size = new Size(82, 22);
            ʂToolStripMenuItem1.Text = "ʂ";
            // 
            // çToolStripMenuItem
            // 
            çToolStripMenuItem.Name = "çToolStripMenuItem";
            çToolStripMenuItem.Size = new Size(82, 22);
            çToolStripMenuItem.Text = "ç";
            // 
            // ʝToolStripMenuItem
            // 
            ʝToolStripMenuItem.Name = "ʝToolStripMenuItem";
            ʝToolStripMenuItem.Size = new Size(82, 22);
            ʝToolStripMenuItem.Text = "ʝ";
            // 
            // xToolStripMenuItem
            // 
            xToolStripMenuItem.Name = "xToolStripMenuItem";
            xToolStripMenuItem.Size = new Size(82, 22);
            xToolStripMenuItem.Text = "x";
            // 
            // ɣToolStripMenuItem
            // 
            ɣToolStripMenuItem.Name = "ɣToolStripMenuItem";
            ɣToolStripMenuItem.Size = new Size(82, 22);
            ɣToolStripMenuItem.Text = "ɣ";
            // 
            // χToolStripMenuItem
            // 
            χToolStripMenuItem.Name = "χToolStripMenuItem";
            χToolStripMenuItem.Size = new Size(82, 22);
            χToolStripMenuItem.Text = "χ";
            // 
            // ʁToolStripMenuItem
            // 
            ʁToolStripMenuItem.Name = "ʁToolStripMenuItem";
            ʁToolStripMenuItem.Size = new Size(82, 22);
            ʁToolStripMenuItem.Text = "ʁ";
            // 
            // ħToolStripMenuItem
            // 
            ħToolStripMenuItem.Name = "ħToolStripMenuItem";
            ħToolStripMenuItem.Size = new Size(82, 22);
            ħToolStripMenuItem.Text = "ħ";
            // 
            // ʕToolStripMenuItem
            // 
            ʕToolStripMenuItem.Name = "ʕToolStripMenuItem";
            ʕToolStripMenuItem.Size = new Size(82, 22);
            ʕToolStripMenuItem.Text = "ʕ";
            // 
            // ʜToolStripMenuItem
            // 
            ʜToolStripMenuItem.Name = "ʜToolStripMenuItem";
            ʜToolStripMenuItem.Size = new Size(82, 22);
            ʜToolStripMenuItem.Text = "ʜ";
            // 
            // ʢToolStripMenuItem
            // 
            ʢToolStripMenuItem.Name = "ʢToolStripMenuItem";
            ʢToolStripMenuItem.Size = new Size(82, 22);
            ʢToolStripMenuItem.Text = "ʢ";
            // 
            // ɦToolStripMenuItem
            // 
            ɦToolStripMenuItem.Name = "ɦToolStripMenuItem";
            ɦToolStripMenuItem.Size = new Size(82, 22);
            ɦToolStripMenuItem.Text = "ɦ";
            // 
            // lateralFricativeToolStripMenuItem
            // 
            lateralFricativeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ɬToolStripMenuItem, ɮToolStripMenuItem });
            lateralFricativeToolStripMenuItem.Name = "lateralFricativeToolStripMenuItem";
            lateralFricativeToolStripMenuItem.Size = new Size(181, 22);
            lateralFricativeToolStripMenuItem.Text = "Lateral Fricative";
            // 
            // ɬToolStripMenuItem
            // 
            ɬToolStripMenuItem.Name = "ɬToolStripMenuItem";
            ɬToolStripMenuItem.Size = new Size(81, 22);
            ɬToolStripMenuItem.Text = "ɬ";
            // 
            // ɮToolStripMenuItem
            // 
            ɮToolStripMenuItem.Name = "ɮToolStripMenuItem";
            ɮToolStripMenuItem.Size = new Size(81, 22);
            ɮToolStripMenuItem.Text = "ɮ";
            // 
            // approximateToolStripMenuItem
            // 
            approximateToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ʋToolStripMenuItem, ɹToolStripMenuItem, ɻToolStripMenuItem, ɰToolStripMenuItem });
            approximateToolStripMenuItem.Name = "approximateToolStripMenuItem";
            approximateToolStripMenuItem.Size = new Size(181, 22);
            approximateToolStripMenuItem.Text = "Approximate";
            // 
            // ʋToolStripMenuItem
            // 
            ʋToolStripMenuItem.Name = "ʋToolStripMenuItem";
            ʋToolStripMenuItem.Size = new Size(84, 22);
            ʋToolStripMenuItem.Text = "ʋ";
            // 
            // ɹToolStripMenuItem
            // 
            ɹToolStripMenuItem.Name = "ɹToolStripMenuItem";
            ɹToolStripMenuItem.Size = new Size(84, 22);
            ɹToolStripMenuItem.Text = "ɹ";
            // 
            // ɻToolStripMenuItem
            // 
            ɻToolStripMenuItem.Name = "ɻToolStripMenuItem";
            ɻToolStripMenuItem.Size = new Size(84, 22);
            ɻToolStripMenuItem.Text = "ɻ";
            // 
            // ɰToolStripMenuItem
            // 
            ɰToolStripMenuItem.Name = "ɰToolStripMenuItem";
            ɰToolStripMenuItem.Size = new Size(84, 22);
            ɰToolStripMenuItem.Text = "ɰ";
            // 
            // lateralApproximateToolStripMenuItem
            // 
            lateralApproximateToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ɭToolStripMenuItem, ʎToolStripMenuItem, ʟToolStripMenuItem });
            lateralApproximateToolStripMenuItem.Name = "lateralApproximateToolStripMenuItem";
            lateralApproximateToolStripMenuItem.Size = new Size(181, 22);
            lateralApproximateToolStripMenuItem.Text = "Lateral Approximate";
            // 
            // ɭToolStripMenuItem
            // 
            ɭToolStripMenuItem.Name = "ɭToolStripMenuItem";
            ɭToolStripMenuItem.Size = new Size(80, 22);
            ɭToolStripMenuItem.Text = "ɭ";
            // 
            // ʎToolStripMenuItem
            // 
            ʎToolStripMenuItem.Name = "ʎToolStripMenuItem";
            ʎToolStripMenuItem.Size = new Size(80, 22);
            ʎToolStripMenuItem.Text = "ʎ";
            // 
            // ʟToolStripMenuItem
            // 
            ʟToolStripMenuItem.Name = "ʟToolStripMenuItem";
            ʟToolStripMenuItem.Size = new Size(80, 22);
            ʟToolStripMenuItem.Text = "ʟ";
            // 
            // nonPulmonicConsonantsToolStripMenuItem
            // 
            nonPulmonicConsonantsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clicksToolStripMenuItem, voicedImplosivesToolStripMenuItem, otherConsonantsToolStripMenuItem });
            nonPulmonicConsonantsToolStripMenuItem.Name = "nonPulmonicConsonantsToolStripMenuItem";
            nonPulmonicConsonantsToolStripMenuItem.Size = new Size(211, 22);
            nonPulmonicConsonantsToolStripMenuItem.Text = "IPA Other Consonants";
            // 
            // clicksToolStripMenuItem
            // 
            clicksToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ʘToolStripMenuItem, ǀDentalClickToolStripMenuItem, ǃAlveolarClickToolStripMenuItem, ǂClickToolStripMenuItem, alveolarLateralClickToolStripMenuItem });
            clicksToolStripMenuItem.Name = "clicksToolStripMenuItem";
            clicksToolStripMenuItem.Size = new Size(170, 22);
            clicksToolStripMenuItem.Text = "Clicks";
            // 
            // ʘToolStripMenuItem
            // 
            ʘToolStripMenuItem.Name = "ʘToolStripMenuItem";
            ʘToolStripMenuItem.Size = new Size(200, 22);
            ʘToolStripMenuItem.Text = "ʘ - Bilabial Click";
            // 
            // ǀDentalClickToolStripMenuItem
            // 
            ǀDentalClickToolStripMenuItem.Name = "ǀDentalClickToolStripMenuItem";
            ǀDentalClickToolStripMenuItem.Size = new Size(200, 22);
            ǀDentalClickToolStripMenuItem.Text = "ǀ - Dental Click";
            // 
            // ǃAlveolarClickToolStripMenuItem
            // 
            ǃAlveolarClickToolStripMenuItem.Name = "ǃAlveolarClickToolStripMenuItem";
            ǃAlveolarClickToolStripMenuItem.Size = new Size(200, 22);
            ǃAlveolarClickToolStripMenuItem.Text = "ǃ - Alveolar Click";
            // 
            // ǂClickToolStripMenuItem
            // 
            ǂClickToolStripMenuItem.Name = "ǂClickToolStripMenuItem";
            ǂClickToolStripMenuItem.Size = new Size(200, 22);
            ǂClickToolStripMenuItem.Text = " ǂ - PalatoAlveolar Click";
            // 
            // alveolarLateralClickToolStripMenuItem
            // 
            alveolarLateralClickToolStripMenuItem.Name = "alveolarLateralClickToolStripMenuItem";
            alveolarLateralClickToolStripMenuItem.Size = new Size(200, 22);
            alveolarLateralClickToolStripMenuItem.Text = "ǁ - Alveolar Lateral Click";
            // 
            // voicedImplosivesToolStripMenuItem
            // 
            voicedImplosivesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ɓBilabialVoicedImplosiveToolStripMenuItem, ɗAlveolarVoicedImplosiveToolStripMenuItem, ʄPalatalVoicedImplosiveToolStripMenuItem, ɠVelarVoicedImplosiveToolStripMenuItem, ʛUvularVoicedImplosiveToolStripMenuItem });
            voicedImplosivesToolStripMenuItem.Name = "voicedImplosivesToolStripMenuItem";
            voicedImplosivesToolStripMenuItem.Size = new Size(170, 22);
            voicedImplosivesToolStripMenuItem.Text = "Voiced Implosives";
            // 
            // ɓBilabialVoicedImplosiveToolStripMenuItem
            // 
            ɓBilabialVoicedImplosiveToolStripMenuItem.Name = "ɓBilabialVoicedImplosiveToolStripMenuItem";
            ɓBilabialVoicedImplosiveToolStripMenuItem.Size = new Size(227, 22);
            ɓBilabialVoicedImplosiveToolStripMenuItem.Text = "ɓ - Bilabial Voiced Implosive";
            // 
            // ɗAlveolarVoicedImplosiveToolStripMenuItem
            // 
            ɗAlveolarVoicedImplosiveToolStripMenuItem.Name = "ɗAlveolarVoicedImplosiveToolStripMenuItem";
            ɗAlveolarVoicedImplosiveToolStripMenuItem.Size = new Size(227, 22);
            ɗAlveolarVoicedImplosiveToolStripMenuItem.Text = "ɗ - Alveolar Voiced Implosive";
            // 
            // ʄPalatalVoicedImplosiveToolStripMenuItem
            // 
            ʄPalatalVoicedImplosiveToolStripMenuItem.Name = "ʄPalatalVoicedImplosiveToolStripMenuItem";
            ʄPalatalVoicedImplosiveToolStripMenuItem.Size = new Size(227, 22);
            ʄPalatalVoicedImplosiveToolStripMenuItem.Text = "ʄ - Palatal Voiced Implosive";
            // 
            // ɠVelarVoicedImplosiveToolStripMenuItem
            // 
            ɠVelarVoicedImplosiveToolStripMenuItem.Name = "ɠValarVoicedImplosiveToolStripMenuItem";
            ɠVelarVoicedImplosiveToolStripMenuItem.Size = new Size(227, 22);
            ɠVelarVoicedImplosiveToolStripMenuItem.Text = "ɠ - Valar Voiced Implosive";
            // 
            // ʛUvularVoicedImplosiveToolStripMenuItem
            // 
            ʛUvularVoicedImplosiveToolStripMenuItem.Name = "ʛUvularVoicedImplosiveToolStripMenuItem";
            ʛUvularVoicedImplosiveToolStripMenuItem.Size = new Size(227, 22);
            ʛUvularVoicedImplosiveToolStripMenuItem.Text = "ʛ - Uvular Voiced Implosive";
            // 
            // otherConsonantsToolStripMenuItem
            // 
            otherConsonantsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ʍVoicelessLabialVelarApproximateToolStripMenuItem, ɥVoicedLabialPalatalApproximateToolStripMenuItem });
            otherConsonantsToolStripMenuItem.Name = "otherConsonantsToolStripMenuItem";
            otherConsonantsToolStripMenuItem.Size = new Size(170, 22);
            otherConsonantsToolStripMenuItem.Text = "Other Consonants";
            // 
            // ʍVoicelessLabialVelarApproximateToolStripMenuItem
            // 
            ʍVoicelessLabialVelarApproximateToolStripMenuItem.Name = "ʍVoicelessLabialVelarApproximateToolStripMenuItem";
            ʍVoicelessLabialVelarApproximateToolStripMenuItem.Size = new Size(277, 22);
            ʍVoicelessLabialVelarApproximateToolStripMenuItem.Text = "ʍ - Voiceless Labial-velar Approximate";
            // 
            // ɥVoicedLabialPalatalApproximateToolStripMenuItem
            // 
            ɥVoicedLabialPalatalApproximateToolStripMenuItem.Name = "ɥVoicedLabialPalatalApproximateToolStripMenuItem";
            ɥVoicedLabialPalatalApproximateToolStripMenuItem.Size = new Size(277, 22);
            ɥVoicedLabialPalatalApproximateToolStripMenuItem.Text = "ɥ - Voiced Labial-Palatal Approximate";
            // 
            // vowelsToolStripMenuItem
            // 
            vowelsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { closeToolStripMenuItem, nearCloseToolStripMenuItem, closeMidToolStripMenuItem, midToolStripMenuItem, openMidToolStripMenuItem, nearOpenToolStripMenuItem, openToolStripMenuItem });
            vowelsToolStripMenuItem.Name = "vowelsToolStripMenuItem";
            vowelsToolStripMenuItem.Size = new Size(211, 22);
            vowelsToolStripMenuItem.Text = "IPA Vowels";
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { iToolStripMenuItem, yToolStripMenuItem, ɨToolStripMenuItem, ʉToolStripMenuItem, ɯToolStripMenuItem, uToolStripMenuItem });
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(131, 22);
            closeToolStripMenuItem.Text = "Close";
            // 
            // iToolStripMenuItem
            // 
            iToolStripMenuItem.Name = "iToolStripMenuItem";
            iToolStripMenuItem.Size = new Size(84, 22);
            iToolStripMenuItem.Text = "i";
            // 
            // yToolStripMenuItem
            // 
            yToolStripMenuItem.Name = "yToolStripMenuItem";
            yToolStripMenuItem.Size = new Size(84, 22);
            yToolStripMenuItem.Text = "y";
            // 
            // ɨToolStripMenuItem
            // 
            ɨToolStripMenuItem.Name = "ɨToolStripMenuItem";
            ɨToolStripMenuItem.Size = new Size(84, 22);
            ɨToolStripMenuItem.Text = "ɨ";
            // 
            // ʉToolStripMenuItem
            // 
            ʉToolStripMenuItem.Name = "ʉToolStripMenuItem";
            ʉToolStripMenuItem.Size = new Size(84, 22);
            ʉToolStripMenuItem.Text = "ʉ";
            // 
            // ɯToolStripMenuItem
            // 
            ɯToolStripMenuItem.Name = "ɯToolStripMenuItem";
            ɯToolStripMenuItem.Size = new Size(84, 22);
            ɯToolStripMenuItem.Text = "ɯ";
            // 
            // uToolStripMenuItem
            // 
            uToolStripMenuItem.Name = "uToolStripMenuItem";
            uToolStripMenuItem.Size = new Size(84, 22);
            uToolStripMenuItem.Text = "u";
            // 
            // nearCloseToolStripMenuItem
            // 
            nearCloseToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ɪToolStripMenuItem, ʏToolStripMenuItem, ɪToolStripMenuItem1, ʊToolStripMenuItem, ʊToolStripMenuItem1 });
            nearCloseToolStripMenuItem.Name = "nearCloseToolStripMenuItem";
            nearCloseToolStripMenuItem.Size = new Size(131, 22);
            nearCloseToolStripMenuItem.Text = "Near-close";
            // 
            // ɪToolStripMenuItem
            // 
            ɪToolStripMenuItem.Name = "ɪToolStripMenuItem";
            ɪToolStripMenuItem.Size = new Size(82, 22);
            ɪToolStripMenuItem.Text = "ɪ";
            // 
            // ʏToolStripMenuItem
            // 
            ʏToolStripMenuItem.Name = "ʏToolStripMenuItem";
            ʏToolStripMenuItem.Size = new Size(82, 22);
            ʏToolStripMenuItem.Text = " ʏ";
            // 
            // ɪToolStripMenuItem1
            // 
            ɪToolStripMenuItem1.Name = "ɪToolStripMenuItem1";
            ɪToolStripMenuItem1.Size = new Size(82, 22);
            ɪToolStripMenuItem1.Text = "ɪ̈";
            // 
            // ʊToolStripMenuItem
            // 
            ʊToolStripMenuItem.Name = "ʊToolStripMenuItem";
            ʊToolStripMenuItem.Size = new Size(82, 22);
            ʊToolStripMenuItem.Text = "ʊ̈";
            // 
            // ʊToolStripMenuItem1
            // 
            ʊToolStripMenuItem1.Name = "ʊToolStripMenuItem1";
            ʊToolStripMenuItem1.Size = new Size(82, 22);
            ʊToolStripMenuItem1.Text = "ʊ";
            // 
            // closeMidToolStripMenuItem
            // 
            closeMidToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { eToolStripMenuItem1, øToolStripMenuItem1, ɘToolStripMenuItem, ɵToolStripMenuItem, ɤToolStripMenuItem1, oToolStripMenuItem1 });
            closeMidToolStripMenuItem.Name = "closeMidToolStripMenuItem";
            closeMidToolStripMenuItem.Size = new Size(131, 22);
            closeMidToolStripMenuItem.Text = "Close-mid";
            // 
            // eToolStripMenuItem1
            // 
            eToolStripMenuItem1.Name = "eToolStripMenuItem1";
            eToolStripMenuItem1.Size = new Size(81, 22);
            eToolStripMenuItem1.Text = "e";
            // 
            // øToolStripMenuItem1
            // 
            øToolStripMenuItem1.Name = "øToolStripMenuItem1";
            øToolStripMenuItem1.Size = new Size(81, 22);
            øToolStripMenuItem1.Text = "ø";
            // 
            // ɘToolStripMenuItem
            // 
            ɘToolStripMenuItem.Name = "ɘToolStripMenuItem";
            ɘToolStripMenuItem.Size = new Size(81, 22);
            ɘToolStripMenuItem.Text = "ɘ";
            // 
            // ɵToolStripMenuItem
            // 
            ɵToolStripMenuItem.Name = "ɵToolStripMenuItem";
            ɵToolStripMenuItem.Size = new Size(81, 22);
            ɵToolStripMenuItem.Text = "ɵ";
            // 
            // ɤToolStripMenuItem1
            // 
            ɤToolStripMenuItem1.Name = "ɤToolStripMenuItem1";
            ɤToolStripMenuItem1.Size = new Size(81, 22);
            ɤToolStripMenuItem1.Text = "ɤ";
            // 
            // oToolStripMenuItem1
            // 
            oToolStripMenuItem1.Name = "oToolStripMenuItem1";
            oToolStripMenuItem1.Size = new Size(81, 22);
            oToolStripMenuItem1.Text = "o";
            // 
            // midToolStripMenuItem
            // 
            midToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { eToolStripMenuItem, øToolStripMenuItem, əToolStripMenuItem, ɤToolStripMenuItem, oToolStripMenuItem });
            midToolStripMenuItem.Name = "midToolStripMenuItem";
            midToolStripMenuItem.Size = new Size(131, 22);
            midToolStripMenuItem.Text = "Mid";
            // 
            // eToolStripMenuItem
            // 
            eToolStripMenuItem.Name = "eToolStripMenuItem";
            eToolStripMenuItem.Size = new Size(81, 22);
            eToolStripMenuItem.Text = "e̞";
            // 
            // øToolStripMenuItem
            // 
            øToolStripMenuItem.Name = "øToolStripMenuItem";
            øToolStripMenuItem.Size = new Size(81, 22);
            øToolStripMenuItem.Text = "ø̞";
            // 
            // əToolStripMenuItem
            // 
            əToolStripMenuItem.Name = "əToolStripMenuItem";
            əToolStripMenuItem.Size = new Size(81, 22);
            əToolStripMenuItem.Text = "ə";
            // 
            // ɤToolStripMenuItem
            // 
            ɤToolStripMenuItem.Name = "ɤToolStripMenuItem";
            ɤToolStripMenuItem.Size = new Size(81, 22);
            ɤToolStripMenuItem.Text = "ɤ̞";
            // 
            // oToolStripMenuItem
            // 
            oToolStripMenuItem.Name = "oToolStripMenuItem";
            oToolStripMenuItem.Size = new Size(81, 22);
            oToolStripMenuItem.Text = "o̞";
            // 
            // openMidToolStripMenuItem
            // 
            openMidToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ɛToolStripMenuItem, œToolStripMenuItem, ɜToolStripMenuItem, ɞToolStripMenuItem, ʌToolStripMenuItem, ɔToolStripMenuItem });
            openMidToolStripMenuItem.Name = "openMidToolStripMenuItem";
            openMidToolStripMenuItem.Size = new Size(131, 22);
            openMidToolStripMenuItem.Text = "Open-mid";
            // 
            // ɛToolStripMenuItem
            // 
            ɛToolStripMenuItem.Name = "ɛToolStripMenuItem";
            ɛToolStripMenuItem.Size = new Size(85, 22);
            ɛToolStripMenuItem.Text = "ɛ";
            // 
            // œToolStripMenuItem
            // 
            œToolStripMenuItem.Name = "œToolStripMenuItem";
            œToolStripMenuItem.Size = new Size(85, 22);
            œToolStripMenuItem.Text = "œ";
            // 
            // ɜToolStripMenuItem
            // 
            ɜToolStripMenuItem.Name = "ɜToolStripMenuItem";
            ɜToolStripMenuItem.Size = new Size(85, 22);
            ɜToolStripMenuItem.Text = "ɜ";
            // 
            // ɞToolStripMenuItem
            // 
            ɞToolStripMenuItem.Name = "ɞToolStripMenuItem";
            ɞToolStripMenuItem.Size = new Size(85, 22);
            ɞToolStripMenuItem.Text = "ɞ";
            // 
            // ʌToolStripMenuItem
            // 
            ʌToolStripMenuItem.Name = "ʌToolStripMenuItem";
            ʌToolStripMenuItem.Size = new Size(85, 22);
            ʌToolStripMenuItem.Text = "ʌ";
            // 
            // ɔToolStripMenuItem
            // 
            ɔToolStripMenuItem.Name = "ɔToolStripMenuItem";
            ɔToolStripMenuItem.Size = new Size(85, 22);
            ɔToolStripMenuItem.Text = "ɔ";
            // 
            // nearOpenToolStripMenuItem
            // 
            nearOpenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { æToolStripMenuItem, ɐToolStripMenuItem });
            nearOpenToolStripMenuItem.Name = "nearOpenToolStripMenuItem";
            nearOpenToolStripMenuItem.Size = new Size(131, 22);
            nearOpenToolStripMenuItem.Text = "Near-open";
            // 
            // æToolStripMenuItem
            // 
            æToolStripMenuItem.Name = "æToolStripMenuItem";
            æToolStripMenuItem.Size = new Size(84, 22);
            æToolStripMenuItem.Text = "æ";
            // 
            // ɐToolStripMenuItem
            // 
            ɐToolStripMenuItem.Name = "ɐToolStripMenuItem";
            ɐToolStripMenuItem.Size = new Size(84, 22);
            ɐToolStripMenuItem.Text = "ɐ";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aToolStripMenuItem, ɶToolStripMenuItem, äToolStripMenuItem, ɑToolStripMenuItem, ɒToolStripMenuItem });
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(131, 22);
            openToolStripMenuItem.Text = "Open";
            // 
            // aToolStripMenuItem
            // 
            aToolStripMenuItem.Name = "aToolStripMenuItem";
            aToolStripMenuItem.Size = new Size(82, 22);
            aToolStripMenuItem.Text = "a";
            // 
            // ɶToolStripMenuItem
            // 
            ɶToolStripMenuItem.Name = "ɶToolStripMenuItem";
            ɶToolStripMenuItem.Size = new Size(82, 22);
            ɶToolStripMenuItem.Text = "ɶ";
            // 
            // äToolStripMenuItem
            // 
            äToolStripMenuItem.Name = "äToolStripMenuItem";
            äToolStripMenuItem.Size = new Size(82, 22);
            äToolStripMenuItem.Text = "\u0061\u0308";
            // 
            // ɑToolStripMenuItem
            // 
            ɑToolStripMenuItem.Name = "ɑToolStripMenuItem";
            ɑToolStripMenuItem.Size = new Size(82, 22);
            ɑToolStripMenuItem.Text = "ɑ";
            // 
            // ɒToolStripMenuItem
            // 
            ɒToolStripMenuItem.Name = "ɒToolStripMenuItem";
            ɒToolStripMenuItem.Size = new Size(82, 22);
            ɒToolStripMenuItem.Text = "ɒ";
            // 
            // symbolsToolStripMenuItem
            // 
            symbolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lengthenedToolStripMenuItem, halfLengthenedToolStripMenuItem, shortenedToolStripMenuItem, rhoticityToolStripMenuItem });
            symbolsToolStripMenuItem.Name = "symbolsToolStripMenuItem";
            symbolsToolStripMenuItem.Size = new Size(211, 22);
            symbolsToolStripMenuItem.Text = "IPA Symbols";
            // 
            // lengthenedToolStripMenuItem
            // 
            lengthenedToolStripMenuItem.Name = "lengthenedToolStripMenuItem";
            lengthenedToolStripMenuItem.Size = new Size(180, 22);
            lengthenedToolStripMenuItem.Text = "ː";
            // 
            // halfLengthenedToolStripMenuItem
            // 
            halfLengthenedToolStripMenuItem.Name = "halfLengthenedToolStripMenuItem";
            halfLengthenedToolStripMenuItem.Size = new Size(180, 22);
            halfLengthenedToolStripMenuItem.Text = "ˑ";
            // 
            // shortenedToolStripMenuItem
            // 
            shortenedToolStripMenuItem.Name = "shortenedToolStripMenuItem";
            shortenedToolStripMenuItem.Size = new Size(180, 22);
            shortenedToolStripMenuItem.Text = "̯";
            // 
            // rhoticityToolStripMenuItem
            // 
            rhoticityToolStripMenuItem.Name = "rhoticityToolStripMenuItem";
            rhoticityToolStripMenuItem.Size = new Size(180, 22);
            rhoticityToolStripMenuItem.Text = "˞";
            // 
            // latinTextDiacriticsToolStripMenuItem
            // 
            latinTextDiacriticsToolStripMenuItem.Name = "latinTextDiacriticsToolStripMenuItem";
            latinTextDiacriticsToolStripMenuItem.Size = new Size(211, 22);
            latinTextDiacriticsToolStripMenuItem.Text = "Latin/Text Diacritics";

            int subMenuCount = (LatinUtilities.DiacriticsMap.Count / 10) + 1;
            for (int i = 0; i < subMenuCount; i++)
            {
                ToolStripMenuItem menuItem = new()
                {
                    Size = new Size(50, 22),
                    Text = string.Format("Part {0}", i + 1)
                };
                _ = latinTextDiacriticsToolStripMenuItem.DropDownItems.Add(menuItem);
                latinDiacriticToolStripSubMenus.Insert(i, menuItem);
            }
            int counter = 0;
            int part = 0;
            foreach (string latinDiacritic in LatinUtilities.DiacriticsMap.Keys)
            {
                ToolStripMenuItem latinToolStripMenuItem = new()
                {
                    Size = new Size(211, 22),
                    Text = string.Format("{0} -- {1}", latinDiacritic, LatinUtilities.DiacriticsMap[latinDiacritic])
                };
                _ = latinDiacriticToolStripSubMenus[part].DropDownItems.Add(latinToolStripMenuItem);
                counter++;
                if (counter % 10 == 0)
                {
                    counter = 0;
                    part++;
                }
            }

            this.Size = new Size(107, 20);
            this.Text = "Character Inserts";
            this.DropDownItems.AddRange(new ToolStripItem[]
            {
                pulmonicConsonantsToolStripMenuItem,
                nonPulmonicConsonantsToolStripMenuItem,
                vowelsToolStripMenuItem,
                symbolsToolStripMenuItem,
                latinTextDiacriticsToolStripMenuItem,
            });
        }

        /// <summary>
        /// Add an event handler delegate that will respond when any of the end-point menu
        /// SpeakingSpeeds is clicked on.  This handler should take the menu item and insert that character
        /// into selected text.
        /// </summary>
        /// <param name="clickDelegate"></param>
        public void AddClickDelegate(EventHandler clickDelegate)
        {
            List<ToolStripMenuItem> ipaMenuItems =
            [
                pToolStripMenuItem,
                bToolStripMenuItem,
                tToolStripMenuItem,
                dToolStripMenuItem,
                ʈToolStripMenuItem,
                ɖToolStripMenuItem,
                cToolStripMenuItem,
                ɟToolStripMenuItem,
                kToolStripMenuItem,
                ɡToolStripMenuItem,
                qToolStripMenuItem,
                ɢToolStripMenuItem,
                ʔToolStripMenuItem,
                mToolStripMenuItem,
                ɱToolStripMenuItem,
                nToolStripMenuItem,
                ɳToolStripMenuItem,
                ɲToolStripMenuItem,
                ŋToolStripMenuItem,
                ɴToolStripMenuItem,
                ʙToolStripMenuItem,
                rToolStripMenuItem,
                ʀToolStripMenuItem,
                ⱱToolStripMenuItem,
                ɾToolStripMenuItem,
                ɽToolStripMenuItem,
                ɸToolStripMenuItem,
                βToolStripMenuItem,
                fToolStripMenuItem,
                vToolStripMenuItem,
                θToolStripMenuItem,
                ðToolStripMenuItem,
                sToolStripMenuItem,
                zToolStripMenuItem,
                ʃToolStripMenuItem,
                ʒToolStripMenuItem,
                ʂToolStripMenuItem,
                ʂToolStripMenuItem1,
                çToolStripMenuItem,
                ʝToolStripMenuItem,
                xToolStripMenuItem,
                ɣToolStripMenuItem,
                χToolStripMenuItem,
                ʁToolStripMenuItem,
                ħToolStripMenuItem,
                ʕToolStripMenuItem,
                ʜToolStripMenuItem,
                ʢToolStripMenuItem,
                ɦToolStripMenuItem,
                ɬToolStripMenuItem,
                ɮToolStripMenuItem,
                ʋToolStripMenuItem,
                ɹToolStripMenuItem,
                ɻToolStripMenuItem,
                ɰToolStripMenuItem,
                ɭToolStripMenuItem,
                ʎToolStripMenuItem,
                ʟToolStripMenuItem,
                ʘToolStripMenuItem,
                ǀDentalClickToolStripMenuItem,
                ǃAlveolarClickToolStripMenuItem,
                ǂClickToolStripMenuItem,
                ɓBilabialVoicedImplosiveToolStripMenuItem,
                ɗAlveolarVoicedImplosiveToolStripMenuItem,
                ʄPalatalVoicedImplosiveToolStripMenuItem,
                ɠVelarVoicedImplosiveToolStripMenuItem,
                ʛUvularVoicedImplosiveToolStripMenuItem,
                ʍVoicelessLabialVelarApproximateToolStripMenuItem,
                ɥVoicedLabialPalatalApproximateToolStripMenuItem,
                iToolStripMenuItem,
                yToolStripMenuItem,
                ɨToolStripMenuItem,
                ʉToolStripMenuItem,
                ɯToolStripMenuItem,
                uToolStripMenuItem,
                ɪToolStripMenuItem,
                ʏToolStripMenuItem,
                ɪToolStripMenuItem1,
                ʊToolStripMenuItem,
                ʊToolStripMenuItem1,
                eToolStripMenuItem,
                øToolStripMenuItem,
                əToolStripMenuItem,
                ɤToolStripMenuItem,
                oToolStripMenuItem,
                eToolStripMenuItem1,
                øToolStripMenuItem1,
                ɘToolStripMenuItem,
                ɵToolStripMenuItem,
                ɤToolStripMenuItem1,
                oToolStripMenuItem1,
                ɛToolStripMenuItem,
                œToolStripMenuItem,
                ɜToolStripMenuItem,
                ɞToolStripMenuItem,
                ʌToolStripMenuItem,
                ɔToolStripMenuItem,
                æToolStripMenuItem,
                ɐToolStripMenuItem,
                aToolStripMenuItem,
                ɶToolStripMenuItem,
                äToolStripMenuItem,
                ɑToolStripMenuItem,
                ɒToolStripMenuItem,
                lengthenedToolStripMenuItem,
                halfLengthenedToolStripMenuItem,
                shortenedToolStripMenuItem,
                rhoticityToolStripMenuItem,
            ];
            foreach (ToolStripMenuItem menuItem in ipaMenuItems)
            {
                if (!String.IsNullOrEmpty(menuItem.Text))
                {
                    string menuChar = menuItem.Text.Trim().Split()[0];
                    menuItem.Text = string.Format("{0} -- {1}", menuChar, IpaUtilities.IpaPhonemesMap[menuChar]);
                }
                menuItem.Click += clickDelegate;
            }
            foreach (ToolStripMenuItem menuItem in latinTextDiacriticsToolStripMenuItem.DropDownItems)
            {
                foreach (ToolStripMenuItem subMenuItem in menuItem.DropDownItems)
                {
                    subMenuItem.Click += clickDelegate;
                }
            }

        }
    }
}
