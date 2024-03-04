using ArendApp.App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArendApp.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsListPage : ContentPage
    {
        public ObservableCollection<Product> Items { get; set; }

        public ProductsListPage()
        {
            InitializeComponent();
            Items = new ObservableCollection<Product>();
            
            MyListView.ItemsSource = Items;
            Task.Run(LoadList);

        }
        async void LoadList()
        {
            Items.Clear();
            var t = new List<Product>
            {
                new Product (){ Description = "Автомобильный аккумулятор MyWay 60 Ач прямая полярность L2 в Туле", MonthPrice=180, Name = "Автомобильный аккумулятор MyWay 60", MainImage ="https://cdn2.kolesa-darom.ru/api/v1/image/bucket/iblock/c32/c3263885182e2281a66f55d2f2e04eb4.jpg?width=600&height=600&quality=80", SecondImages = "https://i.pinimg.com/564x/2a/b2/64/2ab26454f987aaa46d4ffea1272d8d65.jpg | https://i.pinimg.com/564x/90/1a/11/901a111eb0cc834f44615d6e72c23b32.jpg"},
                new Product (){ Description = "Автомобильный аккумулятор MyWay 60 Ач прямая полярность L2 в Туле", MonthPrice=180, Name = "Автомобильный аккумулятор MyWay 60", MainImage ="https://cdn2.kolesa-darom.ru/api/v1/image/bucket/iblock/c32/c3263885182e2281a66f55d2f2e04eb4.jpg?width=600&height=600&quality=80", SecondImages = "https://i.pinimg.com/564x/2a/b2/64/2ab26454f987aaa46d4ffea1272d8d65.jpg | https://i.pinimg.com/564x/90/1a/11/901a111eb0cc834f44615d6e72c23b32.jpg"},

                new Product (){ Description = "Автомобильный аккумулятор MyWay 60 Ач прямая полярность L2 в Туле", MonthPrice=180, Name = "Автомобильный аккумулятор MyWay 60", MainImage ="https://cdn2.kolesa-darom.ru/api/v1/image/bucket/iblock/c32/c3263885182e2281a66f55d2f2e04eb4.jpg?width=600&height=600&quality=80", SecondImages = "https://i.pinimg.com/564x/2a/b2/64/2ab26454f987aaa46d4ffea1272d8d65.jpg | https://i.pinimg.com/564x/90/1a/11/901a111eb0cc834f44615d6e72c23b32.jpg"},

                new Product (){ Description = "Автомобильный аккумулятор MyWay 60 Ач прямая полярность L2 в Туле", MonthPrice=180, Name = "Автомобильный аккумулятор MyWay 60", MainImage ="https://cdn2.kolesa-darom.ru/api/v1/image/bucket/iblock/c32/c3263885182e2281a66f55d2f2e04eb4.jpg?width=600&height=600&quality=80", SecondImages = "https://i.pinimg.com/564x/2a/b2/64/2ab26454f987aaa46d4ffea1272d8d65.jpg | https://i.pinimg.com/564x/90/1a/11/901a111eb0cc834f44615d6e72c23b32.jpg"},


            };
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
