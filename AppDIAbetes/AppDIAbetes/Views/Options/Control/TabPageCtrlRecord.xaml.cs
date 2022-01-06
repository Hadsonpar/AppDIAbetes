using System;
using System.Linq;
using System.Collections.Generic;
using AppDIAbetes.Data;
using AppDIAbetes.Models;
using AppDIAbetes.Utility;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDIAbetes.Views.Options.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabPageCtrlRecord : ContentPage
    {
        //Managing user session
        private string strAuthentifyUser;
        public static string sstrEmailUser;
        LoginAccess OAuthLogin = new LoginAccess(sstrEmailUser);

        private int intId, intIdUser;
        private string strUser;

        PeopleMonitorDB peopleMonitorDB = new PeopleMonitorDB();
        UserDB userDB = new UserDB();

        RecordMonitorDB recordMonitorDB = new RecordMonitorDB();

        public TabPageCtrlRecord()
        {
            strAuthentifyUser = OAuthLogin.AuthentifyLogin();
            InitializeComponent();
            vCaptureIdUser();

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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            IList<RecordMonitor> recordMonitor = recordMonitorDB.myListRecordMonth();
            lvRecordMonth.ItemsSource = recordMonitor;
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string strYear = DateTime.Now.Year.ToString();
            IList<PeopleMonitor> peopleMonitor = peopleMonitorDB.peopleMonitorRecordIngDate(intIdUser);
            
            //var tabPageCtrlRecordDetail = new TabPageCtrlRecordDetail();
            var item = e.SelectedItem as RecordMonitor;
            string strPeriod = item.mes + "-" + strYear;

            switch (item.mes)
            {
                case "01":
                    //await DisplayAlert("Aviso", "Mes de Enero.", "Aceptar");
                    var select01 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select01.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de enero.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "02":
                    //await DisplayAlert("Aviso", "Mes de Febrero.", "Aceptar");
                    var select02 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select02.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de febrero.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "03":
                    //await DisplayAlert("Aviso", "Mes de Marzo.", "Aceptar");
                    var select03 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select03.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de marzo.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }

                    break;
                case "04":
                    //await DisplayAlert("Aviso", "Mes de Abril.", "Aceptar");
                    var select04 = from s in peopleMonitor
                                 where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                 select s;
                    if (select04.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de abril.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "05":
                    var select05 = from s in peopleMonitor
                                 where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                 select s;
                    if (select05.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de mayo.", "Aceptar");
                    }
                    else {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        ////tabPageCtrlRecordDetail.BindingContext = e.SelectedItem as PeopleMonitor;
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                        ////labelMonth.Text = "Registros del mes de Mayo";
                        ////lvMonitor.ItemsSource = select;
                        ////popupImageView.IsVisible = true;
                    }
                    break;

                case "06":
                    //await DisplayAlert("Aviso", "Mes de Junio.", "Aceptar");
                    var select06 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select06.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de junio.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }

                    break;
                case "07":
                    //await DisplayAlert("Aviso", "Mes de Julio.", "Aceptar");
                    var select07 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select07.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de julio.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "08":
                    //await DisplayAlert("Aviso", "Mes de Agosto.", "Aceptar");
                    var select08 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select08.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de agosto.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "09":
                    //await DisplayAlert("Aviso", "Mes de Septiembre.", "Aceptar");
                    var select09 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select09.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de setiembre.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "10":
                    //await DisplayAlert("Aviso", "Mes de Octubre.", "Aceptar");
                    var select10 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select10.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de octubre.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "11":
                    //await DisplayAlert("Aviso", "Mes de Noviembre.", "Aceptar");
                    var select11 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select11.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de noviembre.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;

                case "12":
                    //await DisplayAlert("Aviso", "Mes de Diciembre.", "Aceptar");
                    var select12 = from s in peopleMonitor
                                   where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == strPeriod//item.mes + "-" + strYear
                                   orderby s.ingDateTime
                                   select s;
                    if (select12.Count() == 0)
                    {
                        await DisplayAlert("Aviso", "No se encontraron registros en el mes de diciembre.", "Aceptar");
                    }
                    else
                    {
                        //await DisplayAlert("Aviso", "Mostrar Resultado Histórico.", "Aceptar");
                        await Navigation.PushModalAsync(new TabPageCtrlRecordDetail(strPeriod, intIdUser));
                    }
                    break;
            }
        }
    }
}