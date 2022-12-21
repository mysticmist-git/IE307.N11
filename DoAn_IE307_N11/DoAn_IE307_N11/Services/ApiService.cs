using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.Services
{
    public class ApiService
    {
        #region GET

        async public Task<List<int?>> GetAllTransactionIdByWallet(int walletId)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getWalletString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetTransactionsIdByWallet?walletId={walletId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get wallet
                    var transactionIds = await httpClient.GetStringAsync(getWalletString);
                    var convertedTransactionIds = JsonConvert.DeserializeObject<List<int?>>(transactionIds);

                    if (convertedTransactionIds is null || convertedTransactionIds.Count <= 0)
                    {
                        return null;
                    }

                    return convertedTransactionIds;
                }
            }
            catch
            {
                return null;
            }
        }

        async public Task<List<int?>> GetAllAcquaintance_TransactionIdByTransaction(int transactionId)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getWalletString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetAcquaintance_TransactionIdByTransaction?transactionId={transactionId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get wallet
                    var relationIds = await httpClient.GetStringAsync(getWalletString);
                    var convertedRelationIds = JsonConvert.DeserializeObject<List<int?>>(relationIds);

                    if (convertedRelationIds is null || convertedRelationIds.Count <= 0)
                    {
                        return null;
                    }

                    return convertedRelationIds;
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region PUT

        async public Task<bool> UpdateWallet(Wallet wallet)
        {
            if (wallet is null)
                return false;

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var putString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"UpdateWallet";

            var myContent = JsonConvert.SerializeObject(wallet);

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
                    var result = await httpClient.PutAsync(putString, byteContent);

                    if (!result.IsSuccessStatusCode)
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region DELETE

        async public Task<bool> DeleteAllAcquaintance_TransactionByTransaction(int transactionId)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var deleteString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"DeleteAcquaintance_TransactionAll?transactionId={transactionId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Delete wallet
                    var response = await httpClient.DeleteAsync(deleteString);

                    if (response.IsSuccessStatusCode)
                        return true;

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> DeleteAllTransactionByWallet(int walletId)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var deleteString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"DeleteTransactionAll?walletId={walletId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Delete wallet
                    var response = await httpClient.DeleteAsync(deleteString);

                    if (response.IsSuccessStatusCode)
                        return true;

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        async public Task<bool> DeleteWallet(int id)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var deleteString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"DeleteWallet?id={id}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Delete wallet
                    var response = await httpClient.DeleteAsync(deleteString);

                    if (response.IsSuccessStatusCode)
                        return true;

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
