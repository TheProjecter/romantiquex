using System.Collections.ObjectModel;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Debug=System.Diagnostics.Debug;
using Device=SlimDX.Direct3D10.Device;

namespace RomantiqueX.Engine.Graphics.KBuffer
{
	internal class RenderTargetArrayHolder
	{
		#region Fields

		private readonly Device device;
		private readonly RenderTargetDescription description;
		private readonly RendererConfiguration rendererConfiguration;
		private readonly DeferredShadingConfiguration deferredShadingConfiguration;

		private Texture2D texture;
        private readonly Collection<RenderTargetView> renderTargetViews = new Collection<RenderTargetView>();
		private readonly ReadOnlyCollection<RenderTargetView> renderTargetViewsReadOnly;
		private ShaderResourceView shaderResourceView;

		#endregion

		#region Properties

		public ReadOnlyCollection<RenderTargetView> RenderTargetViews
		{
			get { return renderTargetViewsReadOnly; }
		}

		public ShaderResourceView ShaderResourceView
		{
			get { return shaderResourceView; }
		}

		public RenderTargetDescription Description
		{
			get { return description; }
		}

		#endregion

		#region Initialization

		public RenderTargetArrayHolder(Device device, RenderTargetDescription description,
			RendererConfiguration rendererConfiguration, DeferredShadingConfiguration deferredShadingConfiguration)
		{
			Debug.Assert(device != null && description != null && rendererConfiguration != null &&
						 deferredShadingConfiguration != null);
			
			this.device = device;
			this.description = description;
			this.rendererConfiguration = rendererConfiguration;
			this.deferredShadingConfiguration = deferredShadingConfiguration;
			
			renderTargetViewsReadOnly = new ReadOnlyCollection<RenderTargetView>(renderTargetViews);
			
			CreateTexture();
			CreateRenderTargetViews();
			CreateShaderResourceView();
		}

		private void CreateShaderResourceView()
		{
			var shaderResourceViewDesc = new ShaderResourceViewDescription();
			shaderResourceViewDesc.Format = texture.Description.Format;
			shaderResourceViewDesc.Dimension = ShaderResourceViewDimension.Texture2DArray;
			shaderResourceViewDesc.MipLevels = 1;
			shaderResourceViewDesc.MostDetailedMip = 0;
			shaderResourceViewDesc.FirstArraySlice = 0;
			shaderResourceViewDesc.ArraySize = deferredShadingConfiguration.LayerCount;
			shaderResourceView = new ShaderResourceView(device, texture, shaderResourceViewDesc);
		}

		private void CreateRenderTargetViews()
		{
			var renderTargetViewDesc = new RenderTargetViewDescription();
			renderTargetViewDesc.Format = texture.Description.Format;
			renderTargetViewDesc.Dimension = RenderTargetViewDimension.Texture2DArray;
			renderTargetViewDesc.ArraySize = 1;
			renderTargetViewDesc.MipSlice = 0;
			for (int i = 0; i < deferredShadingConfiguration.LayerCount; ++i)
			{
				renderTargetViewDesc.FirstArraySlice = i;
				var renderTargetView = new RenderTargetView(device, texture, renderTargetViewDesc);
				renderTargetViews.Add(renderTargetView);
			}
		}

		private void CreateTexture()
		{
			var textureDesc = new Texture2DDescription();
			textureDesc.ArraySize = deferredShadingConfiguration.LayerCount;
			textureDesc.BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource;
			textureDesc.CpuAccessFlags = CpuAccessFlags.None;
			textureDesc.Format = description.Format;
			textureDesc.Width = rendererConfiguration.BackBufferWidth;
			textureDesc.Height = rendererConfiguration.BackBufferHeight;
			textureDesc.MipLevels = 1;
			textureDesc.OptionFlags = ResourceOptionFlags.None;
			textureDesc.SampleDescription = new SampleDescription(1, 0);
			textureDesc.Usage = ResourceUsage.Default;

			texture = new Texture2D(device, textureDesc);
		}

		#endregion
	}
}
