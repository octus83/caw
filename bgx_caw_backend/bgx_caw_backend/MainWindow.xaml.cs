using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bgx_caw_backend
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //lstView.ItemsSource = CreatePersonList();
        }

            /*private List<Diagramm> CreatePersonList() 
            {
                List<Diagramm> liste = new List<Diagramm>();
                liste.Add(new Diagramm { ID = 1, SerialNumber = "S0001-14", FieldName = "+GH01", ProjectName = "HTI - Austausch", ProjectNumber = "P9876", ProductionPlace = "1", DeviceID = "ah45Gi",  isActive = true});
                liste.Add(new Diagramm { ID = 2, SerialNumber = "S0002-14", FieldName = "Pumpe 1", ProjectName = "Verdichterhaus", ProjectNumber = "P1234", ProductionPlace = "4", DeviceID = "ad4dsi", isActive = true });
                liste.Add(new Diagramm { ID = 3, SerialNumber = "S0012-14", FieldName = "Lastteil 400V", ProjectName = "Schule am Hindenburgdamm", ProjectNumber = "P4433", ProductionPlace = "7", DeviceID = "bw657i", isActive = false });
                liste.Add(new Diagramm { ID = 4, SerialNumber = "S0023-14", FieldName = "Einspeisung", ProjectName = "Neubau Hansdorf", ProjectNumber = "P1191", ProductionPlace = "9", DeviceID = "1h35fi", isActive = false });
                liste.Add(new Diagramm { ID = 5, SerialNumber = "S0045-14", FieldName = "+UV110", ProjectName = "Intern GLX", ProjectNumber = "P7654", ProductionPlace = "5", DeviceID = "nn4524", isActive = false });
                liste.Add(new Diagramm { ID = 6, SerialNumber = "S0056-14", FieldName = "+HGCC23-1", ProjectName = "Kaverne Ottersheim", ProjectNumber = "P1100", ProductionPlace = "2", DeviceID = "e3aa5Gi", isActive = false });
                liste.Add(new Diagramm { ID = 1, SerialNumber = "S0001-14", FieldName = "+GH01", ProjectName = "HTI - Austausch", ProjectNumber = "P9876", ProductionPlace = "1", DeviceID = "ah45Gi", isActive = true });
                liste.Add(new Diagramm { ID = 2, SerialNumber = "S0002-14", FieldName = "Pumpe 1", ProjectName = "Verdichterhaus", ProjectNumber = "P1234", ProductionPlace = "4", DeviceID = "ad4dsi", isActive = true });
                liste.Add(new Diagramm { ID = 3, SerialNumber = "S0012-14", FieldName = "Lastteil 400V", ProjectName = "Schule am Hindenburgdamm", ProjectNumber = "P4433", ProductionPlace = "7", DeviceID = "bw657i", isActive = false });
                liste.Add(new Diagramm { ID = 4, SerialNumber = "S0023-14", FieldName = "Einspeisung", ProjectName = "Neubau Hansdorf", ProjectNumber = "P1191", ProductionPlace = "9", DeviceID = "1h35fi", isActive = false });
                liste.Add(new Diagramm { ID = 5, SerialNumber = "S0045-14", FieldName = "+UV110", ProjectName = "Intern GLX", ProjectNumber = "P7654", ProductionPlace = "5", DeviceID = "nn4524", isActive = false });
                liste.Add(new Diagramm { ID = 6, SerialNumber = "S0056-14", FieldName = "+HGCC23-1", ProjectName = "Kaverne Ottersheim", ProjectNumber = "P1100", ProductionPlace = "2", DeviceID = "e3aa5Gi", isActive = false });
                return liste;
            }*/

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                var win2 = new ImportWindow();
                win2.Show();
            }
    }
}
