using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WaterIot.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Device : Base.NotificationObject
	{
		private DateTime createTime;

		public Device(int deviceID)
		{
			DeviceID = deviceID;
			Records = new List<Record>();
		}

		/// <summary>
		/// 設備 Id
		/// </summary>
		[JsonProperty]
		public int DeviceID { get; set; }

		/// <summary>
		/// 設備 Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 設備 CName
		/// </summary>
		public string CName { get; set; }

		/// <summary>
		/// 設備 Longitude
		/// </summary>
		public double Longitude { get; set; }

		/// <summary>
		/// 設備 Latitude
		/// </summary>
		public double Latitude { get; set; }

		/// <summary>
		/// 設備 CreateTime
		/// </summary>
		public DateTime CreateTime
		{
			get => createTime;
			set
			{
				createTime = value;
				RaisePropertyChanged();
			}
		}

		[JsonProperty]
		public List<Record> Records { get; set; }
	}
}