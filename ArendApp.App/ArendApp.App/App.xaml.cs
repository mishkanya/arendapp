using ArendApp.App.Models;
using ArendApp.App.Services;
using ArendApp.App.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArendApp.App
{
    public partial class App : Application
    {

        public static User User { get; set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<ApiService>();
            DependencyService.Register<DataStorage>();

            var label = new Label() { Text = "Ожидание загрузки...", FontSize=20, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            var page = new ContentPage();
            page.Content = label;
            MainPage = page;
        }

        private async Task UserInit()
        {
            try
            {
                var dataStorage = DependencyService.Get<DataStorage>();
                var token = await dataStorage.GetToken();

                if (string.IsNullOrWhiteSpace(token))
                {
                    NavigationPage.SetHasNavigationBar(this, true);
                    Application.Current.MainPage = new AppShell();
                    return;
                }
                else
                {
                    var userData = await DependencyService.Get<IApiService>().GetUser();
                    if (userData.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        await dataStorage.SetToken("");
                        User = null;
                        NavigationPage.SetHasNavigationBar(this, true);
                        Application.Current.MainPage = new AppShell();
                        return;
                    }
                    else if (userData.Data?.Confirmed == true)
                    {
                        NavigationPage.SetHasNavigationBar(this, true);
                        Application.Current.MainPage = new AppShell();
                        return;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage = new CodeConfirmPage();
                        });
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                var label = new Label() { Text = "Упс, технические проблемы\nПопробуйте еще раз позже", TextColor=Color.Red, FontSize = 20, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                var page = new ContentPage();
                page.Content = label;
                MainPage = page;
            }
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await UserInit();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
