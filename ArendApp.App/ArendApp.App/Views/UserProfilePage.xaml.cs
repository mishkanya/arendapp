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
    public partial class UserProfilePage : ContentPage
    {
        public ICommand LogoutCommand { get; }
        private IDataStorage _dataStorage => DependencyService.Get<IDataStorage>();
        public UserProfilePage()
        {
            InitializeComponent();
            InitializeComponent();
            LogoutCommand = new Command(async () =>
            {
                await _dataStorage.SetToken("");
                App.User = null;
                App.Current.MainPage = new AppShell();
            });

            if (App.User == null)
            {
                this.Content = new NoAutorizeView();
            }
            this.BindingContext = this;
        }
    }
}