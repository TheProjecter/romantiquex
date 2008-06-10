#region Using declarations

using System;
using RomantiqueX.Utils.Math;
using SlimDX;

#endregion

namespace RomantiqueX.Utils
{
	public abstract class VolumeAdapter
	{
		public abstract bool Intersects(BoundingBox box);

		public abstract bool Intersects(BoundingSphere sphere);

		public abstract bool Intersects(BoundingFrustum frustum);
	}

	public sealed class BoundingBoxVolumeAdapter : VolumeAdapter
	{
		public BoundingBox Box { get; set; }

		public BoundingBoxVolumeAdapter(BoundingBox box)
		{
			this.Box = box;
		}

		public override bool Intersects(BoundingBox box)
		{
			return BoundingBox.Intersects(this.Box, box);
		}

		public override bool Intersects(BoundingSphere sphere)
		{
			return BoundingBox.Intersects(this.Box, sphere);
		}

		public override bool Intersects(BoundingFrustum frustum)
		{
			return BoundingFrustum.Intersects(frustum, this.Box);
		}
	}

	public sealed class BoundingSphereVolumeAdapter : VolumeAdapter
	{
		public BoundingSphere Sphere { get; set; }

		public BoundingSphereVolumeAdapter(BoundingSphere sphere)
		{
			this.Sphere = sphere;
		}

		public override bool Intersects(BoundingBox box)
		{
			return BoundingSphere.Intersects(this.Sphere, box);
		}

		public override bool Intersects(BoundingSphere sphere)
		{
			return BoundingSphere.Intersects(this.Sphere, sphere);
		}

		public override bool Intersects(BoundingFrustum frustum)
		{
			return BoundingFrustum.Intersects(frustum, this.Sphere);
		}
	}

	public sealed class BoundingFrustumVolumeAdapter : VolumeAdapter
	{
		public BoundingFrustum Frustum { get; set; }

		public BoundingFrustumVolumeAdapter(BoundingFrustum frustum)
		{
			this.Frustum = frustum;
		}

		public override bool Intersects(BoundingBox box)
		{
			return BoundingFrustum.Intersects(this.Frustum, box);
		}

		public override bool Intersects(BoundingSphere sphere)
		{
			return BoundingFrustum.Intersects(this.Frustum, sphere);
		}

		public override bool Intersects(BoundingFrustum frustum)
		{
			return BoundingFrustum.Intersects(this.Frustum, frustum);
		}
	}
}