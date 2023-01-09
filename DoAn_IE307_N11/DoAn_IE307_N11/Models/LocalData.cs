using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.Models
{
    public class LocalData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int WalletId { get; set; }
        public bool IsTransactionTypeDownloaded { get; set; } = false;
        public bool IsIconDownloaded { get; set; } = false;
    }
}
