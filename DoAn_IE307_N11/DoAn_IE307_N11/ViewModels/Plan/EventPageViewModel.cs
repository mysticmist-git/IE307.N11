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
    public class EventPageViewModel : BaseViewModel
    {
        #region Parent
        public AppViewModel ParentViewModel { get; set; }

        #endregion

        #region Constants

        const string EVENT_LABEL_INCOME = "Thu nhập";
        const string EVENT_LABEL_OUTCOME = "Đã chi";

        #endregion

        #region Constructor

        public EventPageViewModel(AppViewModel parent)
        {
            ParentViewModel = parent;
        }

        #endregion

        #region Public Properties

        public ObservableCollection<EventViewModel> ActiveEvents { get; set; }
        public ObservableCollection<EventViewModel> FinishedEvent { get; set; }

        #endregion

        #region Methods

        async public Task LoadData()
        {
            var apiService = DependencyService.Get<ApiService>();

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
                        .Any(type => (!type.IsExpense) && type.Id == tran.Transaction.TypeId))
                    .Sum(tran => tran.Transaction.Amount);

                // Calculate outcome
                var eventOutCome = eventTransactions
                    .Where(
                        tran => types
                        .Any(type => (type.IsExpense) && type.Id == tran.Transaction.TypeId))
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

            var finishedEvents = events
                .Where(e => !e.IsActive)
                .ToArray();

            var finishedEventViewModels = new ObservableCollection<EventViewModel>();

            foreach (var theEvent in finishedEvents)
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
                        .Any(type => (!type.IsExpense) && type.Id == tran.Transaction.TypeId))
                    .Sum(tran => tran.Transaction.Amount);

                // Calculate outcome
                var eventOutCome = eventTransactions
                    .Where(
                        tran => types
                        .Any(type => (type.IsExpense) && type.Id == tran.Transaction.TypeId))
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
                finishedEventViewModels.Add(newViewModel);
            }

            ActiveEvents = activeEventViewModels;
            FinishedEvent = finishedEventViewModels;
        }

        public void OnPublicPropertyChange(string v)
        {
            OnPropertyChanged(v);
        }

        #endregion

    }
}
