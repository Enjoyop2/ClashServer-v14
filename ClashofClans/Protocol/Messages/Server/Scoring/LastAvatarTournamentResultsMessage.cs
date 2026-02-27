using System;
using System.Collections.Generic;

using ClashofClans.Logic;

namespace ClashofClans.Protocol.Messages.Server.Scoring
{
	class LastAvatarTournamentResultsMessage : PiranhaMessage
	{
		public LastAvatarTournamentResultsMessage(Device device) : base(device)
		{
			Id = 26782;
		}
		private Player player { get; set; }
		public override void EncodeAsync()
		{
			Resources.Leaderboard.Update(null, null);
			List<Player> players = Resources.Leaderboard.GlobalPreviousSeasonPlayerRanking;
			int count = players.Count;

			Writer.WriteInt(count);

			for (int i = 0; i < count; i++)
			{
				player = players[i];

				player.AvatarRankingEntry(Writer, i + 1, true);
			}

			Writer.WriteInt(DateTime.Now.Month - 1);
			Writer.WriteInt(DateTime.Now.Year);
		}
	}
}