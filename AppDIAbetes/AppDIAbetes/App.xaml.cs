using AppDIAbetes.Data;
using AppDIAbetes.Interface;
using AppDIAbetes.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDIAbetes
{
    public partial class App : Application//, IManageAccess
    {
        //static UserDB userDB;
        public static NavigationPage Mainpage;
        public static MasterDetailPage MasterDetailPage;

        public App()
        {
            InitializeComponent();

            Properties["IsLoggedIn"] = false;

            MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#548faf")//5798D2  3a76a0  57adbc  3399ff  6fb7ff")
            };
        }

        //public void ShowMainPage()
        //{
        //    MainPage = new PageMain();
        //}

        public bool DoBack
        {
            get
            {
                NavigationPage mainPage = MainPage as NavigationPage;
                if (mainPage != null)
                {
                    return mainPage.Navigation.NavigationStack.Count > 1;
                }
                return true;
            }
        }



        public void Logout()
        {
            throw new NotImplementedException();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
