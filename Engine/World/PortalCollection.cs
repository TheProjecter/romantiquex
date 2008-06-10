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
	public class PortalCollection : UniqueNotNullItemsCollection<Portal>
	{
	}

	[Serializable]
	public class PortalReadOnlyCollection : ReadOnlyCollection<Portal>
	{
		public PortalReadOnlyCollection(IList<Portal> portals)
			: base(portals)
		{
		}
	}
}