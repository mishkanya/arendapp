using ArendApp.App.ViewModels;
using ArendApp.App.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ArendApp.App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProductsListPage), typeof(ProductsListPage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
            Routing.RegisterRoute(nameof(BasketPage), typeof(BasketPage));
        }

    }
}
