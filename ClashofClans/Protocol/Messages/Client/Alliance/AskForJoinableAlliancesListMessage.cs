using ClashofClans.Logic;
using ClashofClans.Protocol.Messages.Server.Alliance;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Messages.Client.Alliance
{
	public class AskForJoinableAlliancesListMessage : PiranhaMessage
	{
		public AskForJoinableAlliancesListMessage(Device device, ByteBuffer buffer) : base(device, buffer)
		{
			RequiredState = Device.State.NotDefinied;
		}
		public override async void ProcessAsync()
		{
			await new JoinableAllianceListMessage(Device).SendAsync();
		}
	}
}