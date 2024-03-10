using ArendApp.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using Xamarin.Forms;
using System.Net.Http.Json;

namespace ArendApp.App.Services
{

    public interface IApiService
    {
        Task<ServerResponse<List<Product>>> GetProducts();
        Task<ServerResponse<User>> RegisterUser(User user);
        Task<ServerResponse<User>> LoginUser(User user);
        Task<ServerResponse<User>> ConfirmCode(string code);
        Task<ServerResponse<User>> GetUser();
    }
    public class ApiService : IApiService
    {

        private IDataStorage _dataStorage => DependencyService.Get<IDataStorage>();
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        #region ServerUrl
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
                    var matchString = match.ToString().Replace("\\/", "/");
                    if(matchString.EndsWith("/") == false)
                        matchString = matchString + "/";
                    _serverUrl = matchString;
                    return matchString;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string _serverUrl;
        #endregion

        public async Task<ServerResponse<List<Product>>> GetProducts() => await GetRequest<List<Product>>("api/Products", RequestMethod.Get);
        public async Task<ServerResponse<User>> RegisterUser(User user) 
        { 
           var userData = await GetRequest<User>("api/Users", RequestMethod.Post, user);
            if (userData.Data != null)
                App.User = userData.Data;
            return userData;
        }
        public async Task<ServerResponse<User>> LoginUser(User user)
        { 
           var userData = await GetRequest<User>("api/Users/Login", RequestMethod.Post, user);
            if (userData.Data != null)
                App.User = userData.Data;
            return userData;
        }
        public async Task<ServerResponse<User>> ConfirmCode(string code) => await GetRequest<User>($"api/Users/Confirm/{code}", RequestMethod.Get);
        public async Task<ServerResponse<User>> GetUser()
        { 
            
           var user =  await GetRequest<User>($"api/Users/GetByToken", RequestMethod.Get);
            if(user.Data != null)
                App.User = user.Data;
            return user;
        }

        private async Task<ServerResponse<T>> GetRequest<T>(string route, RequestMethod method, object body = null) where T : class, new()
        {
            string requestUrl = $"{ServerUrl}{route}";
            ServerResponse<T> responseData = new ServerResponse<T>();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ServerUrl);

                var userToken = await _dataStorage.GetToken();
                if (string.IsNullOrWhiteSpace(userToken) == false)
                    httpClient.DefaultRequestHeaders.Add("UserToken", userToken);

                HttpContent bodyContent = default;
                if (body != null)
                    bodyContent = JsonContent.Create(body); 

                HttpResponseMessage response = default;

                if(method == RequestMethod.Get)
                    response = await httpClient.GetAsync(requestUrl);
                else if (method == RequestMethod.Post)
                    response = await httpClient.PostAsync(requestUrl, bodyContent);
                else if (method == RequestMethod.Delete)
                    response = await httpClient.DeleteAsync(requestUrl);
                else if (method == RequestMethod.Put)
                    response = await httpClient.PutAsync(requestUrl, bodyContent);

                responseData.StatusCode = response.StatusCode;

                var content = await response.Content.ReadAsStringAsync();
                responseData.Message = content;

                if (content != null)
                {
                    var data = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);
                    responseData.Data = data;
                }

                return responseData;
            }
        }
        
    }

    public class ServerResponse<T> where T : class, new()
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

    }
    enum RequestMethod
    {
        Get,
        Post,
        Put,
        Delete
    }
}
