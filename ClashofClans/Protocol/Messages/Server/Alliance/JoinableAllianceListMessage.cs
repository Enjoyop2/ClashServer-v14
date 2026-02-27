using System.Collections.Generic;

using ClashofClans.Database;
using ClashofClans.Logic;

namespace ClashofClans.Protocol.Messages.Server.Alliance
{
	public class JoinableAllianceListMessage : PiranhaMessage
	{
		public JoinableAllianceListMessage(Device device) : base(device)
		{
			Id = 23429;
		}
		public override async void EncodeAsync()
		{
			List<Logic.Clan.Alliance> alliances = await AllianceDb.GetRandomAlliancesAsync();

			if (alliances.Count > 0)
			{
				Writer.WriteInt(alliances.Count);

				foreach (Logic.Clan.Alliance alliance in alliances)
				{
					alliance.AllianceHeaderEntry(Writer);
				}
			}
			else
			{
				Writer.WriteInt(0);
			}

			Writer.WriteInt(0);
		}
	}
}