using System.Collections.Generic;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicToggleHeroSleepCommand : LogicCommand
	{
		public LogicToggleHeroSleepCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}
		private int BuildingId { get; set; }
		private int HeroId { get; set; }
		public override void Decode()
		{
			BuildingId = Reader.ReadInt();
			Reader.ReadInt();
			Reader.ReadByte();
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

				HeroId = building.GetBuildingData();
			}

			int hero = Device.Player.Home.Characters.GetID(HeroId);

			home.Characters.SetStateToHero(hero, home.Characters.GetStateFromHero(hero) == 3 ? 2 : 3);
		}
	}
}