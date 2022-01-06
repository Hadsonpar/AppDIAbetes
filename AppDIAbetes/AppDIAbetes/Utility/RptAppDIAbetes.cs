using AppDIAbetes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using DataChart = Microcharts.Entry;

namespace AppDIAbetes.Utility
{
    public class RptAppDIAbetes
    {
        UtiAppDIAbetes uti = new UtiAppDIAbetes();

        #region Filtros con LINQ
        public IList<PeopleMonitor> listFillMonthPeopleMonitor(IList<PeopleMonitor> fillpeopleMonitor, string pStrPeriod, int intViewDetails, string strInd)
        {
            var resultList = from s in fillpeopleMonitor
                             where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == pStrPeriod
                             orderby s.ingDateTime descending
                             select s;

            if (intViewDetails == 1)
            {
                resultList = from s in fillpeopleMonitor
                             where DateTime.Parse(s.ingDateTime.ToString()).ToString("MM-yyyy") == pStrPeriod && strInd.Contains(s.ingInd.ToString())
                             orderby s.ingDateTime descending
                             select s;
            }
            return resultList.ToList();
        }

        public IList<PeopleMonitor> listFillRangePeopleMonitor(IList<PeopleMonitor> fillpeopleMonitor, DateTime strStart, DateTime strEnd, int intViewDetails, string strInd)
        {
            var resultList = from s in fillpeopleMonitor
                             where s.ingDateTime >= strStart && s.ingDateTime <= strEnd
                             orderby s.ingDateTime descending
                             select s;

            if (intViewDetails == 1)
            {
                resultList = from s in fillpeopleMonitor
                             where s.ingDateTime >= strStart && s.ingDateTime <= strEnd && strInd.Contains(s.ingInd.ToString())
                             orderby s.ingDateTime descending
                             select s;
            }
            return resultList.ToList();
        }
        #endregion

        #region Graficas con Microcharts y Detalle (Lista)
        /// <summary>
        /// Método para Gráfica de barras con Microcharts BarChart
        /// </summary>
        /// <param name="resultFillPeriod"></param>
        /// <returns></returns>
        public Microcharts.BarChart reportFillMonthPeopleMonitor(IList<PeopleMonitor> resultFillPeriod)
        {
            Microcharts.BarChart barChart = new Microcharts.BarChart();

            var ingIndHipog = resultFillPeriod.Count(x => x.ingInd == 0) + resultFillPeriod.Count(x => x.ingInd == 1);
            var ingIndNormal = resultFillPeriod.Count(x => x.ingInd == 2);
            var ingIndBueno = resultFillPeriod.Count(x => x.ingInd == 3);
            var ingIndMalo = resultFillPeriod.Count(x => x.ingInd == 4);
            var ingIndHiper = resultFillPeriod.Count(x => x.ingInd == 5) + resultFillPeriod.Count(x => x.ingInd == 6);

            List<DataChart> dataChartList = new List<DataChart> {
                    new DataChart(ingIndHipog)
                    {
                        Color = SkiaSharp.SKColor.Parse("#005CB2"),
                        TextColor = SkiaSharp.SKColor.Parse("#005CB2"),
                        Label = uti.funDesTypeGlucosa(1),
                        ValueLabel = ingIndHipog.ToString()
                    },
                    new DataChart(ingIndNormal)
                    {
                        Color = SkiaSharp.SKColor.Parse("#40BE09"),
                        TextColor = SkiaSharp.SKColor.Parse("#40BE09"),
                        Label = uti.funDesTypeGlucosa(2),
                        ValueLabel = ingIndNormal.ToString()
                    },
                    new DataChart(ingIndBueno)
                    {
                        Color = SkiaSharp.SKColor.Parse("#F2A900"),
                        TextColor = SkiaSharp.SKColor.Parse("#F2A900"),
                        Label = uti.funDesTypeGlucosa(3),
                        ValueLabel = ingIndBueno.ToString()
                    },
                    new DataChart(ingIndMalo)
                    {
                        Color = SkiaSharp.SKColor.Parse("#F06D00"),
                        TextColor = SkiaSharp.SKColor.Parse("#F06D00"),
                        Label = uti.funDesTypeGlucosa(4),
                        ValueLabel = ingIndMalo.ToString()
                    },
                    new DataChart(ingIndHiper)
                    {
                        Color = SkiaSharp.SKColor.Parse("#E60000"),
                        TextColor = SkiaSharp.SKColor.Parse("#E60000"),
                        Label = uti.funDesTypeGlucosa(5),
                        ValueLabel = ingIndHiper.ToString()
                    },
                };

            //Grafica1.Chart = new Microcharts.BarChart
            barChart = new Microcharts.BarChart
            {
                LabelTextSize = 22f,
                Entries = dataChartList
            };

            return barChart;
            /*List<ReportGrid> reportGrids = new List<ReportGrid>();

            reportGrids.Add(new ReportGrid() { ingCount = ingIndHipog, ingDes = uti.funDesTypeGlucosa(1), ingImg = uti.funIndicatorImage(1), ingInd = 1 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndNormal, ingDes = uti.funDesTypeGlucosa(2), ingImg = uti.funIndicatorImage(2), ingInd = 2 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndBueno, ingDes = uti.funDesTypeGlucosa(3), ingImg = uti.funIndicatorImage(3), ingInd = 3 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndMalo, ingDes = uti.funDesTypeGlucosa(4), ingImg = uti.funIndicatorImage(4), ingInd = 4 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndHiper, ingDes = uti.funDesTypeGlucosa(5), ingImg = uti.funIndicatorImage(5), ingInd = 5 });

            lvMonitor.ItemsSource = reportGrids;*/
        }

        /// <summary>
        /// Método para listar los valores de acuerdo a la Cantidad según la Gráfica de barras
        /// </summary>
        /// <param name="resultFillPeriod"></param>
        /// <returns></returns>
        public List<ReportGrid> reportListFillMonthPeopleMonitor(IList<PeopleMonitor> resultFillPeriod)
        {
            List<ReportGrid> reportGrids = new List<ReportGrid>();

            var ingIndHipog = resultFillPeriod.Count(x => x.ingInd == 0) + resultFillPeriod.Count(x => x.ingInd == 1);
            var ingIndNormal = resultFillPeriod.Count(x => x.ingInd == 2);
            var ingIndBueno = resultFillPeriod.Count(x => x.ingInd == 3);
            var ingIndMalo = resultFillPeriod.Count(x => x.ingInd == 4);
            var ingIndHiper = resultFillPeriod.Count(x => x.ingInd == 5) + resultFillPeriod.Count(x => x.ingInd == 6);

            reportGrids.Add(new ReportGrid() { ingCount = ingIndHipog, ingDes = uti.funDesTypeGlucosa(1), ingImg = uti.funIndicatorImage(1), ingInd = 1 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndNormal, ingDes = uti.funDesTypeGlucosa(2), ingImg = uti.funIndicatorImage(2), ingInd = 2 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndBueno, ingDes = uti.funDesTypeGlucosa(3), ingImg = uti.funIndicatorImage(3), ingInd = 3 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndMalo, ingDes = uti.funDesTypeGlucosa(4), ingImg = uti.funIndicatorImage(4), ingInd = 4 });
            reportGrids.Add(new ReportGrid() { ingCount = ingIndHiper, ingDes = uti.funDesTypeGlucosa(5), ingImg = uti.funIndicatorImage(5), ingInd = 5 });

            return reportGrids;
        }
        /// <summary>
        /// Método para Gráfica circular con Microcharts DonutChart
        /// </summary>
        /// <param name="resultFillPeriod"></param>
        /// <param name="intTypeLOHI"></param>
        /// <returns></returns>
        public Microcharts.DonutChart reportFillLOHIPeopleMonitor(IList<PeopleMonitor> resultFillPeriod, string intTypeLOHI)
        {
            Microcharts.DonutChart donutChart = new Microcharts.DonutChart();
            List<DataChart> dataChartList = new List<DataChart>();

            if (intTypeLOHI == "LO")
            {
                var indTypeLOSevera = resultFillPeriod.Count(x => x.ingInd == 0);
                var indTypeLOElevada = resultFillPeriod.Count(x => x.ingInd == 1);

                dataChartList = new List<DataChart> {
                    new DataChart(indTypeLOElevada)
                    {
                        Color = SkiaSharp.SKColor.Parse("#59A7F1"),
                        TextColor = SkiaSharp.SKColor.Parse("#59A7F1"),
                        Label = uti.funDesTypeGlucosa(1)+" Elevada",
                        ValueLabel = indTypeLOElevada.ToString()
                    },
                    new DataChart(indTypeLOSevera)
                    {
                        Color = SkiaSharp.SKColor.Parse("#005CB2"),
                        TextColor = SkiaSharp.SKColor.Parse("#005CB2"),
                        Label = uti.funDesTypeGlucosa(1)+" Severa",
                        ValueLabel = indTypeLOSevera.ToString()
                    }
                };
            }
            else if (intTypeLOHI == "HI")
            {
                var ingIndTypeHIElevada = resultFillPeriod.Count(x => x.ingInd == 5);
                var ingIndTypeHISevera = resultFillPeriod.Count(x => x.ingInd == 6);

                dataChartList = new List<DataChart> {
                    new DataChart(ingIndTypeHIElevada)
                    {
                        Color = SkiaSharp.SKColor.Parse("#F25858"),
                        TextColor = SkiaSharp.SKColor.Parse("#F25858"),
                        Label = uti.funDesTypeGlucosa(5)+ " Elevada",
                        ValueLabel = ingIndTypeHIElevada.ToString()
                    },
                    new DataChart(ingIndTypeHISevera)
                    {
                        Color = SkiaSharp.SKColor.Parse("#E60000"),
                        TextColor = SkiaSharp.SKColor.Parse("#E60000"),
                        Label = uti.funDesTypeGlucosa(5)+ " Severa",
                        ValueLabel = ingIndTypeHISevera.ToString()
                     },
                 };
            }
            //GraficaDonut.Chart = new Microcharts.DonutChart
            donutChart = new Microcharts.DonutChart
            {
                LabelTextSize = 28f,
                Entries = dataChartList
            };

            return donutChart;
        }
        #endregion

        public string funBodyEmail(string strname, string strPeriodo)
        {
            string strBody = null;

            StringBuilder sb = new StringBuilder();
            sb.Append("Estimad@:\n\n");
            sb.Append("Se adjunta el reporte del paciente " + strname + " del periodo " + strPeriodo + ".");
            //sb.Append("Se adjunta el reporte del PACIENTE " + strname + " del PERIODO" + strPeriodo + "\n\n\n");
            //sb.Append("Muchisímas Gracias\nEstamos súper felices por hacer uso de la App DÍAbetes.");
            strBody = sb.ToString();

            return strBody;
        }
    }
}