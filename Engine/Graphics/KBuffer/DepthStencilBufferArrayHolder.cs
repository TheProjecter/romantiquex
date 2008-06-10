using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Debug=System.Diagnostics.Debug;
using Device=SlimDX.Direct3D10.Device;

namespace RomantiqueX.Engine.Graphics.KBuffer
{
	internal class DepthStencilBufferArrayHolder
	{
		#region Fields

		private readonly Device device;
		private readonly RendererConfiguration rendererConfiguration;
		private readonly DeferredShadingConfiguration deferredShadingConfiguration;

		private Texture2D texture;
		private readonly Collection<DepthStencilView> depthStencilViews = new Collection<DepthStencilView>();
		private readonly ReadOnlyCollection<DepthStencilView> depthStencilViewsReadOnly;
		private readonly Collection<ShaderResourceView> shaderResourceViews = new Collection<ShaderResourceView>();
		private readonly ReadOnlyCollection<ShaderResourceView> shaderResourceViewsReadOnly;
		private ShaderResourceView shaderResourceViewAsArray;

		#endregion

		#region Properties

		public ReadOnlyCollection<DepthStencilView> DepthStencilViews
		{
			get { return depthStencilViewsReadOnly; }
		}

		public ReadOnlyCollection<ShaderResourceView> ShaderResourceViews
		{
			get { return shaderResourceViewsReadOnly; }
		}

		public ShaderResourceView ShaderResourceViewAsArray
		{
			get { return shaderResourceViewAsArray; }
		}

		#endregion

		#region Initialization

		public DepthStencilBufferArrayHolder(Device device,
			RendererConfiguration rendererConfiguration, DeferredShadingConfiguration deferredShadingConfiguration)
		{
			Debug.Assert(device != null && rendererConfiguration != null && deferredShadingConfiguration != null);

			this.device = device;
			this.rendererConfiguration = rendererConfiguration;
			this.deferredShadingConfiguration = deferredShadingConfiguration;

			depthStencilViewsReadOnly = new ReadOnlyCollection<DepthStencilView>(depthStencilViews);
			shaderResourceViewsReadOnly = new ReadOnlyCollection<ShaderResourceView>(shaderResourceViews);

			CreateTexture();
			CreateDepthStencilViews();
			CreateShaderResourceViews();
		}

		private void CreateShaderResourceViews()
		{
			var shaderResourceViewDesc = new ShaderResourceViewDescription();
			shaderResourceViewDesc.Format = Format.R32_Float;
			shaderResourceViewDesc.MipLevels = 1;
			shaderResourceViewDesc.MostDetailedMip = 0;
			shaderResourceViewDesc.Dimension = ShaderResourceViewDimension.Texture2DArray;

			shaderResourceViewDesc.ArraySize = deferredShadingConfiguration.LayerCount;
			shaderResourceViewDesc.FirstArraySlice = 0;
			shaderResourceViewAsArray = new ShaderResourceView(device, texture, shaderResourceViewDesc);

			shaderResourceViewDesc.ArraySize = 1;
			for (int i = 0; i < deferredShadingConfiguration.LayerCount; ++i)
			{
				shaderResourceViewDesc.FirstArraySlice = i;
				var shaderResourceView = new ShaderResourceView(device, texture, shaderResourceViewDesc);
				shaderResourceViews.Add(shaderResourceView);
			}
		}

		private void CreateDepthStencilViews()
		{
			var depthStencilViewDesc = new DepthStencilViewDescription();
			depthStencilViewDesc.Format = Format.D32_Float;
			depthStencilViewDesc.Dimension = DepthStencilViewDimension.Texture2DArray;
			depthStencilViewDesc.ArraySize = 1;
			depthStencilViewDesc.MipSlice = 0;
			for (int i = 0; i < deferredShadingConfiguration.LayerCount; ++i)
			{
				depthStencilViewDesc.FirstArraySlice = i;
				var depthStencilView = new DepthStencilView(device, texture, depthStencilViewDesc);
				depthStencilViews.Add(depthStencilView);
			}
		}

		private void CreateTexture()
		{
			var textureDesc = new Texture2DDescription();
			textureDesc.ArraySize = deferredShadingConfiguration.LayerCount;
			textureDesc.BindFlags = BindFlags.DepthStencil | BindFlags.ShaderResource;
			textureDesc.CpuAccessFlags = CpuAccessFlags.None;
			textureDesc.Format = Format.R32_Typeless;
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
