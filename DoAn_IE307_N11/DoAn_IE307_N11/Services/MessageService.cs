using System.Threading.Tasks;
using DoAn_IE307_N11.Interfaces;
using Xamarin.Forms;

namespace DoAn_IE307_N11.Services
{
    public class MessageService : IMessageService
    {
        public async Task<bool> ShowAskAsync(string message)
        {
            bool result = false;

            Device.BeginInvokeOnMainThread(async () => 
                result = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Warning", message, "Yes", "No"));

            return result;
        }

        public async Task ShowAsync(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", message, "Ok"));
        }

    }
}
