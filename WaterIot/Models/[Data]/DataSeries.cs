using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WaterIot.Models
{
	/// <summary xml:lang="zh-TW">
	/// 資料序列
	/// </summary>
	[Serializable]
	[DataContract]
	public abstract class DataSeries
	{
		/// <summary xml:lang="zh-TW">
		/// 物理量Id
		/// </summary>
		[DataMember(Order = 0)]
		public Guid Id;

		/// <summary xml:lang="zh-TW">
		/// 資料序列描述
		/// </summary>
		[DataMember(Order = 1)]
		public string Description;
	}

	/// <summary xml:lang="zh-TW">
	/// 時間不等距資料序列
	/// </summary>
	[Serializable]
	[DataContract]
	public class DataSeriesUs : DataSeries
	{
		/// <summary xml:lang="zh-TW">
		/// 資料點序列
		/// </summary>
		[DataMember(Order = 2)]
		public DataPoint[] DataPoints;
	}

	/// <summary xml:lang="zh-TW">
	/// 時間等距資料序列
	/// </summary>
	[Serializable]
	[DataContract]
	public class DataSeriesEs : DataSeries
	{
		/// <summary xml:lang="zh-TW">
		/// 資料序列所使用彙總運算方法
		/// </summary>
		[DataMember(Order = 2)]
		public Enum.AggregateCalculationMethods AggregateCalculationMethod;

		/// <summary xml:lang="zh-TW">
		/// 資料序列起始時間標記
		/// </summary>
		[DataMember(Order = 3)]
		public DateTimeOffset StartTimeStamp;

		/// <summary xml:lang="zh-TW">
		/// 資料點時間間格，單位:秒
		/// </summary>
		[DataMember(Order = 4)]
		public int TimeInterval;

		/// <summary xml:lang="zh-TW">
		/// 資料點數值
		/// </summary>
		[DataMember(Order = 5)]
		[JsonConverter(typeof(ValueTypeArrayConverter))]
		public ValueType[] Values;

		public DataPoint[] ToDataPointArray(bool ignoreNaN)
		{
			if (Values == null)
			{
				return null;
			}

			if (Values.Length == 0)
			{
				return null;
			}

			if (ignoreNaN) //忽略數值為NaN
			{
				List<DataPoint> results = new List<DataPoint>(Values.Length);
				for (int index = 0; index < Values.Length; index++)
				{
					if (!double.IsNaN((double)Values[index]))
					{
						results.Add(new DataPoint()
						{
							Id = Id,
							TimeStamp = StartTimeStamp.AddSeconds(index * TimeInterval),
							Value = Values[index]
						});
					}
				}
				return results.ToArray();
			}
			else
			{
				DataPoint[] dps = new DataPoint[Values.Length];
				for (int index = 0; index < dps.Length; index++)
				{
					dps[index] = new DataPoint()
					{
						Id = Id,
						TimeStamp = StartTimeStamp.AddSeconds(index * TimeInterval),
						Value = Values[index]
					};
				}
				return dps;
			}
		}
	}

	public class ValueTypeArrayConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			IEnumerable array = (IEnumerable)value;
			writer.WriteStartArray();
			foreach (object item in array)
			{
				if (item is double)
				{
					if (double.IsNaN((double)item))
					{
						serializer.Serialize(writer, "NaN");
					}
					else
					{
						serializer.Serialize(writer, (double)item);
					}
				}

				if (item is int)
				{
					serializer.Serialize(writer, (int)item);
				}
			}
			writer.WriteEndArray();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			List<ValueType> result = new List<ValueType>();
			JArray array = JArray.Load(reader);

			foreach (JValue item in array)
			{
				if (item.Value is string)
				{
					result.Add(double.NaN);
				}
				else
				{
					result.Add((ValueType)item.Value);
				}
			}
			return result.ToArray();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ValueType[]);
		}
	}
}