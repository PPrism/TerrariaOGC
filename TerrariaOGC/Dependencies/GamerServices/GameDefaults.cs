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
	public sealed class GameDefaults
	{
		#region Public Properties

		public GameDifficulty GameDifficulty
		{
			get;
			private set;
		}

		public ControllerSensitivity ControllerSensitivity
		{
			get;
			private set;
		}

		public Color? PrimaryColor
		{
			get;
			private set;
		}

		public Color? SecondaryColor
		{
			get;
			private set;
		}

		public bool AutoAim
		{
			get;
			private set;
		}

		public bool AutoCenter
		{
			get;
			private set;
		}

		public bool MoveWithRightThumbStick
		{
			get;
			private set;
		}

		public bool InvertYAxis
		{
			get;
			private set;
		}

		public bool ManualTransmission
		{
			get;
			private set;
		}

		public RacingCameraAngle RacingCameraAngle
		{
			get;
			private set;
		}

		public bool AccelerateWithButtons
		{
			get;
			private set;
		}

		public bool BrakeWithButtons
		{
			get;
			private set;
		}

		#endregion

		#region Internal Constructor

		internal GameDefaults()
		{
			// FIXME: This is one huge joke. -ade
		}

		#endregion
	}
}
