using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw
{
    public partial class MainWindow
    {
        private void buildSiteFylout()
        {
            stack_right_sites.Children.Clear();
        }

        private void Tile_Site_Click(object sender, EventArgs e)
        {
            MyTile tile = sender as MyTile;
            Page p = (Page)tile.Data;
            goToPage(p.PageInDiagramm+1);
        }
    }
}
