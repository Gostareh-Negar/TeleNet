using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		public static bool TryGetValue<T>(this IDictionary<string, string> dic, string key, out T value)
		{
			value = default;
			return dic.TryGetValue(key, out var v) && TryDeserialize<T>(v, out value);
		}

		public static T GetOrAddValue<T>(this IDictionary<string, string> dic, string key, T value)
		{
			if (dic.TryGetValue<T>(key, out var v))
				return v;
			dic.Add(key, JsonConvert.SerializeObject(value));
			return value;
		}
		public static T AddOrUpdateValue<T>(this IDictionary<string, string> dic, string key, T value)
		{
			if (dic.ContainsKey(key))
			{
				dic[key] = JsonConvert.SerializeObject(value);
			}
			else
			{
				dic.Add(key, JsonConvert.SerializeObject(value));
			}
			return value;
		}

		private static string GetKey(this Type type, string key)
		{
			return string.Format("{0}|{1}", type?.FullName, key);
		}
		public static T GetOrAddValue<T>(this IDictionary<string, object> dic, Func<T> constructor = null, string key = null)
		{
			T result = default(T);
			//key = string.Format("{0}|{1}", typeof(T).FullName, key);
			key = typeof(T).GetKey(key);
			if (dic.TryGetValue(key, out var ret))
			{
				if (ret == null)
					return default(T);
				if (typeof(T).IsAssignableFrom(ret.GetType()))
					return (T)ret;
				result = constructor == null ? result : constructor();
				dic[key] = result;
			}
			else
			{
				result = constructor == null ? result : constructor();
				dic.Add(key, result);
			}
			return result;
		}
		
		public static T AddOrUpdateValue<T>(this IDictionary<string, object> dic, Func<T> constructor, string key = null)
		{
			T result = default(T);
			//key = string.Format("{0}|{1}", typeof(T).FullName, key);
			key = typeof(T).GetKey(key);
			if (dic.TryGetValue(key, out var ret))
			{
				result = constructor == null ? result : constructor();
				dic[key] = result;
			}
			else
			{
				result = constructor == null ? result : constructor();
				dic.Add(key, result);
			}
			return result;
		}
		public static void RemoveValue<T>(this IDictionary<string, object> dic, string key = null)
		{
			//key = string.Format("{0}|{1}", typeof(T).FullName, key);
			key = typeof(T).GetKey(key);
			if (dic.ContainsKey(key))
				dic.Remove(key);
		}
		public static bool TryGetValue<T>(this IDictionary<string, object> dic, out T value, string key = null)
		{
			key = typeof(T).GetKey(key);
			value = default(T);
			if (dic.TryGetValue(key, out var ret))
			{
				if (ret == null)
					return true;
				if (ret != null && typeof(T).IsAssignableFrom(ret.GetType()))
				{
					value = (T)ret;
					return true;
				}
			}
			return false;

		}


	}
}
