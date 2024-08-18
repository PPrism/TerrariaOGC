using Microsoft.Xna.Framework;

namespace Terraria.CreateCharacter
{
	public abstract class AttributeWidget<T> where T : ISelector
	{
		protected T Widget;

		public string WidgetDescription
		{
			get;
			protected set;
		}

		public string ControlDescription
		{
			get;
			protected set;
		}

		internal AttributeWidget()
		{
		}

		public virtual void Draw(Vector2 Position, float Alpha)
		{
			Widget.Draw(Position, Alpha);
		}

		public void Update()
		{
			Widget.Update();
		}

		public bool SelectLeft()
		{
			return Widget.SelectLeft();
		}

		public bool SelectRight()
		{
			return Widget.SelectRight();
		}

		public bool SelectUp()
		{
			return Widget.SelectUp();
		}

		public bool SelectDown()
		{
			return Widget.SelectDown();
		}

		public virtual void SetCursor(Vector2i CursorPosition)
		{
			Widget.SetCursor(CursorPosition);
		}

		public void Reset()
		{
			Widget.Reset();
		}

		public void Back()
		{
			Widget.CancelSelection();
		}

		public void Show()
		{
			Widget.Show();
		}

		public void FlashSelection(int duration)
		{
			Widget.FlashSelection(duration);
		}
	}
}
