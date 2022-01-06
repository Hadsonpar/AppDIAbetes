using AppDIAbetes.Utility;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppDIAbetes.Models;
using AppDIAbetes.Data;

namespace AppDIAbetes.Views.Options.Info
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageInfo : ContentPage
    {
        //Managing user session
        private string strAuthentifyUser;
        public static string sstrEmailUser;
        LoginAccess OAuthLogin = new LoginAccess(sstrEmailUser);

        private int intId, intIdUser;
        private string strUser;

        People people = new People();
        PeopleDB peopleDB = new PeopleDB();
        UserDB userDB = new UserDB();

        public PageInfo()
        {
            strAuthentifyUser = OAuthLogin.AuthentifyLogin();
            InitializeComponent();
            //utiAppDIAbetes.strFormatDateES(dpFechaNac.Date);
            //dpFechaNac.Date.ToString("d", CultureInfo.CreateSpecificCulture("es-PE"));
            vCaptureIdUser();

            entryFirstName.ReturnCommand = new Command(() => entryLastName.Focus());
            entryLastName.ReturnCommand = new Command(() => dpFechaNac.Focus());
            entryPhone.ReturnCommand = new Command(() => pTipyPeople.Focus());
            entryPhoneMedico.ReturnCommand = new Command(() => entryEmailMedico.Focus());
        }

        private async void vCaptureIdUser() {
            IList<User> result = await userDB.userFillEmail(strAuthentifyUser);

            if (result.Count == 1)
            {
                foreach (var item in result)
                {
                    intIdUser = item.Id;//Id de la tabla User
                    strUser = item.name;
                }
            }
            else {
                await DisplayAlert("Alerta", "Ocurrio un problema al cargar los registros.", "Aceptar");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IList<People> result = await peopleDB.peopleFillIdUse(intIdUser);

            foreach (var item in result)
            {
                intId = item.Id;
                entryFirstName.Text = item.firstName;
                entryLastName.Text = item.lastName;
                dpFechaNac.Date = item.birthdate;
                entryPhone.Text = item.phoneUser;
                pTipyPeople.SelectedIndex = item.IdRole;

                entryPhoneMedico.Text = item.phoneMedico;
                entryEmailMedico.Text = item.emailMedico;
            }
        }

        #region control type people
        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                if (selectedIndex == 0)// 0 PACIENTE
                {
                    labelPhoneMedico.IsVisible = true;
                    entryPhoneMedico.IsVisible = true;
                    labelEmailMedico.IsVisible = true;
                    entryEmailMedico.IsVisible = true;
                    fmedico.IsVisible = true;
                }
                else if(selectedIndex == 1) {//1 MEDICO TRATANTE
                    labelPhoneMedico.IsVisible = false;
                    entryPhoneMedico.IsVisible = false;
                    labelEmailMedico.IsVisible = false;
                    entryEmailMedico.IsVisible = false;
                    fmedico.IsVisible = false;
                }
            }
        }
        #endregion

        #region CRUD
        private async void infoInsert_Clicked(object sender, EventArgs e)
        {
            people.Id = intId;//Id de la tabla People
            people.firstName = entryFirstName.Text;
            people.lastName = entryLastName.Text;
            people.birthdate = dpFechaNac.Date;
            people.phoneUser = entryPhone.Text;
            //people.photo = photoUser.Source;
            people.phoneMedico = entryPhoneMedico.Text;
            people.emailMedico = entryEmailMedico.Text;

            people.IdUser = intIdUser;//Id de la tabla User
            people.IdRole = pTipyPeople.SelectedIndex;

            if (people.Id == 0)
            {
                people.regDate = DateTime.Now;
                people.updDate = DateTime.Now;

                people.chkViewPass = false;
                people.chkChangePass = false;
                people.chkSendReport = false;

                var returnResult = peopleDB.savePeopleInfo(people);
                if (returnResult == "Ins")
                    await DisplayAlert("Aviso", "Registros ingresado con éxito.", "Aceptar");
            }
            else
            {
                people.updDate = DateTime.Now;

                var returnResult = peopleDB.savePeopleInfo(people);
                if (returnResult == "Upd")
                    await DisplayAlert("Aviso", "Registros actualizado con éxito.", "Aceptar");
            }
        }
        #endregion
    }
}