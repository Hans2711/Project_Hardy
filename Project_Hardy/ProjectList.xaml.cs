using Project_Hardy.Models;
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
using System.Windows.Shapes;

namespace Project_Hardy
{
    /// <summary>
    /// Interaction logic for ProjectList.xaml
    /// </summary>
    public partial class ProjectList : Window
    {
        DataRepository dataRepository;
        public ProjectList()
        {
            dataRepository = new DataRepository();
            dataRepository.fetchAll();

            InitializeComponent();

            projectsDataGrid.ItemsSource = dataRepository.projects;
            projectsDataGrid.DataContext = dataRepository.projects;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            for (int i = 0; i < dataRepository.projects.Count; i++)
            {
                for (int j = 0; j < dataRepository.projects[i].steps.Count; j++)
                {
                    if (dataRepository.projects[i].steps[j].is_empty)
                    {
                        dataRepository.projects[i].steps.RemoveAt(j);
                    }
                }
            }
            projectsDataGrid.Items.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            dataRepository.projects.Add(new Project());
            ProjectDetails projectsDetails = new ProjectDetails();
            projectsDetails.project= dataRepository.projects.Last();
            projectsDetails.Show();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (projectsDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < projectsDataGrid.SelectedItems.Count; i++)
                {
                    var item = projectsDataGrid.SelectedItems[i] as Project;

                    ProjectDetails projectDetails= new ProjectDetails();
                    projectDetails.project = item;
                    projectDetails.Show();
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (projectsDataGrid.SelectedItems.Count > 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(Properties.Resources.DeletionCheck, "", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            for (int i = 0; i < projectsDataGrid.SelectedItems.Count; i++)
                            {
                                var item = projectsDataGrid.SelectedItems[i] as Project;

                                item.delete();
                                dataRepository.projects.Remove(item);
                            }
                            projectsDataGrid.Items.Refresh();
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void ganttDiagrammButton_Click(object sender, RoutedEventArgs e)
        {
            if (projectsDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < projectsDataGrid.SelectedItems.Count; i++)
                {
                    var item = projectsDataGrid.SelectedItems[i] as Project;

                    GanttDiagramm ganttDiagramm = new GanttDiagramm();
                    ganttDiagramm.project = item;
                    ganttDiagramm.Show();
                }
            }
        }
    }
}
