using ArendApp.App.Models;
using ArendApp.App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArendApp.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketPage : ContentPage
    {
        public ObservableCollection<Product> Items { get; set; }
        IApiService _apiService = DependencyService.Get<IApiService>();
        public ICommand RefreshingCommand { get; }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }
        private bool _isRefreshing;

        public ICommand DeleteFromBasketCommand { get; }
        public BasketPage()
        {
            InitializeComponent();
            Items = new ObservableCollection<Product>();
            RefreshingCommand = new Command(async () => await OnPageStart());
            DeleteFromBasketCommand = new Command( async (id) => 
            {
                await _apiService.DeleteFromBasket(id.ToString());
                await OnPageStart();
            });
            this.BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await OnPageStart();
        }

        private async Task OnPageStart()
        {
            if (App.User == null)
            {
                this.Content = new NoAutorizeView();
                return;
            }

            var userBasket = await _apiService.GetBasket();
            var products = await _apiService.GetProducts(userBasket.Data.Select(x => x.ProductId));

            Items.Clear();
            products.Data.ForEach((ptoduct) => Items.Add(ptoduct));
            IsRefreshing = false;
        }
        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var productPage = new ProductPage((Product)e.Item);
            await Shell.Current.Navigation.PushAsync(productPage);
            ((ListView)sender).SelectedItem = null;
        }
    }
}