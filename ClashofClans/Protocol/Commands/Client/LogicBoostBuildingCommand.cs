using System.Collections.Generic;
using System.Linq;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicBoostBuildingCommand : LogicCommand
	{
		public LogicBoostBuildingCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public List<int> Buildings { get; set; }

		public override void Decode()
		{
			int count = Reader.ReadInt();
			Buildings = new List<int>(count);

			for (int i = 0; i < count; i++)
			{
				Buildings.Add(Reader.ReadInt());
			}

			base.Decode();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			List<Building> buildings = home.GameObjectManager.GetBuildings();

			foreach (Building building in from id in Buildings select buildings.FindIndex(x => x.Id == id) into index where index > -1 select buildings[index])
			{
				building.StartBoost();
			}
		}
	}
}