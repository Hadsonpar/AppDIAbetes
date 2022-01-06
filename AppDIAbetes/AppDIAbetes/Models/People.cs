using SQLite;
using System;

namespace AppDIAbetes.Models
{                 
    public class People
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime birthdate { get; set; }
        public string phoneUser { get; set; }
        //public Byte photo { get; set; }
        public string nameMedico { get; set; }
        public string phoneMedico { get; set; }
        public string emailMedico { get; set; }
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        public DateTime regDate { get; set; }
        public DateTime updDate { get; set; }
        public bool chkViewPass { get; set; }
        public bool chkChangePass { get; set; }
        public bool chkSendReport { get; set; }
    }
}