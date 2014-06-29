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
        private void Tile_Part(Object sender, EventArgs e)
        {
            buildPartFlyout();
        }

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
        private void Tile_Part_Click(object sender, EventArgs e)
        {
            stack_right_sites.Children.Clear();
            MyTile tile = sender as MyTile;
            Part p = (Part)tile.Data;
            List<Page> list = new List<Page>();
            list = data.getPagenumbersFromPartlNames(tile.Title);

            if (!flo_right_sites.IsOpen)
            {
                closeAllRightFlyouts();
                flo_right_sites.IsOpen = true;
            }

            foreach (var item in list)
            {
                int page = (item.PageInDiagramm + 1);
                MyTile t1 = new MyTile();
                t1.Title = item.Title;
                t1.Content = page.ToString();
                t1.Data = item;
                t1.Width = 150;
                t1.Height = 80;
                t1.Click += new RoutedEventHandler(Tile_Site_Click);
                stack_right_sites.Children.Add(t1);
            }
        }
    }
}
