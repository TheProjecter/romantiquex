using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public class AlphaBlender : Blender
	{
		public AlphaBlender(IServiceProvider services)
			: base(services, "Shaders/Blenders/AlphaBlender.fx")
		{
		}
	}
}
