using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		public static object MakeGenericActionCall(Action action, Type type)
		{
			return action.Method.GetGenericMethodDefinition()
				.MakeGenericMethod(type)
				.Invoke(action.Target, new object[] { });
		}

		public static object MakeGenericActionCall<TObject, T>(Action<TObject> action, Type type, T arg)
		{
			return action.Method.GetGenericMethodDefinition()
				.MakeGenericMethod(type)
				.Invoke(action.Target, new object[] { arg });
		}
		public static object MakeGenericActionCall<T1, T2>(Action<T1, T2> action, Type type, T1 arg, T2 arg1)
		{
			return action.Method.GetGenericMethodDefinition()
				.MakeGenericMethod(type)
				.Invoke(action.Target, new object[] { arg, arg1 });
		}
		public static object MakeGenericActionCall<T1, T2, T3>(Action<T1, T2, T3> action, Type type, T1 arg, T2 arg1, T3 arg2)
		{
			return action.Method.GetGenericMethodDefinition()
				.MakeGenericMethod(type)
				.Invoke(action.Target, new object[] { arg, arg1 });
		}

		public static TRes MakeGenericFunctionCall<TRes>(Func<TRes> action, Type type)
		{
			return (TRes)action.Method.GetGenericMethodDefinition()
				.MakeGenericMethod(type)
				.Invoke(action.Target, new object[] { });

		}

		public static TRes MakeGenericFunctionCall<T1, TRes>(Func<T1, TRes> action, Type type, T1 arg)
		{
			return (TRes)action.Method.GetGenericMethodDefinition()
				.MakeGenericMethod(type)
				.Invoke(action.Target, new object[] { arg });

		}
		public static TRes MakeGenericFunctionCall<T1, TRes>(Func<T1, TRes> action, Type type, object arg)
		{
			return (TRes)action.Method.GetGenericMethodDefinition()
				.MakeGenericMethod(type)
				.Invoke(action.Target, new object[] { arg });

		}



	}
}
