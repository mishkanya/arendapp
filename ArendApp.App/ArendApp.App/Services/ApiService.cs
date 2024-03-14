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
using System.Linq;

namespace ArendApp.App.Services
{

    public interface IApiService
    {
        Task<ServerResponse<List<Product>>> GetProducts();
        Task<ServerResponse<User>> RegisterUser(User user);
        Task<ServerResponse<User>> LoginUser(User user);
        Task<ServerResponse<User>> ConfirmCode(string code);
        Task<ServerResponse<UserBasket>> AddToBasket(UserBasket userBasket);
        Task<ServerResponse<User>> GetUser();
        Task<ServerResponse<List<UserBasket>>> GetBasket();
        Task<ServerResponse<List<Product>>> GetProducts(IEnumerable<int> id);
        Task<ServerResponse<object>> DeleteFromBasket(string Id); Task<ServerResponse<object>> ChangeUser(User user);
    }

    class ApiRequestParams
    {
        //string route, RequestMethod method, object body = null
        public string Route { get; set; }
        public RequestMethod Method { get; set; }
        public object Body { get; set; }
        public UriData UriData { get; set; }

    }
    class UriData
    {
        public UriData(string dataName, List<object> data)
        {
            DataName = dataName;
            Data = data;
        }

        public string DataName { get; set; }
        public IEnumerable<object> Data { get; set; }
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
                    if (matchString.EndsWith("/") == false)
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



        public async Task<ServerResponse<object>> ChangeUser(User user)
        {
            int Id = user.Id;
            var apiRequestParams = new ApiRequestParams()
            {
                Method = RequestMethod.Put,
                Body = user,
                Route = $"api/Users/{Id}",
            };
            var response = await GetRequest<object>(apiRequestParams);
            if (response.IsSuccessful)
                await this.GetUser();
            return response;
        }
        public async Task<ServerResponse<UserBasket>> AddToBasket(UserBasket userBasket)
        {
            var apiRequestParams = new ApiRequestParams()
            {
                Method = RequestMethod.Post,
                Body = userBasket,
                Route = $"api/UsersBasket",
            };
            return await GetRequest<UserBasket>(apiRequestParams);
        }

        public async Task<ServerResponse<object>> DeleteFromBasket(string Id)
        {
            var apiRequestParams = new ApiRequestParams()
            {
                Method = RequestMethod.Delete,
                Body = null,
                Route = $"api/UsersBasket/ByProduct/{Id}",
            };
            return await GetRequest<object>(apiRequestParams);
        }
        public async Task<ServerResponse<List<Product>>> GetProducts() => await GetRequest<List<Product>>("api/Products", RequestMethod.Get);
        public async Task<ServerResponse<List<Product>>> GetProducts(IEnumerable<int> id)
        {
            var apiRequestParams = new ApiRequestParams()
            {
                Method = RequestMethod.Get,
                Body = null,
                Route = "api/Products/ById",
                UriData = new UriData("id", id.Cast<object>().ToList())
            };
            return await GetRequest<List<Product>>(apiRequestParams);
        }

        public async Task<ServerResponse<List<UserBasket>>> GetBasket() => await GetRequest<List<UserBasket>>("api/UsersBasket", RequestMethod.Get);
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

            var user = await GetRequest<User>($"api/Users/GetByToken", RequestMethod.Get);
            if (user.Data != null)
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

                if (method == RequestMethod.Get)
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
        private async Task<ServerResponse<T>> GetRequest<T>(ApiRequestParams apiRequestParams) where T : class, new()
        {
            string requestUrl = $"{ServerUrl}{apiRequestParams.Route}";
            if (apiRequestParams.UriData != null)
            {
                requestUrl += $"?{string.Join($"", apiRequestParams.UriData.Data.Select(t => $"{apiRequestParams.UriData.DataName}={t}"))}";
            }

            ServerResponse<T> responseData = new ServerResponse<T>();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ServerUrl);

                var userToken = await _dataStorage.GetToken();
                if (string.IsNullOrWhiteSpace(userToken) == false)
                    httpClient.DefaultRequestHeaders.Add("UserToken", userToken);

                HttpContent bodyContent = default;
                if (apiRequestParams.Body != null)
                    bodyContent = JsonContent.Create(apiRequestParams.Body);

                try
                {
                    HttpResponseMessage response = default;

                    if (apiRequestParams.Method == RequestMethod.Get)
                        response = await httpClient.GetAsync(requestUrl);
                    else if (apiRequestParams.Method == RequestMethod.Post)
                        response = await httpClient.PostAsync(requestUrl, bodyContent);
                    else if (apiRequestParams.Method == RequestMethod.Delete)
                        response = await httpClient.DeleteAsync(requestUrl);
                    else if (apiRequestParams.Method == RequestMethod.Put)
                        response = await httpClient.PutAsync(requestUrl, bodyContent);

                    responseData.StatusCode = response.StatusCode;

                    var content = await response.Content.ReadAsStringAsync();
                    responseData.Message = content;

                    if (content != null && string.IsNullOrEmpty(content) == false)
                    {
                        var data = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);
                        responseData.Data = data;
                    }
                }
                catch (Exception ex)
                {
                    responseData.ErrorMessage = ex.ToString();
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
        public string ErrorMessage { get; set; }
        public bool IsSuccessful { get => ErrorMessage == null; }

    }
    enum RequestMethod
    {
        Get,
        Post,
        Put,
        Delete
    }
}
