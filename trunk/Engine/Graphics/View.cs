using System;
using RomantiqueX.Utils.Viewers;
using SlimDX;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.Graphics
{
	public class View
	{
		public Viewer Viewer { get; private set; }

		public Viewport Viewport { get; private set; }

		public RenderTargetView Target { get; private set; }

		public View(Viewer viewer, Viewport viewport, RenderTargetView target)
		{
			if (viewer == null)
				throw new ArgumentNullException("viewer");
			if (target == null)
				throw new ArgumentNullException("target");

			Viewer = viewer;
			Viewport = viewport;
			Target = target;
		}
	}
}
