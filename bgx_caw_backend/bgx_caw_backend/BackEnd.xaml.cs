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
using GhostscriptSharp;
using GhostscriptSharp.Settings;


namespace bgx_caw_backend
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class BackEnd : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private DXF_Parser dxf_parser;

        public BindingList<Diagramm> DiagrammsList
        {
            get
            {
                if (DataSource != null && InitialCatalog != null)
                {
                    using (DB_CAW db_caw = new DB_CAW())
                    {
                        return new BindingList<Diagramm>(db_caw.getDiagramms());
                    }
                }
                else
                {
                    return null;
                }
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
                propertyChanged("RecentDiagramm");
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
                propertyChanged("ImportDiagramm");
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
                propertyChanged("DXFDialog");
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
                propertyChanged("PDFDialog"); 
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
                if (config.AppSettings.Settings["DataSource"] == null)
                {
                    //flo_Settings_tbx_dsc.BorderBrush = Brushes.Black;                   
                    return null;
                }
                else
                {
                    //flo_Settings_tbx_dsc.BorderBrush = Brushes.Red;
                    return config.AppSettings.Settings["DataSource"].Value;
                }
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    if (config.AppSettings.Settings["DataSource"] != null)
                    {
                        flo_Settings_tbx_dsc.BorderBrush = Brushes.Black;
                        config.AppSettings.Settings.Remove("DataSource");
                    }
                    config.AppSettings.Settings.Add("DataSource", value);

                    config.Save(ConfigurationSaveMode.Modified);
                }
                else
                {
                    flo_Settings_tbx_dsc.BorderBrush = Brushes.Red;
                }                
            }
        }

        public String InitialCatalog
        {
            get
            {
                if (config.AppSettings.Settings["InitialCatalog"] == null)
                {
                    return null;
                }
                else
                {
                    return config.AppSettings.Settings["InitialCatalog"].Value;
                }
                
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

        public String ProgrammPath
        {
            get
            {
                if (config.AppSettings.Settings["ProgrammPath"] == null)
                {
                    return null;
                }
                else
                {
                    return config.AppSettings.Settings["ProgrammPath"].Value;
                }
            }
            set
            {
                if (config.AppSettings.Settings["ProgrammPath"] != null)
                {
                    config.AppSettings.Settings.Remove("ProgrammPath");
                }
                config.AppSettings.Settings.Add("ProgrammPath", value);

                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public BackEnd()
        {
            InitializeComponent();
            this.DataContext = this; 

            if(DataSource == null || InitialCatalog == null || ProgrammPath == null)
            {
                flo_Settings.IsOpen = true;
            }            
        }

        protected void propertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
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
            flo_Menu.IsOpen = false;
            flo_details.IsOpen = false;
            flo_bottom.IsOpen = false;
            flo_Settings.IsOpen ^= true;
        }

        private void dgd_diagrammsList_Select(object sender, SelectionChangedEventArgs e)
        {
            if (dgd_diagrammsList.SelectedItem != null)
            {
                using (DB_CAW db_caw = new DB_CAW())
                {
                    RecentDiagramm = db_caw.getDiagramm(((Diagramm)dgd_diagrammsList.SelectedItem).ID);                       
                }

                flo_Menu.IsOpen = true;
            }
            else
            {
                flo_Menu.IsOpen = false;
            }
        }

        private void dgd_diagrammsList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dgd_diagrammsList.SelectedItem != null)
            {
                using (DB_CAW db_caw = new DB_CAW())
                {
                    RecentDiagramm = db_caw.getDiagramm(((Diagramm)dgd_diagrammsList.SelectedItem).ID);
                }

                flo_Menu.IsOpen = true;
            }
            else
            {
                flo_Menu.IsOpen = false;
            }
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

        private static void GetPdfThumbnail(String sourcePdfFilePath, String destinationPngFilePath, int pageNo)
        {
            // Use GhostscriptSharp to convert the pdf to a png
            GhostscriptWrapper.GenerateOutput(sourcePdfFilePath, destinationPngFilePath,
                new GhostscriptSettings
                {
                    Device = GhostscriptDevices.jpeg,
                    Page = new GhostscriptPages
                    {
                        Start = pageNo,
                        End = pageNo
                    },
                    Resolution = new System.Drawing.Size
                    {
                        // Render at 72x72 dpi
                        Height = 144,
                        Width = 144
                    },
                    Size = new GhostscriptPageSize
                    {
                        // The dimentions of the incoming PDF must be
                        // specified. The example PDF is US Letter sized.
                        Native = GhostscriptPageSizes.a4
                    }
                }
            );
        }
                      
    }
}
