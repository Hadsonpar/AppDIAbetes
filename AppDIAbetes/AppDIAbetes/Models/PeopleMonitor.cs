using SQLite;
using System;

namespace AppDIAbetes.Models
{
    public class PeopleMonitor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ingVal { get; set; }//Indicador HI, LO y demás valores
        public string comentVal { get; set; }//Comentario del valor a ingresar
        public int ingValues { get; set; }//Todod los valores inclusive los valores de HI y LO
        public DateTime ingDateTime { get; set; }
        public string typeGloc { get; set; }
        public int ingInd { get; set; }//Valor Semaforo
        public string imgInd { get; set; }//Imagen Semaforo
        public bool active { get; set; }
        public int IdUser { get; set; }        
    }

    public class ReportGrid
    {
        public int ingCount { get; set; }
        public string ingDes { get; set; }
        public string ingImg { get; set; }
        public int ingInd { get; set; }
    }
}
