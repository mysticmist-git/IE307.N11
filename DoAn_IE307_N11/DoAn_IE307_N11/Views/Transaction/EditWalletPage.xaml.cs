using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.ViewModels.All;
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
    public partial class EditWalletPage : ContentPage
    {
        public EditWalletPage(ChooseWalletViewModel parentViewModel, WalletViewModel walletInfo)
        {
            InitializeComponent();

            this.BindingContext = new EditWalletViewModel(parentViewModel, walletInfo);
        }

        async private void ChooseIcon_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseIconPage(this.BindingContext as CreateWalletViewModel));
        }

        async private void CurrencyAreaTapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ChooseCurrencyPage(this.BindingContext as EditWalletViewModel));
        }

        async private void BalanceAreaTapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new EnterAmountPageForCreateWalletPage(this.BindingContext as EditWalletViewModel));
        }

        private void SaveWallet_Clicked(object sender, EventArgs e)
        {

        }

        private void CreateWallet_Clicked(object sender, EventArgs e)
        {

        }
    }
}