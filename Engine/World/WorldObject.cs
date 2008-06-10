#region Using declarations

using System;
using System.Globalization;
using RomantiqueX.Utils.Math;
using SlimDX;
using RomantiqueX.Engine.Graphics;
using RomantiqueX.Utils.Viewers;

#endregion

namespace RomantiqueX.Engine.World
{

	#region Using declarations

	#endregion

	[Serializable]
	public abstract class WorldObject
	{
		#region Fields

		private readonly string name;

		public string Name
		{
			get { return name; }
		}

		[NonSerialized] private Sector sector;

		public Sector Sector
		{
			get { return sector; }
			internal set { sector = value; }
		}

		private Vector3 position;

		public Vector3 Position
		{
			get { return position; }
			set
			{
				position = value;
				RecalculateWorldMatrix();
			}
		}

		private Quaternion rotation = Quaternion.Identity;

		public Quaternion Rotation
		{
			get { return rotation; }
			set
			{
				rotation = value;
				RecalculateWorldMatrix();
			}
		}

		private Matrix worldMatrix = Matrix.Identity;

		public Matrix WorldMatrix
		{
			get { return worldMatrix; }
		}

		private BoundingBox worldBoundary;

		public BoundingBox WorldBoundary
		{
			get { return worldBoundary; }
		}

		private BoundingBox localBoundary;

		public BoundingBox LocalBoundary
		{
			get { return localBoundary; }
			protected set
			{
				localBoundary = value;
				RecalculateWorldBoundary();
			}
		}

		#endregion

		#region Creation

		protected WorldObject(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("Name should not be null or empty.", "name");

			this.name = name;
		}

		#endregion

		#region Methods

		public abstract BatchReadOnlyCollection GetBatches(Viewer viewer);
		
		private void RecalculateWorldMatrix()
		{
			worldMatrix = Matrix.Transformation(Vector3.Zero, Quaternion.Identity, new Vector3(1f), Vector3.Zero, rotation,
			                                    position);
			RecalculateWorldBoundary();

			// Notify transformation observers
			if (TransformationChanged != null)
				TransformationChanged(this, EventArgs.Empty);
		}

		private void RecalculateWorldBoundary()
		{
			worldBoundary = MathHelper.TransformBoundingBox(localBoundary, worldMatrix);
		}

		#endregion

		#region Stuff

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{{0}}}", name);
		}

		#endregion

		#region Events

		[field : NonSerialized]
		public event EventHandler TransformationChanged;

		#endregion
	}
}