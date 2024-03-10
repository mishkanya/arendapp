using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ArendApp.App.Services
{
    public class DataStorage : IDataStorage
    {
        const string UserTokenPropertieName = "UserToken";
        public DataStorage() { }
        public async Task<string> GetData()
        {
            return await SecureStorage.GetAsync("oauth_token");
        }
        public async Task SetData(string data)
        {

        }

        public async Task SetToken(string token)
        {
            await SecureStorage.SetAsync(UserTokenPropertieName, token);
        }
        public async Task<string> GetToken()
        {
           return await SecureStorage.GetAsync(UserTokenPropertieName);
        }
    }

    public interface IDataStorage
    {
        Task SetToken(string token);
        Task<string> GetToken();
    }
}
