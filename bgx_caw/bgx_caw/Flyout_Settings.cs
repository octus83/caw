using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace bgx_caw
{/// <summary>
    /// Logik des Einstellung Flyouts
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// 2 Weg Databinding eventhandler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Öffnet das Einstellungs Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Comm_btn_Settings_Click(object sender, EventArgs e)
        {
            flo_Settings.IsOpen = true;
        }
        /// <summary>
        /// Speichert die Eingaben der Einstellungen in der Config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flo_Settings_tle_sve_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(flo_Settings_tbx_dsc.Text) || String.IsNullOrWhiteSpace(flo_Settings_tbx_inc.Text))
            {

                flo_Settings_tbx_dsc.BorderBrush = String.IsNullOrWhiteSpace(flo_Settings_tbx_dsc.Text) == false ? Brushes.Black : Brushes.Red;
                flo_Settings_tbx_inc.BorderBrush = String.IsNullOrWhiteSpace(flo_Settings_tbx_inc.Text) == false ? Brushes.Black : Brushes.Red;

                this.DataSource = flo_Settings_tbx_dsc.Text;
                this.InitialCatalog = flo_Settings_tbx_inc.Text;

            
            }
            else
            {
                propertyChanged("DiagrammsList");

                flo_Settings.IsOpen = false;
            }
            Console.WriteLine("Ausgabe-> Programmpfad: " + ProgrammPath);
            Console.WriteLine("Ausgabe-> Datenbank: " + InitialCatalog);
            Console.WriteLine("Ausgabe-> Datasource: " + DataSource);
        }

        private void flo_Settings_tle_ccl_Click(object sender, RoutedEventArgs e)
        {
            flo_Settings_tbx_dsc.Text = this.DataSource;
            flo_Settings_tbx_inc.Text = this.InitialCatalog;
        }
    }
}