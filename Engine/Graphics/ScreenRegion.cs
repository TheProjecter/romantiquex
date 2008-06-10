using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomantiqueX.Engine.Graphics
{
	public struct ScreenRegion
	{
		private int left;
		public int Left
		{
			get { return left; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value", "Value should not be negative.");
				left = value;
			}
		}

		private int top;
		public int Top
		{
			get { return top; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value", "Value should not be negative.");
				top = value;
			}
		}

		private int width;
		public int Width
		{
			get { return width; }
			set
			{
				width = value;
				if (value < 0)
					throw new ArgumentOutOfRangeException("value", "Value should not be negative.");
			}
		}

		private int height;
		public int Height
		{
			get { return height; }
			set
			{
				height = value;
				if (value < 0)
					throw new ArgumentOutOfRangeException("value", "Value should not be negative.");
			}
		}

		public ScreenRegion(int left, int top, int width, int height)
		{
			if (left < 0)
				throw new ArgumentOutOfRangeException("left", "Value should not be negative.");
			if (top < 0)
				throw new ArgumentOutOfRangeException("top", "Value should not be negative.");
			if (width < 0)
				throw new ArgumentOutOfRangeException("width", "Value should not be negative.");
			if (height < 0)
				throw new ArgumentOutOfRangeException("height", "Value should not be negative.");
			
			this.left = left;
			this.top = top;
			this.width = width;
			this.height = height;
		}
	}
}
