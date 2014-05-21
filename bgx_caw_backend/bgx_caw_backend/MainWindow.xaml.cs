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
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Data;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;


namespace bgx_caw_backend
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private List<Diagramm> diagrammsList;
        private DataSet diagrammsSet;

        private DXF_Parser dxf_parser;
        private DirectoryInfo sourceFolder;

        public MainWindow()
        {
            InitializeComponent();
            refreshDiagrammList();

            flo_right.IsOpen = false;       
        }

        private void refreshDiagrammList()
        {
            try
            {
                using (DB_CAW db_caw = new DB_CAW())
                {
                    diagrammsList = db_caw.getDiagramms();
                    //diagrammsSet = db_caw.getDiagrammsSet();

                    dgd_diagrammsList.ItemsSource = diagrammsList;
                    
                }
            }
            catch (Exception exc)
            {
                this.Close();
                System.Windows.MessageBox.Show(exc.Message + exc.StackTrace);
            } 
        }

        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            flo_left.IsOpen = false;
            flo_right.IsOpen = false;
            flo_bottom.IsOpen = true;
            /*this.ShowOverlay();
            ImportWindow importWindow = new ImportWindow();
            importWindow.ShowDialog();

            if (importWindow.DialogResult == true)
            {
                refreshDiagrammList();
                this.HideOverlay();
            }
            else
            {
                this.HideOverlay();
            }*/
        }

        private void btn_Details_Click(object sender, RoutedEventArgs e)
        {
            //new DetailsWindow(diagrammsList[lstView.SelectedIndex].ID).ShowDialog();
            //this.ShowProgressAsync("Please wait...", "Progress message");

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Hi",
                NegativeButtonText = "Go away!",
                FirstAuxiliaryButtonText = "Cancel",
               ColorScheme = MetroDialogColorScheme.Accented
            };

            //this.ShowMessageAsync("Hello!", "Welcome to the world of metro! ",
              //  MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

            flo_right.IsOpen = true;
            
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Cancel",
                ColorScheme = MetroDialogColorScheme.Accented
            };
            this.ShowMessageAsync("Bitte warten...", "Schaltplan wird importiert ",
            MessageDialogStyle.Affirmative, mySettings);


        }

        private void dgd_diagrammsList_Changed(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(dgd_diagrammsList.SelectedIndex.ToString());
            
            flo_left.IsOpen = true;
            //flo_left.Header = 
            //flo_right.IsOpen = true;
        }

        private void btn_chooseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                //pgb_load.IsIndeterminate = false;
                flo_bottom.IsOpen = false;
                this.ShowProgressAsync("Please wait...", "Progress message");

                


                //this.DialogResult = true;
            }

        }

        
    }
}
