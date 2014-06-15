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
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Configuration;

namespace bgx_caw
{
    public  enum State
    {
        None,
        ProjectSelected,
        NoProjectSelected
    }
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow :MetroWindow 
    {

        private int actualPageNumber = 0;
        private int maxPageNumber = 15;
        private Data data;

     


        Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

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

        private State _projectState;
        public State ProjectState
        {
            get
            {
                return this._projectState;
            }
            set
            {
                this._projectState = value;
            }
        }
        private List<CustomBitmapImage> _images;
        public List<CustomBitmapImage> Images
        {
            get
        {
            return this._images;
        }
            set
            {
                this._images = value;
            }
        }
        public int MaxPageNumber
        {
            get
            {
                return this.maxPageNumber;
            }
            set
            {
                this.maxPageNumber = value;
            }

        }
        private String _id;
        public String ID
        {
            get
            {
                return this._id;
            }
            set
            {
             this._id = value;
             this.data = new Data(value);
             maxPageNumber = data.getPageCout();
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            ProjectState = State.NoProjectSelected;
           
        }

       

        

        private void Button_open_project(object sender, RoutedEventArgs e)
        {
            OpenProjectWindow opw = new OpenProjectWindow(this);
            opw.ShowDialog();
        }

        private void Button_next_page(object sender, RoutedEventArgs e)
        {
            nextPage();
        }

        private void Button_previous_page(object sender, RoutedEventArgs e)
        {
            previousPage();
        }

        private void Button_open_info(object sender, RoutedEventArgs e)
        {
            if (ProjectState == State.ProjectSelected)
            {
                flo_right_info.IsOpen = true;
            }
            else
            {
                MessageBox.Show("No Project selected");
            }

        }

        private void nextPage()
        {
            if (actualPageNumber <= maxPageNumber)
            {
                actualPageNumber++;
                updatePagenumberLabel(actualPageNumber.ToString());
                changePicture();
                checkWhichFlyoutIsOpen();
            }
        }

        private void previousPage()
        {
            if (actualPageNumber > 1)
            {
                actualPageNumber--;
                updatePagenumberLabel(actualPageNumber.ToString());
                changePicture();
                checkWhichFlyoutIsOpen();
            }
        }
        private void goToPage(int page)
        {
          
            if (page <= maxPageNumber && page > 0)
            {
                actualPageNumber = page;
                updatePagenumberLabel(actualPageNumber.ToString());
                changePicture();
                checkWhichFlyoutIsOpen();
            }
        }
        private void checkWhichFlyoutIsOpen()
        {
            if (flo_left_parts.IsOpen)
            {
                buildPartFlyout();
            }
            else if (flo_left_potential.IsOpen)
            {
                buildPotentialFlyout();
            }
        }

        private void updatePagenumberLabel(String update)
        {
            sitenumberLabel.Content = "Seite " + update + " von " + maxPageNumber;
        }

        private void changePicture()
        {
            view.Children.Clear();

            if (actualPageNumber <= maxPageNumber && actualPageNumber > 0)
            {
                if (_images.ElementAt((actualPageNumber - 1)).IsCustomImage)
                {
                    showImage.ImageSource = _images.ElementAt((actualPageNumber - 1)).CustomImage;
                }
                else
                {
                    showImage.ImageSource = _images.ElementAt((actualPageNumber - 1)).OrginalImage;
                }
               // showImage.RenderTransform = new ScaleTransform
                        //(1, 1, 0, 0);
            }
            else
            {
                MessageBox.Show("no pictures found");
            }
        }

        public void onProjectOpen()
        {
            
           Images=  data.getBitmapList();
           goToPage(1);
           win_Comm_btn_Drawing.Visibility = Visibility.Visible;
          
        }

        private void closeAllLeftFlyouts()
        {
            flo_left_potential.IsOpen = false;
            flo_left_search.IsOpen = false;
            flo_left_parts.IsOpen = false;
        }
        private void closeAllRightFlyouts()
        {
            flo_right_info.IsOpen = false;
            flo_right_sites.IsOpen = false;
        }
      



        protected void propertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }



        private void clipBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
         /*   Point p = e.GetPosition(clipBorder);
            var st = (ScaleTransform)showImage.RenderTransform;
            double zoom = e.Delta > 0 ? .2 : -.2;
            st.ScaleX += zoom;
            st.ScaleY += zoom;
            st.CenterX = p.X;
            st.CenterY = p.Y;*/
        }
    }
}