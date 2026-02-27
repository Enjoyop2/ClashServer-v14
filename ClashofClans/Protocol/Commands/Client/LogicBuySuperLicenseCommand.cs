using System.Collections.Generic;

using ClashofClans.Logic;
using ClashofClans.Utilities.Netty;
using ClashofClans.Utilities.Utils;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicBuySuperLicenseCommand : LogicCommand
	{
		public LogicBuySuperLicenseCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		private int UnitId { get; set; }

		public override void Decode()
		{
			Reader.ReadInt();
			UnitId = Reader.ReadInt();
			Reader.ReadInt();
		}

		public override void Execute()
		{
			List<int> licenses = Device.Player.Home.GameObjectManager.GetSuperLicences();

			if (licenses.Count < 2)
			{
				licenses.Add(UnitId);
				licenses.Add(TimeUtils.CurrentUnixTimestamp + 259200);
			}
			else if (licenses.Count < 4)
			{
				licenses.Add(UnitId);
				licenses.Add(TimeUtils.CurrentUnixTimestamp + 259200);
			}
			else if (licenses[1] < TimeUtils.CurrentUnixTimestamp)
			{
				licenses[0] = UnitId;
				licenses[1] = TimeUtils.CurrentUnixTimestamp + 259200;
			}
			else if (licenses[3] < TimeUtils.CurrentUnixTimestamp)
			{
				licenses[2] = UnitId;
				licenses[3] = TimeUtils.CurrentUnixTimestamp + 259200;
			}

			Device.Player.Home.GameObjectManager.SetSuperLicences(licenses);
		}
	}
}