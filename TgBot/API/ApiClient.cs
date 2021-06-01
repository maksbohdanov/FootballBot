using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TgBot
{
    public static class ApiClient
    {
        public static string Base = $"https://footballapl.azurewebsites.net/api";

        public static HttpClient _client { get; set; }

        public static void InitializeClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(AppSettings.Url);
        }
    }
}
