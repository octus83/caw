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
using System.Windows.Shapes;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using Microsoft.Win32;

namespace bgx_caw_backend
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        public ImportWindow()
        {
            InitializeComponent();
        }

        private void btn_chooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog().Value)
            {

            }
        }

        private void btn_import_Click(object sender, RoutedEventArgs e)
        {
            using (DB_CAW db_caw = new DB_CAW())
            {
                db_caw.addDiagramm(new Diagramm
                                                {
                                                    Author = tbox_author.Text,
                                                    Date_init = DateTime.Now,
                                                    Date_lastchange = DateTime.Now,
                                                    Fieldname = tbox_fieldName.Text,
                                                    Projectname = tbox_projectName.Text,
                                                    Projectnumber = tbox_projectNumber.Text,
                                                    Productionplace = tbox_productionPlace.Text,
                                                    Serialnumber = tbox_serialNumber.Text,
                                                    Worker = tbox_worker.Text
                                                });

            }
        }



    }
}
