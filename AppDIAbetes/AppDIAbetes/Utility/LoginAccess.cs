using Xamarin.Forms;

namespace AppDIAbetes.Utility
{
    public class LoginAccess
    {
        public static string sstrEmail = "";
        public LoginAccess(string pstrEmailUser)
        {

            if (Application.Current.Properties.ContainsKey("keyEmail"))
            {
                var vKeyEmail = Application.Current.Properties["keyEmail"];
                sstrEmail = vKeyEmail.ToString();
            }
        }
        public string AuthentifyLogin()
        {
            return sstrEmail;
        }

        public void LogoutLogin()
        {
            Application.Current.Properties["IsLoggedIn"] = false;//To close the app, determinet the variable session IsLoggedIn in value false
            Application.Current.Properties["keyEmail"] = null;//Clear value of E-mail
            Application.Current.Properties["keyIdUser"] = null;
        }
    }
}
