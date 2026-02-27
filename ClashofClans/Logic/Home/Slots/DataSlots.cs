using System.Collections.Generic;

using ClashofClans.Logic.Home.Slots.Items;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Logic.Home.Slots
{
	public class DataSlots : List<DataSlot>
	{
		/// <summary>
		///     Add a new dataslot or replace it
		/// </summary>
		/// <param name="id"></param>
		/// <param name="count"></param>
		public void Set(int id, int count)
		{
			int index = FindIndex(x => x.Id == id);
			if (index > -1)
			{
				DataSlot dataslot = this[index];
				dataslot.Count = count;
			}
			else
			{
				Add(new DataSlot
				{
					Id = id,
					Count = count
				});
			}
		}

		/// <summary>
		///     Add or create a new dataslot
		/// </summary>
		/// <param name="id"></param>
		/// <param name="count"></param>
		public void Add(int id, int count)
		{
			int index = FindIndex(x => x.Id == id);
			if (index > -1)
			{
				DataSlot dataslot = this[index];
				dataslot.Count += count;
			}
			else
			{
				Add(new DataSlot
				{
					Id = id,
					Count = count
				});
			}
		}

		/// <summary>
		///     Remove from a dataslot
		/// </summary>
		/// <param name="id"></param>
		/// <param name="count"></param>
		public void Remove(int id, int count)
		{
			int index = FindIndex(x => x.Id == id);
			if (index > -1)
			{
				DataSlot dataslot = this[index];
				dataslot.Count -= count;
			}
		}

		/// <summary>
		///     Get the count of a dataslot by it's id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int GetCount(int id)
		{
			int index = FindIndex(x => x.Id == id);
			return index > -1 ? this[index].Count : 0;
		}

		/// <summary>
		///     Get a dataslot by it's id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DataSlot GetById(int id)
		{
			int index = FindIndex(x => x.Id == id);
			return index > -1 ? this[index] : null;
		}

		/// <summary>
		///     Encodes all dataslots
		/// </summary>
		/// <param name="buffer"></param>
		public void Encode(ByteBuffer buffer)
		{
			buffer.WriteInt(Count);
			ForEach(x => x.Encode(buffer));
		}
	}
}