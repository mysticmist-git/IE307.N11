﻿using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.Views.All;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
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

            this.BindingContext = DependencyService.Get<AppViewModel>();
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

        //private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        //{
        //    this.CustomTabsView.ScrollTo(e.CurrentItem, null, ScrollToPosition.Center, true);
        //}

        private async void OpenTransactionDetail(object sender, EventArgs e)
        {
            var transaction = (sender as StackLayout).BindingContext as TransactionViewModel;
            transaction.Wallet = DependencyService.Get<AppViewModel>().HomeViewModel.CurrentWallet;

            await Navigation.PushAsync(new TransactionDetailPage(transaction));
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
            var parent = (this.BindingContext as AppViewModel).TransactionPageViewModel;

            await Navigation.PushAsync(new ChooseWalletPage(parent, Enums.ForType.ForTransactionTab));
        }

        async private void RecentTransactionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Unselect it because i don't want to =((
            (sender as ListView).SelectedItem = null;

            // Open transaction detail
            if (e.SelectedItem is null) // Null check
                return;

            var transaction = e.SelectedItem as TransactionViewModel;
            transaction.Wallet = DependencyService.Get<AppViewModel>().HomeViewModel.CurrentWallet;

            await Navigation.PushAsync(new TransactionDetailPage(transaction));
        }

        private async void TransactionContextMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.ShowPopupAsync(new TimeIntervalPopup()
            {
                Anchor = sender as Button
            });
        }

        #region Plan Tab

        private void Bill_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BudgetPage());
        }

        private void Transaction_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PeriodicTransactionPage());
        }

        private void Event_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EventPage());
        }

        private void Budget_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InvoicePage());
        }

        #endregion

        #region Account Tab

        private void BtnChangePassword_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangPasswordPage());
        }

        #endregion

        private void Information_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AccountManagementPage());
        }

        async private void BtnBankService_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BankServicePage());
        }

        private void MyWalletTapped(object sender, EventArgs e)
        {

        }

        async private void Tool_Tapped(object sender, EventArgs e)
        {
            
        }

        async private void Setting_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingPage());
        }

        async private void Loan_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoanPage());
        }
    }
}