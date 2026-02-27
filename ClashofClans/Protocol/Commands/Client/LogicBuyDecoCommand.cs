using System.Collections.Generic;
using System.Numerics;

using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicBuyDecoCommand : LogicCommand
	{
		public LogicBuyDecoCommand(Device device, ByteBuffer buffer) : base(device, buffer)
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
			Reader.ReadInt();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			GameObjectManager objects = home.GameObjectManager;
			List<Deco> decos = objects.GetDecos();

			Deco deco = new Deco(home)
			{
				Position = new Vector2(X, Y),
				Data = BuildingData,
				Id = 180000000 + decos.Count
			};

			int cost = deco.DecoData.BuildCost;

			if (home.UseResourceByName(deco.DecoData.BuildResource, cost))
			{
				decos.Add(deco);
			}
			else
			{
				Device.Disconnect("Failed to buy deco.");
			}
		}
	}
}