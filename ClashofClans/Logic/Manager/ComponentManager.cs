using System.Collections.Generic;
using System.Linq;

using ClashofClans.Logic.Manager.Items;
using ClashofClans.Logic.Manager.Items.Components;
using ClashofClans.Logic.Manager.Items.GameObjects;

namespace ClashofClans.Logic.Manager
{
	public class ComponentManager
	{
		public ComponentManager()
		{
			Components = new Dictionary<int, List<Component>>();
		}

		public Home.Home Home { get; set; }
		public Dictionary<int, List<Component>> Components { get; set; }

		public void CollectResources(int data)
		{
			List<Building> buildings = Home.GameObjectManager.GetBuildings();
			foreach (Building collector in buildings.Where(x => x.Data == data))
				collector.ResourceProductionComponent?.CollectResources();
		}

		public int MaxStoredResource(string resource)
		{
			if (!Components.ContainsKey(6)) return 0;

			List<Component> components = Components[6];
			int maxStored = 0;

			foreach (ResourceStorageComponent resourceStorageComponent in components.Cast<ResourceStorageComponent>())
			{
				Building building = (Building)resourceStorageComponent.Parent;

				switch (resource)
				{
					case "Gold":
						{
							maxStored += resourceStorageComponent.MaxStoredGold(building.GetUpgradeLevel());
							break;
						}

					case "Elixir":
						{
							maxStored += resourceStorageComponent.MaxStoredElixir(building.GetUpgradeLevel());
							break;
						}

					case "DarkElixir":
						{
							maxStored += resourceStorageComponent.MaxStoredDarkElixir(building.GetUpgradeLevel());
							break;
						}

					case "Gold2":
						{
							maxStored += resourceStorageComponent.MaxStoredGold2(building.GetUpgradeLevel());
							break;
						}

					case "Elixir2":
						{
							maxStored += resourceStorageComponent.MaxStoredElixir2(building.GetUpgradeLevel());
							break;
						}
				}
			}

			return maxStored;
		}

		public void AddComponent(Component component)
		{
			if (!Components.ContainsKey(component.Type)) Components.Add(component.Type, new List<Component>());

			Components[component.Type].Add(component);
		}

		public void RemoveComponent(Component component)
		{
			if (Components.ContainsKey(component.Type))
				Components[component.Type].Remove(component);
		}

		public void Clear()
		{
			Components = new Dictionary<int, List<Component>>();
		}
	}
}