#region Using declarations

using System;
using System.Diagnostics;
using RomantiqueX.Engine.Graphics.KBuffer;
using RomantiqueX.Utils.Viewers;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Debug=System.Diagnostics.Debug;
using Device = SlimDX.Direct3D10.Device;
using System.ComponentModel.Design;

#endregion

namespace RomantiqueX.Engine.Graphics
{
	public class Renderer
	{
		#region Fields

		private readonly RendererConfiguration rendererConfiguration;
		
		private readonly ViewCollection views = new ViewCollection();

		private readonly DepthPeeledKBufferManager kBufferManager;
		private readonly BatchManager batchManager;
		private readonly VisualEffectManager visualEffectManager;

		#region D3D10 & DXGI stuff

		private SwapChain swapChain;
		private Factory factory;
		private Device device;
		private RenderTargetView defaultRenderTargetView;

		#endregion

		#region Default states

		private BlendState defaultBlendState;
		private RasterizerState defaultRasterizerState;
		private DepthStencilState defaultDepthStencilState;
		private Color4 defaultBlendFactor = new Color4(1, 1, 1, 1);
		private int defaultBlendSampleMask = -1;
		private int defaultDepthStencilRef = 0;

		#endregion

		#endregion

		#region Properties

		#region Internal

		internal BatchManager BatchManager
		{
			get { return batchManager; }
		}

		internal DepthPeeledKBufferManager KBufferManager
		{
			get { return kBufferManager; }
		}

		internal VisualEffectManager VisualEffectManager
		{
			get { return visualEffectManager; }
		}

		#endregion

		#region Public

		public bool CanRender
		{
			get { return !rendererConfiguration.Control.IsDisposed; }
		}

		public string WindowCaption
		{
			get { return rendererConfiguration.Control.Text; }
			set { rendererConfiguration.Control.Text = value; }
		}

		public Device Device
		{
			get { return device; }
		}

		public ViewCollection Views
		{
			get { return views; }
		}

		public RendererConfiguration Configuration
		{
			get { return rendererConfiguration; }
		}

		public VisualEffectCollection VisualEffects
		{
			get { return visualEffectManager.VisualEffects; }
		}

		#endregion

		#endregion

		#region Initialization

		public Renderer(RendererConfiguration rendererConfiguration, DeferredShadingConfiguration deferredShadingConfiguration,
			IServiceContainer services)
		{
			if (rendererConfiguration == null)
				throw new ArgumentNullException("rendererConfiguration");
			if (deferredShadingConfiguration == null)
				throw new ArgumentNullException("deferredShadingConfiguration");
			if (services == null)
				throw new ArgumentNullException("services");

			services.AddService(typeof (Renderer), this);

            this.rendererConfiguration = rendererConfiguration;
			
			InitializeD3D10();
			PrepareWindow();
			PrepareDefaultRenderTarget();
			CreateDefaultRenderStates();

			batchManager = new BatchManager(services);
			kBufferManager = new DepthPeeledKBufferManager(services, deferredShadingConfiguration);
			visualEffectManager = new VisualEffectManager(services);
		}

		private void PrepareDefaultRenderTarget()
		{
			using (var backBuffer = swapChain.GetBuffer<Texture2D>(0))
			{
				defaultRenderTargetView = new RenderTargetView(device, backBuffer);
			}
		}

		private void PrepareWindow()
		{
			// Set caption
			WindowCaption = "RomantiqueX deferred renderer";
			// Show control
			rendererConfiguration.Control.Show();
		}

		private void InitializeD3D10()
		{
			// Create DXGI factory
			factory = new Factory();

			Adapter selectedAdapter = null;
			DriverType driverType = DriverType.Hardware;
			DeviceCreationFlags deviceCreationFlags = DeviceCreationFlags.None;
#if DEBUG
			// Try to find the "NVIDIA PerfHUD" adapter
			for (int i = 0; i < factory.GetAdapterCount(); ++i)
			{
				Adapter adapter = factory.GetAdapter(i);
				bool isPerfHUD = adapter.Description.Description == "NVIDIA PerfHUD";
				if (i == 0 || isPerfHUD)
					selectedAdapter = adapter;
				if (isPerfHUD)
					driverType = DriverType.Reference;
			}

			// Enable debug layer
			deviceCreationFlags = DeviceCreationFlags.Debug;
#endif

			// Create the device.
			device = new Device(selectedAdapter, driverType, deviceCreationFlags);

			// Create a swap chain.
			var modeDescription = new ModeDescription(rendererConfiguration.BackBufferWidth, rendererConfiguration.BackBufferHeight,
													  rendererConfiguration.RefreshRate, rendererConfiguration.BackBufferFormat);
			var swapChainDescription = new SwapChainDescription
										{
											BufferCount = 1,
											IsWindowed = rendererConfiguration.Windowed,
											Flags = SwapChainFlags.AllowModeSwitch,
											OutputHandle = rendererConfiguration.Control.Handle,
											SwapEffect = SwapEffect.Discard,
											Usage = Usage.RenderTargetOutput,
											ModeDescription = modeDescription,
											SampleDescription = new SampleDescription(1, 0),
										};

			swapChain = new SwapChain(factory, device, swapChainDescription);
		}

		private void CreateDefaultRenderStates()
		{
			var blendStateDesc = new BlendStateDescription();
			blendStateDesc.IsAlphaToCoverageEnabled = false;
			blendStateDesc.BlendOperation = BlendOperation.Add;
			blendStateDesc.AlphaBlendOperation = BlendOperation.Add;
			blendStateDesc.SourceBlend = BlendOption.One;
			blendStateDesc.DestinationBlend = BlendOption.Zero;
			blendStateDesc.SourceAlphaBlend = BlendOption.One;
			blendStateDesc.DestinationAlphaBlend = BlendOption.Zero;
			defaultBlendState = BlendState.FromDescription(device, blendStateDesc);
			
			var rasterizerStateDesc = new RasterizerStateDescription();
			rasterizerStateDesc.FillMode = FillMode.Solid;
			rasterizerStateDesc.CullMode = CullMode.Back;
			rasterizerStateDesc.IsFrontCounterclockwise = false;
			rasterizerStateDesc.DepthBias = 0;
			rasterizerStateDesc.DepthBiasClamp = 0;
			rasterizerStateDesc.SlopeScaledDepthBias = 0;
			rasterizerStateDesc.IsDepthClipEnabled = true;
			rasterizerStateDesc.IsScissorEnabled = false;
			rasterizerStateDesc.IsMultisampleEnabled = false;
			rasterizerStateDesc.IsAntialiasedLineEnabled = false;
			defaultRasterizerState = RasterizerState.FromDescription(device, rasterizerStateDesc);
			
			var depthStencilStateDesc = new DepthStencilStateDescription();
			depthStencilStateDesc.IsDepthEnabled = true;
			depthStencilStateDesc.DepthWriteMask = DepthWriteMask.All;
			depthStencilStateDesc.DepthComparison = Comparison.LessEqual;
			depthStencilStateDesc.IsStencilEnabled = false;
			depthStencilStateDesc.StencilReadMask = 0xff;
			depthStencilStateDesc.StencilWriteMask = 0xff;
			var depthStencilStateFaceDesk = new DepthStencilOperationDescription();
			depthStencilStateFaceDesk.Comparison = Comparison.Always;
			depthStencilStateFaceDesk.DepthFailOperation = StencilOperation.Keep;
			depthStencilStateFaceDesk.FailOperation = StencilOperation.Keep;
			depthStencilStateFaceDesk.PassOperation = StencilOperation.Keep;
			depthStencilStateDesc.FrontFace = depthStencilStateFaceDesk;
			depthStencilStateDesc.BackFace = depthStencilStateFaceDesk;
			defaultDepthStencilState = DepthStencilState.FromDescription(device, depthStencilStateDesc);
		}

		#endregion

		#region Methods

		#region Public

		public View CreateDefaultView(Viewer viewer, Viewport viewport)
		{
			return new View(viewer, viewport, defaultRenderTargetView);
		}

		public View CreateDefaultView(Viewer viewer)
		{
			var viewport = new Viewport
			{
				X = rendererConfiguration.Control.ClientRectangle.Left,
				Y = rendererConfiguration.Control.ClientRectangle.Top,
				Width = rendererConfiguration.Control.ClientRectangle.Width,
				Height = rendererConfiguration.Control.ClientRectangle.Height,
				MinZ = 0f,
				MaxZ = 1f
			};

			return CreateDefaultView(viewer, viewport);
		}

		public void RenderFrame()
		{
			foreach (View view in views)
				ProcessView(view);

			swapChain.Present(0, PresentFlags.None);
		}

		public void ProcessView(View view)
		{
			device.Rasterizer.SetViewports(view.Viewport);

			batchManager.PrepareBatchesForViewer(view.Viewer);
			kBufferManager.FillGeometryBuffer();
			visualEffectManager.ApplyVisualEffects(view);
		}

		public void DrawProceduralEffect(EffectTechnique technique)
		{
			if (technique == null)
				throw new ArgumentNullException("technique");
			if (!technique.IsValid)
				throw new ArgumentException("Given technique is not valid.", "technique");

			device.InputAssembler.SetInputLayout(null);
			device.InputAssembler.SetPrimitiveTopology(PrimitiveTopology.TriangleStrip);

			for (int i = 0; i < technique.Description.PassCount; i++)
			{
				technique.GetPassByIndex(i).Apply();
				device.Draw(3, 0);
			}
		}

		public void ResetRenderStatesToDefault()
		{
			// Blend state
			device.OutputMerger.BlendState = defaultBlendState;
			device.OutputMerger.BlendFactor = defaultBlendFactor;
			device.OutputMerger.BlendSampleMask = defaultBlendSampleMask;
			// Depth stencil state
			device.OutputMerger.DepthStencilState = defaultDepthStencilState;
			device.OutputMerger.DepthStencilReference = defaultDepthStencilRef;
			// Rasterizer state
			device.Rasterizer.State = defaultRasterizerState;
		}

		#endregion

		#endregion
	}
}