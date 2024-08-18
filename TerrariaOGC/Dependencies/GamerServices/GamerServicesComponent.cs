#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public class GamerServicesComponent : GameComponent
	{
		#region Public Constructor

		public GamerServicesComponent(Game game) : base(game)
		{
		}

		#endregion

		#region Public Methods

		public override void Initialize()
		{
			GamerServicesDispatcher.WindowHandle = Game.Window.Handle;
			GamerServicesDispatcher.Initialize(Game.Services);
		}

		public override void Update(GameTime gameTime)
		{
			GamerServicesDispatcher.Update();
		}

		#endregion
	}
}
