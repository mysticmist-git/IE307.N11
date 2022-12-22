using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.Transaction;
using DoAn_IE307_N11.ViewModels.Transaction.Wrapper;
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
    public partial class ChooseTransactionTypePage : ContentPage
    {
        public ChooseTransactionTypePage(object parent)
        {
            InitializeComponent();

            this.BindingContext = new ChooseTransactionTypeViewModel(parent);
        }

        private void SearchTransacionType_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewmodel = this.BindingContext as ChooseTransactionTypeViewModel;
            var result = await viewmodel.GETData();

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

            if (result != Utils.CommonResult.Ok)
            {
                return;
            }

        }

        private async void LstTransactionType_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var transactionViewModel = (this.BindingContext as ChooseTransactionTypeViewModel).Parent as TransactionViewModel;
            transactionViewModel.Type = (e.SelectedItem as TransactionTypeViewModel).TransactionType;
            transactionViewModel.TypeImage = (e.SelectedItem as TransactionTypeViewModel).TypeImageUrl;
            await Navigation.PopAsync();
        }
    }
}