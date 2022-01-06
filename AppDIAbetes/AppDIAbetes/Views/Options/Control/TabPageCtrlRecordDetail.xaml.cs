using AppDIAbetes.Data;
using AppDIAbetes.Models;
using AppDIAbetes.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AppDIAbetes.Views.Options.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabPageCtrlRecordDetail : ContentPage
    {
        UtiAppDIAbetes uti = new UtiAppDIAbetes();
        PeopleMonitorDB peopleMonitorDB = new PeopleMonitorDB();
        int localIdUser;
        string localPeriodo;
        
        public TabPageCtrlRecordDetail(string strPeriodo, int intIdUser)
        {
            localIdUser = intIdUser;
            localPeriodo = strPeriodo;
            InitializeComponent();

            var periodTitule = uti.strNameFullMonth(localPeriodo.Substring(0, 2)).ToUpper() + " DEL " + localPeriodo.Substring(3, 4);
            lblTitulo.Text = "DATOS HISTÓRICOS - " + periodTitule;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IList<PeopleMonitor> peopleMonitor = peopleMonitorDB.peopleMonitorRecordIngDate(localIdUser);
            var selectRecordPeriod = from s in peopleMonitor
                                     where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == localPeriodo
                                     orderby s.ingDateTime
                                     select s;
            lvRecordMonth.ItemsSource = selectRecordPeriod;
        }

        private async void close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}