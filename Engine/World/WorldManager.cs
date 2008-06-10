#region Using declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using SlimDX;
using System.ComponentModel.Design;

#endregion

namespace RomantiqueX.Engine.World
{
	[Serializable]
	public sealed class WorldManager
	{
		#region Fields

		private readonly SectorCollection registeredSectors = new SectorCollection();
		private readonly SectorReadOnlyCollection registeredSectorsRO;

		public SectorReadOnlyCollection RegisteredSectors
		{
			get { return registeredSectorsRO; }
		}

		private readonly WorldObjectCollection registeredObjects = new WorldObjectCollection();
		private readonly WorldObjectReadOnlyCollection registeredObjectsRO;

		public WorldObjectReadOnlyCollection RegisteredObjects
		{
			get { return registeredObjectsRO; }
		}

		#region Portals

		private readonly PortalCollection registeredPortals = new PortalCollection();
		private readonly PortalReadOnlyCollection registeredPortalsRO;

		public PortalReadOnlyCollection RegisteredPortals
		{
			get { return registeredPortalsRO; }
		}

		private readonly Dictionary<Sector, List<Portal>> portalsBySourceSector =
			new Dictionary<Sector, List<Portal>>();

		private readonly Dictionary<Sector, List<Portal>> portalsByDestinationSector =
			new Dictionary<Sector, List<Portal>>();

		#endregion

		#endregion

		#region Creation

		public WorldManager(IServiceContainer services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			services.AddService(typeof (WorldManager), this);

			registeredSectorsRO = new SectorReadOnlyCollection(registeredSectors);
			registeredObjectsRO = new WorldObjectReadOnlyCollection(registeredObjects);
			registeredPortalsRO = new PortalReadOnlyCollection(registeredPortals);
		}

		#endregion

		#region Methods

		private void worldObject_TransformationChanged(object sender, EventArgs e)
		{
			var worldObject = (WorldObject) sender;
			Debug.Assert(registeredObjects.Contains(worldObject));

			if (BoundingBox.Intersects(worldObject.WorldBoundary, worldObject.Sector.Boundary))
				return;

			Sector oldSector = worldObject.Sector;
			Sector newSector = FindSectorForObject(worldObject);
			oldSector.UnregisterObject(worldObject);
			if (newSector != null)
				newSector.RegisterObject(worldObject);
		}

		private Sector FindSectorForObject(WorldObject worldObject)
		{
			for (int i = 0; i < registeredSectors.Count; ++i)
			{
				Sector sector = registeredSectors[i];
				if (BoundingBox.Intersects(worldObject.WorldBoundary, sector.Boundary))
					return sector;
			}
			return null;
		}

		internal void RegisterObject(WorldObject worldObject)
		{
			Debug.Assert(worldObject != null);

			if (registeredObjects.Contains(worldObject.Name))
				throw new ArgumentException("Object with name '" + worldObject.Name + "' is already registered.",
				                            "worldObject");

			registeredObjects.Add(worldObject);
			worldObject.TransformationChanged += worldObject_TransformationChanged;
		}

		internal void UnregisterObject(WorldObject worldObject)
		{
			Debug.Assert(worldObject != null && registeredObjects.Contains(worldObject));

			worldObject.TransformationChanged -= worldObject_TransformationChanged;
			registeredObjects.Remove(worldObject);
		}

		public void RegisterSector(Sector sector)
		{
			if (sector == null)
				throw new ArgumentNullException("sector");
			if (sector.Manager != null)
				throw new ArgumentException("Given sector is already registered in some context.", "sector");

			sector.Manager = this;
			registeredSectors.Add(sector);
			foreach (WorldObject worldObject in sector.RegisteredObjects)
				RegisterObject(worldObject);
		}

		public void UnregisterSector(Sector sector)
		{
			if (sector == null)
				throw new ArgumentNullException("sector");
			if (sector.Manager != this)
				throw new ArgumentException("Given sector is not registered in this context.", "sector");
			if ((portalsBySourceSector.ContainsKey(sector) && portalsBySourceSector[sector].Count > 0) ||
			    (portalsByDestinationSector.ContainsKey(sector) && portalsByDestinationSector[sector].Count > 0))
				throw new ArgumentException(
					"Given sector can't be unregistered because it is referenced by some registered portals.",
					"sector");

			sector.Manager = null;
			registeredSectors.Remove(sector);
			foreach (WorldObject worldObject in sector.RegisteredObjects)
				UnregisterObject(worldObject);
		}

		public void RegisterPortal(Portal portal)
		{
			if (portal == null)
				throw new ArgumentNullException("portal");
			if (portal.Manager != null)
				throw new ArgumentException("Given portal is already registered in some context.", "portal");
			if (portal.SourceSector.Manager != this)
				throw new ArgumentException("Source sector of the given portal is not registered in this context",
				                            "portal");
			if (portal.DestinationSector.Manager != this)
				throw new ArgumentException("Destination sector of the given portal is not registered in this context",
				                            "portal");

			registeredPortals.Add(portal);
			portal.Manager = this;

			// Register by source sector key
			if (!portalsBySourceSector.ContainsKey(portal.SourceSector))
				portalsBySourceSector.Add(portal.SourceSector, new List<Portal>());
			portalsBySourceSector[portal.SourceSector].Add(portal);
			// Register by destiantion sector key
			if (!portalsByDestinationSector.ContainsKey(portal.DestinationSector))
				portalsByDestinationSector.Add(portal.DestinationSector, new List<Portal>());
			portalsByDestinationSector[portal.DestinationSector].Add(portal);
		}

		public void UnregisterPortal(Portal portal)
		{
			if (portal == null)
				throw new ArgumentNullException("portal");
			if (portal.Manager != this)
				throw new ArgumentException("Given portal is not registered in this world manager.", "portal");

			registeredPortals.Remove(portal);
			portal.Manager = null;
			portalsBySourceSector[portal.SourceSector].Remove(portal);
			portalsByDestinationSector[portal.DestinationSector].Remove(portal);
		}

		public void GetObjectsInVolume(Sector startSector, VolumeAdapter volume, ICollection<WorldObject> output,
		                               int maxDepth)
		{
			if (startSector == null)
				throw new ArgumentNullException("startSector");
			if (output == null)
				throw new ArgumentNullException("output");
			if (maxDepth < 0)
				throw new ArgumentOutOfRangeException("maxDepth", "Parameter should have non-negative value.");

			GetObjectsInVolumeImpl(startSector, volume, output, maxDepth);
		}

		private void GetObjectsInVolumeImpl(Sector currentSector, VolumeAdapter volume,
		                                    ICollection<WorldObject> output, int curDepth)
		{
			if (!volume.Intersects(currentSector.Boundary))
				return;

			currentSector.GetObjectsInVolume(volume, output);

			if (portalsBySourceSector.ContainsKey(currentSector) && curDepth > 0)
				foreach (Portal portal in portalsBySourceSector[currentSector])
					if (volume.Intersects(portal.Boundary))
						GetObjectsInVolumeImpl(portal.DestinationSector, volume, output, curDepth - 1);
		}

		#region Serialization

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			foreach (WorldObject worldObject in registeredObjects)
				worldObject.TransformationChanged += worldObject_TransformationChanged;

			foreach (Sector sector in registeredSectors)
				sector.Manager = this;

			foreach (Portal portal in registeredPortals)
				portal.Manager = this;
		}

		#endregion

		#endregion
	}
}