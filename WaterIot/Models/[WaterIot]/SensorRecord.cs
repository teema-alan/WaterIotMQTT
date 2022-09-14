using Newtonsoft.Json;
using System;

namespace WaterIot.Models
{
	public class SensorRecord
	{
		[JsonProperty(PropertyName = "create")]
		public DateTime Create { get; set; }

		[JsonProperty(PropertyName = "water")]
		public double Water
		{ 
			get; 
			set; 
		}

		//[JsonProperty(PropertyName = "velocity")]
		//public double Velocity { get; set; }
	}
}