using Microsoft.Extensions.Logging;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Math;
using System.Linq;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using MTProto.NET.Server.Contracts.Authorization;
using MTProto.NET.Server.Contracts.Messages;
using MTProto.NET.Server.Contracts.Updates;
using MTProto.NET.Server.Infrastructure.Helpers;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class MTSessionManager : IMTSessionManager, IHostedService, IMessageHandler<NewSessionCreated>
	{
		
		private readonly ILogger<MTSessionManager> logger;
		private readonly ISessionStore sessionStore;
		private readonly IServiceProvider provider;
		private ConcurrentDictionary<ulong, IMTSession> sessions = new ConcurrentDictionary<ulong, IMTSession>();


		public MTSessionManager(ILogger<MTSessionManager> logger, ISessionStore sessionStore, IServiceProvider provider)
		{
			this.logger = logger;
			this.sessionStore = sessionStore;
			this.provider = provider;
		}
		public IAuthorizationSession GetOrCreateAuthorizationSession(string clientNonce)
		{
			var result = this.sessions.Values.FirstOrDefault(x => x.Data.ClientNonce == clientNonce);
			if (result == null)
			{
				var data = this.sessionStore.GetByClientNoonce(clientNonce, true);
				result = this.provider.GetService<MTSession>();
				result.Initialize(data);
				//this.cache.TryAdd(Convert.ToInt64(new Random().Next()), result);
			}
			return result as IAuthorizationSession;
		}

		public IAuthorizationSession GetOrCreateAuthorizationSession(BigInteger clientNonce)
		{
			return GetOrCreateAuthorizationSession(clientNonce.ToString(16));
		}

		public async Task<IMTSession> KeepAlive(ulong sessionId, ulong auth_key)
		{
			var session = this.GetSession(auth_key);
			if (session == null)
				throw new Exception(
					$"Session Not Found. AuthKeyId:{auth_key}");
			await session.KeepAlive(sessionId);

			return session;

		}

		public IMTSession GetSession(ulong? AuthKeyId)
		{
			IMTSession result = null;
			if (!AuthKeyId.HasValue)
				return result;
			result = this.sessions.GetOrAdd(AuthKeyId.Value, id =>
			  {
				  using (var store = this.provider.GetService<ISessionStore>())
				  {
					  var data = store.GetByAuthId(AuthKeyId.Value);
					  if (data == null)
						  return null;
					  return this.provider.GetService<MTSession>()
							  .Initialize(data);
				  }
			  });
			return result;
		}

		public IMTSession GetSessionByUserId(int userId)
		{
			IMTSession result = this.sessions.Values.FirstOrDefault(x => x.Data.UserId == userId);
			if (result == null)
			{
				var data = this.sessionStore.GetByUserId(userId);
				if (data == null)
					throw new Exception($"Session not found. UserId:{userId}");
				result = this.GetSession(data.AuthKeyId);
				//result = this.provider.GetService<MTSession>()
				//	.Initialize(data);
			}
			return result;
		}

		#region IHostedService

		private CancellationTokenSource stop;
		private Task exceutingTask;

		private async Task<object> SendUpdates(IMessageContext<UpdateCreated> context)
		{
			await Task.CompletedTask;
			var body = context.Body.Payload as MTObject;
			if (body == null)
				throw new Exception("Invalid Payload!");
			var session = this.GetSessionByUserId(context.Body.UserId);
			if (session != null)
			{
				session.AddMessage(body, false, false);
			}


			return null;

		}
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			stop = new CancellationTokenSource();
			var token = stop.Token;

			await MTServer.Bus.Subscribe<UpdateCreated>(cfg =>
			{
				cfg.Handler = x => {

					return this.SendUpdates(x);
				};
			});

			this.exceutingTask = Task.Run(async () =>
			{
				while (!token.IsCancellationRequested)
				{
					await Task.Delay(1 * 100);

				}

			});

			await Task.CompletedTask;
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			stop?.Cancel();
			if (exceutingTask != null)
				await this.exceutingTask;
			await Task.CompletedTask;
		}

		public async Task<object> Handle(IMessageContext<NewSessionCreated> context)
		{
			await Task.CompletedTask;
			this.logger.LogInformation("New Session");
			return null;


		}

		#endregion
	}
}
