using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria
{
	public sealed class CompiledText
	{
		public enum MarkupType
		{
			Plain,
			Html
		}

		[Flags]
		public enum Attributes
		{
			None = 0x0,
			Italic = 0x1,
			Bold = 0x2,
			Highlighted = 0x4,
			Center = 0x8,
			Right = 0x10,
			TextStyle = 0x7,
			Alignment = 0x18
		}

		public class Style
		{
			public Color ForegroundColor;

			public SpriteFont Font;

			public Attributes TextAttributes;

			public Style(SpriteFont FontType)
			{
				ForegroundColor = Color.Transparent;
				Font = FontType;
				TextAttributes = Attributes.None;
			}

			public Style(Style ActiveStyle)
			{
				ForegroundColor = ActiveStyle.ForegroundColor;
				Font = ActiveStyle.Font;
				TextAttributes = ActiveStyle.TextAttributes;
			}
		}

		private sealed class Segment
		{
			public string SegmentText;

			public short SourceCharIndex;

			public short SourceCharLength;

			public Rectangle Position;

			public Style Style;
		}

		private enum TagType
		{
			None,
			Bold,
			Italic,
			Underline,
			Link,
			Center,
			Right,
			Font,
			Paragraph,
			Highlighted,
			FontTitle
		}

		private sealed class OpenTag
		{
			public TagType Tag;

			public Style Style;

			public OpenTag(TagType FontTag, Style FontStyle)
			{
				Tag = FontTag;
				Style = FontStyle;
			}
		}

		public const short WidthOffset = 5;

		public const short HeightOffset = 12;

		private const int ParagraphBreakHeight = 1;

		public string TextString;

		public short Width;

		public short Height;

		private readonly List<Segment> Segments = new List<Segment>();

		private readonly Stack<OpenTag> Tags = new Stack<OpenTag>();

		public CompiledText(string Text, int WrapWidth, Style FontStyle, MarkupType MarkupType = MarkupType.Html)
		{
			Tags.Push(new OpenTag(TagType.None, FontStyle));
			int XPlacement = 0;
			int YPlacement = 0;
			int YSpacing = 0;
			TextString = Text;
			Width = 0;
			Height = 0;

			for (int CharacterIdx = 0; CharacterIdx < Text.Length;)
			{
				if (MarkupType == MarkupType.Html && Text[CharacterIdx] == '<' && Text[CharacterIdx + 1] == '/')
				{
					CharacterIdx += 2;
					SkipTag(Text, MarkupType, ref CharacterIdx, ShouldClose: true, out var TagType, out var UnusedColor);
					while (Tags.Count > 1 && Tags.Pop().Tag != TagType) { }
					continue;
				}
				if (MarkupType == MarkupType.Html && Text[CharacterIdx] == '<')
				{
					CharacterIdx++;
					SkipTag(Text, MarkupType, ref CharacterIdx, ShouldClose: false, out var TagType, out var NewColor);
					Style TagStyle = new Style(Tags.Peek().Style);
					switch (TagType)
					{
						case TagType.Bold:
							TagStyle.TextAttributes |= Attributes.Bold;
							break;
						case TagType.Center:
							TagStyle.TextAttributes |= Attributes.Center;
							break;
						case TagType.Font:
							TagStyle.ForegroundColor = NewColor.Value;
							break;
						case TagType.Italic:
							TagStyle.TextAttributes |= Attributes.Italic;
							break;
						case TagType.Highlighted:
							TagStyle.TextAttributes |= Attributes.Highlighted;
							break;
						case TagType.Right:
							TagStyle.TextAttributes |= Attributes.Right;
							break;
						case TagType.FontTitle:
							TagStyle.Font = UI.BigFont;
							break;
					}
					if (TagType != TagType.Paragraph)
					{
						Tags.Push(new OpenTag(TagType, TagStyle));
						continue;
					}
					YPlacement += Math.Max(UI.LineSpacing(Tags.Peek().Style.Font), YSpacing) + ParagraphBreakHeight;
					XPlacement = 0;
					YSpacing = 0;
					continue;
				}
				int StartIdx = CharacterIdx;
				int StringLength = 0;
				while (CharacterIdx < Text.Length && !IsEol(Text[CharacterIdx]) && (MarkupType != MarkupType.Html || Text[CharacterIdx] != '<'))
				{
					CharacterIdx++;
					StringLength++;
				}
				if (WrapWidth == 0)
				{
					Vector2 TextMeasurement = UI.MeasureString(Tags.Peek().Style.Font, Text.Substring(StartIdx, StringLength));
					TextMeasurement.X += UI.Spacing(Tags.Peek().Style.Font);
					TextMeasurement.Y = UI.LineSpacing(Tags.Peek().Style.Font);
#if !USE_ORIGINAL_CODE
					if (Main.PSMode && Main.ScreenHeightPtr != 2)
					{
						TextMeasurement.Y += 7;
					}
#endif
					AddSegment(Text, StartIdx, StringLength, Tags.Peek().Style, XPlacement, YPlacement, (int)TextMeasurement.X, (int)TextMeasurement.Y);
					XPlacement += (int)TextMeasurement.X;
					YSpacing = Math.Max(YSpacing, (int)TextMeasurement.Y);
				}
				else
				{
					while (StringLength != 0)
					{
						int RevisedLength;
						if ((RevisedLength = AnimationUtils.SqueezeText(Tags.Peek().Style.Font, Text.Substring(StartIdx, StringLength), WrapWidth - XPlacement, AnimationUtils.SqueezeTextFlags.WordBreak)) == 0 && XPlacement != 0)
						{
							YPlacement += Math.Max(YSpacing, UI.LineSpacing(Tags.Peek().Style.Font));
							XPlacement = 0;
							YSpacing = 0;
							RevisedLength = AnimationUtils.SqueezeText(Tags.Peek().Style.Font, Text.Substring(StartIdx, StringLength), WrapWidth - XPlacement, AnimationUtils.SqueezeTextFlags.WordBreak);
						}
						if (RevisedLength == 0)
						{
							RevisedLength = AnimationUtils.SqueezeText(Tags.Peek().Style.Font, Text.Substring(StartIdx, StringLength), WrapWidth - XPlacement, AnimationUtils.SqueezeTextFlags.NoWordBreak);
						}
						if (RevisedLength == 0)
						{
							break;
						}
						Vector2 RevisedMeasurement = UI.MeasureString(Tags.Peek().Style.Font, Text.Substring(StartIdx, RevisedLength));
						RevisedMeasurement.X += UI.Spacing(Tags.Peek().Style.Font);
						RevisedMeasurement.Y = UI.LineSpacing(Tags.Peek().Style.Font);
#if !USE_ORIGINAL_CODE
						if (Main.PSMode && Main.ScreenHeightPtr != 2)
						{
							RevisedMeasurement.Y += 7;
						}
#endif
						AddSegment(Text, StartIdx, RevisedLength, Tags.Peek().Style, XPlacement, YPlacement, (int)RevisedMeasurement.X, (int)RevisedMeasurement.Y);
						XPlacement += (int)RevisedMeasurement.X;
						YSpacing = Math.Max(YSpacing, (int)RevisedMeasurement.Y);
						if (RevisedLength < StringLength)
						{
							YPlacement += Math.Max(YSpacing, UI.LineSpacing(Tags.Peek().Style.Font));
							XPlacement = 0;
							YSpacing = 0;
						}
						StartIdx += RevisedLength;
						StringLength -= RevisedLength;
						while (StringLength != 0 && Text[StartIdx] == ' ')
						{
							StringLength--;
							StartIdx++;
						}
					}
				}
				for (; CharacterIdx < Text.Length && IsEol(Text[CharacterIdx]); CharacterIdx++)
				{
					if (Text[CharacterIdx] == '\n')
					{
						YPlacement += Math.Max(YSpacing, UI.LineSpacing(Tags.Peek().Style.Font));
						XPlacement = 0;
						YSpacing = 0;
					}
				}
			}
			if (WrapWidth == 0)
			{
				return;
			}
			int CurrentYPos = 0;
			int FirstPrevSegment = -1;
			int NextSegmentIdx = -1;
			Attributes ActiveAttributes = Attributes.None;
			for (int SegmentIdx = 0; SegmentIdx < Segments.Count; SegmentIdx = NextSegmentIdx)
			{
				NextSegmentIdx = SegmentIdx + 1;
				if ((Segments[SegmentIdx].Style.TextAttributes & Attributes.Alignment) != 0)
				{
					if (FirstPrevSegment == -1)
					{
						FirstPrevSegment = SegmentIdx;
						CurrentYPos = Segments[SegmentIdx].Position.Y;
						ActiveAttributes = Segments[SegmentIdx].Style.TextAttributes & Attributes.Alignment;
					}
					if (NextSegmentIdx == Segments.Count || Segments[NextSegmentIdx].Position.Y != CurrentYPos || (Segments[NextSegmentIdx].Style.TextAttributes & Attributes.Alignment) != ActiveAttributes)
					{
						int SegXPlacement = Segments[SegmentIdx].Position.Right - Segments[FirstPrevSegment].Position.X;
						int XOffset = (WrapWidth - SegXPlacement) / (((ActiveAttributes & Attributes.Center) == 0) ? 1 : 2);
						for (int PrevSegmentsIdx = FirstPrevSegment; PrevSegmentsIdx != NextSegmentIdx; PrevSegmentsIdx++)
						{
							Segments[PrevSegmentsIdx].Position.X += XOffset;
						}
						FirstPrevSegment = -1;
					}
				}
			}
		}

		public void Draw(SpriteBatch UnusedBatchTarget, Rectangle StringClip, Color BaseColour, Color AccentColour)
		{
			if (StringClip.Width == 0 || StringClip.Height == 0)
			{
				return;
			}
			foreach (Segment TextSegment in Segments)
			{
				Color TextColour = TextSegment.Style.ForegroundColor;
				if ((TextSegment.Style.TextAttributes & Attributes.Italic) != 0)
				{
					TextColour = AccentColour;
				}
				else if ((TextSegment.Style.TextAttributes & Attributes.Highlighted) != 0)
				{
					TextColour = new Color(64, 255, 255, 255);
				}
				if (TextColour == Color.Transparent)
				{
					TextColour = BaseColour;
				}
				if (TextSegment.Position.X < StringClip.Width && TextSegment.Position.Y < StringClip.Height)
				{
					UI.DrawStringLT(TextSegment.Style.Font, TextSegment.SegmentText, StringClip.X + TextSegment.Position.X, StringClip.Y + TextSegment.Position.Y, TextColour);
					if ((TextSegment.Style.TextAttributes & Attributes.Bold) != 0)
					{
						UI.DrawStringLT(TextSegment.Style.Font, TextSegment.SegmentText, StringClip.X + TextSegment.Position.X + 1, StringClip.Y + TextSegment.Position.Y, TextColour);
					}
				}
			}
		}

		public int CharHitTest(Point CharPoint, bool IsFuzzy) // Debug Function
		{
			int num = -1;
			int SelectedYIdx = 0;
			int num2 = -1;
			int SelectedXIdx = -1;
			for (int SegmentIdx = 0; SegmentIdx < Segments.Count; SegmentIdx++)
			{
				Segment TextSegment = Segments[SegmentIdx];
				bool NextHasDiffY = SegmentIdx + 1 < Segments.Count && TextSegment.Position.Y != Segments[SegmentIdx + 1].Position.Y;
				if (TextSegment.Position.Contains(CharPoint))
				{
					return TextSegment.SourceCharIndex + AnimationUtils.SqueezeText(TextSegment.Style.Font, TextSegment.SegmentText, CharPoint.X - TextSegment.Position.Left, AnimationUtils.SqueezeTextFlags.NoWordBreak);
				}
				if (IsFuzzy)
				{
					if (TextSegment.Position.Contains(new Point(TextSegment.Position.X, CharPoint.Y)))
					{
						SelectedXIdx = ((CharPoint.X >= TextSegment.Position.Left) ? (TextSegment.SourceCharIndex + TextSegment.SourceCharLength - (NextHasDiffY ? 1 : 0)) : ((num2 == TextSegment.Position.Top) ? SelectedXIdx : TextSegment.SourceCharIndex));
						num2 = TextSegment.Position.Top;
					}
					else if (TextSegment.Position.Top <= CharPoint.Y)
					{
						SelectedYIdx = ((CharPoint.Y >= TextSegment.Position.Top) ? (TextSegment.SourceCharIndex + TextSegment.SourceCharLength - (NextHasDiffY ? 1 : 0)) : ((num == TextSegment.Position.Top) ? SelectedYIdx : TextSegment.SourceCharIndex));
						num = TextSegment.Position.Top;
					}
				}
			}
			if (IsFuzzy)
			{
				if (SelectedXIdx == -1)
				{
					return SelectedYIdx;
				}
				return SelectedXIdx;
			}
			return -1;
		}

		public Point CharFind(int CharIdx)  // Debug Function
		{
			for (int SegmentIdx = 0; SegmentIdx < Segments.Count; SegmentIdx++)
			{
				Segment TextSegment = Segments[SegmentIdx];
				if (SegmentIdx + 1 == Segments.Count || Segments[SegmentIdx + 1].SourceCharIndex > CharIdx)
				{
					int XPos = TextSegment.Position.X;
					int YPos = TextSegment.Position.Y;
					if (CharIdx > TextSegment.SourceCharIndex)
					{
						XPos += (int)UI.MeasureString(TextSegment.Style.Font, TextSegment.SegmentText.Substring(0, CharIdx - TextSegment.SourceCharIndex)).X;
					}
					return new Point(XPos, YPos);
				}
			}
			return new Point(-1, -1);
		}

		private void SkipTag(string Text, MarkupType TagMarkup, ref int CharIdx, bool ShouldClose, out TagType Type, out Color? TextColor) // The 2 references have different settings for ShouldClose so idk why thats here.
		{
			switch (Text[CharIdx])
			{
			case 'B':
			case 'b':
				if (Text[CharIdx + 1] == 'r' || Text[CharIdx + 1] == 'R')
				{
					Type = TagType.Paragraph;
				}
				else
				{
					Type = TagType.Bold;
				}
				break;
			case 'I':
			case 'i':
				Type = TagType.Italic;
				break;
			case 'C':
			case 'c':
				Type = TagType.Center;
				break;
			case 'F':
			case 'f':
				Type = TagType.Font;
				break;
			case 'R':
			case 'r':
				Type = TagType.Right;
				break;
			case 'P':
			case 'p':
				Type = TagType.Paragraph;
				break;
			case 'H':
			case 'h':
				Type = TagType.Highlighted;
				break;
			case 'T':
			case 't':
				Type = TagType.FontTitle;
				break;
			default:
				Type = TagType.None;
				break;
			}
			while (Text[CharIdx] != '=' && Text[CharIdx] != ' ' && Text[CharIdx] != '>')
			{
				CharIdx++;
			}
			if (Type == TagType.Font && TagMarkup == MarkupType.Html && Text[CharIdx] == ' ' && Text[CharIdx + 1] == 'c')
			{
				while (Text[CharIdx] != '=' && Text[CharIdx] != '>')
				{
					CharIdx++;
				}
			}
			TextColor = null;
			if (Type == TagType.Font && Text[CharIdx] == '=')
			{
				CharIdx++;
				TextColor = AnimationUtils.StringToColor(PopQuotedWord(Text, ref CharIdx, '>', DoStripQuotes: true));
			}
			CharIdx++;
		}

		private string PopQuotedWord(string Text, ref int CharIdx, char EndChar, bool DoStripQuotes)
		{
			int CurrentStartIdx = CharIdx;
			char TextChar = Text[CharIdx];
			string QuotedWord;
			if (TextChar == '"' || TextChar == '\'')
			{
				do
				{
					CharIdx++;
				}
				while (CharIdx < Text.Length && Text[CharIdx] != TextChar);
				CharIdx++;
				QuotedWord = ((!DoStripQuotes) ? Text.Substring(CurrentStartIdx, CharIdx - CurrentStartIdx) : Text.Substring(CurrentStartIdx + 1, CharIdx - CurrentStartIdx - 2));
			}
			else
			{
				while (CharIdx < Text.Length && Text[CharIdx] != EndChar && !IsWhiteEol(Text[CharIdx]))
				{
					CharIdx++;
				}
				QuotedWord = Text.Substring(CurrentStartIdx, CharIdx - CurrentStartIdx);
			}
			while (CharIdx < Text.Length && IsWhiteEol(Text[CharIdx]))
			{
				CharIdx++;
			}
			return QuotedWord;
		}

		private void AddSegment(string Text, int StartIndex, int TextLength, Style TextStyle, int BaseX, int BaseY, int ClipX, int ClipY)
		{
			Segment TextSegment = new Segment
			{
				SegmentText = Text.Substring(StartIndex, TextLength),
				SourceCharIndex = (short)StartIndex,
				SourceCharLength = (short)TextLength,
				Position = new Rectangle(BaseX, BaseY, ClipX, ClipY),
				Style = TextStyle
			};
			Segments.Add(TextSegment);
			Width = (short)Math.Max(Width, TextSegment.Position.Right + WidthOffset);
			Height = (short)Math.Max(Height, TextSegment.Position.Bottom + HeightOffset);
		}

		private bool IsWhiteEol(char TextChar)
		{
			if (TextChar != ' ' && TextChar != '\t' && TextChar != '\r')
			{
				return TextChar == '\n';
			}
			return true;
		}

		private bool IsEol(char TextChar)
		{
			if (TextChar != '\r')
			{
				return TextChar == '\n';
			}
			return true;
		}
	}
}
