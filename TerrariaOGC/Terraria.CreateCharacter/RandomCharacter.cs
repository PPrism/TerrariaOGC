namespace Terraria.CreateCharacter
{
	internal class RandomCharacter
	{
		private enum HairGender
		{
			Male,
			Female,
			Unisex
		}

		private static Vector2i GetRandomColor(FastRandom RandomVal)
		{
			int PalWidth = Assets.Palette.Width;
			int PalUpperBound = Assets.Palette.Height >> 1;
			int PalBoundY = Assets.Palette.Height >> 2;
			return new Vector2i(RandomVal.Next(PalWidth), RandomVal.Next(PalUpperBound) + PalBoundY);
		}

		public static Vector2i[] RandCreate(FastRandom RandomVal)
		{
			HairGender[] HairArray = new HairGender[Assets.HairTypes]
			{
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Male,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Unisex,
				RandomCharacter.HairGender.Female,
				RandomCharacter.HairGender.Male
			};
			Vector2i[] SelectArr = new Vector2i[6]
			{
				new Vector2i(3, 3),
				new Vector2i(3, 3),
				new Vector2i(3, 5),
				new Vector2i(3, 7),
				new Vector2i(0, 8),
				new Vector2i(3, 0)
			};
			Vector2i[] SelectArr2 = new Vector2i[6]
			{
				new Vector2i(9, 4),
				new Vector2i(3, 8),
				new Vector2i(9, 7),
				new Vector2i(3, 8),
				new Vector2i(3, 8),
				new Vector2i(2, 4)
			};
			Vector2i[] CustomArr = new Vector2i[10];
			int HairSelect = RandomVal.Next(Assets.HairTypes);
			ref Vector2i HairSetting = ref CustomArr[1];
			HairSetting = new Vector2i(HairSelect % 9, HairSelect / 9);
			HairGender HairGender = HairArray[HairSelect];
			int Gender = -1;
			switch (HairGender)
			{
			case HairGender.Male:
				Gender = 0;
				break;
			case HairGender.Female:
				Gender = 1;
				break;
			default:
				Gender = RandomVal.Next(2);
				break;
			}
			ref Vector2i GenderSetting = ref CustomArr[0];
			GenderSetting = new Vector2i(Gender, 0);
			int Setting = RandomVal.Next(SelectArr.Length);
			ref Vector2i SkinSetting = ref CustomArr[4];
			SkinSetting = SelectArr[Setting];
			ref Vector2i EyeSetting = ref CustomArr[3];
			EyeSetting = SelectArr2[Setting];
			ref Vector2i HairColourSetting = ref CustomArr[2];
			HairColourSetting = GetRandomColor(RandomVal);
			ref Vector2i ShirtSetting = ref CustomArr[5];
			ShirtSetting = GetRandomColor(RandomVal);
			ref Vector2i UndershirtSetting = ref CustomArr[6];
			UndershirtSetting = GetRandomColor(RandomVal);
			ref Vector2i PantsSetting = ref CustomArr[7];
			PantsSetting = GetRandomColor(RandomVal);
			ref Vector2i ShoeSetting = ref CustomArr[8];
			ShoeSetting = GetRandomColor(RandomVal);
			return CustomArr;
		}
	}
}
