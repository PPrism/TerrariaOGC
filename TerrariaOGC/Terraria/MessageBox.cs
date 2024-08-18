using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace Terraria
{
	public static class MessageBox
	{
		public struct Message
		{
			public bool AutoUpdate;

			public sbyte PlayerIdx;

			public string Caption;

			public string Contents;

			public string[] CurOptions;
		}

		public static IAsyncResult MessBoxResult;

		public static Message CurrentMess; // Accurate

		private static readonly List<Message> Queue;

		public static int? UserChoice;

		static MessageBox()
		{
			MessBoxResult = null;
			Queue = new List<Message>();
			CurrentMess.PlayerIdx = -1;
		}

		public static void Show(PlayerIndex Controller, string BoxCaption, string BoxContents, string[] BoxOptions, bool ShouldAutoUpdate = true)
		{
			lock (Queue)
			{
				Message MessItem = default;
				MessItem.AutoUpdate = ShouldAutoUpdate;
				MessItem.PlayerIdx = (sbyte)Controller;
				MessItem.Caption = BoxCaption;
				MessItem.Contents = BoxContents;
				MessItem.CurOptions = BoxOptions;
				if (CurrentMess.PlayerIdx < 0)
				{
					CurrentMess = MessItem;
				}
				else
				{
					if (!(CurrentMess.Contents != BoxContents))
					{
						return;
					}
					for (int QueuedPlayer = Queue.Count - 1; QueuedPlayer >= 0; QueuedPlayer--)
					{
						if (BoxContents == Queue[QueuedPlayer].Contents)
						{
							return;
						}
					}
					Queue.Add(MessItem);
					return;
				}
			}
		}

		private static void NextMessage()
		{
			if (Queue.Count > 0)
			{
				CurrentMess = Queue[0];
				Queue.RemoveAt(0);
			}
			else
			{
				CurrentMess.AutoUpdate = false;
				CurrentMess.PlayerIdx = -1;
			}
		}

		public static bool IsVisible()
		{
			return CurrentMess.PlayerIdx >= 0;
		}

		public static bool IsAutoUpdate()
		{
			return CurrentMess.AutoUpdate;
		}

		public static bool Update()
		{
			if (CurrentMess.PlayerIdx >= 0)
			{
				lock (Queue)
				{
                    if (!Guide.IsVisible)
					{
						try
						{
							MessBoxResult = Guide.BeginShowMessageBox((PlayerIndex)CurrentMess.PlayerIdx, CurrentMess.Caption, CurrentMess.Contents, CurrentMess.CurOptions, 0, MessageBoxIcon.None, null, null);
#if !USE_ORIGINAL_CODE
							if (MessBoxResult != null && MessBoxResult.IsCompleted)   // Removed asynchronicity to get it working
							{
								UserChoice = Guide.EndShowMessageBox(MessBoxResult);
								MessBoxResult = null;
								NextMessage();
								return true;
							}
#endif
						}
						catch (GuideAlreadyVisibleException)
						{
						}
					}
					else if (MessBoxResult != null && MessBoxResult.IsCompleted)
					{
						UserChoice = Guide.EndShowMessageBox(MessBoxResult);
						MessBoxResult = null;
						NextMessage();
						return true;
					}
				}
			}
			return false;
		}

		public static int GetResult()
		{
			if (!UserChoice.HasValue)
			{
				return -1;
			}
			return UserChoice.Value;
		}

		public static void RemoveMessagesFor(PlayerIndex Controller)
		{
			while ((PlayerIndex)CurrentMess.PlayerIdx == Controller)
			{
				if (MessBoxResult == null)
				{
					NextMessage();
				}
			}
			for (int QueuedPlayer = Queue.Count - 1; QueuedPlayer >= 0; QueuedPlayer--)
			{
				if ((PlayerIndex)Queue[QueuedPlayer].PlayerIdx == Controller)
				{
					Queue.RemoveAt(QueuedPlayer);
				}
			}
		}
	}
}
