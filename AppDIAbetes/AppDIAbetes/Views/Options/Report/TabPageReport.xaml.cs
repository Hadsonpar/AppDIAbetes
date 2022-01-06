using AppDIAbetes.Data;
using AppDIAbetes.Interface;
using AppDIAbetes.Models;
using AppDIAbetes.Utility;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Diagnostics;
//using DataChart = Microcharts.Entry;

using Xamarin.Essentials;
using System.IO;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using AppDIAbetes.Views.Options.Info;
using System.Text;

namespace AppDIAbetes.Views.Options.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabPageReport : ContentPage
    {
        //Managing user session
        private string strAuthentifyUser;
        public static string sstrEmailUser;
        LoginAccess OAuthLogin = new LoginAccess(sstrEmailUser);

        private int intId, intIdUser;
        private string strUser, strFullName, strMedido, strEmailMedico;
        private IList<PeopleMonitor> peopleMonitor;

        PeopleMonitorDB peopleMonitorDB = new PeopleMonitorDB();

        UserDB userDB = new UserDB();
        PeopleDB peopleDB = new PeopleDB();

        UtiAppDIAbetes uti = new UtiAppDIAbetes();
        RptAppDIAbetes rpt = new RptAppDIAbetes();

        public TabPageReport()
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
                //strEmailMedico = await vEmailMedico(intIdUser);
            }
            else
            {
                await DisplayAlert("Alerta", "Ocurrio un problema al cargar los registros.", "Aceptar");
            }
        }

        private async Task<string> vEmailMedico(int intIdUser)
        {
            string strEmail = "";

            IList<People> result = await peopleDB.peopleFillIdUse(intIdUser);

            foreach (var item in result)
            {
                strEmail = item.emailMedico;
                strMedido = item.nameMedico;
                strFullName = (item.lastName +", "+ item.firstName).ToUpper();
            }

            return strEmail;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            peopleMonitor = peopleMonitorDB.peopleReporteChartList(intIdUser);
        }

        /// <summary>
        /// Acción BUSCAR de acuerdo al filtro seleccionado
        /// Filtros: @1Mensual o @2Rango por fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void search_Clicked(object sender, EventArgs e)
        {
            string strPeriod = uti.strMonth + "-" + uti.strYear;
            IList<PeopleMonitor> resultFillPeriod;

            //@1Mensual - Ini
            if (pMonth.SelectedIndex == -1 && searchSelectedMM.IsChecked == true)
            {
                await DisplayAlert("Alerta", "Debe seleccionar un mes.", "Aceptar");
                pMonth.Focus();
                return;
            }
            else if(pMonth.SelectedIndex != -1 && searchSelectedMM.IsChecked == true) 
            {   
                resultFillPeriod = rpt.listFillMonthPeopleMonitor(peopleMonitor, strPeriod, 0, null);//Filtro con LINQ
                if (resultFillPeriod.Count() == 0)
                {
                    await DisplayAlert("Aviso", "Sin registros que mostrar.", "Aceptar");
                    pMonth.Focus();
                    return;
                }
                else {
                    //reportFillMonthPeopleMonitor(resultFillPeriod);
                    Grafica1.Chart = rpt.reportFillMonthPeopleMonitor(resultFillPeriod);
                    lvMonitor.ItemsSource = rpt.reportListFillMonthPeopleMonitor(resultFillPeriod);
                }
            }
            //@1Mensual - Fin
            //@2Rango - Ini
            else if (searchSelectedRG.IsChecked == true)
            {
                if (dpFechaIngFin.Date < dpFechaIngIni.Date)
                {
                    await DisplayAlert("Alerta", "La fecha fin no puede ser menor que el inicio.", "Aceptar");
                    dpFechaIngFin.Focus();
                    return;
                }
                else {
                    resultFillPeriod = rpt.listFillRangePeopleMonitor(peopleMonitor, dpFechaIngIni.Date, dpFechaIngFin.Date.AddDays(1), 0, null);//Filtro con LINQ
                    if (resultFillPeriod.Count() == 0) 
                    {
                        await DisplayAlert("Aviso", "Sin registros que mostrar.", "Aceptar");
                        //dpFechaIngFin.Focus();
                        return;
                    } else { 
                        //reportFillMonthPeopleMonitor(resultFillPeriod);
                        Grafica1.Chart = rpt.reportFillMonthPeopleMonitor(resultFillPeriod);
                        lvMonitor.ItemsSource = rpt.reportListFillMonthPeopleMonitor(resultFillPeriod);
                    }
                }
            }
            //@2Rango - Fin
        }

        /// <summary>
        /// Valor 0 = ENERO, se dará el formato de 01 ...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                uti.strMonth = uti.strFormatMonth(selectedIndex, "0MM");
            }
        }
        private void searchSelected(object sender, EventArgs e)
        {
            var item = typeSearch.SelectedItem.ToString();

            if (item == "MM")
            {
                fSecMonth.IsVisible = true;
                fSecRangeDate.IsVisible = false;
                pMonth.SelectedIndex = -1;
            }
            if (item == "RG")
            {

                fSecMonth.IsVisible = true;
                fSecRangeDate.IsVisible = true;
                pMonth.SelectedIndex = -1;
            }
        }
        private void btnViewDettails_Clicked(object sender, EventArgs e)
        {
            var itemBtn = (Button)sender;
            string stringInd = itemBtn.CommandParameter.ToString();
            string strPeriod = uti.strMonth + "-" + uti.strYear;
            string strBetween = null;
            IList<PeopleMonitor> resultFillPeriod;

            if (pMonth.SelectedIndex == -1 && searchSelectedMM.IsChecked == true)
            {
                DisplayAlert("Alerta", "Debe seleccionar un mes.", "Aceptar");
                pMonth.Focus();
                return;
            }
            else if (pMonth.SelectedIndex != -1 && searchSelectedMM.IsChecked == true)
            {
                strBetween = uti.funWhereIngInd(stringInd);
                resultFillPeriod = rpt.listFillMonthPeopleMonitor(peopleMonitor, strPeriod, 1, strBetween);//Filtro con LINQ

                if (resultFillPeriod.Count() == 0)
                {
                    DisplayAlert("Aviso", "Sin registros que mostrar.", "Aceptar");
                }
                else {
                    autocompletelvViewDetails(resultFillPeriod, stringInd);
                }
            }
            else if (searchSelectedRG.IsChecked == true)
            {
                if (dpFechaIngFin.Date < dpFechaIngIni.Date)
                {
                    DisplayAlert("Alerta", "La fecha fin no puede ser menor que el inicio.", "Aceptar");
                    dpFechaIngFin.Focus();
                    return;
                }
                else
                {
                    strBetween = uti.funWhereIngInd(stringInd);
                    resultFillPeriod = rpt.listFillRangePeopleMonitor(peopleMonitor, dpFechaIngIni.Date, dpFechaIngFin.Date.AddDays(1), 1, strBetween);//Filtro con LINQ

                    if (resultFillPeriod.Count() == 0)
                    {
                        DisplayAlert("Aviso", "Sin registros que mostrar.", "Aceptar");
                    }
                    else
                    {
                        autocompletelvViewDetails(resultFillPeriod, stringInd);
                    }
                }
            }
        }

        void autocompletelvViewDetails(IList<PeopleMonitor> resultFillPeriod, string stringInd) {

            foreach (var item in resultFillPeriod)
            {
                //item.ingVal (en realidad registra valores interos, HI y LO) Ahoro se usa como un autocompletar para la DesTypeGlucosa
                //item.imgInd (en realidad registra el nombre de las imagenes para el ctrl de monitorero) Ahoro se usa como un autocompletar para distingir de Severa y Elevada
                if (item.ingInd == 0)
                {
                    item.ingVal = "H. Severa";//uti.funDesTypeGlucosa(1) + " Severa";
                    item.imgInd = "ic_LO005CB2.png";
                }
                else if (item.ingInd == 1)
                {
                    item.ingVal = "H. Elevada";//uti.funDesTypeGlucosa(1) + " Elevada";
                    item.imgInd = "ic_LO59A7F1.png";
                }
                else if (item.ingInd == 2)
                {
                    item.ingVal = uti.funDesTypeGlucosa(2);
                }
                else if (item.ingInd == 3)
                {
                    item.ingVal = uti.funDesTypeGlucosa(3);
                }
                else if (item.ingInd == 4)
                {
                    item.ingVal = uti.funDesTypeGlucosa(4);
                }
                else if (item.ingInd == 5)
                {
                    item.ingVal = "H. Elevada";//uti.funDesTypeGlucosa(5) + " Elevada";
                    item.imgInd = "ic_HIF25858.png";
                }
                else if (item.ingInd == 6)
                {
                    item.ingVal = "H. Severa";//uti.funDesTypeGlucosa(5) + " Severa";
                    item.imgInd = "ic_HIE60000.png";
                }
            }

            lvViewDetails.ItemsSource = resultFillPeriod;
            
            if (stringInd == "1")
            {   
                fGraficaDonut.IsVisible = true;
                GraficaDonut.Chart = rpt.reportFillLOHIPeopleMonitor(resultFillPeriod, "LO");
            }
            else if (stringInd == "5")
            {
                fGraficaDonut.IsVisible = true;
                GraficaDonut.Chart = rpt.reportFillLOHIPeopleMonitor(resultFillPeriod, "HI");
            }
            else
            {
                fGraficaDonut.IsVisible = false;
            }

            popupViewDetails.IsVisible = true;
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            popupViewDetails.IsVisible = false;
        }

        /// <summary>
        /// Acción Exportar datos a Excel y Envíar el archivos Excel por correo electrónico al del médico tratante
        /// Filtros: @1Mensual o @2Rango por fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnItemExport(object sender, EventArgs e)
        {
            string strPeriod = null, strMMMYYYYMES = null, strMMMYYYYRANGO = null, strBODY = null;
            IList<PeopleMonitor> resultFillPeriod;

            strEmailMedico = await vEmailMedico(intIdUser);

            if (String.IsNullOrEmpty(strFullName)) {
                strFullName = strUser;
            }
            //@1Mensual - Ini
            if (pMonth.SelectedIndex == -1 && searchSelectedMM.IsChecked == true)
            {
                await DisplayAlert("Alerta", "Debe seleccionar un mes.", "Aceptar");
                pMonth.Focus();
                return;
            }
            else if (pMonth.SelectedIndex != -1 && searchSelectedMM.IsChecked == true)
            {
                strPeriod = uti.strMonth + "-" + uti.strYear;
                strMMMYYYYMES = uti.strNameFullMonth(uti.strMonth).ToUpper().Substring(0, 3) + "-" + uti.strYear;

                resultFillPeriod = rpt.listFillMonthPeopleMonitor(peopleMonitor, strPeriod, 0, null);//Filtro con LINQ
                //Ruta y nombre del reporte generado
                var path = DependencyService.Get<IExportFilesToLocation>().GetFolderLocation() + "RPT " + strUser.ToUpper() + " PERIODO " + strMMMYYYYMES + ".xlsx";

                if (resultFillPeriod.Count() > 0) {
                    await rptExportExcel(resultFillPeriod, path, strPeriod);

                    //await DisplayAlert("Aviso", "Se guardo el reporte en la siguiente ruta :" + path, "Aceptar");
                    List<string> toAddress = new List<string>();

                    if (strEmailMedico == null || strEmailMedico == "")
                    {
                        /*var result = await DisplayAlert("Confirmar", "Usted aun no cuenta con un médico tratante, puede acceder a INFO desde la opción Mi Información para agregar los datos de tu médico tratante, en todo caso el reporte se enviará tu correo eléctronico "+ strEmailMedico, "SI", "NO");*/
                        var result = await DisplayAlert("Confirmar", "Usted aun no cuenta con un médico tratante, desea agregar los datos de tu médico tratante.", "SI", "NO");

                        if (result)
                        {
                            await Navigation.PushAsync(new PageInfo());
                            return;
                        }
                        else
                        {
                            toAddress.Add(strAuthentifyUser);
                        }
                    }
                    else
                    {
                        toAddress.Add(strEmailMedico);
                    }

                    strBODY = rpt.funBodyEmail(strFullName, strMMMYYYYMES);
                    await SendEmail("App DIAbetes - RPT " + strUser.ToUpper() + " PERIODO " + strMMMYYYYMES, strBODY, toAddress, path);
                }
                else
                {
                    await DisplayAlert("Aviso", "No se encontraron datos a exportar.", "Aceptar");
                };
            }
            //@1Mensual - Fin
            //@2Rango - Ini
            else if (searchSelectedRG.IsChecked == true)
            {
                if (dpFechaIngFin.Date < dpFechaIngIni.Date)
                {
                    await DisplayAlert("Alerta", "La fecha fin no puede ser menor que el inicio.", "Aceptar");
                    dpFechaIngFin.Focus();
                    return;
                }
                else
                {
                    resultFillPeriod = rpt.listFillRangePeopleMonitor(peopleMonitor, dpFechaIngIni.Date, dpFechaIngFin.Date.AddDays(1), 0, null);//Filtro con LINQ

                    if (resultFillPeriod.Count() > 0)
                    {

                        string strFechaIngIniDay = uti.funFormatDayOneToNine(dpFechaIngIni.Date.Day, "dd");
                        string strFechaIngFinDay = uti.funFormatDayOneToNine(dpFechaIngFin.Date.Day, "dd"); 
                        string strFechaIngIniMonth = uti.strNameFullMonth(uti.strFormatMonth(dpFechaIngIni.Date.Month, "1MM"));
                        string strFechaIngFinMonth = uti.strNameFullMonth(uti.strFormatMonth(dpFechaIngFin.Date.Month, "1MM"));

                        if (strFechaIngFinMonth.Equals(strFechaIngIniMonth))
                        {
                            if (strFechaIngFinDay.Equals(strFechaIngIniDay)) {//Cuando esta seleccionado en el mismo mes y el  mismo día
                                //strPeriod = dpFechaIngFin.Date.Month.ToString() + dpFechaIngFin.Date.Year.ToString();
                                strMMMYYYYRANGO = strFechaIngFinDay + "-" + strFechaIngFinMonth.ToUpper().Substring(0, 3) + "-" + dpFechaIngFin.Date.Year.ToString();
                            }
                            else
                            {//Cuando el rango esta seleccionado en el mismo mes y con diferencia de días
                                strMMMYYYYRANGO = strFechaIngIniDay + " AL " + strFechaIngFinDay +"-"+ strFechaIngFinMonth.ToUpper().Substring(0, 3) + "-"+ dpFechaIngFin.Date.Year.ToString();
                            }
                        }
                        else {
                                strMMMYYYYRANGO = strFechaIngIniDay +"-"+ strFechaIngIniMonth.ToUpper().Substring(0, 3) + "-"+ dpFechaIngIni.Date.Year.ToString() + " AL "+ 
                                                    strFechaIngFinDay + "-" + strFechaIngFinMonth.ToUpper().Substring(0, 3) + "-" + dpFechaIngFin.Date.Year.ToString();
                        }

                        var path = DependencyService.Get<IExportFilesToLocation>().GetFolderLocation() + "RPT " + strUser.ToUpper() + " PERIODO " + strMMMYYYYRANGO + ".xlsx";

                        await rptExportExcel(resultFillPeriod, path, strMMMYYYYRANGO);
                        //await DisplayAlert("Aviso", "Se guardo el reporte en la siguiente ruta :" + path, "Aceptar");
                        List<string> toAddress = new List<string>();
                        //toAddress.Add("hadson1@gmail.com");

                        if (strEmailMedico == null || strEmailMedico == "")
                        {
                            /*var result = await DisplayAlert("Confirmar", "Usted aun no cuenta con un médico tratante, puede acceder a INFO desde la opción Mi Información para agregar los datos de tu médico tratante, en todo caso el reporte se enviará tu correo eléctronico "+ strEmailMedico, "SI", "NO");*/
                            var result = await DisplayAlert("Confirmar", "Usted aun no cuenta con un médico tratante, desea agregar los datos de tu médico tratante.", "SI", "NO");

                            if (result)
                            {
                                await Navigation.PushAsync(new TabPageInfo());
                                return;
                            }
                            else{
                                toAddress.Add(strAuthentifyUser);
                            }
                        }
                        else
                        {
                            toAddress.Add(strEmailMedico);
                        }

                        strBODY = rpt.funBodyEmail(strFullName, strMMMYYYYRANGO);
                        await SendEmail("App DIAbetes - RPT " + strUser.ToUpper() + " PERIODO " + strMMMYYYYRANGO, strBODY, toAddress, path);
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "No se encontraron datos a exportar.", "Aceptar");
                    };
                }
            }
            //@2Rango - Fin
        }

        public async Task SendEmail(string subject, string body, List<string> recipients, string strAdjunto)
        {
            try
            {
                //Propiedades del mensaje
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                var fn = strAdjunto;// "Attachment.txt";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                message.Attachments.Add(new EmailAttachment(file));

                //API que se encarga de abrir el cliente como el Gmail, Outlook u otros para realizar el envío del mensaje
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Email is not supported on this device 
                await DisplayAlert("Error", fnsEx.ToString(), "OK");
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                await DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        async Task rptExportExcel(IList<PeopleMonitor> resultFillPeriod, string strPath, string pStrPeriod) {
            //string strResult = null;

            // Granted storage permission
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }

            if (resultFillPeriod.Count() > 0)
            {
                try
                {
                    using (SpreadsheetDocument document = SpreadsheetDocument.Create(strPath, SpreadsheetDocumentType.Workbook))
                    {
                        WorkbookPart workbookPart = document.AddWorkbookPart();
                        workbookPart.Workbook = new Workbook();

                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet();

                        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "VALORES DE " + pStrPeriod };
                        sheets.Append(sheet);

                        workbookPart.Workbook.Save();

                        SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                        int indItem = 0;
                        // Constructing header
                        Row row = new Row();

                        row.Append(
                            ConstructCell("ITEM", CellValues.String),
                            ConstructCell("VALOR", CellValues.String),
                            ConstructCell("GLUCOSA", CellValues.String),
                            ConstructCell("FECHA", CellValues.String),
                            ConstructCell("NOTA", CellValues.String)
                            );

                        // Insert the header row to the Sheet Data
                        sheetData.AppendChild(row);

                        // Recorrer solo los registro segun el filtro
                        foreach (var item in resultFillPeriod)
                        {
                            indItem += 1;
                            row = new Row();

                            if (item.ingInd == 0)
                            {
                                item.ingVal = uti.funDesTypeGlucosa(1) + " Severa";
                            }
                            else if (item.ingInd == 1)
                            {
                                item.ingVal = uti.funDesTypeGlucosa(1) + " Elevada";
                            }
                            else if (item.ingInd == 2)
                            {
                                item.ingVal = uti.funDesTypeGlucosa(2);
                            }
                            else if (item.ingInd == 3)
                            {
                                item.ingVal = uti.funDesTypeGlucosa(3);
                            }
                            else if (item.ingInd == 4)
                            {
                                item.ingVal = uti.funDesTypeGlucosa(4);
                            }
                            else if (item.ingInd == 5)
                            {
                                item.ingVal = uti.funDesTypeGlucosa(5) + " Elevada";
                            }
                            else if (item.ingInd == 6)
                            {
                                item.ingVal = uti.funDesTypeGlucosa(5) + " Severa";
                            }

                            row.Append(
                                ConstructCell(indItem.ToString(), CellValues.String),
                                ConstructCell(item.ingValues.ToString()+" "+ item.typeGloc, CellValues.String),
                                //ConstructCell(item.typeGloc, CellValues.String),
                                ConstructCell(item.ingVal, CellValues.String),
                                ConstructCell(item.ingDateTime.ToString(), CellValues.String),
                                ConstructCell(item.comentVal.ToString(), CellValues.String));

                            sheetData.AppendChild(row);
                        }

                        worksheetPart.Worksheet.Save();
                        //MessagingCenter.Send(this, "DataExportedSuccessfully");
                        //strResult = "Export";
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ERROR: " + ex.Message);
                    //strResult = "Error" + ex.Message;
                }
            }
            //return strResult;
        }

        /* To create cell in Excel */
        private DocumentFormat.OpenXml.Spreadsheet.Cell ConstructCell(string value, CellValues dataType)
        {
            return new DocumentFormat.OpenXml.Spreadsheet.Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        //public ICommand ExportToExcelCommand { get; set; }

        private ObservableCollection<PeopleMonitor> _developers;
        public ObservableCollection<PeopleMonitor> Developers
        {
            get { return _developers; }
            set { uti.SetProperty(ref _developers, value); }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { uti.SetProperty(ref _filePath, value); }
        }
    }
}