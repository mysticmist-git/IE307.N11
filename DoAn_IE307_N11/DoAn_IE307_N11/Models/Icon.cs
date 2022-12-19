using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DoAn_IE307_N11.Models
{
    public class Icon
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int IsDeleted { get; set; }
    }

}
