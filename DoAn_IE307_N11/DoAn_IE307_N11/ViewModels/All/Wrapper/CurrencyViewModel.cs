using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class CurrencyViewModel : BaseViewModel
    {
        public CurrencyViewModel(Currency currency)
        {
            this.Info = currency;

            this.ImageUrl = DependencyService.Get<UtilsService>().GetFlagFromCode(currency.Code);
            this.CurrencySymbol = DependencyService.Get<UtilsService>()
                .GetCurrencySymbolFromCode(currency.Code);
        }

        public string ImageUrl { get; set; }
        public string CurrencySymbol { get; set; }
        public Currency Info { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
