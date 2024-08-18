#if !USE_ORIGINAL_CODE
using System;
using System.Collections.Generic;

namespace Terraria.Achievements
{
	public class AchievementDetails
	{
		public List<AchievementSystem.TerrariaAchievement> Details = new List<AchievementSystem.TerrariaAchievement>();
		public AchievementDetails()
		{
			Details.Add(new AchievementSystem.TerrariaAchievement("Terraria Expert", "You have completed the tutorial!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Terraria Student", "You have begun the tutorial!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Home Sweet Home", "The Guide has moved in!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("All in the Family", "Every NPC has moved in!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Rock Bottom", "You have reached the bottom of the World!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Exterminator", "You have defeated every boss!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Slimer", "You have killed every type of slime!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Challenge Accepted", "You have unlocked Hard Mode!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Maxed Out", "You have the maximum health and mana!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Corruptible", "Your world is corrupt!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Hallowed Be Thy Name", "Your world is hallowed!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Ophthalmologist", "You have defeated The Twins!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Bona Fide", "You have defeated Skeletron Prime!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Ride The Worm", "You have defeated The Destroyer!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Marathon Runner", "You have traveled over 42km on the ground!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Landscaper", "You have removed more than 10,000 blocks!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Crowd Control", "You have defeated the Goblin Army!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Survivor", "You survived the first night!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Icarus", "You have reached the top of the world!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Vanity of Vanities", "Looking good!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Pet Hoarder", "You seem to like the pets.", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Be Prepared", "You are ready for battle!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Airtime", "Enjoy the view.", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Blacksmith", "You are a master smith!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("I'm Smelting!", "You have smelted 10,000 bars of metal!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("A Knight in Shining Armors", "Obtain every type of armor available.", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Engineer", "You have placed 100 wires!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Red Moon Rises", "You have survived the Blood Moon!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Crafty", "You have used every crafting station!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("To Hell and Back", "You have gone to The Underworld and back without dying!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Back for Seconds", "You have defeated all the prime bosses twice!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Is This Heaven?", "You have found a floating island!", false, DateTime.MinValue));
			// The below one was originally called "Leap a tall building in a single bound", how boring...
			Details.Add(new AchievementSystem.TerrariaAchievement("Superman Jump", "Jump really, really high.", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Safe Fall", "You have landed safely.", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Hellevator", "Go from the surface to The Underworld in under a minute.", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Gone in 60 seconds", "You ran continuously for 60 seconds!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Appease the Volcano Gods", "You sacrificed The Guide in boiling hot magma!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Homicidal", "You killed The Guide, you maniac!", false, DateTime.MinValue));
			Details.Add(new AchievementSystem.TerrariaAchievement("Completionist", "All accomplishments have been unlocked!", false, DateTime.MinValue));
		}
	}
}
#endif