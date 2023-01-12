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
    public partial class AddTransactionPage : ContentPage
    {
        public AddTransactionPage()
        {
            InitializeComponent();

            this.BindingContext = new TransactionViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            date.MaximumDate = DateTime.Now.Date;

            var viewModel = this.BindingContext as TransactionViewModel;
            if (viewModel.CanLoadMore)
                await viewModel.GetWalletForCreateTransaction();
        }

        async private void AmountAreaTapped(object sender, EventArgs e)
        {
            (sender as StackLayout).IsEnabled = false;
            await Navigation.PushAsync(new SimpleEnterAmountPage(this.BindingContext, Enums.ForType.ForAddTransaction));
            (sender as StackLayout).IsEnabled = true;
        }

        async private void TypeAreaTapped(object sender, EventArgs e)
        {
            (sender as StackLayout).IsEnabled = false;
            await Navigation.PushAsync(new ChooseTransactionTypePage(this.BindingContext));
            (sender as StackLayout).IsEnabled = true;
        }

        async private void NoteAreaTapped(object sender, EventArgs e)
        {
            (sender as StackLayout).IsEnabled = false;
            await Navigation.PushAsync(new NotePage(this.BindingContext));
            (sender as StackLayout).IsEnabled = true;
        }

        async private void WalletAreaTapped(object sender, EventArgs e)
        {
            (sender as StackLayout).IsEnabled = false;

            await Navigation.PushAsync(new ChooseWalletPage(this.BindingContext, Enums.ForType.ForAddTransaction));

            (sender as StackLayout).IsEnabled = true;
        }

        async private void EventAreaTapped(object sender, EventArgs e)
        {
            (sender as StackLayout).IsEnabled = false;

            await Navigation.PushAsync(new ChooseEventPage(this.BindingContext, Enums.ForType.ForAddTransaction));

            (sender as StackLayout).IsEnabled = true;
        }

        private void DateAreaTapped(object sender, EventArgs e)
        {
            date.Focus();
        }

        private void date_DateSelected(object sender, DateChangedEventArgs e)
        {
            var viewModel = (this.BindingContext as TransactionViewModel);
            viewModel.PublicOnPropertyChanged(nameof(viewModel.DateDisplayer));
        }

        async private void SaveTransation_Click(object sender, EventArgs e)
        {
            var viewModel = (this.BindingContext as TransactionViewModel);
            var result = await viewModel.CreateTransaction();

            switch (result)
            {
                case TransactionPOSTResult.Ok:
                    await Navigation.PopAsync();
                    break;
                case TransactionPOSTResult.ZeroAmount:
                    await DisplayAlert("Thông báo", "Vui lòng nhập số tiền", "Ok");
                    break;
                case TransactionPOSTResult.NoType:
                    await DisplayAlert("Thông báo", "Vui lòng chọn nhóm giao dịch", "Ok");
                    break;
                case TransactionPOSTResult.Fail:
                    await DisplayAlert("Thông báo", "Thêm giao dịch thất bại", "Ok");
                    break;
                case TransactionPOSTResult.NoInternet:
                    await DisplayAlert("Thông báo", "Lỗi mạng", "Ok");
                    break;
            }
        }

    }
}