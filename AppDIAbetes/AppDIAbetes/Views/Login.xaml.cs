using AppDIAbetes.Data;
using AppDIAbetes.Interface;
using AppDIAbetes.Models;
using AppDIAbetes.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDIAbetes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        UserDB userData = new UserDB();
        UtiAppDIAbetes uti = new UtiAppDIAbetes();

        public Login()
        {
            InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);
            var forgetpassword_tap = new TapGestureRecognizer();
            forgetpassword_tap.Tapped += Forgetpassword_tap_Tapped;
            forgetLabel.GestureRecognizers.Add(forgetpassword_tap);

            userEmailEntry.ReturnCommand = new Command(() => passwordEntry.Focus());
            passwordEntry.ReturnCommand = new Command(() => btnLogin.Focus());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Forgetpassword_tap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecoverPassword());
        }
        /// <summary>
        /// Botón para ingresar a la pantalla principal de la app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void loging_Clicked(object sender, EventArgs e)
        {
            if (await ValidateForm())
            {   
                IEnumerable<User> result = userData.whereUser(userEmailEntry.Text.Trim(), passwordEntry.Text.Trim());
                
                if (result.Count() == 0)
                {
                    await DisplayAlert("Alerta", "El correo electrónico o contraseña son incorrectos.", "Aceptar");
                }
                else if (result.Count() == 1)
                {
                    Application.Current.Properties["keyEmail"] = userEmailEntry.Text.Trim();//Assign value the key
                    Application.Current.Properties["IsLoggedIn"] = true;//Confirm access to out app
                    
                    await Navigation.PushAsync(new PageMain());
                }
                else if (result.Count() >= 1)
                {
                    await DisplayAlert("Alerta", "Existe más de una cuenta registrada, favor de solicitar la correción de la cuenta.", "Aceptar");
                    //await Navigation.PushAsync(new DuplicateAccountSendEmail());
                }
            }
        }

        /// <summary>
        /// Validar las propiedades de la pantalla de envio de email
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateForm()
        {
            //Valida si el valor en el Entry txtTo se encuentra vacio o es igual a Null
            if (String.IsNullOrWhiteSpace(userEmailEntry.Text))
            {
                await this.DisplayAlert("Alerta", "El correo electrónico es obligatorio.", "Aceptar");
                userEmailEntry.Focus();
                return false;
            }
            else
            {
                //Valida que el formato del email sea valido
                bool isEmail = uti.ValidateEmail(userEmailEntry.Text);
                if (!isEmail)
                {
                    await this.DisplayAlert("Alerta", "El formato del Correo electrónico es incorrecto.", "Aceptar");
                    return false;
                }
            }

            //Valida si el valor en el Entry txtSubject se encuentra vacio o es igual a Null
            if (String.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                await this.DisplayAlert("Alerta", "La contraseña es obligatorio.", "Aceptar");
                passwordEntry.Focus();
                return false;
            }
            
            IList<User> result = await userData.userValUserEmail(userEmailEntry.Text.Trim());

            if (result.Count == 0) {
                await this.DisplayAlert("Alerta", "Correo electrónico no registrado.", "Aceptar");
                return false;
            }

            return true;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public void ShowPass(object sender, EventArgs args)
        {
            //passwordEntry.IsPassword = passwordEntry.IsPassword ? false : true;
            passwordEntry.IsPassword = !passwordEntry.IsPassword;

            if (!passwordEntry.IsPassword) {
                iconView.Source = "showicon.png";
            }
            else {
                iconView.Source = "hideicon.png";
            }
        }
    }
}