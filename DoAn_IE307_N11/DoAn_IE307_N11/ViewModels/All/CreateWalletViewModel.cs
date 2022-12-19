using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels.All
{
    public class CreateWalletViewModel : BaseViewModel
    {
        public string WalletIconUrl { get; set; } = "TienMat.png";
        public int IconId { get; set; }
        public string WalletName { get; set; } = "Tiền mặt";
        public Currency Currency { get; set; } = new Currency();
        public int WalletBalance { get; set; }

        async public Task<CommonResult> LoadDefaultValue()
        {
            var vietnamCurrencyCode = "vnd";
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetCurrencyByCode?code={vietnamCurrencyCode}";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var currency = await httpClient.GetStringAsync(getString);
                    var convertedCurrency = JsonConvert.DeserializeObject<Currency>(currency);

                    if (convertedCurrency is null)
                    {
                        return CommonResult.Fail;
                    }

                    Currency = convertedCurrency;
                }

                return CommonResult.Ok;
            }
            catch
            {
                return CommonResult.NoInternet;
            }
        }
    }
}
