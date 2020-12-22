using Microsoft.Extensions.Logging;
using MTProto.NET.Schema.TL;
using MTProto.NET.Server.Infrastructure.Storage;
using MTProto.NET.Server.Infrastructure.UpdateProcessing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MTProto.NET.Server.Infrastructure.Transports;
using Microsoft.Extensions.Hosting;
using System.Threading;
using MTProto.NET.Server.Contracts.Messages;
using MTProto.NET.Server.Contracts.Updates;
using System.Collections.Concurrent;

namespace MTProto.NET.Server.Infrastructure.Implementations
{


	class UpdateManager : IUpdateManager, IHostedService
	{
		private readonly ILogger<UpdateManager> logger;
		private readonly IMTServiceProvider serviceProvider;
		IMTSession session;
		static List<TLUpdates> updates = new List<TLUpdates>();
		private BlockingCollection<Func<Task>> queue = new BlockingCollection<Func<Task>>();
		public UpdateManager(ILogger<UpdateManager> logger, IMTServiceProvider serviceProvider)
		{
			this.logger = logger;
			this.serviceProvider = serviceProvider;
		}

		private async Task UpdateNewMessage(IUpdateProcessingContext context, TLAbsUpdate update, ProcessUpdate next)
		{
			var newMessageUpdate = update as TLUpdateNewMessage;

			var message = newMessageUpdate?.Message as MTProto.NET.Schema.Layer72.TLMessage;
			var user = message?.ToId as TLPeerUser;
			if (user != null)
			{
				var _update = new TLUpdateNewMessage()
				{
					Message = message
				};
				await context.AddUserUpdate(user.UserId, _update);
				return;
			}
			await next(context, update, next);
		}

		public async Task SendUpdate(TLUpdates updates)
		{
			UpdateManager.updates.Add(updates);

			return;


			if (updates == null)
				throw new ArgumentNullException(nameof(updates));
			try
			{
				var context = new UpdateProcesingContext(updates);
				foreach (var update in updates.Updates)
				{
					await UpdateNewMessage(context, update, (a, b, n) => Task.CompletedTask);
				}
				await Task.CompletedTask;
				foreach (var userId in context.GetDistictUsers())
				{
					try
					{
						var userUpdates = context.GetUserUpdates(userId);
						var user_updates = new TLUpdates
						{
							Chats = updates.Chats,
							Users = updates.Users,
							Updates = new TLVector<TLAbsUpdate>()
						};
						userUpdates.ToList().ForEach(x => user_updates.Updates.Add(x));
						user_updates.Seq = 0;
						using (var store = this.serviceProvider.GetService<IStore>().GetUserStore())
						{
							var userData = await store.GetUserById(userId);
							if (userData == null)
								throw new Exception($"User Not Found {userId}");
							var user = this.serviceProvider.Factory().CreateUser(userData);
							await user.AddUpdates(this.serviceProvider.Store(), user_updates);

							/// Send it to session
							/// 
							//await this.serviceProvider.GetService<IMTSessionManager>()
							//	.GetSessionByUserId(userId)
							//	.PostUpdate(user_updates);
							//MTHttpTransport.POSTED = 1;

							UpdateManager.updates.Add(user_updates);
						}
					}
					catch (Exception err)
					{
						this.logger.LogWarning(
							"An error occured while trying to process user updates. Error:\r\n{0}", err.GetBaseException().Message);
					}

				}

			}
			catch (Exception err)
			{
				this.logger.LogError(
					"An error occured while trying to process updates: \r\n{0}", err.GetBaseException().Message);
			}

		}

		public async Task<TLUpdates> GetPedningUpdate(int userId)
		{
			await Task.CompletedTask;
			if (userId == 2)
			{

			}
			var result = new TLUpdates();
			result.Updates = new TLVector<TLAbsUpdate>();
			result.Users = new TLVector<TLAbsUser>();
			result.Chats = new TLVector<TLAbsChat>();
			bool clear = false;
			foreach (var update in updates)
			{
				foreach (var u in update.Updates)
				{
					if (u is TLUpdateNewMessage)
					{
						var m = (u as TLUpdateNewMessage).Message as MTProto.NET.Schema.Layer72.TLMessage;
						if (m != null && m.ToId is TLPeerUser && ((TLPeerUser)m.ToId).UserId == userId)
						{
							result.Updates.Add(u);
							clear = true;
						}
					}
				}
			}
			if (clear)
				updates.Clear();
			return result;
		}

		#region IHostedService

		private CancellationTokenSource stop;
		private Task exceutingTask;


		private async Task<object> ProduceUpdates(IMessageContext context)
		{
			await Task.CompletedTask;
			TLUpdates result = null;

			try
			{
				if (context.MessageType() == typeof(Contracts.Messages.NewMessageCreated))
				{

					var new_message = context.Body as NewMessageCreated;
					if (new_message != null && new_message.PeerIdType == 0)
					{
						using (var store = MTServer.Services.Store().GetUserStore())
						{

							var peer_user = await store.GetUserById(new_message.ToPeerId);
							result = new TLUpdates { };
							result.Updates = new TLVector<TLAbsUpdate>();
							result.Users = new TLVector<TLAbsUser>();
							result.Chats = new TLVector<TLAbsChat>();

							var pts = peer_user.Pts + 1;
							var seq = peer_user.Seq + 1;
							result.Updates.Add(new TLUpdateNewMessage
							{
								Pts = pts,
								PtsCount = 1,
								Message = new MTProto.NET.Schema.Layer72.TLMessage
								{
									Message = new_message.Text,
									ToId = new TLPeerUser { UserId = new_message.ToPeerId },
									FromId = new_message.FromUserId,
									Date = new_message.Date == 0 ? Extensions.ToTelegramDate(DateTime.UtcNow) : new_message.Date,
									Id = new_message.Id
								}
							});
							var from_user = await store.GetUserById(new_message.FromUserId);

							result.Users.Add(new MTProto.NET.Schema.Layer72.TLUser{ 
								Id = from_user.Id,
								FirstName = from_user.FirstName,
								LastName = from_user.LastName
							});

							peer_user.Pts = pts;
							peer_user.Seq = seq;
							result.Seq = seq;
							await store.Update(peer_user);

							await MTServer.Bus.Publish(new UpdateCreated
							{
								Payload = result,
								UserId = new_message.ToPeerId,
							});
						}
					}
				}
				if (context.Body as Contracts.Account.ProfileChanged != null)
				{
					var profileChange = context.Body as Contracts.Account.ProfileChanged;
					using (var store = MTServer.Services.Store().GetUserStore())
					{
						var userIds = new int[] { 1, 2 };
						foreach (var userid in userIds)
						{
							var user_data = await store.GetUserById(userid);
							result = new TLUpdates { };
							result.Updates = new TLVector<TLAbsUpdate>();
							result.Users = new TLVector<TLAbsUser>();
							result.Chats = new TLVector<TLAbsChat>();
							var pts = user_data.Pts;
							var seq = user_data.Seq;
							var ptscount = 0;

							var user = new MTProto.NET.Schema.Layer72.TLUser
							{
								Id = user_data.Id,
								FirstName = user_data.FirstName,
								LastName = user_data.LastName,
								Contact = true
							};
							result.Users.Add(user);
							result.Updates.Add(new TLUpdateUserName
							{
								FirstName = profileChange.FirstName,
								LastName = profileChange.LastName,
								UserId = profileChange.AccountId,
								Username = profileChange.UserName ?? ""
							});

							await MTServer.Bus.Publish(new UpdateCreated
							{
								Payload = result,
								UserId = userid
							});
						}
					}

				}
				if (result != null)
				{

				}
			}
			catch (Exception err)
			{
				this.logger.LogError(
					$"An error occured while trying to ManageUpdates: \r\n{0}", err.GetBaseException().Message);
			}

			return "ok";

		}
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			stop = new CancellationTokenSource();
			var token = stop.Token;

			await MTServer.Bus.Subscribe(cfg =>
			{
				cfg.Topic = "*";
				cfg.Handler = x =>
				{
					this.queue.Add(() =>
					{
						return this.ProduceUpdates(x);
					});
					return Task.FromResult<object>("ok");
				};
			});

			this.exceutingTask = Task.Run(async () =>
			{
				while (!token.IsCancellationRequested)
				{
					while (this.queue.TryTake(out var f, 1000, cancellationToken))
					{
						try
						{
							await (f?.Invoke() ?? Task.CompletedTask);
						}
						catch
						{ }
					}
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




		#endregion

	}
}
