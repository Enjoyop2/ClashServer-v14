using System.Collections.Generic;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicSpeedUpConstructionCommand : LogicCommand
	{
		public LogicSpeedUpConstructionCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int BuildingId { get; set; }

		public override void Decode()
		{
			BuildingId = Reader.ReadInt();
			Reader.ReadInt();
			Reader.ReadInt();

			base.Decode();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			GameObjectManager objects = home.GameObjectManager;

			if (BuildingId - 504000000 < 0)
			{
				List<Building> buildings = objects.GetBuildings();

				int index = buildings.FindIndex(b => b.Id == BuildingId);
				if (index > -1)
				{
					Building building = buildings[index];
					building.SpeedUpConstruction();
				}
				else
				{
					Device.Disconnect($"Building {BuildingId} not found.");
				}
			}
			else if (BuildingId - 506000000 < 0)
			{
				//Device.Disconnect("Decorations are not supported for this command.");
			}
			else
			{
				List<VillageObject> villageObjects = objects.VillageObjects;

				int index = villageObjects.FindIndex(vo => vo.Id == BuildingId);
				if (index > -1)
				{
					VillageObject villageObject = villageObjects[index];
					villageObject.SpeedUpConstruction();
				}
				else
				{
					Device.Disconnect($"VillageObject {BuildingId} not found.");
				}
			}
		}
	}
}