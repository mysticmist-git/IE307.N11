using DoAn_IE307_N11.Enums;
using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels.All;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class ChooseCurrencyViewModel : BaseViewModel
    {
        public object ParentViewModel { get; set; }
        public ForType Type { get; set; }

        public ChooseCurrencyViewModel(object parent, ForType type)
        {
            ParentViewModel = parent;
            Type = type;
        }

        public List<CurrencyViewModel> CurrencyList { get; set; } = new List<CurrencyViewModel>();
        async public Task<CommonResult> LoadCurrencies()
        {
            IsBusy = true;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var ip = DependencyService.Get<ConstantService>().MY_IP;
                    var currencies = await httpClient.GetStringAsync($"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetAllCurrencies");
                    var convertedCurrencies = JsonConvert.DeserializeObject<List<Currency>>(currencies);

                    if (convertedCurrencies is null || convertedCurrencies.Count <= 0)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    CurrencyList = convertedCurrencies
                        .Select(currency => new CurrencyViewModel(currency))
                        .ToList();
                }

                IsBusy = false;
                return CommonResult.Ok;
            }
            catch
            {
                IsBusy = false;
                return CommonResult.NoInternet;
            }

        }

        public void LoadSeletedCurrency()
        {
            switch (Type)
            {
                case ForType.ForCreateWallet:
                    {
                        var parent = ParentViewModel as CreateWalletViewModel;
                        // Get to know which one is currenyly selected
                        var selected = CurrencyList
                            .Where(currency => currency.Info.Id == parent.Currency.Id)
                            .FirstOrDefault();

                        selected.IsSelected = true;
                    }

                    break;
                case ForType.ForEditWallet:
                    {
                        var parent = ParentViewModel as EditWalletViewModel;
                        // Get to know which one is currenyly selected
                        var selected = CurrencyList
                            .Where(currency => currency.Info.Id == parent.Currency.Id)
                            .FirstOrDefault();

                        selected.IsSelected = true;
                    }

                    break;
            }


        }
    }
}
