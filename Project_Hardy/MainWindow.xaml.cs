using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_Hardy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeList employeeList = new EmployeeList();
            employeeList.Show();
            this.Close();
        }

        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectList projectList = new ProjectList();
            projectList.Show();
            this.Close();
        }

        private void englandButton_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            EmployeesButton.Content = Properties.Resources.Employees;
            ProjectsButton.Content = Properties.Resources.Projects;
        }

        private void germanyButton_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de");
            EmployeesButton.Content = Properties.Resources.Employees;
            ProjectsButton.Content = Properties.Resources.Projects;
        }
    }
}
