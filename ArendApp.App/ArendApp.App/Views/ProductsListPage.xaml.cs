using ArendApp.App.Models;
using ArendApp.App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ArendApp.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsListPage : ContentPage
    {
        public ObservableCollection<Product> Items { get; set; }
        public IApiService ApiService => DependencyService.Get<IApiService>();

        public ProductsListPage()
        {
            InitializeComponent();
            Items = new ObservableCollection<Product>();
            
            MyListView.ItemsSource = Items;
            Task.Run(LoadList);

        }
        private async void LoadList()
        {
            Items.Clear();

            var t = await ApiService.GetProducts();
            //t.ForEach(x => Items.Add(x));
            t.AsParallel().ForAll((ptoduct) => Items.Add(ptoduct));
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var productPage =  new ProductPage((Product)e.Item);
            await Shell.Current.Navigation.PushAsync(productPage);
            ((ListView)sender).SelectedItem = null;
        }
    }
}
