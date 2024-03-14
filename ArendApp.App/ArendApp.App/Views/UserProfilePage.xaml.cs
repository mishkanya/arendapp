using ArendApp.App.Models;
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
        public User UserData
        {
            get => _user; 
            set
            {
                _user = value;
                OnPropertyChanged(nameof(UserData));
            }
        }

        public bool MayChangeUser 
        {
            get => _mayChangeUser;
            set
            {
                _mayChangeUser = value;
                OnPropertyChanged(nameof(MayChangeUser));
            }
        }
        private bool _mayChangeUser = false;

        private User _user;
        public ICommand LogoutCommand { get; }
        public ICommand ResetUserDataCommand { get; }
        public ICommand EnableChangeUserCommand { get; }
        public ICommand SaveUserDataCommand { get; }
        private IDataStorage _dataStorage => DependencyService.Get<IDataStorage>();
        private IApiService _apiService => DependencyService.Get<IApiService>();
        public UserProfilePage()
        {
            InitializeComponent();
            LogoutCommand = new Command(async () =>
            {
                await _dataStorage.SetToken("");
                App.User = null;
                App.Current.MainPage = new AppShell();
            });
            ResetUserDataCommand = new Command(async () =>
            {
                UserData = App.User.Clone() as User;
            });
            EnableChangeUserCommand = new Command(async () =>
            {
                MayChangeUser = !MayChangeUser;
            });
            SaveUserDataCommand = new Command(async () =>
            {
                var response = await _apiService.ChangeUser(UserData);
                if (response.IsSuccessful)
                    await DisplayAlert("Успех", "Данные успешно сохранены", "Ок");
                else

                    UserData = App.User.Clone() as User;
            });
            if (App.User == null)
            {
                this.Content = new NoAutorizeView();
            }
            else
            {
                UserData = App.User.Clone() as User;
            }
            this.BindingContext = this;


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}