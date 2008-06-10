using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Globalization;
using SlimDX;

namespace RomantiqueX.Utils.Math
{
	[Serializable]
	public class TriangleCollection : Collection<Triangle>
	{
	}

	[Serializable]
	public class TriangleReadOnlyCollection : ReadOnlyCollection<Triangle>
	{
		public TriangleReadOnlyCollection(IList<Triangle> triangles)
			: base(triangles)
		{
		}
	}

	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Triangle : IEquatable<Triangle>
	{
		#region Fields

		public Vector3 Point0;
		public Vector3 Point1;
		public Vector3 Point2;

		public Vector3 Normal
		{
			get { return Vector3.Normalize(Vector3.Cross(Point1 - Point0, Point2 - Point0)); }
		}

		#endregion

		#region Creation

		public Triangle(Vector3 point0, Vector3 point1, Vector3 point2)
		{
			this.Point0 = point0;
			this.Point1 = point1;
			this.Point2 = point2;
		}

		#endregion

		#region Operators

		public static bool operator ==(Triangle value1, Triangle value2)
		{
			return value1.Point0 == value2.Point0 && value1.Point1 == value2.Point1 && value1.Point2 == value2.Point2;
		}

		public static bool operator !=(Triangle value1, Triangle value2)
		{
			return value1.Point0 != value2.Point0 || value1.Point1 != value2.Point1 || value1.Point2 != value2.Point2;
		}

		#endregion

		#region Stuff

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			return this == (Triangle)obj;
		}

		public override int GetHashCode()
		{
			return Point0.GetHashCode() + Point1.GetHashCode() + Point2.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{{P0:{0} P1:{1} P2:{2}}}", Point0, Point1, Point2);
		}

		#endregion

		#region IEquatable<Triangle> Members

		public bool Equals(Triangle other)
		{
			return this == other;
		}

		#endregion
	}
}