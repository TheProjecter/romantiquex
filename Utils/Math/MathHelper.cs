using SlimDX;

namespace RomantiqueX.Utils.Math
{
	public static class MathHelper
	{
		public static BoundingBox TransformBoundingBox(BoundingBox boundary, Matrix transformation)
		{
			var corners = boundary.GetCorners();
			for (int i = 0; i < 8; ++i)
			{
				var transformed = Vector3.Transform(corners[i], transformation);
				corners[i] = new Vector3(transformed.X, transformed.Y, transformed.Z);
			}
			return BoundingBox.FromPoints(corners);
		}
	}
}