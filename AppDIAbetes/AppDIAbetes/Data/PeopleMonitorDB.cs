using AppDIAbetes.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDIAbetes.Data
{
    public class PeopleMonitorDB
    {
        #region conexionbd
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DataBase.DatabasePath, DataBase.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public PeopleMonitorDB()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(PeopleMonitor).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(PeopleMonitor)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }
        #endregion

        #region metodos crud
        public Task<List<PeopleMonitor>> peopleMonitorFillIdUse(int intIdUser)
        {
            return Database.QueryAsync<PeopleMonitor>("Select * from PeopleMonitor Where IdUser=? order by ingDateTime DESC", intIdUser); ;
        }

        public List<PeopleMonitor> peopleMonitorRecordIngDate(int intIdUser)//, string strMonth)
        {
            var peopleMonitors = Database.QueryAsync<PeopleMonitor>("Select imgInd, ingVal, typeGloc, ingDateTime from PeopleMonitor " +
                "                                                   Where IdUser=? order by ingDateTime DESC", intIdUser);
            return peopleMonitors.Result;
        }


        public Task<List<PeopleMonitor>> peopleMonitorDelete()
        {
            return Database.QueryAsync<PeopleMonitor>("Delete from PeopleMonitor");
        }

        public string savePeopleMonitor(PeopleMonitor peopleMonitor)
        {
            string srtResult = null;
            Database.InsertAsync(peopleMonitor);
            srtResult = "Ins";
            return srtResult;
        }
        #endregion

        #region metodos reports
        public List<PeopleMonitor> peopleReporteChartList(int intIdUser)//, string strPeriod)
        {
            var peopleMonitors = Database.QueryAsync<PeopleMonitor>("Select imgInd, ingInd, ingVal, ingValues, typeGloc, ingDateTime, comentVal from PeopleMonitor " +
                                                                    "Where IdUser=? order by ingDateTime DESC", intIdUser);//, strPeriod);
            return peopleMonitors.Result;
        }
        #endregion
    }

}