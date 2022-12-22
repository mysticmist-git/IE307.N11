using DoAn_IE307_N11.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            //this.BindingContext = new HomeViewModel();
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTransactionPage());
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            //var result = await (this.BindingContext as HomeViewModel).GETData();

            //switch (result)
            //{
            //    case Utils.CommonResult.Ok:
            //        //await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
            //        break;
            //    case Utils.CommonResult.Fail:
            //        await DisplayAlert("Lỗi", "Đã có lỗi gì đó xảy ra", "Ok");
            //        break;
            //    case Utils.CommonResult.NoInternet:
            //        await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
            //        break;
            //}

            //if (result != Utils.CommonResult.Ok)
            //    return;
        }

        private void AllWallet_Clicked(object sender, EventArgs e)
        {

        }

        //async private void AllWallet_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new ChooseWalletPage())
        //}
    }
}