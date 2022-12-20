using DoAn_IE307_N11.ViewModels.All;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.ViewModels
{
    public class EnterAmountPageForCreateWalletPageViewModel : BaseViewModel
    {
        public CreateWalletViewModel ParentViewModel { get; set; }

        public EnterAmountPageForCreateWalletPageViewModel(CreateWalletViewModel createWalletViewModel)
        {
            ParentViewModel = createWalletViewModel;
        }
    }
}
