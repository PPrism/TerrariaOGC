using System;

namespace Terraria.CreateCharacter
{
	public class HairAttributeWidget : AttributeWidget<HairSelector>, IAttributeWidget
	{
		private readonly Action<Player, int> Modifier;

		public static HairAttributeWidget Create(Action<Player, int> Modifier, Vector2i Default, string WidgetDescription, string ControlDescription)
		{
			HairSelector Widget = new HairSelector(9, Assets.Sources, Assets.HairContainer, Assets.SelectedHairContainer, Default);
			return new HairAttributeWidget(Widget, Modifier, WidgetDescription, ControlDescription);
		}

		private HairAttributeWidget(HairSelector Widget, Action<Player, int> Modifier, string WidgetDescription, string ControlDescription)
		{
			base.Widget = Widget;
			this.Modifier = Modifier;
			base.WidgetDescription = WidgetDescription;
			base.ControlDescription = ControlDescription;
		}

		public void Apply(Player Player)
		{
			Modifier.Invoke(Player, Widget.Selected);
		}
	}
}
