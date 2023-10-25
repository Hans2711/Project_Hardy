using Project_Hardy.Classes;
using System.Windows;

namespace Project_Hardy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DBWorker.install("db.db");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de");
        }
    }
}
