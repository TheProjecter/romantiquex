using System;
using RomantiqueX.Engine.World;
using RomantiqueX.Utils.Math;
using RomantiqueX.Utils.Viewers;
using RomantiqueX.Utils;
using SlimDX;
using System.Drawing;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D10.Buffer;
using RomantiqueX.Engine.Graphics;
using System.IO;
using SlimDX.Direct3D10;
using RomantiqueX.Engine.ContentPipeline;
using System.Collections;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;

namespace TestArea
{
	public class SimpleObject : WorldObject
	{
		#region Fields

		private Batch batch;
		private readonly BatchReadOnlyCollection batchesRO;
		private readonly BatchCollection batches = new BatchCollection();

		private readonly Effect effect;
		private readonly ShaderResourceView diffuseMapView;

		#endregion

		#region Initialization

		public SimpleObject(string name, IServiceProvider services, PositionTexCoordNormalVertex[] vertices, short[] indices,
			string effectAsset, string diffuseMapAsset)
			: base(name)
		{
			if (services == null)
				throw new ArgumentNullException("services");
			if (vertices == null)
				throw new ArgumentNullException("vertices");
			if (indices == null)
				throw new ArgumentNullException("indices");

			batchesRO = new BatchReadOnlyCollection(batches);

			// Services
			var resourceManager = (ResourceManager)services.GetService(typeof(ResourceManager));
			var renderer = (Renderer)services.GetService(typeof(Renderer));

			// Load effect & textures
			effect = resourceManager.Load<Effect>(effectAsset, false);
			diffuseMapView = new ShaderResourceView(renderer.Device, resourceManager.Load<Texture2D>(diffuseMapAsset));
			effect.SetVariableByName("DiffuseMap", diffuseMapView, true);

			// Create batch
			CreateBatch(vertices, indices, renderer);

			// Calculate boundary
			CalculateBoundary(vertices);
		}

		private void CalculateBoundary(PositionTexCoordNormalVertex[] vertices)
		{
			var positions = new Vector3[vertices.Length];
			for (int i = 0; i < vertices.Length; ++i)
				positions[i] = new Vector3(vertices[i].Position.X, vertices[i].Position.Y, vertices[i].Position.Z);
			LocalBoundary = BoundingBox.FromPoints(positions);
		}

		private void CreateBatch(PositionTexCoordNormalVertex[] vertices, short[] indices, Renderer renderer)
		{
			Debug.Assert(vertices != null && indices != null);

			int vertexBufferSize = PositionTexCoordNormalVertex.SizeInBytes * vertices.Length;
			var verticesData = new DataStream(vertexBufferSize, true, true);
			verticesData.WriteRange(vertices);
			verticesData.Seek(0, SeekOrigin.Begin);
			var vertexBuffer = new Buffer(renderer.Device, verticesData, vertexBufferSize, ResourceUsage.Default,
			                              BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None);
			verticesData.Close();

			int indexBufferSize = sizeof(short) * indices.Length;
			var indicesData = new DataStream(indexBufferSize, true, true);
			indicesData.WriteRange(indices);
			indicesData.Seek(0, SeekOrigin.Begin);
			var indexBuffer = new Buffer(renderer.Device, indicesData, indexBufferSize, ResourceUsage.Default,
			                             BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None);
			indicesData.Close();

			// Create batch
			var vertexBufferBinding = new VertexBufferBinding(vertexBuffer, PositionTexCoordNormalVertex.SizeInBytes, 0);
			var inputLayout = new InputLayout(renderer.Device, PositionTexCoordNormalVertex.InputElements,
			                                  effect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature);
			batch = new DrawIndexedBatch(renderer.Device, effect, new[] { vertexBufferBinding }, inputLayout, indexBuffer, Format.R16_UInt,
			                             indices.Length, 0, 0);
			batches.Add(batch);
		}

		public static SimpleObject CreateCube(string effectAsset, string diffuseMapAsset, string name, IServiceProvider services)
		{
			// Cube vertices
			var vertices = new[]{
			                    	// back
			                    	new PositionTexCoordNormalVertex(new Vector4(-1, -1, -1, 1f), new Vector2(0, 0), -Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4(-1,  1, -1, 1f), new Vector2(0, 1), -Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1,  1, -1, 1f), new Vector2(1, 1), -Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1, -1, -1, 1f), new Vector2(1, 0), -Vector3.UnitZ),
			                    	// front
			                    	new PositionTexCoordNormalVertex(new Vector4(-1, -1,  1, 1f), new Vector2(0, 0), Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4(-1,  1,  1, 1f), new Vector2(0, 1), Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1,  1,  1, 1f), new Vector2(1, 1), Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1, -1,  1, 1f), new Vector2(1, 0), Vector3.UnitZ),
			                    	// left
			                    	new PositionTexCoordNormalVertex(new Vector4(-1,  1,  1, 1f), new Vector2(0, 0), -Vector3.UnitX),
			                    	new PositionTexCoordNormalVertex(new Vector4(-1,  1, -1, 1f), new Vector2(0, 1), -Vector3.UnitX),
			                    	new PositionTexCoordNormalVertex(new Vector4(-1, -1, -1, 1f), new Vector2(1, 1), -Vector3.UnitX),
			                    	new PositionTexCoordNormalVertex(new Vector4(-1, -1,  1, 1f), new Vector2(1, 0), -Vector3.UnitX),
			                    	// right
			                    	new PositionTexCoordNormalVertex(new Vector4( 1,  1,  1, 1f), new Vector2(0, 0), Vector3.UnitX),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1,  1, -1, 1f), new Vector2(0, 1), Vector3.UnitX),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1, -1, -1, 1f), new Vector2(1, 1), Vector3.UnitX),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1, -1,  1, 1f), new Vector2(1, 0), Vector3.UnitX),
			                    	// bottom
			                    	new PositionTexCoordNormalVertex(new Vector4(-1, -1, -1, 1f), new Vector2(0, 0), -Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1, -1, -1, 1f), new Vector2(0, 1), -Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1, -1,  1, 1f), new Vector2(1, 1), -Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4(-1, -1,  1, 1f), new Vector2(1, 0), -Vector3.UnitZ),
			                    	// top
			                    	new PositionTexCoordNormalVertex(new Vector4(-1,  1, -1, 1f), new Vector2(0, 0), Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1,  1, -1, 1f), new Vector2(0, 1), Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4( 1,  1,  1, 1f), new Vector2(1, 1), Vector3.UnitZ),
			                    	new PositionTexCoordNormalVertex(new Vector4(-1,  1,  1, 1f), new Vector2(1, 0), Vector3.UnitZ),
			                    };
			// Cube indices
			short[] indices = {
			                  	0, 1, 2, 0, 2, 3,
			                  	4, 5, 6, 4, 6, 7,
			                  	8, 9, 10, 8, 10, 11,
			                  	12, 13, 14, 12, 14, 15,
			                  	16, 17, 18, 16, 18, 19,
			                  	20, 21, 22, 20, 22, 23
			                  };

			return new SimpleObject(name, services, vertices, indices, effectAsset, diffuseMapAsset);
		}

		public static SimpleObject CreatePlane(string effectAsset, string diffuseMapAsset, string name, IServiceProvider services, float width, float height)
		{
			float halfWidth = width * 0.5f;
			float halfHeight = height * 0.5f;
			
			// Plane vertices
			var vertices = new[]
			               	{
			               		new PositionTexCoordNormalVertex(new Vector4(-halfWidth, 0, -halfHeight, 1f), new Vector2(0, 0),
			               		                                 Vector3.UnitY),
			               		new PositionTexCoordNormalVertex(new Vector4(halfWidth, 0, -halfHeight, 1f), new Vector2(0, 1),
			               		                                 Vector3.UnitY),
			               		new PositionTexCoordNormalVertex(new Vector4(halfWidth, 0, halfHeight, 1f), new Vector2(1, 1),
			               		                                 Vector3.UnitY),
			               		new PositionTexCoordNormalVertex(new Vector4(-halfWidth, 0, halfHeight, 1f), new Vector2(1, 0),
			               		                                 Vector3.UnitY),
			               	};
			// Plane indices
			short[] indices = { 0, 1, 2, 0, 2, 3 };

			return new SimpleObject(name, services, vertices, indices, effectAsset, diffuseMapAsset);
		}

		#endregion

		public override BatchReadOnlyCollection GetBatches(Viewer viewer)
		{
			batch.Boundary = WorldBoundary;
			batch.WorldMatrix = WorldMatrix;
			return batchesRO;
		}
	}
}