using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConferenceApi.Common
{
    public class CustomAzureHttpClient : ICustomAzureHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomAzureHttpClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(DemoConferenceHelper.DemoConferenceBaseUrl);
            _httpClient.DefaultRequestHeaders.Add(DemoConferenceHelper.OcpApimKey, DemoConferenceHelper.SubscriptionKey);
        }

        public async Task<JObject> GetAllSessions(string speakerName = null, string dayno = null, string keyword = null)
        {
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request parameters
                queryString["speakername"] = speakerName;
                queryString["dayno"] = dayno;
                queryString["keyword"] = keyword;


                var response = await _httpClient.GetAsync(DemoConferenceHelper.DemoConferenceAllSessionPart + queryString);
                return await GetJsonObject(response);
        }

        public async Task<JObject> GetAllSpeakers(string speakerName = null, string dayno = null)
        {
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                queryString["dayno"] = dayno;
                queryString["speakername"] = speakerName;

                var uri = string.Concat( DemoConferenceHelper.DemoConferenceAllSpeakersPart+ queryString);


                var response = await _httpClient.GetAsync(uri);
                return await GetJsonObject(response);
        }

        public async Task<HttpResponseMessage> GetSessionById(int id)
        {
                var response = await _httpClient.GetAsync(DemoConferenceHelper.DemoConferenceSessionByIdPart + id);
                return response;
        }


        private async Task<JObject> GetJsonObject(HttpResponseMessage response)
        {
            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }
            return JsonConvert.DeserializeObject<JObject>(json);
        }
    }
}
