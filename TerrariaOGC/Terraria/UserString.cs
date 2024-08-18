using System;
using System.IO;
using Microsoft.Xna.Framework.GamerServices;

namespace Terraria
{
	public sealed class UserString
	{
		public string UserText;

		public bool IsVerified;

		public bool IsCensored;

		private IAsyncResult AsyncResult;

		public static implicit operator UserString(string String)
		{
			return new UserString(String, NowVerified: true);
		}

		public UserString(string String, bool NowVerified)
		{
			UserText = String;
			IsVerified = NowVerified;
			IsCensored = false;
			AsyncResult = null;
		}

		public UserString(BinaryReader Input)
		{
			int StrByte = Input.ReadByte();
			IsVerified = (StrByte & 1) != 0;
			IsCensored = (StrByte & 2) != 0;
			if (IsCensored)
			{
				UserText = null;
			}
			else
			{
				UserText = Input.ReadString();
			}
			AsyncResult = null;
		}

		public void Write(BinaryWriter Output)
		{
			int Verified = (IsVerified ? 1 : 0);
			if (IsCensored)
			{
				Verified |= 2;
			}
			Output.Write((byte)Verified);
			if (!IsCensored)
			{
				Output.Write(UserText);
			}
		}

		public void SetUserString(string String)
		{
			UserText = String;
			IsVerified = String.Length == 0;
			IsCensored = false;
		}

		public void SetSystemString(string String)
		{
			UserText = String;
			IsVerified = true;
			IsCensored = false;
		}

		public string GetString()
		{
#if USE_ORIGINAL_CODE
			if (!IsVerified && AsyncResult == null && Main.NetMode > (byte)NetModeSetting.LOCAL)
			{
				AsyncResult = StringChecker.BeginCheckString(UserText, OnCheckStringDone, null);
            }
#endif
			if (AsyncResult != null)
			{
				return Lang.InterfaceText[76];
			}
			if (!IsCensored)
			{
				return UserText;
			}
			return Lang.InterfaceText[77];
		}

		public bool IsEditable()
		{
			if (!IsVerified)
			{
				return Main.NetMode == (byte)NetModeSetting.LOCAL;
			}
			return true;
		}

		private void OnCheckStringDone(object String)
		{
			try
			{
#if USE_ORIGINAL_CODE
				IsCensored = !StringChecker.EndCheckString(AsyncResult);
#else
				IsCensored = false;
#endif
				IsVerified = true;
			}
			catch
			{
			}
			AsyncResult = null;
		}
	}
}
