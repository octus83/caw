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
        Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //Überprüfen ob Key existiert

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
        public PdfReader PDFReader
        {
            get;
            set;
        }

        public String DataSource
        {
            get
            {
                return config.AppSettings.Settings["DataSource"].Value;
            }
            set
            {
                if (config.AppSettings.Settings["DataSource"] != null)
                {
                    config.AppSettings.Settings.Remove("DataSource");
                }
                config.AppSettings.Settings.Add("DataSource", value);
                
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public String InitialCatalog
        {
            get
            {
                return config.AppSettings.Settings["InitialCatalog"].Value;
            }
            set
            {
                if (config.AppSettings.Settings["InitialCatalog"] != null)
                {
                    config.AppSettings.Settings.Remove("InitialCatalog");
                }
                config.AppSettings.Settings.Add("InitialCatalog", value);

                config.Save(ConfigurationSaveMode.Modified);
            }
        }


        public BackEnd()
        {           
            InitializeComponent();
            DataSource = "N005509\\trans_edb_p8";
            InitialCatalog = "CAWFinal";
            refreshDiagrammList();
            dgd_diagrammsList.DataContext = DiagrammsList;            
            this.DataContext = this; 
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void refreshDiagrammList()
        {
            using (DB_CAW db_caw = new DB_CAW())
            {
                DiagrammsList = db_caw.getDiagramms();                    
            } 
        }

        private void win_Comm_btn_Import_Click(object sender, RoutedEventArgs e)
        {
            flo_Menu.IsOpen = false;
            flo_details.IsOpen = false;
            flo_Settings.IsOpen = false;
            flo_bottom.IsOpen ^= true;
        }

        private void win_Comm_btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            flo_bottom.IsOpen = false;
            flo_Settings.IsOpen ^= true;
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
            flo_Export.IsOpen = true;
        }

        private void flo_Menu_tle_Delete_Click(object sender, RoutedEventArgs e)
        {
            using(DB_CAW db_caw = new DB_CAW())
            {
                db_caw.deleteDiagramm(((Diagramm)dgd_diagrammsList.SelectedItem).ID);
            }

            refreshDiagrammList();
        }

        private void dgd_diagrammsList_Changed(object sender, SelectionChangedEventArgs e)
        {
            using (DB_CAW db_caw = new DB_CAW())
            {
                //List<Diagramm> recentList = new List<Diagramm>();
                RecentDiagramm = db_caw.getDiagramm(((Diagramm)dgd_diagrammsList.SelectedItem).ID);
                //recentList.Add(RecentDiagramm);

            }
            flo_Menu.IsOpen = true;
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

            if (PDFDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OnPropertyChanged("PDFDialog");
            }
        }

        private async void flo_import_tle_import_Click(object sender, RoutedEventArgs e)
        {
            String projectFolder = @"e:\programme\caw\projects\";
            String diagrammPath = System.IO.Path.Combine(projectFolder, ImportDiagramm.ID);
            String pagePath;

            try
            {
                pdfReader = new PdfReader(PDFDialog.FileName);

                if (ImportDiagramm.pages_List.Count == pdfReader.NumberOfPages)
                {
                    //try
                    //{
                    this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
                    var progressDialog = await this.ShowProgressAsync("Schaltplan wird importiert", "Diagram Ordner " + ImportDiagramm.ID + " wird erstellt");

                    progressDialog.SetProgress(0.1);

                    //Create DiagrammFolder (ID)                    
                    System.IO.Directory.CreateDirectory(diagrammPath);

                    await Task.Delay(200);

                    int counter = 0;

                    //Create PageFolder (P_id) containing Overlays                
                    foreach (Page page in ImportDiagramm.pages_List)
                    {
                        progressDialog.SetMessage("Page Ordner " + page.P_id + " wird erstellt");
                        pagePath = System.IO.Path.Combine(diagrammPath, page.P_id);
                        System.IO.Directory.CreateDirectory(pagePath);

                        await Task.Delay(100);

                        counter++;
                        progressDialog.SetProgress((0.7 / ImportDiagramm.pages_List.Count) * counter);
                    }

                    //Kopiere pdf in project - Ordner
                    progressDialog.SetMessage("PDF wird kopiert");
                    System.IO.File.Copy(PDFDialog.FileName, System.IO.Path.Combine(diagrammPath, ImportDiagramm.ID + ".pdf"));

                    await Task.Delay(300);
                    progressDialog.SetProgress(1.0);

                    await progressDialog.CloseAsync();

                    flo_bottom.IsOpen = false;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Das PDF hat " + pdfReader.NumberOfPages + " Seiten und das DXF " + ImportDiagramm.pages_List.Count);
                }
            }
            catch
            {

            }
            finally
            {
                ImportDiagramm = null;
                dxf_parser = null;
                DXFDialog = null;
                PDFDialog = null;
            }
            
        }

        private void flo_import_tle_del_Click(object sender, RoutedEventArgs e)
        {
            DXFDialog = null;
            PDFDialog = null;
            ImportDiagramm = null;
        }

        #region ScaleValue Depdency Property
        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(BackEnd), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

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

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            BackEnd mainWindow = o as BackEnd;
            if (mainWindow != null)
            {
                return mainWindow.OnCoerceScaleValue((double)value);
            }
            else
            {
                return value;
            }                     
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            BackEnd mainWindow = o as BackEnd;
            if (mainWindow != null)
            {
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
            }               
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
            {
                return 1.0d;
            }
                
            return Math.Max(0.1, value);
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {

        }
       
        #endregion

        private void StackPanel_SizeChanged(object sender, EventArgs e)
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

        private async void showMessage()
        {
            await this.ShowMessageAsync("Fehler", "");
        }

        private void dgd_diagrammsList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (DB_CAW db_caw = new DB_CAW())
            {
                //List<Diagramm> recentList = new List<Diagramm>();
                RecentDiagramm = db_caw.getDiagramm(((Diagramm)dgd_diagrammsList.SelectedItem).ID);
                //recentList.Add(RecentDiagramm);

            }
            flo_Menu.IsOpen = true;
        }

        private void flo_Settings_tle_sve_Click(object sender, RoutedEventArgs e)
        {
            this.DataSource = flo_Settings_tbx_dsc.Text;
            this.InitialCatalog = flo_Settings_tbx_inc.Text;

            refreshDiagrammList();

            flo_Settings.IsOpen = false;
        }

        private void flo_Settings_tle_ccl_Click(object sender, RoutedEventArgs e)
        {
            flo_Settings_tbx_dsc.Text = this.DataSource;
            flo_Settings_tbx_inc.Text = this.InitialCatalog;
        }

        private void flo_Print_tle_Print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void flo_Export_tle_Export_Click(object sender, RoutedEventArgs e)
        {

        }      
    }
}
