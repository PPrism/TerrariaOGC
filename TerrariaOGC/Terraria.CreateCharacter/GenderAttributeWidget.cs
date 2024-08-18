using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	public class GenderAttributeWidget : AttributeWidget<HorizontalListSelector>, IAttributeWidget
	{
		public enum Gender
		{
			MALE,
			FEMALE
		}

		private readonly Action<Player, bool> Modifier;

		public static GenderAttributeWidget Create(Action<Player, bool> Modifier, Gender Default, string WidgetDescription, string ControlDescription)
		{
			HorizontalListSelector Widget = new HorizontalListSelector(new Texture2D[2]
			{
				Assets.MaleIcon,
				Assets.FemaleIcon
			}, Assets.HorizontalContainer, Assets.SelectedHorizontalContainer, (Default != 0) ? 1 : 0);
			return new GenderAttributeWidget(Widget, Modifier, WidgetDescription, ControlDescription);
		}

		private GenderAttributeWidget(HorizontalListSelector Widget, Action<Player, bool> Modifier, string WidgetDescription, string ControlDescription)
		{
			base.Widget = Widget;
			this.Modifier = Modifier;
			base.WidgetDescription = WidgetDescription;
			base.ControlDescription = ControlDescription;
		}

		public void Apply(Player Player)
		{
			Modifier.Invoke(Player, Widget.Selected == 0);
		}
	}
}
