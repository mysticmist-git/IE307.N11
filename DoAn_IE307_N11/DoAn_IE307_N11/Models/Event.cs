﻿using SQLite;
using System;

namespace DoAn_IE307_N11.Models
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public int AccountId { get; set; }
        public int IconId { get; set; }
        public string Name { get; set; }
        public DateTime ExpiredDate{ get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int IsDeleted { get; set; }
    }

}
