using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace RomantiqueX.Utils
{
	[Serializable]
	public class UniqueNotNullItemsCollection<T> : Collection<T>
	{
		private void CheckItem(T item)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			if (Contains(item))
				throw new ArgumentException("Given item has been already added to collection.", "item");
		}
		
		protected override void InsertItem(int index, T item)
		{
			CheckItem(item);			
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, T item)
		{
			CheckItem(item);			
			base.SetItem(index, item);
		}
	}
}
