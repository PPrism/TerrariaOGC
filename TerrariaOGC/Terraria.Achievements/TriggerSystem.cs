using System.Collections;
using Microsoft.Xna.Framework.GamerServices;

namespace Terraria.Achievements
{
	public class TriggerSystem
	{
		private struct TriggerLink
		{
			public Trigger Trigger;

			public Achievement Achievement;
		}

		private readonly TriggerLink[] Links = new TriggerLink[AchievementSystem.MaxAchievementCount]
		{
			Link(Trigger.HighestPosition, Achievement.BewareIcharus),
			Link(Trigger.HouseGuide, Achievement.HomeSweetHome),
			Link(Trigger.HousedAllNPCs, Achievement.FamilyNight),
			Link(Trigger.LowestPosition, Achievement.GoingDown),
			Link(Trigger.AllSlimesKilled, Achievement.KingOfSlimes),
			Link(Trigger.AllBossesKilled, Achievement.Exterminator),
			Link(Trigger.UnlockedHardMode, Achievement.LookingForChallenge),
			Link(Trigger.MaxHealthAndMana, Achievement.AllPumpedUp),
			Link(Trigger.CorruptedWorld, Achievement.CorruptedSoul),
			Link(Trigger.HallowedWorld, Achievement.HallowedBeThyName),
			Link(Trigger.FirstTutorialTaskCompleted, Achievement.TerrariaStudent),
			Link(Trigger.AllTutorialTasksCompleted, Achievement.TerrariaExpert),
			Link(Trigger.KilledTheTwins, Achievement.Ophthalmologist),
			Link(Trigger.KilledSkeletronPrime, Achievement.Anthropologist),
			Link(Trigger.KilledDestroyer, Achievement.Biologist),
			Link(Trigger.Walked42KM, Achievement.MarathonRunner),
			Link(Trigger.RemovedLotsOfTiles, Achievement.LandscapeArchitect),
			Link(Trigger.KilledGoblinArmy, Achievement.DefeatedTheMob),
			Link(Trigger.Sunrise, Achievement.Survivor),
			Link(Trigger.SunriseAfterBloodMoon, Achievement.WhenTheMoonTurnsRed),
			Link(Trigger.AllVanitySlotsEquipped, Achievement.FashionModel),
			Link(Trigger.CreatedLotsOfBars, Achievement.Smelter),
			Link(Trigger.Has5Buffs, Achievement.PreparationIsEverything),
			Link(Trigger.SpawnedAllPets, Achievement.AnimalShelter),
			Link(Trigger.CollectedAllArmor, Achievement.Collector),
			Link(Trigger.UsedLotsOfAnvils, Achievement.Blacksmith),
			Link(Trigger.UsedAllCraftingStations, Achievement.ExpertCrafter),
			Link(Trigger.PlacedLotsOfWires, Achievement.Engineer),
			Link(Trigger.WentDownAndUpWithoutDyingOrWarping, Achievement.HellAndBack),
			Link(Trigger.InTheSky, Achievement.Airtime),
#if !USE_ORIGINAL_CODE
			Link(Trigger.BackForSeconds, Achievement.BackForSeconds),
			Link(Trigger.CouldThisBeHeaven, Achievement.CouldThisBeHeaven),
			Link(Trigger.LeapTallBuildingInSingleBound, Achievement.LeapTallBuildingInSingleBound),
			Link(Trigger.SafeFall, Achievement.SafeFall),
			Link(Trigger.Hellevator, Achievement.Hellevator),
			Link(Trigger.GoneIn60Seconds, Achievement.GoneIn60Seconds),
			Link(Trigger.OldFashioned, Achievement.OldFashioned),
			Link(Trigger.Homicidal, Achievement.Homicidal),
			Link(Trigger.MadLad, Achievement.Completionist)
#endif
		};

		private readonly bool[] Triggers = new bool[AchievementSystem.MaxAchievementCount];

		private readonly bool[] CheckTriggers = new bool[AchievementSystem.MaxAchievementCount];

		private static TriggerLink Link(Trigger Trigger, Achievement Achievement)
		{
			TriggerLink Link = default;
			Link.Trigger = Trigger;
			Link.Achievement = Achievement;
			return Link;
		}

		public bool CheckEnabled(Trigger Trigger)
		{
			return CheckTriggers[(int)Trigger];
		}

		public void SetState(Trigger Trigger, bool State)
		{
			Triggers[(int)Trigger] = State;
		}

		public void ReadProfile(SignedInGamer CurrentUser)
		{
			if (!CurrentUser.IsGuest)
			{
#if USE_ORIGINAL_CODE
				Main.AchievementSystem.GetEarnedAchievements(CurrentUser, UpdateTriggerChecks);
#else
				BitArray CurrentAchievements = Main.AchievementSystem.GetEarnedAchievements();
				UpdateTriggerChecks(CurrentAchievements);
#endif
			}
		}

		private void UpdateTriggerChecks(BitArray EarnCheck)
		{
			for (int Link = (int)Trigger.NumTriggers - 1; Link >= 0; Link--)
			{
				int Trigger = (int)Links[Link].Trigger;
				int Achievement = (int)Links[Link].Achievement;
				CheckTriggers[Trigger] = !EarnCheck.Get(Achievement);
				Triggers[Trigger] = false;
			}
		}

		public void UpdateAchievements(SignedInGamer CurrentUser)
		{
			for (int Link = (int)Trigger.NumTriggers - 1; Link >= 0; Link--)
			{
				int Trigger = (int)Links[Link].Trigger;
				Achievement Achievement = Links[Link].Achievement;
				if (!Main.IsTrial)
				{
					if (CheckTriggers[Trigger] && Triggers[Trigger])
					{
						CheckTriggers[Trigger] = false;
						Main.AchievementSystem.Award(CurrentUser, Achievement);
					}
				}
			}
		}
	}
}
