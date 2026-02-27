using System.Collections.Generic;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicCollectResourcesCommand : LogicCommand
	{
		public LogicCollectResourcesCommand(Device device, ByteBuffer buffer) : base(device, buffer)
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
			List<Building> buildings = objects.GetBuildings();

			int index = buildings.FindIndex(b => b.Id == BuildingId);
			if (index > -1)
			{
				Building building = buildings[index];

				Logger.Log($"Collected: {building.ResourceProductionComponent.AvailableToCollect}", GetType(), Logger.ErrorLevel.Debug);

				home.ComponentManager.CollectResources(building.Data);
			}
			else
			{
				Device.Disconnect($"Building {BuildingId} not found.");
			}
		}
	}
}