using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;

namespace RomantiqueX.Engine.ContentPipeline
{
	internal class EffectIncludeHandler : Include
	{
		private readonly FileSystem fileSystem;
        private readonly string originalAssetName;

		#region Initialization

		public EffectIncludeHandler(FileSystem fileSystem, string originalAssetName)
		{
			if (fileSystem == null)
				throw new ArgumentNullException("fileSystem");
			if (originalAssetName == null)
				throw new ArgumentNullException("originalAssetName");

			this.fileSystem = fileSystem;
			this.originalAssetName = originalAssetName;
		}

		#endregion

		#region Include Members

		public void Open(IncludeType type, string fileName, out Stream stream)
		{
			if (type == IncludeType.System)
				stream = fileSystem.GetAssetStream(fileName);
			else
			{
				string path = Path.Combine(Path.GetDirectoryName(fileSystem.GetAssetFullPath(originalAssetName)), fileName);
				stream = fileSystem.GetAssetStream(path);
			}
		}

		public void Close(Stream stream)
		{
			stream.Close();
		}

		#endregion
	}
}
