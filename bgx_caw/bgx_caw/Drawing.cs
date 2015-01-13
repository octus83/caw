using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        /// Boolean der dafür sorgt, dass ein sizeChangeEvent alle x sekunden ausgelöst wird
        /// </summary>
        Boolean sizeChangedEventBreak = true;
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
                    Line hiddenLine = new Line();
                    line.StrokeThickness = 4;
                    hiddenLine.StrokeThickness = 6;
                    if (drawState == DrawState.green)
                    {
                        line.Stroke = Brushes.LightGreen;
                        hiddenLine.Stroke = Brushes.LightGreen;
                    }                       
                    else if (drawState == DrawState.red)
                    {
                        line.Stroke = Brushes.Red;
                        hiddenLine.Stroke = Brushes.Red;
                    }                      
                    else if (drawState == DrawState.mark)
                    {
                        line.Stroke = Brushes.Gray;
                        hiddenLine.Stroke = Brushes.Gray;
                    }
                      
                    line.X1 = currentPoint.X;
                    line.Y1 = currentPoint.Y;
                    line.X2 = e.GetPosition(view).X;
                    line.Y2 = e.GetPosition(view).Y;

                    hiddenLine.X1 = currentPoint.X;
                    hiddenLine.Y1 = currentPoint.Y;
                    hiddenLine.X2 = e.GetPosition(view).X;
                    hiddenLine.Y2 = e.GetPosition(view).Y;

                    currentPoint = e.GetPosition(view);
                    transformLine(hiddenLine);
                    view.Children.Add(line);    
                    viewHidden.Children.Add(hiddenLine);
                }
            }
        }
        
        /// <summary>
        /// Sizechange Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_SizeChanged(object sender, SizeChangedEventArgs e)
        {       
            if(sizeChangedEventBreak)
            {
                logger.log("SIZE CHANGE TO : Height: " + view.ActualHeight + " Width: " + view.ActualWidth, "Drawing.cs");
                sizeChangedEventBreak = false;
                Thread workerThread = new Thread(sizeChangedBreak);
                workerThread.Start();
            }           
        }
        /// <summary>
        /// Sizechange Event soll nur jede Sekunde ausgelöst werden und nicht öfters
        /// </summary>
        private void sizeChangedBreak()
        {          
            Thread.Sleep(1000);
            sizeChangedEventBreak = true;
        }

        /// <summary>
        /// Transformiert Linien auf die Auflösung des Hidden Canvas
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private Line transformLine(Line line)
        {
            Double faktorHeight = customPictureHeight / view.ActualHeight;
            Double faktorWidth = customPictureWidth / view.ActualWidth;
            line.X1 = line.X1 * faktorWidth;
            line.X2 = line.X2 * faktorWidth;
            line.Y1 = line.Y1 * faktorHeight;
            line.Y2 = line.Y2 * faktorHeight;
            return line;
        }
        
    }
}
