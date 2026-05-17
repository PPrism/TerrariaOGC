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
	public class PacketReader : BinaryReader
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

		public PacketReader() : base(new MemoryStream())
		{
		}

		public PacketReader(int capacity) : base(new MemoryStream(capacity))
		{
		}

		#endregion

		#region Public Methods

		public Color ReadColor()
		{
			// FIXME: Only using floats because of the overloads...? -flibit
			float r = ReadSingle();
			float g = ReadSingle();
			float b = ReadSingle();
			float a = ReadSingle();
			return new Color(r, g, b, a);
		}

		public Matrix ReadMatrix()
		{
			float m11 = ReadSingle();
			float m12 = ReadSingle();
			float m13 = ReadSingle();
			float m14 = ReadSingle();
			float m21 = ReadSingle();
			float m22 = ReadSingle();
			float m23 = ReadSingle();
			float m24 = ReadSingle();
			float m31 = ReadSingle();
			float m32 = ReadSingle();
			float m33 = ReadSingle();
			float m34 = ReadSingle();
			float m41 = ReadSingle();
			float m42 = ReadSingle();
			float m43 = ReadSingle();
			float m44 = ReadSingle();
			return new Matrix(
				m11, m12, m13, m14,
				m21, m22, m23, m24,
				m31, m32, m33, m34,
				m41, m42, m43, m44
			);
		}

		public Quaternion ReadQuaternion()
		{
			float x = ReadSingle();
			float y = ReadSingle();
			float z = ReadSingle();
			float w = ReadSingle();
			return new Quaternion(x, y, z, w);
		}

		public Vector2 ReadVector2()
		{
			float x = ReadSingle();
			float y = ReadSingle();
			return new Vector2(x, y);
		}

		public Vector3 ReadVector3()
		{
			float x = ReadSingle();
			float y = ReadSingle();
			float z = ReadSingle();
			return new Vector3(x, y, z);
		}

		public Vector4 ReadVector4()
		{
			float x = ReadSingle();
			float y = ReadSingle();
			float z = ReadSingle();
			float w = ReadSingle();
			return new Vector4(x, y, z, w);
		}

		// FIXME: Why do these two exist? -flibit

		public override float ReadSingle()
		{
			return base.ReadSingle();
		}

		public override double ReadDouble()
		{
			return base.ReadDouble();
		}

		#endregion
	}
}
