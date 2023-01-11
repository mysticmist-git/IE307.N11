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
    public enum TransactionTabType
    {
        Day,
        Week,
        Month,
        Year,
    }

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

        public TransactionTabType TabType { get; set; } = TransactionTabType.Day;

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

        public void GenerateTabs()
        {
            switch (TabType)
            {
                case TransactionTabType.Day:
                    GenerateDayTabs();
                    break;
                case TransactionTabType.Week:
                    GenerateWeekTabs();
                    break;
                case TransactionTabType.Month:
                    GenerateMonthTabs();
                    break;
                //case TransactionTabType.Quarter:
                //    GenerateYearTabs();
                //    break;
                case TransactionTabType.Year:
                    GenerateYearTabs();
                    break;
            }
        }

        #region Generate Tabs

        #endregion

        /// <summary>
        /// Generate day tabs
        /// </summary>
        private void GenerateDayTabs()
        {
            this.TabVms = new ObservableCollection<TabViewModel>();

            //this.TabVms.Add(new TabViewModel("TƯƠNG LAI", this)
            //{
            //    StartDate = DateTime.Now.Date.AddDays(1),
            //    EndDate = DateTime.MaxValue.Date,
            //});

            this.TabVms.Add(new TabViewModel("HÔM NAY", this)
            {
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
                    TabType = this.TabType
            });

            this.TabVms.Add(new TabViewModel("HÔM QUA", this)
            {
                StartDate = DateTime.Now.Date.AddDays(-1),
                EndDate = DateTime.Now.Date.AddDays(-1),
                    TabType = this.TabType
            });

            for (int i = 2; i < 15; i++)
            {
                this.TabVms.Add(new TabViewModel(String.Format("{0:dd} THÁNG {0:MM} {0:yyyy}", DateTime.Now.AddDays(-i)).ToString(), this)
                {
                    StartDate = DateTime.Now.Date.AddDays(-i),
                    EndDate = DateTime.Now.Date.AddDays(-i),
                    TabType = this.TabType
                });
            }

            //this.CurrentTabVm = this.TabVms.FirstOrDefault();
        }

        /// <summary>
        /// Generate week tabs
        /// </summary>
        private void GenerateWeekTabs()
        {
            this.TabVms = new ObservableCollection<TabViewModel>();

            //this.TabVms.Add(new TabViewModel("TƯƠNG LAI", this)
            //{
            //    StartDate = DateTime.Now.Date.AddDays(1),
            //    EndDate = DateTime.MaxValue.Date,
            //});

            this.TabVms.Add(new TabViewModel("TUẦN NÀY", this)
            {
                StartDate = DateTimeExtensions.StartOfWeek(DateTime.Now, DayOfWeek.Monday),
                EndDate = DateTimeExtensions.StartOfWeek(DateTime.Now, DayOfWeek.Monday).AddDays(6),
                TabType = this.TabType
            });

            this.TabVms.Add(new TabViewModel("TUẦN TRƯỚC", this)
            {
                StartDate = DateTimeExtensions.StartOfWeek(DateTime.Now.AddDays(-7), DayOfWeek.Monday),
                EndDate = DateTimeExtensions.StartOfWeek(DateTime.Now.AddDays(-7), DayOfWeek.Monday).AddDays(6),
                TabType = this.TabType
            });

            for (int i = 2; i < 15; i++)
            {
                var newTabViewModel = new TabViewModel("", this)
                {
                    StartDate = DateTimeExtensions.StartOfWeek(DateTime.Now.AddDays(-i * 7), DayOfWeek.Monday),
                    EndDate = DateTimeExtensions.StartOfWeek(DateTime.Now.AddDays(-i * 7), DayOfWeek.Monday).AddDays(6),
                    TabType = this.TabType
                };

                newTabViewModel.Title = String.Format("{0:dd}/{0:MM} - {1:dd}/{1:MM}", newTabViewModel.StartDate, newTabViewModel.EndDate);

                this.TabVms.Add(newTabViewModel);
            }

            //this.CurrentTabVm = this.TabVms.FirstOrDefault();
        }

        /// <summary>
        /// Generate week tabs
        /// </summary>
        private void GenerateMonthTabs()
        {
            this.TabVms = new ObservableCollection<TabViewModel>();

            //this.TabVms.Add(new TabViewModel("TƯƠNG LAI", this)
            //{
            //    StartDate = DateTime.Now.Date.AddDays(1),
            //    EndDate = DateTime.MaxValue.Date,
            //});

            this.TabVms.Add(new TabViewModel("THÁNG NÀY", this)
            {
                StartDate = DateTimeExtensions.firstDayOfMonth(DateTime.Now),
                EndDate = DateTimeExtensions.lastDayOfMonth(DateTime.Now),
                TabType = this.TabType
            });

            this.TabVms.Add(new TabViewModel("THÁNG TRƯỚC", this)
            {
                StartDate = DateTimeExtensions.firstDayOfMonth(DateTime.Now.AddMonths(-1)),
                EndDate = DateTimeExtensions.lastDayOfMonth(DateTime.Now.AddMonths(-1)),
                TabType = this.TabType
            });

            for (int i = 2; i < 15; i++)
            {
                var newTabViewModel = new TabViewModel("", this)
                {
                    StartDate = DateTimeExtensions.firstDayOfMonth(DateTime.Now.AddMonths(-i)),
                    EndDate = DateTimeExtensions.lastDayOfMonth(DateTime.Now.AddMonths(-i)),
                    TabType = this.TabType
                };

                newTabViewModel.Title = String.Format("{0:MM}/{0:yyyy}", newTabViewModel.StartDate);

                this.TabVms.Add(newTabViewModel);
            }

            //this.CurrentTabVm = this.TabVms.FirstOrDefault();
        }

        /// <summary>
        /// Generate week tabs
        /// </summary>
        private void GenerateYearTabs()
        {
            this.TabVms = new ObservableCollection<TabViewModel>();

            //this.TabVms.Add(new TabViewModel("TƯƠNG LAI", this)
            //{
            //    StartDate = DateTime.Now.Date.AddDays(1),
            //    EndDate = DateTime.MaxValue.Date,
            //});

            this.TabVms.Add(new TabViewModel("NĂM NÀY", this)
            {
                StartDate = DateTimeExtensions.firstDayOfYear(DateTime.Now),
                EndDate = DateTimeExtensions.lastDayOfYear(DateTime.Now),
                TabType = this.TabType
            });

            this.TabVms.Add(new TabViewModel("NĂM TRƯỚC", this)
            {
                StartDate = DateTimeExtensions.firstDayOfYear(DateTime.Now.AddYears(-1)),
                EndDate = DateTimeExtensions.lastDayOfYear(DateTime.Now.AddYears(-1)),
                TabType = this.TabType
            });

            for (int i = 2; i < 15; i++)
            {
                var newTabViewModel = new TabViewModel("", this)
                {
                    StartDate = DateTimeExtensions.firstDayOfYear(DateTime.Now.AddYears(-i)),
                    EndDate = DateTimeExtensions.lastDayOfYear(DateTime.Now.AddYears(-i)),
                    TabType = this.TabType
                };

                newTabViewModel.Title = String.Format("{0:yyyy}", newTabViewModel.StartDate);

                this.TabVms.Add(newTabViewModel);
            }

            //this.CurrentTabVm = this.TabVms.FirstOrDefault();
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

            // Get local data
            var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var ip = DependencyService.Get<ConstantService>().MY_IP;
                    var walletId = localData.WalletId;

                    var datas = await httpClient.GetStringAsync($"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetTransactionsByWallet?walletId={walletId}");
                    var convertedData = JsonConvert.DeserializeObject<List<Models.Transaction>>(datas);

                    if (convertedData is null)
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
                    UpdateTabItem();
                    OnPropertyChanged(nameof(Transactions));
                    OnPropertyChanged(nameof(TabVms));

                    IsBusy = false;
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

        public void UpdateTabItem()
        {
            foreach (var item in TabVms)
            {
                item.LoadTransactions();
            }
        }

        #endregion
    }
}
