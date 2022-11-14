using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.Models
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<Wallet> Wallets { get; set; }
    }
}
