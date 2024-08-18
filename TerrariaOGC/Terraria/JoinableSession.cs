using Microsoft.Xna.Framework.Net;

namespace Terraria
{
	public sealed class JoinableSession
	{
		public const int SearchDelay = 5000;

		public string HostTag;

		public int PlayerCount;

        public AvailableNetworkSession AvailableSession;

		public JoinableSession(AvailableNetworkSession Session)
		{
			HostTag = Session.HostGamertag;
			PlayerCount = Session.CurrentGamerCount;
			AvailableSession = Session;
		}
	}
}
