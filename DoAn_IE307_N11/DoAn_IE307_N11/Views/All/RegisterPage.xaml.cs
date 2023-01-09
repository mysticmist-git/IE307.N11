using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public enum RegisterResult
        {
            None,
            WrongUsernameOrPassword,
            Success
        }

        public enum RegisterInfoCheckResult
        {
            None,
            EmptyUsername,
            EmptyPassword,
            Valid,
            EmptyReEntryPassword,
            PasswordNotMatched,
            UsernameExisted
        }


        public RegisterPage()
        {
            InitializeComponent();

        }

        async private void Register_Clicked(object sender, EventArgs e)
        {
            ToggleBusyMode(false);

            var username = username_entry.Text;
            var password = password_entry.Text;
            var reEntryPassword = password_reentry.Text;

            RegisterInfoCheckResult result = await CheckRegisterInfo(username, password, reEntryPassword);

            await DisplayRegisterResult(result);

            if (result != RegisterInfoCheckResult.Valid)
            {
                ToggleBusyMode(true);
                return;
            }

            Account newAccount = new Account
            {
                Username = username,
                Password = password
            };

            var apiCallresult = await DependencyService.Get<ApiService>().RegisterAccount(newAccount);

            await AccountRegisterApiCallResultHandler(apiCallresult);

            if (apiCallresult != ApiCallResult.Success)
            {
                ToggleBusyMode(true);
                return;
            }

            // Return to login page
            await Navigation.PopAsync();
            ToggleBusyMode(true);
        }

        private async Task AccountRegisterApiCallResultHandler(ApiCallResult apiCallResult)
        {
            switch (apiCallResult)
            {
                case ApiCallResult.Success:
                    await DisplayAlert("Thành công", "Tạo tài khoản thành công", "Ok");
                    break;
                case ApiCallResult.Fail:
                    await DisplayAlert("Lỗi", "Tạo tài khoản thất bại", "Ok");
                    break;
                case ApiCallResult.UnknownError:
                    await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case ApiCallResult.None:
                    await DisplayAlert("Lỗi", "Nhận enum \"NONE\"", "Ok");
                    break;
                default:
                    break;
            }
        }

        async private Task DisplayRegisterResult(RegisterInfoCheckResult result)
        {
            switch ((result))
            {
                case RegisterInfoCheckResult.None:
                    await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case RegisterInfoCheckResult.EmptyUsername:
                    await DisplayAlert("Lỗi", "Vui lòng nhập tên tài khoản", "Ok");
                    break;
                case RegisterInfoCheckResult.EmptyPassword:
                    await DisplayAlert("Lỗi", "Vui lòng nhập mật khẩu", "Ok");
                    break;
                case RegisterInfoCheckResult.PasswordNotMatched:
                    await DisplayAlert("Lỗi", "Mật khẩu không khớp", "Ok");
                    break;
                case RegisterInfoCheckResult.UsernameExisted:
                    await DisplayAlert("Lỗi", "Tên tài khoản đã tồn tại", "Ok");
                    break;
                default:
                    break;
            }
        }

        private void ToggleBusyMode(bool mode)
        {
            register_btn.IsEnabled = mode;
            username_entry.IsEnabled = mode;
            password_entry.IsEnabled = mode;
            password_reentry.IsEnabled = mode;
            close_btn.IsEnabled = mode;
        }

        async private Task<RegisterInfoCheckResult> CheckRegisterInfo(string username, string password, string reEntryPassword)
        {
            if (string.IsNullOrEmpty(username))
                return RegisterInfoCheckResult.EmptyUsername;
            if (string.IsNullOrEmpty(password))
                return RegisterInfoCheckResult.EmptyPassword;
            if (string.IsNullOrEmpty(reEntryPassword))
                return RegisterInfoCheckResult.EmptyReEntryPassword;

            if (password != reEntryPassword)
                return RegisterInfoCheckResult.PasswordNotMatched;

            // Check account existed
            var usernameExisted = await DependencyService.Get<ApiService>().CheckUsernameExisted(username);

            if (usernameExisted)
                return RegisterInfoCheckResult.UsernameExisted;

            return RegisterInfoCheckResult.Valid;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            ToggleBusyMode(false);

            var accounts = DependencyService.Get<SQLiteDB>().DB.Table<Account>().ToArray();
            if (accounts != null && accounts.Length > 0)
            {
                var createWalletPage = DependencyService.Get<ViewService>().BuildCreateWalletPage(Enums.ForType.ForOriginalCreateWallet);
                Application.Current.MainPage = createWalletPage;
            }

            ToggleBusyMode(true);
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}