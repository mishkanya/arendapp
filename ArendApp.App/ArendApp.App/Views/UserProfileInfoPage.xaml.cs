using ArendApp.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArendApp.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfileInfoPage : ContentPage
    {
        public ICommand LogoutCommand { get; set; }
        private IApiService _apiService = DependencyService.Get<IApiService>();
        private IDataStorage _dataStorage => DependencyService.Get<IDataStorage>();
        public UserProfileInfoPage()
        {
            InitializeComponent();
            LogoutCommand = new Command(async () =>
            {
                await _dataStorage.SetToken("");
                App.User = null;
            });

            if(App.User == null)
            {
                this.Content = new NoAutorizeView();
            }
            this.BindingContext = this;
        }
    }
}