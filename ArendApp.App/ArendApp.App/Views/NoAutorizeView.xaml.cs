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
    public partial class NoAutorizeView : ContentView
    {
        public ICommand OpenLoginPage { get; }
        public NoAutorizeView()
        {
            InitializeComponent();
            OpenLoginPage = new Command(() =>
            {
                Navigation.PushModalAsync(new LoginPage());
            });
            this.BindingContext = this;
        }
    }
}