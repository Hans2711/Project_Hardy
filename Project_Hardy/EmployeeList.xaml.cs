using Project_Hardy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Resources;

namespace Project_Hardy
{
    /// <summary>
    /// Interaction logic for EmployeeList.xaml
    /// </summary>
    public partial class EmployeeList : Window
    {
        DataRepository dataRepository;

        public EmployeeList()
        {
            dataRepository = new DataRepository();
            dataRepository.fetchAll();

            InitializeComponent();

            employeesDataGrid.ItemsSource = dataRepository.employees;
            employeesDataGrid.DataContext = dataRepository.employees;
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            dataRepository.employees.Add(new Employee());
            EmployeeDetails employeeDetails = new EmployeeDetails();
            employeeDetails.employee = dataRepository.employees.Last();
            employeeDetails.Show();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeesDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < employeesDataGrid.SelectedItems.Count; i++)
                {
                    var item = employeesDataGrid.SelectedItems[i] as Employee;

                    EmployeeDetails employeeDetails = new EmployeeDetails();
                    employeeDetails.employee = item;
                    employeeDetails.Show();
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeesDataGrid.SelectedItems.Count > 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(Properties.Resources.DeletionCheck, "", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            for (int i = 0; i < employeesDataGrid.SelectedItems.Count; i++)
                            {
                                var item = employeesDataGrid.SelectedItems[i] as Employee;

                                item.delete();
                                dataRepository.employees.Remove(item);
                            }   
                            employeesDataGrid.Items.Refresh();
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            employeesDataGrid.Items.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void employeesDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

    }
}
