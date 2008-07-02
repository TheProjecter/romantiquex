using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using SlimDX;
using SlimDX.DirectInput;
using RomantiqueX.Engine.Graphics;
using System.Windows.Forms;

namespace RomantiqueX.Engine.Input
{
	public class InputManager : EngineComponent<InputManager>
	{
		#region Fields

		private readonly Control control;
		private readonly Device<KeyboardState> keyboard;

		private readonly Dictionary<Key, bool> keyState = new Dictionary<Key, bool>();

		private readonly KeyEventArgs keyEventArgs = new KeyEventArgs();

		#endregion

		#region Constants

		public const int KeyboardBufferSize = 8;

		#endregion

		#region Properties

		public int MouseX
		{
			get { return control.PointToClient(Cursor.Position).X; }
			set { Cursor.Position = control.PointToScreen(new Point(value, MouseY)); }
		}

		public int MouseY
		{
			get { return control.PointToClient(Cursor.Position).Y; }
			set { Cursor.Position = control.PointToScreen(new Point(MouseX, value)); }
		}

		#endregion

		#region Initialization

		public InputManager(IServiceContainer services)
			: base(services)
		{
			DirectInput.Initialize();

			var windowsControlProvider = (WindowsControlProvider)services.GetService(typeof(WindowsControlProvider));
			control = windowsControlProvider.Control;

			keyboard = new Device<KeyboardState>(SystemGuid.Keyboard);
			keyboard.SetCooperativeLevel(control, CooperativeLevel.Exclusive | CooperativeLevel.Foreground);
			keyboard.Properties.BufferSize = KeyboardBufferSize;

			// Initialize keys
			foreach (Key key in Enum.GetValues(typeof(Key)))
				keyState.Add(key, false);
		}

		#endregion

		#region Events

		public event EventHandler<KeyEventArgs> KeyDown;

		public event EventHandler<KeyEventArgs> KeyUp;

		#endregion

		#region Methods

		#region Public

		public bool IsKeyPressed(Key key)
		{
			return keyState[key];
		}

		public override void Update(TimeInfo timeInfo)
		{
			// Be sure that the device is still acquired
			if (keyboard.Acquire().IsFailure)
				return;

			// Poll for more input
			if (keyboard.Poll().IsFailure)
				return;

			// Get the list of buffered data events
			BufferedDataCollection<KeyboardState> bufferedData = keyboard.GetBufferedData();
			if (Result.Last.IsFailure)
				return;

			// Update key states and raise appropriate events
			foreach (BufferedData<KeyboardState> packet in bufferedData)
			{
				foreach (Key key in packet.Data.PressedKeys)
					if (!keyState[key])
					{
						keyState[key] = true;
						keyEventArgs.Key = key;
						if (KeyDown != null)
							KeyDown(this, keyEventArgs);
					}
				foreach (Key key in packet.Data.ReleasedKeys)
					if (keyState[key])
					{
						keyState[key] = false;
						keyEventArgs.Key = key;
						if (KeyUp != null)
							KeyUp(this, keyEventArgs);
					}
			}
		}

		#endregion

		#region Protected

		protected override void Dispose(bool disposing)
		{
			keyboard.Unacquire();
			keyboard.Dispose();
		}

		#endregion

		#endregion
	}
}