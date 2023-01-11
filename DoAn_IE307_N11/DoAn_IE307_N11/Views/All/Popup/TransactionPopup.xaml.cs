using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionPopup : Popup
    {
        public TransactionPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Change transaction view time interval mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TimeInterval_Clicked(object sender, EventArgs e)
        {
            await Navigation.ShowPopupAsync(new TimeIntervalPopup()
            {
                Anchor = sender as Button
            });
        }
    }
}