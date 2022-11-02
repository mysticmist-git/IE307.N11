using DoAn_IE307_N11.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionBookPage : ContentPage
    {
        public TransactionBookPage()
        {
            InitializeComponent();

            MockBindingContext();

            this.CustomTabsView.ScrollTo(carouselView.CurrentItem, null, ScrollToPosition.Center, true);
        }

        private void MockBindingContext()
        {
            this.BindingContext = new TransactionsViewModel();
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            this.CustomTabsView.ScrollTo(e.CurrentItem, null, ScrollToPosition.Center, true);
        }
    }
}