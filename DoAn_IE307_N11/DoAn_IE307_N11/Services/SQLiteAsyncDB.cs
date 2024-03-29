﻿using SQLite;
using System;
using System.IO;

namespace DoAn_IE307_N11.Services
{
    public class SQLiteDBAsync
    {
        public SQLiteAsyncConnection DB { get; set; }

        public SQLiteDBAsync()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(folder, "lab4.db");
            DB = new SQLiteAsyncConnection(path);
        }
    }
}
