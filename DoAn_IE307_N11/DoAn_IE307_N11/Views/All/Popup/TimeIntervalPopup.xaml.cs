using DoAn_IE307_N11.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeIntervalPopup : Popup
    {
        public TimeIntervalPopup()
        {
            InitializeComponent();
        }

        private void Day_Clicked(object sender, EventArgs e)
        {
            UpdateTransactionTab(TransactionTabType.Day);
        }


        private void Week_Clicked(object sender, EventArgs e)
        {
            UpdateTransactionTab(TransactionTabType.Week);

        }

        private void Month_Clicked(object sender, EventArgs e)
        {
            UpdateTransactionTab(TransactionTabType.Month);

        }

        private void Year_Clicked(object sender, EventArgs e)
        {
            UpdateTransactionTab(TransactionTabType.Year);

        }

        #region Methods

        private void UpdateTransactionTab(TransactionTabType type)
        {
            var appViewModel = DependencyService.Get<AppViewModel>();

            appViewModel.TransactionPageViewModel.TabType = type;

            appViewModel.TransactionPageViewModel.GenerateTabs();
            appViewModel.TransactionPageViewModel.UpdateTabItem();
        }

        #endregion
    }
}