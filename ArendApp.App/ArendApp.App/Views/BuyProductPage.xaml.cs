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
    public partial class BuyProductPage : ContentPage
    {
        public ICommand BuyCommand { get; }
        public string CalculatedPrice { get => _calculatedPrice; 
            set 
            {
                _calculatedPrice = value;
                OnPropertyChanged(nameof(CalculatedPrice));
            } 
        }
        private string _calculatedPrice = "";

        
        public int LabelGridColumn
        {
            get => _labelGridColumn;
            set
            {
                _labelGridColumn = value - 1;
                OnPropertyChanged(nameof(LabelGridColumn));
            }
        }
        private int _labelGridColumn  = 0;


        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }
        private Product _product;

        public IApiService _apiService => DependencyService.Get<IApiService>();
        public IDataStorage DataStorage => DependencyService.Get<IDataStorage>();
        public BuyProductPage(Product product)
        {
            Product = product;
            InitializeComponent();
            BuyCommand = new Command(async () =>
            {
                DateTime endperiod = DateTime.Now;
                switch ((int)PeriodSlider.Value)
                {
                    case 1:
                        endperiod.AddDays(1);
                        break;
                    case 2:
                        endperiod.AddDays(3);
                        break;
                    case 3:
                        endperiod.AddDays(7);
                        break;
                    case 4:
                        endperiod.AddDays(14);
                        break;
                    case 5:
                        endperiod.AddDays(30);
                        break;

                }
                await _apiService.BuyProduct(_product.Id, endperiod);
                await DisplayAlert("Арендавано!", "Вы успешно арендавали аккумулятор", "Ок");
                App.Current.MainPage = new AppShell();
            });
            CalculatedPrice = $"Цена за аренду на один день: {_product.OncePrice} в день\nИтого: {_product.OncePrice + _product.Deposit}";
            this.BindingContext = this;
        }



        private Dictionary<int, string> intToPeriodString = new Dictionary<int, string>()
        {
            {1, "один день" },
            {2, "три дня" },
            {3, "неделю" },
            {4, "две недели" },
            {5, "месяц" },

        };


        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var slider = (Slider)sender;
            if (slider == null) return;

            var newValue = Math.Round(e.NewValue);

            slider.Value = newValue;
            var value = intToPeriodString[(int)slider.Value];

            
            var sliderValue = (int)slider.Value;
            switch (sliderValue)
            {
                case 1:
                    CalculatedPrice = $"Цена за аренду на {value}: {_product.OncePrice} в день\nИтого: {_product.OncePrice + _product.Deposit}";
                    break;
                case 2:
                    CalculatedPrice = $"Цена за аренду на {value}: {_product.ThreeDayPrice} в день\nИтого: {_product.ThreeDayPrice * 3 + _product.Deposit}";
                    break;
                case 3:
                    CalculatedPrice = $"Цена за аренду на {value}: {_product.SevenDayPrice} в день\nИтого: {_product.SevenDayPrice * 7 + _product.Deposit}";
                    break;
                case 4:
                    CalculatedPrice = $"Цена за аренду на {value}: {_product.TwoWeekPrice} в день\nИтого: {_product.TwoWeekPrice * 14 + _product.Deposit}";
                    break;
                case 5:
                    CalculatedPrice = $"Цена за аренду на  {value}: {_product.MonthPrice} в день\nИтого: {_product.MonthPrice * 30 + _product.Deposit}";
                    break;
            }
        }
    }
}