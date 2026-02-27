using System.Collections.Generic;

using ClashofClans.Files.Logic;
using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicCancelConstructionCommand : LogicCommand
	{
		public LogicCancelConstructionCommand(Device device, ByteBuffer buffer) : base(device, buffer)
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
			GameObjectManager objects = home.GameObjectManager;

			if (BuildingId - 503000000 < 0)
			{
				List<Building> buildings = objects.GetBuildings();

				int index = buildings.FindIndex(b => b.Id == BuildingId);
				if (index > -1)
				{
					Building building = buildings[index];

					building.ConstructionTimer = null;

					int amount = building.BuildingData.BuildCost[building.GetUpgradeLevel() + 1];
					string resource = building.BuildingData.BuildResource;

					home.AddResourceByName(resource, amount);
				}
				else
				{
					Device.Disconnect($"Building {BuildingId} not found.");
				}
			}
			else if (BuildingId - 504000000 < 0)
			{
				List<Obstacle> obstacles = objects.GetObstacles();

				int index = obstacles.FindIndex(b => b.Id == BuildingId);
				if (index > -1)
				{
					Obstacle obstacle = obstacles[index];
					Obstacles data = obstacle.ObstacleData;

					obstacle.ClearingTimer = null;

					int amount = data.ClearCost / 2;
					string resource = data.ClearResource;

					home.AddResourceByName(resource, amount);
				}
				else
				{
					Device.Disconnect($"Obstacle {BuildingId} not found.");
				}
			}
			else if (BuildingId - 506000000 < 0)
			{
				// Decorations
				Device.Disconnect("Decorations are not supported for this command yet.");
			}
			else
			{
				// VillageObjects
				Device.Disconnect("VillageObjects are not supported for this command yet.");
			}
		}
	}
}