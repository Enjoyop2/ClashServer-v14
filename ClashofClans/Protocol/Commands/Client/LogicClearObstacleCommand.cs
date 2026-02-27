using System.Collections.Generic;
using System.Linq;

using ClashofClans.Files;
using ClashofClans.Files.Logic;
using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Manager.Items.GameObjects;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicClearObstacleCommand : LogicCommand
	{
		public LogicClearObstacleCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public int ObstacleId { get; set; }

		public override void Decode()
		{
			ObstacleId = Reader.ReadInt();

			base.Decode();
		}

		public override void Execute()
		{
			Home home = Device.Player.Home;
			GameObjectManager objects = home.GameObjectManager;
			List<Obstacle> obstacles = objects.GetObstacles();

			int index = obstacles.FindIndex(o => o.Id == ObstacleId);
			if (index > -1)
			{
				Obstacle obstacle = obstacles[index];
				Obstacles data = obstacle.ObstacleData;

				if (!data.IsTombstone && home.State == 1 && objects.GetBuilderhallLevel() < Csv.Tables
						.Get(Csv.Files.Globals)
						.GetData<Globals>("VILLAGE2_DO_NOT_ALLOW_CLEAR_OBSTACLE_TH").NumberValue) return;

				if (home.UseResourceByName(data.ClearResource, data.ClearCost))
				{
					if (data.IsTombstone)
						foreach (Obstacle o in obstacles.Where(o => o.ObstacleData.IsTombstone))
							o.StartClearing();
					else
						obstacle.StartClearing();

					// TODO: loot
				}
				else
				{
					Device.Disconnect("Payment failed.");
				}
			}
			else
			{
				Device.Disconnect($"Obstacle {ObstacleId} not found.");
			}
		}
	}
}