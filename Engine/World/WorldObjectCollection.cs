#region Using declarations

using System;
using System.Collections.ObjectModel;

#endregion

namespace RomantiqueX.Engine.World
{

	#region Using declarations

	#endregion

	[Serializable]
	public class WorldObjectCollection : KeyedCollection<string, WorldObject>
	{
		protected override string GetKeyForItem(WorldObject item)
		{
			return item.Name;
		}
	}

	[Serializable]
	public class WorldObjectReadOnlyCollection : ReadOnlyCollection<WorldObject>
	{
		private readonly WorldObjectCollection internalCollection;

		public WorldObjectReadOnlyCollection(WorldObjectCollection objects)
			: base(objects)
		{
			internalCollection = objects;
		}

		public WorldObject this[string key]
		{
			get { return internalCollection[key]; }
		}
	}
}