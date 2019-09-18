using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApi
{
    public interface IConferenceService
    {
        Task<JObject> GetSpeakersAndSessionsAsync(string speakerName = null, string dayno = null, string keyword = null);
        Task<JObject> GetSessionByIdAsync(int id);
    }
}
