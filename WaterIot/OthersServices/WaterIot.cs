using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WaterIot.Models;

namespace WaterIot.OthersServices
{
	public class WaterIot
	{
		/// <summary>
		/// Cookie
		/// </summary>
		private CookieContainer Cookie { get; set; }

		/// <summary>
		/// Accuunt
		/// </summary>
		public string Id { get; set; } = "ifem3";

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; } = "NT123";

		/// <summary>
		/// URL
		/// </summary>
		public string BaseUrl { get; set; } = "https://water-iot.ithinkr.com/~water-iot/index.php/";

		public WaterIot(string id, string password)
		{
			Id = id;
			Password = password;
		}

		/// <summary>
		/// 登入
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public bool Login(out string status)
		{
			Cookie = new CookieContainer();
			string response = GetResponse($"export/login/{Id}/{Password}", Cookie);
			dynamic tmp = JsonConvert.DeserializeObject(response);
			string _status = tmp.status?.Value;
			if (_status is "OK")
			{
				status = "OK";
				return true;
			}
			status = $"{_status ?? "Error"}";
			return false;
		}

		/// <summary>
		/// 登出
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public bool Logout(out string status)
		{
			string response = GetResponse($"export/logout");
			dynamic tmp = JsonConvert.DeserializeObject(response);
			string _status = tmp.status?.Value;
			if (_status is "OK")
			{
				status = "OK";
				Cookie = null;
				return true;
			}
			status = $"{_status ?? "Error"}";
			return false;
		}

		/// <summary>
		/// 下載設備資料
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public List<Project> ListSensor(out string status)
		{
			status = "Unknown";
			for (int i = 0; i < 2; i++)
			{
				string response = GetResponse($"export/list_sensor");
				dynamic tmp = JsonConvert.DeserializeObject(response);
				string _status = tmp.status?.Value;
				if (_status is "OK")
				{
					status = "OK";
					List<Project> _projects = tmp.projects?.ToObject<List<Project>>();
					return _projects;
				}
				else if (_status is "Unauthorized")
				{
					status = "Unauthorized";
					Login(out _);
					continue;
				}
				status = $"{_status ?? "Error"}";
				break;
			}
			return null;
		}

		/// <summary>
		/// 下載設備值
		/// </summary>
		/// <param name="status"></param>
		/// <param name="device"></param>
		/// <param name="begin"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public List<SensorRecord> QuerySensor(out string status, int device, DateTime begin, DateTime end)
		{
			status = "Unknown";

			for (int i = 0; i < 2; i++)
			{
				string response = GetResponse($"export/query_sensor/{device}/{begin:yyyyMMddHHmmss}/{end:yyyyMMddHHmmss}");
				dynamic tmp = JsonConvert.DeserializeObject(response);
				string _status = tmp.status?.Value;
				if (_status is "OK")
				{
					status = "OK";
					List<SensorRecord> _records = tmp.records?.ToObject<List<SensorRecord>>();
					return _records;
				}
				else if (_status is "Unauthorized")
				{
					status = "Unauthorized";
					Login(out _);
					continue;
				}
				status = $"{_status ?? "Error"}";
				break;
			}
			return null;
		}

		/// <summary>
		/// 下載設備值
		/// </summary>
		/// <param name="status"></param>
		/// <param name="device"></param>
		/// <returns></returns>
		public List<SensorRecord> QuerySensor(out string status, int device)
		{
			return QuerySensor(out status, device, DateTime.Now.AddDays(-1), DateTime.Now);
		}

		public string GetResponse(string url, CookieContainer cookie = null)	//取得
		{
			if (cookie is null)
			{
				cookie = Cookie;
			}
			//string url = "https://water-iot.ithinkr.com/~water-iot/index.php/";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUrl + url);
			request.Credentials = CredentialCache.DefaultCredentials;
			request.Method = "GET";
			request.ContentType = "text/html; charset=utf-8";
			request.CookieContainer = cookie;
			ServicePointManager.ServerCertificateValidationCallback = delegate
			{
				return true;
			};
			WebResponse response = request.GetResponse();
			//Console.WriteLine(((HttpWebResponse)response).StatusDescription);
			string responseFromServer = string.Empty;
			using (Stream dataStream = response.GetResponseStream())
			{
				StreamReader reader = new StreamReader(dataStream);
				responseFromServer = reader.ReadToEnd();
				//Console.WriteLine(responseFromServer);
			}
			response.Close();
			return responseFromServer;
		}

		public string LineNorifyPostResponse(string url, string message)    //取得
		{
			try
			{

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Headers.Add(HttpRequestHeader.Authorization, "Bearer 8468DOxmp82N2KCRQSZqbi9iY0SnCsqEXNBZzoUatbG");
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.Credentials = CredentialCache.DefaultCredentials;
				NameValueCollection postParams = new NameValueCollection();
				postParams.Add("message", message);
				string aa = "message=" + message;
				byte[] byteArray = Encoding.UTF8.GetBytes(aa);
				request.ContentLength = byteArray.Length;
				using (Stream reqStream = request.GetRequestStream())
				{
					reqStream.Write(byteArray, 0, byteArray.Length);
				}

				WebResponse response = request.GetResponse();

				string responseFromServer = string.Empty;
				using (Stream dataStream = response.GetResponseStream())
				{
					StreamReader reader = new StreamReader(dataStream);
					responseFromServer = reader.ReadToEnd();
				}
				response.Close();
				return responseFromServer;
			}
            catch (Exception ex)
            {
				return "";
            }
		}
	}
}