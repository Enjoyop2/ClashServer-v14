using System.Collections.Generic;
using System.Numerics;

using ClashofClans.Files;
using ClashofClans.Files.Logic;
using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicBuyTrapCommand : LogicCommand
	{
		public LogicBuyTrapCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int BuildingData { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public override void Decode()
		{
			X = Reader.ReadInt();
			Y = Reader.ReadInt();

			BuildingData = Reader.ReadInt();

			base.Decode();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			List<Trap> traps = home.GameObjectManager.GetTraps();

			Traps data = Csv.Tables.Get(Csv.Files.Traps).GetDataWithId<Traps>(BuildingData);
			int cost = data.BuildCost[0];

			Trap trap = new Trap(home)
			{
				Position = new Vector2(X, Y),
				Data = BuildingData,
				Id = 500000000 + traps.Count
			};

			if (home.UseResourceByName(trap.TrapData.BuildResource, cost))
			{
				trap.SetUpgradeLevel(-1);
				trap.StartUpgrade();

				traps.Add(trap);
			}
			else
			{
				Device.Disconnect("Failed to buy building.");
			}
		}
	}
}