using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDIAbetes.Views.Options
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageHome : ContentPage
    {
        public PageHome()
        {
            InitializeComponent();
        }
        private void acercaDe_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new PageAbout());
        }
    }
}