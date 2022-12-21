using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseCurrencyPage : ContentPage
    {
        public ForType Type { get; set; }
        public ChooseCurrencyPage(object parent, ForType type)
        {
            InitializeComponent();

            this.BindingContext = new ChooseCurrencyViewModel(parent, type);
            Type = type;
        }

        async protected override void OnAppearing()
        {
            switch (Type)
            {
                case ForType.ForCreateWallet:
                    await CreateWalletAppear();
                    break;
                case ForType.ForEditWallet:
                    await EditWalletAppear();
                    break;
            }
            
        }

        async private Task CreateWalletAppear()
        {
            var viewModel = (this.BindingContext as ChooseCurrencyViewModel);
            var parentViewModel = viewModel.ParentViewModel as CreateWalletViewModel;

            viewModel.IsBusy = true;

            base.OnAppearing();

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
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
            }

            if (viewModel.CurrencyList != null || viewModel.CurrencyList.Count > 0)
            {
                parentViewModel.Currency = viewModel.CurrencyList
                    .Where(currency => currency.Info.Code == "vnd")
                    .Select(currency => currency.Info)
                    .FirstOrDefault();

                if (parentViewModel.Currency is null)
                    parentViewModel.Currency = viewModel.CurrencyList.FirstOrDefault().Info;
            }

            if (result != Utils.CommonResult.Ok)
            {
                viewModel.IsBusy = false;
                return;
            }

            viewModel.LoadSeletedCurrency();
            viewModel.IsBusy = false;
        }

        async private Task EditWalletAppear()
        {
            var viewModel = (this.BindingContext as ChooseCurrencyViewModel);
            var parentViewModel = viewModel.ParentViewModel as EditWalletViewModel;

            viewModel.IsBusy = true;

            base.OnAppearing();

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
                    await DisplayAlert("Lỗi", "Lỗi mạng", "Ok");
                    break;
            }

            if (viewModel.CurrencyList != null || viewModel.CurrencyList.Count > 0)
            {
                parentViewModel.Currency = viewModel.CurrencyList
                    .Where(currency => currency.Info.Code == "vnd")
                    .Select(currency => currency.Info)
                    .FirstOrDefault();

                if (parentViewModel.Currency is null)
                    parentViewModel.Currency = viewModel.CurrencyList.FirstOrDefault().Info;
            }

            if (result != Utils.CommonResult.Ok)
            {
                viewModel.IsBusy = false;
                return;
            }

            viewModel.LoadSeletedCurrency();
            viewModel.IsBusy = false;
        }

        async private void Currency_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            switch (Type)
            {
                case ForType.ForCreateWallet:
                    {
                        var viewModel = (this.BindingContext as ChooseCurrencyViewModel);
                        viewModel.IsBusy = true;
                        (sender as ListView).IsEnabled = false;

                        var parentViewModel =
                            viewModel.ParentViewModel as CreateWalletViewModel;

                        parentViewModel.Currency =
                            (e.SelectedItem as CurrencyViewModel).Info;

                        viewModel.CurrencyList.ForEach(currency => currency.IsSelected = false);
                        (e.SelectedItem as CurrencyViewModel).IsSelected = true;


                        await Navigation.PopAsync();

                        (sender as ListView).IsEnabled = true;
                        viewModel.IsBusy = false;
                    }
                    break;
                case ForType.ForEditWallet:
                    {
                        var viewModel = (this.BindingContext as ChooseCurrencyViewModel);
                        viewModel.IsBusy = true;
                        (sender as ListView).IsEnabled = false;

                        var parentViewModel =
                            viewModel.ParentViewModel as EditWalletViewModel;

                        parentViewModel.Currency =
                            (e.SelectedItem as CurrencyViewModel).Info;

                        viewModel.CurrencyList.ForEach(currency => currency.IsSelected = false);
                        (e.SelectedItem as CurrencyViewModel).IsSelected = true;


                        await Navigation.PopAsync();

                        (sender as ListView).IsEnabled = true;
                        viewModel.IsBusy = false;
                    }
                    break;
            }

        }
    }
}