using ArendApp.App.Models;
using ArendApp.App.Services;
using ArendApp.App.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArendApp.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }


        public string RegisterEmail { get; set; }
        public string RegisterPassword { get; set; }
        public string RegisterPasswordRepeat { get; set; }

        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }

        private IApiService _apiService = DependencyService.Get<IApiService>();
        private IDataStorage _dataStorage => DependencyService.Get<IDataStorage>();

        #region FrameViewController
        public ICommand SwitchFrame { get; }
        public bool ViewLoginFrame
        {
            get => _viewLoginFrame;
            set
            {
                _viewLoginFrame = value;
                OnPropertyChanged("ViewLoginFrame");
                OnPropertyChanged("ViewRegisterFrame");
            }
        }
        public bool ViewRegisterFrame
        {
            get => !_viewLoginFrame;
            set
            {
                _viewLoginFrame = value;
                OnPropertyChanged("ViewLoginFrame");
                OnPropertyChanged("ViewRegisterFrame");
            }
        }
        private bool _viewLoginFrame = true;
        #endregion
        public LoginPage()
        {
            InitializeComponent();

            RegisterCommand = new Command(async () =>
            {
                if (RegisterPassword != RegisterPasswordRepeat)
                {
                    await DisplayAlert("Ошибка", "Пароли должны совпадать", "Ок");
                    return;
                }
                var user = new User()
                {
                    Name = "",
                    Email = RegisterEmail,
                    Password = RegisterPassword,
                };
                var response =  await _apiService.RegisterUser(user);

                if(response.IsSuccessful == false) {
                    await DisplayAlert("Ошибка авторизации", "Неверный логин или пароль", "Ок");
                    return;
                }

                await _dataStorage.SetToken(response.Data.Token);
                if (response.Data.Confirmed == false)
                {
                    await Navigation.PopModalAsync();
                    App.Current.MainPage = new CodeConfirmPage();
                }
                else
                {
                    App.Current.MainPage = new AppShell();
                }
            });
            LoginCommand = new Command(async () =>
            {
                var user = new User()
                {
                    Email = LoginEmail,
                    Password = LoginPassword,
                };
                var response = await _apiService.LoginUser(user);

                if (response.IsSuccessful == false)
                {
                    await DisplayAlert("Ошибка авторизации", "Неверный логин или пароль", "Ок");
                    return;
                }

                await _dataStorage.SetToken(response.Data.Token);
                if (response.Data.Confirmed == false)
                {
                    await Navigation.PopModalAsync();
                    App.Current.MainPage = new CodeConfirmPage();
                }
                else
                {
                    App.Current.MainPage = new AppShell();
                }
            });
            SwitchFrame = new Command( () => ViewLoginFrame = !ViewLoginFrame);

            this.BindingContext = this;
        }
    }
}