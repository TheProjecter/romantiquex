using System.Drawing;
using RomantiqueX.Engine;
using RomantiqueX.Engine.Graphics;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DXGI;
using RomantiqueX.Utils.Viewers;
using View = RomantiqueX.Engine.Graphics.View;
using RomantiqueX.Engine.World;
using RomantiqueX.Engine.ContentPipeline;
using RomantiqueX.Engine.Graphics.VisualEffects;

namespace TestArea
{
	public class Program : GameApplication
	{
		// View
		private Camera camera;
		private View view;

		// World
		private Sector sector;
		private WorldObject cubeObject;
		private WorldObject floorObject;

		// Effects
		private DebugEffect testEffect;

		public Program(Control control)
			: base(
				new RendererConfiguration(
					control.ClientRectangle.Width, control.ClientRectangle.Height,
					Format.R8G8B8A8_UNorm, new Rational(60, 1), true, control),
				DeferredShadingConfiguration.Default)
		{
		}

		protected override void Initialize()
		{
			// Setup resource manager
			ResourceManager.FileSystems.Add(new DiskFileSystem("Content"));
			
			// Add view
			camera = new Camera(new Vector3(0, 1, 0), Vector2.Zero, 3.1415f / 3f, 4f / 3f, 0.1f, 100f);
			view = Renderer.CreateDefaultView(camera);
			Renderer.Views.Add(view);

			// Add sector
			sector = new ListSector(new BoundingBox(new Vector3(-10), new Vector3(10)));
			WorldManager.RegisterSector(sector);

			// Fill world
			cubeObject = SimpleGeometryObject.CreateCube("Shaders/Test.fx", "Cube", Services);
			cubeObject.Position = new Vector3(0, 1f, -7);
			sector.RegisterObject(cubeObject);
			floorObject = SimpleGeometryObject.CreatePlane("Shaders/Test.fx", "Floor", Services, 20, 20);
			sector.RegisterObject(floorObject);

			// Add test effect
			testEffect = new DebugEffect(RenderTargetLayerType.Color, 0, Services);
			Renderer.VisualEffects.Add(testEffect);
		}

		protected override void Update(TimeInfo timeInfo)
		{
			cubeObject.Rotation = Quaternion.RotationAxis(Vector3.UnitY, (float)timeInfo.Total) *
			                      Quaternion.RotationAxis(Vector3.UnitX, 0.5f * (float) timeInfo.Total);
			Renderer.WindowCaption = string.Format("FPS: {0}", Timer.FramesPerSecond);
		}

		public static void Main(string[] args)
		{
			SlimDX.Configuration.ThrowOnError = false;
			
			using (var form = new Form())
			{
				form.ClientSize = new Size(1024, 768);
				
				var program = new Program(form);
				program.Run();
			}
		}
	}
}
