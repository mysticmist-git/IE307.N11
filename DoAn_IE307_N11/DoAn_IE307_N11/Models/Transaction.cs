using SQLite;
using System;

namespace DoAn_IE307_N11.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int TransactionTypeId { get; set; }
        [Indexed]
        public int WalletId { get; set; }   
        //[Indexed]
        //public int EventId { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
    }
}
