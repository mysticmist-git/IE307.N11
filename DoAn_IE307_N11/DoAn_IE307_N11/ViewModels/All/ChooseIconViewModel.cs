using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels.Transaction.Wrapper;
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
    public class ChooseIconViewModel : BaseViewModel
    {
        public CreateWalletViewModel ParentViewModel { get; set; }
        private FlexLayout FlexLayout { get; set; }
        public List<Icon> IconList { get; set; } = new List<Icon>();


        /// <summary>
        /// This construction is bad, real bad!
        /// </summary>
        /// <param name="flex"></param>
        public ChooseIconViewModel(FlexLayout flex)
        {
            this.FlexLayout = flex;
        }

        /// <summary>
        /// 1: Ok
        /// 2: Fail
        /// 3: No internet
        /// </summary>
        /// <returns></returns>
        async public Task<CommonResult> GETIcon()
        {
            IsBusy = true;
            
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var ip = DependencyService.Get<ConstantService>().MY_IP;
                    var icons = await httpClient.GetStringAsync($"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetAllIcons");
                    var convertedIcons = JsonConvert.DeserializeObject<List<Icon>>(icons);

                    if (convertedIcons is null || convertedIcons.Count <= 0)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    IconList = convertedIcons;
                }
            }
            catch
            {
                IsBusy = false;
                return CommonResult.NoInternet;
            }

            IsBusy = false;
            return CommonResult.Ok;
        }
    }
}
