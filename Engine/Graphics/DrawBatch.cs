using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.Graphics
{
	public class DrawBatch : Batch
	{
		public int VertexCount { get; set; }

		public int BaseVertex { get; set; }

		public DrawBatch(Device device, Effect effect, IEnumerable<VertexBufferBinding> vertexBuffers, InputLayout inputLayout,
			int vertexCount, int baseVertex)
			: base(device, effect, vertexBuffers, inputLayout)
		{
			VertexCount = vertexCount;
			BaseVertex = baseVertex;
		}

		protected override void Draw()
		{
			Device.Draw(VertexCount, BaseVertex);
		}
	}
}
