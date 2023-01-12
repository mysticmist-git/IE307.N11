using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseEventPage : ContentPage
    {
        public ChooseEventPage(object bindingContext, ForType type)
        {
            InitializeComponent();

            this.BindingContext = new ChooseEventPageViewModel(bindingContext, type);
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load data
            var viewModel = this.BindingContext as ChooseEventPageViewModel;
            await viewModel.LoadData();
        }

        async private void EventSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Unselect it immediately
            (sender as ListView).SelectedItem = null;

            // Get selected item
            var selectedEvent = e.SelectedItem as EventViewModel;

            if (selectedEvent is null)
                return;

            // Get the parent view's viewmodel which is add transaction page
            var viewModel = (this.BindingContext as ChooseEventPageViewModel);

            // Select item
            if (viewModel.Type == ForType.ForAddTransaction || viewModel.Type == ForType.ForEditWallet)
            {
                var parentViewModel = viewModel.Parent as TransactionViewModel;
                parentViewModel.Event = selectedEvent.Event;
            } 

            // Return 
            await Navigation.PopAsync();
        }
    }
}