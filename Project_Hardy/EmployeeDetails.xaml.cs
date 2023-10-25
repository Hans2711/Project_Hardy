using Project_Hardy.Models;
using System;
using System.Windows;

namespace Project_Hardy
{

    /// <summary>
    /// Interaction logic for EmployeeDetails.xaml
    /// </summary>
    public partial class EmployeeDetails : Window
    {
        public Employee employee { get; set; }

        public EmployeeDetails()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            this.DataContext = employee;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (employee != null)
            {
                employee.persist();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (employee != null)
            {
                employee.persist();
            }
        }
    }
}
