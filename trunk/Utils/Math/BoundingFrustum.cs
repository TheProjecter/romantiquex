using SlimDX;

namespace RomantiqueX.Utils.Math
{
	public class BoundingFrustum
	{
		private Matrix frustumMatrix = Matrix.Identity;

		public BoundingFrustum(Matrix frustumMatrix)
		{
			this.frustumMatrix = frustumMatrix;
		}

		public Matrix FrustumMatrix
		{
			get { return frustumMatrix; }
			set { frustumMatrix = value; }
		}

		public static bool Intersects(BoundingFrustum frustum1, BoundingFrustum frustum2)
		{
			return true;
		}

		public static bool Intersects(BoundingFrustum frustum, BoundingBox box)
		{
			return true;
		}

		public static bool Intersects(BoundingFrustum frustum, BoundingSphere sphere)
		{
			return true;
		}
	}
}