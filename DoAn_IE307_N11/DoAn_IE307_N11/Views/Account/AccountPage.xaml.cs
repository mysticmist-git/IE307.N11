using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.ViewModels;
using DoAn_IE307_N11.Views.All;
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
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
        }

        async private void SignOut_Clicked(object sender, EventArgs e)
        {
            
        }

        private void Information_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AccountManagementPage());
        }

        private void BtnBankService_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BankServicePage());
        }
    }
}