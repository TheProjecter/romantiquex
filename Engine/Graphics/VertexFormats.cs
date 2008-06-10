using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;

namespace RomantiqueX.Engine.Graphics
{
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionColorVertex
	{
		private Vector4 position;

		public Vector4 Position
		{
			get { return position; }
			set { position = value; }
		}

		private int color;

		public int Color
		{
			get { return color; }
			set { color = value; }
		}

		public const int SizeInBytes = 20;

		public static InputElement[] InputElements = new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
				new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 16, 0), 
			};

		public PositionColorVertex(Vector4 position, int color)
		{
			this.position = position;
			this.color = color;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PositionNormalColorVertex
	{
		private Vector4 position;

		public Vector4 Position
		{
			get { return position; }
			set { position = value; }
		}

		private Vector3 normal;

		public Vector3 Normal
		{
			get { return normal; }
			set { normal = value; }
		}
		
		private int color;

		public int Color
		{
			get { return color; }
			set { color = value; }
		}

		public const int SizeInBytes = 32;

		public static InputElement[] InputElements = new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
				new InputElement("NORMAL", 0, Format.R32G32B32_Float, 16, 0),
				new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 28, 0), 
			};

		public PositionNormalColorVertex(Vector4 position, Vector3 normal, int color)
		{
			this.position = position;
			this.normal = normal;
			this.color = color;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PositionTexCoordVertex
	{
		private Vector4 position;

		public Vector4 Position
		{
			get { return position; }
			set { position = value; }
		}

		private Vector2 texCoord;

		public Vector2 TexCoord
		{
			get { return texCoord; }
			set { texCoord = value; }
		}

		public const int SizeInBytes = 24;

		public static InputElement[] InputElements = new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
				new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0), 
			};

		public PositionTexCoordVertex(Vector4 position, Vector2 texCoord)
		{
			this.position = position;
			this.texCoord = texCoord;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PositionTexCoordNormalVertex
	{
		private Vector4 position;

		public Vector4 Position
		{
			get { return position; }
			set { position = value; }
		}

		private Vector2 texCoord;

		public Vector2 TexCoord
		{
			get { return texCoord; }
			set { texCoord = value; }
		}

		private Vector3 normal;

		public Vector3 Normal
		{
			get { return normal; }
			set { normal = value; }
		}

		public const int SizeInBytes = 36;

		public static InputElement[] InputElements = new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
				new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0), 
				new InputElement("NORMAL", 0, Format.R32G32B32_Float, 24, 0),
			};

		public PositionTexCoordNormalVertex(Vector4 position, Vector2 texCoord, Vector3 normal)
		{
			this.position = position;
			this.texCoord = texCoord;
			this.normal = normal;
		}
	}
}
