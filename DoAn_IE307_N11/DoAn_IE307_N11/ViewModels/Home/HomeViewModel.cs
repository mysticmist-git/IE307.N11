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
        //public HomeViewModel(AppViewModel appViewModel)
        //{

        //    this.ParentViewModel = appViewModel;
        //}

        public ICommand OpenWebCommand { get; }

    }
}