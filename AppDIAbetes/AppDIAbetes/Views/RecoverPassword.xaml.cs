using AppDIAbetes.Data;
using AppDIAbetes.Models;
using AppDIAbetes.Utility;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDIAbetes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecoverPassword : ContentPage
    {
        private string strEmail;
        private bool boValActive;
        UserDB userDB = new UserDB();
        User user = new User();
        UtiAppDIAbetes uti = new UtiAppDIAbetes();

        public RecoverPassword()
        {
            InitializeComponent();
            lblTextApp.Text = uti.strEtiquetaApp();
            entryPassword.ReturnCommand = new Command(() => entryConfPassword.Focus());
        }

        private async void btnRecoverPass_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entryValEmail.Text)){
                await DisplayAlert("Alerta", "Debe ingresar su correo electrónico.", "Aceptar");
                entryValEmail.Focus();
            }
            else { 
                strEmail = entryValEmail.Text.Trim();
                IList<User> result = await userDB.userValUserEmail(strEmail);

                foreach (var item in result)
                {
                    boValActive = item.active;
                    entryEmail.Text = item.email;
                    entryName.Text = item.name;
                }

                if (boValActive == true)
                {
                    popupViewDetails.IsVisible = true;
                }
                else {
                    await DisplayAlert("Alerta", "La cuenta se encuentra inactivo.", "Aceptar");
                }
            }
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            popupViewDetails.IsVisible = false;
        }

        private async void btnGenContra_Clicked(object sender, EventArgs e) 
        {

            if (string.IsNullOrEmpty(entryPassword.Text) ||  string.IsNullOrWhiteSpace(entryPassword.Text) ||
                string.IsNullOrEmpty(entryConfPassword.Text) || string.IsNullOrWhiteSpace(entryConfPassword.Text))
            {
                await DisplayAlert("Alerta", "Debe ingresar su nueva contraseña.", "Aceptar");
                entryPassword.Focus();
            }
            else if (!string.Equals(entryPassword.Text, entryConfPassword.Text))
            {
                warningPassword.Text = string.Empty;
                warningPassword.Text = "Ingresar la misma contraseña";
                entryPassword.Text = string.Empty;
                entryConfPassword.Text = string.Empty;
                warningPassword.IsVisible = true;
            }
            else if (!uti.ValidatePassword(entryPassword.Text))
            {
                warningPassword.Text = string.Empty;
                entryPassword.Text = string.Empty;
                entryConfPassword.Text = string.Empty;
                warningPassword.Text = "Ingresar al menos 8 dígitos entre mayúsculas, minúsculas, 1 caracter especial y 2 números.";
                warningPassword.IsVisible = true;
            }
            else
            {
                user.email = entryEmail.Text.Trim();
                user.password = entryPassword.Text.Trim();
                user.updDate = DateTime.Now;

                try
                {

                    var returnResult = userDB.newUserPassword(user);
                    if (returnResult == "Upd")
                        await DisplayAlert("Aviso", "Su nueva contraseña ha sido generada con éxito.", "Aceptar");
                        await Navigation.PushAsync(new Login());
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.ToString(), "Aceptar");
                    warningPassword.Text = string.Empty;
                    entryConfPassword.Text = string.Empty;
                    warningPassword.Text = string.Empty;
                }
            }
        }
    }
}