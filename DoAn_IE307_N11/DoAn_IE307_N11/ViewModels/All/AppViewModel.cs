using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DoAn_IE307_N11.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        #region Child View Model

        public HomeViewModel HomeViewModel { get; set; }
        public TransactionPageViewModel TransactionPageViewModel { get; set; }
        //public PlanViewModel PlanViewModel { get; set; }
        public AccountViewModel AccountViewModel { get; set; }
        public EventPageViewModel EventPageViewModel { get; set; }
        #endregion

        #region Public Property

        public bool IsAppBusy => HomeViewModel.IsBusy || TransactionPageViewModel.IsBusy || AccountViewModel.IsBusy;

        #endregion


        #region Constructor

        public AppViewModel() 
        {
            HomeViewModel = new HomeViewModel(this);
            TransactionPageViewModel = new TransactionPageViewModel(this);
            AccountViewModel = new AccountViewModel(this);
            EventPageViewModel = new EventPageViewModel(this);
        }

        #endregion

        #region Methods

        async public Task<CommonResult> GETData()
        {
            //IsBusy = true;
            HomeViewModel.IsBusy = true;
            OnPropertyChanged(nameof(IsAppBusy));
            var homePageGETResult = await HomeViewModel.GETData();
            OnPropertyChanged(nameof(IsAppBusy));

            TransactionPageViewModel.IsBusy = true;
            OnPropertyChanged(nameof(IsAppBusy));
            var transactionPageGETResult = await TransactionPageViewModel.GETData();
            OnPropertyChanged(nameof(IsAppBusy));

            HomeViewModel.UpdateRecentTransactions();

            await EventPageViewModel.LoadData();

            // Update events status (active or not active)
            await UpdateEventsStatus();

            //IsBusy = false;

            if (homePageGETResult == CommonResult.NoInternet || transactionPageGETResult == CommonResult.NoInternet)
            {
                //IsBusy = false;
                Application.Current.MainPage = DependencyService.Get<ViewService>().BuildCreateWalletPage(Enums.ForType.ForOriginalCreateWallet);
                return CommonResult.NoInternet;
            }

            if (homePageGETResult == CommonResult.Fail || transactionPageGETResult == CommonResult.Fail)
            {
                //IsBusy = false;
                Application.Current.MainPage = DependencyService.Get<ViewService>().BuildCreateWalletPage(Enums.ForType.ForOriginalCreateWallet);
                return CommonResult.Fail;
            }

            OnPropertyChanged(nameof(TransactionPageViewModel));
            OnPropertyChanged(nameof(HomeViewModel));

            return CommonResult.Ok;
        }

        #endregion

        public void PublicOnPropertyChanged(string name)
        {
            OnPropertyChanged(name);
        }

        async public Task UpdateEventsStatus()
        {
            var apiService = DependencyService.Get<ApiService>();
            await apiService.UpdateEventsStatusAsync();
        }
    }
}
