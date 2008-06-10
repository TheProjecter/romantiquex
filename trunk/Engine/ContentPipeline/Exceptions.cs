#region Using declarations

using System;
using System.Globalization;
using System.Runtime.Serialization;

#endregion

namespace RomantiqueX.Engine.ContentPipeline
{

	#region Using declarations

	#endregion

	[Serializable]
	public class SuitableFileSystemNotFoundException : Exception
	{
		public SuitableFileSystemNotFoundException()
			: base("Can't find suitable file system.")
		{
		}

		public SuitableFileSystemNotFoundException(string assetName)
			: base(
				string.Format(CultureInfo.CurrentCulture, "Can't find file system that can load asset with name '{0}'.", assetName))
		{
		}

		public SuitableFileSystemNotFoundException(string assetName, Exception inner)
			: base(
				string.Format(CultureInfo.CurrentCulture, "Can't find file system that can load asset with name '{0}'.", assetName),
				inner)
		{
		}

		protected SuitableFileSystemNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class AppropriateImporterNotFoundException : Exception
	{
		public AppropriateImporterNotFoundException(string assetName, Type requestedType)
			: base(
				string.Format(CultureInfo.CurrentCulture, "Can't find importer for asset with name '{0}' and result type {1}.",
				              assetName, requestedType))
		{
		}

		public AppropriateImporterNotFoundException(string assetName)
			: base(string.Format(CultureInfo.CurrentCulture, "Can't find importer for asset with name '{0}'.", assetName))
		{
		}

		public AppropriateImporterNotFoundException(string assetName, Exception inner)
			: base(string.Format(CultureInfo.CurrentCulture, "Can't find importer for asset with name '{0}'.", assetName), inner)
		{
		}

		public AppropriateImporterNotFoundException()
			: base("Can't find importer for asset.")
		{
		}

		protected AppropriateImporterNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}