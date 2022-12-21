using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.ValueConverters;
using DoAn_IE307_N11.ViewModels.All;
using DoAn_IE307_N11.ViewModels.Transaction.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseIconPage : ContentPage
    {
        public ChooseIconPage(CreateWalletViewModel parentViewModel)
        {
            InitializeComponent();

            var viewModel = new ChooseIconViewModel(flex);
            viewModel.ParentViewModel = parentViewModel;

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
                    await DisplayAlert("Lỗi", "Không có Internet", "Ok");
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
            viewModel.IsBusy = true;

            var parentViewModel =
                viewModel.ParentViewModel;
            
            parentViewModel.IconId = ((sender as ImageButton).BindingContext as Icon).Id

            parentViewModel.WalletIconUrl =
                ((sender as ImageButton).BindingContext as Icon).ImageUrl;


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