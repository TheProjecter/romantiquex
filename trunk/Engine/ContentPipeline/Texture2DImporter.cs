using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RomantiqueX.Engine.Graphics;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.ContentPipeline
{
	public class Texture2DImporter : ResourceImporter<Texture2D>
	{
		public Texture2DImporter()
			: base(".png", ".bmp", ".jpg", ".dds")
		{
		}

		protected override Texture2D LoadAsset(Stream assetStream, AssetImportContext context)
		{
			var renderer = (Renderer)context.Services.GetService(typeof(Renderer));

			Texture2D texture = Texture2D.FromStream(renderer.Device, assetStream, (int)assetStream.Length);
			return texture;
		}
	}
}
