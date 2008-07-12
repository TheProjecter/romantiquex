using System;
using System.Collections.Generic;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Utils;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.Graphics.VisualEffects
{
	internal delegate void SetGeometryBufferTexturesHandler(Effect effect, IEnumerable<RenderTargetLayerType> requiredRenderTargets);

	public class VisualEffect
	{
		#region Fields
        private readonly Effect effect;
		private readonly EffectTechnique effectTechnique;

		private readonly Renderer renderer;
		private readonly ResourceManager resourceManager;

		private readonly IEnumerable<RenderTargetLayerType> requiredRenderTargets;

		private readonly int effectLayerCount;
        #endregion

		#region Properties

		public Renderer Renderer
		{
			get { return renderer; }
		}

		protected Effect Effect
		{
			get { return effect; }
		}

		#endregion

		#region Constants

		public const string VisualEffectTechniqueName = "RenderEffect";

		#endregion

		protected VisualEffect(IServiceProvider services, string effectAsset,
			int effectLayerCount, IEnumerable<RenderTargetLayerType> requiredRenderTargets)
		{
			if (services == null)
				throw new ArgumentNullException("services");
			if (effectAsset == null)
				throw new ArgumentNullException("effectAsset");
			if (requiredRenderTargets == null)
				throw new ArgumentNullException("requiredRenderTargets");
			if (effectLayerCount < 0)
				throw new ArgumentOutOfRangeException("affectedLayers", "Parameter should have non-negative value.");

			renderer = (Renderer)services.GetService(typeof(Renderer));
			resourceManager = (ResourceManager)services.GetService(typeof(ResourceManager));
			
			this.requiredRenderTargets = requiredRenderTargets;
			this.effectLayerCount = effectLayerCount > 0 ? effectLayerCount : renderer.KBufferManager.Configuration.LayerCount;

			effect = resourceManager.Load<Effect>(effectAsset);
			effectTechnique = effect.GetTechniqueByName(VisualEffectTechniqueName);
			if (!effectTechnique.IsValid)
				throw new ArgumentException(
					string.Format("Given effect asset '{0}' does not contain technique {1}.", effectAsset, VisualEffectTechniqueName),
					"effectAsset");
		}

		internal void Apply(View view, SetGeometryBufferTexturesHandler setGeometryBufferTexturesHandler)
		{
			if (view == null)
				throw new ArgumentNullException("view");
			if (setGeometryBufferTexturesHandler == null)
				throw new ArgumentNullException("setGeometryBufferTexturesHandler");

			//var region = CalculateEffectRegion()
			SetupEffectParameters(setGeometryBufferTexturesHandler);
			PrepareForApply(view);
			renderer.DrawProceduralEffect(effectTechnique);
		}

		private void SetupEffectParameters(SetGeometryBufferTexturesHandler setGeometryBufferTexturesHandler)
		{
			setGeometryBufferTexturesHandler.Invoke(effect, requiredRenderTargets);
			effect.SetVariableBySemantic(StandartSemantics.LayerCount, effectLayerCount, false);
		}

		protected virtual void PrepareForApply(View view)
		{
		}

		protected virtual ScreenRegion CalculateEffectRegion(View view)
		{
			return new ScreenRegion(view.Viewport.X, view.Viewport.Y, view.Viewport.Width, view.Viewport.Height);
		}

		public virtual bool AffectsVolume(VolumeAdapter volumeAdapter)
		{
			return true;
		}
	}
}