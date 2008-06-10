#region Using declarations

using System.Diagnostics;
using System;

#endregion

namespace RomantiqueX.Engine.ContentPipeline
{

	#region Using declarations

	#endregion

	public class AssetImportContext
	{
		public ResourceManager ResourceManager { get; private set; }

		public IServiceProvider Services { get; private set; }

		public string AssetName { get; private set; }

		public FileSystem FileSystem { get; private set; }

		internal AssetImportContext(ResourceManager resourceManager, IServiceProvider services, string assetName, FileSystem fileSystem)
		{
			Debug.Assert(resourceManager != null && services != null);

			ResourceManager = resourceManager;
			Services = services;
			AssetName = assetName;
			FileSystem = fileSystem;
		}
	}
}