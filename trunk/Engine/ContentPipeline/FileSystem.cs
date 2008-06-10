#region Using declarations

using System.IO;

#endregion

namespace RomantiqueX.Engine.ContentPipeline
{

	#region Using declarations

	#endregion

	public abstract class FileSystem
	{
		public abstract bool CanLoadAsset(string assetName);

		public abstract Stream GetAssetStream(string assetName);

		public abstract string GetAssetFullPath(string assetName);
	}
}