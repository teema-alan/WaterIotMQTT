using System;

namespace WaterIot.Enum
{
	/// <summary>
	/// 資料紀錄分法
	/// </summary>
	[Flags]
	public enum RecordingTimeStampMethods
	{
		/// <summary>
		/// 不記錄(設備維護中或已拔除) (Senslink 2.0 / 3.0)
		/// </summary>
		DoNotRecord = 0,

		/// <summary>
		/// 使用 Physical Quantity 中的 Interval 設定，將資料寫入下一個Time Solt，例如，每10分鐘紀錄一筆資料
		/// Time Slot為每小時的0,10,20,30,40,50整分，若收到24分的資料，則寫入30分。
		/// Physical Quantity 中的 Interval 設定優先權高於 RdaqDevice 設定。
		/// </summary>
		MoveToNextTimeSlot = 1,

		/// <summary>
		/// 使用系統收到資料時的時間，作為寫入的時間
		/// </summary>
		UseReceivedTimeStamp = 2,

		/// <summary>
		/// 使用資料時間
		/// </summary>
		UseDataTimeStamp = 32,

		//---------------------Below Senslink 2.0 Only----

		/// <summary>
		/// 全部記錄 (Senslink 2.0)
		/// </summary>
		[Obsolete("Senslink 2.0")]
		All = 4,

		/// <summary>
		/// 僅記錄歷史資料 (Senslink 2.0)
		/// </summary>
		[Obsolete("Senslink 2.0")]
		HistoricalDataOnly = 8,

		/// <summary>
		/// 僅記錄即時資料 (Senslink 2.0)
		/// </summary>
		[Obsolete("Senslink 2.0")]
		RealTimeDataOnly = 16,
	}
}