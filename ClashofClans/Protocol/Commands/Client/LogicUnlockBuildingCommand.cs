using System.Collections.Generic;

using ClashofClans.Files.Logic;
using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicUnlockBuildingCommand : LogicCommand
	{
		public LogicUnlockBuildingCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int BuildingId { get; set; }

		public override void Decode()
		{
			BuildingId = Reader.ReadInt();

			base.Decode();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			List<Building> buildings = home.GameObjectManager.GetBuildings();

			int index = buildings.FindIndex(b => b.Id == BuildingId);
			if (index > -1)
			{
				Building building = buildings[index];
				Buildings data = building.BuildingData;

				int cost = data.BuildCost[0];
				string resource = data.BuildResource;

				if (home.UseResourceByName(resource, cost))
				{
					building.SetUpgradeLevel(-1);
					building.StartUpgrade();
				}
				else
				{
					Device.Disconnect("Payment failed.");
				}
			}
			else
			{
				Device.Disconnect($"Building {BuildingId} not found.");
			}
		}
	}
}