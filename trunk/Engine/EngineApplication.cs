#region Using declarations

using System;
using System.Windows.Forms;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Engine.Graphics;
using RomantiqueX.Engine.Input;
using RomantiqueX.Engine.World;
using System.ComponentModel.Design;

#endregion

namespace RomantiqueX.Engine
{
	public class EngineApplication : IDisposable
	{
		#region Fields

		private bool loopFinished;
		private bool isFirstTimeUpdate = true;

		private readonly WindowsControlProvider windowsControlProvider;
		private readonly Renderer renderer;
		private readonly WorldManager worldManager;
		private readonly ResourceManager resourceManager;
		private readonly Timer timer;
		private readonly InputManager inputManager;

		private readonly ServiceContainer services;

        #endregion

		#region Properties

		public Renderer Renderer
		{
			get { return renderer; }
		}

		public WindowsControlProvider WindowsControlProvider
		{
			get { return windowsControlProvider; }
		}

		public WorldManager WorldManager
		{
			get { return worldManager; }
		}

		public ResourceManager ResourceManager
		{
			get { return resourceManager; }
		}

		public Timer Timer
		{
			get { return timer; }
		}

		public InputManager InputManager
		{
			get { return inputManager; }
		}

		public ServiceContainer Services
		{
			get { return services; }
		}

		#endregion

		#region Initialization

		public EngineApplication(Control control, RendererConfiguration rendererConfig, DeferredShadingConfiguration deferredShadingConfig)
		{
			services = new ServiceContainer();

			windowsControlProvider = new WindowsControlProvider(control, Services);
			resourceManager = new ResourceManager(Services);
			renderer = new Renderer(rendererConfig, deferredShadingConfig, Services);
			worldManager = new WorldManager(Services);
			timer = new Timer(Services);
			inputManager = new InputManager(Services);
		}

		protected virtual void Initialize()
		{
		}

		#endregion

		#region Methods

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
				timer.Start();
				isFirstTimeUpdate = false;
			}

			// Update subsystems
			timer.Update(timer.Time);
			inputManager.Update(timer.Time);
			worldManager.Update(timer.Time);
			resourceManager.Update(timer.Time);
			renderer.Update(timer.Time);

			// Invoke application update
			Update(timer.Time);

			// Let window system process all messages
			Application.DoEvents();
		}

		protected virtual void Update(TimeInfo timeInfo)
		{
		}

		#endregion

		#region Render

		private void OnRender()
		{
			// Fill attached views
			renderer.RenderFrame();

			// Invoke application render
			Render(timer.Time);
		}

		protected virtual void Render(TimeInfo timeInfo)
		{
		}

		#endregion

		#region IDisposable Members

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			inputManager.Dispose();
			timer.Dispose();
			worldManager.Dispose();
			renderer.Dispose();
			resourceManager.Dispose();
		}

		#endregion

		#endregion
	}
}