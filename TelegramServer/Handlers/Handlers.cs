using MTProto.NET;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDLib.Server2;

namespace TelegramServer.Handlers
{
	
	public static class Handlers
	{
		public static async Task<MTResPQ> Handle(MTReqPq request)
		{
			MTResPQ res = await Task.FromResult<MTResPQ>(new MTResPQ());
			await Task.CompletedTask;
			//var writer = request.Output;
			//var auth = request.AuthKey;
			//var key = request.MessageId;
			//var size = request.Size;

			//writer.Write(auth);
			//writer.Write(key);
			//writer.Write(size);
			//var res = new MTResPQ();
			res.Nonce = request.Nonce;// ; new BigInteger(clientOnce);
			res.Pq = BitConverter.GetBytes(RsaKey.m_pq());
			var vector = new MTProto.NET.Schema.TL.TLVector<long>();
			var finger = RsaKey.GetFingerPrint();
			var finger2 = BitConverter.ToInt64(finger); ;
			vector.Add(finger2);
			res.ServerPublicKeyFingerprints = vector;

			var bytes = new List<byte>();
			var part1 = BitConverter.GetBytes(0x56781234abcdef00);
			var part2 = BitConverter.GetBytes(0x56781234abcdef00);
			bytes.AddRange(part2);
			bytes.AddRange(part1);

			var _ServerNonce = new Org.BouncyCastle.Math.BigInteger(BitConverter.GetBytes(0x56781234abcdef00));
			_ServerNonce.Multiply(new Org.BouncyCastle.Math.BigInteger(BitConverter.GetBytes(0xbcdefabcd0011224)));
			res.ServerNonce = RsaKey.ServerOnce();// new Org.BouncyCastle.Math.BigInteger(bytes.ToArray());
			return res;
		}
		public static Func<object, Task<object>> GetHandler(object request)
		{
			Func<object, Task<object>> result = async o => {
				object _result = null;

				switch (o)
				{
					case MTReqPq req:
						_result =await Handle(req);
						break;
					case MTReqDhParams hh:
						{

						}
						break;
					default:
						break;
				}
				return _result;
			
			};
			return result;

			
		}
	
		public static IMTProtoHandler GetHandlerEx(object request)
		{
			IMTProtoHandler result = null;
			switch (request)
			{
				case MTReqPq req:
					result = new MTReqPqHandler();
					
					break;
				case MTReqDhParams hh:
					{
						result = new ReqDHParamsHandler();

					}
					break;
				default:
					break;
			}
			return result;
		}
	}
}
