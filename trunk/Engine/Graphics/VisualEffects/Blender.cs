using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Utils;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	public abstract class Blender
	{
		#region Fields
        private readonly ResourceManager resourceManager;
		private readonly Renderer renderer;

		private readonly Effect effect;
		private readonly EffectTechnique effectTechnique;
		#endregion

		#region Constants
		public const string BlenderTechniqueName = "BlendLayers";
		#endregion

		#region Properties
		protected Effect Effect
		{
			get { return effect; }
		} 
		#endregion

		#region Initialization
		protected Blender(IServiceProvider services, string effectAsset)
		{
			if (services == null)
				throw new ArgumentNullException("services");
			if (effectAsset == null)
				throw new ArgumentNullException("effectAsset");

			resourceManager = (ResourceManager) services.GetService(typeof (ResourceManager));
			renderer = (Renderer) services.GetService(typeof (Renderer));

			effect = resourceManager.Load<Effect>(effectAsset);
			effectTechnique = effect.GetTechniqueByName(BlenderTechniqueName);
		} 
		#endregion

		internal void Blend(ShaderResourceView renderTargetsToBlend)
		{
			if (renderTargetsToBlend == null)
				throw new ArgumentNullException("renderTargetsToBlend");

			effect.SetVariableBySemantic(StandartSemantics.ColorLayersToBlend, renderTargetsToBlend, true);
			effect.SetVariableBySemantic(StandartSemantics.LayerCount, renderTargetsToBlend.Description.ArraySize, true);

			PrepareForBlend();

			renderer.DrawProceduralEffect(effectTechnique);
		}

		protected virtual void PrepareForBlend()
		{
		}
	}
}
