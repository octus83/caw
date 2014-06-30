using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bgx_caw
{   /// <summary>
    /// Logik des Part Flyouts
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Click Event des Part Tile aus dem Info Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Part(Object sender, EventArgs e)
        {
            buildPartFlyout();
        }
        /// <summary>
        /// Click Event eines Tile aus dem Part Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Part_Click(object sender, EventArgs e)
        {
            stack_right_sites.Children.Clear();
            MyTile tile = sender as MyTile;         
            List<Page> list = new List<Page>();
            list = data.getPagenumbersFromPartlNames(tile.Title);
            buildPagenumberFlyout(list);        
        }
        /// <summary>
        /// Erzeugt das PartFylout anhand der aktuellen Seitenzahl
        /// 
        /// </summary>
        private void buildPartFlyout()
        {
            stack_left_parts.Children.Clear();
            if (actualPageNumber > 0)
            {
                if (!flo_left_parts.IsOpen)
                {
                    closeAllLeftFlyouts();
                    flo_left_parts.IsOpen = true;
                }
                List<Part> list = new List<Part>();
                list = data.getPartFomPageNumber(actualPageNumber);

                foreach (var item in list)
                {
                    MyTile tile = new MyTile();
                    tile.Title = item.BMK;
                    tile.Data = item;
                    tile.TiltFactor = 2;
                    tile.Width = 150;
                    tile.Height = 50;
                    tile.Click += new RoutedEventHandler(Tile_Part_Click);
                    stack_left_parts.Children.Add(tile);
                }
            }
        }
        
    }
}
