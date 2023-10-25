using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Project_Hardy.Classes;
using System.Data.SQLite;

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
