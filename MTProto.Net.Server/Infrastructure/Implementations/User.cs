using MTProto.NET.Schema.TL;
using MTProto.NET.Server.Infrastructure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class User : Peer, IUser
	{
		public User(IUserData data) : base(data)
		{

		}

		public IUserData UserData => this.data;

		public int Id => this.UserData.Id;

		public string Phone => this.UserData.Phone;

		public async Task AddUpdates(IStore store, TLUpdates updates)
		{
			var pts = data.Pts;
			//data.Seq++;
			foreach (var update in updates.Updates)
			{
				pts++;
				update.GetType().GetProperty("Pts").SetValue(update, pts);
				update.GetType().GetProperty("PtsCount").SetValue(update, 1);
			}
			data.Seq++;
			data.Pts = pts;
			updates.Seq = data.Seq;
			updates.Date = MTServer.Services.Utils().ToTelegramDate(DateTime.UtcNow);
			using (var update_store = store.GetUserUpdateStore())
			{
				var _data = MTServer.Services.Factory().CreateUserUpdate();
				_data.UserId = this.data.Id;
				_data.Pts = data.Pts;
				_data.PtsCount = updates.Updates.Count;
				_data.Content = updates.ToByteArray();
				await update_store.AddUpdate(_data);
				using (var user_store = store.GetUserStore())
				{
					await user_store.Update(this.data);
				}
			}
		}
	}
}
