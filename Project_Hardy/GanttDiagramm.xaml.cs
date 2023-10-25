using Project_Hardy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project_Hardy
{
    /// <summary>
    /// Interaction logic for GanttDiagramm.xaml
    /// </summary>
    public partial class GanttDiagramm : Window
    {
        public Project project { get; set; }

        int totalWidthPixel = 350;

        int totalHeightPixel = 400;

        int verticalStepSize;

        int horizontalStepSize;

        int stepTotal;

        public GanttDiagramm()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (project == null)
                return;

            verticalStepSize = totalWidthPixel / project.getTotalStepDuration();
            stepTotal = project.stepCount;

            setDurationScala();

            horizontalStepSize = (totalHeightPixel / stepTotal);

            if (horizontalStepSize > 20)
            {
                horizontalStepSize -= 10;
            }

            setStepDescriptions();
        }


        public void setStepDescriptions()
        {
            int startingPoint = 0;

            for (int i = 1; i < stepTotal + 1; i++)
            {
                startingPoint += horizontalStepSize;
                Label label = new Label();
                label.Content = project.steps[i - 1].description + " (" + project.steps[i - 1].identifier + ")";
                label.Height = 40;
                label.Width = 130;

                gdCanvas.Children.Add(label);

                Canvas.SetTop(label, i * 40);
                Canvas.SetLeft(label, 20);
            }

        }


        public void setDurationScala()
        {
            int startingPoint = 150;

            for (int i = 1; i < project.getTotalStepDuration() * 1.5; i++)
            {
                startingPoint += verticalStepSize;
                Line line = new Line();

                line.X1 = startingPoint;
                line.X2 = startingPoint;
                line.Y1 = 400;
                line.Y2 = 410;
                line.StrokeThickness = 2;
                line.Stroke = new SolidColorBrush(Colors.Black);

                Label label = new Label();
                label.Content = "" + i;

                gdCanvas.Children.Add(label);
                Canvas.SetTop(label, 420);
                Canvas.SetLeft(label, startingPoint - 8);

                line.Visibility = Visibility.Visible;
                gdCanvas.Children.Add(line);
            }
        }
    }
}
