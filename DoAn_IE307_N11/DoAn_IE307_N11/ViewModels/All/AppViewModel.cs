using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels.All
{
    public class AppViewModel : BaseViewModel
    {
        #region Child View Model
        
        public HomeViewModel HomeViewModel { get; set; }
        public TransactionsViewModel TransactionViewModel { get; set; }
        //public PlanViewModel PlanViewModel { get; set; }
        //public AccountViewModel AccountViewModel { get; set; } 

        #endregion

        #region Public Property

        public string WalletIconUrl { get; set; }
        public Wallet CurrentWallet { get; set; }

        #endregion


        #region Constructor

        public AppViewModel()
        {
            //HomeViewModel = new HomeViewModel(this);
            //TransactionViewModel = new TransactionsViewModel(this);
        }

        #endregion

        #region Methods


        async public Task<CommonResult> GETData()
        {
            IsBusy = true;

            var getWalletResult = await GETWallet();

            IsBusy = false;
            return CommonResult.Ok;
        }

        async private Task<CommonResult> GETWallet()
        {
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getWalletString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetAllWallets";

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

                    CurrentWallet = convertedWalelts.FirstOrDefault();

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

        #endregion
    }
}
