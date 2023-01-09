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
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;
using Xamarin.Forms.PlatformConfiguration.GTKSpecific;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views.All
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public enum LoginResult
        {
            None,
            WrongUsernameOrPassword,
            Success
        }

        public enum LoginInfoCheckResult
        {
            None,
            EmptyUsername,
            EmptyPassword,
            Valid
        }


        public LoginPage()
        {
            InitializeComponent();

        }

        async private void Login_Clicked(object sender, EventArgs e)
        {
            ToggleBusyMode(false);

            var username = username_entry.Text;
            var password = password_entry.Text;

            LoginInfoCheckResult result = CheckLoginInfo(username, password);

            switch ((result))
            {
                case LoginInfoCheckResult.None:
                    await DisplayAlert("Lỗi", "Lỗi không xác định", "Ok");
                    break;
                case LoginInfoCheckResult.EmptyUsername:
                    await DisplayAlert("Lỗi", "Vui lòng nhập tên tài khoản", "Ok");
                    break;
                case LoginInfoCheckResult.EmptyPassword:
                    await DisplayAlert("Lỗi", "Vui lòng nhập mật khẩu", "Ok");
                    break;
                default:
                    break;
            }

            if (result != LoginInfoCheckResult.Valid)
            {
                ToggleBusyMode(true);
                return;
            }

            Account userAccount = null;

            try
            {

                using (var httpClient = new HttpClient())
                {
                    var ip = DependencyService.Get<ConstantService>().MY_IP;
                    var accounts = await httpClient.GetStringAsync($"http://{ip}/moneybook/api/ServiceController/" +
                        $"LoginAccountIn?username={username}&password={password}");
                    var convertedAccounts = JsonConvert.DeserializeObject<List<Account>>(accounts);

                    if (convertedAccounts is null || convertedAccounts.Count <= 0)
                    {
                        await DisplayAlert("Lỗi", "Sai tên tài khoản hoặc mật khẩu", "Ok");
                        ToggleBusyMode(true);
                        return;
                    }

                    userAccount = convertedAccounts.FirstOrDefault();
                }

            }
            catch
            {
                await DisplayAlert("Lỗi", "Không có kết nối Internet", "Ok");
                ToggleBusyMode(true);
                return;
            }

            if (userAccount is null)
            {
                await DisplayAlert("Lỗi", "Sai tên tài khoản hoặc mật khẩu", "Ok");
                ToggleBusyMode(true);
                return;
            }


            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Account>();
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Wallet>();
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Acquaintance>();
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Transaction>();
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Event>();
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<Acquaintance_Transaction>();
            await DependencyService.Get<SQLiteDBAsync>().DB.DeleteAllAsync<LocalData>();

            userAccount.ServerId = userAccount.Id;

            await DependencyService.Get<SQLiteDBAsync>().DB.InsertAsync(userAccount);
            await DependencyService.Get<SQLiteDBAsync>().DB.InsertAsync(new LocalData());

            var createWalletPage = DependencyService.Get<ViewService>().BuildCreateWalletPage(Enums.ForType.ForOriginalCreateWallet);
            Xamarin.Forms.Application.Current.MainPage = createWalletPage;
        }

        private void ToggleBusyMode(bool mode)
        {
            login_btn.IsEnabled = mode;
            username_entry.IsEnabled = mode;
            password_entry.IsEnabled = mode;
        }

        private LoginInfoCheckResult CheckLoginInfo(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                return LoginInfoCheckResult.EmptyUsername;
            if (string.IsNullOrEmpty(password))
                return LoginInfoCheckResult.EmptyPassword;

            return LoginInfoCheckResult.Valid;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            ToggleBusyMode(false);

            var accounts = DependencyService.Get<SQLiteDB>().DB.Table<Account>().ToArray();
            if (accounts != null && accounts.Length > 0)
            {
                var createWalletPage = DependencyService.Get<ViewService>().BuildCreateWalletPage(Enums.ForType.ForOriginalCreateWallet);
                Xamarin.Forms.Application.Current.MainPage = createWalletPage;
            }

            ToggleBusyMode(true);
        }

        /// <summary>
        /// Register a new account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void Register_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }

    
}