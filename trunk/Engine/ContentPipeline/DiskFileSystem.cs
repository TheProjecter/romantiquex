#region Using declarations

using System;
using System.Diagnostics;
using System.IO;

#endregion

namespace RomantiqueX.Engine.ContentPipeline
{

	#region Using declarations

	#endregion

	public class DiskFileSystem : FileSystem
	{
		private string root = "";

		public string Root
		{
			get { return root; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				root = value;
			}
		}

		public DiskFileSystem()
		{
		}

		public DiskFileSystem(string root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			this.root = root;
		}

		public override string GetAssetFullPath(string assetName)
		{
			if (assetName == null)
				throw new ArgumentNullException("assetName");
            
			if (!Path.IsPathRooted(assetName))
				return Path.GetFullPath(Path.Combine(root, assetName));
			
			return assetName;
		}
		
		public override bool CanLoadAsset(string assetName)
		{
			if (String.IsNullOrEmpty(assetName))
				throw new ArgumentException("Asset name can't be null or empty.", "assetName");

			return File.Exists(GetAssetFullPath(assetName));
		}

		public override Stream GetAssetStream(string assetName)
		{
			if (String.IsNullOrEmpty(assetName))
				throw new ArgumentException("Asset name can't be null or empty.", "assetName");

			return File.OpenRead(GetAssetFullPath(assetName));
		}
	}
}