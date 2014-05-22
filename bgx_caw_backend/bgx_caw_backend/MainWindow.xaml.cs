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
    public partial class MainWindow
    {
        private List<Diagramm> diagrammsList;
        private DataSet diagrammsSet;

        private DXF_Parser dxf_parser;
        private DirectoryInfo sourceFolder;

        public MainWindow()
        {
            InitializeComponent();
            refreshDiagrammList();      
        }

        private void refreshDiagrammList()
        {
            try
            {
                using (DB_CAW db_caw = new DB_CAW())
                {
                    diagrammsList = db_caw.getDiagramms();
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

            //flo_right.IsOpen = true;
            
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Cancel",
                ColorScheme = MetroDialogColorScheme.Accented
            };
            //this.ShowMessageAsync("Bitte warten...", "Schaltplan wird importiert ",
            //MessageDialogStyle.Affirmative, mySettings);


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
                //tbox_sourceFolder.Text = folderDialog.SelectedPath;
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

        private void flo_bottom_tle_ImportFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sourceFolder = new DirectoryInfo(folderDialog.SelectedPath);
                //tbox_sourceFolder.Text = folderDialog.SelectedPath;
            }
        }

        private void ttl_mnu_Import_Click(object sender, RoutedEventArgs e)
        {
            flo_bottom.IsOpen = true;
        }

        private void FlipView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        #region ScaleValue Depdency Property
        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                return mainWindow.OnCoerceScaleValue((double)value);
            else
                return value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
                return 1.0d;

            value = Math.Max(0.1, value);
            return value;
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {

        }

        public double ScaleValue
        {
            get
            {
                return (double)GetValue(ScaleValueProperty);
            }
            set
            {
                SetValue(ScaleValueProperty, value);
            }
        }
        #endregion

        private void StackPanel_SizeChanged_1(object sender, EventArgs e)
        {
            CalculateScale();
        }

        private void CalculateScale()
        {
            double yScale = ActualHeight / 750.0d;
            double xScale = ActualWidth / 1200.0d;
            double value = Math.Min(xScale, yScale);
            ScaleValue = (double)OnCoerceScaleValue(stp_Main, value);
        }
 
    }
}
