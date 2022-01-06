using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AppDIAbetes.Utility
{
    public class UtiAppDIAbetes
    {
        public string strMonth = DateTime.Now.Month.ToString();
        public string strYear = DateTime.Now.Year.ToString();

        public string strEtiquetaApp() {
            string strText = "Aplicación diseñada pensando en las personas con diabetes que necesitan llevar un registro y control diario realizado por un glucómetro.";
            return strText;
        }
        public bool ValidateEmail(string strEmail) { 
            return Regex.IsMatch(strEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
        public bool ValidatePassword(string pwd, int minLength = 8,
                                        int numUpper = 1, int numLower = 1,
                                        int numNumbers = 2, int numSpecial = 1)
        {
            //H@dson2013
            // Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            var upper = new System.Text.RegularExpressions.Regex("[A-Z]");
            var lower = new System.Text.RegularExpressions.Regex("[a-z]");
            var number = new System.Text.RegularExpressions.Regex("[0-9]");
            // Special is "none of the above".
            var special = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");

            // Check the length.
            if (pwd.Length < minLength)
                return false;
            // Check for minimum number of occurrences.
            if (upper.Matches(pwd).Count < numUpper)
                return false;
            if (lower.Matches(pwd).Count < numLower)
                return false;
            if (number.Matches(pwd).Count < numNumbers)
                return false;
            if (special.Matches(pwd).Count < numSpecial)
                return false;
            // Passed all checks.
            return true;
        }

        public void strFormatDateES(DateTime dtParameter) {

            dtParameter.ToString("d", CultureInfo.CreateSpecificCulture("es-PE"));
        }

        public string funDesTypeGlucosa(int ingValue)
        {
            string resultImage = null;
            switch (ingValue)
            {
                case 1:
                    resultImage = "Hipoglucemia";
                    break;
                case 2:
                    resultImage = "Nivel Normal";
                    break;
                case 3:
                    resultImage = "Nivel Bueno";
                    break;
                case 4:
                    resultImage = "Nivel Malo";
                    break;
                case 5:
                    resultImage = "Hiperglucemia";
                    break;
            }
            return resultImage;
        }

        public string funIndicatorLOHI(int ingValue)
        {
            string resultRango;

            if (ingValue >= 0 && ingValue <= 20)
            {
                resultRango = "LO";//LO = hipoglucemia severa, inferior a 20 mg / dL.
            }
            else if (ingValue >= 601)
            {
                resultRango = "HI";//HI = hiperglucemia severa, superior a 600 mg / dL.
            }
            else
            {
                resultRango = Convert.ToString(ingValue);
            }
            return resultRango;
        }

        public string funIndicatorImage(int ingValue)
        {
            string resultImage = null;
            switch (ingValue)
            {
                case 0:
                    resultImage = "heartBlue24.png";
                    break;
                case 1:
                    resultImage = "heartBlue24.png";
                    break;
                case 2:
                    resultImage = "heartGreen24.png";
                    break;
                case 3:
                    resultImage = "heartYellow24.png";
                    break;
                case 4:
                    resultImage = "heartOrange24.png";
                    break;
                case 5:
                    resultImage = "heartRed24.png";
                    break;
                case 6:
                    resultImage = "heartRed24.png";
                    //resultImage = "heartBroken24.png";
                    break;
            }
            return resultImage;
        }

        public int funIndicatorValue(int ingValue)
        {
            int resultRango = -1;

            if (ingValue >= 0 && ingValue <= 20)
            {
                resultRango = 0;//LO = hipoglucemia severa, inferior a 20 mg / dL.
            }else if (ingValue >= 21 && ingValue <= 65)
            {
                resultRango = 1;//Valor = hipoglucemia elevada, 21 entre 65 mg / dL.
            }
            else if (ingValue >= 66 && ingValue <= 132)
            {
                resultRango = 2;
            }
            else if (ingValue >= 133 && ingValue <= 197)
            {
                resultRango = 3;
            }
            else if (ingValue >= 198 && ingValue <= 397)
            {
                resultRango = 4;
            }
            else if (ingValue >= 398 && ingValue <= 600)
            {
                resultRango = 5;//Valor = hiperglucemia elevada, 398 entre 600 mg / dL.
            }
            else if (ingValue >= 601)
            {
                resultRango = 6;//HI = hiperglucemia severa, superior a 600 mg / dL.
            }
            return resultRango;
        }

        public string strNameFullMonth(string strValue) {
            string strResult = null;

            switch (strValue)
            {
                case "01"://ENERO
                    strResult = "Enero";
                    break;
                case "02":
                    strResult = "Febrero";
                    break;
                case "03":
                    strResult = "Marzo";
                    break;
                case "04":
                    strResult = "Abril";
                    break;
                case "05":
                    strResult = "Mayo";
                    break;
                case "06":
                    strResult = "Junio";
                    break;
                case "07":
                    strResult = "Julio";
                    break;
                case "08":
                    strResult = "Agosto";
                    break;
                case "09":
                    strResult = "Septiembre";
                    break;
                case "10":
                    strResult = "Octubre";
                    break;
                case "11":
                    strResult = "Noviembre";
                    break;
                case "12":
                    strResult = "Diciembre";
                    break;
            }
            return strResult;
        }


        public string funFormatDayOneToNine(int intValue, string strFormat)
        {

            string strValue = null;

            if (strFormat == "dd")
            {
                switch (intValue)
                {
                    case 1:
                        strValue = "01";
                        break;
                    case 2:
                        strValue = "02";
                        break;
                    case 3:
                        strValue = "03";
                        break;
                    case 4:
                        strValue = "04";
                        break;
                    case 5:
                        strValue = "05";
                        break;
                    case 6:
                        strValue = "06";
                        break;
                    case 7:
                        strValue = "07";
                        break;
                    case 8:
                        strValue = "08";
                        break;
                    case 9:
                        strValue = "09";
                        break;
                    default:
                        strValue = intValue.ToString();
                        break;

                }
            }
            return strValue;
        }


        /// <summary>
        /// Valor de RETORNO en formato 01 AL 12 de acuerdo al valor de la LISTA DESPLEGABLE por meses según formato incial MM
        /// Valor de RETORNO en formato ENE AL DIC de acuerdo al valor de la LISTA DESPLEGABLE por meses según formato incial MMM
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="strFormat"></param>
        /// <returns></returns>
        public string strFormatMonth(int intValue, string strFormat) {

            string strValue = null;
            if (strFormat == "1MM")
            {
                switch (intValue)
                {
                    case 1://ENERO
                        strValue = "01";
                        break;
                    case 2:
                        strValue = "02";
                        break;
                    case 3:
                        strValue = "03";
                        break;
                    case 4:
                        strValue = "04";
                        break;
                    case 5:
                        strValue = "05";
                        break;
                    case 6:
                        strValue = "06";
                        break;
                    case 7:
                        strValue = "07";
                        break;
                    case 8:
                        strValue = "08";
                        break;
                    case 9:
                        strValue = "09";
                        break;
                    case 10:
                        strValue = "10";
                        break;
                    case 11:
                        strValue = "11";
                        break;
                    case 12:
                        strValue = "12";
                        break;
                }
            }
            else if (strFormat == "0MM")
            {
                switch (intValue)
                {
                    case 0://ENERO
                        strValue = "01";
                        break;
                    case 1:
                        strValue = "02";
                        break;
                    case 2:
                        strValue = "03";
                        break;
                    case 3:
                        strValue = "04";
                        break;
                    case 4:
                        strValue = "05";
                        break;
                    case 5:
                        strValue = "06";
                        break;
                    case 6:
                        strValue = "07";
                        break;
                    case 7:
                        strValue = "08";
                        break;
                    case 8:
                        strValue = "09";
                        break;
                    case 9:
                        strValue = "10";
                        break;
                    case 10:
                        strValue = "11";
                        break;
                    case 11:
                        strValue = "12";
                        break;
                }
            }
            else if (strFormat == "0MMN") {
                switch (intValue)
                {
                    case 0://ENERO
                        strValue = "ENE";
                        break;
                    case 1:
                        strValue = "FEB";
                        break;
                    case 2:
                        strValue = "MAR";
                        break;
                    case 3:
                        strValue = "ABR";
                        break;
                    case 4:
                        strValue = "MAY";
                        break;
                    case 5:
                        strValue = "JUN";
                        break;
                    case 6:
                        strValue = "JUL";
                        break;
                    case 7:
                        strValue = "AGO";
                        break;
                    case 8:
                        strValue = "SEP";
                        break;
                    case 9:
                        strValue = "OCT";
                        break;
                    case 10:
                        strValue = "NOV";
                        break;
                    case 11:
                        strValue = "DIC";
                        break;
                }
            }            
            return strValue;
        }

        public string funWhereIngInd(string stringInd)
        {
            string strBetween = null;
            if (stringInd == "1")
            {
                strBetween = "0,1";
            }
            else if (stringInd == "5")
            {
                strBetween = "5,6";
            }
            else
            {
                strBetween = stringInd;
            }
            return strBetween;
        }


        public string funDefaultNote(DateTime dateTime)
        {
            string strAMPM = dateTime.ToString(@"tt");
            string strNota = null;
            
            if (strAMPM == "AM")
            {
                strNota = "Valor registrado en la mañana";
            }
            else
            {
                strNota = "Valor registrado en la tarde";
            }
            return strNota;
        }

        #region INotifyPropertyChanged
        public bool SetProperty<T>(ref T backingStore, T value,
             [CallerMemberName]string propertyName = "",
             Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}