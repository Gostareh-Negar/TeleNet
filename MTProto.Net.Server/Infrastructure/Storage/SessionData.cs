using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MTProto.NET.Server.Infrastructure.Storage
{
	public class SessionData
	{
		public Dictionary<string, string> Headers { get; set; }
		public Guid Id { get; set; }
		public ulong AuthKeyId { get; set; }
		public ulong SessionId { get; set; }
		public string AuthKey { get; set; }

		public int UserId { get; set; }
		public string ClientNonce { get; set; }
		public string ServerNonce { get; set; }
		public string Dh_ma { get; set; }
		public string Dh_mga { get; set; }

		public DateTime? CreatedOn { get; set; }
		public string NewNonce { get; set; }
		public uint ContentRelatedMessageNumber { get; set; }

		
		public BigInteger GetNewNonce()
		{
			return new BigInteger(this.NewNonce);
		}

		public  string SetNewNonce( byte[] newNonce)
		{
			return SetNewNoce(new BigInteger(1, newNonce));
			
		}
		public string SetNewNoce(BigInteger newNonce)
		{
			this.NewNonce = newNonce.ToString();
			return this.NewNonce;
		}
		public string SetDh_ma(BigInteger value = null)
		{
			this.Dh_ma = value.ToString();
			return this.Dh_ma;
		}
		public BigInteger GetDh_ma()
		{
			return new BigInteger(this.Dh_ma);
		}
		public string Set_mgA(BigInteger value)
		{
			this.Dh_mga = value.ToString();
			return this.Dh_mga;
		}

		public void SetAuthKey(BigInteger value)
		{
			this.AuthKey = value.ToString(16);
		}
		public BigInteger GetAuthKey()
		{
			return new BigInteger(this.AuthKey,16);
			
		}
		public byte[] GetAuthKeyAsByteArray()
		{
			return this.GetAuthKey().ToByteArray();
		}
		public byte[] GetServerKeyPart()
		{

			return GetAuthKeyAsByteArray().Skip(96).Take(32).ToArray();
		}
		public byte[] GetClientKeyPart()
		{

			return GetAuthKeyAsByteArray().Skip(88).Take(32).ToArray();
		}

	}

	public static class SessionDataExtensions
	{
		public static string UpdateNewNoce(this SessionData data, byte[] newNonce)
		{
			data.NewNonce = new BigInteger(1, newNonce).ToString();
			return data.NewNonce;
		}
		private static ISessionStore GetStore()
		{
			return MTServer.Services.GetService<ISessionStore>();
		}
		public static SessionData Upsert(this SessionData data)
		{
			using(var store = GetStore())
			{
				return store.Upsert(data);
			}
		}
	
		
	}
}
