using System;
using System.Collections.Generic;
using WaterIot.Enum;

namespace WaterIot.Models
{
	public class PhysicalQuantityInfo : InfoBase
	{
		/// <summary>
		/// [必要] 監測物理量 Id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// [必要] 監測物理量名稱 maxLength:100
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// [非必要] 完整名稱,如果沒有設定則給予Name的值
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// [非必要] json format 資訊
		/// </summary>
		public string JsonProerties { get; set; }

		/// <summary>
		/// [非必要] 監測物理量描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// [非必要] 物理量單位
		/// </summary>
		public string SIUnit { get; set; }

		/// <summary>
		/// [必要] 是否啟用
		/// </summary>
		public bool IsEnable { get; set; }

		/// <summary>
		/// [必要] 顯示方式，例如位數/經緯度/角度等
		/// </summary>
		public DisplayFormats DisplayFormat { get; set; }

		/// <summary>
		/// [必要] 擷取歷史資料時統計方法，原 StatisticalMethods
		/// </summary>
		public AggregateCalculationMethods AggregateCalculationMethod { get; set; }

		/// <summary>
		/// [必要] 時間序列資料圖表類型
		/// </summary>
		public TimeSeriesChartTypes TimeSeriesChartType { get; set; }

		/// <summary>
		/// [必要] 紀錄類型為全部紀錄、僅紀錄歷史資料、僅紀錄即時資料或不紀錄
		/// </summary>
		public RecordingTimeStampMethods RecordingTimeStampMethod { get; set; }

		/// <summary>
		/// [必要] 資料型態，只有兩種型態，分別為 Double 與 Boolean
		/// </summary>
		public TypeCode PhysicalQuantityDataType { get; set; }

		/// <summary>
		/// [必要] 收到資料後，寫入資料庫的最大時距，單位為Sec，若為SensMateSeries，則此參數無效，改為使用
		/// SensMateSeries的RecordInterval參數
		/// </summary>
		public int MaxDatabaseRecordInterval { get; set; }

		/// <summary>
		/// [非必要] 所對應 RdaqDevice 中的 Channel, FK: ChannelInfo Id
		/// </summary>
		public Guid? Channel_Id { get; set; }

		/// <summary>
		/// [非必要]
		/// </summary>
		public IEnumerable<CategoryInfo> LinkedCategoryInfos { get; set; }

		/// <summary>
		/// [非必要]
		/// </summary>
		public IEnumerable<OneVariableFormulaInfo> LinkedOneVariableFormulaInfos { get; set; }
	}
}