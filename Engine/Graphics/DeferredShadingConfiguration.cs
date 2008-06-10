using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using System.Drawing;

namespace RomantiqueX.Engine.Graphics
{
	public class DeferredShadingConfiguration
	{
		#region Fields & properties

		public RenderTargetDescriptionReadOnlyCollection RenderTargets { get; private set; }

		public int LayerCount { get; private set; }

		#endregion

		#region Static fields and properties

		private static readonly DeferredShadingConfiguration defaultConfig =
			new DeferredShadingConfiguration(
											new RenderTargetDescriptionCollection
			                                 	{
													new RenderTargetDescription(Format.R8G8B8A8_UNorm, RenderTargetLayerTypeDescription.Color, new Color4(Color.Black)),
													new RenderTargetDescription(Format.R10G10B10A2_UNorm, RenderTargetLayerTypeDescription.Normal, new Color4(Color.Black)),
			                                 	},
											4);

		public static DeferredShadingConfiguration Default
		{
			get { return defaultConfig; }
		}

		#endregion

		#region Constants

		private const int maxRenderTargetCount = 8;
		private const int maxLayerCount = 8;

		#endregion

		public DeferredShadingConfiguration(RenderTargetDescriptionCollection renderTargetDescriptions, int layerCount)
		{
			if (renderTargetDescriptions.Count == 0 || renderTargetDescriptions.Count > maxRenderTargetCount)
				throw new ArgumentException(string.Format("There can be from 1 to {0} render targets simultaneously.",
														  maxRenderTargetCount), "renderTargetDescriptions");
			if (layerCount < 1 || layerCount > maxLayerCount)
				throw new ArgumentOutOfRangeException("layerCount", string.Format("There can be from 1 to {0} layers.", layerCount));

			foreach (RenderTargetDescription description1 in renderTargetDescriptions)
				foreach (RenderTargetDescription description2 in renderTargetDescriptions)
					if (description1 != description2 && description1.LayerType == description2.LayerType)
						throw new ArgumentException("Deferred renderer should use only one render target for each usage.",
							"renderTargetDescriptions");

			LayerCount = layerCount;

			// Create copy of given render target configuration
			var descriptions = new List<RenderTargetDescription>();
			descriptions.AddRange(renderTargetDescriptions);
			// And use this copy to initialize internal RT description holder
			RenderTargets = new RenderTargetDescriptionReadOnlyCollection(descriptions);
		}
	}
}
