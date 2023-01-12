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
    public partial class EventDetailPage : ContentPage
    {
        public EventDetailPage(EventViewModel eventViewModel)
        {
            InitializeComponent();

            this.BindingContext = new EventDetailPageViewModel(eventViewModel);
        }

        async private void SaveEventClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập tên sự kiện", "Ok");
                return;
            }

            // Assemply the event object
            var viewModel = (this.BindingContext as EventDetailPageViewModel);
            var theEvent = viewModel.EventViewModel.Event;

            // Save updated event
            var apiService = DependencyService.Get<ApiService>();
            var result = await apiService.UpdateEventAsync(theEvent);

            await HandleUpdateEventResult(result);

            // A trick to make viewmodel update
            var appViewModel = DependencyService.Get<AppViewModel>();
            viewModel.EventViewModel.PublicOnPropretyChanged(nameof(viewModel.EventViewModel.Event));

            // Check if expired date is outdated of push back to be valid
            if (theEvent.IsActive && theEvent.ExpiredDate.Date < DateTime.Now.Date)
            {
                theEvent.IsActive = false;

                // Update tabs
                appViewModel.EventPageViewModel.ActiveEvents.Remove(viewModel.EventViewModel);
                appViewModel.EventPageViewModel.FinishedEvent.Add(viewModel.EventViewModel);
            }
            else if (!theEvent.IsActive && theEvent.ExpiredDate.Date >= DateTime.Now.Date)
            {
                theEvent.IsActive = true;

                // Update tabs
                appViewModel.EventPageViewModel.FinishedEvent.Remove(viewModel.EventViewModel);
                appViewModel.EventPageViewModel.ActiveEvents.Add(viewModel.EventViewModel);
            }
        }

        private void DateAreaTapped(object sender, EventArgs e)
        {
            date.Focus();
        }

        private void date_DateSelected(object sender, DateChangedEventArgs e)
        {
            var viewModel = (this.BindingContext as EventDetailPageViewModel);
            viewModel.PublicOnPropertyChanged(nameof(viewModel.EventViewModel.DateDisplayer));
            viewModel.PublicOnPropertyChanged(nameof(viewModel.EventViewModel));
        }

        async private void ChooseIcon_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseIconPage(this.BindingContext, ForType.ForEditEvent));
        }

        #region Result Handlers

        private async Task HandleUpdateEventResult(bool result)
        {
            switch (result)
            {
                case true:
                    await DisplayAlert("Thông báo", "Cập nhật sự kiện thành công", "Ok");
                    break;
                case false:
                    await DisplayAlert("Thông báo", "Cập nhật sự kiện thất bại", "Ok");
                    break;
            }

            #endregion
        }

        async private void EventDelete_Clicked(object sender, EventArgs e)
        {
            // Ask again to make sure user want to delete the event
            var isAgreeToDelete = await DisplayAlert("Thông báo", "Bạn có chắc muốn xoá sự kiện này?", "Có", "Không");

            if (!isAgreeToDelete) // The user don't want to delete
                return;

            // Get current viewmodel
            var viewModel = this.BindingContext as EventDetailPageViewModel;

            // Get current event id
            var eventId = -1;
            eventId = viewModel.EventViewModel.Event.Id;

            // Get api service singleton
            var apiService = DependencyService.Get<ApiService>();

            // Proceed to delete transaction
            var isSuccess = await apiService.DeleteEventAsync(eventId);

            if (isSuccess)
            {
                // Remove it from its parent
                var appViewModel = DependencyService.Get<AppViewModel>();
                appViewModel.EventPageViewModel.ActiveEvents.Remove(viewModel.EventViewModel);
                appViewModel.EventPageViewModel.FinishedEvent.Remove(viewModel.EventViewModel);

                await DisplayAlert("Thông báo", "Đã xoá sự kiện thành công.", "Ok");
                await Navigation.PopAsync();
                return;
            }

            await DisplayAlert("Thông báo", "Xoá sự kiện thất bại.", "Ok");
        }
    }
}