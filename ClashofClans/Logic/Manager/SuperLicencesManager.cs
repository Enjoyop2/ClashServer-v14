using System.Collections.Generic;

using ClashofClans.Logic.Manager.Items;

using Newtonsoft.Json;

namespace ClashofClans.Logic.Manager
{
	public class SuperLicencesManager
	{
		public List<int> Licenses { get; set; }
		public SuperLicences Save()
		{
			SuperLicences superLicences = new SuperLicences()
			{
				LicenceEnds = Licenses
			};

			return superLicences;
		}

		public void Load(string Json)
		{
			SuperLicences superLicences = JsonConvert.DeserializeObject<SuperLicences>(Json);

			Licenses = superLicences.LicenceEnds;
		}
	}
}