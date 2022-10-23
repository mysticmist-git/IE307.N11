using DoAn_IE307_N11.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace DoAn_IE307_N11.Views
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