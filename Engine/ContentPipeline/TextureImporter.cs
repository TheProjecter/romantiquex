using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RomantiqueX.Engine.Graphics;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.ContentPipeline
{
	public class TextureImporter : ResourceImporter<Texture>
	{
		public TextureImporter()
			: base(".tga", ".bmp", ".jpg", ".dds")
		{

		}

		protected override Texture LoadAsset(Stream assetStream, AssetImportContext context)
		{
			var renderer = (Renderer)context.Services.GetService(typeof(Renderer));

			Texture texture = Texture.FromStream(renderer.Device, assetStream, (int)assetStream.Length);
			return texture;
		}
	}
}
