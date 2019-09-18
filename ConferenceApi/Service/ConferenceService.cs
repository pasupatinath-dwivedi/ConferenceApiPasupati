using ConferenceApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ConferenceApi
{
    public class ConferenceService : IConferenceService
    {
        public readonly ICustomAzureHttpClient _azureClient;
        public ConferenceService(ICustomAzureHttpClient azureClient)
        {
            _azureClient = azureClient;
        }

        public async Task<JObject> GetSessionByIdAsync(int id)
        {
            var result = await _azureClient.GetSessionById(id);
            var sessionData = JObject.FromObject(new
            {
                SessionDetail = await result.Content.ReadAsStringAsync()
            });
            // call only if there is a valid session detail available
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var sessions = await _azureClient.GetAllSessions();
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
            var sessions = _azureClient.GetAllSessions(speakerName, dayno, keyword);
            var speakers = _azureClient.GetAllSpeakers(speakerName, dayno);

            await Task.WhenAll(sessions, speakers);

            speakers.Result.Merge(sessions.Result, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Concat
            });

            return speakers.Result;

        }
        
    }
}
