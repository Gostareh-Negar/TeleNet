using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Storage.Implementations.Internal.Internal
{
	class LibOptions
	{
		public static LibOptions Current = new LibOptions();
		public static LibOptions Default => Current;// new LibSettings();
													//private Action<MessageBusOptions> configureMessageBus;
		public LibOptions()
		{
		}
		//public LibOptions AddMessageBus(Action<MessageBusOptions> configure)
		//{
		//	this.configureMessageBus = configure;
		//	return this;
		//}
		//internal Action<MessageBusOptions> GetMessageBusConfigurator()
		//{
		//	return this.configureMessageBus;
		//}

		public string DocumentStoreDefaultDirectory => "./Data";
		public string DocumentStoreDefaultFileName => "db.dat";
		public string GetLocalDbFileName()
		{
			var result = Path.GetFullPath("./Data/local.db");
			if (!Directory.Exists(Path.GetDirectoryName(result)))
				Directory.CreateDirectory(Path.GetDirectoryName(result));
			return result;

		}
		public string GetCommonApplicationDataFolder()
		{
			var result = Path.GetDirectoryName(
				Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
				"Gostareh Negar\\sample.dat"));
			if (!Directory.Exists(result))
				Directory.CreateDirectory(result);
			return result;
		}
		public string GetUserDbFileName()
		{
			var result = Path.GetFullPath(
				Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"Gostareh Negar\\data\\user.db"));
			if (!Directory.Exists(Path.GetDirectoryName(result)))
				Directory.CreateDirectory(Path.GetDirectoryName(result));
			return result;
		}
		public string GetPublicDbFileName()
		{
			var result = Path.GetFullPath(
				Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
				"MTProto.Server\\Data\\public.db"));
			if (!Directory.Exists(Path.GetDirectoryName(result)))
				Directory.CreateDirectory(Path.GetDirectoryName(result));
			return result;
		}
		public string GetGlobalDbFileName()
		{
			var result = Path.GetFullPath(
				Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
				//@"\\BABAK-PC\Data\global.db"));
				"Gostareh Negar\\Data\\global.db"));
			result = @"\\BABAK-PC\Data\global.db";

			//\\BABAK-PC\Data
			if (!Directory.Exists(Path.GetDirectoryName(result)))
				Directory.CreateDirectory(Path.GetDirectoryName(result));
			return result;
		}
	}
}
