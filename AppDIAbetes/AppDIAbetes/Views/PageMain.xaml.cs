using AppDIAbetes.Views.Options;
using AppDIAbetes.Models;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppDIAbetes.Utility;
using AppDIAbetes.Views.Options.Control;
using AppDIAbetes.Views.Options.Report;
using AppDIAbetes.Views.Options.Info;
using AppDIAbetes.Data;

namespace AppDIAbetes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMain : MasterDetailPage
    {
        private string strAuthentifyUser;
        public static string sstrEmailUser;
        LoginAccess OAuthLogin = new LoginAccess(sstrEmailUser);
        UserDB userDB = new UserDB();
        private string strUser;

        public PageMain()
        {
            strAuthentifyUser = OAuthLogin.AuthentifyLogin();
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
            myPageMain();
            vCaptureIdUser();
        }

        public void myPageMain() {
            Detail = new NavigationPage(new PageHome());

            List<Option> options = new List<Option>
            {
                new Option{ page=new PageHome(),title="Inicio", detail="Inicio", image = "ic_ac_home.png" },
                new Option{ page=new TabPageInfo(),title="Mi Información", detail="Actualizar registros", image = "ic_ac_control.png" },
                new Option{ page=new TabPageCtrl(),title="Registro de Control", detail="Ingresa tu monitoreo", image = "ic_ac_registry.png" },
                new Option{ page=new TabPageReport(),title="Mis Reportes", detail="Reportes seamanales", image = "ic_ac_report.png" },
                new Option{ page=new PageAbout(),title="Acerca de", detail="App DIAbetes", image = "ic_ac_about.png" },
                new Option{ page=null,title="Cerrar Sesión", detail="Abandonar App", image = "ic_ac_signoff.png" }
            };
            listPageMain.ItemsSource = options;
        }
        private async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var option = e.SelectedItem as Option;
            if (option.page != null)
            {                
                IsPresented = false;
                Detail = new NavigationPage(option.page);
            }
            else if (option.page == null || option.title == "Cerrar Sesión") {

                var result = await DisplayAlert("Confirmar", "Estas seguro de cerrar la aplicación", "SI", "NO");
                if (result)
                {
                    OAuthLogin.LogoutLogin();
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    return;
                }
            }
        }
        private async void vCaptureIdUser()
        {
            IList<User> result = await userDB.userFillEmail(strAuthentifyUser);

            if (result.Count == 1)
            {
                foreach (var item in result)
                {
                    strUser = item.name;
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Ocurrio un problema al cargar los registros.", "Aceptar");
            }
            labelEmail.Text = "¡Bienvenido " + strUser + "!";
        }

        protected override bool OnBackButtonPressed()
        {
            //DisplayAlert("Alerta", "Deshabilitar botón atras.", "Aceptar");
            return true;
        }
    }
}