using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConferenceApi.Common
{
    public interface ICustomAzureHttpClient
    {
        Task<JObject> GetAllSessions(string speakerName = null, string dayno = null, string keyword = null);
        Task<JObject> GetAllSpeakers(string speakerName = null, string dayno = null);
        Task<HttpResponseMessage> GetSessionById(int id);
    }
}