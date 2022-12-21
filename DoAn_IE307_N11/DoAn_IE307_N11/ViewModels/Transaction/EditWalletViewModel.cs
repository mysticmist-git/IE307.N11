using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MvvmHelpers;
using System.Data;

namespace DoAn_IE307_N11.ViewModels
{
    public class EditWalletViewModel : BaseViewModel
    {
        public ChooseWalletViewModel ParentViewModel { get; set; }
        public EditWalletViewModel(ChooseWalletViewModel parentViewModel, WalletViewModel walletInfo)
        {
            //this.ParentViewModel = parentViewModel;
            //this.WalletIconUrl = walletInfo.WalletImageUrl;
            //this.IconId = walletInfo.Wallet.IconId;
            //this.WalletName = walletInfo.Wallet.Name;
            //// Load currency please
            //this.WalletBalance = walletInfo.Wallet.Balance;

            Wallet = walletInfo;
        }
        public WalletViewModel Wallet { get; set; }
        public Currency Currency { get; set; } = new Currency();
        //public string DisplayWalletBalance => WalletBalance == 0 ? "Số dư" : WalletBalance.ToString();
        public bool IsZeroBalance => Wallet.Wallet.Balance == 0;

        #region Methods

        async public Task<CommonResult> LoadCurrency(int currencyId)
        {
            IsBusy = true;

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getCurrencyString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetCurrencyById?id={currencyId}";
            
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

        #endregion

        private bool CheckWalletInfo()
        {
            if (string.IsNullOrEmpty(Wallet.Wallet.Name))
                return false;

            if (Currency.Id <= 0)
                return false;

            if (Wallet.Wallet.IconId <= 0)
                return false;

            if (Wallet.Wallet.Balance < 0)
                return false;

            return true;
        }

        public async Task<CommonResult> DELETEWallet()
        {
            IsBusy = true;
            var result = await DELETEWalletApi();
            IsBusy = false;
            return result;
        }

        public async Task<CommonResult> DELETEWalletApi()
        {
            // get wallet id
            var apiService = DependencyService.Get<ApiService>();

            var id = Wallet.Wallet.Id;
            var transactionIds = await apiService.GetAllTransactionIdByWallet(id);

            // Delete acquaintances
            if (transactionIds != null && transactionIds.Count > 0)
            {
                foreach (var tranId in transactionIds)
                {
                    if (!await apiService.DeleteAllAcquaintance_TransactionByTransaction((int)tranId))
                    {
                        return CommonResult.Fail;
                    }
                } 
            }

            if (!await apiService.DeleteAllTransactionByWallet(id))
            {
                return CommonResult.Fail;
            }

            if (!await apiService.DeleteWallet(id))
            {
                return CommonResult.Fail;
            }

            return CommonResult.Ok;
        }

        public void PublicOnPropertyChanged(string propretyName)
        {
            OnPropertyChanged(propretyName);
        }
    }
}


//public async Task<CommonResult> CreateWallet()
//{
//    IsBusy = true;

//    var result = CheckWalletInfo();
//    if (!result)
//    {
//        IsBusy = false;
//        return CommonResult.Fail;
//    }

//    // Get account info
//    var account = await DependencyService.Get<SQLiteDBAsync>().DB.Table<Account>().FirstOrDefaultAsync();

//    if (account is null)
//    {
//        IsBusy = false;
//        return CommonResult.Fail;
//    }

//    var accountId = account.ServerId;

//    // Generate wallet model
//    Wallet newWallet = new Wallet
//    {
//        CurrencyId = Currency.Id,
//        AccountId = accountId,
//        IconId = IconId,
//        Name = WalletName,
//        Balance = WalletBalance
//    };

//    var postResult = await POSTWallet(newWallet);

//    IsBusy = false;
//    return postResult;
//}

//async private Task<CommonResult> POSTWallet(Wallet newWallet)
//{
//    IsBusy = true;

//    if (newWallet is null)
//        return CommonResult.Fail;

//    var ip = DependencyService.Get<ConstantService>().MY_IP;
//    var postString = $"http://{ip}/moneybook/api/ServiceController/" +
//                $"CreateWallet";

//    var myContent = JsonConvert.SerializeObject(newWallet);

//    // construct a content object to send this data
//    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
//    var byteContent = new ByteArrayContent(buffer);

//    // Next, you want to set the content type to let the API know this is JSON.
//    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

//    // Then you can send your request very similar to your previous example with the form content:
//    try
//    {
//        using (var httpClient = new HttpClient())
//        {
//            var result = await httpClient.PostAsync(postString, byteContent);

//            if (!result.IsSuccessStatusCode)
//            {
//                IsBusy = false;
//                return CommonResult.Fail;
//            }

//            IsBusy = false;
//            return CommonResult.Ok;
//        }
//    }
//    catch
//    {
//        IsBusy = false;
//        return CommonResult.NoInternet;
//    }
//}



//async public Task<CommonResult> CheckWalletExist()
//{
//    IsBusy = true;

//    // Get local data
//    var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();

//    // Get account info
//    var account = await DependencyService.Get<SQLiteDBAsync>().DB.Table<Account>().FirstOrDefaultAsync();

//    if (account is null)
//    {
//        IsBusy = false;
//        return CommonResult.Fail;
//    }

//    var accountId = account.ServerId;

//    // Get wallet
//    var ip = DependencyService.Get<ConstantService>().MY_IP;
//    var getString = $"http://{ip}/moneybook/api/ServiceController/" +
//                $"GetWalletsByAccount?accountId={accountId}";

//    List<Wallet> convertedWalelts = null;

//    try
//    {
//        using (var httpClient = new HttpClient())
//        {
//            // Get currency
//            var wallets = await httpClient.GetStringAsync(getString);
//            convertedWalelts = JsonConvert.DeserializeObject<List<Wallet>>(wallets);

//            if (convertedWalelts is null || convertedWalelts.Count <= 0)
//            {
//                IsBusy = false;
//                return CommonResult.Fail;
//            }

//            if (!convertedWalelts.Where(wallet => wallet.Id == localData.WalletId).Any())
//            {
//                localData.WalletId = convertedWalelts.FirstOrDefault().Id;
//                await DependencyService.Get<SQLiteDBAsync>().DB.UpdateAsync(localData);
//            }
//        }
//    }
//    catch
//    {
//        IsBusy = false;
//        return CommonResult.NoInternet;
//    }


//    IsBusy = false;
//    return CommonResult.Ok;
//}