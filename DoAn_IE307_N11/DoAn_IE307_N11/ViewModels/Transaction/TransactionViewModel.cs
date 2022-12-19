using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class TransactionViewModel : BaseViewModel
    {
        public string TypeImage { get; set; }
        public TransactionType Type { get; set; }
        public Models.Transaction Transaction { get; set; }
        public TransactionType TransactionType { get;set; }

        #region Commands

        public ICommand OpenTransactionCommand { get; set; }

        #endregion

        public TransactionViewModel()
        {
            OpenTransactionCommand = new Command<TransactionViewModel>(async (transactionViewModel) =>
            {
                await Shell.Current.GoToAsync($"{nameof(TransactionDetailPage)}?bindingContext={transactionViewModel}");
            });
        }
    }
}
