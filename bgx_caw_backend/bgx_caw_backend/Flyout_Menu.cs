using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bgx_caw_backend
{
    public partial class BackEnd
    {
        private void flo_Menu_tle_Details_Click(object sender, RoutedEventArgs e)
        {
            flo_details.IsOpen = true;
        }

        private void flo_Menu_tle_Print_Click(object sender, RoutedEventArgs e)
        {
            flo_Print.IsOpen = true;
        }

        private void flo_Menu_tle_Export_Click(object sender, RoutedEventArgs e)
        {
            flo_Export.IsOpen = true;
        }

        private void flo_Menu_tle_Delete_Click(object sender, RoutedEventArgs e)
        {
            using (DB_CAW db_caw = new DB_CAW())
            {
                db_caw.deleteDiagramm(((Diagramm)dgd_diagrammsList.SelectedItem).ID);
            }
            dgd_diagrammsList.UnselectAll();

            propertyChanged("DiagrammsList");

            flo_Menu.IsOpen = false;
        }
    }
}
