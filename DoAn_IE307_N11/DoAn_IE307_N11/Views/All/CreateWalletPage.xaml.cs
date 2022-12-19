using DoAn_IE307_N11.ViewModels.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views.All
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
            await Navigation.PushAsync(new ChooseIconPage(this.BindingContext as CreateWalletViewModel));
        }

        async private void CurrencyAreaTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseCurrencyPage(this.BindingContext as CreateWalletViewModel));
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (this.BindingContext as CreateWalletViewModel);
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
                    await DisplayAlert("Lỗi", "Không có Internet", "Ok");
                    break;
            }

            if (result != Utils.CommonResult.Ok)
                return;

            viewModel.CanLoadMore = false;
        }
    }
}