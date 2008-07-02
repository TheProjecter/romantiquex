﻿using System;
using System.Drawing;
using RomantiqueX.Engine;
using RomantiqueX.Engine.Graphics;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DirectInput;
using SlimDX.DXGI;
using RomantiqueX.Utils.Viewers;
using View = RomantiqueX.Engine.Graphics.View;
using RomantiqueX.Engine.World;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Engine.Graphics.VisualEffects;

namespace TestArea
{
	public class Program : EngineApplication
	{
		// Global
		private static Form form;
		
		// View
		private Camera camera;
		private View view;

		// World
		private Sector sector;
		private WorldObject cubeObject;
		private WorldObject fireObject;
		private WorldObject floorObject;

		// Effects
		private AmbientLightingEffect ambientLighting;

		public Program(Control control)
			: base(control,
				new RendererConfiguration(
					control.ClientRectangle.Width, control.ClientRectangle.Height,
					Format.R8G8B8A8_UNorm, new Rational(60, 1), true),
				DeferredShadingConfiguration.Default)
		{
		}

		protected override void Initialize()
		{
			// Setup resource manager
			ResourceManager.FileSystems.Add(new DiskFileSystem("Content"));

			// Setup debug blender
			//Renderer.Blender = new DebugBlender(0, Services);
			
			// Add view
			camera = new Camera(new Vector3(0, 5, 5), new Vector2(-(float)Math.PI / 4f, 0), 3.1415f / 3f, 4f / 3f, 0.1f, 100f);
			view = Renderer.CreateDefaultView(camera);
			Renderer.Views.Add(view);

			// Add sector
			sector = new ListSector(new BoundingBox(new Vector3(-10), new Vector3(10)));
			WorldManager.RegisterSector(sector);

			// Fill world
			cubeObject = SimpleObject.CreateCube("Shaders/Test.fx", "Textures/Crate.jpg", "Cube", Services);
			cubeObject.Position = new Vector3(0, 2, 0);
			sector.RegisterObject(cubeObject);
			fireObject = SimpleObject.CreatePlane("Shaders/Test.fx", "Textures/Fire.dds", "Fire", Services, 5, 5);
			fireObject.Position = new Vector3(0, 0, -6);
			fireObject.Rotation = Quaternion.RotationAxis(Vector3.UnitX, (float) Math.PI / 2f);
			sector.RegisterObject(fireObject);
			floorObject = SimpleObject.CreatePlane("Shaders/Test.fx", "Textures/Floor.jpg", "Floor", Services, 6, 6);
			floorObject.Position = new Vector3(0, -1, 0);
			sector.RegisterObject(floorObject);

			// Add lighting
			ambientLighting = new AmbientLightingEffect(Services);
			Renderer.VisualEffects.Add(ambientLighting);

			// Register input handler
			InputManager.KeyDown += InputManager_KeyDown;
		}

		void InputManager_KeyDown(object sender, RomantiqueX.Engine.Input.KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					Exit();
					break;
			}
		}

		protected override void Update(TimeInfo timeInfo)
		{
			UpdateCamera(timeInfo);

			cubeObject.Rotation = Quaternion.RotationAxis(Vector3.UnitY, (float)timeInfo.Total) *
			                      Quaternion.RotationAxis(Vector3.UnitX, 0.5f * (float) timeInfo.Total);

			Renderer.WindowCaption = string.Format("FPS: {0}", Timer.FramesPerSecond);
		}

		private void UpdateCamera(TimeInfo timeInfo)
		{
			const float cameraSpeed = 2f;
			const float cameraSensitivity = 0.002f;

			var deltaTime = (float) timeInfo.Elapsed;

			if (InputManager.IsKeyPressed(Key.W))
				camera.Move(deltaTime * cameraSpeed);
			if (InputManager.IsKeyPressed(Key.S))
				camera.Move(-deltaTime * cameraSpeed);
			if (InputManager.IsKeyPressed(Key.A))
				camera.Strafe(-deltaTime * cameraSpeed);
			if (InputManager.IsKeyPressed(Key.D))
				camera.Strafe(deltaTime * cameraSpeed);

			Vector2 delta = new Vector2(InputManager.MouseX, InputManager.MouseY) - GetScreenCenter();
			camera.RotateX(-delta.Y * cameraSensitivity);
			camera.RotateY(-delta.X * cameraSensitivity);
			SetupMouseToScreenCenter();
		}

		/// <summary>
		/// Helper for getting application window center position.
		/// </summary>
		/// <returns>Position of application window center.</returns>
		private static Vector2 GetScreenCenter()
		{
			return new Vector2(form.ClientSize.Width * 0.5f, form.ClientSize.Height * 0.5f);
		}

		/// <summary>
		/// Helper for setting mouse position at the center of application window.
		/// </summary>
		private void SetupMouseToScreenCenter()
		{
			Vector2 screenCenter = GetScreenCenter();
			InputManager.MouseX = (int) screenCenter.X;
			InputManager.MouseY = (int) screenCenter.Y;
		}

		public static void Main(string[] args)
		{
			SlimDX.Configuration.ThrowOnError = false;
			
			using (form = new Form {ClientSize = new Size(1024, 768)})
			using (var program = new Program(form))
			{
				program.Run();
			}
		}
	}
}
