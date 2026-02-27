using System.Collections.Generic;
using System.Threading.Tasks;

using ClashofClans.Logic.Clan;

namespace ClashofClans.Database.Cache
{
	public class Alliances : Dictionary<long, Alliance>
	{
		private readonly object _syncObject = new object();

		public void Add(Alliance alliance)
		{
			lock (_syncObject)
			{
				if (!ContainsKey(alliance.Id)) Add(alliance.Id, alliance);
			}
		}

		public new void Remove(long allianceId)
		{
			lock (_syncObject)
			{
				if (ContainsKey(allianceId))
				{
					Alliance alliance = this[allianceId];

					alliance.Save();

					bool result = base.Remove(allianceId);

					if (!result)
						Logger.Log($"Couldn't remove alliance {allianceId}", GetType(), Logger.ErrorLevel.Error);
				}
			}
		}

		public async Task<Alliance> GetAllianceAsync(long allianceId, bool onlineOnly = false)
		{
			lock (_syncObject)
			{
				if (ContainsKey(allianceId))
					return this[allianceId];
			}

			if (onlineOnly) return null;

			Alliance alliance = Resources.ObjectCache.GetCachedAlliance(allianceId);

			if (alliance != null) return alliance;

			alliance = await AllianceDb.GetAsync(allianceId);

			Resources.ObjectCache.CacheAlliance(alliance);

			return alliance;
		}

		/*/// <summary>
        ///     Returns a list of random cached Alliances
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<Alliance>> GetRandomAlliancesAsync(int count = 40, bool notFull = true)
        {
            var alliances = new List<Alliance>(count);

            for (var i = 0; i < count; i++)
            {
                var alliance = await Redis.GetRandomAllianceAsync();

                if (alliance != null && alliances.FindIndex(a => a.Id == alliance.Id) == -1)
                {
                    if (notFull)
                    {
                        if (alliance.Members.Count < 50) alliances.Add(alliance);
                    }
                    else
                    {
                        alliances.Add(alliance);
                    }
                }
            }

            return alliances;
        }*/
	}
}