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
    public enum ChangePasswordCheckInfoResult
    {
        WrongPassword,
        NewPasswordNotMatched,
        Error,
        NewPasswordRequired,
        OldPasswordEmpty,
        NewPasswordEmpty,
        ReNewPasswordEmpty,
        Valid,
    }


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangPasswordPage : ContentPage
    {
        public ChangPasswordPage()
        {
            InitializeComponent();
        }

        private void BtnForgetPassword_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgetPasswordPage());
        }

        private async void BtnChangePassword_Clicked(object sender, EventArgs e)
        {
            // Get info
            var oldPassword = oldPasswordEntry.Text;
            var newPassword = newPasswordEntry.Text;
            var reNewPassword = reNewPasswordEntry.Text;

            // Check password info
            var infoCheckResult = await CheckChangePasswordInfo(oldPassword, newPassword, reNewPassword);

            // Handle result
            await HandleCheckChangePasswordInfoResult(infoCheckResult);

            // Proceed to change password if infos is valid
            if (infoCheckResult != ChangePasswordCheckInfoResult.Valid)
                return;

            // Get current account
            var dataService = DependencyService.Get<SQLiteDBAsync>();
            var currentAccount = await dataService.DB.Table<Account>().FirstOrDefaultAsync();

            // Something real wrong there if it's null. It's not suppose to be null
            if (currentAccount is null)
                return;

            // Change password of current account (local)
            currentAccount.Password = newPassword;            

            // Get api service
            var apiService = DependencyService.Get<ApiService>();

            // Call api to update password
            var changePasswordResult = await apiService.UpdateAccount(currentAccount);

            await HandleChangePasswordResult(changePasswordResult);
        }

        #region Methods

        public async Task<ChangePasswordCheckInfoResult> CheckChangePasswordInfo(string oldPassword, string newPassword, string reNewPassword)
        {
            // Check if fields is not empty
            if (string.IsNullOrEmpty(oldPassword))
                return ChangePasswordCheckInfoResult.OldPasswordEmpty;

            if (string.IsNullOrEmpty(newPassword))
                return ChangePasswordCheckInfoResult.NewPasswordEmpty;
            //await DisplayAlert("Thông báo", "Vui lòng nhập mật khẩu mới", "Ok");
            if (string.IsNullOrEmpty(reNewPassword))
                return ChangePasswordCheckInfoResult.ReNewPasswordEmpty;
            //await DisplayAlert("Thông báo", "Mật khẩu không khớp", "Ok");

            // Get current account
            var dataService = DependencyService.Get<SQLiteDBAsync>();
            var currentAccount = await dataService.DB.Table<Account>().FirstOrDefaultAsync();

            // Something real wrong there if it's null. It's not suppose to be null
            if (currentAccount is null)
                return ChangePasswordCheckInfoResult.Error;

            // Get account info
            var username = currentAccount.Username;

            // Get api service
            var apiService = DependencyService.Get<ApiService>();

            // Check old password
            var isOldPasswordCorrect = await apiService.CheckPasswordAsync(username, oldPassword);

            if (!isOldPasswordCorrect)
                return ChangePasswordCheckInfoResult.WrongPassword;

            // Check new password if it is indeed different from the old one
            if (oldPassword == newPassword)
                return ChangePasswordCheckInfoResult.NewPasswordRequired;

            // Check if new password and its reentry is matched
            if (newPassword != reNewPassword)
                return ChangePasswordCheckInfoResult.NewPasswordNotMatched;

            // Infos is valid
            return ChangePasswordCheckInfoResult.Valid;
        }

        /// <summary>
        /// Log user out
        /// THIS IS BAD: DEPENDENCY THINGY
        /// </summary>
        /// <returns></returns>
        async public Task Signout()
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

        #endregion

        #region Result Handlers

        async public Task HandleCheckChangePasswordInfoResult(ChangePasswordCheckInfoResult result)
        {
            switch (result)
            {
                case ChangePasswordCheckInfoResult.WrongPassword:
                    await DisplayAlert("Thông báo", "Sai mật khẩu", "Ok");
                    break;
                case ChangePasswordCheckInfoResult.NewPasswordNotMatched:
                    await DisplayAlert("Thông báo", "Mật khẩu mới không khớp", "Ok");
                    break;
                case ChangePasswordCheckInfoResult.Error:
                    await DisplayAlert("Thông báo", "Đã có lỗi xảy ra", "Ok");
                    break;
                case ChangePasswordCheckInfoResult.NewPasswordRequired:
                    await DisplayAlert("Thông báo", "Mật khẩu mới phải khác với mật khẩu cũ", "Ok");
                    break;
                case ChangePasswordCheckInfoResult.OldPasswordEmpty:
                    await DisplayAlert("Thông báo", "Vui lòng nhập mật khẩu cũ", "Ok");
                    break;
                case ChangePasswordCheckInfoResult.NewPasswordEmpty:
                    await DisplayAlert("Thông báo", "Vui lòng nhập mật khẩu mới", "Ok");
                    break;
                case ChangePasswordCheckInfoResult.ReNewPasswordEmpty:
                    await DisplayAlert("Thông báo", "Vui lòng nhập lại mật khẩu mới", "Ok");
                    break;
            }
        }
        
        async public Task HandleChangePasswordResult(bool result)
        {
            if (result) // Success
            {
                await DisplayAlert("Thông báo", "Đổi mật khẩu thành công", "Ok");
                await Signout();
                return;
            }

            await DisplayAlert("Thông báo", "Đổi mật khẩu thất bại", "Ok");
        }

        #endregion

    }
}