#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Microsoft.Xna.Framework.Net
{
	public class NetworkSessionProperties : IList<int?>, ICollection<int?>, IEnumerable<int?>, IEnumerable
	{
		#region Public Properties

		public int Count
		{
			get
			{
				return properties.Count;
			}
		}

		public int? this[int index]
		{
			get
			{
				return properties[index];
			}
			set
			{
				if (index >= properties.Count)
				{
					// TODO: Expand list to index size? -flibit
					properties.Add(value);
				}
				else
				{
					properties[index] = value;
				}
			}
		}

		#endregion

		#region Private Variables

		private List<int?> properties;

		#endregion

		#region Public Constructor

		public NetworkSessionProperties()
		{
			properties = new List<int?>();
		}

        #endregion

        #region Public Methods

        public static void WriteProperties(NetworkSessionProperties properties, int[] propertyData)
        {

            for (int x = 0; x < properties.Count; x++) // if out of bounds, go 8
            {
                if ((properties != null) && properties[x].HasValue)
                {
                    // flag it as having a value
                    propertyData[x * 2] = 1;
                    propertyData[x * 2 + 1] = properties[x].Value;

                }
                else
                {
                    // flag it as not having a value
                    propertyData[x * 2] = 0;
                    propertyData[x * 2 + 1] = 0;

                }
            }
        }

        public static void ReadProperties(NetworkSessionProperties properties, int[] propertyData)
        {
            for (int x = 0; x < properties.Count; x++) // if out of bounds, go 8
            {
                // set it to null to start
                properties[x] = null;
                // and only if the flag is turned on do we have a value.
                if (propertyData[x * 2] > 0)
                    properties[x] = propertyData[x * 2 + 1];
            }
        }

        public IEnumerator<int?> GetEnumerator()
		{
			return properties.GetEnumerator();
		}

		#endregion

		#region IList Implementation

		int IList<int?>.IndexOf(int? item)
		{
			return properties.IndexOf(item);
		}

		void IList<int?>.Insert(int index, int? item)
		{
			properties.Insert(index, item);
		}

		void IList<int?>.RemoveAt(int index)
		{
			properties.RemoveAt(index);
		}

		#endregion

		#region ICollection Implementation

		bool ICollection<int?>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		void ICollection<int?>.Add(int? item)
		{
			properties.Add(item);
		}

		bool ICollection<int?>.Remove(int? item)
		{
			return properties.Remove(item);
		}

		bool ICollection<int?>.Contains(int? item)
		{
			return properties.Contains(item);
		}

		void ICollection<int?>.Clear()
		{
			properties.Clear();
		}

		void ICollection<int?>.CopyTo(int?[] array, int arrayIndex)
		{
			properties.CopyTo(array, arrayIndex);
		}

		#endregion

		#region IEnumerable Implementation

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
