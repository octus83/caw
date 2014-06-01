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
        private void flo_Settings_tle_sve_Click(object sender, RoutedEventArgs e)
        {
            this.DataSource = flo_Settings_tbx_dsc.Text;
            this.InitialCatalog = flo_Settings_tbx_inc.Text;

            propertyChanged("DiagrammsList");

            flo_Settings.IsOpen = false;
        }

        private void flo_Settings_tle_ccl_Click(object sender, RoutedEventArgs e)
        {
            flo_Settings_tbx_dsc.Text = this.DataSource;
            flo_Settings_tbx_inc.Text = this.InitialCatalog;
        }
    }
}
