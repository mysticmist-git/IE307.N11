using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseWalletPage : ContentPage
    {
        public ForType Type { get; set; }
        public ChooseWalletPage(object parent, ForType type)
        {
            InitializeComponent();

            this.BindingContext = new ChooseWalletViewModel(parent, type);
            this.Type = type;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var result = await(this.BindingContext as ChooseWalletViewModel).GETData();

            switch (result)
            {
                case Utils.CommonResult.Ok:
                    //await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case Utils.CommonResult.Fail:
                    await DisplayAlert("Lỗi", "Load data thất bại", "Ok");
                    await Navigation.PopAsync();
                    break;
                case Utils.CommonResult.NoInternet:
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
            }

            if (result != Utils.CommonResult.Ok)
            {
                return;
            }
        }

        private async void WalletSelected(object sender, SelectedItemChangedEventArgs e)
        {
            switch (Type)
            {
                case ForType.ForTransactionTab:
                    {
                        // Get local data
                        var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();
                        var homeViewModel = ((this.BindingContext as ChooseWalletViewModel).ParentViewModel as TransactionPageViewModel)
                            .ParentViewModel.HomeViewModel;

                        homeViewModel.CurrentWallet = (e.SelectedItem as WalletViewModel).Wallet;
                        homeViewModel.WalletIconUrl = (e.SelectedItem as WalletViewModel).WalletImageUrl;
                        localData.WalletId = (e.SelectedItem as WalletViewModel).Wallet.Id;
                        await DependencyService.Get<SQLiteDBAsync>().DB.UpdateAsync(localData);
                    }
                    break;
                case ForType.ForAddTransaction:
                    {

                        //var homeViewModel = ((this.BindingContext as ChooseWalletViewModel).ParentViewModel as TransactionPageViewModel)
                        //    .ParentViewModel.HomeViewModel;

                        var transactionViewModel = ((this.BindingContext as ChooseWalletViewModel).ParentViewModel as TransactionViewModel);
                        transactionViewModel.Wallet= (e.SelectedItem as WalletViewModel).Wallet;
                    }
                    break;
                default:
                    break;
            }

            await Navigation.PopAsync();
        }

        private async void OnEdit_Clicked(object sender, EventArgs e)
        {
            var walletInfo = (sender as MenuItem).CommandParameter as WalletViewModel;

            var editWalletPage = new EditWalletPage(this.BindingContext as ChooseWalletViewModel, walletInfo);
            await (editWalletPage.BindingContext as EditWalletViewModel).LoadCurrency(walletInfo.Wallet.CurrencyId);

            await Navigation.PushAsync(editWalletPage);
            
        }

        private async void CreateWallet_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateWalletPage(Enums.ForType.ForCreateWalletFromChooseWallet));
        }
    }
}