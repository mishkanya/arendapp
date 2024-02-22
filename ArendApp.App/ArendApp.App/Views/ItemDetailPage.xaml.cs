using ArendApp.App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ArendApp.App.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}