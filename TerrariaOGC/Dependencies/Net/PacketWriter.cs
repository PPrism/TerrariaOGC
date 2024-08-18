#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System.IO;
#endregion

namespace Microsoft.Xna.Framework.Net
{
	public class PacketWriter : BinaryWriter
	{
		#region Public Properties

		public int Length
		{
			get
			{
				return (int) BaseStream.Length;
			}
		}

		public int Position
		{
			get
			{
				return (int) BaseStream.Position;
			}
			set
			{
				BaseStream.Position = value;
			}
		}

		#endregion

		#region Public Constructors

		public PacketWriter() : base(new MemoryStream())
		{
		}

		public PacketWriter(int capacity) : base(new MemoryStream(capacity))
		{
		}

		#endregion

		#region Public Methods

		public void Write(Color value)
		{
			base.Write(value.R);
			base.Write(value.G);
			base.Write(value.B);
			base.Write(value.A);
		}

		public void Write(Matrix value)
		{
			base.Write(value.M11);
			base.Write(value.M12);
			base.Write(value.M13);
			base.Write(value.M14);
			base.Write(value.M21);
			base.Write(value.M22);
			base.Write(value.M23);
			base.Write(value.M24);
			base.Write(value.M31);
			base.Write(value.M32);
			base.Write(value.M33);
			base.Write(value.M34);
			base.Write(value.M41);
			base.Write(value.M42);
			base.Write(value.M43);
			base.Write(value.M44);
		}

		public void Write(Quaternion value)
		{
			base.Write(value.X);
			base.Write(value.Y);
			base.Write(value.Z);
			base.Write(value.W);
		}

		public void Write(Vector2 value)
		{
			base.Write(value.X);
			base.Write(value.Y);
		}

		public void Write(Vector3 value)
		{
			base.Write(value.X);
			base.Write(value.Y);
			base.Write(value.Z);
		}

		public void Write(Vector4 value)
		{
			base.Write(value.X);
			base.Write(value.Y);
			base.Write(value.Z);
			base.Write(value.W);
		}

		// FIXME: Why do these two exist? -flibit

		public override void Write(float value)
		{
			base.Write(value);
		}

		public override void Write(double value)
		{
			base.Write(value);
		}

		#endregion
	}
}
