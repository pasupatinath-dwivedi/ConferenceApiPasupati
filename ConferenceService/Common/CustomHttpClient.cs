using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ConferenceServiceLibs.Common
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

    }
}
