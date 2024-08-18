using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using System.Runtime.InteropServices.ComTypes;

#if !USE_ORIGINAL_CODE
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static Terraria.Tile;
#endif

namespace Terraria.Achievements
{
	public class AchievementSystem
	{
		private class EarnedAchievementsData
		{
			public EarnedAchievementsCallback Callback;

			public SignedInGamer Gamer;
		}

		private const int MaxConcurrentSubmissions = 10;

		private readonly IAsyncResult[] Submissions;

		private int FirstFree;

		public const int MaxAchievementCount = (int)Trigger.NumTriggers;

#if USE_ORIGINAL_CODE
		public AchievementSystem()
		{
			Submissions = new IAsyncResult[MaxConcurrentSubmissions];
			FirstFree = 0;
		}

		private static void ProcessEarnedAchievements(IAsyncResult Result)
		{
			EarnedAchievementsData EarnedAchievements = (EarnedAchievementsData)Result.AsyncState;
			BitArray BitArray = new BitArray(MaxAchievementCount + (MaxConcurrentSubmissions - 1));
			try
			{
				AchievementCollection AchievementCollection = EarnedAchievements.Gamer.EndGetAchievements(Result);
				foreach (Microsoft.Xna.Framework.GamerServices.Achievement Achievement in AchievementCollection)
				{
					int AchievementIdx = -1;
					try
					{
						AchievementIdx = (int)Enum.Parse(typeof(Achievement), Achievement.Key);
					}
					catch (ArgumentException)
					{
						continue;
					}
					BitArray.Set(AchievementIdx, Achievement.IsEarned);
				}
			}
			catch
			{
			}
			EarnedAchievements.Callback(BitArray);
		}

		public void GetEarnedAchievements(SignedInGamer Gamer, EarnedAchievementsCallback Callback)
		{
			EarnedAchievementsData EarnedAchievements = new EarnedAchievementsData();
			EarnedAchievements.Callback = Callback;
			EarnedAchievements.Gamer = Gamer;
			Gamer.BeginGetAchievements(ProcessEarnedAchievements, EarnedAchievements);
		}

		public void Award(SignedInGamer Gamer, Achievement Achievement)
		{
			if (Achievement < Achievement.BackForSeconds) // BackForSeconds and beyond are not featured in the XBLA version
			{
				IAsyncResult AsyncResult = null;
				try
				{
					AsyncResult = Gamer.BeginAwardAchievement(Achievement.ToString(), null, null);
				}
				catch (ArgumentException)
				{
					return;
				}
				Submissions[FirstFree] = AsyncResult;
				FirstFree++;
			}
		}

		public void Update()
		{
			int Submission = 0;
			while (Submission < FirstFree)
			{
				if (!Submissions[Submission].IsCompleted)
				{
					Submission++;
					continue;
				}
				FirstFree--;
				Submissions[Submission] = Submissions[FirstFree];
				Submissions[FirstFree] = null;
			}
		}
#else
		private static string SavePath;

		private static readonly byte[] EncryptionKey = Encoding.ASCII.GetBytes("ENGINE505RELOGIC");

		public struct TerrariaAchievement
		{
			public string Name, Description;
			public bool IsEarned, EarnedOnline;
			public DateTime EarnedDate;
			public byte GamerScore;

			public TerrariaAchievement(string name, string description, bool earned, DateTime earnedDateTime)
			{
				Name = name;
				Description = description;
				IsEarned = earned;
				EarnedDate = earnedDateTime;
				EarnedOnline = false; // Got these 2 just for the hell of it. Might do something with it later.
				GamerScore = 0;
			}

			public void Award(bool EarnStatus, DateTime When) // "'When' what?" "When did I ask?"
			{
				IsEarned = EarnStatus;
				EarnedDate = When;
			}

			public bool GetStatus()
			{
				return IsEarned;
			}
		}

		private static Dictionary<Achievement, TerrariaAchievement> AchievementsList = new Dictionary<Achievement, TerrariaAchievement>();

		public AchievementSystem()
		{
			Submissions = new IAsyncResult[MaxConcurrentSubmissions];
			FirstFree = 0;
			AchievementDetails Setup = new AchievementDetails();
			for (int i = 0; i < (int)Trigger.NumTriggers; i++)
			{
				AchievementsList.Add((Achievement)i, Setup.Details[i]);
			}
		}

		public void SaveAchievements()
		{
			SavePath = string.Format("Achievements-{0}.dat", UI.MainUI.SignedInGamer.Gamertag);

			if (!UI.MainUI.HasPlayerStorage())
			{
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged().CreateEncryptor(EncryptionKey, EncryptionKey), CryptoStreamMode.Write))
				{
					BsonWriter val = new BsonWriter(cryptoStream);
					try
					{
						JsonSerializer.Create(new JsonSerializerSettings()).Serialize((JsonWriter)(object)val, AchievementsList);
						val.Flush();
						cryptoStream.FlushFinalBlock();
						Main.ShowSaveIcon();

						if (UI.MainUI.TestStorageSpace("Achievements", SavePath, (int)memoryStream.Length))
						{
							using (StorageContainer storageContainer = UI.MainUI.OpenPlayerStorage("Achievements"))
							{
								using (Stream stream = storageContainer.OpenFile(SavePath, FileMode.Create))
								{
									stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
									stream.Close();
								}
							}
						}
					}
					catch (IOException)
					{
						UI.MainUI.WriteError();
					}
					catch (Exception)
					{
					}
					finally
					{
						cryptoStream.Close();
						Main.HideSaveIcon();
						if (val != null)
						{
							((IDisposable)val).Dispose();
						}
					}
				}
			}
		}

		public static void LoadAchievements()
		{
			SavePath = string.Format("Achievements-{0}.dat", Main.Gamertag);

			try
			{
				using (StorageContainer storageContainer = UI.MainUI.OpenPlayerStorage("Achievements"))
				{
					if (storageContainer.FileExists(SavePath))
					{
						try
						{
							using (Stream stream = storageContainer.OpenFile(SavePath, FileMode.Open))
							{
								using (CryptoStream cryptoStream = new CryptoStream(stream, new RijndaelManaged().CreateDecryptor(EncryptionKey, EncryptionKey), CryptoStreamMode.Read))
								{
									BsonReader val = new BsonReader(cryptoStream);
									try
									{
										AchievementsList = JsonSerializer.Create().Deserialize<Dictionary<Achievement, TerrariaAchievement>>((JsonReader)(object)val);
									}
									finally
									{
										if (val != null)
										{
											((IDisposable)val).Dispose();
										}
									}
								}
								stream.Close();
							}
						}
						catch (InvalidOperationException)
						{
							Main.ShowSaveIcon();
							storageContainer.DeleteFile(SavePath);
							Main.HideSaveIcon();
						}
						catch (Exception)
						{
						}
					}
				}
			}
			catch (IOException)
			{
				UI.MainUI.ReadError();
			}
			catch (Exception)
			{
			}
		}

		private static BitArray ProcessEarnedAchievements()
		{
			LoadAchievements();
			BitArray BitArray = new BitArray(MaxAchievementCount + (MaxConcurrentSubmissions - 1));
			try
			{
				foreach (KeyValuePair<Achievement, TerrariaAchievement> Single in AchievementsList)
				{
					int AchievementIdx = -1;
					
					try
					{
						AchievementIdx = (int)Single.Key;
					}
					catch (ArgumentException)
					{
						continue;
					}
					BitArray.Set(AchievementIdx, Single.Value.GetStatus());
				}
			}
			catch
			{
			}
			return BitArray;
		}

		public BitArray GetEarnedAchievements()
		{
			return ProcessEarnedAchievements();
		}

		public Dictionary<Achievement, TerrariaAchievement> GetCurrentAchievements()
		{
			return AchievementsList;
		}

		public void Award(SignedInGamer Gamer, Achievement AwardedAchievement)
		{
			if (AwardedAchievement < Achievement.AchievementCount)
			{
				TerrariaAchievement value = AchievementsList[AwardedAchievement];
				value.Award(true, DateTime.Today);
				AchievementsList[AwardedAchievement] = value;
				Main.PlaySound(24);
				Main.NewText("Achievement Unlocked: " + value.Name, 255, 200, 62);

				bool IsNowCompletionist = true;
				for (int i = 0; i < (int)Achievement.AchievementCount - 1; i++)
				{
					if (!AchievementsList[(Achievement)i].IsEarned)
					{
						IsNowCompletionist = false;
						break;
					}
				}
				if (IsNowCompletionist)
				{
					value = AchievementsList[Achievement.Completionist];
					value.Award(true, DateTime.Today);
					AchievementsList[Achievement.Completionist] = value;
					Main.NewText("Congratulations, you have unlocked all the achievements!", 255, 200, 62);
				}
				SaveAchievements();
			}
		}

		public void Update()
		{
			int Submission = 0;
			while (Submission < FirstFree)
			{
				if (!Submissions[Submission].IsCompleted)
				{
					continue;
				}
			}
		}
#endif
		}
}
