using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanPage : ContentPage
    {
        public PlanPage()
        {
            InitializeComponent();
        }

        private void Bill_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BudgetPage());
        }

        private void Transaction_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PeriodicTransactionPage());
        }

        private void Event_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EventPage());
        }

        private void Budget_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InvoicePage());
        }
    }
}