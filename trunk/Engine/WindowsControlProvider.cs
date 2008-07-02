using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace RomantiqueX.Engine
{
	public class WindowsControlProvider : EngineComponent<WindowsControlProvider>
	{
		public Control Control { get; private set; }
		
		public WindowsControlProvider(Control control, IServiceContainer services)
			: base(services)
		{
			this.Control = control;
		}
	}
}
