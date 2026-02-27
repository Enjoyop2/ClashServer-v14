using System.Collections.Generic;
using System.Numerics;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicMoveMultipleBuildingsCommand : LogicCommand
	{
		public LogicMoveMultipleBuildingsCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int Count { get; set; }

		public override void Decode()
		{
			Count = Reader.ReadInt();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			List<Building> buildings = home.GameObjectManager.GetBuildings();

			for (int i = 0; i < Count; i++)
			{
				int x = Reader.ReadInt();
				int y = Reader.ReadInt();
				int id = Reader.ReadInt();

				int index = buildings.FindIndex(b => b.Id == id);
				if (index > -1)
				{
					Building building = buildings[index];
					building.Position = new Vector2(x, y);
				}
				else
				{
					Device.Disconnect($"Building {id} not found.");
				}
			}

			base.Decode();
		}
	}
}