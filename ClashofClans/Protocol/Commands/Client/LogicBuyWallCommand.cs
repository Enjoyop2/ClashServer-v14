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
	public class LogicBuyWallCommand : LogicCommand
	{
		public LogicBuyWallCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int BuildingData { get; set; }
		public List<Vector2> Positions { get; set; }

		public override void Decode()
		{
			int count = Reader.ReadInt();

			Positions = new List<Vector2>(count);

			for (int i = 0; i < count; i++)
			{
				int x = Reader.ReadInt();
				int y = Reader.ReadInt();

				Positions.Add(new Vector2(x, y));
			}

			BuildingData = Reader.ReadInt();

			base.Decode();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			List<Building> buildings = home.GameObjectManager.GetBuildings();

			//if (home.GameObjectManager.IsWorkerAvailable())
			//{
			Buildings data = Csv.Tables.Get(Csv.Files.Buildings).GetDataWithId<Buildings>(BuildingData);
			int cost = data.BuildCost[0];

			if (home.UseResourceByName(data.BuildResource, cost))
			{
				int wallId = buildings.Count;
				int count = 0;

				foreach (Vector2 pos in Positions)
				{
					Building building = new Building(home)
					{
						Position = pos,
						Data = BuildingData,
						Id = 500000000 + buildings.Count,
						WallIndex = wallId
					};

					if (count == 0)
						building.WallPosition = 1;

					building.WallX = count++;

					buildings.Add(building);
				}
			}
			else
			{
				Device.Disconnect("Failed to buy wall.");
			}
			/*}
            else
            {
                Device.Disconnect("No worker available!");
            }*/
		}
	}
}