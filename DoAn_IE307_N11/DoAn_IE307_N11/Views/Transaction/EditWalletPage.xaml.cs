using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditWalletPage : ContentPage
    {
        public EditWalletPage(ChooseWalletViewModel parentViewModel, WalletViewModel walletInfo)
        {
            InitializeComponent();

            this.BindingContext = new EditWalletViewModel(parentViewModel, walletInfo);
        }

        async private void ChooseIcon_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseIconPage(this.BindingContext, ForType.ForEditWallet));
        }

        async private void CurrencyAreaTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseCurrencyPage(this.BindingContext, ForType.ForEditWallet));
        }

        async private void BalanceAreaTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SimpleEnterAmountPage(this.BindingContext, ForType.ForEditWallet));
        }

        async private void SaveWallet_Clicked(object sender, EventArgs e)
        {
            var accepted = await DisplayAlert("Xoá ví", "Bạn có chắc muốn lưu chỉnh sửa?", "Có", "Không");

            if (!accepted)
                return;

            var viewModel = (this.BindingContext as EditWalletViewModel);


            switch (result)
            {
                case Utils.CommonResult.Ok:
                    //await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case Utils.CommonResult.Fail:
                    await DisplayAlert("Lỗi", "Xoá ví thất bại", "Ok");
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
        private async void DeleteWallet_Clicked(object sender, EventArgs e)
        {
            var accepted = await DisplayAlert("Xoá ví", "Bạn có chắc muốn xoá ví này?", "Có", "Không");

            if (!accepted)
                return;

            var viewModel = (this.BindingContext as EditWalletViewModel);

            var result = await viewModel.DELETEWallet();

            switch (result)
            {
                case Utils.CommonResult.Ok:
                    //await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case Utils.CommonResult.Fail:
                    await DisplayAlert("Lỗi", "Xoá ví thất bại", "Ok");
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
    }
}