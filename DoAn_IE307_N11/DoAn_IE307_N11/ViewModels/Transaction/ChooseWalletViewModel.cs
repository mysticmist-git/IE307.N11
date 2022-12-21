using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Forms;
using static System.Net.WebRequestMethods;

namespace DoAn_IE307_N11.ViewModels
{
    public class ChooseWalletViewModel : BaseViewModel
    {
        public object ParentViewModel { get; set; }
        public Type ParentType { get; set; }
        public ObservableCollection<WalletViewModel> WalletList { get; set; }

        public ChooseWalletViewModel(object parent)
        {
            this.ParentViewModel = parent;
            this.ParentType = parent.GetType();
        }

        async public Task<CommonResult> GETData()
        {
            IsBusy = true;

            // Get account info
            var account = await DependencyService.Get<SQLiteDBAsync>().DB.Table<Account>().FirstOrDefaultAsync();

            if (account is null)
            {
                IsBusy = false;
                return CommonResult.Fail;
            }

            var accountId = account.ServerId;

            // Get ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;

            var getString = $"http://{ip}/moneybook/api/ServiceController/GetWalletsByAccount?accountId={accountId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var datas = await httpClient.GetStringAsync(getString);
                    var convertedData = JsonConvert.DeserializeObject<List<Models.Wallet>>(datas);

                    if (convertedData is null || convertedData.Count <= 0)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    var wrappedDatas = convertedData
                        .Select(wallet => new WalletViewModel(wallet))
                        .ToList();

                    foreach (var data in wrappedDatas)
                    {
                        // Get icon
                        var iconId = data.Wallet.IconId;
                        var getIconString = $"http://{ip}/moneybook/api/ServiceController/" +
                                    $"GetIconById?id={iconId}";

                        var icon = await httpClient.GetStringAsync(getIconString);
                        var convertedIcon = JsonConvert.DeserializeObject<Icon>(icon);

                        if (convertedIcon is null)
                        {
                            IsBusy = false;
                            return CommonResult.Fail;
                        }

                        data.WalletImageUrl = convertedIcon.ImageUrl;
                        data.IsSelected = false;
                    }

                    // TODO: not dynamically
                    var parent = ParentViewModel as TransactionPageViewModel;
                    var currentWalletId = parent.ParentViewModel.HomeViewModel.CurrentWallet.ServerId;

                    wrappedDatas.Where(data => data.Wallet.Id == currentWalletId)
                        .FirstOrDefault()
                        .IsSelected = true;

                    WalletList = new ObservableCollection<WalletViewModel>(wrappedDatas);
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
    }
}
