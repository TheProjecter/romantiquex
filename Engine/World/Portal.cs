#region Using declarations

using System;
using System.Collections.Generic;
using RomantiqueX.Utils.Math;
using SlimDX;

#endregion

namespace RomantiqueX.Engine.World
{

	#region Using declarations

	#endregion

	[Serializable]
	public class Portal
	{
		#region Fields

		private readonly Sector sourceSector;

		public Sector SourceSector
		{
			get { return sourceSector; }
		}

		private readonly Sector destinationSector;

		public Sector DestinationSector
		{
			get { return destinationSector; }
		}

		[NonSerialized] private WorldManager manager;

		public WorldManager Manager
		{
			get { return manager; }
			internal set { manager = value; }
		}

		private readonly Matrix worldMatrix;

		public Matrix WorldMatrix
		{
			get { return worldMatrix; }
		}

		private BoundingBox boundary;

		public BoundingBox Boundary
		{
			get { return boundary; }
		}

		private readonly List<Triangle> triangles = new List<Triangle>();
		private readonly TriangleReadOnlyCollection trianglesRO;

		public TriangleReadOnlyCollection Triangles
		{
			get { return trianglesRO; }
		}

		#endregion

		#region Creation

		public Portal(Sector sourceSector, Sector destinationSector, Vector3 position, Quaternion rotation,
		              IEnumerable<Triangle> triangles)
		{
			if (sourceSector == null)
				throw new ArgumentNullException("sourceSector");
			if (destinationSector == null)
				throw new ArgumentNullException("destinationSector");
			if (triangles == null)
				throw new ArgumentNullException("triangles");
			if (sourceSector == destinationSector)
				throw new ArgumentException("sourceSector and destinationSector must differ.");

			this.sourceSector = sourceSector;
			this.destinationSector = destinationSector;
			this.worldMatrix = Matrix.Transformation(Vector3.Zero, Quaternion.Identity, new Vector3(1f), Vector3.Zero, rotation,
			                                         position);

			this.triangles.AddRange(triangles);
			this.trianglesRO = new TriangleReadOnlyCollection(this.triangles);
			CalculateBounary();
		}

		public static TriangleCollection CreateRectGeometry(float width, float height)
		{
			float halfWidth = width * 0.5f;
			float halfHeight = height * 0.5f;
			var tri1 = new Triangle(
				new Vector3(-halfWidth, -halfHeight, 0f),
				new Vector3(-halfWidth, halfHeight, 0f),
				new Vector3(halfWidth, halfHeight, 0f));
			var tri2 = new Triangle(
				new Vector3(-halfWidth, -halfHeight, 0f),
				new Vector3(halfWidth, halfHeight, 0f),
				new Vector3(halfWidth, -halfHeight, 0f));

			var result = new TriangleCollection();
			result.Add(tri1);
			result.Add(tri2);
			return result;
		}

		private void CalculateBounary()
		{
			var points = new Vector3[triangles.Count * 3];
			int i = 0;
			foreach (Triangle triangle in triangles)
			{
				points[i++] = triangle.Point0;
				points[i++] = triangle.Point1;
				points[i++] = triangle.Point2;
			}

			BoundingBox localBoundary = BoundingBox.FromPoints(points);
			boundary = MathHelper.TransformBoundingBox(localBoundary, worldMatrix);
		}

		#endregion
	}
}