using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels;
using System;
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

            this.BindingContext = new AppViewModel(this);
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

        private async void AddNewTransactionPage_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NewTransactionPage));
        }

        private async void OpenTransactionDetail(object sender, EventArgs e)
        {
            var transaction = (sender as StackLayout).BindingContext as TransactionViewModel;

            await Shell.Current.GoToAsync($"{nameof(TransactionDetailPage)}?transactionId={transaction.Transaction.Id}");
        }

        #endregion

        private void MiddleButton_Tapped(object sender, EventArgs e)
        {

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

            await Navigation.PushAsync(new ChooseWalletPage(parent));
        }
    }
}