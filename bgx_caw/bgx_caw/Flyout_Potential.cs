using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bgx_caw
{ /// <summary>
    /// Logik des Potential Flyouts
    /// </summary>
    public partial class MainWindow
    {  /// <summary>
        /// Click Event des Potential Tile aus dem Info Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Potential(object sender, RoutedEventArgs e)
        {
            closeAllRightFlyouts();
            buildPotentialFlyout();
        }
        /// <summary>
        /// Click Event eines Tile aus dem Potential Flyouts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Potential_Click(object sender, EventArgs e)
        {
            MyTile tile = sender as MyTile;
            List<Page> list = new List<Page>();
            list = data.getPagenumbersFromPotentialNames(tile.Title);
            buildPagenumberFlyout(list);
        }
        /// <summary>
        /// Erzeugt das Potential Fylout anhand der aktuellen Seitenzahl
        /// </summary>
        private void buildPotentialFlyout()
        {
            stack_left_potential.Children.Clear();
            if (actualPageNumber > 0)
            {
                if (!flo_left_potential.IsOpen)
                {
                    closeAllLeftFlyouts();
                    flo_left_potential.IsOpen = true;
                    renderContainer.Margin = new Thickness(205, 45, 205, 0);
                }

                List<Potential> list = new List<Potential>();
                list = data.getPotentialFromPageNumber(actualPageNumber);
                foreach (var item in list)
                {
                    MyTile tile = new MyTile();
                    tile.Title = item.Name;
                    tile.Data = item;
                    tile.TiltFactor = 2;
                    tile.Width = 150;
                    tile.Height = 50;
                    tile.Click += new RoutedEventHandler(Tile_Potential_Click);
                    stack_left_potential.Children.Add(tile);
                }
            }
        }
    }
}

