using SQLite;
using System;
using System.IO;

namespace DoAn_IE307_N11.Services
{
    public class SQLiteDB
    {
        public SQLiteConnection DB { get; set; }

        public SQLiteDB() 
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(folder, "lab4.db");
            DB = new SQLiteConnection(path);
        }
    }
}
