using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppDIAbetes.Models;
using AppDIAbetes.Data;
using System.Diagnostics;
using AppDIAbetes.Utility;

namespace AppDIAbetes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpLogin : ContentPage
    {
        User users = new User();

        UserDB userDB = new UserDB();
        UtiAppDIAbetes uti = new UtiAppDIAbetes();

        public SignUpLogin()
        {
            InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);
            //Focus entry por entry
            entryEmail.ReturnCommand = new Command(() => entryName.Focus());
            entryName.ReturnCommand = new Command(() => entryPassword.Focus());
            entryPassword.ReturnCommand = new Command(() => entryConfPassword.Focus());
            entryConfPassword.ReturnCommand = new Command(() => signUp.Focus());
        }

        private async void signUp_Clicked(object sender, EventArgs e)
        {
            if (validateProperties() == "Of")
            {
                await DisplayAlert("Alerta", "Debe ingresar los datos solicitados.", "Aceptar");

            } 
            else if (!string.Equals(entryPassword.Text, entryConfPassword.Text))
            {
                warningPassword.Text = string.Empty;
                warningPassword.Text = "Ingresa la misma contraseña";
                entryPassword.Text = string.Empty;
                entryConfPassword.Text = string.Empty;
                //warningPassword.TextColor = Color.IndianRed;
                warningPassword.IsVisible = true;
            }
            else if (!uti.ValidatePassword(entryPassword.Text))
            {
                warningPassword.Text = string.Empty;
                //await DisplayAlert("Alerta", "La contraseña ingresada no cumple con el nivel de seguridad.", "Aceptar");
                entryPassword.Text = string.Empty;
                entryConfPassword.Text = string.Empty;
                warningPassword.Text = "Ingresar al menos 8 dígitos entre mayúsculas, minúsculas, 1 caracter especial y 2 números.";
                warningPassword.IsVisible = true;
            //}
            //else if (entryPhone.Text.Length != 9)
            //{
            //    warningPhone.Text = string.Empty;
            //    warningPhone.Text = "Ingresar 9 dígitos";
            //    //warningPhone.TextColor = Color.IndianRed;
            //    warningPhone.IsVisible = true;
            }
            else {

                users.email = entryEmail.Text.Trim();
                users.name = entryName.Text.Trim();
                users.password = entryPassword.Text.Trim();
                //users.phone = entryPhone.Text.Trim();
                users.active = true;//value for default in the insert
                users.image = "check16.png";
                users.regDate = DateTime.Now;
                users.updDate = DateTime.Now;

                try
                {
                    var returnResult = userDB.SaveItem(users);//, people);
                    if (returnResult == "Ins"){
                        await DisplayAlert("Aviso", "Su cuenta ha sido creada con éxito.", "Aceptar");
                        await Navigation.PushAsync(new Login());
                        cleanProperties();
                    }
                    else if (returnResult == "Exi"){
                        await DisplayAlert("Alerta", "El correo " + users.email + " ya existe.", "Aceptar");
                    }
                    //else if (returnResult == "Err") {
                    //    await DisplayAlert("Error", "Sucedio un error inesperado.", "Aceptar");
                    //}
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.ToString(), "Aceptar");
                    cleanProperties();
                    //Debug.WriteLine(ex); ;
                }
            }
        }

        /// <summary>
        /// Limpia las propiedades después de ingresar los datos
        /// </summary>
        void cleanProperties()
        {
            entryEmail.Text = string.Empty;
            entryName.Text = string.Empty;

            entryPassword.Text = string.Empty;

            entryConfPassword.Text = string.Empty;
            warningPassword.Text = string.Empty; //"Ingresar la misma contraseña";
            warningPassword.IsVisible = false;

            //entryPhone.Text = string.Empty;
            //warningPhone.Text = string.Empty; //"Ingresar 9 dígitos";
            //warningPhone.IsVisible = false;

        }

        /// <summary>
        /// Validación de cada propiedad
        /// </summary>
        /// <returns></returns>
        string validateProperties()
        {
            string result = null;
            if ((string.IsNullOrWhiteSpace(entryEmail.Text)) || (string.IsNullOrWhiteSpace(entryName.Text)) ||
                (string.IsNullOrWhiteSpace(entryPassword.Text)) || //(string.IsNullOrWhiteSpace(entryPhone.Text)) ||
                (string.IsNullOrEmpty(entryPassword.Text)) || //(string.IsNullOrEmpty(entryPhone.Text)) ||
                (string.IsNullOrEmpty(entryPassword.Text)))// || (string.IsNullOrEmpty(entryPhone.Text)))
            {
                result = "Of";
            }
            else
            {
                result = "Ok";
            }
            return result;
        }

        public void ShowPass(object sender, EventArgs args)
        {
            //passwordEntry.IsPassword = passwordEntry.IsPassword ? false : true;
            entryPassword.IsPassword = !entryPassword.IsPassword;

            if (!entryPassword.IsPassword)
            {
                iconView.Source = "showicon.png";
            }
            else
            {
                iconView.Source = "hideicon.png";
            }

        }

        public void ShowConfPass(object sender, EventArgs args)
        {
            //passwordEntry.IsPassword = passwordEntry.IsPassword ? false : true;
            entryConfPassword.IsPassword = !entryConfPassword.IsPassword;

            if (!entryConfPassword.IsPassword)
            {
                iconViewConfPass.Source = "showicon.png";
            }
            else
            {
                iconViewConfPass.Source = "hideicon.png";
            }

        }
    }
}