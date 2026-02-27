using System.Collections.Generic;
using System.Numerics;

using ClashofClans.Files.Logic;
using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicUpgradeBuildingCommand : LogicCommand
	{
		public LogicUpgradeBuildingCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int BuildingId { get; set; }
		public bool UseAltResource { get; set; }

		public override void Decode()
		{
			BuildingId = Reader.ReadInt();
			UseAltResource = Reader.ReadBoolean();

			Reader.ReadByte();
			Reader.ReadByte();
			Reader.ReadByte();

			base.Decode();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			GameObjectManager objects = home.GameObjectManager;

			if (BuildingId - 504000000 < 0 && BuildingId != 500000000)
			{
				/*if (objects.IsWorkerAvailable())
                {*/
				List<Building> buildings = objects.GetBuildings();

				int index = buildings.FindIndex(b => b.Id == BuildingId);
				if (index > -1)
				{
					Building building = buildings[index];

					// For Builderbase Tutorial
					/*if (home.State == 1 && building.Data == 1000034 && building.Level == 0)
                        foreach (var b in buildings)
                            if (b.RequiredTownhallLevel(false) <= 2)
                            {
                                b.StartUnlocking();
                                b.FinishUnlocking();
                            }*/

					bool paid = home.UseResourceByName(
						UseAltResource
							? building.BuildingData.AltBuildResource[building.GetUpgradeLevel()]
							: building.BuildingData.BuildResource,
						building.BuildingData.BuildCost[building.GetUpgradeLevel() + 1]);

					if (paid)
						building.StartUpgrade();
					else
						Device.Disconnect("Payment failed.");
				}
				else
				{
					Device.Disconnect($"Building {BuildingId} not found.");
				}

				/*}
                else
                {
                    Device.Disconnect("No worker available!");
                }*/
			}
			else if (BuildingId == 500000000)
			{
				int townhallLevel = objects.GetTownhallLevel();
				List<Building> buildings = objects.GetBuildings();
				int index = buildings.FindIndex(b => b.Id == BuildingId);

				if (index > -1)
				{
					Building building = buildings[index];

					if (townhallLevel >= 11)
					{

						if (building.GetTownHallWeaponLevel() == 4)
						{
							building.SetTownHallWeaponLevel(0);
						}
						else
						{
							Device.Disconnect();
						}
					}

					bool paid = home.UseResourceByName(
						UseAltResource
							? building.BuildingData.AltBuildResource[building.GetUpgradeLevel()]
							: building.BuildingData.BuildResource,
						building.BuildingData.BuildCost[building.GetUpgradeLevel() + 1]);

					if (paid)
						building.StartUpgrade();
					else
						Device.Disconnect("Payment failed.");
				}
				else
				{
					Device.Disconnect("Townhall doesn't exist");
				}
			}
			/*else if (BuildingId - 507000000 < 0)
            {
                // Decorations
                //Device.Disconnect("Decorations are not supported for this command yet.");
            }*/
			else if (BuildingId - 508000000 < 0)
			{
				// Traps
				List<Trap> traps = objects.GetTraps();

				int index = traps.FindIndex(b => b.Id == BuildingId);

				if (index > -1)
				{
					Trap trap = traps[index];

					trap.Level++;
					//trap.StartUpgrade();
					/*// For Builderbase Tutorial
                    /*if (home.State == 1 && building.Data == 1000034 && building.Level == 0)
                        foreach (var b in buildings)
                            if (b.RequiredTownhallLevel(false) <= 2)
                            {
                                b.StartUnlocking();
                                b.FinishUnlocking();
                            }

                    var paid = home.UseResourceByName(trap.TrapData.BuildResource, trap.TrapData.BuildCost[trap.GetUpgradeLevel() + 1]);
                    /*var paid = home.UseResourceByName(
                        UseAltResource
                            ? trap.TrapData.AltBuildResource[building.GetUpgradeLevel()]
                            : building.BuildingData.BuildResource,
                        building.BuildingData.BuildCost[building.GetUpgradeLevel() + 1]);

                    if (paid)
                        trap.StartUpgrade();
                    else
                        Device.Disconnect("Payment failed.");*/
				}
				else
				{
					Device.Disconnect($"Trap {BuildingId} not found.");
				}

				//Device.Disconnect("Traps are not supported for this command yet.");
			}
			else
			{
				List<VillageObject> villageObjects = objects.VillageObjects;

				int index = villageObjects.FindIndex(vo => vo.Id == BuildingId);
				if (index > -1)
				{
					VillageObject villageObject = villageObjects[index];

					VillageObjects villageObjectData = villageObject.VillageObjectsData;

					bool paid = home.UseResourceByName(villageObjectData.BuildResource, villageObjectData.BuildCost);
					if (paid)
						villageObject.StartUpgrade();
					else
						Device.Disconnect("Payment failed.");
				}
				else if (BuildingId == 508000000)
				{
					List<Building> buildings = objects.GetBuildings();

					Building building = new Building(home)
					{
						Position = new Vector2(27, 57),
						Data = 39000000,
						Id = 500000000 + buildings.Count
					};
				}
				else
				{
					Device.Disconnect($"VillageObject {BuildingId} not found.");
				}
			}
		}
	}
}