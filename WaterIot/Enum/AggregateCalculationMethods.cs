namespace WaterIot.Enum
{
	/// <summary>
	/// 彙總運算方法
	/// Aggregate Calculation Methods
	/// </summary>
	public enum AggregateCalculationMethods
	{
		/// <summary>
		/// Do not use aggregate calculation
		/// </summary>
		None = 0,

		/// <summary>
		/// Average
		/// </summary>
		avg = 1,

		/// <summary>
		/// Summation
		/// </summary>

		sum = 2,

		/// <summary>
		/// Integration
		/// </summary>
		Integration = 3,

		/// <summary>
		/// Maximum
		/// </summary>
		max = 4,

		/// <summary>
		/// Minimum
		/// </summary>
		min = 5,

		/// <summary>
		/// Counts
		/// </summary>
		count = 6,

		/// <summary>
		/// The first data point int each interval
		/// </summary>
		first = 7,

		/// <summary>
		/// The last data point in each interval
		/// </summary>
		last = 8,

		/// <summary>
		/// Marks gaps in data according to sampling rate with a null data point.
		/// </summary>
		gaps = 9,

		/// <summary>
		/// Returns two points for the range which represent the best fit line through the set of points.
		/// </summary>
		least_squares = 10,

		/// <summary>
		/// Finds the percentile of the data range. Calculates a probability distribution and returns the specified percentile for the distribution.
		/// </summary>
		percentile = 11,

		/// <summary>
		/// Computes standard deviation
		/// </summary>
		dev = 12,

		/// <summary>
		/// Computes the difference between successive data points.
		/// </summary>
		diff = 13
	}
}