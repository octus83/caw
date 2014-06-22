using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bgx_caw
{
    public partial class MainWindow
    {
        private void Tile_Potential(object sender, RoutedEventArgs e)
        {
            buildPotentialFlyout();
        }
        private void Tile_Potential_Click(object sender, EventArgs e)
        {
            stack_right_sites.Children.Clear();
            MyTile tile = sender as MyTile;
            Potential p = (Potential)tile.Data;
            List<Page> list = new List<Page>();
            list = data.getPagenumbersFromPotentialNames(tile.Title);

            closeAllRightFlyouts();

            flo_right_sites.IsOpen = true;
            foreach (var item in list)
            {
                int page = (item.PageInDiagramm +1);
                MyTile t1 = new MyTile();
                t1.TitleFontSize = 15;
                t1.Title = page.ToString() + " " + item.Title;
                t1.Data = item;
                t1.TiltFactor = 2;
                t1.Width = 150;
                t1.Height = 50;
                t1.Click += new RoutedEventHandler(Tile_Site_Click);
                stack_right_sites.Children.Add(t1);
            }
        }
        private void buildPotentialFlyout()
        {
            stack_left_potential.Children.Clear();
            if (actualPageNumber > 0)
            {
                closeAllLeftFlyouts();
                flo_left_potential.IsOpen = true;
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

