using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace RomantiqueX.Engine.Graphics
{
	public class RenderTargetDescriptionCollection : Collection<RenderTargetDescription>
	{
	}

	public class RenderTargetDescriptionReadOnlyCollection : ReadOnlyCollection<RenderTargetDescription>
	{
		public RenderTargetDescriptionReadOnlyCollection(IList<RenderTargetDescription> descriptions)
			: base(descriptions)
		{
		}
	}
}
