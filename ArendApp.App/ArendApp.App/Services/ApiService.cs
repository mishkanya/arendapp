using ArendApp.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ArendApp.App.Services
{

    public interface IApiService
    {
        Task<IEnumerable<Product>> GetProducts();
    }
    public class ApiService : IApiService
    {
        public static string ServerUrl { get => _serverUrl != null ? _serverUrl : GetServerUrl(); }

        private static string GetServerUrl()
        {
            try
            {
                var requestUrl = "https://api.telegra.ph/getPage/ServerUrl-03-07?return_content=true";
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(requestUrl);
                    var response = httpClient.GetStringAsync(requestUrl).Result;

                    response = response.Trim().Replace(Environment.NewLine, "");
                    var regex = new Regex("(?<=\"children\":\\[\")(.*)(?=\"\\])");
                    var match = regex.Match(response);
                    var matchString = match.ToString().Replace("\\/", "/") + "/";
                    _serverUrl = matchString;
                    return matchString;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private static string _serverUrl;

        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public async Task<IEnumerable<Product>> GetProducts()
        {
            string requestUrl = $"{ServerUrl}api/Products";
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ServerUrl);
                var response = await httpClient.GetAsync(requestUrl);
                if(response.IsSuccessStatusCode == false)
                    return new List<Product>();
                var content = await response.Content.ReadAsStringAsync();

                var products =  JsonSerializer.Deserialize<IEnumerable<Product>>(content, jsonSerializerOptions);

                return products;
            }
        }
    }
}
