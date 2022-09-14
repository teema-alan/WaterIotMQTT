using System;

namespace WaterIot.Enum
{
	/// <summary>
	/// Senslink 3.0 使用的 Info 型態
	/// </summary>
	[Flags]
	public enum SenslinkInfoTypes : long
	{
		/// <summary>
		/// 忽略
		/// </summary>
		None = 0,

		/// <summary>
		/// 使用者，Senslink中每一個帳號均為一個User
		/// </summary>
		User = 1,

		/// <summary>
		/// 收發器，在 SensLink 2.0 中為 DB-[ManagementPlan]
		/// </summary>
		Transceiver = 1 << 1,

		/// <summary>
		/// 監測站分群，在 SensLink 2.0 中為 DB-[Theme]
		/// </summary>
		StationGroup = 1 << 2,

		/// <summary>
		/// 監測站
		/// </summary>
		Station = 1 << 3,

		/// <summary>
		/// 監測物理量，在 SensLink 2.0 中為 DB-[Measure]
		/// </summary>
		PhysicalQuantity = 1 << 4,

		/// <summary>
		/// 單變數公式
		/// </summary>
		OneVariableFormula = 1 << 5,

		/// <summary>
		/// 傳輸紀錄設備 (RdaqDevice) 頻道
		/// </summary>
		Channel = 1 << 6,

		/// <summary>
		/// 警戒值設定 在 SensLink 2.0 中為 DB-[Alarm]
		/// </summary>
		AlarmSetting = 1 << 7,

		/// <summary>
		/// 監測項目分類
		/// </summary>
		Category = 1 << 8,

		/// <summary>
		/// in SensLink 2.0 DB-[Mesh]
		/// </summary>
		Mesh = 1 << 9,

		/// <summary>
		/// 自動跳圖區域
		/// </summary>
		Region = 1 << 10,

		/// <summary>
		/// 攝影機型號
		/// </summary>
		VideoModel = 1 << 11,

		/// <summary>
		/// The video source
		/// </summary>
		VideoSource = 1 << 12,

		/// <summary>
		/// 即時監測看板
		/// </summary>
		MonitoringDashBoard = 1 << 13,

		/// <summary>
		/// 事件
		/// </summary>
		Event = 1 << 14,

		/// <summary>
		/// 警報發生紀錄
		/// </summary>
		AlarmRecord = 1 << 15,

		/// <summary>
		/// 警報發送紀錄
		/// </summary>
		AlarmSentRecord = 1 << 16,

		/// <summary>
		/// 警報訂閱
		/// </summary>
		AlarmSubscription = 1 << 17,

		AlarmContact = 1 << 18,

		TelecomBase = 1 << 19,

		/// <summary>
		/// 傳輸紀錄器，在 SensLink 中為 2.0 DB-[Logger]
		/// </summary>
		RdaqDevice = 1 << 20,

		/// <summary>
		/// 自動計算機器人
		/// </summary>
		CalculateRobot = 1 << 21,

		ExtraLink = 1 << 22
	}
}