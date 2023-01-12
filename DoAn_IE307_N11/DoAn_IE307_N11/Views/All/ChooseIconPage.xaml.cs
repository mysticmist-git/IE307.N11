using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
using System;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseIconPage : ContentPage
    {
        public ForType Type { get; set; }
        public ChooseIconPage(object parentViewModel, ForType type)
        {
            InitializeComponent();

            var viewModel = new ChooseIconViewModel(flex, parentViewModel, type);
            Type = type;

            this.BindingContext = viewModel;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var result = await (this.BindingContext as ChooseIconViewModel).GETIcon();

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
            {
                return;
            }

            LoadIconToScreen();
        }

        private void LoadIconToScreen()
        {
            var iconList = (this.BindingContext as ChooseIconViewModel).IconList;

            if (iconList is null || iconList.Count <= 0)
            {
                return;
            }

            foreach (var icon in iconList)
            {
                var image = new ImageButton
                {
                    WidthRequest = 52,
                    Margin = new Thickness(12, 12),
                    BackgroundColor = Color.Transparent,
                    BindingContext = icon
                };

                image.SetBinding(ImageButton.SourceProperty, new Binding("ImageUrl"));
                image.SetBinding(ImageButton.IsEnabledProperty, new Binding("IsBusy")
                {
                    Source = this.BindingContext,
                    Converter = new InvertedBoolConverter()
                });

                image.Clicked += Icon_Clicked;

                flex.Children.Add(image);
            }
        }

        private async void Icon_Clicked(object sender, EventArgs e)
        {
            flex.IsEnabled = false;

            var viewModel = (this.BindingContext as ChooseIconViewModel);

            switch (Type)
            {
                case ForType.ForCreateWallet:
                    {
                        var parentViewModel = viewModel.ParentViewModel as CreateWalletViewModel;

                        parentViewModel.IconId = ((sender as ImageButton).BindingContext as Icon).Id;
                        parentViewModel.WalletIconUrl =
                            ((sender as ImageButton).BindingContext as Icon).ImageUrl;
                    }
                    break;
                case ForType.ForEditWallet:
                    {
                        var parentViewModel = viewModel.ParentViewModel as EditWalletViewModel;

                        parentViewModel.Wallet.Wallet.IconId = ((sender as ImageButton).BindingContext as Icon).Id;
                        parentViewModel.Wallet.WalletImageUrl =
                            ((sender as ImageButton).BindingContext as Icon).ImageUrl;
                    }
                    break;
                case ForType.ForAddEvent:
                    {
                        var parentViewModel = viewModel.ParentViewModel as AddEventPageViewModel;

                        parentViewModel.EventViewModel.Event.IconId = ((sender as ImageButton).BindingContext as Icon).Id;
                        parentViewModel.EventViewModel.EventImageUrl =
                            ((sender as ImageButton).BindingContext as Icon).ImageUrl;
                    }
                    break;
                case ForType.ForEditEvent:
                    {
                        var parentViewModel = viewModel.ParentViewModel as EventDetailPageViewModel;

                        parentViewModel.EventViewModel.Event.IconId = ((sender as ImageButton).BindingContext as Icon).Id;
                        parentViewModel.EventViewModel.EventImageUrl =
                            ((sender as ImageButton).BindingContext as Icon).ImageUrl;
                    }
                    break;
            }


            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            flex.IsEnabled = false;

            var viewModel = (this.BindingContext as ChooseIconViewModel);
            viewModel.IsBusy = true;

            return false;
        }
    }
}