using Java.IO;
using AppDIAbetes.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppDIAbetes.Droid.ExportFilesToLocation))]
namespace AppDIAbetes.Droid
{
    public class ExportFilesToLocation : IExportFilesToLocation
    {
        public ExportFilesToLocation()
        {
        }

        public string GetFolderLocation()
        {
            string root = null;
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            }
            else
                root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            File myDir = new File(root + "/DIAbetes");
            if (!myDir.Exists())
                myDir.Mkdir();

            return root + "/DIAbetes/";
        }
    }
}