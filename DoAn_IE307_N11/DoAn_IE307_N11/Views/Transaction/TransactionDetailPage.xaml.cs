using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    public partial class TransactionDetailPage : ContentPage
    {
        public TransactionDetailPage(TransactionViewModel transaction)
        {
            InitializeComponent();

            this.BindingContext = transaction;
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

        //async private void WalletAreaTapped(object sender, EventArgs e)
        //{
        //    (sender as StackLayout).IsEnabled = false;

        //    await Navigation.PushAsync(new ChooseWalletPage(this.BindingContext, Enums.ForType.ForAddTransaction));

        //    (sender as StackLayout).IsEnabled = true;
        //}

        private void DateAreaTapped(object sender, EventArgs e)
        {
            date.Focus();
        }

        private void date_DateSelected(object sender, DateChangedEventArgs e)
        {
            var viewModel = (this.BindingContext as TransactionViewModel);
            viewModel.PublicOnPropertyChanged(nameof(viewModel.DateDisplayer));
        }

        /// <summary>
        /// Save changes to current transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void SaveTransation_Click(object sender, EventArgs e)
        {
            // Get current viewmodel
            var viewModel = this.BindingContext as TransactionViewModel;

            // Update some fields in transaction that can't be binding
            viewModel.Transaction.TypeId = viewModel.Type.Id;

            // Get api service singleton
            var apiService = DependencyService.Get<ApiService>();

            // Proceed to delete transaction
            var isSuccess = await apiService.UpdateTransaction(viewModel.Transaction);

            if (isSuccess)
            {
                await DisplayAlert("Thông báo", "Cập nhật giao dịch thành công.", "Ok");
            }
            else
            {
                await DisplayAlert("Thông báo", "Cập nhật giao dịch thất bại.", "Ok");
            }
            
            //switch (result)
            //{
            //    case TransactionPOSTResult.Ok:
            //        await Navigation.PopAsync();
            //        break;
            //    case TransactionPOSTResult.ZeroAmount:
            //        await DisplayAlert("Thông báo", "Vui lòng nhập số tiền", "Ok");
            //        break;
            //    case TransactionPOSTResult.NoType:
            //        await DisplayAlert("Thông báo", "Vui lòng chọn nhóm giao dịch", "Ok");
            //        break;
            //    case TransactionPOSTResult.Fail:
            //        await DisplayAlert("Thông báo", "Thêm giao dịch thất bại", "Ok");
            //        break;
            //    case TransactionPOSTResult.NoInternet:
            //        await DisplayAlert("Thông báo", "Lỗi mạng", "Ok");
            //        break;
            //}
        }

        /// <summary>
        /// Delete current transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void DeleteTransation_Click(object sender, EventArgs e)
        {
            // Ask again to make sure user want to delete the transaction
            var isAgreeToDelete = await DisplayAlert("Thông báo", "Bạn có chắc muốn xoá giao dịch này?", "Có", "Không");

            if (!isAgreeToDelete) // The user don't want to delete
                return;

            // Get current viewmodel
            var viewModel = this.BindingContext as TransactionViewModel;

            // Get current transaction id
            var transactionId = -1;
            transactionId = viewModel.Transaction.Id;

            // Get api service singleton
            var apiService = DependencyService.Get<ApiService>();

            // Proceed to delete transaction
            var isSuccess = await apiService.DeleteTransaction(transactionId);

            if (isSuccess)
            {
                await DisplayAlert("Thông báo", "Đã xoá giao dịch thành công.", "Ok");
                await Navigation.PopAsync();
                return;
            }

            await DisplayAlert("Thông báo", "Xoá giao dịch thất bại.", "Ok");
        }

        async private void EventAreaTapped(object sender, EventArgs e)
        {
            (sender as StackLayout).IsEnabled = false;

            await Navigation.PushAsync(new ChooseEventPage(this.BindingContext, Enums.ForType.ForEditWallet));

            (sender as StackLayout).IsEnabled = true;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            // Get current viewmodel
            var viewModel = this.BindingContext as TransactionViewModel;

            // Load event name
            var apiService = DependencyService.Get<ApiService>();
            viewModel.Event = await apiService.GetEventByIdAsync(viewModel.Transaction.EventId);
        }
    }
}