using System;
using System.ComponentModel.Design;

namespace RomantiqueX.Engine
{
	[Serializable]
	public abstract class EngineComponent<T> : IDisposable
	{
		#region Fields

		private readonly IServiceProvider services;

		private bool isDisposed;

		#endregion

		#region Properties

		public IServiceProvider Services
		{
			get { return services; }
		}

		public bool IsDisposed
		{
			get { return isDisposed; }
		}

		#endregion

		#region Initialization

		protected EngineComponent(IServiceContainer services)
		{
			if (services == null)
				throw new ArgumentNullException("services");

			this.services = services;
			services.AddService(typeof(T), this);
		}

		#endregion

		#region Methods

		#region Public

		#region IDisposable Members

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			if (isDisposed)
				throw new ObjectDisposedException("Object has been already disposed.");

			Dispose(true);
			GC.SuppressFinalize(this);
			isDisposed = true;
		}

		#endregion

		public virtual void Update(TimeInfo timeInfo)
		{
		}

		#endregion

		#region Protected

		protected virtual void Dispose(bool disposing)
		{
		}

		#endregion

		#endregion
	}
}
