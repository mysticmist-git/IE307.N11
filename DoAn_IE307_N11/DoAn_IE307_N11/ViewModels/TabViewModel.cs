using DoAn_IE307_N11.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;

namespace DoAn_IE307_N11.ViewModels
{
    public class TabViewModel : BaseViewModel
    {
        public TabViewModel(string title)
        {
            this.Title = title;
        }

        public bool IsSelected { get; set; }

        /// <summary>
        /// A positive number represent total income
        /// </summary>
        public int Income => TransactionPods.Sum(t => t.Income);

        /// <summary>
        /// A negative number represent total outcome
        /// </summary>
        public int Outcome => TransactionPods.Sum(t => t.Outcome);

        /// <summary>
        /// The balance
        /// </summary>
        public int Balance => Income + Outcome;

        /// <summary>
        /// A list contains all transaction in a container such as: day, week, month
        /// </summary>
        public ObservableCollection<TransactionPod> TransactionPods { get; set; } = new ObservableCollection<TransactionPod>();
    }
}
