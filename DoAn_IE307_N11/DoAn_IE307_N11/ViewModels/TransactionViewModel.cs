using DoAn_IE307_N11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoAn_IE307_N11.ViewModels
{
    public class TransactionViewModel : BaseViewModel
    {
        #region Private Members

        private TransactionType _transactionType;

        #endregion

        public Transaction Transaction { get; set; }
        public TransactionType TransactionType
        {
            get
            {
                if (_transactionType is null)
                    _transactionType = Services.SQLiteDB.Db.Table<TransactionType>()
                        .Where(type => type.Id == this.Transaction.TransactionTypeId)
                        .FirstOrDefault();

                if (_transactionType is null)
                    _transactionType = new TransactionType
                    {
                        Id = -1,
                        Name = "Chọn nhóm",
                        Image = "QuestionMarkIcon.png"
                    };

                return _transactionType;
            }

            set
            {
                if (value is null)
                    return;

                var transactionType = value as TransactionType;

                Transaction.TransactionTypeId = Services.SQLiteDB.Db.Table<TransactionType>()
                    .Where(tran => tran.Id == transactionType.Id)
                    .Select(tran => tran.Id)
                    .FirstOrDefault();

                _transactionType = transactionType;
            }
        }
    }
}
