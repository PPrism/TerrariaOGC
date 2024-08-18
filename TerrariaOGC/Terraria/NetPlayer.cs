namespace Terraria
{
	public sealed class NetPlayer
	{
		public bool zoneDungeon;

		public bool zoneEvil;

		public bool zoneHoly;

		public bool zoneMeteor;

		public bool zoneJungle;

		public sbyte selectedItem;

		public bool controlUp;

		public bool controlDown;

		public bool controlLeft;

		public bool controlRight;

		public bool controlJump;

		public bool controlUseItem;

		public short statLifeMax;

		public short statLife;

		public short statMana;

		public short statManaMax;

		public short chest;

		public short talkNPC;

		public Item[] inventory = new Item[Player.MaxNumInventory + 1];

		public Item[] armor = new Item[Player.MaxNumArmor];

		public Buff[] buff = new Buff[Player.MaxNumBuffs];
	}
}
