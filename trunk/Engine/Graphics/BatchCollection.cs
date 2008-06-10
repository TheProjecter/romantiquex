using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RomantiqueX.Utils;

namespace RomantiqueX.Engine.Graphics
{
	public class BatchCollection : UniqueNotNullItemsCollection<Batch>
	{
	}

	public class BatchReadOnlyCollection : ReadOnlyCollection<Batch>
	{
		public BatchReadOnlyCollection(IList<Batch> list)
			: base(list)
		{
		}

		private static readonly BatchReadOnlyCollection empty = new BatchReadOnlyCollection(new BatchCollection());

		public static BatchReadOnlyCollection Empty
		{
			get { return empty; }
		}
	}
}
