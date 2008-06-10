#region Using declarations

using System;
using SlimDX;
using RomantiqueX.Engine.Graphics;
using RomantiqueX.Utils.Viewers;

#endregion

namespace RomantiqueX.Engine.World
{

	#region Using declarations

	#endregion

	[Serializable]
	public class DummyObject : WorldObject
	{
		public DummyObject(string name, BoundingBox localBoundary)
			: base(name)
		{
			this.LocalBoundary = localBoundary;
		}

		public override BatchReadOnlyCollection GetBatches(Viewer viewer)
		{
			return BatchReadOnlyCollection.Empty;
		}
	}
}