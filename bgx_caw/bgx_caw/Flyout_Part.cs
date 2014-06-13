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
                closeAllLeftFlyouts();
                flo_left_parts.IsOpen = true;
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
            MessageBox.Show("Liste mit seiten");
        }
    }
}
