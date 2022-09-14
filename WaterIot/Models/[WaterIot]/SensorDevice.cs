using Newtonsoft.Json;

namespace WaterIot.Models
{
	public class SensorDevice
	{
		[JsonProperty(PropertyName = "device")]
		public int DeviceID { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "C_name")]
		public string CName { get; set; }

		[JsonProperty(PropertyName = "latitude")]
		public double Longitude { get; set; }

		[JsonProperty(PropertyName = "longitude")]
		public double Latitude { get; set; }
	}
}