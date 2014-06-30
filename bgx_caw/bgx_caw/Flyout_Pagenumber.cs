using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bgx_caw
{
    /// <summary>
    /// Teilklasse die die Logik des Seiten Flyouts behinhaltet
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Click Event für ein Seiten Tile aus dem Seiten Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Site_Click(object sender, EventArgs e)
        {
            MyTile tile = sender as MyTile;
            Page p = (Page)tile.Data;
            goToPage(p.PageInDiagramm+1);
        }
        /// <summary>
        /// Erzeugt das Seiten Flyout für die Navigation
        /// </summary>
        /// <param name="list">Liste mit page objekten</param>
        private void buildPagenumberFlyout(List<Page> list)
        {
            stack_right_sites.Children.Clear();
            if (!flo_right_sites.IsOpen)
            {
                closeAllRightFlyouts();
                flo_right_sites.IsOpen = true;
            }
            foreach (var item in list)
            {
                int page = (item.PageInDiagramm + 1);
                MyTile t1 = new MyTile();
                t1.Title = page.ToString() + " | " + item.Title;
                t1.Data = item;
                t1.Width = 150;
                t1.Height = 80;
                t1.Click += new RoutedEventHandler(Tile_Site_Click);
                stack_right_sites.Children.Add(t1);
            }
        }
    }
}
