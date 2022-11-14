using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.Models
{
    public class Wallet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Indexed]
        public int Name { get; set; }
        public int Balance { get; set; }
        //public ICollection<Event> Events { get; set; }
    }
}
