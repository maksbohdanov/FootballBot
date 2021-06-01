using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using TgBot.Models;

namespace TgBot.Commands
{
    public class ApiRepository
    {
        public static async Task<List<FixtureResponse>> GetFixtures(string url)
        {
            using (HttpResponseMessage response = await ApiClient._client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<List<FixtureResponse>>(content);                    
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<TeamStatisticsResponse> GetStatistics(string url)
        {
            using (HttpResponseMessage response = await ApiClient._client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<TeamStatisticsResponse>(content);
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<StandingsResponse> GetStandings(string url)
        {
            using (HttpResponseMessage response = await ApiClient._client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<StandingsResponse>(content);
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<List<FixtureResponse>> GetAllFavorite(string url)
        {
            using (HttpResponseMessage response = await ApiClient._client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<List<FixtureResponse>>(content);
                    return result;
                }
                else
                {
                    return null;
                    throw new Exception(response.ReasonPhrase);
                    
                }
            }
        }

        public static async Task AddToFavor(string url, string chatId, FixtureResponse fixture)
        {
            fixture.IdChat = chatId;

            var json = JsonConvert.SerializeObject(fixture);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await ApiClient._client.PostAsync($"{url}", data);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Succesfully added");
                await Program.client.SendTextMessageAsync(chatId, "Added to favorites", ParseMode.Default);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public static async Task DeleteFromFavor(string url, long id)
        {

            var response = ApiClient._client.DeleteAsync($"{url}").Result;

            

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Succesfully deleted");
                await Program.client.SendTextMessageAsync(id, "Deleted from favorites", ParseMode.Default);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
