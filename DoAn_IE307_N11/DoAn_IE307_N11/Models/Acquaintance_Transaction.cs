using SQLite;
using System;

namespace DoAn_IE307_N11.Models
{
    public class Acquaintance_Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public int AcquaintanceId { get; set; }
        public int TransactionId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int IsDeleted { get; set; }
    }

}
