using System.Collections.Generic;

using ClashofClans.Database;
using ClashofClans.Logic;

namespace ClashofClans.Protocol.Messages.Server.Alliance
{
	public class AllianceListMessage : PiranhaMessage
	{
		public AllianceListMessage(Device device) : base(device)
		{
			Id = 29511;
		}
		public string AllianceName { get; set; }
		public override async void EncodeAsync()
		{
			List<Logic.Clan.Alliance> alliances = await AllianceDb.GetAllianceByNameAsync(AllianceName);
			Writer.WriteString(AllianceName);

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