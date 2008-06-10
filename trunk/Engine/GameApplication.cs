#region Using declarations

using System;
using System.Windows.Forms;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Engine.Graphics;
using RomantiqueX.Engine.World;
using System.ComponentModel.Design;

#endregion

namespace RomantiqueX.Engine
{
	public class GameApplication
	{
		#region Fields

		private bool loopFinished;
		private bool isFirstTimeUpdate = true;

		public Renderer Renderer { get; private set; }
		public WorldManager WorldManager { get; private set; }
		public ResourceManager ResourceManager { get; private set; }
		public Timer Timer { get; private set; }

		public ServiceContainer Services { get; private set; }

		#endregion

		#region Initialization

		public GameApplication(RendererConfiguration rendererConfig, DeferredShadingConfiguration dsConfig)
		{
			Services = new ServiceContainer();
			
			Renderer = new Renderer(rendererConfig, dsConfig, Services);
			WorldManager = new WorldManager(Services);
			ResourceManager = new ResourceManager(Services);
			Timer = new Timer(Services);
		}

		protected virtual void Initialize()
		{
		}

		#endregion

		#region High-level management

		public void Run()
		{
			Initialize();
			
			while (!loopFinished && Renderer.CanRender)
			{
				OnUpdate();
				OnRender();
			}
		}

		public void Exit()
		{
			loopFinished = true;
		}

		#endregion

		#region Update

		private void OnUpdate()
		{
			// Only for the first time
			if (isFirstTimeUpdate)
			{
				Timer.Start();
				isFirstTimeUpdate = false;
			}

			// Let window system process all messages
			Application.DoEvents();

			// Update subsystems
			Timer.Update();

			// Invoke application update
			Update(Timer.Time);
		}

		protected virtual void Update(TimeInfo timeInfo)
		{
		}

		#endregion

		#region Render

		private void OnRender()
		{
			// Fill attached views
			Renderer.RenderFrame();

			// Invoke application render
			Render(Timer.Time);
		}

		protected virtual void Render(TimeInfo timeInfo)
		{
		}

		#endregion
	}
}