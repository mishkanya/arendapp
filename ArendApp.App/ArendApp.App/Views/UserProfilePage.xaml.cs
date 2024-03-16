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
    public partial class UserProfilePage : ContentPage
    {
        public User UserData
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(UserData));
                OnPropertyChanged(nameof(UserIsNotNull));
            }
        }
        public bool UserIsNotNull => _user != null;
        public bool MayChangeUser
        {
            get => _mayChangeUser;
            set
            {
                _mayChangeUser = value;
                OnPropertyChanged(nameof(MayChangeUser));
                OnPropertyChanged(nameof(UserIsNotNull));
            }
        }
        private bool _mayChangeUser = false;

        private User _user;
        public ObservableCollection<Product> Inventory { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<Product> History { get; set; } = new ObservableCollection<Product>();

        #region Commands
        public ICommand LogoutCommand { get; }
        public ICommand ResetUserDataCommand { get; }
        public ICommand EnableChangeUserCommand { get; }
        public ICommand SaveUserDataCommand { get; }
        public ICommand OpenProductPageCommand { get; }


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
        #endregion

        private IDataStorage _dataStorage => DependencyService.Get<IDataStorage>();
        private IApiService _apiService => DependencyService.Get<IApiService>();
        public UserProfilePage()
        {
            InitializeComponent();
            LogoutCommand = new Command(async () =>
            {
                await _dataStorage.SetToken("");
                App.User = null;
                App.Current.MainPage = new AppShell();
            });
            ResetUserDataCommand = new Command(async () =>
            {
                UserData = App.User.Clone() as User;
            });
            EnableChangeUserCommand = new Command(async () =>
            {
                MayChangeUser = !MayChangeUser;
            });
            OpenProductPageCommand = new Command(async (object id) =>
            {
                var allProducts = new List<Product>();
                allProducts.AddRange(History);
                allProducts.AddRange(Inventory);

                var product = allProducts.FirstOrDefault(t => t.Id == (int)id);

                if (product == null) return;

                var productPage = new ProductPage(product);
                await Shell.Current.Navigation.PushAsync(productPage);
            });
            SaveUserDataCommand = new Command(async () =>
            {
                var response = await _apiService.ChangeUser(UserData);
                if (response.IsSuccessful)
                {
                    await DisplayAlert("Успех", "Данные успешно сохранены", "Ок");
                    MayChangeUser = false;

                }
                else
                {
                    await DisplayAlert("Ошибка", "Данные не сохранены", "Ок");
                    UserData = App.User.Clone() as User;
                }
            });
            RefreshingCommand = new Command(async () =>
            {
                await LoadPageData();
            });
            this.BindingContext = this;

        }
        private async Task LoadPageData()
        {
            if (App.User == null)
            {
                this.Content = new NoAutorizeView();
            }
            else
            {
                UserData = App.User.Clone() as User;
            }

            var allInventory = await _apiService.GetInventory();
            if (allInventory.IsSuccessful)
            {
                var inventory = allInventory.Data.Where(t => t.EndPeriod > DateTime.Now);
                var inventoryProducts = await _apiService.GetProducts(inventory.Select(x => x.ProductId));

                Inventory.Clear();
                inventoryProducts.Data.ForEach((ptoduct) => Inventory.Add(ptoduct));

                var histori = allInventory.Data.Where(t => t.EndPeriod < DateTime.Now);
                var historiProducts = await _apiService.GetProducts(histori.Select(x => x.ProductId));

                History.Clear();
                historiProducts.Data.ForEach((ptoduct) => History.Add(ptoduct));
            }


            IsRefreshing = false;
        }

        protected override async void OnAppearing()
        {
            await LoadPageData();
            base.OnAppearing();
        }
    }
}