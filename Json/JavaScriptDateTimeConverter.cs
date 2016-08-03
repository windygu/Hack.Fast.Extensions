﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.ObjectModel;
using System.Collections;

namespace Hack.Fast.Extensions.Json
{
	internal class JavaScriptDateTimeConverter : JavaScriptConverter
	{
		public override IEnumerable<Type> SupportedTypes
		{
			get { 
				return new[] { typeof(DateTime) };
			}
		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			if( !(obj is DateTime) )
				return null;

			return new DateTimeString(((DateTime)obj).ToUniversalTime().ToString("O"));
		}

		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if( dictionary.Count > 0 ) {
				return serializer.ConvertToType<DateTime>(dictionary);
			}
			return null;
		}

		private class DateTimeString : Uri, IDictionary<string, object>
		{
			public DateTimeString(string str)
				: base(str, UriKind.Relative)
			{
			}

			void IDictionary<string, object>.Add(string key, object value)
			{
				throw new NotImplementedException();
			}

			bool IDictionary<string, object>.ContainsKey(string key)
			{
				throw new NotImplementedException();
			}

			ICollection<string> IDictionary<string, object>.Keys
			{
				get { throw new NotImplementedException(); }
			}

			bool IDictionary<string, object>.Remove(string key)
			{
				throw new NotImplementedException();
			}

			bool IDictionary<string, object>.TryGetValue(string key, out object value)
			{
				throw new NotImplementedException();
			}

			ICollection<object> IDictionary<string, object>.Values
			{
				get { throw new NotImplementedException(); }
			}

			object IDictionary<string, object>.this[string key]
			{
				get
				{
					throw new NotImplementedException();
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
			{
				throw new NotImplementedException();
			}

			void ICollection<KeyValuePair<string, object>>.Clear()
			{
				throw new NotImplementedException();
			}

			bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
			{
				throw new NotImplementedException();
			}

			void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			int ICollection<KeyValuePair<string, object>>.Count
			{
				get { throw new NotImplementedException(); }
			}

			bool ICollection<KeyValuePair<string, object>>.IsReadOnly
			{
				get { throw new NotImplementedException(); }
			}

			bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
			{
				throw new NotImplementedException();
			}

			IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}
		}
	}
}
