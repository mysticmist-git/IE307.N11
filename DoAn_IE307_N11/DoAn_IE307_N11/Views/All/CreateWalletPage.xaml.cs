using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
using Newtonsoft.Json;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateWalletPage : ContentPage
    {
        public CreateWalletPage()
        {
            InitializeComponent();

            this.BindingContext = new CreateWalletViewModel();
        }

        async private void ChooseIcon_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseIconPage(this.BindingContext, ForType.ForCreateWallet));
        }

        async private void CurrencyAreaTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseCurrencyPage(this.BindingContext, ForType.ForCreateWallet));
        }

        async private void BalanceAreaTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SimpleEnterAmountPage(this.BindingContext, ForType.ForCreateWallet));
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (this.BindingContext as CreateWalletViewModel);

            // Check wallet exist
            CommonResult checkWalletResult = await viewModel.CheckWalletExist();

            switch (checkWalletResult)
            {
                case Utils.CommonResult.NoInternet:
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
                default:
                    break;
            }

            if (checkWalletResult == CommonResult.Ok)
            {
                var mainPage = new NavigationPage(new MainPage());
                mainPage.BarBackgroundColor = Color.White;
                mainPage.BarTextColor = Color.Black;

                Application.Current.MainPage = mainPage;
                return;
            }

            // Load default data for wallet creation
            if (!viewModel.CanLoadMore)
                return;

            var result = await viewModel.LoadDefaultValue();

            switch (result)
            {
                case Utils.CommonResult.Ok:
                    //await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case Utils.CommonResult.Fail:
                    await DisplayAlert("Lỗi", "Load Icon thất bại", "Ok");
                    break;
                case Utils.CommonResult.NoInternet:
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
            }

            if (result != Utils.CommonResult.Ok)
                return;

            viewModel.CanLoadMore = false;
        }

        async private void CreateWallet_Clicked(object sender, EventArgs e)
        {
            ToggleAllView(false);
            var viewModel = (this.BindingContext as CreateWalletViewModel);

            CommonResult result = await viewModel.CreateWallet();

            switch (result)
            {
                case Utils.CommonResult.Ok:
                    //await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case Utils.CommonResult.Fail:
                    await DisplayAlert("Lỗi", "Tạo ví thất bại", "Ok");
                    break;
                case Utils.CommonResult.NoInternet:
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
            }

            if (result != Utils.CommonResult.Ok)
            {
                ToggleAllView(true);
                return;
            }


            // Check wallet exist
            CommonResult checkWalletResult = await viewModel.CheckWalletExist();

            switch (checkWalletResult)
            {
                case Utils.CommonResult.NoInternet:
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
                default:
                    break;
            }

            if (checkWalletResult == CommonResult.Ok)
            {
                var mainPage = new NavigationPage(new MainPage());
                mainPage.BarBackgroundColor = Color.White;
                mainPage.BarTextColor = Color.Black;

                Application.Current.MainPage = mainPage;
                ToggleAllView(true);
                return;
            }


            ToggleAllView(true);
        }

        private void ToggleAllView(bool mode)
        {
            CreateWallet_Btn.IsEnabled = mode;
            BalanceArea.IsEnabled = mode;
            CurrencyArea.IsEnabled = mode;
            NameArea.IsEnabled = mode;
        }
    }
}