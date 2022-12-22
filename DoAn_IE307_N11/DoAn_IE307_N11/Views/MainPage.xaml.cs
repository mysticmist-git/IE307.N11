using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.Views.All;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new AppViewModel();
        }

        #region Home Page

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTransactionPage());
        }

        #endregion

        #region Transaction Page

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            this.CustomTabsView.ScrollTo(e.CurrentItem, null, ScrollToPosition.Center, true);
        }

        private async void OpenTransactionDetail(object sender, EventArgs e)
        {
            var transaction = (sender as StackLayout).BindingContext as TransactionViewModel;

            await Shell.Current.GoToAsync($"{nameof(TransactionDetailPage)}?transactionId={transaction.Transaction.Id}");
        }

        #endregion

        #region Account
        async private void SignOut_Clicked(object sender, EventArgs e)
        {
            var viewModel = this.BindingContext as AppViewModel;
            viewModel.AccountViewModel.IsBusy = true;

            // Delete account
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Account>();

            // Delete local data
            var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();
            localData.WalletId = 0;
            await DependencyService.Get<SQLiteDBAsync>().DB.UpdateAsync(localData);

            Application.Current.MainPage = new LoginPage();

            viewModel.AccountViewModel.IsBusy = false;
        }

        #endregion

        private async void MiddleButton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTransactionPage());
        }

        async protected override void OnAppearing()
        {
            var viewModel = (this.BindingContext as AppViewModel);

            // Load default data for wallet creation
            //if (!viewModel.CanLoadMore)
            //    return;

            // This will get wallet info
            var result = await viewModel.GETData();

            //viewModel.CanLoadMore = false;

            await HandleGETResult(result);

            // Scroll to end of transaction tab
            //var tabCount = (this.BindingContext as AppViewModel).TransactionPageViewModel.TabVms.Count;
            //this.CustomTabsView.ScrollTo(tabCount - 1, null, ScrollToPosition.MakeVisible, true);
            //CustomTabsView.ScrollTo(viewModel.TransactionPageViewModel.TabVms.Last(), null, ScrollToPosition.End, true);
        }

        async private Task HandleGETResult(CommonResult result)
        {
            switch (result)
            {
                case Utils.CommonResult.Ok:
                    //await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case Utils.CommonResult.Fail:
                    await DisplayAlert("Lỗi", "Load data thất bại", "Ok");
                    break;
                case Utils.CommonResult.NoInternet:
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
            }
        }

        async private void WalletAreaTapped(object sender, EventArgs e)
        {
            var parent = (this.BindingContext as AppViewModel).TransactionPageViewModel;

            await Navigation.PushAsync(new ChooseWalletPage(parent, Enums.ForType.ForTransactionTab));
        }

        async private void ViewAllWallet_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}