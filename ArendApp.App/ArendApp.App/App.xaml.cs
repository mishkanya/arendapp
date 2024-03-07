using ArendApp.App.Services;
using ArendApp.App.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArendApp.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<ApiService>();

            MainPage = new AppShell();
            NavigationPage.SetHasNavigationBar(this, true);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
