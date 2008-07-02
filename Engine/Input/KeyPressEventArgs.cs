using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.DirectInput;

namespace RomantiqueX.Engine.Input
{
	public class KeyEventArgs : EventArgs
	{
		public Key Key { get; internal set; }
	}
}
