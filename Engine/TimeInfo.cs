namespace RomantiqueX.Engine
{
	public struct TimeInfo
	{
		public double Elapsed { get; set; }

		public double Total { get; set; }

		public TimeInfo(double elapsedTime, double totalTime) : this()
		{
			this.Elapsed = elapsedTime;
			this.Total = totalTime;
		}
	}
}