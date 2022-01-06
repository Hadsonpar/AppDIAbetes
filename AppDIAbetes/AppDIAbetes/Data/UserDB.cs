using AppDIAbetes.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppDIAbetes.Data
{
    public class UserDB
    {
        #region conexionbd
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DataBase.DatabasePath, DataBase.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public UserDB()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(User).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(User)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }
        #endregion

        #region metodos crud  
        public Task<List<User>> GetItemsAsync()
        {
            return Database.Table<User>().ToListAsync();
        }

        public Task<List<User>> userFillEmail(string strEmail)
        {
            return Database.QueryAsync<User>("Select * from User Where email=?", strEmail);
        }

        public Task<User> GetItemAsync(int id)
        {
            return Database.Table<User>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public string SaveItem(User user)
        {
            IEnumerable<User> result = valEmail(user.email);

            if (result.Count() == 0)
            {
                Database.InsertAsync(user);
                return "Ins"; //Insertar
            }
            else {
                return "Exi";//Existe
            }
        }

        public IEnumerable<User> valEmail(string strEmail)
        {
            var result = Database.QueryAsync<User>("Select * from User Where email=?", strEmail);
            return result.Result;
        }

        public Task<int> UpdateItemAsync(User user)
        {
            if (user.active == true)
            {
                user.image = "check16.png";
            }
            else
            {
                user.image = "delete16.png";
            }
            return Database.UpdateAsync(user);
        }

        public Task<int> DeleteItemAsync(User user)
        {
            return Database.DeleteAsync(user);
        }
        #endregion

        #region metodos login
        public IEnumerable<User> whereUser(string strEmail, string strPwd)
        {
            var result = Database.QueryAsync<User>("Select * from User Where email=? and password=?", strEmail, strPwd);

            return result.Result;
        }
        #endregion

        #region metodo recuperar contraseña
        public Task<List<User>> userValUserEmail(string strEmail)
        {
            return Database.QueryAsync<User>("Select * from User Where email=?", strEmail);
        }

        public string newUserPassword(User user)
        {
            string srtResult = null;
            Database.QueryAsync<User>("Update User Set password=? Where email=?", user.password, user.email);
            srtResult = "Upd";
            return srtResult;
        }
        #endregion
    }
}
