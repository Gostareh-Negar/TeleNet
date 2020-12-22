using MTProto.NET.Server.Infrastructure.Helpers;
using MTProto.NET.Server.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BigInteger = System.Numerics.BigInteger;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Infrastructure.Implementations;
using MTProto.NET.Server.Infrastructure.Storage;

namespace MTProto.NET.Server
{
	public static partial class Extensions
	{
		public static IMTUtilityService Utils(this IMTService context)
		{
			return GetService<IMTUtilityService>() ?? new MTUtilityServices(null);
		}
		public static IMTUtilityService Utils(this IMTServiceProvider provider)
		{
			return provider.GetService<IMTUtilityService>() ?? GetService<MTUtilityServices>();
		}
		//public static IMTUtilityService Utils()
		//{
		//	return GetService<IMTUtilityService>() ?? GetService<MTUtilityServices>();
		//}

		public static byte[] ReadAllBytes(this BinaryReader reader)
		{
			const int bufferSize = 4096;
			using (var ms = new MemoryStream())
			{
				byte[] buffer = new byte[bufferSize];
				int count;
				while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
					ms.Write(buffer, 0, count);
				return ms.ToArray();
			}

		}
		public static bool TryDeserialize<T>(string input, out T output)
		{
			try
			{
				output = JsonConvert.DeserializeObject<T>(input);
				return true;
			}
			catch { }
			output = default;
			return false;
		}

		public static string ToHex(this IMTUtilityService utils, ulong value)
		{
			return string.Format("{0:X}", value);
		}
		
		public static IMTObjectFactory Factory(this IMTServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<IMTObjectFactory>();
		}
		public static IStore Store(this IMTServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<IStore>();
		}

		public static IMTObjectFactory Factory(this IMTUtilityService utils)
		{
			return utils.ServiceProvider.Factory();
		}
		public static IMTMapper Mapper(this IMTUtilityService utils)
		{
			return utils.ServiceProvider.GetService<IMTMapper>();
		}
		public static IMTMapper Mapper(this IMTServiceProvider provider)
		{
			return provider.GetService<IMTMapper>();
		}

		public static int ToTelegramDate(DateTime dateTime)
		{
			return DateTimeHelper.ToTelegarmDate(dateTime);
		}

		public static int ToTelegramDate(this IMTUtilityService utils, DateTime dateTime)
		{
			return DateTimeHelper.ToTelegarmDate(dateTime);
		}

		public static BigInteger ToBigInteger(this IMTUtilityService utils, int sign, byte[] bytes)
		{
			byte? insert_byte = (sign > 0 && (bytes[bytes.Length - 1] & 0x80) > 0)
				? (byte)0
				: (sign < 0 && (bytes[bytes.Length - 1] & 0x80) == 0)
				? (byte)0x80
				: (byte?)null;
			return new BigInteger(bytes.Concat(insert_byte.HasValue ? new byte[] { insert_byte.Value } : new byte[] { }).ToArray());
		}

		public static async Task TimeoutAfter(this Task task, int millisecondsTimeout, CancellationToken token, bool Throw = true)
		{
			if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
				await task;
			else if (Throw)
				throw new TimeoutException();

		}
		public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, int millisecondsTimeout, CancellationToken token, bool Throw = true)
		{
			if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout, token)))
				return await task.ConfigureAwait(false);
			else if (Throw)
				throw new TimeoutException();
			return default(TResult);
		}
		public static Task<object> Ok(this MessageHandler handler)
		{
			return Task.FromResult<object>(true);
		}

		public static MobilePhoneHelper GetMobilePhoneHelper(string phone)
		{
			return new MobilePhoneHelper().Parse(phone);
		}

		public static MobilePhoneHelper GetMobilePhoneHelper(this IMTUtilityService utils, string phone)
		{
			return GetMobilePhoneHelper(phone);
		}
		
		public static T Clone<T>(this T mtObject) where T:MTObject
		{
			var ret = Activator.CreateInstance(mtObject.GetType());
			ret.GetType().GetProperties().ToList().ForEach(p => p.SetValue(ret, p.GetValue(mtObject)));
			return ret as T;
			//return mtObject.ToByteArray().ToMTObject() as T;
		}
	
		public static string GetTopic(Type type)
		{
			if (type.IsGenericType && typeof(IMessageContext<>).IsAssignableFrom(type.GetGenericTypeDefinition()))// type.GetGenericTypeDefinition() == typeof(IMTMessageContext<>))
			{
				type = type.GetGenericArguments()[0];
			}
			if (type.IsGenericType && type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMessageContext<>)) != null)
			{

				type = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMessageContext<>)).GetGenericArguments()[0];
			}
			return type.FullName;
		}

	}
}
