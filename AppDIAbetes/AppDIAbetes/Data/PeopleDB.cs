using AppDIAbetes.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDIAbetes.Data
{
    public class PeopleDB
    {
        #region conexionbd
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DataBase.DatabasePath, DataBase.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public PeopleDB()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(People).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(People)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }
        #endregion

        #region metodos crud 
        public Task<List<People>> peopleFillIdUse(int intIdUser)
        {
            return Database.QueryAsync<People>("Select * from People Where IdUser=?", intIdUser);
        }

        public string savePeopleInfo(People people)//, string strEmail)
        {
            string srtResult = null;

            IEnumerable<People> result = valPeople(people.IdUser);
            if(result.Count()== 0) {
                Database.InsertAsync(people);
                srtResult = "Ins";
            }
            else{
                //Database.UpdateAsync(people);
                Database.QueryAsync<People>("Update People Set firstName=?, lastName=?, birthdate=?, phoneUser=?, phoneMedico=?, emailMedico=?, IdRole=?, updDate=? Where Id=? and IdUser=?", people.firstName, people.lastName, people.birthdate, people.phoneUser, people.phoneMedico, people.emailMedico, people.IdRole, people.updDate, people.Id, people.IdUser);
                srtResult = "Upd";
            }
            return srtResult;
        }

        public string savePeopleConfig(People people)
        {
            string srtResult = null;
         
            Database.QueryAsync<People>("Update People Set chkViewPass=?, chkChangePass=?, chkSendReport=?, updDate=? Where Id=? and IdUser=?", people.chkViewPass, people.chkChangePass, people.chkSendReport, people.updDate, people.Id, people.IdUser);
            srtResult = "Upd";
            return srtResult;
        }

        public IEnumerable<People> valPeople(int intIdUser)
        {
            var result = Database.QueryAsync<People>("Select * from People p, User u Where p.IdUser=u.Id and p.IdUser=?", intIdUser);
            return result.Result;
        }
        #endregion
    }
}