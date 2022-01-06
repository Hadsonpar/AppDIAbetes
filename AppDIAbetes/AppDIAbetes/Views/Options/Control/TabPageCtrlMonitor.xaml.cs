using AppDIAbetes.Data;
using AppDIAbetes.Models;
using AppDIAbetes.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDIAbetes.Views.Options.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabPageCtrlMonitor : ContentPage
    {
        Frame fGrilla = new Frame();

        //Managing user session
        private string strAuthentifyUser;
        public static string sstrEmailUser;
        LoginAccess OAuthLogin = new LoginAccess(sstrEmailUser);

        private int intId, intIdUser;
        private string strUser;
        
        PeopleMonitor peopleMonitor = new PeopleMonitor();
        PeopleMonitorDB peopleMonitorDB = new PeopleMonitorDB();

        UserDB userDB = new UserDB();

        UtiAppDIAbetes uti = new UtiAppDIAbetes();
        public TabPageCtrlMonitor()
        {
            strAuthentifyUser = OAuthLogin.AuthentifyLogin();
            
            InitializeComponent();

            tpickerTime.Time = DateTime.Now.TimeOfDay;//Horario actual
            dpFechaIng.Date = DateTime.Now.Date;//Fecha actual
            vCaptureIdUser();
            editorValueGlu.Focus();

        }

        private async void vCaptureIdUser()
        {
            IList<User> result = await userDB.userFillEmail(strAuthentifyUser);

            if (result.Count == 1)
            {
                foreach (var item in result)
                {
                    intIdUser = item.Id;//Id de la tabla User
                    strUser = item.name;
                }
            }
            else
            {
                await DisplayAlert("Alerta", "Ocurrio un problema al cargar los registros.", "Aceptar");
            }
        }

        #region 1radioButton
        private async void ingSelected(object sender, EventArgs e)
        {  
            var item = tipIngreso.SelectedItem.ToString();

            if (item == "TD")
            {
                fSecValue.IsVisible = true;
                fSecDateTime.IsVisible = false;
            }
            if (item == "PD")
            {
                tpickerTime.Time = DateTime.Now.TimeOfDay;//Horario actual
                dpFechaIng.Date = DateTime.Now.Date;//Fecha actual

                fSecValue.IsVisible = true;
                fSecDateTime.IsVisible = true;
            }
        }
        #endregion

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Image imageInd = new Image();

            IList<PeopleMonitor> resultFillPeriod = await peopleMonitorDB.peopleMonitorFillIdUse(intIdUser);

            //string strPeriod = uti.strFormatMonthOneNine(Convert.ToInt32(uti.strMonth), "MM") + "-" + uti.strYear;
            string strPeriod = uti.strFormatMonth(Convert.ToInt32(uti.strMonth), "1MM") + "-" + uti.strYear;
            IList<PeopleMonitor> peopleMonitor = listFillMonth(resultFillPeriod, strPeriod);

            lvMonitor.ItemsSource = peopleMonitor;
        }

        /// <summary>
        /// Filtro del mes actual de acuerdo a la fecha de SO
        /// </summary>
        /// <param name="fillpeopleMonitor"></param>
        /// <param name="strPeriod"></param>
        /// <returns></returns>
        IList<PeopleMonitor> listFillMonth(IList<PeopleMonitor> fillpeopleMonitor, string strPeriod)
        {
            var resultList = from s in fillpeopleMonitor
                             where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod
                             orderby s.ingDateTime descending
                             select s;
            return resultList.ToList();
        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    Image imageInd = new Image();

        //    IList<PeopleMonitor> peopleMonitor = await peopleMonitorDB.peopleMonitorFillIdUse(intIdUser);
        //    lvMonitor.ItemsSource = peopleMonitor;
        //}

        private async void ing_Clicked(object sender, EventArgs e)
        {
            DateTime dateTimeIng = new DateTime();

            if((string.IsNullOrWhiteSpace(editorValueGlu.Text)) || (string.IsNullOrWhiteSpace(editorValueGlu.Text)))
            {
                await DisplayAlert("Alerta", "Ingresar el valor del glucómetro.", "Aceptar");
                editorValueGlu.Focus();
            }
            else{

                //var item = tipIngreso.SelectedItem.ToString();
                //var str = dateTimeIng.ToString(@"yyyy/MM/dd hh:mm:ss tt", new CultureInfo("es-PE"));
                //var ampm = dateTimeIng.ToString(@"tt");

                if (ingSelectedTD.IsChecked == true)
                {
                    dateTimeIng = DateTime.Now;
                }
                if (ingSelectedPD.IsChecked == true)
                {
                    dateTimeIng = Convert.ToDateTime(dpFechaIng.Date+tpickerTime.Time);
                }

                peopleMonitor.ingVal = uti.funIndicatorLOHI(Convert.ToInt32(editorValueGlu.Text.Trim()));
                peopleMonitor.ingValues = Convert.ToInt32(editorValueGlu.Text.Trim());

                if (comentValueGlu.Text == "" || comentValueGlu.Text == null)
                {
                    peopleMonitor.comentVal = uti.funDefaultNote(dateTimeIng);
                }
                else {
                    peopleMonitor.comentVal = comentValueGlu.Text.Trim();
                }

                peopleMonitor.ingDateTime = dateTimeIng;
                peopleMonitor.typeGloc = "mg/dL";
                peopleMonitor.ingInd = uti.funIndicatorValue(Convert.ToInt32(editorValueGlu.Text.Trim()));
                peopleMonitor.imgInd = uti.funIndicatorImage(peopleMonitor.ingInd);
                peopleMonitor.active = true;
                peopleMonitor.IdUser = intIdUser;

                var returnResult = peopleMonitorDB.savePeopleMonitor(peopleMonitor);
                if (returnResult == "Ins")
                    await DisplayAlert("Aviso", "Registros ingresado con éxito.", "Aceptar");
                editorValueGlu.Text = null;
                comentValueGlu.Text = null;
                OnAppearing();
            }
        }

        private async void delete_Clicked(object sender, EventArgs e)
        {
            await peopleMonitorDB.peopleMonitorDelete();
            editorValueGlu.Text = null;
            OnAppearing();
        }
    }
}