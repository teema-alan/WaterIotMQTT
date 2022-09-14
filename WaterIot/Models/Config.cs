using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterIot.Enum;

namespace WaterIot.Models
{
	public class Config
	{
		/// <summary>
		/// 資料來源 帳號
		/// </summary>
		public string ID { get; set; }

		/// <summary>
		/// 資料來源 密碼
		/// </summary>
		public string Password { get; set; }

		public string clientId { get; set; }
		public string clientSecret { get; set; }

		/// <summary>
		/// 自動下載、上傳
		/// </summary>
		public bool AutoMode { get; set; } = false;

		public bool FakeData { get; set; } = false;

		public string NotifyMessage { get; set; } = "";
		/// <summary>
		/// 自動間隔 (ms)
		/// </summary>
		public int UploadInterval { get; set; } = 600;

		/// <summary>
		/// 淹水間隔 (ms)
		/// </summary>
		public int FloodingUploadInterval { get; set; } = 300;
		public UploadType UploadType { get; set; } = UploadType.API;
		public string Topic { get; set; } = "TimeSeriesData/Write";

		/// <summary>
		/// 設備
		/// </summary>
		public List<Device> Devices { get; set; }
		
	}
}