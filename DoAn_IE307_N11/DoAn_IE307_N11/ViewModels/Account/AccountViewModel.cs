using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        #region Parent
        public AppViewModel ParentViewModel { get; set; }

        #endregion
        public AccountViewModel(AppViewModel appViewModel)
        {
            ParentViewModel = appViewModel;
        }
    }
}
