using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DoAn_IE307_N11.ViewModels.All;

namespace DoAn_IE307_N11.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public string WalletIconUrl { get; set; }
        public Wallet CurrentWallet { get; set; }
        public AppViewModel ParentViewModel { get; set; }

        public HomeViewModel(AppViewModel appViewModel)
        {

            this.ParentViewModel = appViewModel;
        }

        public ICommand OpenWebCommand { get; }

        async public Task<CommonResult> GETData()
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

            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getWalletString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetWalletsByAccount?accountId={accountId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get wallet
                    var wallets = await httpClient.GetStringAsync(getWalletString);
                    var convertedWalelts = JsonConvert.DeserializeObject<List<Wallet>>(wallets);

                    if (convertedWalelts is null || convertedWalelts.Count <= 0)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    CurrentWallet = convertedWalelts.Where(wallet => wallet.Id == localData.WalletId).FirstOrDefault();
                    CurrentWallet.ServerId = CurrentWallet.Id;

                    // Get icon

                    var getIconString = $"http://{ip}/moneybook/api/ServiceController/" +
                                $"GetIconById?id={CurrentWallet.IconId}";

                    var icon = await httpClient.GetStringAsync(getIconString);
                    var convertedIcon = JsonConvert.DeserializeObject<Icon>(icon);

                    if (convertedIcon is null)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    WalletIconUrl = convertedIcon.ImageUrl;
                }
            }
            catch
            {
                IsBusy = false;
                return CommonResult.NoInternet;
            }
            return CommonResult.Ok;
        }
    }
}