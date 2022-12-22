using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels.Transaction.Wrapper;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.ViewModels
{
    public class ChooseTransactionTypeViewModel : BaseViewModel
    {
        public object Parent { get; set; }

        public ObservableCollection<TransactionTypeViewModel> TransactionTypeList { get; set; }

        public ChooseTransactionTypeViewModel(object parent)
        {
            Parent = parent;
        }

        async public Task<CommonResult> GETData()
        {
            IsBusy = true;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var ip = DependencyService.Get<ConstantService>().MY_IP;
                    var types = await httpClient.GetStringAsync($"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetAllTransactionTypes");
                    var convertedTypes = JsonConvert.DeserializeObject<List<TransactionType>>(types);

                    if (convertedTypes is null || convertedTypes.Count <= 0)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    var wrappedDatas = convertedTypes.Select(type => new TransactionTypeViewModel(type)).ToList();

                    foreach (var data in wrappedDatas)
                    {
                        // Get icon
                        var iconId = data.TransactionType.IconId;
                        var getIconString = $"http://{ip}/moneybook/api/ServiceController/" +
                                    $"GetIconById?id={iconId}";

                        var icon = await httpClient.GetStringAsync(getIconString);
                        var convertedIcon = JsonConvert.DeserializeObject<Icon>(icon);

                        if (convertedIcon is null)
                        {
                            IsBusy = false;
                            return CommonResult.Fail;
                        }


                        data.TypeImageUrl = convertedIcon.ImageUrl;
                    }

                    var currentTypeId = (Parent as TransactionViewModel).Type.Id;

                    var selectedType = wrappedDatas.Where(data => data.TransactionType.Id == currentTypeId).FirstOrDefault();


                    if (selectedType != null)
                        selectedType.IsSelected = true;

                    TransactionTypeList = new ObservableCollection<TransactionTypeViewModel>(wrappedDatas);
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
