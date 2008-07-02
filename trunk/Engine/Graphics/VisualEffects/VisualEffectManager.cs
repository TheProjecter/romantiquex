using System;
using System.Collections.Generic;
using RomantiqueX.Engine.Graphics.VisualEffects;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Debug=System.Diagnostics.Debug;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	internal class VisualEffectManager
	{
		#region Fields
        private readonly Renderer renderer;

		private Blender blender;

		private BlendState visualEffectsBlendState;
		private BlendState blenderBlendState;
		private RasterizerState rasterizerState;
		private DepthStencilState depthStencilState;

		private readonly Dictionary<Format, RenderTargetArrayHolder> renderTargetsForGivenFormat =
			new Dictionary<Format, RenderTargetArrayHolder>();

		private readonly VisualEffectCollection visualEffects = new VisualEffectCollection();
		#endregion

		#region Properties
		public VisualEffectCollection VisualEffects
		{
			get { return visualEffects; }
		}

		public Blender Blender
		{
			get { return blender; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				blender = value;
			}
		}
		#endregion

		#region Initialization

		internal VisualEffectManager(IServiceProvider services, DeferredShadingConfiguration deferredShadingConfiguration)
		{
			Debug.Assert(services != null && deferredShadingConfiguration != null);

			renderer = (Renderer)services.GetService(typeof(Renderer));
			blender = new AlphaBlender(services);

			CreateStates();
		}

		private void CreateStates()
		{
			var blendStateDesc = new BlendStateDescription();
			blendStateDesc.BlendOperation = BlendOperation.Add;
			blendStateDesc.AlphaBlendOperation = BlendOperation.Add;
			blendStateDesc.SourceBlend = BlendOption.One;
			blendStateDesc.DestinationBlend = BlendOption.One;
			blendStateDesc.SourceAlphaBlend = BlendOption.One;
			blendStateDesc.DestinationAlphaBlend = BlendOption.Zero;
			blendStateDesc.IsAlphaToCoverageEnabled = false;
			blendStateDesc.SetBlendEnable(0, true);
			visualEffectsBlendState = BlendState.FromDescription(renderer.Device, blendStateDesc);
			blendStateDesc.DestinationBlend = BlendOption.Zero;
			blenderBlendState = BlendState.FromDescription(renderer.Device, blendStateDesc);

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

			// Retrieve layer holder for the given format
			RenderTargetArrayHolder targetHolder = SelectRenderTargetArray(view);
			
			// Prepare device
			renderer.Device.OutputMerger.BlendState = visualEffectsBlendState;
			renderer.Device.OutputMerger.DepthStencilState = depthStencilState;
			renderer.Device.OutputMerger.BlendFactor = new Color4(1, 1, 1, 1);
			renderer.Device.OutputMerger.BlendSampleMask = -1;
			renderer.Device.Rasterizer.State = rasterizerState;

			// Apply effects
			renderer.Device.OutputMerger.SetTargets((DepthStencilView)null, targetHolder.RenderTargetViewAsArray);
			renderer.Device.ClearRenderTargetView(targetHolder.RenderTargetViewAsArray, new Color4(0, 0, 0, 0));
			foreach (VisualEffect visualEffect in visualEffects)
				visualEffect.Apply(view, renderer.KBufferManager.SetupGeometryBufferTargets);

			// Blend layers
            renderer.Device.OutputMerger.SetTargets((DepthStencilView)null, view.Target);
			renderer.Device.OutputMerger.BlendState = blenderBlendState;
			blender.Blend(targetHolder.ShaderResourceViewAsArray);
		}

		private RenderTargetArrayHolder SelectRenderTargetArray(View view)
		{
			Format format = view.Target.Description.Format;

			if (renderTargetsForGivenFormat.ContainsKey(format))
				return renderTargetsForGivenFormat[format];

			var newArrayHolder = new RenderTargetArrayHolder(renderer.Device, renderer.Configuration.BackBufferWidth,
			                                                 renderer.Configuration.BackBufferHeight,
			                                                 view.Target.Description.Format,
			                                                 renderer.KBufferManager.Configuration.LayerCount);
			renderTargetsForGivenFormat.Add(format, newArrayHolder);
			return newArrayHolder;
		}
	}
}