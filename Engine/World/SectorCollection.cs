#region Using declarations

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RomantiqueX.Utils;

#endregion

namespace RomantiqueX.Engine.World
{

	#region Using declarations

	#endregion

	[Serializable]
	public class SectorCollection : UniqueNotNullItemsCollection<Sector>
	{
	}

	[Serializable]
	public class SectorReadOnlyCollection : ReadOnlyCollection<Sector>
	{
		public SectorReadOnlyCollection(IList<Sector> sectors)
			: base(sectors)
		{
		}
	}
}