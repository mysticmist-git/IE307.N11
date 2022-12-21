using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.ViewModels.All;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.ViewModels
{
    public class EnterAmountViewModel : BaseViewModel
    {
        public object ParentViewModel { get; set; }
        public ForType Type { get; set; }
        public EnterAmountViewModel(object createWalletViewModel, ForType type)
        {
            ParentViewModel = createWalletViewModel;
            this.Type = type;
        }
    }
}
