using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace TelegramServer
{
	interface ICovariant<out R, in A>
	{
		R GetSomething(A arg);
	}
	class SampleImplementation<R, A> : ICovariant<R, A>
	{
		public R GetSomething(A arg)
		{
			// Some code.
			return default(R);
		}
	}
	public class Program
	{

		public static void Main(string[] args)
		{
			//var g = new System.Security.Cryptography.RSAParameters();

			//var f = new SampleImplementation<string, string>() as ICovariant<string, string>;
			//var Keysize = 256;
			//var password = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes("123456789"));


			//var keyBytes = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes("123456789"));
			//var ivStringBytes = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes("123456789"));
			//var engine = new RijndaelEngine(256);
			//var blockCipher = new CbcBlockCipher(engine);
			//var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
			//var keyParam = new KeyParameter(keyBytes);
			//var keyParamWithIV = new ParametersWithIV(keyParam, ivStringBytes, 0, 32);

			//var eng = new Org.BouncyCastle.Crypto.Engines.RijndaelEngine(256);
			//eng.Init(true, keyParamWithIV);

			//cipher.Init(true, keyParamWithIV);
			//var comparisonBytes = new byte[cipher.GetOutputSize(cipherTextBytes.Length)];
			//var length = cipher.ProcessBytes(cipherTextBytes, comparisonBytes, 0);
			//cipher.DoFinal(comparisonBytes, length);
			//using (var symmetricKey = new RijndaelManaged())
			//{
			//	symmetricKey.BlockSize = 256;
			//	symmetricKey.Mode = CipherMode.CBC;
			//	symmetricKey.Padding = PaddingMode.PKCS7;
			//}
			//	//ICovariant<Object,Object> s = f;
			//	var SymmetricKey = new AesManaged();
			//SymmetricKey.Mode = CipherMode.CTS;
			//var iv = new byte[32];
			//SymmetricKey.BlockSize = 256;
			//SymmetricKey.IV = iv;


			//var aes = Aes.Create("AES");
			//aes.BlockSize = 256;
			//aes.Mode = CipherMode.CTS;
			//aes.GenerateIV();




			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging(l =>
				{
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
					webBuilder.UseKestrel(options =>
					{
						options.AllowSynchronousIO = true;
					});
					webBuilder.UseUrls("http://localhost:12345");
				});
	}
}
