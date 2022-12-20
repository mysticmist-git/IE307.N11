using SQLite;
using System;

namespace DoAn_IE307_N11.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public int WalletId { get; set; }
        public int TypeId { get; set; }
        public int? EventId { get; set; }
        public int Amount{ get; set; }
        public string Note{ get; set; }
        public DateTime Date{ get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int IsDeleted { get; set; }
    }

}
