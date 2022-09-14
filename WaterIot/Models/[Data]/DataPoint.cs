using Newtonsoft.Json;
using System;

namespace WaterIot.Models
{
	/// <summary>
	/// 資料點
	/// </summary>
	[Serializable]
	public struct DataPoint
	{
		/// <summary>
		/// Data Id, so as metric in time series database
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]  //在Web Service回傳的結果中，若為defult，則不顯示
		public Guid Id;

		/// <summary xml:lang="zh-TW">
		/// 資料時間標記
		/// </summary>
		public DateTimeOffset TimeStamp;

		/// <summary xml:lang="zh-TW">
		/// 資料點數值
		/// </summary>
		[JsonConverter(typeof(ValueTypeConverter))]
		public ValueType Value;
	}

	public class ValueTypeConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is float)
			{
				if (float.IsNaN((float)value))
				{
					serializer.Serialize(writer, "NaN");
				}
				else
				{
					serializer.Serialize(writer, Convert.ToDouble(value));
				}
			}
			else if (value is double)
			{
				if (double.IsNaN((double)value))
				{
					serializer.Serialize(writer, "NaN");
				}
				else
				{
					serializer.Serialize(writer, (double)value);
				}
			}
			else if (value is int)
			{
				serializer.Serialize(writer, (int)value);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value is string)
			{
				return double.NaN;
			}
			else
			{
				return (ValueType)reader.Value;
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ValueType[]);
		}
	}
}