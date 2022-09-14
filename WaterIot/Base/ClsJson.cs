using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WaterIot.Base
{
	public static class ClsJson
	{
		public static T LoadFromFile<T>(string file, bool isNullNewFile = false) where T : class, new()
		{
			if (File.Exists(file))
			{
				using (var reader = new StreamReader(file))
				{
					var fileContents = reader.ReadToEnd();
					var obj = JsonToClass<T>(fileContents);
					reader.Close();
					reader.Dispose();
					return obj;
				}
			}
			else
			{
				if (isNullNewFile)
				{
					T newObj = new T();
					SaveToFile(newObj, file);
					return newObj;
				}
				else
				{
					return null;
				}
			}
		}

		public static Newtonsoft.Json.Linq.JObject LoadFromFile(string file)
		{
			if (File.Exists(file))
			{
				using (var reader = new StreamReader(file, Encoding.UTF8))
				{
					var fileContents = reader.ReadToEnd();
					var obj = JsonToClass(fileContents);
					reader.Close();
					reader.Dispose();
					if (obj is Newtonsoft.Json.Linq.JObject jobj)
					{
						return jobj;
					}
				}
			}
			return null;
		}

		public static void SaveToFile<T>(T obj, string file)
		{
			string text = ClassToJson(obj, true);
			File.WriteAllText(file, text);
		}

		public static string ClassToJson<T>(T input, bool newLine = false)
		{
			string jsonText;
			if (newLine)
			{
				jsonText = JsonConvert.SerializeObject(input, Newtonsoft.Json.Formatting.Indented);
			}
			else
			{
				jsonText = JsonConvert.SerializeObject(input);
			}
			return jsonText;
		}

		public static T JsonToClass<T>(string jsonText)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			};
			T responseObj = JsonConvert.DeserializeObject<T>(jsonText, jsonSerializerSettings);
			return responseObj;
		}

		public static object JsonToClass(string jsonText)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			};
			object responseObj = JsonConvert.DeserializeObject(jsonText, jsonSerializerSettings);
			//T responseObj = JsonConvert.DeserializeObject<T>(jsonText, jsonSerializerSettings);
			return responseObj;
		}
	}
}
