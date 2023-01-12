using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class ChooseEventPageViewModel : BaseViewModel
    {
        #region Parent ViewModel

        // Which is add transaction page view model
        public object Parent { get; set; }
        public ForType Type { get; set; }

        #endregion

        #region Constants

        const string EVENT_LABEL_INCOME = "Thu nhập";
        const string EVENT_LABEL_OUTCOME = "Đã chi";

        #endregion

        #region Constructor

        public ChooseEventPageViewModel(object parent, ForType type)
        {
            this.Parent = parent;
            this.Type = type;
        }

        #endregion

        #region Public Properties

        public ObservableCollection<EventViewModel> Events { get; set; }

        #endregion

        #region Methods

        async public Task LoadData()
        {
            // Get api service
            var apiService = DependencyService.Get<ApiService>();

            // Get all events
            var events = await apiService.GetAllEventsAsync();

            if (events is null)
                return;

            // get transaction type to calculate balance later
            var types = await apiService.GetAllTransactionTypesAsync();

            var activeEvents = events
                .Where(e => e.IsActive)
                .ToArray();

            var activeEventViewModels = new ObservableCollection<EventViewModel>();

            foreach (var theEvent in activeEvents)
            {
                // Load event image
                var icon = await apiService.GetIconById(theEvent.IconId);
                var iconImageUrl = icon.ImageUrl;

                // Load balance
                var eventTransactions = DependencyService.Get<AppViewModel>().TransactionPageViewModel.Transactions
                    .Where(tran => tran.Transaction.EventId == theEvent.Id)
                    .ToList();

                // Calculate income
                var eventIncome = eventTransactions
                    .Where(
                        tran => types
                        .Any(type => !type.IsExpense && type.Id == tran.Transaction.EventId))
                    .Sum(tran => tran.Transaction.Amount);

                // Calculate outcome
                var eventOutCome = eventTransactions
                    .Where(
                        tran => types
                        .Any(type => type.IsExpense && type.Id == tran.Transaction.EventId))
                    .Sum(tran => tran.Transaction.Amount);

                // Calculate balance
                var balance = eventIncome - eventOutCome;

                // Decide which label will be display
                var label = "";

                if (balance > 0)
                    label = EVENT_LABEL_INCOME;
                if (balance <= 0)
                    label = EVENT_LABEL_OUTCOME;

                // Absolute balance
                balance = Math.Abs(balance);

                // Create new viewmodel
                var newViewModel = new EventViewModel
                {
                    Event = theEvent,
                    EventImageUrl = iconImageUrl,
                    BalanceLabel = label,
                    BalanceValue = balance,
                };

                // Add event to list
                activeEventViewModels.Add(newViewModel);
            }

            Events = activeEventViewModels;
        }

        #endregion

    }
}
