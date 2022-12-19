using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseCurrencyPage : ContentPage
    {
        public ChooseCurrencyPage(ViewModels.All.CreateWalletViewModel createWalletViewModel)
        {
            InitializeComponent();

            this.BindingContext = new ChooseCurrencyViewModel(createWalletViewModel);
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (this.BindingContext as ChooseCurrencyViewModel);
            viewModel.IsBusy = true;

            var result = await viewModel.LoadCurrencies();


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

            if (viewModel.CurrencyList != null || viewModel.CurrencyList.Count > 0)
            {
                viewModel.ParentViewModel.Currency = viewModel.CurrencyList
                    .Where(currency => currency.Info.Code == "vnd")
                    .Select(currency => currency.Info)
                    .FirstOrDefault();

                if (viewModel.ParentViewModel.Currency is null)
                    viewModel.ParentViewModel.Currency = viewModel.CurrencyList.FirstOrDefault().Info; 
            }

            if (result != Utils.CommonResult.Ok)
                return;

            viewModel.LoadSeletedCurrency();
            viewModel.IsBusy = false;
        }

        async private void Currency_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var viewModel = (this.BindingContext as ChooseCurrencyViewModel);
            (sender as ListView).IsEnabled = false;

            var parentViewModel =
                viewModel.ParentViewModel as CreateWalletViewModel;

            parentViewModel.Currency =
                (e.SelectedItem as CurrencyViewModel).Info;

            viewModel.CurrencyList.ForEach(currency => currency.IsSelected = false);
            (e.SelectedItem as CurrencyViewModel).IsSelected = true;


            await Navigation.PopAsync();

            (sender as ListView).IsEnabled = true;

        }
    }
}