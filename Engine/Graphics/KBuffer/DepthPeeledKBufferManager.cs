using System;
using System.Collections.Generic;
using RomantiqueX.Utils;
using SlimDX.Direct3D10;
using Debug = System.Diagnostics.Debug;

namespace RomantiqueX.Engine.Graphics.KBuffer
{
	internal class DepthPeeledKBufferManager
	{
		#region Fields

		private readonly IServiceProvider services;
		private readonly DeferredShadingConfiguration configuration;

		private DepthStencilBufferArrayHolder depthArrayHolder;
        private readonly Dictionary<RenderTargetLayerType, RenderTargetArrayHolder> renderTargetArraysByLayerType =
			new Dictionary<RenderTargetLayerType, RenderTargetArrayHolder>();

		private RenderTargetView[][] renderTargetViewsByLayer;

		#endregion

		#region Initialization

		public DepthPeeledKBufferManager(IServiceProvider services, DeferredShadingConfiguration configuration)
		{
			Debug.Assert(services != null && configuration != null);

			this.services = services;
			this.configuration = configuration;

			CreateRenderTargets();
		}

		private void CreateRenderTargets()
		{
			var renderer = (Renderer)services.GetService(typeof(Renderer));

			depthArrayHolder = new DepthStencilBufferArrayHolder(renderer.Device, renderer.Configuration, configuration);
			foreach (RenderTargetDescription description in configuration.RenderTargets)
			{
				var target = new RenderTargetArrayHolder(renderer.Device, description, renderer.Configuration, configuration);
				RenderTargetLayerType layerType = ConvertLayerTypeDescToLayerType(description.LayerType);
				renderTargetArraysByLayerType.Add(layerType, target);
			}

			renderTargetViewsByLayer = new RenderTargetView[configuration.LayerCount][];
			for (int i = 0; i < configuration.LayerCount; ++i)
			{
				int j = 0;
				renderTargetViewsByLayer[i] = new RenderTargetView[renderTargetArraysByLayerType.Count];
				foreach (RenderTargetArrayHolder targetArrayHolder in renderTargetArraysByLayerType.Values)
					renderTargetViewsByLayer[i][j++] = targetArrayHolder.RenderTargetViews[i];
			}
		}

		private static RenderTargetLayerType ConvertLayerTypeDescToLayerType(RenderTargetLayerTypeDescription renderTargetLayerTypeDescription)
		{
			switch (renderTargetLayerTypeDescription)
			{
				case RenderTargetLayerTypeDescription.Color:
					return RenderTargetLayerType.Color;
				case RenderTargetLayerTypeDescription.Normal:
					return RenderTargetLayerType.Normal;
				default:
					throw new ArgumentOutOfRangeException("renderTargetLayerTypeDescription");
			}
		}

		#endregion

		#region Methods

		#region Public

		public void FillGeometryBuffer()
		{
			var renderer = (Renderer)services.GetService(typeof(Renderer));

			// Reset states to default
			renderer.ResetRenderStatesToDefault();

			for (int i = 0; i < configuration.LayerCount; ++i)
			{
				// Setup render targets and depth buffer
				renderer.Device.OutputMerger.SetTargets(depthArrayHolder.DepthStencilViews[i], renderTargetViewsByLayer[i]);
				
				// Clear current layer
				renderer.Device.ClearDepthStencilView(depthArrayHolder.DepthStencilViews[i], DepthStencilClearFlags.Depth, 1, 0);
				foreach (RenderTargetArrayHolder renderTargetArrayHolder in renderTargetArraysByLayerType.Values)
					renderer.Device.ClearRenderTargetView(renderTargetArrayHolder.RenderTargetViews[i],
														  renderTargetArrayHolder.Description.InitialColor);

				// Select previous depth buffer
				ShaderResourceView previousDepthBuffer = null;
				if (i != 0)
					previousDepthBuffer = depthArrayHolder.ShaderResourceViews[i - 1];

				// Draw batches
				renderer.BatchManager.RenderPreparedBatches(
					effect => effect.SetVariableBySemantic(StandartSemantics.PreviousLayerDepthTexture, previousDepthBuffer, true));
			}
		}

		public void SetupGeometryBufferTargets(Effect effect, IEnumerable<RenderTargetLayerType> requiredRenderTargets)
		{
			if (effect == null)
				throw new ArgumentNullException("effect");
			if (requiredRenderTargets == null)
				throw new ArgumentNullException("requiredRenderTargets");

			foreach (RenderTargetLayerType layerType in requiredRenderTargets)
			{
				if (!renderTargetArraysByLayerType.ContainsKey(layerType) && layerType != RenderTargetLayerType.Depth)
					throw new ArgumentException(string.Format("Required render target '{0}' is not available.", layerType),
					                            "requiredRenderTargets");
				if (layerType == RenderTargetLayerType.Depth)
					effect.SetVariableBySemantic(StandartSemantics.DepthTextureArrayPrefix + StandartSemantics.TextureArrayPostfix,
						depthArrayHolder.ShaderResourceViewAsArray, true);
				else
					effect.SetVariableBySemantic(layerType.ToString().ToUpper() + StandartSemantics.TextureArrayPostfix,
						renderTargetArraysByLayerType[layerType].ShaderResourceView, false);
			}
		}

		#endregion

		#endregion
	}
}
