using System.Collections.Generic;

using Newtonsoft.Json;

namespace ClashofClans.Logic.Manager.Items
{
	public class SuperLicences
	{
		[JsonProperty("licence_ends")]
		public List<int> LicenceEnds { get; set; }
	}
}