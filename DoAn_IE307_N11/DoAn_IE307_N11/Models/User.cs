using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoAn_IE307_N11.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
