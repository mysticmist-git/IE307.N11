using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using MvvmHelpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        public string BalanceLabel { get; set; }
        public int BalanceValue { get; set; }
        
        public Event Event { get; set; } = new Event();
        public string DateDisplayer
        {
            get
            {
                var date = Event.ExpiredDate;
                var returnDate = "";

                if (date.Date.Date == DateTime.Now.Date)
                    returnDate = "Hôm nay";
                else if (date.Date.Date == DateTime.Now.AddDays(-1).Date)
                    returnDate = "Hôm qua";
                else
                    returnDate = date.Date.ToString("dd/MM/yyyy");

                returnDate = returnDate + " (Ngày hết hạn)";
                return returnDate;
            }
        }

        public string EventImageUrl { get; set; }

        public void PublicOnPropretyChanged(string name)
        {
            OnPropertyChanged(name);
        }
    }
}
