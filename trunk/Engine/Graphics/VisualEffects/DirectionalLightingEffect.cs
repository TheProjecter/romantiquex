using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using RomantiqueX.Utils;
using SlimDX;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public class DirectionalLightingEffect : LightingEffectBase
	{
		#region Properties

		private Vector3 direction;
		public Vector3 Direction
		{
			get { return direction; }
			set
			{
				value.Normalize();
				direction = value;
			}
		}

		#endregion

		#region Initialization

		public DirectionalLightingEffect(IServiceProvider services)
			: this(services, new Color4(Color.White).ToColor3(), 1)
		{
		}

		public DirectionalLightingEffect(IServiceProvider services, Color3 lightingColor, float lightingIntensity)
			: base(lightingColor, lightingIntensity,
			services, "Shaders/VisualEffects/DirectionalLighting.fx", new[] { RenderTargetLayerType.Color, RenderTargetLayerType.Normal })
		{
			Direction = new Vector3(1f, 1f, -1f);
		}

		#endregion

		#region Methods

		#region Protected

		protected override void PrepareForApply(View view)
		{
			base.PrepareForApply(view);

			Effect.SetVariableByName("LightDirection", new Vector4(direction, 0), true);
		} 

		#endregion

		#endregion
	}
}
