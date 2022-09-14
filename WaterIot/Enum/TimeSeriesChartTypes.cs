using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterIot.Enum
{
	/// <summary>
	/// 時間序列趨勢圖型態
	/// </summary>
	[Flags]
	public enum TimeSeriesChartTypes
	{
		/// <summary>
		/// SeriesChartTypeLine
		/// </summary>
		Line = 1,

		/// <summary>
		/// SeriesChartTypeColumn
		/// </summary>
		Bar = 3
	}
}