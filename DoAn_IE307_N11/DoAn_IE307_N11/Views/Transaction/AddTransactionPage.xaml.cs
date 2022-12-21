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
    public partial class AddTransactionPage : ContentPage
    {
        public AddTransactionPage()
        {
            InitializeComponent();
        }


        private void EnterAmount_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnterAmountPage());
        }

        private void ChooseType_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChooseTransactionTypePage());
        }

        private void AddNote_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotePage());
        }

        private void Wallet_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChooseWalletPage(this.BindingContext as AddTransactionViewModel));
        }
    }
}