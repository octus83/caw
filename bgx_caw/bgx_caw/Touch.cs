using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace bgx_caw
{
    public partial class MainWindow
    {
        Point touchPointStart;
        Point touchPointEnd;


        private void renderContainer_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            touchPointStart = e.GetPosition(view);
            Console.WriteLine("Ausgabe -> Touchpointstart : (" + touchPointStart.X + ", " + touchPointStart.Y + ")");
        }

        private void renderContainer_MouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            touchPointEnd = e.GetPosition(view);
            Console.WriteLine("Ausgabe -> Touchpointstart : (" + touchPointEnd.X + ", " + touchPointEnd.Y + ")");
            checkTouchGesture();
        }

        private void checkTouchGesture()
        {
            if (drawState == DrawState.None)
            {
                if (touchPointEnd.X > (touchPointStart.X + 50)
                    && touchPointEnd.Y < (touchPointStart.Y + 50)
                    && touchPointEnd.Y > (touchPointStart.Y - 50))
                {
                    previousPage();
                    Console.WriteLine("Ausgabe -> Nextpage Osten");
                }
                else if (touchPointEnd.X < (touchPointStart.X - 50)
                    && touchPointEnd.Y < (touchPointStart.Y + 50)
                    && touchPointEnd.Y > (touchPointStart.Y - 50))
                {
                    nextPage();

                    Console.WriteLine("Ausgabe -> previousPage Westen");
                }
                else if (touchPointEnd.Y > (touchPointStart.Y + 50)
                    && touchPointEnd.X < (touchPointStart.X + 50)
                    && touchPointEnd.X > (touchPointStart.X - 50))
                {
                    previousPage();
                    Console.WriteLine("Ausgabe -> Nextpage Süden");
                }
                else if (touchPointEnd.Y < (touchPointStart.Y - 50)
                    && touchPointEnd.X < (touchPointStart.X + 50)
                    && touchPointEnd.X > (touchPointStart.X - 50))
                {
                    nextPage();
                    Console.WriteLine("Ausgabe -> previousPage Norden");
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
