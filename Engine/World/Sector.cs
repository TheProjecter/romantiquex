#region Using declarations

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SlimDX;

#endregion

namespace RomantiqueX.Engine.World
{

	#region Using declarations

	#endregion

	[Serializable]
	public abstract class Sector
	{
		#region Fields

		private readonly BoundingBox boundary;

		public BoundingBox Boundary
		{
			get { return boundary; }
		}

		[NonSerialized] private WorldManager manager;

		public WorldManager Manager
		{
			get { return manager; }
			internal set { manager = value; }
		}

		private readonly WorldObjectCollection registeredObjects = new WorldObjectCollection();
		private readonly WorldObjectReadOnlyCollection registeredObjectsRO;

		public WorldObjectReadOnlyCollection RegisteredObjects
		{
			get { return registeredObjectsRO; }
		}

		#endregion

		#region Creation

		protected Sector(BoundingBox boundary)
		{
			this.boundary = boundary;
			this.registeredObjectsRO = new WorldObjectReadOnlyCollection(registeredObjects);
		}

		#endregion

		#region Methods

		#region Object register/unregister

		public void RegisterObject(WorldObject worldObject)
		{
			if (worldObject == null)
				throw new ArgumentNullException("worldObject");
			if (worldObject.Sector != null)
				throw new ArgumentException("Given object is already registered somewhere.", "worldObject");
			if (!BoundingBox.Intersects(boundary, worldObject.WorldBoundary))
				throw new ArgumentException("Given object is not in volume of the sector.", "worldObject");

			if (manager != null)
				manager.RegisterObject(worldObject);

			registeredObjects.Add(worldObject);
			ReallyRegisterObject(worldObject);
			worldObject.Sector = this;
			worldObject.TransformationChanged += worldObject_TransformationChanged;
		}

		protected abstract void ReallyRegisterObject(WorldObject worldObject);

		public void UnregisterObject(WorldObject worldObject)
		{
			if (worldObject.Sector != this)
				throw new ArgumentException("Given object is not registered in this sector.", "worldObject");

			if (manager != null)
				manager.UnregisterObject(worldObject);

			registeredObjects.Remove(worldObject);
			ReallyUnregisterObject(worldObject);
			worldObject.Sector = null;
			worldObject.TransformationChanged -= worldObject_TransformationChanged;
		}

		protected abstract void ReallyUnregisterObject(WorldObject worldObject);

		#endregion

		#region Update

		private void worldObject_TransformationChanged(object sender, EventArgs e)
		{
			var worldObject = (WorldObject) sender;

			// In the case when object orientation update cause current sector change
			if (!registeredObjects.Contains(worldObject))
				return;

			ReallyUpdateObject(worldObject);
		}

		protected abstract void ReallyUpdateObject(WorldObject worldObject);

		#endregion

		#region SpatialRequests

		public abstract void GetObjectsInVolume(VolumeAdapter volume, ICollection<WorldObject> output);

		#endregion

		#region Serialization

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			foreach (WorldObject worldObject in registeredObjects)
			{
				worldObject.Sector = this;
				worldObject.TransformationChanged += worldObject_TransformationChanged;
			}
		}

		#endregion

		#endregion
	}
}