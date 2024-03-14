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
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace ArendApp.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        public Product Product { get; set; }
        public ObservableCollection<string> Images { get; set; }
        public ICommand AddToBasketCommand { get; }

        public IApiService _apiService => DependencyService.Get<IApiService>();
        public IDataStorage DataStorage => DependencyService.Get<IDataStorage>();
        public ProductPage(Product product)
        {
            InitializeComponent();
            Product = product;
            AddToBasketCommand = new Command(async () =>
            {
                var userBasket = new UserBasket() { ProductId = product.Id, UsedId = App.User.Id };
                await _apiService.AddToBasket(userBasket);
            });
            List<string> images = new List<string>() { product.MainImage };
            if(string.IsNullOrWhiteSpace(product.SecondImages) == false )
            {
                images.AddRange(product.SecondImages.Split('|'));
            }

            Images = new ObservableCollection<string>(images);
            this.BindingContext = this;
        }
    }
}