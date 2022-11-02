using System;

namespace DoAn_IE307_N11.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
        public TransactionType Type { get; set; }
    }
}
