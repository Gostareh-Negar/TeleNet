using MTProto.NET.Server.Infrastructure.Encryption;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Configurations
{
	public class ServerOptions
	{
		public static ServerOptions Instance = new ServerOptions();

		public IRSAKey ServerKey => RSAKey.Default;
		public string Nonce { get; set; }

		public ulong pq => Defaultm_pq();

		

		public ServerOptions()
		{
			Validate();
		}
		public ServerOptions Validate()
		{
			this.Nonce = string.IsNullOrEmpty(this.Nonce) ? DefaultServerOnce().ToString() : this.Nonce;
			return this;
		}
		public BigInteger GetNonce()
		{
			return DefaultServerOnce();
			return string.IsNullOrEmpty(this.Nonce) ? DefaultServerOnce() : new BigInteger(this.Nonce);
		}
		public static BigInteger DefaultServerOnce()
		{
			var bytes = new List<byte>();
			var part1 = BitConverter.GetBytes(0x56781234abcdef00);
			var part2 = BitConverter.GetBytes(0xbcdefabcd0011224);
			bytes.AddRange(part2);
			bytes.AddRange(part1);
			var big = new Org.BouncyCastle.Math.BigInteger(bytes.ToArray());
			return big;
			//var bytes = new List<byte>();
			//var part1 = BitConverter.GetBytes(0x56781234abcdef00);
			//var part2 = BitConverter.GetBytes(0xbcdefabcd0011224);
			//bytes.AddRange(BitConverter.GetBytes(0x56781234abcdef00));
			//bytes.AddRange(BitConverter.GetBytes(0xbcdefabcd0011224));
			//return new Org.BouncyCastle.Math.BigInteger(bytes.ToArray());
		}
		public static ulong Defaultm_pq()
		{
			var m_p = 1244159563ul;
			var m_q = 1558201013ul;
			//var m_pq = m_p * m_q;
			return m_p * m_q;
		}


	}
}
