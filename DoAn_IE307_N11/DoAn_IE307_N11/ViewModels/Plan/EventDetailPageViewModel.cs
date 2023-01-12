using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class EventDetailPageViewModel : BaseViewModel
    {
        public EventViewModel EventViewModel { get; set; } = new EventViewModel();

        #region Constructor

        public EventDetailPageViewModel(EventViewModel theEvent)
        {
            EventViewModel = theEvent;
        }

        #endregion

        #region Methods

        //async public Task LoadData(Event theEvent)
        //{
        //    // Get api service
        //    var apiService = DependencyService.Get<ApiService>();

        //    // Load Icon Url (use the first icon)
        //    // Load icons from api
        //    var icon = await apiService.GetIconById(theEvent.IconId);

        //    var firstIcon = icon;
        //    EventViewModel.EventImageUrl = firstIcon.ImageUrl;
        //    EventViewModel.Event.IconId = firstIcon.Id;

        //    // Load default expire date (which is today)
        //    EventViewModel.Event.ExpiredDate = theEvent.ExpiredDate;
        //    OnPropertyChanged(nameof(EventViewModel));
        //}

        #endregion

        #region OnPublicPropertyChanged

        public void PublicOnPropertyChanged(string name)
        {
            OnPropertyChanged(name);
        }

        #endregion
    }
}
