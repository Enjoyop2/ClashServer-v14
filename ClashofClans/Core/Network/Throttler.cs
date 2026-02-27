using System;

namespace ClashofClans.Core.Network
{
	public class Throttler
	{
		private readonly int _interval;
		private readonly int _maxPackets;
		private int _count;
		private DateTime _lastReset = DateTime.UtcNow;

		public Throttler(int maxPackets, int interval)
		{
			_maxPackets = maxPackets;
			_interval = interval;
		}

		public bool CanProcess()
		{
			if (_count++ < _maxPackets) return true;
			if (GetMillisecondsSinceLastReset() < _interval) return false;

			_count = 1;
			_lastReset = DateTime.UtcNow;
			return true;
		}

		private int GetMillisecondsSinceLastReset()
		{
			return (int)DateTime.UtcNow.Subtract(_lastReset).TotalMilliseconds;
		}
	}
}