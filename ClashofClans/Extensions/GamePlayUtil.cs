using System;

using ClashofClans.Files;
using ClashofClans.Files.CsvHelpers;
using ClashofClans.Files.Logic;

namespace ClashofClans.Extensions
{
	public class GamePlayUtil
	{

		private static DataTable GetGlobalsDataTable()
		{
			return Csv.Tables.Get(Csv.Files.Globals);
		}

		private int GetGlobalNumberValue(string rowName)
		{
			return GetGlobalsDataTable().GetData<Globals>(rowName).NumberValue;
		}

		private static int CalculateResourceCost(int sup, int inf, int supCost, int infCost, int amount)
		{
			return (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;
		}

		private static int CalculateSpeedUpCost(int sup, int inf, int supCost, int infCost, int amount)
		{
			return (int)Math.Round((supCost - infCost) * (long)(amount - inf) / (sup - inf * 1.0)) + infCost;
		}

		public int GetResourceDiamondCost(int resourceCount, string resource)
		{
			int result = 0;
			if (resource == "DarkElixir")
			{
				result = GetDarkElixirDiamondCost(resourceCount);
			}
			else
			{

				if (resourceCount >= 1)
				{
					int supCost = 0;
					int infCost = 0;

					if (resourceCount >= 100)
					{
						if (resourceCount >= 1000)
						{
							if (resourceCount >= 10000)
							{
								if (resourceCount >= 100000)
								{
									if (resourceCount >= 1000000)
									{
										supCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_10000000");
										infCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_1000000");
										result = CalculateResourceCost(10000000, 1000000, supCost, infCost, resourceCount);
									}
									else
									{
										supCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_1000000");
										infCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_100000");
										result = CalculateResourceCost(1000000, 100000, supCost, infCost, resourceCount);
									}
								}
								else
								{
									supCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_100000");
									infCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_10000");
									result = CalculateResourceCost(100000, 10000, supCost, infCost, resourceCount);
								}
							}
							else
							{
								supCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_10000");
								infCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_1000");
								result = CalculateResourceCost(10000, 1000, supCost, infCost, resourceCount);
							}
						}
						else
						{
							supCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_1000");
							infCost = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_100");
							result = CalculateResourceCost(1000, 100, supCost, infCost, resourceCount);
						}
					}
					else
					{
						result = GetGlobalNumberValue("RESOURCE_DIAMOND_COST_100");
					}
				}
			}

			return result;
		}

		public int GetDarkElixirDiamondCost(int resourceCount)
		{
			DataTable globals = Csv.Tables.Get(Csv.Files.Globals);
			int result = 0;
			if (resourceCount >= 1)
			{
				if (resourceCount >= 10)
				{
					if (resourceCount >= 100)
					{
						if (resourceCount >= 1000)
						{
							if (resourceCount >= 10000)
							{
								int supCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_100000").NumberValue;
								int infCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_10000").NumberValue;
								result = CalculateResourceCost(100000, 10000, supCost, infCost, resourceCount);
							}
							else
							{
								int supCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_10000").NumberValue;
								int infCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_1000").NumberValue;
								result = CalculateResourceCost(10000, 1000, supCost, infCost, resourceCount);
							}
						}
						else
						{
							int supCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_1000").NumberValue;
							int infCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_100").NumberValue;
							result = CalculateResourceCost(1000, 100, supCost, infCost, resourceCount);
						}
					}
					else
					{
						int supCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_100")
							.NumberValue;
						int infCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_10")
							.NumberValue;
						result = CalculateResourceCost(100, 10, supCost, infCost, resourceCount);
					}
				}
				else
				{
					int supCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_10")
						.NumberValue;
					int infCost = globals.GetData<Globals>("DARK_ELIXIR_DIAMOND_COST_1")
						.NumberValue;
					result = CalculateResourceCost(10, 1, supCost, infCost, resourceCount);
				}
			}

			return result;
		}


		public static int GetSpeedUpCost(int seconds)
		{
			DataTable globals = Csv.Tables.Get(Csv.Files.Globals);
			int cost = 0;
			if (seconds >= 1)
			{
				if (seconds >= 60)
				{
					if (seconds >= 3600)
					{
						if (seconds >= 86400)
						{
							int supCost = globals.GetData<Globals>("SPEED_UP_DIAMOND_COST_1_WEEK").NumberValue;
							int infCost = globals.GetData<Globals>("SPEED_UP_DIAMOND_COST_24_HOURS").NumberValue;
							cost = CalculateSpeedUpCost(604800, 86400, supCost, infCost, seconds);
						}
						else
						{
							int supCost = globals.GetData<Globals>("SPEED_UP_DIAMOND_COST_24_HOURS").NumberValue;
							int infCost = globals.GetData<Globals>("SPEED_UP_DIAMOND_COST_1_HOUR").NumberValue;
							cost = CalculateSpeedUpCost(86400, 3600, supCost, infCost, seconds);
						}
					}
					else
					{
						int supCost = globals.GetData<Globals>("SPEED_UP_DIAMOND_COST_1_HOUR")
							.NumberValue;
						int infCost = globals.GetData<Globals>("SPEED_UP_DIAMOND_COST_1_MIN")
							.NumberValue;
						cost = CalculateSpeedUpCost(3600, 60, supCost, infCost, seconds);
					}
				}
				else
				{
					cost = globals.GetData<Globals>("SPEED_UP_DIAMOND_COST_1_MIN")
						.NumberValue;
				}
			}

			return cost;
		}
	}
}