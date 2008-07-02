#region Using declarations

using System;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DXGI;

#endregion

namespace RomantiqueX.Engine.Graphics
{
	[Serializable]
	public class RendererConfiguration
	{
		public bool Windowed { get; private set; }

		public int BackBufferWidth { get; private set; }

		public int BackBufferHeight { get; private set; }

		public Format BackBufferFormat { get; private set; }

		public Rational RefreshRate { get; private set; }

		public RendererConfiguration(int backBufferWidth, int backBufferHeight, Format backBufferFormat,
			Rational refreshRate, bool windowed)
		{
			BackBufferWidth = backBufferWidth;
			BackBufferHeight = backBufferHeight;
			BackBufferFormat = backBufferFormat;
			RefreshRate = refreshRate;
			Windowed = windowed;
		}
	}
}