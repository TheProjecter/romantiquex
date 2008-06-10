using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RomantiqueX.Utils;
using SlimDX;
using SlimDX.Direct3D10;
using RomantiqueX.Utils.Viewers;

namespace RomantiqueX.Engine.Graphics
{
	public abstract class Batch
	{
		#region Properties

		public BoundingBox Boundary { get; set; }

		public Matrix WorldMatrix { get; set; }

		public Effect Effect { get; private set; }

		public InputLayout InputLayout { get; set; }

		public PrimitiveTopology PrimitiveTopology { get; set; }

		public Collection<VertexBufferBinding> VertexBuffers { get; private set; }

		public Device Device { get; private set; }

		#endregion

		#region Initialization

		protected Batch(Device device, Effect effect,
			IEnumerable<VertexBufferBinding> vertexBuffers, InputLayout inputLayout)
		{
			if (device == null)
				throw new ArgumentNullException("device");
			if (effect == null)
				throw new ArgumentNullException("effect");
			if (inputLayout == null)
				throw new ArgumentNullException("inputLayout");

			VertexBuffers = new Collection<VertexBufferBinding>();
			foreach (VertexBufferBinding buffer in vertexBuffers)
				VertexBuffers.Add(buffer);

			Device = device;
			Effect = effect;
			InputLayout = inputLayout;
			WorldMatrix = Matrix.Identity;
			Boundary = new BoundingBox();
			PrimitiveTopology = PrimitiveTopology.TriangleList;
		}

		#endregion

		#region Methods

		#region Protected

		protected virtual void Setup()
		{
			Device.InputAssembler.SetInputLayout(InputLayout);
			Device.InputAssembler.SetPrimitiveTopology(PrimitiveTopology);
			for (int i = 0; i < VertexBuffers.Count; ++i)
				Device.InputAssembler.SetVertexBuffers(i, VertexBuffers[i]);
		}

		protected abstract void Draw();

		#endregion

		#region Public

		#endregion

		public void SetupAndDraw()
		{
			Setup();
			Draw();
		}

		internal void SetupGlobalParameters(Viewer viewer, Timer timer)
		{
			if (viewer == null)
				throw new ArgumentNullException("viewer");
			if (timer == null)
				throw new ArgumentNullException("timer");

			EffectConstantBuffer perViewParameters = Effect.GetConstantBufferByName("Global");

			perViewParameters.SetVariableBySemantic(StandartSemantics.ViewMatrix,
				viewer.ViewMatrix, false);
			perViewParameters.SetVariableBySemantic(StandartSemantics.ProjectionMatrix,
				viewer.ProjectionMatrix, false);
			perViewParameters.SetVariableBySemantic(StandartSemantics.ViewProjectionMatrix,
				viewer.ViewMatrix * viewer.ProjectionMatrix, false);
			perViewParameters.SetVariableBySemantic(StandartSemantics.TotalTime, (float)timer.Time.Total, false);
			perViewParameters.SetVariableBySemantic(StandartSemantics.ElapsedTime, (float)timer.Time.Elapsed, false);
		}

		internal protected virtual void SetupPerBatchParameters(Viewer viewer)
		{
			if (viewer == null)
				throw new ArgumentNullException("viewer");

			EffectConstantBuffer perBatchParameters = Effect.GetConstantBufferByName("PerBatch");

			perBatchParameters.SetVariableBySemantic(StandartSemantics.WorldMatrix,
				WorldMatrix, false);
			perBatchParameters.SetVariableBySemantic(StandartSemantics.WorldViewMatrix,
				WorldMatrix * viewer.ViewMatrix, false);
			perBatchParameters.SetVariableBySemantic(StandartSemantics.WorldViewProjectionMatrix,
				WorldMatrix * viewer.ViewMatrix * viewer.ProjectionMatrix, false);
		}

		#endregion
	}
}
