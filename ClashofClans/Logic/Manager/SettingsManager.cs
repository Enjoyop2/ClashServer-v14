using Newtonsoft.Json;

namespace ClashofClans.Logic.Manager
{
	public class SettingsManager
	{
		[JsonProperty("triggerHeroAbilityOnDeath")] public int TriggerHeroAbilityOnDeath { get; set; }

		public void SetTriggerHeroAbilityOnDeath(int state)
		{
			TriggerHeroAbilityOnDeath = state;
		}
	}
}