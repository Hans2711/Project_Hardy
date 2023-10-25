using Project_Hardy.Models;
using System;
using System.Windows;

namespace Project_Hardy
{
    /// <summary>
    /// Interaction logic for ProjectDetails.xaml
    /// </summary>
    public partial class ProjectDetails : Window
    {
        public Project project { get; set; }
        public ProjectDetails()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.DataContext = project;

            project.properlyOrderSteps();

            if (project.steps != null)
            {
                this.stepsDataGrid.ItemsSource = project.steps;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (project != null)
            {
                project.persist();
            }
            this.stepsDataGrid.ItemsSource = null;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (project != null)
            {
                project.persist();
            }
            this.stepsDataGrid.ItemsSource = null;
        }

        private void validateStepsButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < project.steps.Count; i++)
            {
                if (project.steps[i] == null) continue;

                project.steps[i].persist(project.id);
                if (project.steps[i].id == 0 || project.steps[i].deletionMark)
                {
                    project.steps.Remove(project.steps[i]);
                }
            }
            project.properlyOrderSteps();
            stepsDataGrid.ItemsSource = project.steps;
            stepsDataGrid.Items.Refresh();
        }

        private void deleteStepButton_Click(object sender, RoutedEventArgs e)
        {
            if (stepsDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < stepsDataGrid.SelectedItems.Count; i++)
                {
                    var item = stepsDataGrid.SelectedItems[i] as ProjectStep;

                    item.delete();
                    project.steps.Remove((ProjectStep)item);
                }
            }

            stepsDataGrid.Items.Refresh();
        }
    }
}
