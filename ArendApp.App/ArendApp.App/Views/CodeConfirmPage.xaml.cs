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
    public partial class CodeConfirmPage : ContentPage
    {
        public ICommand CodeConfirmCommand { get; set; }
        public string Code { get; set; }

        private IApiService _apiService = DependencyService.Get<IApiService>();
        private IDataStorage _dataStorage => DependencyService.Get<IDataStorage>();
        public CodeConfirmPage()
        {
            InitializeComponent();


            CodeConfirmCommand = new Command(async () =>
            {
                var user = await _apiService.ConfirmCode(Code);
                if(user.StatusCode == System.Net.HttpStatusCode.OK) 
                { 
                   if( user.Data.Confirmed == true)
                    {
                        App.Current.MainPage = new AppShell();
                    }
                }
            });
            this.BindingContext = this;
        }
    }
}