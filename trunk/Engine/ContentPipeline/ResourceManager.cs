#region Using declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ComponentModel.Design;

#endregion

namespace RomantiqueX.Engine.ContentPipeline
{

	#region Using declarations

	#endregion

	public class ResourceManager : EngineComponent<ResourceManager>
	{
		#region Nested type: LoadedAssetDescription

		private class LoadedAssetDescription
		{
			#region Fields

			private readonly Type type;

			public Type Type
			{
				get { return type; }
			}

			private readonly object asset;

			public object Asset
			{
				get { return asset; }
			}

			#endregion

			#region Creation

			public LoadedAssetDescription(Type type, object asset)
			{
				Debug.Assert(type != null && asset != null);

				this.type = type;
				this.asset = asset;
			}

			#endregion
		}

		#endregion

		#region Fields

		private readonly FileSystemCollection fileSystems = new FileSystemCollection();

		public FileSystemCollection FileSystems
		{
			get { return fileSystems; }
		}

		private readonly ResourceImporterCollection resourceImporters = new ResourceImporterCollection();

		public ResourceImporterCollection ResourceImporters
		{
			get { return resourceImporters; }
		}

		private readonly Dictionary<string, List<LoadedAssetDescription>> cachedAssets =
			new Dictionary<string, List<LoadedAssetDescription>>();

		#endregion

		#region Initialization

		public ResourceManager(IServiceContainer services)
			: base(services)
		{
			// Add default importers
			resourceImporters.Add(new EffectImporter());
			resourceImporters.Add(new Texture2DImporter());
			// Add default file system
			fileSystems.Add(new DiskFileSystem("Content/Engine"));
		}

		#endregion

		#region Methods

		#region Public

		public T Load<T>(string assetName)
		{
			return Load<T>(assetName, true);
		}

		public T Load<T>(string assetName, bool lookInCache)
		{
			if (String.IsNullOrEmpty(assetName))
				throw new ArgumentException("Asset name can't be null or empty.", "assetName");

			Type requestedType = typeof(T);

			// Check cache for already loaded assets of given name and type
			if (lookInCache)
			{
				object cachedAsset = RequestAssetFromCache(assetName, requestedType);
				if (cachedAsset != null)
					return (T)cachedAsset;
			}

			// Find file system and importer for asset
			FileSystem fileSystem = FindSuitableFileSystem(assetName);
			if (fileSystem == null)
				throw new SuitableFileSystemNotFoundException(assetName);
			IResourceImporter importer = FindAppropriateImporter(assetName, requestedType);
			if (importer == null)
				throw new AppropriateImporterNotFoundException(assetName, requestedType);

			// Load asset
			Stream assetStream = fileSystem.GetAssetStream(assetName);
			var asset = (T)importer.Load(assetStream, new AssetImportContext(this, Services, assetName, fileSystem));
			assetStream.Close();

			// Save asset in cache
			SaveAssetInCache(assetName, requestedType, asset);

			return asset;
		}

		#endregion

		#region Private

		private void SaveAssetInCache(string assetName, Type requestedType, object asset)
		{
			var desc = new LoadedAssetDescription(requestedType, asset);
			if (!cachedAssets.ContainsKey(assetName))
				cachedAssets.Add(assetName, new List<LoadedAssetDescription>());
			cachedAssets[assetName].Add(desc);
		}

		private object RequestAssetFromCache(string assetName, Type type)
		{
			if (!cachedAssets.ContainsKey(assetName))
				return null;

			List<LoadedAssetDescription> assetsWithGivenName = cachedAssets[assetName];
			Debug.Assert(assetsWithGivenName != null);
			foreach (LoadedAssetDescription desc in assetsWithGivenName)
				if (desc.Type == type)
					return desc.Asset;

			return null;
		}

		private IResourceImporter FindAppropriateImporter(string assetName, Type assetType)
		{
			foreach (IResourceImporter importer in resourceImporters)
				if (importer.CanLoadAsset(assetName) && importer.AssetType == assetType)
					return importer;
			return null;
		}

		private FileSystem FindSuitableFileSystem(string assetName)
		{
			foreach (FileSystem fileSystem in fileSystems)
				if (fileSystem.CanLoadAsset(assetName))
					return fileSystem;
			return null;
		}

		#endregion

		#region Protected

		protected override void Dispose(bool disposing)
		{
			foreach (KeyValuePair<string, List<LoadedAssetDescription>> pair in cachedAssets)
			{
				foreach (LoadedAssetDescription description in pair.Value)
				{
					var disposableAsset = description.Asset as IDisposable;
					if (disposableAsset != null)
						disposableAsset.Dispose();
				}
			}

			cachedAssets.Clear();
		}

		#endregion

		#endregion
	}
}