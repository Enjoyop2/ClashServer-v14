using System.Collections.Generic;

using ClashofClans.Logic.Manager.Items;

using Newtonsoft.Json;

namespace ClashofClans.Logic.Manager
{
	public class HeroesManager
	{
		[JsonProperty("heroes")] internal List<Hero> Heroes = new List<Hero>();

		public void UpdradeHero(int id, int lvl = 1)
		{
			int index = Heroes.FindIndex(hero => hero.Id == id);

			if (index > -1)
				Heroes[index].Level++;
			else
				Heroes.Add(new Hero
				{
					Id = id,
					Level = lvl
				});
		}

		public void IsHeroExist(int id)
		{
			int index = Heroes.FindIndex(hero => hero.Id == id);

			if (index == -1)
				Heroes.Add(new Hero
				{
					Id = id,
					Level = 0,
					State = 3
				});
		}
		public int AddHero(int id)
		{
			Heroes.Add(new Hero
			{
				Id = id,
				Level = 0
			});

			int index = Heroes.FindIndex(hero => hero.Id == id);

			return index;
		}
		public void SetSkinToHero(int heroId, int skinId)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);

			if (index > -1)
				Heroes[index].Skin = skinId;
			else
				Heroes[AddHero(heroId)].Skin = skinId;
		}
		public int GetSkinFromHero(int heroId)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);
			int skin = 0;

			if (index > -1)
			{
				skin = Heroes[index].Skin;
			}
			else
			{
				Logger.Log("Hero with id: " + heroId + " doesn't exist", null, Logger.ErrorLevel.Error);
				skin = -1;
			}
			return skin;
		}
		public void SetStateToHero(int heroId, int state)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);

			if (index > -1)
				Heroes[index].State = state;
			else
				Logger.Log("Hero with id: " + heroId + " doesn't exist", null, Logger.ErrorLevel.Error);
		}
		public int GetStateFromHero(int heroId)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);
			int mode = 0;

			if (index > -1)
				mode = Heroes[index].State;
			else
				Logger.Log("Hero with id: " + heroId + " doesn't exist", null, Logger.ErrorLevel.Error);
			return mode;
		}
		public void SetModeToHero(int heroId, int mode)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);

			if (index > -1)
				Heroes[index].Mode = mode;
			else
				Logger.Log("Hero with id: " + heroId + " doesn't exist", null, Logger.ErrorLevel.Error);
		}
		public int GetModeFromHero(int heroId)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);
			int mode = 0;

			if (index > -1)
				mode = Heroes[index].Mode;
			else
				Logger.Log("Hero with id: " + heroId + " doesn't exist", null, Logger.ErrorLevel.Error);
			return mode;
		}
		public void SetPetToHero(int heroId, int petId)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);

			if (index > -1)
				Heroes[index].PetId = petId;
			else
				Heroes[AddHero(heroId)].PetId = petId;
		}
		public int GetPetFromHero(int heroId)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);
			int petId = 0;

			if (index > -1)
				petId = Heroes[index].PetId;
			else
				Logger.Log("Hero with id: " + heroId + " doesn't exist", null, Logger.ErrorLevel.Error);

			return petId;
		}
		public void RemovePetFromHero(int heroId)
		{
			int index = Heroes.FindIndex(hero => hero.Id == heroId);

			if (index > -1)
				Heroes[index].PetId = 0;
			else
				Logger.Log("Hero with id: " + heroId + " doesn't exist", null, Logger.ErrorLevel.Error);
		}
		public int GetID(int id)
		{
			switch (id)
			{
				case 1000022:
					return 28000000;
				case 1000025:
					return 28000001;
				case 1000030:
					return 28000002;
				case 1000066:
					return 28000004;
				default:
					return -1;
			}
		}
	}
}