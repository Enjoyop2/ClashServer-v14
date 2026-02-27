using System.Collections.Generic;
using System.Numerics;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicMoveBuildingCommand : LogicCommand
	{
		public LogicMoveBuildingCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int BuildingId { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public override void Decode()
		{
			Reader.ReadInt();
			BuildingId = Reader.ReadVInt();
			X = Reader.ReadVInt();
			Y = Reader.ReadVInt();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;

			if (BuildingId - 504000000 < 0)
			{
				List<Building> buildings = home.GameObjectManager.GetBuildings();

				int index = buildings.FindIndex(b => b.Id == BuildingId);
				if (index > -1)
				{
					Building building = buildings[index];
					building.Position = new Vector2(X, Y);
				}
				else
				{
					Device.Disconnect($"Building {BuildingId} not found.");
				}
			}
			else if (BuildingId - 505000000 < 0)
			{
				List<Trap> traps = home.GameObjectManager.GetTraps();

				int index = traps.FindIndex(b => b.Id == BuildingId);
				if (index > -1)
				{
					Trap trap = traps[index];
					trap.Position = new Vector2(X, Y);
				}
				else
				{
					Device.Disconnect($"Trap {BuildingId} not found.");
				}
			}
			else if (BuildingId - 507000000 < 0)
			{
				List<Deco> decos = home.GameObjectManager.GetDecos();

				int index = decos.FindIndex(b => b.Id == BuildingId);
				if (index > -1)
				{
					Deco deco = decos[index];
					deco.Position = new Vector2(X, Y);
				}
				else
				{
					Device.Disconnect($"Deco {BuildingId} not found.");
				}
			}
			else
			{
				Device.Disconnect("Unhandled object.");
			}
		}
	}
}