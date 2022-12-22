using DoAn_IE307_N11.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.ViewModels.Transaction.Wrapper
{
    public class TransactionTypeViewModel : BaseViewModel
    {
        public string TypeImageUrl { get; set; }
        public TransactionType TransactionType { get; set; } = new TransactionType();

        public bool IsSelected { get; set; } = false;
        public TransactionTypeViewModel()
        {

        }

        public TransactionTypeViewModel(TransactionType type)
        {
            TransactionType = type;
        }
    }
}
