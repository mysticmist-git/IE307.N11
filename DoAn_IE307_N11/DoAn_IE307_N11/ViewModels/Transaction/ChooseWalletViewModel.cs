using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.Views;
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
        public ForType Type{ get; set; }
        public ObservableCollection<WalletViewModel> WalletList { get; set; }
        public bool IsAddButtonVisible => Type == ForType.ForTransactionTab;
        public ChooseWalletViewModel(object parent, ForType type)
        {
            this.ParentViewModel = parent;
            this.Type = type;
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

                    switch (Type)
                    {
                        case ForType.ForTransactionTab:
                            {

                                var parent = ParentViewModel as TransactionPageViewModel;
                                var currentWalletId = parent.ParentViewModel.HomeViewModel.CurrentWallet.ServerId;

                                wrappedDatas.Where(data => data.Wallet.Id == currentWalletId)
                                    .FirstOrDefault()
                                    .IsSelected = true;

                                WalletList = new ObservableCollection<WalletViewModel>(wrappedDatas);
                            }
                            break;
                        case ForType.ForAddTransaction:
                            {
                                var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();
                                var currentWalletId = localData.WalletId;

                                wrappedDatas.Where(data => data.Wallet.Id == currentWalletId)
                                    .FirstOrDefault()
                                    .IsSelected = true;

                                WalletList = new ObservableCollection<WalletViewModel>(wrappedDatas);
                            }
                            break;
                        default:
                            break;
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
    }
}
