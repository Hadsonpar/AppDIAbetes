using SQLite;
using System;

namespace AppDIAbetes.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        [MaxLength(8)]
        public string password { get; set; }
        //[MaxLength(10)]
        //public string phone { get; set; }
        public bool active { get; set; }
        public string image { get; set; }
        public DateTime regDate { get; set; }
        public DateTime updDate { get; set; }
    }
}