using ClashofClans.Logic;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	public class LogicEndCombatCommand : LogicCommand
	{
		public LogicEndCombatCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public override void Decode()
		{
			Reader.ReadInt();
		}
	}
}