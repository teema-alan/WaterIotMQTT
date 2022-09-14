using Newtonsoft.Json;
using System.Collections.Generic;

namespace WaterIot.Models
{
	public class Project
	{
		[JsonProperty(PropertyName = "project")]
		public int ProjectID { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "sensors")]
		public List<SensorDevice> Sensors { get; set; }

		[JsonProperty(PropertyName = "longitude")]
		public double Longitude { get; set; }

		[JsonProperty(PropertyName = "latitude")]
		public double Latitude { get; set; }

		[JsonProperty(PropertyName = "altitude")]
		public double Altitude { get; set; }
	}
}