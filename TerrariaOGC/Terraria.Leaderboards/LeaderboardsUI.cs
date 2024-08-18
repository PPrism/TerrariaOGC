using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terraria.Leaderboards
{
	internal class LeaderboardsUI
	{
		private enum LeaderboardMode
		{
			Overall,
			MyScore,
			Friends
		}

		public const uint MaxSupportedNum = 99999999u;

		public string MaxSupportedNumStr = MaxSupportedNum.ToString(); // Was previously defined as a constant "99999999"; this way, it matches with any changes automatically.

#if USE_ORIGINAL_CODE
		private const int RowHeight = 50, RowWidth = 900, RowSpacing = 5, BackgroundX = 20,
			RankX = 80, GamertagX = 250, StatsX = 375, StatsWidth = 550, MaxRowsPerScreen = 7;
#else
		private const int RowSpacing = 5;

		private static readonly int RowHeight = (int)(50 * Main.ScreenMultiplier),
			RowWidth = (int)(900 * Main.ScreenMultiplier),
			BackgroundX = (int)(20 * Main.ScreenMultiplier),
			RankX = (int)(80 * Main.ScreenMultiplier),
			GamertagX = (int)(250 * Main.ScreenMultiplier),
			StatsX = (int)(375 * Main.ScreenMultiplier),
			StatsWidth = (int)(550 * Main.ScreenMultiplier),
			MaxRowsPerScreen = 7;
#endif

		private const Leaderboard DefLeaderboard = Leaderboard.DISTANCE;

#if VERSION_INITIAL
		private static Color RowColour = UI.DefaultDialogColor;

		private static Color SelectedRowColour = Color.White;

		private readonly BoxGraphic Box;
#endif

		private readonly LeaderboardData Data;

		private readonly UI ParentUI;

		private int UIDelay;

		private LeaderboardMode Mode;

		private Leaderboard SelectedLeaderboard;

		private int[] StatsPositions, IconPositions;

		private Texture2D[] IconIndices;

		public LeaderboardsUI(UI ParentUI)
		{
			Data = new LeaderboardData(MaxRowsPerScreen);
#if VERSION_INITIAL
			Box = BoxGraphic.Create(RowWidth, RowHeight, HowToPlay.Assets.TextBG, 8, RowColour);
#endif
			UIDelay = 0;
			this.ParentUI = ParentUI;
			SelectedLeaderboard = DefLeaderboard;
		}

		public void InitializeData()
		{
			Mode = LeaderboardMode.Overall;
			SelectedLeaderboard = DefLeaderboard;
			LoadLeaderboard();
		}

		private void CalculateStatisticPositions()
		{
			SpriteFont Font = UI.BoldSmallFont;
			float StringX = UI.MeasureString(Font, MaxSupportedNumStr).X;
			Column[] ColumnMetrics = Data.Columns;
			int ColumnCount = ColumnMetrics.Length;
			float ColumnPos = (StatsWidth / ColumnCount - StringX) * 0.5f;
			StatsPositions = new int[ColumnCount];
			IconIndices = new Texture2D[ColumnCount];
			IconPositions = new int[ColumnCount];
			float StatsXPos = StatsX;

			for (int i = 0; i < ColumnCount; i++)
			{
				StatsPositions[i] = (int)(StatsXPos + ColumnPos + StringX * 0.5f);
				StatsXPos += StringX + ColumnPos * 2f;
				Column ColumnMetric = ColumnMetrics[i];
				IconIndices[i] = Assets.ColumnIcons[(int)ColumnMetric];
#if USE_ORIGINAL_CODE
				IconPositions[i] = StatsPositions[i] - (IconIndices[i].Width >> 1);
#else
				IconPositions[i] = StatsPositions[i] - ((int)(IconIndices[i].Width * Main.ScreenMultiplier) >> 1);
#endif
			}
		}

		public void Update()
		{
			if (!ParentUI.HasOnline() || !Data.Update())
			{
				MessageBox.Show(ParentUI.controller, Lang.MenuText[5], Lang.InterfaceText[36], new string[1] { Lang.MenuText[90] });
				ParentUI.PrevMenu();
			}
			else if (UIDelay > 0)
			{
				UIDelay--;
			}
            else if (Data.Ready)
			{
				bool MoveReq = false;
				if (ParentUI.IsDownButtonDown())
				{
					MoveReq = Data.MoveDown();
				}
				else if (ParentUI.IsUpButtonDown())
				{
					MoveReq = Data.MoveUp();
				}
				else if (ParentUI.IsButtonTriggered(Buttons.X) && !ParentUI.SignedInGamer.IsGuest)
				{
					SwitchLeaderboardFilter();
				}
				else if (ParentUI.IsButtonTriggered(UI.BTN_PREV_ITEM))
				{
					PreviousLeaderboard();
				}
				else if (ParentUI.IsButtonTriggered(UI.BTN_NEXT_ITEM))
				{
					NextLeaderboard();
				}
				else if (ParentUI.IsButtonTriggered(Buttons.A) && ParentUI.CanViewGamerCard() && Data.NumEntries > 0)
				{
					ParentUI.ShowGamerCard(Data.GetSelectedGamer());
				}
				if (MoveReq)
				{
					UIDelay = 12;
					Main.PlaySound(12);
				}
			}
		}

		private void NextLeaderboard()
		{
			int SelectedLeaderboard = (int)this.SelectedLeaderboard;
			SelectedLeaderboard++;
			if (SelectedLeaderboard == 5)
			{
				SelectedLeaderboard = 0;
			}
			this.SelectedLeaderboard = (Leaderboard)SelectedLeaderboard;
			LoadLeaderboard();
		}

		private void PreviousLeaderboard()
		{
			int SelectedLeaderboard = (int)this.SelectedLeaderboard;
			SelectedLeaderboard--;
			if (SelectedLeaderboard < 0)
			{
				SelectedLeaderboard = 4;
			}
			this.SelectedLeaderboard = (Leaderboard)SelectedLeaderboard;
			LoadLeaderboard();
		}

		private void LoadLeaderboard()
		{
			switch (Mode)
			{
			case LeaderboardMode.MyScore:
				Data.LoadLeaderboard(SelectedLeaderboard, ParentUI.SignedInGamer);
				break;
            case LeaderboardMode.Friends:
			{
				SignedInGamer CurrentGamer = ParentUI.SignedInGamer;
				FriendCollection Friends = CurrentGamer.GetFriends();
				Data.LoadLeaderboard(SelectedLeaderboard, Friends, CurrentGamer);
				break;
			}
			default:
				Data.LoadLeaderboard(SelectedLeaderboard);
				break;
			}
			CalculateStatisticPositions();
		}

		private void SwitchLeaderboardFilter()
		{
			if (Mode == LeaderboardMode.Overall)
			{
				Mode = LeaderboardMode.MyScore;
			}
			else if (Mode == LeaderboardMode.MyScore)
			{
				Mode = LeaderboardMode.Friends;
			}
			else if (Mode == LeaderboardMode.Friends)
			{
				Mode = LeaderboardMode.Overall;
			}
			LoadLeaderboard();
		}

		public void Draw(WorldView View)
		{
			SpriteFont FontSmallOutline = UI.BoldSmallFont;
			int SafeAreaOffsetLeft = View.SafeAreaOffsetLeft;
			int SafeAreaOffsetTop = View.SafeAreaOffsetTop;
			Main.StrBuilder.Length = 0;
			Main.StrBuilder.Append(Lang.InterfaceText[37]);
			Main.StrBuilder.Append(Data.NumEntries);
			Main.StrBuilder.Append('\n');
			Main.StrBuilder.Append(Data.LeaderboardName);
			UI.DrawStringLT(FontSmallOutline, SafeAreaOffsetLeft, SafeAreaOffsetTop, Color.White);

#if USE_ORIGINAL_CODE
			Vector2 Vector = new Vector2(SafeAreaOffsetLeft, SafeAreaOffsetTop + 10);
			for (int i = 0; i < IconIndices.Length; i++)
			{
				Vector.X = IconPositions[i];
				Main.SpriteBatch.Draw(IconIndices[i], Vector, Color.White);
			}
			Vector.Y = View.SafeAreaOffsetTop + 95;

			LeaderboardData.Row[] Rows = Data.GetRows();
			for (int Index = 0; Index < Rows.Length; Index++)
			{
				int Selection = Index + Data.BatchStart;
				Color Colour = (Selection == Data.Selected) ? SelectedRowColour : RowColour;
				Box.Color = Colour;
				Box.Draw(new Vector2i(BackgroundX, (int)Vector.Y - 25), 1f);

				LeaderboardData.Row Row = Rows[Index];
				if (Row != null && Row.Available)
				{
					Vector.X = RankX;
					string RankText = Row.Rank.ToStringLookup();
					Vector2 Pivot = UI.MeasureString(FontSmallOutline, RankText);
					Pivot.X = (int)Pivot.X >> 1;
					Pivot.Y = (int)Pivot.Y >> 1;
					UI.DrawStringScaled(FontSmallOutline, RankText, Vector, Color.White, Pivot, 1f);

					Vector.X = GamertagX;
                    string UserGamertag = Row.Gamertag;
					Pivot = UI.MeasureString(FontSmallOutline, UserGamertag);
					Pivot.X = (int)Pivot.X >> 1;
					Pivot.Y = (int)Pivot.Y >> 1;
					UI.DrawStringScaled(FontSmallOutline, UserGamertag, Vector, Color.White, Pivot, 1f);

					for (int Metric = Math.Min(StatsPositions.Length, Row.Statistics.Length) - 1; Metric >= 0; Metric--)
					{
						Vector.X = StatsPositions[Metric];
						uint MetricStat = Row.Statistics[Metric];
						string MetricText = ((MetricStat < MaxSupportedNum) ? ((int)MetricStat).ToStringLookup() : MaxSupportedNumStr);
						Pivot = UI.MeasureString(FontSmallOutline, MetricText);
						Pivot.X = (int)Pivot.X >> 1;
						Pivot.Y = (int)Pivot.Y >> 1;
						UI.DrawStringScaled(FontSmallOutline, MetricText, Vector, Color.White, Pivot, 1f);
					}
				}
				Vector.Y += 11f * RowSpacing;
			}
#else
			Vector2 Vector = new Vector2(SafeAreaOffsetLeft, SafeAreaOffsetTop + (10f * Main.ScreenMultiplier));
			for (int i = 0; i < IconIndices.Length; i++)
			{
				Vector.X = IconPositions[i];
				Main.SpriteBatch.Draw(IconIndices[i], Vector, null, Color.White, 0f, default, Main.ScreenMultiplier, SpriteEffects.None, 0f);
			}
			Vector.Y = View.SafeAreaOffsetTop + (95 * Main.ScreenMultiplier);

			LeaderboardData.Row[] Rows = Data.GetRows();
			for (int Index = 0; Index < Rows.Length; Index++)
			{
				int Selection = Index + Data.BatchStart;
#if VERSION_INITIAL // I know its currently only usable with original code, but here are the 1.01 differences anyway.
				Color Colour = (Selection == Data.Selected) ? SelectedRowColour : RowColour;
				Box.Color = Colour;
				Box.Draw(new Vector2i(BackgroundX, (int)(Vector.Y - (25 * Main.ScreenMultiplier))), 1f);
#else
				int TexID = (Selection == Data.Selected) ? (int)_sheetSprites.ID.INVENTORY_BACK10: (int)_sheetSprites.ID.INVENTORY_BACK;
				Main.DrawRect(TexID, new Rectangle(BackgroundX + 8, (int)(Vector.Y - (25 * Main.ScreenMultiplier)) + 8, RowWidth - 16, RowHeight - 16), 192);
#endif

				LeaderboardData.Row Row = Rows[Index];
				if (Row != null && Row.Available)
				{
					Vector.X = RankX;
					string RankText = Row.Rank.ToStringLookup();
					Vector2 Pivot = UI.MeasureString(FontSmallOutline, RankText);
					Pivot.X = (int)Pivot.X >> 1;
					Pivot.Y = (int)Pivot.Y >> 1;
					UI.DrawStringScaled(FontSmallOutline, RankText, Vector, Color.White, Pivot, 1f);

					Vector.X = GamertagX;
					string UserGamertag = Row.Gamertag;
					Pivot = UI.MeasureString(FontSmallOutline, UserGamertag);
					Pivot.X = (int)Pivot.X >> 1;
					Pivot.Y = (int)Pivot.Y >> 1;
					UI.DrawStringScaled(FontSmallOutline, UserGamertag, Vector, Color.White, Pivot, 1f);

					for (int Metric = Math.Min(StatsPositions.Length, Row.Statistics.Length) - 1; Metric >= 0; Metric--)
					{
						Vector.X = StatsPositions[Metric];
						uint MetricStat = Row.Statistics[Metric];
						string MetricText = ((MetricStat < MaxSupportedNum) ? ((int)MetricStat).ToStringLookup() : MaxSupportedNumStr);
						Pivot = UI.MeasureString(FontSmallOutline, MetricText);
						Pivot.X = (int)Pivot.X >> 1;
						Pivot.Y = (int)Pivot.Y >> 1;
						UI.DrawStringScaled(FontSmallOutline, MetricText, Vector, Color.White, Pivot, 1f);
					}
				}
				Vector.Y += 11f * RowSpacing * Main.ScreenMultiplier;
			}
#endif
		}

		public void ControlDescription(StringBuilder StrBuilder)
		{
#if !VERSION_INITIAL
			StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
			StrBuilder.Append(' ');
#endif
			StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SWITCH_LEADERBOARD));
			StrBuilder.Append(' ');
			if (ParentUI.CanViewGamerCard() && Data.NumEntries > 0)
			{
				StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_GAMERCARD));
				StrBuilder.Append(' ');
			}

#if VERSION_INITIAL
			StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
			if (!ParentUI.SignedInGamer.IsGuest)
			{
				StrBuilder.Append(' ');
#else
			if (!ParentUI.SignedInGamer.IsGuest)
			{
#endif
				if (Mode == LeaderboardMode.Friends)
				{
					StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_TOP));
				}
				else if (Mode == LeaderboardMode.Overall)
				{
					StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_MYSELF));
				}
				else if (Mode == LeaderboardMode.MyScore)
				{
					StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_FRIENDS));
				}
			}
		}
	}
}
