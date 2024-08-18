using Microsoft.Xna.Framework;

namespace Terraria.CreateCharacter
{
	public interface ISelector
	{
		void Draw(Vector2 Position, float Alpha);

		void Update();

		bool SelectLeft();

		bool SelectRight();

		bool SelectUp();

		bool SelectDown();

		void SetCursor(Vector2i CursorVect);

		void Reset();

		void Show();

		void FlashSelection(int Duration);

		void CancelSelection();
	}
}
