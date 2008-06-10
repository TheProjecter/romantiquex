using System;
using System.Collections.Generic;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Engine.Graphics.KBuffer;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.Graphics
{
	public delegate void SetGeometryBufferTexturesHandler(Effect effect, IEnumerable<RenderTargetLayerType> requiredRenderTargets);
	
	public class VisualEffect
	{
		#region Fields

		private readonly Effect effect;
		private readonly EffectTechnique effectTechnique;
		private readonly Renderer renderer;
		private readonly ResourceManager resourceManager;
		private readonly IEnumerable<RenderTargetLayerType> requredRenderTargets;

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

		protected VisualEffect(IServiceProvider services, string effectAsset, IEnumerable<RenderTargetLayerType> requredRenderTargets)
		{
			if (services == null)
				throw new ArgumentNullException("services");
			if (effectAsset == null)
				throw new ArgumentNullException("effectAsset");
			if (requredRenderTargets == null)
				throw new ArgumentNullException("requredRenderTargets");

			this.requredRenderTargets = requredRenderTargets;
			
			renderer = (Renderer) services.GetService(typeof (Renderer));
			resourceManager = (ResourceManager)services.GetService(typeof(ResourceManager));
			
			effect = resourceManager.Load<Effect>(effectAsset);
			effectTechnique = effect.GetTechniqueByName(VisualEffectTechniqueName);
			if (!effectTechnique.IsValid)
				throw new ArgumentException(
					string.Format("Given effect asset '{0}' does not contain technique {1}.", effectAsset, VisualEffectTechniqueName),
					"effectAsset");
		}

		public void Apply(View view, SetGeometryBufferTexturesHandler setGeometryBufferTexturesHandler)
		{
			if (view == null)
				throw new ArgumentNullException("view");
			if (setGeometryBufferTexturesHandler == null)
				throw new ArgumentNullException("setGeometryBufferTexturesHandler");

			//var region = CalculateEffectRegion()
			setGeometryBufferTexturesHandler.Invoke(effect, requredRenderTargets);
			PrepareForApply(view);
			renderer.DrawProceduralEffect(effectTechnique);
		}

		protected virtual void PrepareForApply(View view)
		{
		}

		protected virtual ScreenRegion CalculateEffectRegion(View view)
		{
			return new ScreenRegion(view.Viewport.X, view.Viewport.Y, view.Viewport.Width, view.Viewport.Height);
		}
	}
}
