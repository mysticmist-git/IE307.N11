using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Services;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels.All;
using DoAn_IE307_N11.Views;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoAn_IE307_N11.ViewModels
{
    public enum TransactionPOSTResult
    {
        Ok,
        ZeroAmount,
        NoType,
        Fail,
        NoInternet
    }

    public class TransactionViewModel : BaseViewModel
    {
        public string TypeImage { get; set; } = "question_mark.png";
        public TransactionType Type { get; set; } = new TransactionType();
        public Models.Transaction Transaction { get; set; } = new Models.Transaction()
        {
            Date = DateTime.Now.Date,
        };
        public Wallet Wallet { get; set; }
        public Event Event { get; set; } = new Event();
        public string DateDisplayer
        {
            get
            {
                var date = Transaction.Date;

                if (date.Date.Date == DateTime.Now.Date)
                    return "Hôm nay";

                if (date.Date.Date == DateTime.Now.AddDays(-1).Date)
                    return "Hôm qua";

                return date.Date.ToString("dd/MM/yyyy");
            }
        }

        public TransactionViewModel()
        {
        }

        public TransactionViewModel(Models.Transaction tran)
        {
            this.Transaction = tran;
        }

        #region Methods

        async public Task<CommonResult> GetWalletForCreateTransaction()
        {
            IsBusy = true;

            var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();
            var walletId = localData.WalletId;

            var wallet = await DependencyService.Get<ApiService>().GetWalletById(walletId);

            if (wallet != null)
                Wallet = wallet;
            else
                Wallet = new Wallet()
                {
                    Name = "Không có ví nào",
                };

            IsBusy = false;
            CanLoadMore = false;
            return CommonResult.Ok;
        }

        //async public Task<CommonResult> GetEventForCreateTransaction()
        //{
        //    IsBusy = true;

        //    var localData = await DependencyService.Get<SQLiteDBAsync>().DB.Table<LocalData>().FirstOrDefaultAsync();
        //    var walletId = localData.WalletId;

        //    var wallet = await DependencyService.Get<ApiService>().GetWalletById(walletId);

        //    if (wallet != null)
        //        Wallet = wallet;
        //    else
        //        Wallet = new Wallet()
        //        {
        //            Name = "Không có ví nào",
        //        };

        //    IsBusy = false;
        //    CanLoadMore = false;
        //    return CommonResult.Ok;
        //}

        public void PublicOnPropertyChanged(string v)
        {
            OnPropertyChanged(v);
        }

        async public Task<TransactionPOSTResult> CreateTransaction()
        {
            IsBusy = true;

            var result = CheckTransactionInfo();
            if (result != TransactionPOSTResult.Ok)
            {
                IsBusy = false;
                return result;
            }

            int? eventId = Event is null || Event.Id <= 0 ? null : (int?)Event.Id;

            // Generate wallet model
            var newTransaction = new Models.Transaction
            {
                WalletId = Wallet.Id,
                TypeId = Type.Id,
                EventId = eventId,
                Amount = Transaction.Amount,
                Note = Transaction.Note,
                Date = Transaction.Date
            };

            var postResult = await POSTTransaction(newTransaction);

            IsBusy = false;
            return postResult;
        }


        async private Task<TransactionPOSTResult> POSTTransaction(Models.Transaction transaction)
        {
            IsBusy = true;

            if (transaction is null)
                return TransactionPOSTResult.Fail;

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var postString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"CreateTransaction";

            var myContent = JsonConvert.SerializeObject(transaction);

            // construct a content object to send this data
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            // Next, you want to set the content type to let the API know this is JSON.
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Then you can send your request very similar to your previous example with the form content:
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.PostAsync(postString, byteContent);

                    if (!result.IsSuccessStatusCode)
                    {
                        IsBusy = false;
                        return TransactionPOSTResult.Fail;
                    }

                    IsBusy = false;
                    return TransactionPOSTResult.Ok;
                }
            }
            catch
            {
                IsBusy = false;
                return TransactionPOSTResult.NoInternet;
            }
        }

        private TransactionPOSTResult CheckTransactionInfo()
        {
            if (Transaction.Amount <= 0)
                return TransactionPOSTResult.ZeroAmount;

            if (Type.Id == 0)
                return TransactionPOSTResult.NoType;

            return TransactionPOSTResult.Ok;
        }

        #endregion
    }
}
