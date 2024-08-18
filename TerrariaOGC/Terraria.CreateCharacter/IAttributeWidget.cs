using Microsoft.Xna.Framework;

namespace Terraria.CreateCharacter
{
	public interface IAttributeWidget
	{
		string ControlDescription
		{
			get;
		}

		string WidgetDescription
		{
			get;
		}

		void Draw(Vector2 Position, float Alpha);

		void Update();

		bool SelectLeft();

		bool SelectRight();

		bool SelectUp();

		bool SelectDown();

		void Back();

		void SetCursor(Vector2i CursorVect);

		void Apply(Player Player);

		void Reset();

		void Show();

		void FlashSelection(int Duration);
	}
}
