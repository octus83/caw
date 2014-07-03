using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

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

            

            /*var result = await this.ShowInputAsync("Hello!", "What is your name?");

            if (result == null) //user pressed cancel
                return;

            await this.ShowMessageAsync("Hello", "Hello " + result + "!");*/
        }

        private async void flo_Menu_tle_Delete_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "JA",
                NegativeButtonText = "NEIN",
                FirstAuxiliaryButtonText = "ABBRECHEN",
                ColorScheme = MetroDialogColorScheme.Accented
            };

            MessageDialogResult result = await this.ShowMessageAsync("Achtung", "Soll der Schaltplan " + Environment.NewLine + RecentDiagramm.SerialNumber + 
                                        Environment.NewLine + RecentDiagramm.FieldName + Environment.NewLine + Environment.NewLine + "gelöscht werden? Es gehen alle Daten verloren",
                                        MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Negative)
            {
                return;
            } 

            if (result == MessageDialogResult.Affirmative)
            {
                this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
                var progressDialog = await this.ShowProgressAsync("Schaltplan wird gelöscht", "Ordner " + RecentDiagramm.ID + " wird gelöscht");

                await Task.Delay(800);

                flo_Menu.IsOpen = false;

                using (DB_CAW db_caw = new DB_CAW())
                {
                    db_caw.deleteDiagramm(RecentDiagramm.ID);
                }

                //Directory.Delete(System.IO.Path.Combine(ProgrammPath, RecentDiagramm.ID), true);


                await progressDialog.CloseAsync();

                

                dgd_diagrammsList.UnselectAll();

                propertyChanged("DiagrammsList");

                flo_Menu.IsOpen = false;
            }

                       
        }
    }
}
