using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using RomantiqueX.Utils;
using SlimDX;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public class LightingEffectBase : VisualEffect
	{
		#region Properties

		public Color3 LightingColor { get; set; }
		public float LightingIntensity { get; set; }

		#endregion

		#region Initialization
		
		public LightingEffectBase(Color3 lightingColor, float lightingIntensity,
			IServiceProvider services, string effectAsset, IEnumerable<RenderTargetLayerType> requiredRenderTargets)
			: base(services, effectAsset, 0, requiredRenderTargets)
		{
			LightingColor = lightingColor;
			LightingIntensity = lightingIntensity;
		}

		#endregion

		#region Methods

		#region Protected

		protected override void PrepareForApply(View view)
		{
			Effect.SetVariableByName("LightColor", new Color4(LightingColor).ToVector4(), true);
			Effect.SetVariableByName("LightIntensity", LightingIntensity, true);
		} 

		#endregion

		#endregion
	}
}
