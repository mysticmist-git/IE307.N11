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
    public partial class EventPage : ContentPage
    {
        public EventPage()
        {
            InitializeComponent();

            this.BindingContext = DependencyService.Get<AppViewModel>().EventPageViewModel;
        }

        async private void AddEventClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEventPage());
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            // Viewmodel load
            var viewModel = this.BindingContext as EventPageViewModel;

            if (viewModel.CanLoadMore)
            {
                await viewModel.LoadData();
                viewModel.CanLoadMore = false;
            }
        }

        private async void EventSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Unselect item
            (sender as ListView).SelectedItem = null;

            // Get item
            var theEvent = e.SelectedItem as EventViewModel;

            // Null check
            if (theEvent is null) return;

            // Open event detail page
            await Navigation.PushAsync(new EventDetailPage(theEvent));
        }
    }
}