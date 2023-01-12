using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.ViewModels;
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
    public partial class AddEventPage : ContentPage
    {
        public AddEventPage()
        {
            InitializeComponent();

            this.BindingContext = new AddEventPageViewModel();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load default value
            var viewModel = this.BindingContext as AddEventPageViewModel;

            if (!viewModel.CanLoadMore)
                return;

            await viewModel.LoadDefaultData();
            viewModel.CanLoadMore = false;  
        }

        async private void SaveEventClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập tên sự kiện", "Ok");
                return;
            }

            // Get local data to get the current account
            var sqliteService = DependencyService.Get<SQLiteDBAsync>();
            var currentAccount = await sqliteService.DB.Table<Account>().FirstOrDefaultAsync();

            if (currentAccount is null) // This is so wrong if it's null there
                return;

            // Assemply the event object
            var viewModel = (this.BindingContext as AddEventPageViewModel);
            var newEvent = viewModel.EventViewModel.Event;

            newEvent.AccountId = currentAccount.Id;

            // Save new event
            var apiService = DependencyService.Get<ApiService>();
            var result = await apiService.AddNewEvent(newEvent);
                
            await HandleNewEventResult(result);

            // Add it to the paprent tabs
            if (result == ApiCallResult.Success) {
                var appViewModel = DependencyService.Get<AppViewModel>();
                if (newEvent.ExpiredDate.Date >= DateTime.Now.Date)
                    appViewModel.EventPageViewModel.ActiveEvents.Add(viewModel.EventViewModel);
                else
                    appViewModel.EventPageViewModel.FinishedEvent.Add(viewModel.EventViewModel);
                await Navigation.PopAsync(); 
            }
        }

        private void DateAreaTapped(object sender, EventArgs e)
        {
            date.Focus();
        }

        private void date_DateSelected(object sender, DateChangedEventArgs e)
        {
            var viewModel = (this.BindingContext as AddEventPageViewModel);
            viewModel.PublicOnPropertyChanged(nameof(viewModel.EventViewModel.DateDisplayer));
            viewModel.PublicOnPropertyChanged(nameof(viewModel.EventViewModel));
        }

        async private void ChooseIcon_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseIconPage(this.BindingContext, ForType.ForAddEvent));
        }

        #region Result Handlers

        private async Task HandleNewEventResult(ApiCallResult result)
        {
            switch (result)
            {
                case ApiCallResult.NullData:
                    await DisplayAlert("Thông báo", "Null Error", "Ok");
                    break;
                case ApiCallResult.Success:
                    await DisplayAlert("Thông báo", "Thêm sự kiện mới thành công", "Ok");
                    break;
                case ApiCallResult.Fail:
                    await DisplayAlert("Thông báo", "Thêm sự kiến mới thất bại", "Ok");
                    break;
                case ApiCallResult.UnknownError:
                    await DisplayAlert("Thông báo", "Đã có lỗi xảy ra", "Ok");
                    break;
                case ApiCallResult.None:
                    break;
            }

            #endregion
        }
    }
}