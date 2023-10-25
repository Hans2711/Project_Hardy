using System.Windows;

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
