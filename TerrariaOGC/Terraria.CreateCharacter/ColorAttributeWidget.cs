using System;
using Microsoft.Xna.Framework;

namespace Terraria.CreateCharacter
{
	public class ColorAttributeWidget : AttributeWidget<ColorSelector>, IAttributeWidget
	{
		private readonly Action<Player, Color> Modifier;

		public static ColorAttributeWidget Create(Action<Player, Color> Modifier, Vector2i ResetValue, string WidgetDescription, string ControlDescription)
		{
			ColorSelector Widget = new ColorSelector(Assets.Palette, Assets.SelectedColorContainer, ResetValue);
			return new ColorAttributeWidget(Widget, Modifier, WidgetDescription, ControlDescription);
		}

		private ColorAttributeWidget(ColorSelector Widget, Action<Player, Color> Modifier, string WidgetDescription, string ControlDescription)
		{
			base.Widget = Widget;
			this.Modifier = Modifier;
			base.WidgetDescription = WidgetDescription;
			base.ControlDescription = ControlDescription;
		}

		public void Apply(Player player)
		{
			Modifier.Invoke(player, Widget.SelectedColor);
		}
	}
}
