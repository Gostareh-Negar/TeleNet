using MTProto.NET.Schema.TL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MTProto.NET.Server.Infrastructure.UpdateProcessing
{
	public interface IUpdateProcessingContext
	{
		TLUpdates Updates { get; }
		Task AddUserUpdate(int userId, TLAbsUpdate update);
	}
	class UpdateProcesingContext : IUpdateProcessingContext
	{
		private Dictionary<int, List<TLAbsUpdate>> userUpdates = new Dictionary<int, List<TLAbsUpdate>>();
		public TLUpdates Updates { get; set; }
		public UpdateProcesingContext(TLUpdates updates)
		{
			this.Updates = updates;
		}

		public async Task AddUserUpdate(int userId, TLAbsUpdate update)
		{
			if (this.userUpdates == null)
				this.userUpdates = new Dictionary<int, List<TLAbsUpdate>>();
			if (!this.userUpdates.ContainsKey(userId))
				this.userUpdates.Add(userId, new List<TLAbsUpdate>());
			this.userUpdates[userId].Add(update);

			
			await Task.CompletedTask;
		}
		public int[] GetDistictUsers()
		{
			return this.userUpdates.Keys.ToArray();
		}
		public IEnumerable<TLAbsUpdate> GetUserUpdates(int userId)
		{
			return this.userUpdates.ContainsKey(userId)
				? this.userUpdates[userId] ?? new List<TLAbsUpdate>()
				: new List<TLAbsUpdate>();



		}
	}
}
