using System;
using SlimDX;
using SlimDX.Direct3D10;
using Debug=System.Diagnostics.Debug;

namespace RomantiqueX.Engine.Graphics
{
	internal class VisualEffectManager
	{
		#region Fields

		private readonly IServiceProvider services;

		private BlendState blendState;
		private RasterizerState rasterizerState;
		private DepthStencilState depthStencilState;

		private readonly VisualEffectCollection visualEffects = new VisualEffectCollection();

		#endregion

		#region Properties

		public VisualEffectCollection VisualEffects
		{
			get { return visualEffects; }
		}

		#endregion

		#region Initialization

		internal VisualEffectManager(IServiceProvider services)
		{
			Debug.Assert(services != null);
			
			this.services = services;

			CreateStates();
		}

		private void CreateStates()
		{
			var renderer = (Renderer)services.GetService(typeof(Renderer));
			
			var blendStateDesc = new BlendStateDescription();
			blendStateDesc.BlendOperation = BlendOperation.Add;
			blendStateDesc.AlphaBlendOperation = BlendOperation.Add;
			blendStateDesc.SourceBlend = BlendOption.One;
			blendStateDesc.DestinationBlend = BlendOption.One;
			blendStateDesc.SourceAlphaBlend = BlendOption.One;
			blendStateDesc.DestinationAlphaBlend = BlendOption.One;
			blendStateDesc.IsAlphaToCoverageEnabled = false;
			blendStateDesc.SetBlendEnable(0, true);
			blendState = BlendState.FromDescription(renderer.Device, blendStateDesc);

			var rasterizerStateDesc = new RasterizerStateDescription();
			rasterizerStateDesc.CullMode = CullMode.None;
			rasterizerStateDesc.FillMode = FillMode.Solid;
			rasterizerStateDesc.IsAntialiasedLineEnabled = false;
			rasterizerStateDesc.IsMultisampleEnabled = false;
			// TODO: probably use scissor test
			rasterizerStateDesc.IsScissorEnabled = false;
			rasterizerState = RasterizerState.FromDescription(renderer.Device, rasterizerStateDesc);

			var depthStencilStateDesc = new DepthStencilStateDescription();
			depthStencilStateDesc.IsDepthEnabled = false;
			depthStencilStateDesc.IsStencilEnabled = false;
			depthStencilState = DepthStencilState.FromDescription(renderer.Device, depthStencilStateDesc);
		}

		#endregion

		public void ApplyVisualEffects(View view)
		{
			Debug.Assert(view != null);

			var renderer = (Renderer) services.GetService(typeof (Renderer));
			
			// Prepare device
			renderer.Device.OutputMerger.SetTargets((DepthStencilView) null, view.Target);
			renderer.Device.ClearRenderTargetView(view.Target, new Color4(0, 0, 0, 0));
			renderer.Device.OutputMerger.BlendState = blendState;
			renderer.Device.OutputMerger.DepthStencilState = depthStencilState;
			renderer.Device.OutputMerger.BlendFactor = new Color4(1, 1, 1, 1);
			renderer.Device.OutputMerger.BlendSampleMask = -1;
			renderer.Device.Rasterizer.State = rasterizerState;

			// Apply effects
			foreach (VisualEffect visualEffect in visualEffects)
				visualEffect.Apply(view, renderer.KBufferManager.SetupGeometryBufferTargets);
		}
	}
}
