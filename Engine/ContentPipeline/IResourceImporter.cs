#region Using declarations

using System;
using System.IO;

#endregion

namespace RomantiqueX.Engine.ContentPipeline
{

	#region Using declarations

	#endregion

	public interface IResourceImporter
	{
		object Load(Stream assetStream, AssetImportContext context);

		bool CanLoadAsset(string assetName);

		Type AssetType { get; }
	}
}