using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
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
        public int Balance
        {
            get
            {
                var balance = SQLiteDB.Db.Table<Transaction>().Sum(tran => tran.Amount);
                return balance;
            }
        }

        #endregion

        #region Commands

        public Command AddTransactionCommand { get; }

        #endregion

        #region Constructor

        public TransactionsViewModel()
        {
            // Init commands
            AddTransactionCommand = new Command(OnAddTransaction);

            this.TabVms = new ObservableCollection<TabViewModel>();

            this.TabVms.Insert(0, new TabViewModel("TƯƠNG LAI")
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(1),
            });

            this.TabVms.Insert(0, new TabViewModel("HÔM NAY")
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            });

            this.TabVms.Insert(0, new TabViewModel("HÔM QUA") 
            { 
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(-1),
            });

            for (int i = 2; i < 15; i++)
            {
                this.TabVms.Insert(0, new TabViewModel(String.Format("{0:dd} THÁNG {0:MM} {0:yyyy}", DateTime.Now.AddDays(-i)).ToString()));
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

        private async void OnAddTransaction(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewTransactionPage));
        }
    }
}
