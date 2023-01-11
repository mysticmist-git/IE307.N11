using DoAn_IE307_N11.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{

    public class TransactionPod : BaseViewModel
    {
        #region Private Members

        private ObservableCollection<TransactionViewModel> _transactions;

        #endregion

        public TransactionTabType TransactionTabType { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int Income { get; set; }
        

        public int Outcome { get; set; }
       

        public int Balance { get; set; }

        public ObservableCollection<TransactionViewModel> Transactions
        {
            get
            {
                if (_transactions is null)
                    _transactions = new ObservableCollection<TransactionViewModel>();

                return _transactions;
            }

            set => _transactions = value;
        }

        #region Methods

        public void UpdateBalance()
        {
            Income = Transactions
                        .Where(tran => !tran.Type.IsExpense)
                        .Sum(tran => tran.Transaction.Amount);

            Outcome = Transactions
                    .Where(tran => tran.Type.IsExpense)
                    .Sum(tran => tran.Transaction.Amount);

            Balance = Income - Outcome;
        }

        #endregion
    }
}
