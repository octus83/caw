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
using MahApps.Metro.Controls.Dialogs;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

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

        private int actualPageNumber = -1;     
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
        public String ProgrammPath
        {
            get
            {
                if (config.AppSettings.Settings["ProgrammPath"] == null)
                {
                    return null;
                  //  return "D:\\caw";
                }
                else
                {
                    return config.AppSettings.Settings["ProgrammPath"].Value;
                    //return "D:\\caw";
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
      
        private int _maxPageNumber = -1;
        public int MaxPageNumber
        {
            get
            {
                return this._maxPageNumber;
            }
            set
            {
                this._maxPageNumber = value;
            }

        }
        private String _diagrammId;
        public String DiagrammId
        {
            get
            {
                return this._diagrammId;
            }
            set
            {
                this._diagrammId = value;
                this.data = new Data(value,this);
                this.MaxPageNumber = data.getPageCout();
                Console.WriteLine("Console -> with Maxpagenumber : " + MaxPageNumber);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ProjectState = State.NoProjectSelected;
           
        }           

        private void Button_open_project(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenProjectWindow opw = new OpenProjectWindow(this);
                opw.ShowDialog();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Fehler beim Zugriff der Datenbank");
            }
            catch (Exception)
            {

                throw;
            }
            
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
            if (ProjectState == State.ProjectSelected)
            {
                if (actualPageNumber < this.MaxPageNumber)
                {
                    actualPageNumber++;
                    updatePagenumberLabel(actualPageNumber.ToString());
                    changePicture();
                    checkWhichFlyoutIsOpen();
                }
            }
        }

        private void previousPage()
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (actualPageNumber > 1)
                {
                    actualPageNumber--;
                    updatePagenumberLabel(actualPageNumber.ToString());
                    changePicture();
                    checkWhichFlyoutIsOpen();
                }
            }
        }
        private void goToPage(int page)
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (page <= this.MaxPageNumber && page > 0)
                {
                    actualPageNumber = page;
                    updatePagenumberLabel(actualPageNumber.ToString());
                    changePicture();
                    checkWhichFlyoutIsOpen();
                }
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
            sitenumberLabel.Content = "Seite " + update + " von " + this.MaxPageNumber;
        }

        private void changePicture()
        {
            if (ProjectState == State.ProjectSelected)
            {
                // clear possible drawings
                view.Children.Clear();

                if (actualPageNumber <= this.MaxPageNumber && actualPageNumber > 0)
                {

                    if (!checkIfFileExist(getOrginalPicturesPath(actualPageNumber)))
                    {
                        loadPictureToFileystemWithPriority(actualPageNumber);
                    }
                  
                    if(checkIfFileExist(getCustomPicturesPath(actualPageNumber)))
                    {
                     showImage.ImageSource = getBitmapImageFromUri(getCustomPicturesPath(actualPageNumber));
                    } else
                    {
                          showImage.ImageSource = getBitmapImageFromUri(getOrginalPicturesPath(actualPageNumber));
                    }

                }
                else
                {
                    MessageBox.Show("no pictures found");
                }
            }
        }

        private BitmapImage getBitmapImageFromUri(String path)
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(path);
            myBitmapImage.EndInit();
            return myBitmapImage;
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

        protected void propertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
//---------------------Public functions -----------------------------------------------
        public void cleanProject()
        {
            ProjectState = State.NoProjectSelected;
            this.actualPageNumber = -1;
            this.MaxPageNumber = -1;
            this._diagrammId = null;
            this.drawState = DrawState.None;
        }
        public void onProjectOpen()
        {
           ProjectState = State.ProjectSelected; 
           Thread thread = new Thread(new ThreadStart(getAllPictures));
           thread.Start();
           goToPage(1);
        }

        public void onProjectOpenFinish()
        {
            goToPage(1);
            win_Comm_btn_Drawing.Visibility = Visibility.Visible;
        }

        public void getAllPictures()
        {
            try
            {
                for (int i = 1; i <= MaxPageNumber; i++)
                {
                    if (!checkIfFileExist(getOrginalPicturesPath(i)))
                    {
                        loadPictureToFilesystem(i);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }
        public void loadPictureToFileystemWithPriority(int pagenumber)
        {
            Console.WriteLine("Ausgabe -> getting Picture " + pagenumber);
            CustomBitmapImage cbi = data.getBlobFast(data.getPIDFromPagenumber(pagenumber));
            data.savaOrginalBitmapimageToFile(cbi.OrginalImage, pagenumber);
            if (cbi.IsCustomImage)
            {
                data.savaCustomBitmapimageToFile(cbi.CustomImage, pagenumber);
            }
        }

        public void loadPictureToFilesystem(int pagenumber)
        {
            Console.WriteLine("Ausgabe -> getting Picture " + pagenumber);
            CustomBitmapImage cbi = data.getBlob(data.getPIDFromPagenumber(pagenumber));
            data.savaOrginalBitmapimageToFile(cbi.OrginalImage, pagenumber);
            if (cbi.IsCustomImage)
            {
                data.savaCustomBitmapimageToFile(cbi.CustomImage, pagenumber);
            }
        }
        public bool checkIfFileExist(String path)
        {
            if (File.Exists(path))
                return true;
            else
                return false;
        }
        public String getOrginalPicturesPath(int pagenumber)
        {
            return System.IO.Path.Combine(ProgrammPath, DiagrammId, data.getPIDFromPagenumber(pagenumber), pagenumber + "orginal.jpg"); ;
        }
        public String getCustomPicturesPath(int pagenumber)
        {
            return System.IO.Path.Combine(ProgrammPath, DiagrammId, data.getPIDFromPagenumber(pagenumber), pagenumber + "custom.jpg"); ;
        }
   
    }
}