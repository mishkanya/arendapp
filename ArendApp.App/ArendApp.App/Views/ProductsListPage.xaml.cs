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
using System.Windows.Input;
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
        public IDataStorage DataStorage => DependencyService.Get<IDataStorage>();


        public ICommand RefreshingCommand { get;}
        public bool IsRefreshing { 
            get => _isRefreshing; 
            set 
            {
                _isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            } }
        private bool _isRefreshing;


        public ProductsListPage()
        {
            InitializeComponent();
            Items = new ObservableCollection<Product>();
            RefreshingCommand = new Command( async() =>
            {
                var t = await ApiService.GetProducts();
                if (t.StatusCode != System.Net.HttpStatusCode.OK)
                    return;
                Items.Clear();
                t.Data.ForEach((ptoduct) => Items.Add(ptoduct));
                IsRefreshing = false;
            });
            RefreshingCommand.Execute(null);

            BindingContext = this;
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
