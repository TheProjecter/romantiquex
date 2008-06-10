using System;
using RomantiqueX.Utils;
using SlimDX;
using System.Drawing;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public class AmbientLightingEffect : VisualEffect
	{
		#region Properties

		public Color4 AmbientColor { get; set; }

		public float AmbientIntensity { get; set; }

		#endregion

		public AmbientLightingEffect(IServiceProvider services)
			: base(services, "Shaders/VisualEffects/AmbientLighting.fx", new []{RenderTargetLayerType.Color})
		{
			AmbientColor = new Color4(Color.White);
			AmbientIntensity = 0.3f;
		}

		protected override void PrepareForApply(View view)
		{
			Effect.SetVariableByName("AmbientColor", AmbientColor.ToVector4(), true);
			Effect.SetVariableByName("AmbientIntensity", AmbientIntensity, true);
		}
	}
}