using System;
using System.Collections.Generic;

namespace WaterIot.Models
{
	/// <summary>
	/// 監測站資訊
	/// </summary>
	public class StationInfo : InfoBase
	{
		/// <summary>
		/// [必要] 監測站Id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// [必要] 監測站名稱
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// [非必要] 儲存監測站的 MetaData，對應 SensorThings 的 JsonProperty 欄位
		/// </summary>
		public string JsonProperties { get; set; }

		/// <summary>
		/// [非必要] 監測站代碼，使用者定義
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// [非必要] 監測站地址
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// [非必要] 監測站描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// [必要] 監測站WGS84座標，緯度
		/// </summary>
		/// <value>The latitude.</value>
		public double Latitude { get; set; }

		/// <summary>
		/// [必要] 監測站WGS84座標，經度
		/// </summary>
		/// <value>The longitude.</value>
		public double Longitude { get; set; }

		/// <summary>
		/// [必要] 高程
		/// </summary>
		/// <value>The altitude.</value>
		public double Altitude { get; set; }

		/// <summary>
		/// [必要] 是否啟用
		/// </summary>
		public bool IsEnable { get; set; }   //IsDisable

		/// <summary>
		/// [非必要] 此監測站連結哪些物理量
		/// </summary>
		/// <value>The linked physicalQuantity infos.</value>
		public IEnumerable<PhysicalQuantityInfo> LinkedPhysicalQuantities { get; set; }
	}
}