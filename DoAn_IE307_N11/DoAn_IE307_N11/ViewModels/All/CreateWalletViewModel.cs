using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DoAn_IE307_N11.ViewModels.All
{
    public class CreateWalletViewModel : BaseViewModel
    {
        public string WalletIconUrl { get; set; } = "TienMat.png";
        public int IconId { get; set; }
        public string WalletName { get; set; } = "Tiền mặt";
        public Currency Currency { get; set; } = new Currency();
        public int WalletBalance { get; set; }
        public string DisplayWalletBalance => WalletBalance == 0 ? "Số dư" : WalletBalance.ToString();
        public bool IsZeroBalance => WalletBalance == 0;

        async public Task<CommonResult> LoadDefaultValue()
        {
            IsBusy = true;

            var vietnamCurrencyCode = "vnd";
            var defaultWalletIconCode = "wallet";

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getCurrencyString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetCurrencyByCode?code={vietnamCurrencyCode}";
            var getIconString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetIconByCode?code={defaultWalletIconCode}";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get currency
                    var currency = await httpClient.GetStringAsync(getCurrencyString);
                    var convertedCurrency = JsonConvert.DeserializeObject<Currency>(currency);

                    if (convertedCurrency is null)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    Currency = convertedCurrency;

                    // Get icon
                    var icon = await httpClient.GetStringAsync(getIconString);
                    var convertedIcon = JsonConvert.DeserializeObject<Icon>(icon);

                    if (convertedIcon is null)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    IconId = convertedIcon.Id;
                    WalletIconUrl = convertedIcon.ImageUrl;
                }

                IsBusy = false;
                return CommonResult.Ok;
            }
            catch
            {
                IsBusy = false;
                return CommonResult.NoInternet;
            }
        }

        public async Task<CommonResult> CreateWallet()
        {
            IsBusy = true;

            var result = CheckWalletInfo();
            if (!result)
            {
                IsBusy = false;
                return CommonResult.Fail;
            }

            // Get account info
            var account = await DependencyService.Get<SQLiteDBAsync>().DB.Table<Account>().FirstOrDefaultAsync();

            if (account is null)
            {
                IsBusy = false;
                return CommonResult.Fail;
            }

            var accountId = account.ServerId;

            // Generate wallet model
            Wallet newWallet = new Wallet
            {
                CurrencyId = Currency.Id,
                AccountId = accountId,
                IconId = IconId,
                Name = WalletName,
                Balance = WalletBalance
            };

            var postResult = await POSTWallet(newWallet);

            IsBusy = false;
            return postResult;
        }


        async private Task<CommonResult> POSTWallet(Wallet newWallet)
        {
            IsBusy = true;

            if (newWallet is null)
                return CommonResult.Fail;

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var postString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"CreateWallet";

            var myContent = JsonConvert.SerializeObject(newWallet);

            // construct a content object to send this data
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            // Next, you want to set the content type to let the API know this is JSON.
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Then you can send your request very similar to your previous example with the form content:
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.PostAsync(postString, byteContent);

                    if (!result.IsSuccessStatusCode)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    IsBusy = false;
                    return CommonResult.Ok;
                }
            }
            catch
            {
                IsBusy = false;
                return CommonResult.NoInternet;
            }
        }

        private bool CheckWalletInfo()
        {
            if (string.IsNullOrEmpty(WalletName))
                return false;

            if (Currency.Id <= 0)
                return false;

            if (IconId <= 0)
                return false;

            if (WalletBalance < 0)
                return false;

            return true;
        }

        async public Task<CommonResult> CheckWalletExist()
        {
            IsBusy = true;

            // Get local data
            var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();

            // Get account info
            var account = await DependencyService.Get<SQLiteDBAsync>().DB.Table<Account>().FirstOrDefaultAsync();

            if (account is null)
            {
                IsBusy = false;
                return CommonResult.Fail;
            }

            var accountId = account.ServerId;

            // Get wallet
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetWalletsByAccount?accountId={accountId}";

            List<Wallet> convertedWalelts = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get currency
                    var wallets = await httpClient.GetStringAsync(getString);
                    convertedWalelts = JsonConvert.DeserializeObject<List<Wallet>>(wallets);

                    if (convertedWalelts is null || convertedWalelts.Count <= 0)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    if (!convertedWalelts.Where(wallet => wallet.Id == localData.WalletId).Any())
                    {
                        localData.WalletId = convertedWalelts.FirstOrDefault().Id;
                        await DependencyService.Get<SQLiteDBAsync>().DB.UpdateAsync(localData);
                    }
                }
            }
            catch
            {
                IsBusy = false;
                return CommonResult.NoInternet;
            }


            IsBusy = false;
            return CommonResult.Ok;
        }

        #region Getter Setter

        public string GetWalletIconUrl()
        {
            return WalletIconUrl;
        }

        public int GetIconId()
        {
            return IconId;
        }

        public void SetWalletIconUrl(string value)
        {
            WalletIconUrl = value;
        }

        public void SetIconId(int value)
        {
            IconId = value;
        }

        #endregion
    }
}
