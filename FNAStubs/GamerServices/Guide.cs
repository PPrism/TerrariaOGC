#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using SDL3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms; // Would like this to be removed and just have SDL do it but with colours.
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public static class Guide
	{
		#region Public Static Properties

		public static bool IsScreenSaverEnabled
		{
			// FIXME: Should we use SDL here? -flibit
			get
			{
				return SDL.SDL_ScreenSaverEnabled();
			}
			set
			{
				if (value)
				{
					SDL.SDL_EnableScreenSaver();
				}
				else
				{
					SDL.SDL_DisableScreenSaver();
				}
			}
		}

		public static bool IsTrialMode
		{
			get;
			set;
		}

		public static bool IsVisible
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public static NotificationPosition NotificationPosition
		{
			get
			{
				return position;
			}
			set
			{
				if (value != position)
				{
					position = value;
				}
			}
		}

		public static bool SimulateTrialMode
		{
			get;
			set;
		}

		#endregion

		#region Private Static Variables

		private static NotificationPosition position = NotificationPosition.BottomRight;

		#endregion

		#region Async Object Type

		internal class GuideAction : IAsyncResult
		{
			public object AsyncState
			{
				get;
				private set;
			}

			public bool CompletedSynchronously
			{
				get
				{
					return false;
				}
			}

			public bool IsCompleted
			{
				get;
				internal set;
			}

			public WaitHandle AsyncWaitHandle
			{
				get;
				private set;
			}

			public readonly AsyncCallback Callback;

			public GuideAction(object state, AsyncCallback callback)
			{
				AsyncState = state;
				Callback = callback;
				IsCompleted = false;
				AsyncWaitHandle = new ManualResetEvent(true);
			}
		}

		#endregion

		#region Static Constructor

		static Guide()
		{
			IsTrialMode = false;
			SimulateTrialMode = false;
		}

		#endregion

		#region Public Static Methods
		public static IAsyncResult BeginShowKeyboardInput(
			PlayerIndex player,
			string title,
			string description,
			string defaultText,
			AsyncCallback callback,
			object state
		) {
			return BeginShowKeyboardInput(
				player,
				title,
				description,
				defaultText,
				callback,
				state,
				false
			);
		}
		private static Task<string> ShowInputBox(string title, string description, string defaultText = "", bool usePasswordMode = false)
		{
			if (IsVisible)
				throw new Exception("Guide is already visible; cannot execute further");

			IsVisible = true;
			var result = InputShow(title, description, defaultText, usePasswordMode);
			IsVisible = false;
			return result;
		}

		public static IAsyncResult BeginShowKeyboardInput(
			PlayerIndex player,
			string title,
			string description,
			string defaultText,
			AsyncCallback callback,
			object state,
			bool usePasswordMode
		) {
			return ShowInputBox(title, description, defaultText, usePasswordMode);
		}

		public static string EndShowKeyboardInput(IAsyncResult result)
		{
			var State = (Task<string>)result;
			return State.Result;
		}

		private static Task<int?> ShowMessageBox(string title,
			string text,
			IEnumerable<string> buttons,
			int focusButton,
			MessageBoxIcon icon)
		{
			if (IsVisible)
				throw new Exception("Guide is already visible; cannot execute further");

			IsVisible = true;
			var buttonsList = buttons.ToList();
			if (buttonsList.Count > 3 || buttonsList.Count == 0)
				throw new ArgumentException("Invalid number of buttons: one to three required", "buttons");

			var result = MessageShow(title, text, buttonsList);
			IsVisible = false;
			return result;
		}

		public static IAsyncResult BeginShowMessageBox(
			PlayerIndex player,
			string title,
			string text,
			IEnumerable<string> buttons,
			int focusButton,
			MessageBoxIcon icon,
			AsyncCallback callback,
			object state
			) {
			return ShowMessageBox(title, text, buttons, focusButton, icon);
		}

		public static IAsyncResult BeginShowMessageBox(
			string title,
			string text,
			IEnumerable<string> buttons,
			int focusButton,
			MessageBoxIcon icon,
			AsyncCallback callback,
			object state
		) {
			return BeginShowMessageBox(PlayerIndex.One, title, text, buttons, focusButton, icon, callback, state);
		}

		public static int? EndShowMessageBox(IAsyncResult result)
		{
			var State = (Task<int?>)result;
			return State.Result;
		}

		public static void DelayNotifications(TimeSpan delay)
		{
		}

		public static void ShowComposeMessage(
			PlayerIndex player,
			string text,
			IEnumerable<Gamer> recipients
		) {
		}

		public static void ShowFriendRequest(PlayerIndex player, Gamer gamer)
		{
		}

		public static void ShowFriends(PlayerIndex player)
		{
		}

		public static void ShowGameInvite(
			PlayerIndex player,
			IEnumerable<Gamer> recipients
		) {
		}

		public static void ShowGameInvite(string sessionId)
		{
		}

		public static void ShowGamerCard(PlayerIndex player, Gamer gamer)
		{
		}

		public static void ShowMarketplace(PlayerIndex player)
		{
		}

		public static void ShowMessages(PlayerIndex player)
		{
		}

		public static void ShowParty(PlayerIndex player)
		{
		}

		public static void ShowPartySessions(PlayerIndex player)
		{
		}

		public static void ShowPlayerReview(PlayerIndex player, Gamer gamer)
		{
		}

		public static void ShowPlayers(PlayerIndex player)
		{
		}

		public static void ShowSignIn(int paneCount, bool onlineOnly)
		{
		}

		public static void ShowAchievementsEXT(AchievementCollection oi)
		{
		}

		/*public class GameAchievement
		{
			public Achievement AssociatedEntry { get; set; }
			public string Title { get; set; }
			public string Description { get; set; }
			public string Icon { get; set; }
			
		}*/

		#endregion

		#region Private InputShow Method
		private static Form InputDialog;
		private static TaskCompletionSource<string> InputCompletion;

		private static Task<string> InputShow(string title, string description, string defaultText, bool usePasswordMode)
		{
			InputCompletion = new TaskCompletionSource<string>();

			var dialog = InputDialog = new Form();
			dialog.Text = title;
			dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
			dialog.MinimizeBox = false;
			dialog.MaximizeBox = false;
			dialog.ControlBox = false;
			dialog.ForeColor = System.Drawing.Color.White;
			dialog.BackColor = System.Drawing.Color.DarkGray;
			dialog.StartPosition = FormStartPosition.CenterParent;
			dialog.AutoSizeMode = AutoSizeMode.GrowOnly;
			dialog.AutoSize = true;

			var desc = new Label
			{
				Text = description,
				Parent = dialog,
				Top = 25,
				TextAlign = ContentAlignment.MiddleCenter,
				AutoSize = true,
				BackColor = System.Drawing.Color.DarkGray,
				Font = new Font(Control.DefaultFont, FontStyle.Bold)
			};

			var input = new TextBox
			{
				Text = defaultText,
				Parent = dialog,
				Top = desc.Bottom + 15,
				UseSystemPasswordChar = usePasswordMode,
				AutoSize = true,
				TabIndex = 0
			};

			var bgroup = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.LeftToRight,
				Parent = dialog,
				Top = input.Bottom + 15,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				BackColor = System.Drawing.Color.DarkGray
			};

			var button = new Button
			{
				Text = "&Done",
				DialogResult = DialogResult.OK,
				Parent = bgroup,
				TabIndex = 1,
				ForeColor = System.Drawing.Color.White,
				BackColor = System.Drawing.Color.OliveDrab,
				Font = new Font(Control.DefaultFont, FontStyle.Bold)
			};
			dialog.AcceptButton = button;

			button = new Button
			{
				Text = "&Cancel",
				DialogResult = DialogResult.Cancel,
				Parent = bgroup,
				TabIndex = 2,
				ForeColor = System.Drawing.Color.White,
				BackColor = System.Drawing.Color.OliveDrab,
				Font = new Font(Control.DefaultFont, FontStyle.Bold)
			};
			dialog.CancelButton = button;

			dialog.Width += description.Length * 2;
			dialog.Height = desc.Top + input.Top + bgroup.Top;
			desc.Left = (desc.Parent.ClientSize.Width - desc.Width) / 2;
			input.Width = (int)(input.Parent.ClientSize.Width * 0.75f);
			input.Left = (input.Parent.ClientSize.Width - input.Width) / 2;
			bgroup.Left = (bgroup.Parent.ClientSize.Width - bgroup.Width) / 2;

			var result = dialog.ShowDialog();
			InputDialog = null;

			if (result == DialogResult.OK)
				InputCompletion.SetResult(input.Text);
			else
				InputCompletion.SetResult(null);

			return InputCompletion.Task;
		}
		#endregion

		#region Private MessageShow Method
		private static Form MessageDialog;
		private static TaskCompletionSource<int?> MessageCompletion;

		private static Task<int?> MessageShow(string title, string description, List<string> buttons)
		{
			MessageCompletion = new TaskCompletionSource<int?>();

			var dialog = MessageDialog = new Form();
			dialog.Text = title;
			dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
			dialog.MinimizeBox = false;
			dialog.MaximizeBox = false;
			dialog.ControlBox = false;
			dialog.ForeColor = System.Drawing.Color.White;
			dialog.BackColor = System.Drawing.Color.DarkGray;
			dialog.StartPosition = FormStartPosition.CenterParent;
			dialog.AutoSizeMode = AutoSizeMode.GrowOnly;
			dialog.AutoSize = true;

			var desc = new Label
			{
				Text = description,
				Parent = dialog,
				Top = 25,
				TextAlign = ContentAlignment.MiddleCenter,
				AutoSize = true,
				BackColor = System.Drawing.Color.DarkGray,
				Font = new Font(Control.DefaultFont, FontStyle.Bold),
			};

			var bgroup = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.LeftToRight,
				Parent = dialog,
				Top = desc.Bottom + 25,
				AutoSize = true,
				Margin = new Padding(15),
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				BackColor = System.Drawing.Color.DarkGray
			};

			for (var i = 0; i < buttons.Count; i++)
			{
				string btext = buttons[i];
				var button = new Button
				{
					Text = btext,
					DialogResult = (DialogResult)i + 1,
					Parent = bgroup,
					TextAlign = ContentAlignment.MiddleCenter, // Currently here for the save menu, as I opted for the Xbox version and have it ask by the system. This will eventually change to in-game prompts.
					AutoSize = true,
					ForeColor = System.Drawing.Color.White,
					BackColor = System.Drawing.Color.OliveDrab,
					Font = new Font(Control.DefaultFont, FontStyle.Bold)
				};
				if (i == 0)
					dialog.AcceptButton = button;
			}

			dialog.Width += description.Length;
			dialog.Height = desc.Top + bgroup.Top;
			desc.Left = (desc.Parent.ClientSize.Width - desc.Width) / 2;
			bgroup.Left = (bgroup.Parent.ClientSize.Width - bgroup.Width) / 2;

			var result = (int)dialog.ShowDialog();
			MessageDialog = null;

			MessageCompletion.SetResult(result - 1);

			return MessageCompletion.Task;
		}
		#endregion
	}
}
