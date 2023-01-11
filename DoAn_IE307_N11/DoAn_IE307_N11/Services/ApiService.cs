using DoAn_IE307_N11.Models;
using DoAn_IE307_N11.Utils;
using DoAn_IE307_N11.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Forms;

namespace DoAn_IE307_N11.Services
{
    public enum ApiCallResult
    {
        NullData,
        Success,
        Fail,
        UnknownError,
        None,
    }

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
        
        async public Task<Wallet> GetWalletById(int walletId)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getWalletString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetWalletById?id={walletId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get wallet
                    var wallet = await httpClient.GetStringAsync(getWalletString);
                    var convertedWallet = JsonConvert.DeserializeObject<Wallet>(wallet);

                    if (convertedWallet is null)
                    {
                        return null;
                    }

                    convertedWallet.ServerId = convertedWallet.Id;
                    return convertedWallet;
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

        /// <summary>
        /// Check if a username is already existed
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True: username existed</returns>
        public async Task<bool> CheckUsernameExisted(string username)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getAccountString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetAccountByUsername?username={username}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get wallet
                    var accounts = await httpClient.GetStringAsync(getAccountString);
                    var convertedAccount = JsonConvert.DeserializeObject<Account>(accounts);

                    return convertedAccount != null;
                }
            }
            catch
            {
                return false;
            }
        }


        #region Services

        /// <summary>
        /// Check password of username is correct
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        async public Task<bool> CheckPasswordAsync(string username, string password)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var getAccountString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"GetAccountByUsername?username={username}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Get wallet
                    var accounts = await httpClient.GetStringAsync(getAccountString);
                    var convertedAccount = JsonConvert.DeserializeObject<Account>(accounts);

                    if (convertedAccount is null)
                        return false;

                    return convertedAccount.Password == password;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region POST

        #region Account Related

        /// <summary>
        /// Register a new account
        /// </summary>
        /// <param name="newAccount"></param>
        /// <returns>Indicates if the registeration is successful or not</returns>
        public async Task<ApiCallResult> RegisterAccount(Account newAccount)
        {
            if (newAccount == null)
                return ApiCallResult.NullData;

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var postString = $"http://{ip}/moneybook/api/ServiceController/" +
                $"CreateAccount";

            var myContent = JsonConvert.SerializeObject(newAccount);

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
                        return ApiCallResult.Fail;
                    }

                    return ApiCallResult.Success;
                }
            }
            catch
            {
                return ApiCallResult.UnknownError;
            }
        }

        #endregion

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

        async public Task<bool> UpdateTransaction(Models.Transaction transaction)
        {
            if (transaction is null)
                return false;

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var putString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"UpdateTransaction";

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

        async public Task<bool> UpdateAccount(Models.Account account)
        {
            if (account is null)
                return false;

            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var putString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"UpdateAccount";

            var myContent = JsonConvert.SerializeObject(account);

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

        /// <summary>
        /// DELETE All Acquaintance_Transaction by Transaction Id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// DELETE all transaction by wallet id
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// DELETE wallet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        async public Task<bool> DeleteTransaction(int transactionId)
        {
            // Ip info
            var ip = DependencyService.Get<ConstantService>().MY_IP;
            var deleteString = $"http://{ip}/moneybook/api/ServiceController/" +
                        $"DeleteTransaction?id={transactionId}";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Delete transaction
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
