using DoAn_IE307_N11.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        }
        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            this.CustomTabsView.ScrollTo(e.CurrentItem, null, ScrollToPosition.Center, true);
        }

        private async void AddNewTransactionPage_Click(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync(nameof(NewTransactionPage));
        }

        private async void OpenTransactionDetail(object sender, EventArgs e)
        {
            var transaction = (sender as StackLayout).BindingContext as TransactionViewModel;

            await Shell.Current.GoToAsync($"{nameof(TransactionDetailPage)}?transactionId={transaction.Transaction.Id}");
        }
    }
}