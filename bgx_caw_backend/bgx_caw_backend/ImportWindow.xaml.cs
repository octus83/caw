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
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.IO;

namespace bgx_caw_backend
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        private DXF_Parser dxf_parser;
        private DirectoryInfo sourceFolder;

        public ImportWindow()
        {
            InitializeComponent();
        }

        private void btn_chooseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if(folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sourceFolder = new DirectoryInfo(folderDialog.SelectedPath);
                tbox_sourceFolder.Text = folderDialog.SelectedPath;
            }
        }

        private void btn_import_Click(object sender, RoutedEventArgs e)
        {
            dxf_parser = new DXF_Parser(sourceFolder);

            using (DB_CAW db_caw = new DB_CAW())
            {

                db_caw.addDiagramm(dxf_parser.Diagramm);
            }

        }
    }
}
