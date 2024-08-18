namespace Terraria
{
	public struct ControlDesc
	{
		public int Alignment;

		public ushort X;

		public ushort Y;

		public string Label;

		public ControlDesc(int LabelAlignment, int LabelX, int LabelY, string Text)
		{
			Alignment = LabelAlignment;
			X = (ushort)LabelX;
			Y = (ushort)LabelY;
			Label = Text;
		}
	}
}
