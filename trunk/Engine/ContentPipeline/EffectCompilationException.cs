using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomantiqueX.Engine.ContentPipeline
{
	public class EffectCompilationException : Exception
	{
		public EffectCompilationException(string message)
			: base(message)
		{
		}
	}
}
