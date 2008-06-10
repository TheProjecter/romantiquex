using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RomantiqueX.Engine.World;
using RomantiqueX.Utils.Math;
using RomantiqueX.Utils.Viewers;
using SlimDX;
using SlimDX.Direct3D10;
using BoundingFrustumVolumeAdapter=RomantiqueX.Engine.World.BoundingFrustumVolumeAdapter;
using Debug=System.Diagnostics.Debug;

namespace RomantiqueX.Engine.Graphics
{
	internal delegate void SetupPreviousLayerDepthHandler(Effect effect);
	
	internal class BatchManager
	{
		#region Fields

		private readonly IServiceProvider services;
		private readonly List<Batch> batchesToRender = new List<Batch>();
		private readonly List<Batch> batchesToRenderInSingleShot = new List<Batch>();
		private readonly WorldObjectCollection visibleObjects = new WorldObjectCollection();

		private Viewer viewerForBatches;

		#endregion

		#region Constants

		public const string FillGBufferTechniqueName = "FillGBuffer";

		#endregion

		#region Initialization

		public BatchManager(IServiceProvider services)
		{
			Debug.Assert(services != null);
			
			this.services = services;
		}

		#endregion

		#region Methods

		#region Public

		public void PrepareBatchesForViewer(Viewer viewer)
		{
			Debug.Assert(viewer != null);
			viewerForBatches = viewer;
			
			Sector startSector = DetermineStartSector(viewer);

			batchesToRender.Clear();
			visibleObjects.Clear();
			
			if (startSector == null)
				return;

			PrepareBatches(startSector);
		}

		public void RenderPreparedBatches(SetupPreviousLayerDepthHandler setupPreviousLayerDepthHandler)
		{
			Debug.Assert(setupPreviousLayerDepthHandler != null);
			
			Effect lastEffect = null;
			foreach (Batch batch in batchesToRender)
			{
				if (batch.Effect != lastEffect && lastEffect != null)
				{
					RenderBatchesInSingleShot(batchesToRenderInSingleShot, lastEffect, setupPreviousLayerDepthHandler);
					batchesToRenderInSingleShot.Clear();
				}

				batchesToRenderInSingleShot.Add(batch);
				lastEffect = batch.Effect;
			}

			if (lastEffect != null)
			{
				RenderBatchesInSingleShot(batchesToRenderInSingleShot, lastEffect, setupPreviousLayerDepthHandler);
				batchesToRenderInSingleShot.Clear();
			}
		}

		#endregion

		#region Private

		private Sector DetermineStartSector(Viewer viewer)
		{
			var worldManager = (WorldManager) services.GetService(typeof (WorldManager));
			Sector startSector = null;
			foreach (Sector sector in worldManager.RegisteredSectors)
				if (BoundingFrustum.Intersects(viewer.Frustum, sector.Boundary) &&
					(startSector == null || BoundingBox.Contains(startSector.Boundary, sector.Boundary) == ContainmentType.Contains))
					startSector = sector;

			return startSector;
		}

		private void PrepareBatches(Sector startSector)
		{
			// Retrieve visible objects
			startSector.Manager.GetObjectsInVolume(startSector, new BoundingFrustumVolumeAdapter(viewerForBatches.Frustum), visibleObjects,
											  startSector.Manager.RegisteredSectors.Count);

			// Get list of batches
			foreach (WorldObject visibleObject in visibleObjects)
			{
				var batches = visibleObject.GetBatches(viewerForBatches);
				batchesToRender.AddRange(batches);
			}

			// Sort by effect
			batchesToRender.Sort(delegate(Batch batch1, Batch batch2)
			{
				int code1 = batch1.Effect.GetHashCode();
				int code2 = batch2.Effect.GetHashCode();
				return code1 < code2 ? -1 : code1 == code2 ? 0 : 1;
			});

			// Apply global effect parameters
			foreach (Batch batch in batchesToRender)
				batch.SetupGlobalParameters(viewerForBatches, (Timer) services.GetService(typeof (Timer)));
		}

		private void RenderBatchesInSingleShot(List<Batch> batches, Effect effect,
			SetupPreviousLayerDepthHandler setupPreviousLayerDepthHandler)
		{
			Debug.Assert(batches != null && effect != null && setupPreviousLayerDepthHandler != null && batches.Count > 0);

			setupPreviousLayerDepthHandler.Invoke(effect);

			var technique = effect.GetTechniqueByName(FillGBufferTechniqueName);
			if (!technique.IsValid)
				return;

			for (int i = 0; i < technique.Description.PassCount; ++i)
			{
				foreach (Batch batch in batches)
				{
					Debug.Assert(batch.Effect == effect);

					batch.SetupPerBatchParameters(viewerForBatches);
					technique.GetPassByIndex(i).Apply();
					batch.SetupAndDraw();
				}
			}
		}

		#endregion

		#endregion
	}
}
