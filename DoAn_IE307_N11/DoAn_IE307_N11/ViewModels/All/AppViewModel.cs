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

namespace DoAn_IE307_N11.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        #region Child View Model
        
        public HomeViewModel HomeViewModel { get; set; }
        public TransactionPageViewModel TransactionPageViewModel { get; set; }
        //public PlanViewModel PlanViewModel { get; set; }
        //public AccountViewModel AccountViewModel { get; set; } 

        #endregion

        #region Public Property

        #endregion


        #region Constructor

        public AppViewModel()
        {
            HomeViewModel = new HomeViewModel(this);
            TransactionPageViewModel = new TransactionPageViewModel(this);
        }

        #endregion

        #region Methods


        async public Task<CommonResult> GETData()
        {
            IsBusy = true;

            var homePageGETResult = await HomeViewModel.GETData();
            var transactionPageGETResult = await TransactionPageViewModel.GETData();

            IsBusy = false;

            if (homePageGETResult == CommonResult.NoInternet || transactionPageGETResult == CommonResult.NoInternet)
            {
                IsBusy = false;
                return CommonResult.NoInternet;
            }

            if (homePageGETResult == CommonResult.Fail || transactionPageGETResult == CommonResult.Fail)
            {
                IsBusy = false;
                return CommonResult.Fail;
            }

            return CommonResult.Ok;
        }

        #endregion
    }
}
