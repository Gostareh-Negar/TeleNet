using MTProto.NET;
using MTProto.NET.Schema.MT;
using MTProto.NET.Schema.MT.Requests;
using MTProto.NET.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TDLib.Server2;
using TelegramServer.Handlers;

namespace TelegramServer
{
	
	public class MTReqPqHandler :MTProtoHandler<MTReqPq, MTResPQ>
	{
		public override async Task<MTResPQ> Handle(MTReqPq request)
		{
			MTResPQ res =  await Task.FromResult<MTResPQ>(new MTResPQ());
										//await Task.CompletedTask;
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
			res.ServerNonce = RsaKey.ServerOnce();

			return res;

		}

		
	}
}
