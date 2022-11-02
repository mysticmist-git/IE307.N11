using DoAn_IE307_N11.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;

namespace DoAn_IE307_N11.ViewModels
{
    public class TransactionsViewModel : BaseViewModel
    {
        #region Private Members
        
        private TabViewModel _currentTabVm;

        #endregion

        #region Public Properties

        public ObservableCollection<TabViewModel> TabVms { get; set; }
        public TabViewModel CurrentTabVm
        {
            get => _currentTabVm;
            set
            {
                _currentTabVm = value;
                SetSelection();
            }
        }
        public int Balance => GenerateTransaction().Sum(tran => tran.Amount);

        #endregion

        #region Constructor

        public TransactionsViewModel()
        {

            this.TabVms = new ObservableCollection<TabViewModel>();

            this.TabVms.Insert(0, new TabViewModel("TƯƠNG LAI")
            {

            });
            
            this.TabVms.Insert(0, new TabViewModel("HÔM NAY")
            {
                TransactionPods = new ObservableCollection<TransactionPod>
                {
                    new TransactionPod
                    {
                        TransactionPodType = TransactionPodType.Day,
                        DateTime = DateTime.Now,
                        Transactions = new ObservableCollection<Transaction>(GenerateTransaction().Where(t => t.DateTime.Date.Equals(DateTime.Now.Date)))
                    }
                }
            });

            this.TabVms.Insert(0, new TabViewModel("HÔM QUA")
            {
                TransactionPods = new ObservableCollection<TransactionPod>
                {
                    new TransactionPod
                    {
                        TransactionPodType = TransactionPodType.Day,
                        DateTime = DateTime.Now.AddDays(-1).Date,
                        Transactions = new ObservableCollection<Transaction>(GenerateTransaction().Where(t => t.DateTime.Date.Equals(DateTime.Now.AddDays(-1).Date)))
                    }
                }
            });

            for (int i = 2; i < 15; i++)
            {
                this.TabVms.Insert(0, new TabViewModel(String.Format("{0:dd} THÁNG {0:MM} {0:yyyy}", DateTime.Now.AddDays(-i)).ToString())
                {
                    TransactionPods = new ObservableCollection<TransactionPod>
                    {
                        new TransactionPod
                        {
                            TransactionPodType = TransactionPodType.Day,
                            DateTime = DateTime.Now.AddDays(-i).Date,
                            Transactions = new ObservableCollection<Transaction>(GenerateTransaction().Where(t => t.DateTime.Date.Equals(DateTime.Now.AddDays(-i).Date)))
                        }
                    }
                });
            }

            this.CurrentTabVm = this.TabVms[this.TabVms.Count - 2];
        }

        #endregion

        #region Private Functions

        private void SetSelection()
        {
            this.TabVms.ForEach(vm => vm.IsSelected = false);
            this.CurrentTabVm.IsSelected = true;
        }

        #endregion

        #region Statics

        private static List<Transaction> GenerateTransaction()
        {
            return new List<Transaction>
            {
                new Transaction
                {
                    DateTime = DateTime.Now,
                    Amount = 25000,
                    Id = 1,
                    Type = GenerateTransactionType().Where(type => type.Id == 1).FirstOrDefault(),
                    Description = "This is for today"
                },
                new Transaction
                {
                    DateTime = DateTime.Now,
                    Amount = 35000,
                    Id = 2,
                    Type = GenerateTransactionType().Where(type => type.Id == 2).FirstOrDefault(),
                    Description = "This is for today"
                },
                new Transaction
                {
                    DateTime = DateTime.Now,
                    Amount = -10000,
                    Id = 5,
                    Type = GenerateTransactionType().Where(type => type.Id == 1).FirstOrDefault(),
                    Description = "This is for today breakfast"
                },
                new Transaction
                {
                    DateTime = DateTime.Now.AddDays(-1),
                    Amount = 35000,
                    Id = 3,
                    Type = GenerateTransactionType().Where(type => type.Id == 1).FirstOrDefault(),
                    Description = "This is for yesterday"
                },
                new Transaction
                {
                    DateTime = DateTime.Now.AddDays(-2),
                    Amount = 45000,
                    Id = 4,
                    Type = GenerateTransactionType().Where(type => type.Id == 2).FirstOrDefault(),
                    Description = "This is for 2 days ago"
                }
            };
        }

        private static List<TransactionType> GenerateTransactionType()
        {
            return new List<TransactionType>
            {
                new TransactionType
                {
                    Id = 1,
                    Name = "Ăn uống",
                    Image = "AnUong.png"
                },
                new TransactionType
                {
                    Id = 2,
                    Name = "Di chuyển",
                    Image = "DiChuyen.png"
                }
            };
        }

        #endregion
    }
}
