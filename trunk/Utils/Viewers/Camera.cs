using System;
using System.Collections.Generic;
using System.Text;
using RomantiqueX.Utils.Math;
using SlimDX;

namespace RomantiqueX.Utils.Viewers
{
	public class Camera : Viewer
	{
		#region Fields

		private Matrix viewMatrix;
		private Matrix projectionMatrix;
		private BoundingFrustum frustum = new BoundingFrustum(Matrix.Identity);
		private Vector3 position;
		private Vector2 angles;
		private Vector3 forward;
		private Vector3 right;
		private Vector3 up;
		private float fieldOfView;
		private float aspectRatio;
		private float nearPlane;
		private float farPlane;

		#endregion

		#region Properties

		public override Matrix ViewMatrix
		{
			get { return viewMatrix; }
		}

		public override Matrix ProjectionMatrix
		{
			get { return projectionMatrix; }
		}

		public override BoundingFrustum Frustum
		{
			get { return frustum; }
		}

		public override Vector3 Position
		{
			get { return position; }
			set
			{
				position = value;
				RecalculateData();
			}
		}

		public Vector2 Angles
		{
			get { return angles; }
			set
			{
				angles = value;
				RecalculateData();
			}
		}

		public float FieldOfView
		{
			get { return fieldOfView; }
			set
			{
				fieldOfView = value;
				RecalculateData();
			}
		}

		public float AspectRatio
		{
			get { return aspectRatio; }
			set
			{
				aspectRatio = value;
				RecalculateData();
			}
		}

		public override float NearPlane
		{
			get { return nearPlane; }
			set
			{
				nearPlane = value;
				RecalculateData();
			}
		}

		public override float FarPlane
		{
			get { return farPlane; }
			set
			{
				farPlane = value;
				RecalculateData();
			}
		}

		#endregion

		#region Creation

		public Camera(Vector3 position, Vector2 angles, float fieldOfView, float aspectRatio, float nearPlane, float farPlane)
		{
			this.position = position;
			this.angles = angles;
			this.fieldOfView = fieldOfView;
			this.aspectRatio = aspectRatio;
			this.nearPlane = nearPlane;
			this.farPlane = farPlane;

			RecalculateData();
		}

		#endregion

		#region Methods

		private void RecalculateData()
		{
			var rotation = Matrix.RotationX(angles.X) * Matrix.RotationY(angles.Y);
			
			forward = Vector3.TransformNormal(-Vector3.UnitZ, rotation);
			up = Vector3.TransformNormal(Vector3.UnitY, rotation);
			right = Vector3.TransformNormal(Vector3.UnitX, rotation);

			viewMatrix = Matrix.LookAtRH(position, position + forward, up);
			projectionMatrix = Matrix.PerspectiveFovRH(fieldOfView, aspectRatio, nearPlane, farPlane);
			frustum.FrustumMatrix = viewMatrix * projectionMatrix;
		}

		public void Move(float distance)
		{
			position += forward * distance;
			RecalculateData();
		}
		
		public void Strafe(float distance)
		{
			position += right * distance;
			RecalculateData();
		}

		public void RotateX(float angle)
		{
			angles.X += angle;
			RecalculateData();
		}

		public void RotateY(float angle)
		{
			angles.Y += angle;
			RecalculateData();
		}

		#endregion
	}
}
