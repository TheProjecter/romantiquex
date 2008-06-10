using System;
using SlimDX.Direct3D10;
using SlimDX;
using Debug=System.Diagnostics.Debug;

namespace RomantiqueX.Utils
{
	public static class EffectHelper
	{
		#region Retrieve effect variable

		private static EffectVariable RetrieveVariableBySemantic(Effect effect, string semantic, bool throwIfNotFound)
		{
			if (String.IsNullOrEmpty(semantic))
				throw new ArgumentNullException("semantic");
			Debug.Assert(effect != null);

			EffectVariable variable = effect.GetVariableBySemantic(semantic);
			if (!variable.IsValid && throwIfNotFound)
				throw new ArgumentException(string.Format("Can't retrieve variable with given semantic {0}.", semantic), "semantic");
			return variable;
		}

		private static EffectVariable RetrieveVariableByName(Effect effect, string name, bool throwIfNotFound)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			Debug.Assert(effect != null);

			EffectVariable variable = effect.GetVariableByName(name);
			if (!variable.IsValid && throwIfNotFound)
				throw new ArgumentException(string.Format("Can't retrieve variable with given name {0}.", name), "name");
			return variable;
		}

		#endregion

		#region Set effect variable by semantic

		public static void SetVariableBySemantic(this Effect effect, string semantic, Matrix value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(effect, semantic, throwIfNotFound);
			if (variable != null && variable.AsMatrix() != null)
				variable.AsMatrix().SetMatrix(value);
		}

		public static void SetVariableBySemantic(this Effect effect, string semantic, Vector4 value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(effect, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsVector() != null)
				variable.AsVector().Set(value);
		}

		public static void SetVariableBySemantic(this Effect effect, string semantic, float value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(effect, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableBySemantic(this Effect effect, string semantic, int value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(effect, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableBySemantic(this Effect effect, string semantic, ShaderResourceView value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(effect, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsResource() != null)
				variable.AsResource().SetResource(value);
		}

		#endregion

		#region Set effect variable by name

		public static void SetVariableByName(this Effect effect, string name, Matrix value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(effect, name, throwIfNotFound);
			if (variable != null && variable.AsMatrix() != null)
				variable.AsMatrix().SetMatrix(value);
		}

		public static void SetVariableByName(this Effect effect, string name, Vector4 value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(effect, name, throwIfNotFound);
			if (variable.IsValid && variable.AsVector() != null)
				variable.AsVector().Set(value);
		}

		public static void SetVariableByName(this Effect effect, string name, float value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(effect, name, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableByName(this Effect effect, string name, int value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(effect, name, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableByName(this Effect effect, string name, ShaderResourceView value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(effect, name, throwIfNotFound);
			if (variable.IsValid && variable.AsResource() != null)
				variable.AsResource().SetResource(value);
		}
		
		#endregion

		#region Retrieve constant buffer variable variable

		private static EffectVariable RetrieveVariableBySemantic(EffectConstantBuffer constantBuffer, string semantic, bool throwIfNotFound)
		{
			if (constantBuffer == null)
				throw new ArgumentNullException("constantBuffer");
			if (String.IsNullOrEmpty(semantic))
				throw new ArgumentNullException("semantic");

			EffectVariable variable = constantBuffer.GetMemberBySemantic(semantic);
			if (!variable.IsValid && throwIfNotFound)
				throw new ArgumentException(string.Format("Can't retrieve variable with given semantic {0}.", semantic), "semantic");
			return variable;
		}

		private static EffectVariable RetrieveVariableByName(EffectConstantBuffer constantBuffer, string name, bool throwIfNotFound)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (constantBuffer == null)
				throw new ArgumentNullException("constantBuffer");

			EffectVariable variable = constantBuffer.GetMemberByName(name);
			if (!variable.IsValid && throwIfNotFound)
				throw new ArgumentException(string.Format("Can't retrieve variable with given name {0}.", name), "name");
			return variable;
		}

		#endregion

		#region Set constant buffer variable by semantic

		public static void SetVariableBySemantic(this EffectConstantBuffer constantBuffer, string semantic, Matrix value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(constantBuffer, semantic, throwIfNotFound);
			if (variable != null && variable.AsMatrix() != null)
				variable.AsMatrix().SetMatrix(value);
		}

		public static void SetVariableBySemantic(this EffectConstantBuffer constantBuffer, string semantic, Vector4 value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(constantBuffer, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsVector() != null)
				variable.AsVector().Set(value);
		}

		public static void SetVariableBySemantic(this EffectConstantBuffer constantBuffer, string semantic, float value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(constantBuffer, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableBySemantic(this EffectConstantBuffer constantBuffer, string semantic, int value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(constantBuffer, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableBySemantic(this EffectConstantBuffer constantBuffer, string semantic, ShaderResourceView value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableBySemantic(constantBuffer, semantic, throwIfNotFound);
			if (variable.IsValid && variable.AsResource() != null)
				variable.AsResource().SetResource(value);
		}

		#endregion

		#region Set constant buffer variable by name

		public static void SetVariableByName(this EffectConstantBuffer constantBuffer, string name, Matrix value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(constantBuffer, name, throwIfNotFound);
			if (variable != null && variable.AsMatrix() != null)
				variable.AsMatrix().SetMatrix(value);
		}

		public static void SetVariableByName(this EffectConstantBuffer constantBuffer, string name, Vector4 value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(constantBuffer, name, throwIfNotFound);
			if (variable.IsValid && variable.AsVector() != null)
				variable.AsVector().Set(value);
		}

		public static void SetVariableByName(this EffectConstantBuffer constantBuffer, string name, float value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(constantBuffer, name, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableByName(this EffectConstantBuffer constantBuffer, string name, int value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(constantBuffer, name, throwIfNotFound);
			if (variable.IsValid && variable.AsScalar() != null)
				variable.AsScalar().Set(value);
		}

		public static void SetVariableByName(this EffectConstantBuffer constantBuffer, string name, ShaderResourceView value, bool throwIfNotFound)
		{
			var variable = RetrieveVariableByName(constantBuffer, name, throwIfNotFound);
			if (variable.IsValid && variable.AsResource() != null)
				variable.AsResource().SetResource(value);
		}

		#endregion
	}
}