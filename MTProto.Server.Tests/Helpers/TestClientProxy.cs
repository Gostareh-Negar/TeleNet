using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTProto.Server.Tests.Helpers
{
	public class TestClient : MarshalByRefObject
	{
		private static IWebHost host;
		private static IWebHost GetHost(int port)
		{
			if (host == null)
			{
				host = Utils.CreateWebHost(port:port);
			}
			return host;
		}
		static TestClient()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}
		private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			AssemblyName name = new AssemblyName(args.Name);
			return Assembly.Load(name.Name);
		}
		public void Stop()
		{
			host?.StopAsync(default(CancellationToken)).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		public int Start(int idx)
		{
			var result = 0;
			var p = new Random((int)DateTime.Now.Ticks).Next(2000, 3000) + idx;
			var host = GetHost(idx);

			host.StartAsync(default(CancellationToken)).ConfigureAwait(false).GetAwaiter().GetResult();
			return result;
		}
	}


}

