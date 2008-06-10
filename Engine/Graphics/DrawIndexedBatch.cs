using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D10.Buffer;
using Device = SlimDX.Direct3D10.Device;

namespace RomantiqueX.Engine.Graphics
{
	public class DrawIndexedBatch : Batch
	{
		public Buffer IndexBuffer { get; private set; }

		public Format IndexBufferFormat { get; private set; }

		public int IndexCount { get; private set; }

		public int StartIndex { get; private set; }

		public int BaseVertex { get; private set; }

		public DrawIndexedBatch(Device device, Effect effect, IEnumerable<VertexBufferBinding> vertexBuffers, InputLayout inputLayout,
								Buffer indexBuffer, Format indexBufferFormat,
								int indexCount, int startIndex, int baseVertex)
			: base(device, effect, vertexBuffers, inputLayout)
		{
			IndexBuffer = indexBuffer;
			IndexBufferFormat = indexBufferFormat;
			IndexCount = indexCount;
			StartIndex = startIndex;
			BaseVertex = baseVertex;
		}

		protected override void Setup()
		{
			base.Setup();

			Device.InputAssembler.SetIndexBuffer(IndexBuffer, IndexBufferFormat, 0);
		}

		protected override void Draw()
		{
			Device.DrawIndexed(IndexCount, StartIndex, BaseVertex);
		}
	}
}
