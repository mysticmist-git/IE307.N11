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
    public partial class NotePage : ContentPage
    {
        public NotePage(object parent)
        {
            InitializeComponent();

            this.BindingContext = new NoteViewModel(parent);
        }

        private async void SaveNote_Clicked(object sender, EventArgs e)
        {
            var viewModel = this.BindingContext as NoteViewModel;
            var parent = viewModel.Parent as TransactionViewModel;
            parent.Transaction.Note = viewModel.Note;
            parent.PublicOnPropertyChanged(nameof(parent.Transaction));
            await Navigation.PopAsync();
        }
    }
}