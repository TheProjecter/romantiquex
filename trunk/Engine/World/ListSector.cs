#region Using declarations

using System;
using System.Collections.Generic;
using SlimDX;

#endregion

namespace RomantiqueX.Engine.World
{

	#region Using declarations

	#endregion

	[Serializable]
	public class ListSector : Sector
	{
		// List of registered objects for fast iteration
		private readonly List<WorldObject> registeredObjects = new List<WorldObject>();

		public ListSector(BoundingBox boundary)
			: base(boundary)
		{
		}

		protected override void ReallyRegisterObject(WorldObject worldObject)
		{
			registeredObjects.Add(worldObject);
		}

		protected override void ReallyUnregisterObject(WorldObject worldObject)
		{
			registeredObjects.Remove(worldObject);
		}

		protected override void ReallyUpdateObject(WorldObject worldObject)
		{
		}

		public override void GetObjectsInVolume(VolumeAdapter volume, ICollection<WorldObject> output)
		{
			if (output == null)
				throw new ArgumentNullException("output");

			foreach (WorldObject worldObject in registeredObjects)
				// BUG: in intersection method
				//if (volume.Intersects(worldObject.WorldBoundary))
					output.Add(worldObject);
		}
	}
}