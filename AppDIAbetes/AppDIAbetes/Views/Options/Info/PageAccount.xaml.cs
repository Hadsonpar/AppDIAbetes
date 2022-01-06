using AppDIAbetes.Data;
using AppDIAbetes.Models;
using AppDIAbetes.Utility;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDIAbetes.Views.Options.Info
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAccount : ContentPage
    {
        //Managing user session
        private string strAuthentifyUser;
        public static string sstrEmailUser;
        LoginAccess OAuthLogin = new LoginAccess(sstrEmailUser);

        User users = new User();
        UserDB userDB = new UserDB();
   
        public PageAccount()
        {
            strAuthentifyUser = OAuthLogin.AuthentifyLogin();//Capture email
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IList<User> result = await userDB.userFillEmail(strAuthentifyUser);

            foreach (var item in result)
            {
                labelId.Text = item.Id.ToString();
                entryEmail.Text = item.email;
                entryName.Text = item.name;
                entryPassword.Text = item.password;
                entryConfPassword.Text = item.password;
                valActive.IsToggled = item.active;
            }
        }

        private async void cuentaEdit_Clicked(object sender, EventArgs e)
        {
            if (validateProperties() == "Of")
            {
                await DisplayAlert("Alerta", "Debe ingresar los datos solicitados.", "Aceptar");

            }
            else if (!string.Equals(entryPassword.Text, entryConfPassword.Text))
            {
                warningPassword.Text = "Ingresar la misma contraseña";
                entryPassword.Text = string.Empty;
                entryConfPassword.Text = string.Empty;
                warningPassword.TextColor = Color.IndianRed;
                warningPassword.IsVisible = true;
            }
            else
            {
                users.Id = Convert.ToInt32(labelId.Text);
                users.email = entryEmail.Text.Trim();
                users.name = entryName.Text.Trim();
                users.password = entryPassword.Text.Trim();
                users.active = valActive.IsToggled;
                users.updDate = DateTime.Now;

                try
                {

                    var result = await DisplayAlert("Confirmar", "Estas seguro de actualizado los datos.", "SI", "NO");
                    if (result)
                    {
                        await userDB.UpdateItemAsync(users);
                        await DisplayAlert("Aviso", "Registro actualizado exitosamente.", "Aceptar");
                    }
                    else
                    {
                        return;
                    }
                    
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.ToString(), "Aceptar");
                }
            }
        }

        string validateProperties()
        {
            string result = null;
            if ((string.IsNullOrWhiteSpace(entryPassword.Text)) || 
                (string.IsNullOrEmpty(entryPassword.Text)) || 
                (string.IsNullOrEmpty(entryPassword.Text)))
            {
                result = "Of";
            }
            else
            {
                result = "Ok";
            }
            return result;
        }
    }
}