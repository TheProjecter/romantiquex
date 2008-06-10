using SlimDX.DXGI;
using SlimDX;

namespace RomantiqueX.Engine.Graphics
{
	public sealed class RenderTargetDescription
	{
		public Format Format { get; private set; }

		public RenderTargetLayerTypeDescription LayerType { get; private set; }

		public Color4 InitialColor { get; private set; }

		public RenderTargetDescription(Format format, RenderTargetLayerTypeDescription layerType, Color4 initialColor)
		{
			Format = format;
			LayerType = layerType;
			InitialColor = initialColor;
		}
	}
}