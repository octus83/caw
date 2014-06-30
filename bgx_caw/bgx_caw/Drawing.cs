using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace bgx_caw
{
    /// <summary>
    /// Teilklasse der MainWindow Klasse
    /// Funktionaltät die sich mit dem Zeichnes auf dem Canvas beschäftigt
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// speichert einen aktuellen Punkt
        /// </summary>
        private Point currentPoint = new Point();
        /// <summary>
        /// Wird die Maustaste gedrückt oder der Bildschirm berührt
        /// wird das MouseDown event ausgelöst welches dann einfach nur 
        /// die aktuelle Position speichert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(view);
        }
        /// <summary>
        /// Bei bewegung der Maus oder bei Bewegung einer Touchgeste
        /// wird das event ausgelöst und verbindet den aktuellen Punkt
        /// mit den vorherigen punkt zu einer Line. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (drawState != DrawState.None)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Line line = new Line();
                    line.StrokeThickness = 4;
                    if (drawState == DrawState.green)
                        line.Stroke = Brushes.LightGreen;
                    else if (drawState == DrawState.red)
                        line.Stroke = Brushes.Red;
                    else if (drawState == DrawState.mark)
                        line.Stroke = Brushes.Gray;

                    line.X1 = currentPoint.X;
                    line.Y1 = currentPoint.Y;
                    line.X2 = e.GetPosition(view).X;
                    line.Y2 = e.GetPosition(view).Y;

                    currentPoint = e.GetPosition(view);

                    view.Children.Add(line);
                }
            }
        }
       
        
    }
}
