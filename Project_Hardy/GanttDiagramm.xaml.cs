using Project_Hardy.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Project_Hardy
{
    /// <summary>
    /// Interaction logic for GanttDiagramm.xaml
    /// </summary>
    public partial class GanttDiagramm : Window
    {
        public Project project { get; set; }

        int totalWidthPixel = 620;

        int totalHeightPixel = 400;

        int horizontalStepSize;

        int verticalStepSize;

        int stepTotal;

        public GanttDiagramm()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DataContext = project;
            if (project.stepCount <= 0) return;

            if (project == null)
                return;

            project.properlyOrderSteps();

            horizontalStepSize = totalWidthPixel / project.getTotalStepDuration();
            stepTotal = project.stepCount;

            setDurationScala();

            verticalStepSize = (totalHeightPixel / stepTotal);

            if (verticalStepSize > 20)
            {
                verticalStepSize -= 10;
            }

            setStepDescriptions();
            setRectagles();

            description.Content = project.description;
        }

        private void setRectagles()
        {
            Brush[] brushes = new Brush[] {
              Brushes.Red,
              Brushes.Orange,
              Brushes.Green,
              Brushes.Blue,
              Brushes.Indigo,
            };

            int i = 1;
            int heightOffset = 0;
            foreach (var step in project.steps)
            {
                Rectangle r = new Rectangle();
                r.Width = step.duration * horizontalStepSize;
                r.Height = objectHeight;

                Random rnd = new Random();
                r.Fill = brushes[rnd.Next() % brushes.Length];

                gdCanvas.Children.Add(r);

                Canvas.SetTop(r, (i * objectHeight) + heightOffset);
                Canvas.SetLeft(r, (project.backtrackPrevDurations(step.prev_identifier) * horizontalStepSize) + 150);

                i++;
            }
        }


        private int objectHeight = 40;

        public void setStepDescriptions()
        {
            int startingPoint = 0;

            for (int i = 1; i < stepTotal + 1; i++)
            {
                startingPoint += verticalStepSize;
                Label label = new Label();
                label.Content = project.steps[i - 1].description + " (" + project.steps[i - 1].identifier + ")";
                label.Height = objectHeight;
                label.Width = 130;

                gdCanvas.Children.Add(label);

                Canvas.SetTop(label, i * objectHeight);
                Canvas.SetLeft(label, 20);
            }

        }


        public void setDurationScala()
        {
            int startingPoint = 150;

            for (int i = 1; i < project.getTotalStepDuration() * 1.5; i++)
            {
                startingPoint += horizontalStepSize;
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
