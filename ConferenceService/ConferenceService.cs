using ConferenceServiceLibs.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ConferenceServiceLibs
{
    public class ConferenceService : IConferenceService
    {
        public async Task<JObject> GetSessionByIdAsync(int id)
        {
            var result = await GetSessionById(id);
            var sessionData = JObject.FromObject(new
            {
                SessionDetail = await result.Content.ReadAsStringAsync()
            });
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var sessions = await GetAllSessions();
                string filter = $"/session/{id}";
                List<JToken> linkHrefs = sessions["collection"]["items"].Children().ToList();
                JToken element = null;
                foreach (var hrefItem in linkHrefs)
                {
                    if (hrefItem["href"].ToString().Contains(filter))
                    {
                        element = hrefItem;
                        break;
                    }
                }
                JObject resultJson = JObject.Parse(element.ToString());
                sessionData.Merge(resultJson, new JsonMergeSettings()
                {
                    MergeArrayHandling = MergeArrayHandling.Union
                }
               );
            }
            return sessionData;
        }

        public async Task<JObject> GetSpeakersAndSessionsAsync(string speakerName = null, string dayno = null, string keyword = null)
        {
            var sessions = GetAllSessions(speakerName, dayno, keyword);
            var speakers = GetAllSpeakers(speakerName, dayno);

            await Task.WhenAll(sessions, speakers);

            speakers.Result.Merge(sessions.Result, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Concat
            });

            return speakers.Result;

        }

        private async Task<JObject> GetAllSessions(string speakerName = null, string dayno = null, string keyword = null)
        {
            using (var client = new HttpClient())
            {
                // Request headers add ocp-apim key
                client.DefaultRequestHeaders.Add(DemoConferenceHelper.OcpApimKey, DemoConferenceHelper.SubscriptionKey);

                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request parameters
                queryString["speakername"] = speakerName;
                queryString["dayno"] = dayno;
                queryString["keyword"] = keyword;

                var uri = string.Concat(DemoConferenceHelper.DemoConferenceBaseUrl, DemoConferenceHelper.DemoConferenceAllSessionPart, queryString);

                var response = await client.GetAsync(uri);
                return await GetJsonObject(response);
            }

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

        private async Task<JObject> GetAllSpeakers(string speakerName = null, string dayno = null)
        {
            using (var client = new HttpClient())
            {
                // Request headers add ocp-apim key
                client.DefaultRequestHeaders.Add(DemoConferenceHelper.OcpApimKey, DemoConferenceHelper.SubscriptionKey);

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                // Request parameters

                queryString["dayno"] = dayno;
                queryString["speakername"] = speakerName;

                var uri = string.Concat(DemoConferenceHelper.DemoConferenceBaseUrl, DemoConferenceHelper.DemoConferenceAllSpeakersPart, queryString);


                var response = await client.GetAsync(uri);
                return await GetJsonObject(response);
            }

        }

        private async Task<HttpResponseMessage> GetSessionById(int id)
        {
            using (var client = new HttpClient())

            {
                // Request headers add ocp-apim key
                client.DefaultRequestHeaders.Add(DemoConferenceHelper.OcpApimKey, DemoConferenceHelper.SubscriptionKey);

                var uri = string.Concat(DemoConferenceHelper.DemoConferenceBaseUrl, DemoConferenceHelper.DemoConferenceSessionByIdPart, id);
                var response = await client.GetAsync(uri);
                return response;
            }
        }
    }
}
