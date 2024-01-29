/*
 * Class with IPA reference structures etc.
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ConlangAudioHoning
{
	public static class IpaUtilities
	{
		private static string[] _p_consonants =
		{
			"b", "\u03b2", "\u0299", "c", "\u00e7", "d", "\u0256", "\u1d91", "\u02a3", "\u02a5", "\u02a4", "\uab66",
			"f", "\u0278", "g", "\u0262", "\u0270", "h", "\u0266", "\u0127", "\u0267", "j", "\u029d", "\u025f", "k",
			"l", "\u026b", "\u026c", "\u026e", "\ud837\udf05", "\u026d", "\ua78e", "\u029f", "\ud837\udf04", "m",
			"\u0271", "n", "\u0273", "\u0272", "\u014b", "\u0274", "p", "q", "r", "\u0279", "\u027e", "\u027d",
			"\u027b", "\u027a", "\ud837\udf08", "\u0281", "\u0280", "s", "\u0282", "\u0283", "t", "\u0288", "\u02a6",
			"\u02a8", "\u02a7", "\uab67", "v", "\u2c71", "\u028b", "x", "\u0263", "\u03c7", "\u028e", "\ud837\udf06",
			"z", "\u0290", "\u0292", "\u03b8", "\u00f0", "\u0294", "\u0295", "R"
		};
        /*
		 * b β ʙ c ç d ɖ ᶑ ʣ ʥ ʤ ꭦ		"b", "\u03b2", "\u0299", "c", "\u00e7", "d", "\u0256", "\u1d91", "\u02a3", "\u02a5", "\u02a4", "\uab66",
		 * f ɸ g ɢ ɰ h ɦ ħ ɧ j ʝ ɟ k	"f", "\u0278", "g", "\u0262", "\u0270", "h", "\u0266", "\u0127", "\u0267", "j", "\u029d", "\u025f", "k",
		 * l ɫ ɬ ɮ 𝼅 ɭ ꞎ ʟ 𝼄 m			"l", "\u026b", "\u026c", "\u026e", "\ud837\udf05", "\u026d", "\ua78e", "\u029f", "\ud837\udf04", "m",
		 * ɱ n ɳ ɲ ŋ ɴ p q r ɹ ɾ ɽ		"\u0271", "n", "\u0273", "\u0272", "\u014b", "\u0274", "p", "q", "r", "\u0279", "\u027e", "\u027d",
		 * ɻ ɺ 𝼈 ʁ ʀ s ʂ ʃ t ʈ ʦ ʨ		"\u027b", "\u027a", "\ud837\udf08", "\u0281", "\u0280", "s", "\u0282", "\u0283", "t", "\u0288", "\u02a6",
		 * ʧ ꭧ v ⱱ ʋ x ɣ χ ʎ 𝼆			"\u02a8", "\u02a7", "\uab67", "v", "\u2c71", "\u028b", "x", "\u0263", "\u03c7", "\u028e", "\ud837\udf06",
		 * z ʐ ʒ θ ð ʔ ʕ R				"z", "\u0290", "\u0292", "\u03b8", "\u00f0", "\u0294", "\u0295", "R"
		 */

        private static string[] _np_consonants =
		{
			"\u0253", "\u0257", "\u0284", "\u0260", "\u029b", "w", "\u028d", "\u0265", "\u02a1", "\u02a2", "\u0255",
			"\u0291", "\u029c", "\u0298", "\u01c0", "\u01c3", "\u01c2", "\u01c1", "\ud837\udf0a"
		};

		private static string[] _vowels =
		{
			"a", "\u00e6", "\u0251", "\u0252", "\u0250", "e", "\u025b", "\u025c", "\u025e", "\u0259", "i", "\u0268",
			"\u026a", "y", "\u028f", "\u00f8", "\u0258", "\u0275", "\u0153", "\u0276", "\u0264", "o", "\u0254", "u",
			"\u0289", "\u028a", "\u026f", "\u028c", "\u025a", "\u02de"
		};

		private static string[] _suprasegmentals =
		{
			"\u02d0", "\u02d1", "\u02c8", "\02cc", "\u035c", "\u0361"
		};

		private static string[] _diacritics =
		{
			"\u02f3", "\u0325", "\u030a", "\u0324", "\u032a", "\u02cc", "\u0329", "\u0c3c", "\u032c", "\u02f7", "\u0330",
			"\u02f7", "\u0330", "\u02fd", "\u033a", "\u032f", "\u02b0", "\u033c", "\u033b", "\u02d2", "\u0339", "\u20b7",
			"\u0303", "\u02b2", "\u02d3", "\u031c", "\u02d6", "\u031f", "\u207f", "\u00a8", "\u0308", "\u02e0", "\u02cd",
			"\u0320", "\u20e1", "\u02df", "\u033d", "\u02e4", "\uab68", "\u0319", "\u02de"
		};

		public static String[] PConsonants
		{
			get => _p_consonants;
		}

		public static string[] NpConsonants
		{
			get => _np_consonants;
		}

		public static string[] Vowels
		{
			get => _vowels;
		}

		public static string[] Suprasegmentals
		{
			get => _suprasegmentals;
		}

		public static string[] Diacritics
		{
			get => _diacritics;
		}
	}
}
