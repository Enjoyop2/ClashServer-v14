using System.Collections.Generic;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	class LogicUpgradeWeaponCommand : LogicCommand
	{
		public LogicUpgradeWeaponCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}
		private int BuildingId { get; set; }
		public override void Decode()
		{
			BuildingId = Reader.ReadInt();
			Reader.ReadInt();
			Reader.ReadInt();
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

					building.UpgradeTownHallWeapon();
				}
			}
		}
	}
}