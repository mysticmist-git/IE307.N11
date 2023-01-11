using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using MvvmHelpers;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Transactions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Transaction = DoAn_IE307_N11.Models.Transaction;

namespace DoAn_IE307_N11.ViewModels
{

    public class TabViewModel : BaseViewModel
    {
        #region Parent

        public TransactionPageViewModel ParentViewModel { get; set; }

        #endregion

        #region Private Members

        private ObservableCollection<TransactionPod> _transactionPods;

        #endregion

        public TabViewModel(string title = "", TransactionPageViewModel transactionPageViewModel = null)
        {
            this.Title = title;
            ParentViewModel = transactionPageViewModel;
        }

        public bool IsSelected { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// A positive number represent total income
        /// </summary>
        public int Income => TransactionPods is null ? 0 : TransactionPods.Sum(t => t.Income);

        /// <summary>
        /// A negative number represent total outcome
        /// </summary>
        public int Outcome => TransactionPods is null ? 0 : TransactionPods.Sum(t => t.Outcome);

        /// <summary>
        /// The balance
        /// </summary>
        public int Balance => Income - Outcome;

        /// <summary>
        /// A list contains all transaction in a container such as: day, week, month
        /// </summary>
        public ObservableCollection<TransactionPod> TransactionPods { get; set; }
        //{
        //    get
        //    {
        //        if (_transactionPods is null)
        //            LoadTransactions();

        //        return _transactionPods;
        //    }

        //    set => _transactionPods = value;
        //}

        public bool HaveTransaction => TransactionPods != null && TransactionPods.Count > 0;

        #region Private Functions

        public TransactionTabType TabType { get; set; }
        public bool IsYearTabType => TabType == TransactionTabType.Year;

        public void LoadTransactions()
        {
            //    List<Models.Transaction> transactions = null;

            //    using (var httpClient = new HttpClient())
            //    {
            //        var ip = DependencyService.Get<ConstantService>().MY_IP;
            //        var walletId = 1;

            //        var datas = await httpClient.GetStringAsync($"http://{ip}/moneybook/api/ServiceController/GetTransactionsByWallet?walletId={walletId}");
            //        var convertedData = JsonConvert.DeserializeObject<List<Models.Transaction>>(datas);

            //        transactions = convertedData;
            //    }

            TransactionPods = new ObservableCollection<TransactionPod>();

            if (ParentViewModel is null || ParentViewModel.Transactions is null)
            {
                return;
            }


            var temp = ParentViewModel.Transactions
                .Where(tran =>
                    tran.Transaction.Date.Date >= this.StartDate.Date &&
                    tran.Transaction.Date.Date <= this.EndDate.Date
                ).ToArray();

            temp = temp.OrderByDescending(tran => tran.Transaction.Date).ToArray();

            if (temp.Count() <= 0)
            {
                return;
            }

            switch (TabType)
            {
                case TransactionTabType.Day:
                    {
                        var transactionPod = new TransactionPod
                        {
                            TransactionTabType = TabType,
                            DateTime = temp.First().Transaction.Date,
                        };

                        transactionPod.Transactions = new ObservableCollection<TransactionViewModel>(temp);

                        transactionPod.UpdateBalance();

                        TransactionPods.Add(transactionPod);
                    }
                    break;
                case TransactionTabType.Week:
                    {
                        foreach (var transaction in temp)
                        {
                            var transactionPod = TransactionPods
                                .Where(pod => pod.DateTime.Date == transaction.Transaction.Date.Date)
                                .FirstOrDefault();

                            if (transactionPod is null)
                            {
                                transactionPod = new TransactionPod
                                {
                                    TransactionTabType = TabType,
                                    DateTime = transaction.Transaction.Date
                                };

                                TransactionPods.Add(transactionPod);
                            }

                            transactionPod.Transactions.Add(transaction);

                            transactionPod.UpdateBalance();
                        }


                        //transactionPod.Transactions = new ObservableCollection<TransactionViewModel>(temp);

                        //TransactionPods.Add(transactionPod);
                    }
                    break;
                case TransactionTabType.Month:
                    {
                        foreach (var transaction in temp)
                        {
                            var transactionPod = TransactionPods
                                .Where(pod => pod.DateTime.Date == transaction.Transaction.Date.Date)
                                .FirstOrDefault();

                            if (transactionPod is null)
                            {
                                transactionPod = new TransactionPod
                                {
                                    TransactionTabType = TabType,
                                    DateTime = transaction.Transaction.Date
                                };

                                TransactionPods.Add(transactionPod);
                            }

                            transactionPod.Transactions.Add(transaction);

                            transactionPod.UpdateBalance();
                        }
                        break;
                    }
                case TransactionTabType.Year:
                    {
                        foreach (var transaction in temp)
                        {
                            var transactionPod = TransactionPods
                                .Where(
                                    pod => 
                                    transaction.Transaction.Date.Date >= pod.DateTime.Date &&
                                    transaction.Transaction.Date.Date <= pod.EndDateTime.Date 

                                )
                                .FirstOrDefault();

                            if (transactionPod is null)
                            {
                                transactionPod = new TransactionPod
                                {
                                    TransactionTabType = TabType,
                                    DateTime = DateTimeExtensions.firstDayOfMonth(transaction.Transaction.Date),
                                    EndDateTime = DateTimeExtensions.lastDayOfMonth(transaction.Transaction.Date),
                                };

                                TransactionPods.Add(transactionPod);
                            }

                            transactionPod.Transactions.Add(transaction);

                            transactionPod.UpdateBalance();
                        }
                        break;
                    }
            }

            OnPropertyChanged(nameof(TransactionPods));
            OnPropertyChanged(nameof(HaveTransaction));
            OnPropertyChanged(nameof(Income));
            OnPropertyChanged(nameof(Outcome));
            OnPropertyChanged(nameof(Balance));
        }

        #endregion
    }
}
