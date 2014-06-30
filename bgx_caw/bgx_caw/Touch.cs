using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace bgx_caw
{
    /// <summary>
    /// Teilklasse die die Logik der Touch events beinhaltet
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Startpunkt der berührung
        /// </summary>
        Point touchPointStart;
        /// <summary>
        /// Endpunkt der berührung
        /// </summary>
        Point touchPointEnd;

        /// <summary>
        /// Mausdown oder Touchdown  Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderContainer_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            touchPointStart = e.GetPosition(view);
            Console.WriteLine("Ausgabe -> Touchpointstart : (" + touchPointStart.X + ", " + touchPointStart.Y + ")");
        }
        /// <summary>
        /// Mouse up oder loslassen der berührung Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renderContainer_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            touchPointEnd = e.GetPosition(view);
            Console.WriteLine("Ausgabe -> Touchpointstart : (" + touchPointEnd.X + ", " + touchPointEnd.Y + ")");
            checkTouchGesture();
        }
        /// <summary>
        /// Überprüfung welche Touch geste erzeugt wurde
        /// </summary>
        private void checkTouchGesture()
        {
            if (drawState == DrawState.None)
            {
                if (touchPointEnd.X > (touchPointStart.X + 50)
                    && touchPointEnd.Y < (touchPointStart.Y + 50)
                    && touchPointEnd.Y > (touchPointStart.Y - 50))
                {
                    previousPage();
                }
                else if (touchPointEnd.X < (touchPointStart.X - 50)
                    && touchPointEnd.Y < (touchPointStart.Y + 50)
                    && touchPointEnd.Y > (touchPointStart.Y - 50))
                {
                    nextPage();
                }
                else if (touchPointEnd.Y > (touchPointStart.Y + 50)
                    && touchPointEnd.X < (touchPointStart.X + 50)
                    && touchPointEnd.X > (touchPointStart.X - 50))
                {
                    previousPage();
                }
                else if (touchPointEnd.Y < (touchPointStart.Y - 50)
                    && touchPointEnd.X < (touchPointStart.X + 50)
                    && touchPointEnd.X > (touchPointStart.X - 50))
                {
                    nextPage();
                }
                else
                {
                    closeAllFlyouts();
                }
            }
            else
            {
                closeAllFlyouts();
            }
        }
    }
}
