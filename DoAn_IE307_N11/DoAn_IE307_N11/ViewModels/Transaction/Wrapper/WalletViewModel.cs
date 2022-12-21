using DoAn_IE307_N11.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.ViewModels
{
    public class WalletViewModel : BaseViewModel
    {
        public WalletViewModel(Wallet wallet)
        {
            Wallet = wallet;
        }

        public string WalletImageUrl { get; set; }
        public Wallet Wallet { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
