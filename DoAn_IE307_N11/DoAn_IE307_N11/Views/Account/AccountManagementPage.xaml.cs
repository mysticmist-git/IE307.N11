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
    public partial class AccountManagementPage : ContentPage
    {
        public AccountManagementPage()
        {
            InitializeComponent();
        }

        private void BtnChangePassword_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangPasswordPage());
        }

        async private void SignOut_Clicked(object sender, EventArgs e)
        {
            var viewModel = DependencyService.Get<AppViewModel>();
            viewModel.AccountViewModel.IsBusy = true;

            // Delete account
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Account>();

            // Delete local data
            var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();
            localData.WalletId = 0;
            await DependencyService.Get<SQLiteDBAsync>().DB.UpdateAsync(localData);

            Application.Current.MainPage = new NavigationPage(new LoginPage());

            viewModel.AccountViewModel.IsBusy = false;
        }
    }
}