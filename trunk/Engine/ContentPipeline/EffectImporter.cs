using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;
using RomantiqueX.Engine.Graphics;

namespace RomantiqueX.Engine.ContentPipeline
{
	public class EffectImporter : ResourceImporter<Effect>
	{
		public EffectImporter()
			: base(".fx")
		{
		}
		
		protected override Effect LoadAsset(Stream assetStream, AssetImportContext context)
		{
			var renderer = (Renderer)context.Services.GetService(typeof(Renderer));

			string compilationErrors;
			var includeHandler = new EffectIncludeHandler(context.FileSystem, context.AssetName);
			Effect effect = Effect.FromStream(renderer.Device, assetStream, "fx_4_0", ShaderFlags.None, EffectFlags.None, null,
				includeHandler, out compilationErrors);
			if (!String.IsNullOrEmpty(compilationErrors))
				throw new EffectCompilationException(compilationErrors);

			return effect;
		}
	}
}
