using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClashofClans.Logic;

namespace ClashofClans.Database.Cache
{
	public class Players : Dictionary<long, Player>
	{
		public readonly object SyncObject = new object();

		public async Task<Player> Login(long userId, string token)
		{
			Player player;

			if (userId <= 0 && string.IsNullOrEmpty(token))
			{
				player = await PlayerDb.CreateAsync();
				player.Home.Status = 1;
				player.Home.CurrentSeasonMonth = DateTime.Now.Month;
			}
			else
			{
				//var p = await Redis.GetPlayerAsync(userId);
				Player p = Resources.ObjectCache.GetCachedPlayer(userId);

				if (p != null)
					player = p;
				else
					player = await PlayerDb.GetAsync(userId);

				if (player == null) return null;
				if (player.Home.UserToken != token) return null;
			}

			lock (SyncObject)
			{
				if (player == null) return null;

				Logout(ref player);

				bool result = TryAdd(player.Home.Id, player);

				if (!result) return null;

				//Logger.Log($"User {player.Home.Id} logged in.", GetType(), ErrorLevel.Debug);

				return player;
			}
		}

		public void Logout(ref Player player)
		{
			lock (SyncObject)
			{
				if (!ContainsKey(player.Home.Id)) return;

				Player p = this[player.Home.Id];
				p.ValidateSession();
				p.Save();

				player = p;

				bool result = Remove(p.Home.Id);

				if (!result) Logger.Log($"Couldn't logout player {p.Home.Id}", GetType(), Logger.ErrorLevel.Error);
				//else Logger.Log($"User {player.UserId} logged out.", GetType(), ErrorLevel.Debug);
			}
		}

		public bool LogoutById(long userId)
		{
			lock (SyncObject)
			{
				if (!ContainsKey(userId)) return true;

				Player player = this[userId];
				player.ValidateSession();
				player.Save();

				bool result = Remove(userId);

				if (!result) Logger.Log($"Couldn't logout player {userId}", GetType(), Logger.ErrorLevel.Error);

				return result;
			}
		}

		public async Task<Player> GetPlayerAsync(long userId, bool onlineOnly = false)
		{
			lock (SyncObject)
			{
				if (ContainsKey(userId))
					return this[userId];
			}

			if (onlineOnly) return null;

			Player player = Resources.ObjectCache.GetCachedPlayer(userId);

			if (player != null) return player;

			player = await PlayerDb.GetAsync(userId);

			Resources.ObjectCache.CachePlayer(player);

			return player;
		}
	}
}