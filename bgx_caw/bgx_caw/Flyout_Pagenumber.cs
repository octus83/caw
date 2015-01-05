using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
            changeColorOfTileToOrginal(tile);
            Page p = (Page)tile.Data;         
            goToPage(p.PageInDiagramm+1);
            findTileToActualPagenumber();
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
                Console.WriteLine("Console -> t1 number: "+ page);
                t1.pagenumber  = page;
                if (page == this.actualPageNumber)
                {
                    changeColorOfTileToMarkActualPagenumber(t1);
                }
                stack_right_sites.Children.Add(t1);
            }
        }
        /// <summary>
        /// Sucht das Entsprechende Tile anhand der aktuellen Seitenzahl
        /// und makiert es in bilfinger gelb. Entfernt außerdem eine Makierung wieder
        /// zum Original Farbton
        /// </summary>
        private void findTileToActualPagenumber()
        {
            if (flo_right_sites.IsOpen)
            {
                foreach (var item in stack_right_sites.Children)
                {
                    try
                    {
                        MyTile t = (MyTile)item;
                        if (t.pagenumber == actualPageNumber)
                        {
                            changeColorOfTileToMarkActualPagenumber(t);
                        }
                        if (t.pagenumber == lastActualPageNumber)
                        {
                            changeColorOfTileToOrginal(t);
                        }

                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }
        /// <summary>
        /// Ändert die Farbe eines Tiles zur oiriginal Farbe
        /// </summary>
        /// <param name="m"></param>
        private void changeColorOfTileToOrginal(MyTile m)
        {
            var converter = new System.Windows.Media.BrushConverter();
            //original tile color
            var brush = (Brush)converter.ConvertFromString("#CC119EDA");
            m.Background = brush;
        }
        /// <summary>
        /// Ändert die Farbe eines Tile zur Bilfinger Gelb Farbe
        /// </summary>
        /// <param name="m"></param>
        private void changeColorOfTileToMarkActualPagenumber(MyTile m)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#CCFFE400");
            m.Background = brush;
        }
    }
}
