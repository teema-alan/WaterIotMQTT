using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WaterIot.OthersServices
{
	public class OAuth2Client
	{
		private Uri baseUri;
		private Uri redirectUri;
		private AccessToken cunAccessToken;

		public class AccessToken
		{
			public string Access_token { get; set; }
			public long Expires_in { get; set; }
			public string Refresh_token { get; set; }
			public string Token_type { get; set; }
		}

		#region Public Methods

		/// <summary>
		/// 取得或設定 Client Id
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		/// 取得或設定 Client Secret
		/// </summary>
		public string ClientSecret { get; set; }

		/// <summary>
		/// 目前 Token 失效時間
		/// </summary>
		public DateTime CurrentTokenExipreTime { get; private set; } = DateTime.Now;

		/// <summary>
		/// OAuth2 Authentication Code
		/// </summary>
		/// <param name="baseUri">for senslink 3.0, http://{root}/v3/oauth2 </param>
		public OAuth2Client(Uri baseUri, Uri redirectUri, string clientId, string clientSecret)
		{
			this.baseUri = baseUri;
			ClientId = clientId;
			ClientSecret = clientSecret;
			this.redirectUri = redirectUri;
		}

		/// <summary>
		/// OAuth2 Client Credential
		/// </summary>
		/// <param name="baseUri">for senslink 3.0, http://{root}/v3/oauth2 </param>
		public OAuth2Client(Uri baseUri, string clientId, string clientSecret)
		{
			this.baseUri = baseUri;
			ClientId = clientId;
			ClientSecret = clientSecret;
		}

		/// <summary>
		/// 取得Token，使用Authorization Code方法，使用登入認證後取得的Code，配合其他參數取得Token
		/// </summary>
		/// <param name="code"></param>
		/// <param name="redirect_uri"></param>
		/// <param name="clientId"></param>
		/// <param name="clientSecret"></param>
		/// <returns></returns>
		public AccessToken GetAccessToken(string code)
		{
			HttpClient httpClient = new HttpClient
			{
				Timeout = new TimeSpan(0, 0, 60),
				BaseAddress = baseUri
			};

			FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("code", code),
				new KeyValuePair<string, string>("redirect_uri", redirectUri.ToString()),
				new KeyValuePair<string, string>("client_id", ClientId),
				new KeyValuePair<string, string>("client_secret", ClientSecret)
			});

			Task<HttpResponseMessage> postTask = httpClient.PostAsync("token", content);
			postTask.Wait(5000);

			if (postTask.IsCompleted && !postTask.IsFaulted && !postTask.IsCanceled)
			{
				HttpResponseMessage response = postTask.Result;

				Task<string> readTask = response.Content.ReadAsStringAsync();
				readTask.Wait(5000);

				if (readTask.IsCompleted && !readTask.IsCanceled && !readTask.IsFaulted)
				{
					string responseContent = readTask.Result;
					AccessToken tokenResponse = null;
					try
					{
						tokenResponse = JsonConvert.DeserializeObject<AccessToken>(responseContent);
					}
					catch { }
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						return tokenResponse;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// 使用 clientId 及 clientSecret 取得 Token
		/// </summary>
		/// <param name="foreceUpdate">是否強迫重新取得 Token，若為false，則Token失效時會自動取得</param>
		/// <returns></returns>
		public AccessToken GetAccessToken(bool forceUpdate)
		{
			if (CurrentTokenExipreTime.AddMinutes(-1) < DateTime.Now || forceUpdate)
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = baseUri; //http://{root}/v3/oauth2

					// We want the response to be JSON.
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					// Build up the data to POST.
					List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>("grant_type", "client_credentials"),
						new KeyValuePair<string, string>("client_id", ClientId),
						new KeyValuePair<string, string>("client_secret", ClientSecret)
					};

					FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
					Task<HttpResponseMessage> postTask = client.PostAsync("token", content);
					postTask.Wait(5000);

					if (postTask.IsCompleted && !postTask.IsFaulted && !postTask.IsCanceled)
					{
						HttpResponseMessage message = postTask.Result;
						if (message.IsSuccessStatusCode)
						{
							Task<string> readTask = message.Content.ReadAsStringAsync();
							readTask.Wait(5000);
							if (readTask.IsCompleted && !postTask.IsFaulted && !postTask.IsCanceled)
							{
								cunAccessToken = null;
								try
								{
									cunAccessToken = JsonConvert.DeserializeObject<AccessToken>(readTask.Result);
									CurrentTokenExipreTime = DateTime.Now.AddSeconds(cunAccessToken.Expires_in);
								}
								catch { }
								return cunAccessToken;
							}
						}
					}
					return null;
				}
			}
			else
			{
				return cunAccessToken;
			}
		}

		public AccessToken RefreshAccessToken(string refreshToken)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = baseUri; //http://{root}/v3/oauth2

				// We want the response to be JSON.
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				// Build up the data to POST.
				List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("grant_type", "refresh_token"),
					new KeyValuePair<string, string>("refresh_token", refreshToken),
                    //new KeyValuePair<string, string>("scope", "")
                };

				FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

				Task<HttpResponseMessage> response = client.PostAsync("token", content);
				response.Wait(5000);

				if (response.IsCompleted)
				{
					HttpResponseMessage message = response.Result;
					if (message.IsSuccessStatusCode)
					{
						Task<string> responseContent = message.Content.ReadAsStringAsync();
						responseContent.Wait(5000);
						if (responseContent.IsCompleted)
						{
							AccessToken responseData = JsonConvert.DeserializeObject<AccessToken>(responseContent.Result);
							return responseData;
						}
					}
				}
				return null;
			}
		}

		#endregion Public Methods
	}
}