using DoAn_IE307_N11.Views;
using Xamarin.Forms;

namespace DoAn_IE307_N11.Services
{
    public class ViewService
    {
        public NavigationPage BuildCreateWalletPage()
        {
            var navigationPage = new NavigationPage(new CreateWalletPage());
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.FromHex("#1f1f1f");

            return navigationPage;
        }
    }
}
