using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class NewTransactionViewModel : BaseViewModel
    {
        #region Private Members

        private readonly Interfaces.IMessageService _messageService;
        private ObservableCollection<TransactionType> _transactionTypes;
        private ObservableCollection<Wallet> _wallets;

        #endregion

        #region Public Properties

        public TransactionViewModel Transaction { get; set; }
        public ObservableCollection<TransactionType> TransactionTypes
        {
            get
            {
                if (_transactionTypes is null)
                {
                    _transactionTypes = new ObservableCollection<TransactionType>(
                        Services.SQLiteDB.Db.Table<TransactionType>().ToArray());

                    _transactionTypes.Insert(0, new TransactionType
                    {
                        Id = -1,
                        Name = "Chọn nhóm",
                        Image = "QuestionMarkIcon.png"
                    });
                }
                    

                return _transactionTypes;
            }
        }
        public ObservableCollection<Wallet> Wallets
        {
            get
            {
                if (_wallets is null)
                    _wallets = new ObservableCollection<Wallet>(
                        Services.SQLiteDB.Db.Table<Wallet>().ToArray());

                return _wallets;
            }
        }

        #endregion

        #region Commands

        public ICommand InsertTransactionCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        #endregion

        public NewTransactionViewModel()
        {
            this._messageService = DependencyService.Get<Interfaces.IMessageService>();

            Transaction = new TransactionViewModel
            {
                Transaction = new Transaction
                {
                    TransactionTypeId = -1,
                    Amount = 0,
                    DateTime = DateTime.Now,
                }
            };

            InsertTransactionCommand = new Command<TransactionViewModel>(InsertTransaction);
            CancelCommand = new Command(OnCancel);
        }

        #region Functions

        public async void InsertTransaction(TransactionViewModel transaction)
        {
            if (transaction is null)
            {
                await Task.Run(() => ShowInsertTransactionResult(false));
                await Task.Run(() => OnCancel());

                return;
            }

            var insertValue = transaction;

            if (insertValue.Transaction.TransactionTypeId == -1)
            {
                await Task.Run(() => ShowInsertTransactionResult(false));
                await Task.Run(() => OnCancel());
                return;
            }

            if (insertValue.TransactionType.IsIncome == false)
                insertValue.Transaction.Amount = -insertValue.Transaction.Amount;

            int result = await SQLiteAsyncDB.Db.InsertAsync(insertValue.Transaction);

            await Task.Run(() => ShowInsertTransactionResult(result > 0));

            await Task.Run(() => OnCancel());
        }

        #endregion

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void ShowInsertTransactionResult(bool result)
        {
            if (result)
                await this._messageService.ShowAsync("Thêm giao dịch thành công");
            else
                await this._messageService.ShowAsync("Thêm giao dịch thất bại");
        }
    }
}
