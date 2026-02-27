using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

using ClashofClans.Database;
using ClashofClans.Files;
using ClashofClans.Files.CsvHelpers;
using ClashofClans.Files.Logic;
using ClashofClans.Logic;
using ClashofClans.Logic.Clan;
using ClashofClans.Utilities;

namespace ClashofClans.Core.Leaderboard
{
	public class Leaderboard
	{
		private readonly System.Timers.Timer _timer = new System.Timers.Timer(20000);

		public List<Alliance> GlobalAllianceRanking = new List<Alliance>(200);
		public List<Player> LeagueMemberList = new List<Player>(200);
		public List<Player> GlobalPlayerRanking = new List<Player>(200);
		public List<Player> GlobalPreviousSeasonPlayerRanking = new List<Player>(200);
		public Dictionary<string, List<Player>> LocalPlayerRanking = new Dictionary<string, List<Player>>(18);

		public Leaderboard()
		{
			_timer.Elapsed += Update;
			_timer.Start();

			foreach (Data regions in Csv.Tables.Get(Csv.Files.Regions).GetDatas())
				LocalPlayerRanking.Add(((Regions)regions).Name, new List<Player>(200));

			Update(null, null);
		}

		public async void UpdateLeagueMemberList(int league)
		{
			await Task.Run(async () =>
			{
				try
				{
					List<Player> leagueMemberList = await PlayerDb.GetLeagueMemberListAsync(league);
					for (int i = 0; i < leagueMemberList.Count; i++)
						LeagueMemberList.UpdateOrInsert(i, leagueMemberList[i]);
				}
				catch (Exception exception)
				{
					Logger.Log($"Error while updating leaderboads {exception}", GetType(), Logger.ErrorLevel.Error);
				}
			});
		}

		public async void Update(object state, ElapsedEventArgs args)
		{
			await Task.Run(async () =>
			{
				try
				{
					List<Player> currentGlobalPlayerRanking = await PlayerDb.GetGlobalPlayerRankingAsync();
					for (int i = 0; i < currentGlobalPlayerRanking.Count; i++)
					{
						GlobalPlayerRanking.UpdateOrInsert(i, currentGlobalPlayerRanking[i]);
					}

					List<Player> previousGlobalPlayerRanking = await PlayerDb.GetPreviousSeasonGlobalPlayerRankingAsync();
					for (int i = 0; i < previousGlobalPlayerRanking.Count; i++)
					{
						GlobalPreviousSeasonPlayerRanking.UpdateOrInsert(i, previousGlobalPlayerRanking[i]);
					}

					foreach ((string key, List<Player> value) in LocalPlayerRanking)
					{
						List<Player> currentLocalPlayerRanking = await PlayerDb.GetLocalPlayerRankingAsync(key);
						for (int i = 0; i < currentLocalPlayerRanking.Count; i++)
						{
							value.UpdateOrInsert(i, currentLocalPlayerRanking[i]);
						}
					}

					List<Alliance> currentGlobalAllianceRanking = await AllianceDb.GetGlobalAlliancesAsync();
					for (int i = 0; i < currentGlobalAllianceRanking.Count; i++)
					{
						GlobalAllianceRanking.UpdateOrInsert(i, currentGlobalAllianceRanking[i]);
					}
				}
				catch (Exception exception)
				{
					Logger.Log($"Error while updating leaderboads {exception}", GetType(), Logger.ErrorLevel.Error);
				}
			});
		}
	}
}