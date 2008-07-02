using System;
using RomantiqueX.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public class DebugBlender : Blender
	{
		public int Layer { get; set; }
		
		public DebugBlender(int layer, IServiceProvider services)
			: base(services, "Shaders/Blenders/DebugBlender.fx")
		{
			Layer = layer;
		}

		protected override void PrepareForBlend()
		{
			Effect.SetVariableByName("Layer", Layer, true);
		}
	}
}
