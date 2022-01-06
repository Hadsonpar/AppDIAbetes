using System;
using System.ComponentModel;
using Xamarin.Forms;
using AppDIAbetes.Views;
using System.Threading.Tasks;
using AppDIAbetes.Interface;
using AppDIAbetes.Utility;

namespace AppDIAbetes
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    //[DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        UtiAppDIAbetes uti = new UtiAppDIAbetes();
        //IManageAccess iloginManager = null;
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            lblTextApp.Text = uti.strEtiquetaApp();

        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            //await Task.Delay(3000);   
            await this.Navigation.PushAsync(new Login());
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpLogin());
        }
        
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}