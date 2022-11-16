using DoAn_IE307_N11.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public enum TransactionPodType
    {
        Day,
        Month
    }

    public class TransactionPod : BaseViewModel
    {
        #region Private Members

        private ObservableCollection<TransactionViewModel> _transactions;

        #endregion

        public TransactionPodType TransactionPodType { get; set; }
        public DateTime DateTime { get; set; }

        public int Income
        {
            get
            {
                if (this.TransactionPodType == TransactionPodType.Day)
                {
                    return Transactions
                        .Where(tran => tran.Transaction.Amount > 0)
                        .Sum(tran => tran.Transaction.Amount);
                }
                else
                {
                    return Transactions
                        .Where(tran =>
                            tran.Transaction.Amount > 0
                            )
                        .Sum(tran => tran.Transaction.Amount);
                }
            }
        }

        public int Outcome
        {
            get
            {
                if (this.TransactionPodType == TransactionPodType.Day)
                {
                    return Transactions
                        .Where(tran => tran.Transaction.Amount < 0)
                        .Sum(tran => tran.Transaction.Amount);
                }
                else
                {
                    return Transactions
                        .Where(tran =>
                            tran.Transaction.Amount < 0
                            )
                        .Sum(tran => tran.Transaction.Amount);
                }
            }
        }

        public int Balance => Income + Outcome;

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
    }
}
