using Newtonsoft.Json;
using System;

namespace WaterIot.Models
{
	[JsonObject(MemberSerialization.OptOut)]
	public class Record : Base.NotificationObject
	{
		private DateTime _lastUploadTime;
		private object _value;
		private object _uploadValue;

		public Record(string name)
		{
			Name = name;
		}

		/// <summary>
		/// UUID
		/// </summary>
		public Guid Guid { get; set; }

		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 啟用
		/// </summary>
		public bool Upload { get; set; }

		/// <summary>
		/// 上傳時間
		/// </summary>
		public DateTime LastUploadTime
		{
			get => _lastUploadTime;
			set
			{
				_lastUploadTime = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 值
		/// </summary>
		[JsonIgnore]
		public object Value
		{
			get => _value;
			set
			{
				_value = value;
				RaisePropertyChanged();
			}
		}

		//[JsonIgnore]
		//public object UploadValue
		//{
		//	get => _uploadValue;
		//	set
		//	{
		//		_uploadValue = value;
		//		RaisePropertyChanged();
		//	}
		//}
		[JsonIgnore]
		public Device Parents { get; set; }
	}
}