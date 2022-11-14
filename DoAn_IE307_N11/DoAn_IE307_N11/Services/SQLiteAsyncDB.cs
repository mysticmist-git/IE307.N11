using SQLite;
using System;
using System.IO;

namespace DoAn_IE307_N11.Services
{
    public class SQLiteAsyncDB
    {
        private static SQLiteAsyncConnection _db;

        public static SQLiteAsyncConnection Db
        {
            get
            {
                if (_db is null)
                    _db = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DNTDB.db"));

                return _db;
            }

            set => Db = value;
        }

        private SQLiteAsyncDB() { }
    }

    public class SQLiteDB
    {
        private static SQLiteConnection _db;

        public static SQLiteConnection Db 
        { 
            get
            {
                if (_db is null)
                    _db = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DNTDB.db"));

                return _db;
            }

            set => Db = value;
        }

        private SQLiteDB() { }
    }
}
