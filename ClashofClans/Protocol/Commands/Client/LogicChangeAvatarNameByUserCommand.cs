using ClashofClans.Logic;
using ClashofClans.Logic.Home;
using ClashofClans.Protocol.Commands.Server;
using ClashofClans.Protocol.Messages.Server;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Protocol.Commands.Client
{
	class LogicChangeAvatarNameByUserCommand : LogicCommand
	{
		public LogicChangeAvatarNameByUserCommand(Device device, ByteBuffer buffer) : base(device, buffer)
		{
		}

		public string Name { get; set; }

		public override void Decode()
		{
			Name = Reader.ReadString();
			Reader.ReadByte();
			Reader.ReadByte();
		}

		public override async void Execute()
		{
			if (Name.Length >= 3 && Name.Length <= 15)
			{
				Home home = Device.Player.Home;

				if (home.NameSet < 2)
				{
					home.Name = Name;
					home.NameSet++;

					await new AvailableServerCommandMessage(Device)
					{
						Command = new LogicChangeAvatarNameCommand(Device)
						{
							Name = Name
						}.Handle()
					}.SendAsync();

					// SKIP MISSIONS
					//await new OwnHomeDataMessage(Device).SendAsync();
				}
				else
				{
					Device.Disconnect("Can't change mame anymore.");
				}
			}
			else
			{
				Device.Disconnect("Invalid name.");
			}
		}
	}
}