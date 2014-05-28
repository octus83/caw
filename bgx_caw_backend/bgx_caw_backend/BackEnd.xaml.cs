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
using System.Configuration;
using iTextSharp.text.pdf;
using System.Threading;


namespace bgx_caw_backend
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class BackEnd : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DXF_Parser dxf_parser;
        private DirectoryInfo sourceFolder;
        
        private List<Diagramm> _diagrammsList;
        private List<Diagramm> DiagrammsList
        {
            get
            {
                return _diagrammsList;
            }
            set
            {
                _diagrammsList = value;
                OnPropertyChanged("diagrammsList");
            }
        }

        private Diagramm _recentDiagramm;       
        public Diagramm RecentDiagramm
        {
            get
            {
                return _recentDiagramm;
            }
            set
            {
                _recentDiagramm = value;
                OnPropertyChanged("RecentDiagramm");
            }
        }

        private Diagramm _importDiagramm;
        public Diagramm ImportDiagramm
        {
            get
            {
                return _importDiagramm;
            }
            set
            {
                _importDiagramm = value;
                OnPropertyChanged("ImportDiagramm");
            }
        }

        private FolderBrowserDialog _dxfdialog;
        public FolderBrowserDialog DXFDialog
        {
            get
            {
                return _dxfdialog;
            }
            set
            {
                _dxfdialog = value;
                OnPropertyChanged("DXFDialog");
            }
        }

        private OpenFileDialog _pdfdialog;
        public OpenFileDialog PDFDialog
        {
            get
            {
                return _pdfdialog;
            }
            set
            {
                _pdfdialog = value;
                OnPropertyChanged("PDFDialog"); 
            }
        }

        private PdfReader pdfReader;


        public BackEnd()
        {  
            //DELETE BEFORE RELEASE

            //Laden der AppSettings
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //Überprüfen ob Key existiert
            
            if (config.AppSettings.Settings["DataSource"] != null)
            {
                //Key existiert. Löschen des Keys zum "überschreiben"
                config.AppSettings.Settings.Remove("DataSource");
            }
            if (config.AppSettings.Settings["InitialCatalog"] != null)
            {
                //Key existiert. Löschen des Keys zum "überschreiben"
                config.AppSettings.Settings.Remove("InitialCatalog");
            }
            //Anlegen eines neuen KeyValue-Paars
            config.AppSettings.Settings.Add("DataSource", "N005509\\trans_edb_p8");
            config.AppSettings.Settings.Add("InitialCatalog", "CAWFinal");
            //Speichern der aktualisierten AppSettings
            config.Save(ConfigurationSaveMode.Modified);
            ///////////////////////////////



            InitializeComponent();
            refreshDiagrammList();
            dgd_diagrammsList.DataContext = DiagrammsList;            
            this.DataContext = this; 
        }

        private void refreshDiagrammList()
        {
            using (DB_CAW db_caw = new DB_CAW())
            {
                DiagrammsList = db_caw.getDiagramms();                    
            } 
        }

        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            flo_Menu.IsOpen = false;
            flo_details.IsOpen = false;
            flo_bottom.IsOpen = true;
        }

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
            //Flyout open export
        }

        private void flo_Menu_tle_Delete_Click(object sender, RoutedEventArgs e)
        {
            this.ShowProgressAsync("Please wait...", "Progress message");
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgd_diagrammsList_Changed(object sender, SelectionChangedEventArgs e)
        {
            using (DB_CAW db_caw = new DB_CAW())
            {
                List<Diagramm> recentList = new List<Diagramm>();
                
                RecentDiagramm = db_caw.getDiagramm(((Diagramm)dgd_diagrammsList.SelectedItem).ID);
                recentList.Add(RecentDiagramm);
                
            }
            flo_Menu.IsOpen = true;
        }

        private void btn_chooseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sourceFolder = new DirectoryInfo(folderDialog.SelectedPath);
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
            }
        }

        private void ttl_mnu_Import_Click(object sender, RoutedEventArgs e)
        {
            flo_bottom.IsOpen = true;
        }

        #region ScaleValue Depdency Property
        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(BackEnd), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            BackEnd mainWindow = o as BackEnd;
            if (mainWindow != null)
                return mainWindow.OnCoerceScaleValue((double)value);
            else
                return value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            BackEnd mainWindow = o as BackEnd;
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
            double xScale = ActualWidth / 1100.0d;
            double value = Math.Min(xScale, yScale);
            ScaleValue = (double)OnCoerceScaleValue(stp_Main, value);
        }

        

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }            
        }

        private void flo_import_btn_dxf_Click(object sender, RoutedEventArgs e)
        {
            DXFDialog = new FolderBrowserDialog();



            if (DXFDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OnPropertyChanged("DXFDialog");
                dxf_parser = new DXF_Parser(new DirectoryInfo(DXFDialog.SelectedPath));
                ImportDiagramm = dxf_parser.Diagramm;
            }
        }

        private void flo_import_btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            PDFDialog = new OpenFileDialog();

            if(PDFDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OnPropertyChanged("PDFDialog");
            }
        }

        private async void flo_import_tle_import_Click(object sender, RoutedEventArgs e)
        {
            pdfReader = new PdfReader(PDFDialog.FileName);

            flo_bottom.IsOpen = false;            

            if (ImportDiagramm.pages_List.Count == pdfReader.NumberOfPages)
            {
                this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;

                var controller = await this.ShowProgressAsync("Schaltplan wird importiert", "Diagram Ordner " + ImportDiagramm.ID  + " wird erstellt");           
                controller.SetProgress(0.1);

                //Create DiagrammFolder (ID)
                String folderName = @"e:\programme\caw\projects\";
                string pathString = System.IO.Path.Combine(folderName, ImportDiagramm.ID);
                System.IO.Directory.CreateDirectory(pathString);
                await Task.Delay(200);
                int counter = 0;
                
                //Create PageFolder (P_id) containing Overlays                
                foreach(Page page in ImportDiagramm.pages_List)
                {
                    controller.SetMessage("Page Ordner " + page.P_id + " wird erstellt");
                    string pagePathString = System.IO.Path.Combine(pathString, page.P_id);
                    System.IO.Directory.CreateDirectory(pagePathString);
                    await Task.Delay(200);                    
                    counter++;
                    controller.SetProgress((0.7/ImportDiagramm.pages_List.Count) * counter);
                }


                //Kopiere pdf in project - Ordner

                await controller.CloseAsync();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Das PDF hat " + pdfReader.NumberOfPages + " Seiten und das DXF " + ImportDiagramm.pages_List.Count);
            }
        }

        private void flo_import_tle_del_Click(object sender, RoutedEventArgs e)
        {
            DXFDialog = null;
            PDFDialog = null;
            ImportDiagramm = null;
        }
    }
}
