using ClashofClans.Logic;
using ClashofClans.Protocol.Messages.Server.Alliance;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Messages.Client.Alliance
{
	public class AskForAllianceDataMessage : PiranhaMessage
	{
		public AskForAllianceDataMessage(Device device, ByteBuffer buffer) : base(device, buffer)
		{
			RequiredState = Device.State.NotDefinied;
		}

		public long AllianceId { get; set; }

		public override void Decode()
		{
			AllianceId = Reader.ReadLong();
		}

		public override async void ProcessAsync()
		{
			Logic.Clan.Alliance alliance = await Resources.Alliances.GetAllianceAsync(AllianceId);

			if (alliance != null)
				await new AllianceDataMessage(Device)
				{
					Alliance = alliance
				}.SendAsync();
		}
	}
}