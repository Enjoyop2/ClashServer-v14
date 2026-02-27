using System;
using System.Collections.Generic;
using System.Xml.Linq;

using ClashofClans.Files;
using ClashofClans.Files.Logic;
using ClashofClans.Logic.Clan;
using ClashofClans.Logic.Home.Slots;
using ClashofClans.Logic.Home.StreamEntry;
using ClashofClans.Logic.Manager;
using ClashofClans.Logic.Sessions;

using Newtonsoft.Json;

namespace ClashofClans.Logic.Home
{
	public class Home
	{
		[JsonProperty("clanInfo")] public AllianceInfo AllianceInfo = new AllianceInfo();
		[JsonIgnore] public ComponentManager ComponentManager = new ComponentManager();
		[JsonIgnore] public GameObjectManager GameObjectManager = new GameObjectManager();
		[JsonIgnore] public VariableSlots VariableSlots = new VariableSlots();
		[JsonProperty("resources")] public ResourceSlots Resources = new ResourceSlots();
		[JsonProperty("characters")] public HeroesManager Characters = new HeroesManager();
		[JsonProperty("units")] public UnitsManager Units = new UnitsManager();
		[JsonProperty("settings")] public SettingsManager Settings = new SettingsManager();
		[JsonIgnore] public List<Session> Sessions = new List<Session>(50);
		[JsonProperty("stream")] public List<AvatarStreamEntry> Stream = new List<AvatarStreamEntry>(40);
		[JsonIgnore] public Time Time = new Time();
		[JsonIgnore] public Battle Battle = new Battle();
		[JsonIgnore] public GameMatchmakingManager GameMatchmakingManager = new GameMatchmakingManager();

		public Home()
		{
			GameObjectManager.Home = this;
			ComponentManager.Home = this;
		}

		public Home(long id, string token)
		{
			Id = id;
			UserToken = token;

			PreferredDeviceLanguage = "EN";

			Name = "Deed";
			ExpLevel = 1;
			Status = 0;

			Diamonds = 10000000;
			Resources.Initialize();

			GameObjectManager.Home = this;
			ComponentManager.Home = this;
			GameObjectManager.LoadJson(Levels.StartingHome);
		}

		[JsonProperty("name")] public string Name { get; set; }
		[JsonProperty("token")] public string UserToken { get; set; }
		[JsonProperty("nameSet")] public int NameSet { get; set; }
		[JsonProperty("createdIp")] public string CreatedIpAddress { get; set; }
		[JsonProperty("highId")] public int HighId { get; set; }
		[JsonProperty("lowId")] public int LowId { get; set; }
		[JsonProperty("diamonds")] public int Diamonds { get; set; }
		[JsonProperty("language")] public string PreferredDeviceLanguage { get; set; }
		[JsonProperty("fcbId")] public string FacebookId { get; set; }
		[JsonProperty("totalSessions")] public int TotalSessions { get; set; }
		[JsonProperty("totalPlayTimeSeconds")] public int TotalPlayTimeSeconds { get; set; }

		[JsonProperty("lastSave")] public DateTime LastSaveTime { get; set; }

		/// <summary>
		///     1 = Online, 0 = Offline
		/// </summary>
		[JsonProperty("status")]
		public int Status { get; set; }

		/// <summary>
		///     1 = Builderbase, 0 = Home Village
		/// </summary>
		[JsonProperty("state")]
		public int State { get; set; }

		// Player Stats
		[JsonProperty("expLevel")] public int ExpLevel { get; set; }
		[JsonProperty("expPoints")] public int ExpPoints { get; set; }
		[JsonProperty("attacksWon")] public int AttacksWon { get; set; }
		[JsonProperty("defensesWon")] public int DefensesWon { get; set; }
		[JsonProperty("league")] public int League { get; set; }
		[JsonProperty("trophies")] public int Trophies { get; set; }
		[JsonProperty("duelTrophies")] public int DuelTrophies { get; set; }
		[JsonProperty("currentSeasonMonth")] public int CurrentSeasonMonth { get; set; }
		[JsonProperty("previousSeasonMonth")] public int PreviousSeasonMonth { get; set; }
		[JsonProperty("previousSeasonTrophies")] public int PreviousSeasonTrophies { get; set; }
		[JsonProperty("playerLabels")] public List<int> PlayerLabels = new List<int>();

		[JsonIgnore]
		public long Id
		{
			get => ((long)HighId << 32) | (LowId & 0xFFFFFFFFL);
			set
			{
				HighId = Convert.ToInt32(value >> 32);
				LowId = (int)value;
			}
		}

		/// <summary>
		///     Add's experience Points to the players account and increments the players level if available
		/// </summary>
		/// <param name="expPoints"></param>
		public void AddExpPoints(int expPoints)
		{
			ExpPoints += expPoints;

			while (true)
			{
				ExperienceLevels data = Csv.Tables.Get(Csv.Files.ExperienceLevels).GetDataWithId<ExperienceLevels>(ExpLevel - 1);
				if (data == null) return;

				if (ExpPoints < data.ExpPoints) return;

				ExpLevel++;
				ExpPoints -= data.ExpPoints;

				if (ExpPoints >= data.ExpPoints)
					continue;

				break;
			}
		}

		public void Tick()
		{
			GameObjectManager.Tick();
		}

		public void FastForward(int seconds)
		{
			GameObjectManager.FastForward(seconds);
		}

		public void Reset(bool notResetGameObjects = false)
		{
			Diamonds = 10000000;
			Resources.Initialize();

			Name = "NoName";
			NameSet = 0;
			ExpLevel = 1;
			ExpPoints = 0;
			Trophies = 0;

			State = 0;

			if (!notResetGameObjects)
				GameObjectManager.LoadJson(Levels.StartingHome);
		}

		private bool UseResource(int resourceId, int spentAmount)
		{
			int resAmount = Resources.GetById(resourceId).Count;

			if (resAmount - spentAmount < 0)
			{
				return false;
			}

			Resources.Remove(resourceId, spentAmount);
			return true;
		}

		private int AddResource(string resourceName, int resourceId, int receivedAmount)
		{
			int storageAvailable = ComponentManager.MaxStoredResource(resourceName);
			int storageUsed = Resources.GetCount(resourceId);

			if (storageUsed > storageAvailable) return -1;

			if (storageUsed + receivedAmount > storageAvailable)
			{
				receivedAmount = storageAvailable - storageUsed;
			}

			Resources.Add(resourceId, receivedAmount);
			return receivedAmount;
		}

		public bool UseDiamonds(int spentAmount)
		{
			if (Diamonds - spentAmount < 0)
			{
				return false;
			}

			Diamonds -= spentAmount;
			return true;
		}

		public bool UseResourceByName(string name, int amount)
		{
			switch (name)
			{
				case "Gold":
					{
						return UseResource(3000001, amount);
					}

				case "Elixir":
					{
						return UseResource(3000002, amount);
					}

				case "DarkElixir":
					{
						return UseResource(3000003, amount);
					}

				case "Diamonds":
					{
						return UseDiamonds(amount);
					}

				case "Gold2":
					{
						return UseResource(3000007, amount);
					}

				case "Elixir2":
					{
						return UseResource(3000008, amount);
					}

				case "Medals":
					{
						return UseResource(3000009, amount);
					}
			}

			return false;
		}

		public int AddResourceByName(string name, int amount)
		{
			switch (name)
			{
				case "Gold":
					{
						return AddResource(name, 3000001, amount);
					}

				case "Elixir":
					{
						return AddResource(name, 3000002, amount);
					}

				case "DarkElixir":
					{
						return AddResource(name, 3000003, amount);
					}

				case "Diamonds":
					{
						Diamonds += amount;
						break;
					}

				case "Gold2":
					{
						return AddResource(name, 3000007, amount);
					}

				case "Elixir2":
					{
						return AddResource(name, 3000008, amount);
					}

				case "Medals":
					{
						return AddResource(name, 3000009, amount);
					}
			}

			return amount;
		}
	}
}