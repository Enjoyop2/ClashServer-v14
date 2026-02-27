using System;
using System.Net;

using ClashofClans.Logic;
using ClashofClans.Utilities.Netty;

using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;

namespace ClashofClans.Core.Network.Handlers
{
	public class PacketHandler : ChannelHandlerAdapter
	{
		public PacketHandler()
		{
			Throttler = new Throttler(10, 500);
			Device = new Device(this);
		}

		public Device Device { get; set; }
		public IChannel Channel { get; set; }
		public Throttler Throttler { get; set; }

		public override void ChannelRead(IChannelHandlerContext context, object message)
		{
			ByteBuffer buffer = new ByteBuffer((IByteBuffer)message);

			if (buffer == null) return;

			if (Throttler.CanProcess())
			{
				Device.Process(buffer);
			}
			else
			{
				Logger.Log("Client reached ratelimit. Disconnecting...", GetType(), Logger.ErrorLevel.Warning);
				Device.Disconnect();
			}
		}

		public override void ChannelRegistered(IChannelHandlerContext context)
		{
			Channel = context.Channel;

			IPEndPoint remoteAddress = (IPEndPoint)Channel.RemoteAddress;

			Logger.Log($"Client {remoteAddress.Address.MapToIPv4()}:{remoteAddress.Port} connected.", GetType(),
				Logger.ErrorLevel.Debug);

			base.ChannelRegistered(context);
		}

		public override async void ChannelUnregistered(IChannelHandlerContext context)
		{
			if (Device?.Player?.Home != null)
			{
				Player player = await Resources.Players.GetPlayerAsync(Device.Player.Home.Id, true);
				if (player != null)
				{
					player.Home.Status = 0;

					if (player.Home.GameMatchmakingManager.searchTimer != null)
					{
						player.Home.GameMatchmakingManager.searchTimer.Stop();
					}

					if (Device.Player.Home.Battle.GetBattleStatus())
						Device.Player.Home.Battle.EndBattle(Device.Player, Device);

					if (player.Device.Session.SessionId == Device.Session.SessionId)
						Resources.Players.LogoutById(player.Home.Id);
				}
			}

			IPEndPoint remoteAddress = (IPEndPoint)Channel.RemoteAddress;

			Logger.Log($"Client {remoteAddress.Address.MapToIPv4()}:{remoteAddress.Port} disconnected.", GetType(),
				Logger.ErrorLevel.Debug);

			base.ChannelUnregistered(context);
		}

		public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
		{
			if (exception.GetType() != typeof(ReadTimeoutException) &&
				exception.GetType() != typeof(WriteTimeoutException))
				Logger.Log(exception, GetType(), Logger.ErrorLevel.Error);

			context.CloseAsync();
		}
	}
}