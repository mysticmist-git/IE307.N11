using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels.All;
using DoAn_IE307_N11.Views;
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
using Xamarin.Forms.Internals;

namespace DoAn_IE307_N11.ViewModels
{
    public class TransactionPageViewModel : BaseViewModel
    {
        #region Parent
        public AppViewModel ParentViewModel { get; set; }

        #endregion

        #region Private Members

        private TabViewModel _currentTabVm;

        #endregion

        #region Public Properties

        public ObservableCollection<TransactionViewModel> Transactions { get; set; }
        public ObservableCollection<TabViewModel> TabVms { get; set; }
        public TabViewModel CurrentTabVm
        {
            get => _currentTabVm;
            set
            {
                _currentTabVm = value;
                SetSelection();
            }
        }

        #endregion

        #region Commands

        #endregion

        #region Constructor

        public TransactionPageViewModel()
        {
            GenerateTabs();
        }

        public TransactionPageViewModel(AppViewModel appViewModel)
        {
            this.ParentViewModel = appViewModel;
            GenerateTabs();
        }

        private void GenerateTabs()
        {
            this.TabVms = new ObservableCollection<TabViewModel>();

            this.TabVms.Insert(0, new TabViewModel("TƯƠNG LAI", this)
            {
                StartDate = DateTime.Now.Date.AddDays(1),
                EndDate = DateTime.MaxValue.Date,
            });

            this.TabVms.Insert(0, new TabViewModel("HÔM NAY", this)
            {
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
            });

            this.TabVms.Insert(0, new TabViewModel("HÔM QUA", this)
            {
                StartDate = DateTime.Now.Date.AddDays(-1),
                EndDate = DateTime.Now.Date.AddDays(-1),
            });

            for (int i = 2; i < 15; i++)
            {
                this.TabVms.Insert(0, new TabViewModel(String.Format("{0:dd} THÁNG {0:MM} {0:yyyy}", DateTime.Now.AddDays(-i)).ToString(), this)
                {
                    StartDate = DateTime.Now.Date.AddDays(-i),
                    EndDate = DateTime.Now.Date.AddDays(-i),
                });
            }

            this.CurrentTabVm = this.TabVms[this.TabVms.Count - 2];
        }

        #endregion

        #region Private Functions

        private void SetSelection()
        {
            this.TabVms.ForEach(vm => vm.IsSelected = false);
            this.CurrentTabVm.IsSelected = true;
        }

        #endregion

        #region Methods

        async public Task<CommonResult> GETData()
        {
            IsBusy = true;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var ip = DependencyService.Get<ConstantService>().MY_IP;
                    var walletId = ParentViewModel.HomeViewModel.CurrentWallet.Id;

                    var datas = await httpClient.GetStringAsync($"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetTransactionsByWallet?walletId={walletId}");
                    var convertedData = JsonConvert.DeserializeObject<List<Models.Transaction>>(datas);

                    if (convertedData is null || convertedData.Count <= 0)
                    {
                        IsBusy = false;
                        return CommonResult.Fail;
                    }

                    var wrappedDatas = convertedData
                        .Select(tran => new TransactionViewModel(tran))
                        .ToList();

                    foreach (var data in wrappedDatas)
                    {
                        // Get transaction type
                        var typeId = data.Transaction.TypeId;
                        var getTypeString = $"http://{ip}/moneybook/api/ServiceController/" +
                                    $"GetTransactionTypeById?id={typeId}";

                        var currency = await httpClient.GetStringAsync(getTypeString);
                        var convertedTransactionType = JsonConvert.DeserializeObject<TransactionType>(currency);

                        if (convertedTransactionType is null)
                        {
                            IsBusy = false;
                            return CommonResult.Fail;
                        }

                        data.Type = convertedTransactionType;

                        // Get icon
                        var iconId = data.Type.Id;
                        var getIconString = $"http://{ip}/moneybook/api/ServiceController/" +
                                    $"GetIconById?id={iconId}";

                        var icon = await httpClient.GetStringAsync(getIconString);
                        var convertedIcon = JsonConvert.DeserializeObject<Icon>(icon);

                        if (convertedIcon is null)
                        {
                            IsBusy = false;
                            return CommonResult.Fail;
                        }

                        data.TypeImage = convertedIcon.ImageUrl;
                    }

                    Transactions = new ObservableCollection<TransactionViewModel>(wrappedDatas);
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

        #endregion
    }
}
