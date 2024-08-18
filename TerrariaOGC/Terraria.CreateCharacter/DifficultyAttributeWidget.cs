using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	public class DifficultyAttributeWidget : AttributeWidget<DifficultySelector>, IAttributeWidget
	{
		private readonly Action<Player, byte> Modifier;

		public static DifficultyAttributeWidget Create(Action<Player, byte> Modifier, Difficulty ResetValue, string WidgetDescription, string ControlDescription)
		{
			DifficultySelector Widget = new DifficultySelector(new Texture2D[(int)Difficulty.DIFFICULTY_COUNT]
			{
				Assets.SoftcoreIcon,
				Assets.MediumcoreIcon,
				Assets.HardcoreIcon
			}, ResetValue);
			return new DifficultyAttributeWidget(Widget, Modifier, WidgetDescription, ControlDescription);
		}

		private DifficultyAttributeWidget(DifficultySelector Widget, Action<Player, byte> Modifier, string WidgetDescription, string ControlDescription)
		{
			base.Widget = Widget;
			this.Modifier = Modifier;
			base.WidgetDescription = WidgetDescription;
			base.ControlDescription = ControlDescription;
		}

		public void Apply(Player Player)
		{
			Modifier.Invoke(Player, (byte)Widget.Selected);
		}
	}
}
