using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RomantiqueX.Utils;

namespace RomantiqueX.Engine.Graphics
{
	public class ViewCollection : UniqueNotNullItemsCollection<View>
	{
	}

	public class ViewReadOnlyCollection : ReadOnlyCollection<View>
	{
		public ViewReadOnlyCollection(IList<View> list)
			: base(list)
		{
		}
	}
}
