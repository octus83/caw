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
    public partial class MainWindow : INotifyPropertyChanged
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

        private String _diagrammID;
        public String DiagrammID
        {
            get
            {
                return _diagrammID;
            }
            set
            {
                _diagrammID = value;
                OnPropertyChanged("DiagrammID");
            }
        }

        public MainWindow()
        {
            
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
            //Flyout open Delete
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
                //System.Windows.Forms.MessageBox.Show("NICHTNULL");
            }            
        }



        

        

    }
}
