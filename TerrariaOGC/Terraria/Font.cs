﻿#if !USE_ORIGINAL_CODE
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria
{
	internal class Font
	{
		private static readonly string StackCombatCharacters = "/0123456789:";
		private static readonly string BigSmlCharacters = @" !#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		private static readonly string BigSmlCharacters2 = "¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſƀƁƂƃƄƅƆƇƈƉƊƋƌƍƎƏƐƑƒƓƔƕƖƗƘƙƚƛƜƝƞƟƠơƢƣƤƥƦƧƨƩƪƫƬƭƮƯưƱƲƳƴƵƶƷƸƹƺƻƼƽƾƿǀǁǂǃǄǅǆǇǈǉǊǋǌǍǎǏǐǑǒǓǔǕǖǗǘǙǚǛǜǝǞǟǠǡǢǣǤǥǦǧǨǩǪǫǬǭǮǯǰǱǲǳǴǵǶǷǸǹǺǻǼǽǾǿȀ";

		public List<Rectangle> StackGlyphBounds = new List<Rectangle>()
		{
			new Rectangle(87, 0, 13, 20),
			new Rectangle(15, 0, 14, 20),
			new Rectangle(30, 0, 9, 20),
			new Rectangle(40, 0, 15, 20),
			new Rectangle(56, 0, 15, 20),
			new Rectangle(72, 0, 14, 20),
			new Rectangle(0, 0, 14, 20),
			new Rectangle(101, 0, 15, 20),
			new Rectangle(0, 21, 13, 20),
			new Rectangle(14, 21, 14, 20),
			new Rectangle(29, 21, 13, 20),
			new Rectangle(43, 21, 9, 20),
		};

		public List<Rectangle> StackSCGlyphBounds = new List<Rectangle>()
		{
			new Rectangle(0, 16, 9, 15),
			new Rectangle(11, 0, 10, 15),
			new Rectangle(22, 0, 6, 15),
			new Rectangle(29, 0, 11, 15),
			new Rectangle(41, 0, 10, 15),
			new Rectangle(52, 0, 10, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(10, 16, 11, 15),
			new Rectangle(22, 16, 10, 15),
			new Rectangle(33, 16, 10, 15),
			new Rectangle(44, 16, 10, 15),
			new Rectangle(55, 16, 6, 15),
		};

		//----------------------------------------------------------------------
		public List<Rectangle> CombatGlyphBounds = new List<Rectangle>()
		{
			new Rectangle(84, 0, 19, 21),
			new Rectangle(16, 0, 14, 21),
			new Rectangle(31, 0, 18, 21),
			new Rectangle(50, 0, 14, 21),
			new Rectangle(65, 0, 18, 21),
			new Rectangle(0, 0, 15, 21),
			new Rectangle(104, 0, 16, 21),
			new Rectangle(0, 22, 17, 21),
			new Rectangle(18, 22, 16, 21),
			new Rectangle(35, 22, 17, 21),
		};

		public List<Rectangle> CombatBothSCGlyphBounds = new List<Rectangle>()
		{
			new Rectangle(16, 18, 16, 17),
			new Rectangle(13, 0, 11, 17),
			new Rectangle(25, 0, 15, 17),
			new Rectangle(41, 0, 12, 17),
			new Rectangle(0, 18, 15, 17),
			new Rectangle(0, 0, 12, 17),
			new Rectangle(33, 18, 14, 17),
			new Rectangle(48, 18, 14, 17),
			new Rectangle(0, 36, 13, 17),
			new Rectangle(14, 36, 14, 17),
		};

		public List<Rectangle> Combat2GlyphBounds = new List<Rectangle>()
		{
			new Rectangle(84, 0, 20, 22),
			new Rectangle(16, 0, 13, 22),
			new Rectangle(30, 0, 19, 22),
			new Rectangle(50, 0, 14, 22),
			new Rectangle(65, 0, 18, 22),
			new Rectangle(0, 0, 15, 22),
			new Rectangle(105, 0, 16, 22),
			new Rectangle(0, 23, 18, 22),
			new Rectangle(19, 23, 16, 22),
			new Rectangle(36, 23, 16, 22),
		};

		//----------------------------------------------------------------------
		public List<Rectangle> StackGlyphCrops = new List<Rectangle>()
		{
			new Rectangle(0, 0, 13, 20),
			new Rectangle(0, 0, 14, 20),
			new Rectangle(0, 0, 9, 20),
			new Rectangle(0, 0, 15, 20),
			new Rectangle(0, 0, 15, 20),
			new Rectangle(0, 0, 14, 20),
			new Rectangle(0, 0, 14, 20),
			new Rectangle(0, 0, 15, 20),
			new Rectangle(0, 0, 13, 20),
			new Rectangle(0, 0, 14, 20),
			new Rectangle(0, 0, 13, 20),
			new Rectangle(0, 0, 9, 20),
		};

		public List<Rectangle> StackSCGlyphCrops = new List<Rectangle>()
		{
			new Rectangle(0, 0, 9, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(0, 0, 6, 15),
			new Rectangle(0, 0, 11, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(0, 0, 11, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(0, 0, 10, 15),
			new Rectangle(0, 0, 6, 15),
		};

		//----------------------------------------------------------------------
		public List<Rectangle> CombatGlyphCrops = new List<Rectangle>()
		{
			new Rectangle(0, 0, 19, 21),
			new Rectangle(0, 0, 14, 21),
			new Rectangle(0, 0, 18, 21),
			new Rectangle(0, 0, 14, 21),
			new Rectangle(0, 0, 18, 21),
			new Rectangle(0, 0, 15, 21),
			new Rectangle(0, 0, 16, 21),
			new Rectangle(0, 0, 17, 21),
			new Rectangle(0, 0, 16, 21),
			new Rectangle(0, 0, 17, 21),
		};

		public List<Rectangle> CombatBothSCGlyphCrops = new List<Rectangle>()
		{
			new Rectangle(0, 0, 16, 17),
			new Rectangle(0, 0, 11, 17),
			new Rectangle(0, 0, 15, 17),
			new Rectangle(0, 0, 12, 17),
			new Rectangle(0, 0, 15, 17),
			new Rectangle(0, 0, 12, 17),
			new Rectangle(0, 0, 14, 17),
			new Rectangle(0, 0, 14, 17),
			new Rectangle(0, 0, 13, 17),
			new Rectangle(0, 0, 14, 17),
		};

		public List<Rectangle> Combat2GlyphCrops = new List<Rectangle>()
		{
			new Rectangle(0, 0, 20, 22),
			new Rectangle(0, 0, 13, 22),
			new Rectangle(0, 0, 19, 22),
			new Rectangle(0, 0, 14, 22),
			new Rectangle(0, 0, 18, 22),
			new Rectangle(0, 0, 15, 22),
			new Rectangle(0, 0, 16, 22),
			new Rectangle(0, 0, 18, 22),
			new Rectangle(0, 0, 16, 22),
			new Rectangle(0, 0, 16, 22),
		};

		//----------------------------------------------------------------------
		public List<char> StackChars = new List<char>()
        {
			StackCombatCharacters[0],
			StackCombatCharacters[1],
			StackCombatCharacters[2],
			StackCombatCharacters[3],
			StackCombatCharacters[4],
			StackCombatCharacters[5],
			StackCombatCharacters[6],
			StackCombatCharacters[7],
			StackCombatCharacters[8],
			StackCombatCharacters[9],
			StackCombatCharacters[10],
			StackCombatCharacters[11]
		};

		public List<char> CombatChars = new List<char>()
		{
			StackCombatCharacters[1],
			StackCombatCharacters[2],
			StackCombatCharacters[3],
			StackCombatCharacters[4],
			StackCombatCharacters[5],
			StackCombatCharacters[6],
			StackCombatCharacters[7],
			StackCombatCharacters[8],
			StackCombatCharacters[9],
			StackCombatCharacters[10]
		};

		//----------------------------------------------------------------------
		public List<Vector3> StackKerning = new List<Vector3>()
		{
			new Vector3(0, 13, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 9, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 13, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 13, 0),
			new Vector3(0, 9, 0),
		};

		public List<Vector3> StackSCKerning = new List<Vector3>()
		{
			new Vector3(0, 9, 0),
			new Vector3(0, 10, 0),
			new Vector3(0, 6, 0),
			new Vector3(0, 11, 0),
			new Vector3(0, 10, 0),
			new Vector3(0, 10, 0),
			new Vector3(0, 10, 0),
			new Vector3(0, 11, 0),
			new Vector3(0, 10, 0),
			new Vector3(0, 10, 0),
			new Vector3(0, 10, 0),
			new Vector3(0, 6, 0),
		};

		//----------------------------------------------------------------------
		public List<Vector3> CombatKerning = new List<Vector3>()
		{
			new Vector3(0, 19, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 18, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 18, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 17, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 17, 0),
		};

		public List<Vector3> CombatBothSCKerning = new List<Vector3>()
		{
			new Vector3(0, 16, 0),
			new Vector3(0, 11, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 12, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 12, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 13, 0),
			new Vector3(0, 14, 0),
		};

		public List<Vector3> Combat2Kerning = new List<Vector3>()
		{
			new Vector3(0, 20, 0),
			new Vector3(0, 13, 0),
			new Vector3(0, 19, 0),
			new Vector3(0, 14, 0),
			new Vector3(0, 18, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 18, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 16, 0),
		};

		//----------------------------------------------------------------------
		public List<char> BigSmlChars = new List<char>()
        {
			BigSmlCharacters[0],
			BigSmlCharacters[1],
			(char)0x22,
			BigSmlCharacters[2],
			BigSmlCharacters[3],
			BigSmlCharacters[4],
			BigSmlCharacters[5],
			BigSmlCharacters[6],
			BigSmlCharacters[7],
			BigSmlCharacters[8],
			BigSmlCharacters[9],
			BigSmlCharacters[10],
			BigSmlCharacters[11],
			BigSmlCharacters[12],
			BigSmlCharacters[13],
			BigSmlCharacters[14],
			BigSmlCharacters[15],
			BigSmlCharacters[16],
			BigSmlCharacters[17],
			BigSmlCharacters[18],
			BigSmlCharacters[19],
			BigSmlCharacters[20],
			BigSmlCharacters[21],
			BigSmlCharacters[22],
			BigSmlCharacters[23],
			BigSmlCharacters[24],
			BigSmlCharacters[25],
			BigSmlCharacters[26],
			BigSmlCharacters[27],
			BigSmlCharacters[28],
			BigSmlCharacters[29],
			BigSmlCharacters[30],
			BigSmlCharacters[31],
			BigSmlCharacters[32],
			BigSmlCharacters[33],
			BigSmlCharacters[34],
			BigSmlCharacters[35],
			BigSmlCharacters[36],
			BigSmlCharacters[37],
			BigSmlCharacters[38],
			BigSmlCharacters[39],
			BigSmlCharacters[40],
			BigSmlCharacters[41],
			BigSmlCharacters[42],
			BigSmlCharacters[43],
			BigSmlCharacters[44],
			BigSmlCharacters[45],
			BigSmlCharacters[46],
			BigSmlCharacters[47],
			BigSmlCharacters[48],
			BigSmlCharacters[49],
			BigSmlCharacters[50],
			BigSmlCharacters[51],
			BigSmlCharacters[52],
			BigSmlCharacters[53],
			BigSmlCharacters[54],
			BigSmlCharacters[55],
			BigSmlCharacters[56],
			BigSmlCharacters[57],
			BigSmlCharacters[58],
			BigSmlCharacters[59],
			BigSmlCharacters[60],
			BigSmlCharacters[61],
			BigSmlCharacters[62],
			BigSmlCharacters[63],
			BigSmlCharacters[64],
			BigSmlCharacters[65],
			BigSmlCharacters[66],
			BigSmlCharacters[67],
			BigSmlCharacters[68],
			BigSmlCharacters[69],
			BigSmlCharacters[70],
			BigSmlCharacters[71],
			BigSmlCharacters[72],
			BigSmlCharacters[73],
			BigSmlCharacters[74],
			BigSmlCharacters[75],
			BigSmlCharacters[76],
			BigSmlCharacters[77],
			BigSmlCharacters[78],
			BigSmlCharacters[79],
			BigSmlCharacters[80],
			BigSmlCharacters[81],
			BigSmlCharacters[82],
			BigSmlCharacters[83],
			BigSmlCharacters[84],
			BigSmlCharacters[85],
			BigSmlCharacters[86],
			BigSmlCharacters[87],
			BigSmlCharacters[88],
			BigSmlCharacters[89],
			BigSmlCharacters[90],
			BigSmlCharacters[91],
			BigSmlCharacters[92],
			BigSmlCharacters[93],
			(char)0x7F,					// Blank
			Lang.LeftTrigger,			// LT / L2
			Lang.RightTrigger,			// RT / R2
			Lang.LeftSButton,			// LB / L1
			Lang.RightSButton,			// RB / R1
			Lang.LeftStick,				// LS
			Lang.RightStick,			// RS
			Lang.DPad,					// D-Pad
			Lang.BackButton,			// Back / Options
			Lang.StartButton,			// Start
			Lang.HomeButton,			// Guide / PS
			Lang.XButton,				// X / Square
			Lang.YButton,				// Y / Triangle
			Lang.AButton,				// A / Xross (no, that is not an error, that is how I spell it for PlayStation)
			Lang.BButton,				// B / Circle
			Lang.LeftStickPress,		// L3
			Lang.RightStickPress,		// R3
			(char)0xC290,		// Blank / Contact
			(char)0xC291, 		// Blank
			(char)0xC292, 		// Blank
			(char)0xC293, 		// Blank
			(char)0xC294, 		// Blank
			(char)0xC295, 		// Blank
			(char)0xC296, 		// Blank
			(char)0xC297, 		// Blank
			(char)0xC298, 		// Blank
			(char)0xC299, 		// Blank
			(char)0xC29A, 		// Blank
			(char)0xC29B, 		// Blank
			(char)0xC29C, 		// Blank
			(char)0xC29D, 		// Blank
			(char)0xC29E, 		// Blank
			(char)0xC29F, 		// Blank
			(char)0xC2A0,		// Blank
			BigSmlCharacters2[0],
			BigSmlCharacters2[1],
			BigSmlCharacters2[2],
			BigSmlCharacters2[3],
			BigSmlCharacters2[4],
			BigSmlCharacters2[5],
			BigSmlCharacters2[6],
			BigSmlCharacters2[7],
			BigSmlCharacters2[8],
			BigSmlCharacters2[9],
			BigSmlCharacters2[10],
			BigSmlCharacters2[11],
			BigSmlCharacters2[12],
			BigSmlCharacters2[13],
			BigSmlCharacters2[14],
			BigSmlCharacters2[15],
			BigSmlCharacters2[16],
			BigSmlCharacters2[17],
			BigSmlCharacters2[18],
			BigSmlCharacters2[19],
			BigSmlCharacters2[20],
			BigSmlCharacters2[21],
			BigSmlCharacters2[22],
			BigSmlCharacters2[23],
			BigSmlCharacters2[24],
			BigSmlCharacters2[25],
			BigSmlCharacters2[26],
			BigSmlCharacters2[27],
			BigSmlCharacters2[28],
			BigSmlCharacters2[29],
			BigSmlCharacters2[30],
			BigSmlCharacters2[31],
			BigSmlCharacters2[32],
			BigSmlCharacters2[33],
			BigSmlCharacters2[34],
			BigSmlCharacters2[35],
			BigSmlCharacters2[36],
			BigSmlCharacters2[37],
			BigSmlCharacters2[38],
			BigSmlCharacters2[39],
			BigSmlCharacters2[40],
			BigSmlCharacters2[41],
			BigSmlCharacters2[42],
			BigSmlCharacters2[43],
			BigSmlCharacters2[44],
			BigSmlCharacters2[45],
			BigSmlCharacters2[46],
			BigSmlCharacters2[47],
			BigSmlCharacters2[48],
			BigSmlCharacters2[49],
			BigSmlCharacters2[50],
			BigSmlCharacters2[51],
			BigSmlCharacters2[52],
			BigSmlCharacters2[53],
			BigSmlCharacters2[54],
			BigSmlCharacters2[55],
			BigSmlCharacters2[56],
			BigSmlCharacters2[57],
			BigSmlCharacters2[58],
			BigSmlCharacters2[59],
			BigSmlCharacters2[60],
			BigSmlCharacters2[61],
			BigSmlCharacters2[62],
			BigSmlCharacters2[63],
			BigSmlCharacters2[64],
			BigSmlCharacters2[65],
			BigSmlCharacters2[66],
			BigSmlCharacters2[67],
			BigSmlCharacters2[68],
			BigSmlCharacters2[69],
			BigSmlCharacters2[70],
			BigSmlCharacters2[71],
			BigSmlCharacters2[72],
			BigSmlCharacters2[73],
			BigSmlCharacters2[74],
			BigSmlCharacters2[75],
			BigSmlCharacters2[76],
			BigSmlCharacters2[77],
			BigSmlCharacters2[78],
			BigSmlCharacters2[79],
			BigSmlCharacters2[80],
			BigSmlCharacters2[81],
			BigSmlCharacters2[82],
			BigSmlCharacters2[83],
			BigSmlCharacters2[84],
			BigSmlCharacters2[85],
			BigSmlCharacters2[86],
			BigSmlCharacters2[87],
			BigSmlCharacters2[88],
			BigSmlCharacters2[89],
			BigSmlCharacters2[90],
			BigSmlCharacters2[91],
			BigSmlCharacters2[92],
			BigSmlCharacters2[93],
			BigSmlCharacters2[94],
			BigSmlCharacters2[95],
			BigSmlCharacters2[96],
			BigSmlCharacters2[97],
			BigSmlCharacters2[98],
			BigSmlCharacters2[99],
			BigSmlCharacters2[100],
			BigSmlCharacters2[101],
			BigSmlCharacters2[102],
			BigSmlCharacters2[103],
			BigSmlCharacters2[104],
			BigSmlCharacters2[105],
			BigSmlCharacters2[106],
			BigSmlCharacters2[107],
			BigSmlCharacters2[108],
			BigSmlCharacters2[109],
			BigSmlCharacters2[110],
			BigSmlCharacters2[111],
			BigSmlCharacters2[112],
			BigSmlCharacters2[113],
			BigSmlCharacters2[114],
			BigSmlCharacters2[115],
			BigSmlCharacters2[116],
			BigSmlCharacters2[117],
			BigSmlCharacters2[118],
			BigSmlCharacters2[119],
			BigSmlCharacters2[120],
			BigSmlCharacters2[121],
			BigSmlCharacters2[122],
			BigSmlCharacters2[123],
			BigSmlCharacters2[124],
			BigSmlCharacters2[125],
			BigSmlCharacters2[126],
			BigSmlCharacters2[127],
			BigSmlCharacters2[128],
			BigSmlCharacters2[129],
			BigSmlCharacters2[130],
			BigSmlCharacters2[131],
			BigSmlCharacters2[132],
			BigSmlCharacters2[133],
			BigSmlCharacters2[134],
			BigSmlCharacters2[135],
			BigSmlCharacters2[136],
			BigSmlCharacters2[137],
			BigSmlCharacters2[138],
			BigSmlCharacters2[139],
			BigSmlCharacters2[140],
			BigSmlCharacters2[141],
			BigSmlCharacters2[142],
			BigSmlCharacters2[143],
			BigSmlCharacters2[144],
			BigSmlCharacters2[145],
			BigSmlCharacters2[146],
			BigSmlCharacters2[147],
			BigSmlCharacters2[148],
			BigSmlCharacters2[149],
			BigSmlCharacters2[150],
			BigSmlCharacters2[151],
			BigSmlCharacters2[152],
			BigSmlCharacters2[153],
			BigSmlCharacters2[154],
			BigSmlCharacters2[155],
			BigSmlCharacters2[156],
			BigSmlCharacters2[157],
			BigSmlCharacters2[158],
			BigSmlCharacters2[159],
			BigSmlCharacters2[160],
			BigSmlCharacters2[161],
			BigSmlCharacters2[162],
			BigSmlCharacters2[163],
			BigSmlCharacters2[164],
			BigSmlCharacters2[165],
			BigSmlCharacters2[166],
			BigSmlCharacters2[167],
			BigSmlCharacters2[168],
			BigSmlCharacters2[169],
			BigSmlCharacters2[170],
			BigSmlCharacters2[171],
			BigSmlCharacters2[172],
			BigSmlCharacters2[173],
			BigSmlCharacters2[174],
			BigSmlCharacters2[175],
			BigSmlCharacters2[176],
			BigSmlCharacters2[177],
			BigSmlCharacters2[178],
			BigSmlCharacters2[179],
			BigSmlCharacters2[180],
			BigSmlCharacters2[181],
			BigSmlCharacters2[182],
			BigSmlCharacters2[183],
			BigSmlCharacters2[184],
			BigSmlCharacters2[185],
			BigSmlCharacters2[186],
			BigSmlCharacters2[187],
			BigSmlCharacters2[188],
			BigSmlCharacters2[189],
			BigSmlCharacters2[190],
			BigSmlCharacters2[191],
			BigSmlCharacters2[192],
			BigSmlCharacters2[193],
			BigSmlCharacters2[194],
			BigSmlCharacters2[195],
			BigSmlCharacters2[196],
			BigSmlCharacters2[197],
			BigSmlCharacters2[198],
			BigSmlCharacters2[199],
			BigSmlCharacters2[200],
			BigSmlCharacters2[201],
			BigSmlCharacters2[202],
			BigSmlCharacters2[203],
			BigSmlCharacters2[204],
			BigSmlCharacters2[205],
			BigSmlCharacters2[206],
			BigSmlCharacters2[207],
			BigSmlCharacters2[208],
			BigSmlCharacters2[209],
			BigSmlCharacters2[210],
			BigSmlCharacters2[211],
			BigSmlCharacters2[212],
			BigSmlCharacters2[213],
			BigSmlCharacters2[214],
			BigSmlCharacters2[215],
			BigSmlCharacters2[216],
			BigSmlCharacters2[217],
			BigSmlCharacters2[218],
			BigSmlCharacters2[219],
			BigSmlCharacters2[220],
			BigSmlCharacters2[221],
			BigSmlCharacters2[222],
			BigSmlCharacters2[223],
			BigSmlCharacters2[224],
			BigSmlCharacters2[225],
			BigSmlCharacters2[226],
			BigSmlCharacters2[227],
			BigSmlCharacters2[228],
			BigSmlCharacters2[229],
			BigSmlCharacters2[230],
			BigSmlCharacters2[231],
			BigSmlCharacters2[232],
			BigSmlCharacters2[233],
			BigSmlCharacters2[234],
			BigSmlCharacters2[235],
			BigSmlCharacters2[236],
			BigSmlCharacters2[237],
			BigSmlCharacters2[238],
			BigSmlCharacters2[239],
			BigSmlCharacters2[240],
			BigSmlCharacters2[241],
			BigSmlCharacters2[242],
			BigSmlCharacters2[243],
			BigSmlCharacters2[244],
			BigSmlCharacters2[245],
			BigSmlCharacters2[246],
			BigSmlCharacters2[247],
			BigSmlCharacters2[248],
			BigSmlCharacters2[249],
			BigSmlCharacters2[250],
			BigSmlCharacters2[251],
			BigSmlCharacters2[252],
			BigSmlCharacters2[253],
			BigSmlCharacters2[254],
			BigSmlCharacters2[255],
			BigSmlCharacters2[256],
			BigSmlCharacters2[257],
			BigSmlCharacters2[258],
			BigSmlCharacters2[259],
			BigSmlCharacters2[260],
			BigSmlCharacters2[261],
			BigSmlCharacters2[262],
			BigSmlCharacters2[263],
			BigSmlCharacters2[264],
			BigSmlCharacters2[265],
			BigSmlCharacters2[266],
			BigSmlCharacters2[267],
			BigSmlCharacters2[268],
			BigSmlCharacters2[269],
			BigSmlCharacters2[270],
			BigSmlCharacters2[271],
			BigSmlCharacters2[272],
			BigSmlCharacters2[273],
			BigSmlCharacters2[274],
			BigSmlCharacters2[275],
			BigSmlCharacters2[276],
			BigSmlCharacters2[277],
			BigSmlCharacters2[278],
			BigSmlCharacters2[279],
			BigSmlCharacters2[280],
			BigSmlCharacters2[281],
			BigSmlCharacters2[282],
			BigSmlCharacters2[283],
			BigSmlCharacters2[284],
			BigSmlCharacters2[285],
			BigSmlCharacters2[286],
			BigSmlCharacters2[287],
			BigSmlCharacters2[288],
			BigSmlCharacters2[289],
			BigSmlCharacters2[290],
			BigSmlCharacters2[291],
			BigSmlCharacters2[292],
			BigSmlCharacters2[293],
			BigSmlCharacters2[294],
			BigSmlCharacters2[295],
			BigSmlCharacters2[296],
			BigSmlCharacters2[297],
			BigSmlCharacters2[298],
			BigSmlCharacters2[299],
			BigSmlCharacters2[300],
			BigSmlCharacters2[301],
			BigSmlCharacters2[302],
			BigSmlCharacters2[303],
			BigSmlCharacters2[304],
			BigSmlCharacters2[305],
			BigSmlCharacters2[306],
			BigSmlCharacters2[307],
			BigSmlCharacters2[308],
			BigSmlCharacters2[309],
			BigSmlCharacters2[310],
			BigSmlCharacters2[311],
			BigSmlCharacters2[312],
			BigSmlCharacters2[313],
			BigSmlCharacters2[314],
			BigSmlCharacters2[315],
			BigSmlCharacters2[316],
			BigSmlCharacters2[317],
			BigSmlCharacters2[318],
			BigSmlCharacters2[319],
			BigSmlCharacters2[320],
			BigSmlCharacters2[321],
			BigSmlCharacters2[322],
			BigSmlCharacters2[323],
			BigSmlCharacters2[324],
			BigSmlCharacters2[325],
			BigSmlCharacters2[326],
			BigSmlCharacters2[327],
			BigSmlCharacters2[328],
			BigSmlCharacters2[329],
			BigSmlCharacters2[330],
			BigSmlCharacters2[331],
			BigSmlCharacters2[332],
			BigSmlCharacters2[333],
			BigSmlCharacters2[334],
			BigSmlCharacters2[335],
			BigSmlCharacters2[336],
			BigSmlCharacters2[337],
			BigSmlCharacters2[338],
			BigSmlCharacters2[339],
			BigSmlCharacters2[340],
			BigSmlCharacters2[341],
			BigSmlCharacters2[342],
			BigSmlCharacters2[343],
			BigSmlCharacters2[344],
			BigSmlCharacters2[345],
			BigSmlCharacters2[346],
			BigSmlCharacters2[347],
			BigSmlCharacters2[348],
			BigSmlCharacters2[349],
			BigSmlCharacters2[350],
			BigSmlCharacters2[351]
		};

		public List<Rectangle> XBSmlBoundReplace = new List<Rectangle>()
		{
			new Rectangle(111, 64, 25, 31),
			new Rectangle(137, 64, 25, 31),
			new Rectangle(163, 64, 39, 31),
			new Rectangle(203, 64, 39, 31),
			new Rectangle(243, 64, 33, 31),
			new Rectangle(277, 64, 33, 31),
			new Rectangle(311, 64, 34, 31),
			new Rectangle(346, 64, 27, 31),
			new Rectangle(374, 64, 26, 31),
			new Rectangle(401, 64, 32, 31),
			new Rectangle(434, 64, 27, 31),
			new Rectangle(462, 64, 27, 31),
			new Rectangle(0, 96, 27, 31),
			new Rectangle(28, 96, 27, 31),
			new Rectangle(56, 96, 30, 31),
			new Rectangle(87, 96, 33, 31)
		};

		public List<Rectangle> XBSml2BoundReplace = new List<Rectangle>()
		{
			new Rectangle(495, 36, 24, 35),
			new Rectangle(520, 36, 25, 35),
			new Rectangle(546, 36, 39, 35),
			new Rectangle(586, 36, 39, 35),
			new Rectangle(626, 36, 34, 35),
			new Rectangle(661, 36, 34, 35),
			new Rectangle(696, 36, 34, 35),
			new Rectangle(731, 36, 27, 35),
			new Rectangle(759, 36, 26, 35),
			new Rectangle(786, 36, 32, 35),
			new Rectangle(819, 36, 26, 35),
			new Rectangle(846, 36, 27, 35),
			new Rectangle(874, 36, 27, 35),
			new Rectangle(902, 36, 27, 35),
			new Rectangle(930, 36, 29, 35),
			new Rectangle(960, 36, 35, 35),
		};

		public List<Rectangle> XBSml2SCBoundReplace = new List<Rectangle>()
		{
			new Rectangle(351, 22, 15, 21),
			new Rectangle(367, 22, 15, 21),
			new Rectangle(383, 22, 22, 21),
			new Rectangle(406, 22, 23, 21),
			new Rectangle(430, 22, 19, 21),
			new Rectangle(450, 22, 20, 21),
			new Rectangle(471, 22, 20, 21),
			new Rectangle(492, 22, 16, 21),
			new Rectangle(0, 44, 16, 21),
			new Rectangle(17, 44, 20, 21),
			new Rectangle(38, 44, 16, 21),
			new Rectangle(55, 44, 16, 21),
			new Rectangle(72, 44, 17, 21),
			new Rectangle(90, 44, 17, 21),
			new Rectangle(108, 44, 18, 21),
			new Rectangle(127, 44, 20, 21),
		};

		public List<Rectangle> XBSmlCropReplace = new List<Rectangle>()
		{
			new Rectangle(0, 0, 25, 31),
			new Rectangle(0, 0, 25, 31),
			new Rectangle(0, 0, 39, 31),
			new Rectangle(0, 0, 39, 31),
			new Rectangle(0, 0, 33, 31),
			new Rectangle(0, 0, 33, 31),
			new Rectangle(0, 0, 34, 31),
			new Rectangle(0, 0, 27, 31),
			new Rectangle(0, 0, 26, 31),
			new Rectangle(0, 0, 32, 31),
			new Rectangle(0, 0, 27, 31),
			new Rectangle(0, 0, 27, 31),
			new Rectangle(0, 0, 27, 31),
			new Rectangle(0, 0, 27, 31),
			new Rectangle(0, 0, 30, 31),
			new Rectangle(0, 0, 33, 31)
		};

		public List<Rectangle> XBSml2CropReplace = new List<Rectangle>()
		{
			new Rectangle(0, 0, 24, 35),
			new Rectangle(0, 0, 25, 35),
			new Rectangle(0, 0, 39, 35),
			new Rectangle(0, 0, 39, 35),
			new Rectangle(0, 0, 34, 35),
			new Rectangle(0, 0, 34, 35),
			new Rectangle(0, 0, 34, 35),
			new Rectangle(0, 0, 27, 35),
			new Rectangle(0, 0, 26, 35),
			new Rectangle(0, 0, 32, 35),
			new Rectangle(0, 0, 26, 35),
			new Rectangle(0, 0, 27, 35),
			new Rectangle(0, 0, 27, 35),
			new Rectangle(0, 0, 27, 35),
			new Rectangle(0, 0, 29, 35),
			new Rectangle(0, 0, 35, 35),
		};

		public List<Rectangle> XBSml2SCCropReplace = new List<Rectangle>()
		{
			new Rectangle(0, 0, 15, 21),
			new Rectangle(0, 0, 15, 21),
			new Rectangle(0, 0, 22, 21),
			new Rectangle(0, 0, 23, 21),
			new Rectangle(0, 0, 19, 21),
			new Rectangle(0, 0, 20, 21),
			new Rectangle(0, 0, 20, 21),
			new Rectangle(0, 0, 16, 21),
			new Rectangle(0, 0, 16, 21),
			new Rectangle(0, 0, 20, 21),
			new Rectangle(0, 0, 16, 21),
			new Rectangle(0, 0, 16, 21),
			new Rectangle(0, 0, 17, 21),
			new Rectangle(0, 0, 17, 21),
			new Rectangle(0, 0, 18, 21),
			new Rectangle(0, 0, 20, 21),
		};

		public List<Vector3> XBSmlKerningReplace = new List<Vector3>()
		{
			new Vector3(0, 25, 0),
			new Vector3(0, 25, 0),
			new Vector3(0, 39, 0),
			new Vector3(0, 39, 0),
			new Vector3(0, 33, 0),
			new Vector3(0, 33, 0),
			new Vector3(0, 34, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 26, 0),
			new Vector3(0, 32, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 30, 0),
			new Vector3(0, 33, 0)
		};

		public List<Vector3> XBSml2KerningReplace = new List<Vector3>()
		{
			new Vector3(0, 24, 0),
			new Vector3(0, 25, 0),
			new Vector3(0, 39, 0),
			new Vector3(0, 39, 0),
			new Vector3(0, 34, 0),
			new Vector3(0, 34, 0),
			new Vector3(0, 34, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 26, 0),
			new Vector3(0, 32, 0),
			new Vector3(0, 26, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 27, 0),
			new Vector3(0, 29, 0),
			new Vector3(0, 35, 0),
		};

		public List<Vector3> XBSml2SCKerningReplace = new List<Vector3>()
		{
			new Vector3(0, 15, 0),
			new Vector3(0, 15, 0),
			new Vector3(0, 22, 0),
			new Vector3(0, 23, 0),
			new Vector3(0, 19, 0),
			new Vector3(0, 20, 0),
			new Vector3(0, 20, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 20, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 16, 0),
			new Vector3(0, 17, 0),
			new Vector3(0, 17, 0),
			new Vector3(0, 18, 0),
			new Vector3(0, 20, 0),
		};
	}
}
#endif