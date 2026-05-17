#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed class PropertyDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		#region Public Properties

		public int Count
		{
			get
			{
				return dictionary.Count;
			}
		}

		public object this[string key]
		{
			get
			{
				return dictionary[key];
			}
			set
			{
				dictionary[key] = value;
			}
		}

		#endregion

		#region Private Variables

		private Dictionary<string, object> dictionary;

		#endregion

		#region Internal Constructor

		internal PropertyDictionary(Dictionary<string, object> dictionary)
		{
			this.dictionary = dictionary;
		}

		#endregion

		#region Public Methods

		public bool ContainsKey(string key)
		{
			return dictionary.ContainsKey(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return dictionary.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		#endregion

		#region Public Get Methods

		public DateTime GetValueDateTime(string key)
		{
			return (DateTime) dictionary[key];
		}

		public double GetValueDouble(string key)
		{
			return (double) dictionary[key];
		}

		public int GetValueInt32(string key)
		{
			return (int) dictionary[key];
		}

		public long GetValueInt64(string key)
		{
			return (long) dictionary[key];
		}

		public LeaderboardOutcome GetValueOutcome(string key)
		{
			return (LeaderboardOutcome) dictionary[key];
		}

		public float GetValueSingle(string key)
		{
			return (float) dictionary[key];
		}

		public Stream GetValueStream(string key)
		{
			return (Stream) dictionary[key];
		}

		public string GetValueString(string key)
		{
			return (string) dictionary[key];
		}

		public TimeSpan GetValueTimeSpan(string key)
		{
			return (TimeSpan) dictionary[key];
		}

		#endregion

		#region Public Set Methods

		public void SetValue(string key, DateTime value)
		{
			dictionary[key] = value;
		}

		public void SetValue(string key, double value)
		{
			dictionary[key] = value;
		}

		public void SetValue(string key, int value)
		{
			dictionary[key] = value;
		}

		public void SetValue(string key, long value)
		{
			dictionary[key] = value;
		}

		public void SetValue(string key, LeaderboardOutcome value)
		{
			dictionary[key] = value;
		}

		public void SetValue(string key, float value)
		{
			dictionary[key] = value;
		}

		public void SetValue(string key, string value)
		{
			dictionary[key] = value;
		}

		public void SetValue(string key, TimeSpan value)
		{
			dictionary[key] = value;
		}

		#endregion

		#region IDictionary Implementation

		ICollection<string> IDictionary<string, object>.Keys
		{
			get
			{
				return dictionary.Keys;
			}
		}

		ICollection<object> IDictionary<string, object>.Values
		{
			get
			{
				return dictionary.Values;
			}
		}

		void IDictionary<string, object>.Add(string key, object value)
		{
			dictionary.Add(key, value);
		}

		bool IDictionary<string, object>.Remove(string key)
		{
			return dictionary.Remove(key);
		}

		#endregion

		#region ICollection<KeyValuePair> Implementation

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			dictionary.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			if (!dictionary.ContainsKey(item.Key) || dictionary[item.Key] != item.Value)
			{
				return false;
			}
			return dictionary.Remove(item.Key);
		}

		void ICollection<KeyValuePair<string, object>>.Clear()
		{
			dictionary.Clear();
		}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
		{
			return dictionary.ContainsKey(item.Key) && dictionary.ContainsValue(item.Key);
		}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
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
