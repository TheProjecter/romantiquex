using System;
using System.Collections.Generic;
using RomantiqueX.Utils;
using SlimDX;
using System.Drawing;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public class AmbientLightingEffect : LightingEffectBase
	{
		public AmbientLightingEffect(IServiceProvider services)
			: this(services, new Color4(Color.White).ToColor3(), 0.2f)
		{
		}

		public AmbientLightingEffect(IServiceProvider services, Color3 ambientColor, float ambientIntensity)
			: base(ambientColor, ambientIntensity,
			services, "Shaders/VisualEffects/AmbientLighting.fx", new[] { RenderTargetLayerType.Color })
		{
		}
	}
}