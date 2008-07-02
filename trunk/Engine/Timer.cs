#region Using declarations

using System;
using System.ComponentModel.Design;

#endregion

namespace RomantiqueX.Engine
{
	public class Timer : EngineComponent<Timer>
	{
		#region Fields

		private long lastUpdateTime;
		private double totalTime;
		private bool started;

		private double lastFPSTime;
		private int frameNumber;

		#endregion

		#region Properties

		public TimeInfo Time { get; private set; }

		public int FramesPerSecond { get; private set; }

		#endregion

		#region Initialization

		public Timer(IServiceContainer services)
			: base(services)
		{
		}

		#endregion

		public void Start()
		{
			if (started)
				throw new InvalidOperationException("Timer was already started.");

			started = true;
			Reset();
		}

		public void Stop()
		{
			if (!started)
				throw new InvalidOperationException("Timer has not been started yet.");

			started = false;
		}

		public void Reset()
		{
			if (!started)
				throw new InvalidOperationException("Timer has not been started yet.");

			lastUpdateTime = Environment.TickCount;
			totalTime = 0;
			lastFPSTime = 0;
		}

		public override void Update(TimeInfo timeInfo)
		{
			var currentTime = Environment.TickCount;
			var elapsedTime = (double)(currentTime - lastUpdateTime) / 1000;
			totalTime += elapsedTime;
			lastUpdateTime = currentTime;

			frameNumber += 1;
			if (totalTime - lastFPSTime >= 1)
			{
				FramesPerSecond = frameNumber;
				frameNumber = 0;
				lastFPSTime = totalTime;
			}

			Time = new TimeInfo(elapsedTime, totalTime);
		}
	}
}