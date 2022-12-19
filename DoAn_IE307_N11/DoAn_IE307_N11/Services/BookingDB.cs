using DoAn_IE307_N11.Models;
using SQLite;
using SQLitePCL;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DoAn_IE307_N11.Services
{
    public class LocalDataService
    {
        #region Init

        public bool InitDB()
        {
            try
            {
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Icon>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Currency>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Currency>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Account>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Wallet>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<TransactionType>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Acquaintance>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Transaction>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Event>();
                DependencyService.Get<SQLiteDB>().DB.CreateTable<Acquaintance_Transaction>();

                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion
    }
}
