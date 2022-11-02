using DoAn_IE307_N11.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DoAn_IE307_N11.ViewModels
{
    public enum TransactionPodType 
    {
        Day,
        Month
    }

    public class TransactionPod : BaseViewModel
    {
        public TransactionPodType TransactionPodType { get; set; } 
        public DateTime DateTime { get; set; }

        public int Income
        {
            get
            {
                if (this.TransactionPodType == TransactionPodType.Day)
                {
                    return Transactions
                        .Where(tran => tran.Amount > 0)
                        .Sum(tran => tran.Amount);
                }
                else
                {
                    return Transactions
                        .Where(tran =>
                            tran.Amount > 0
                            )
                        .Sum(tran => tran.Amount);
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
                        .Where(tran => tran.Amount < 0)
                        .Sum(tran => tran.Amount);
                }
                else
                {
                    return Transactions
                        .Where(tran =>
                            tran.Amount < 0
                            )
                        .Sum(tran => tran.Amount);
                }
            }
        }

        public int Balance => Income + Outcome;
        
        public ObservableCollection<Transaction> Transactions { get; set; }

    }
}
