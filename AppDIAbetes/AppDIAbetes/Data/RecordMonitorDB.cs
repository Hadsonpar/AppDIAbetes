using AppDIAbetes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDIAbetes.Data
{
    public class RecordMonitorDB
    {

        public List<RecordMonitor> myListRecordMonth()
        {
            List<RecordMonitor> recordMonitors = new List<RecordMonitor>
            {
                new RecordMonitor{ mes="01", title="ENERO", detail="Datos Históricos del mes de Enero", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="02", title="FEBRERO", detail="Datos Históricos del mes de Febrero", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="03", title="MARZO", detail="Datos Históricos del mes de Marzo", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="04", title="ABRIL", detail="Datos Históricos del mes de Abril", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="05", title="MAYO", detail="Datos Históricos del mes de Mayo", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="06", title="JUNIO", detail="Datos Históricos del mes de Junio", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="07", title="JULIO", detail="Datos Históricos del mes de Julio", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="08", title="AGOSTO", detail="Datos Históricos del mes de Agosto", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="09", title="SEPTIEMBRE", detail="Datos Históricos del mes de Septiembre", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="10", title="OCTUBRE", detail="Datos Históricos del mes de Octubre", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="11", title="NOVIEMBRE", detail="Datos Históricos del mes de Noviembre", image = "ic_ac_report.png" },
                new RecordMonitor{ mes="12", title="DICIEMBRE", detail="Datos Históricos del mes de Diciembre", image = "ic_ac_report.png" }
            };
            return recordMonitors;
        }
    }
}
