using System;
using System.Collections.Generic;
using System.Text;
using RomantiqueX.Utils.Math;
using SlimDX;

namespace RomantiqueX.Utils.Viewers
{
	public abstract class Viewer
	{
		public abstract Matrix ViewMatrix { get; }

		public abstract Matrix ProjectionMatrix { get; }

		public abstract BoundingFrustum Frustum { get; }

		public abstract Vector3 Position { get; set; }

		public abstract float NearPlane { get; set; }

		public abstract float FarPlane { get; set; }
	}
}