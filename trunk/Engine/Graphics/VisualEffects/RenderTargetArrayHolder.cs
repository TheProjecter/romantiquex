using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SlimDX.DXGI;
using SlimDX.Direct3D10;
using Debug=System.Diagnostics.Debug;
using Device=SlimDX.Direct3D10.Device;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	internal class RenderTargetArrayHolder
	{
		#region Fields

		private readonly Device device;
		private readonly int width;
		private readonly int height;
		private readonly Format format;
		private readonly int layerCount;

		private Texture2D texture;
		private ShaderResourceView shaderResourceViewAsArray;
		private RenderTargetView renderTargetViewAsArray; 
		#endregion

		#region Properties
		public ShaderResourceView ShaderResourceViewAsArray
		{
			get { return shaderResourceViewAsArray; }
		}

		public RenderTargetView RenderTargetViewAsArray
		{
			get { return renderTargetViewAsArray; }
		} 
		#endregion

		#region Initialization
		public RenderTargetArrayHolder(Device device, int width, int height, Format format, int layerCount)
		{
			Debug.Assert(device != null);
			
			this.device = device;
			this.width = width;
			this.height = height;
			this.format = format;
			this.layerCount = layerCount;

			CreateTexture();
			CreateShaderResourceView();
			CreateRenderTargetView();
		}

		private void CreateTexture()
		{
			var textureDesc = new Texture2DDescription();
			textureDesc.ArraySize = layerCount;
			textureDesc.BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource;
			textureDesc.CpuAccessFlags = CpuAccessFlags.None;
			textureDesc.Format = format;
			textureDesc.Width = width;
			textureDesc.Height = height;
			textureDesc.MipLevels = 1;
			textureDesc.OptionFlags = ResourceOptionFlags.None;
			textureDesc.SampleDescription = new SampleDescription(1, 0);
			textureDesc.Usage = ResourceUsage.Default;

			texture = new Texture2D(device, textureDesc);
		} 

		private void CreateShaderResourceView()
		{
			var shaderResourceViewDesc = new ShaderResourceViewDescription();
			shaderResourceViewDesc.ArraySize = layerCount;
			shaderResourceViewDesc.Dimension = ShaderResourceViewDimension.Texture2DArray;
			shaderResourceViewDesc.FirstArraySlice = 0;
			shaderResourceViewDesc.Format = format;
			shaderResourceViewDesc.MipLevels = 1;
			shaderResourceViewDesc.MostDetailedMip = 0;

			shaderResourceViewAsArray = new ShaderResourceView(device, texture, shaderResourceViewDesc);
		}

		private void CreateRenderTargetView()
		{
			var renderTargetViewDesc = new RenderTargetViewDescription();
			renderTargetViewDesc.ArraySize = layerCount;
			renderTargetViewDesc.Dimension = RenderTargetViewDimension.Texture2DArray;
			renderTargetViewDesc.FirstArraySlice = 0;
			renderTargetViewDesc.Format = format;
			renderTargetViewDesc.MipSlice = 0;

			renderTargetViewAsArray = new RenderTargetView(device, texture, renderTargetViewDesc);
		}
		#endregion
	}
}
