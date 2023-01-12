using DoAn_IE307_N11.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class AddEventPageViewModel : BaseViewModel
    {
        public EventViewModel EventViewModel { get; set; } = new EventViewModel();

        #region Methods

        async public Task LoadDefaultData()
        {
            // Get api service
            var apiService = DependencyService.Get<ApiService>();

            // Load Icon Url (use the first icon)
            // Load icons from api
            var icons = await apiService.GetAllIconsAsync();

            if (icons is null || icons.Count <= 0)
                EventViewModel.EventImageUrl = "DiChuyen.png";
            else
            {
                var firstIcon = icons.FirstOrDefault();
                EventViewModel.EventImageUrl = firstIcon.ImageUrl;
                EventViewModel.Event.IconId = firstIcon.Id;
            }

            // Load default expire date (which is today)
            EventViewModel.Event.ExpiredDate = DateTime.Now.Date;
            OnPropertyChanged(nameof(EventViewModel));
        }

        #endregion

        #region OnPublicPropertyChanged

        public void PublicOnPropertyChanged(string name)
        {
            OnPropertyChanged(name);
        }

        #endregion

    }
}
