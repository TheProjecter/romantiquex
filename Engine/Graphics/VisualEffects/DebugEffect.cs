using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Utils;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public class DebugEffect : VisualEffect
	{
		#region Properties

		public int Layer { get; set; }

		public RenderTargetLayerType LayerType { get; set; }

		#endregion

		#region Initialization

		public DebugEffect(RenderTargetLayerType layerType, int layer, IServiceProvider services)
			: base(services, "Shaders/VisualEffects/Debug.fx", new []
			                                                   	{
			                                                   		RenderTargetLayerType.Color,
																	RenderTargetLayerType.Normal,
																	RenderTargetLayerType.Depth
																})
		{
			Layer = layer;
			LayerType = layerType;
		}

		#endregion

		#region Methods

		protected override void PrepareForApply(View view)
		{
			Effect.SetVariableByName("Layer", Layer, true);
			Effect.SetVariableByName("LayerType", (int)LayerType, true);
		}

		#endregion
	}
}
