#region Using declarations

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace RomantiqueX.Engine.ContentPipeline
{

	#region Using declarations

	#endregion

	public abstract class ResourceImporter<T> : IResourceImporter
	{
		#region Fields

		private readonly List<string> supportedExtensions = new List<string>();

		#endregion

		#region Creation

		protected ResourceImporter(params string[] supportedExtensions)
		{
			if (supportedExtensions == null)
				throw new ArgumentNullException("supportedExtensions");

			this.supportedExtensions.AddRange(supportedExtensions);
		}

		#endregion

		#region Methods

		protected abstract T LoadAsset(Stream assetStream, AssetImportContext context);

		#endregion

		#region IResourceImporter Members

		public object Load(Stream assetStream, AssetImportContext context)
		{
			if (assetStream == null)
				throw new ArgumentNullException("assetStream");
			if (context == null)
				throw new ArgumentNullException("context");

			return LoadAsset(assetStream, context);
		}

		public bool CanLoadAsset(string assetName)
		{
			string extension = Path.GetExtension(assetName);
			return supportedExtensions.Contains(extension);
		}

		public Type AssetType
		{
			get { return typeof (T); }
		}

		#endregion
	}
}