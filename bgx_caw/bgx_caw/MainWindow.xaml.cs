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
                if (this._images != null)
                {
                    this._images.Clear();
                   
                }
                this._images = value;
                
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
             this.data = new Data(value,this);
             this.MaxPageNumber = data.getPageCout();
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
            if (ProjectState == State.ProjectSelected)
            {
                if (actualPageNumber <= this.MaxPageNumber)
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
            // clear possible drawings
            view.Children.Clear();

            if (actualPageNumber <= this.MaxPageNumber && actualPageNumber > 0)
            {
                //showImage.ImageSource = getBitmapSource();
                if (_images.ElementAt((actualPageNumber - 1)).IsCustomImage)
                {
                    showImage.ImageSource = _images.ElementAt((actualPageNumber - 1)).CustomImage;
                }
                else
                {               
                    showImage.ImageSource = _images.ElementAt((actualPageNumber - 1)).OrginalImage;
                   
                  
                    Console.WriteLine("Ausgabe Console ->");
                    
                }
               // showImage.RenderTransform = new ScaleTransform
                        //(1, 1, 0, 0);
            }
            else
            {
                MessageBox.Show("no pictures found");
            }
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
            this._id = null;
            this.drawState = DrawState.None;
        }
        public void onProjectOpen()
        {
           ProjectState = State.ProjectSelected; 
           //data.getBitmapList(this);
         // getBitmapList();
         //  goToPage(1);      
           //data.creatAllPictures();
          // getonePicture();
         //  Task getPictures = new Task(new Action(getonePicture));
        //   getPictures.Start();
           Thread thread = new Thread(new ThreadStart(getonePicture));
           thread.Start();
        }

        public void onProjectOpenFinish()
        {
            goToPage(1);
            win_Comm_btn_Drawing.Visibility = Visibility.Visible;
        }

        public async void getBitmapList()
        {
            List<String> sortedPageIdList = data.SortedPageIdList;
            List<CustomBitmapImage> list = new List<CustomBitmapImage>();
            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var progressDialog = await this.ShowProgressAsync("Schaltplan wird geladen ....", "");
            progressDialog.SetProgress(0.0);

            await Task.Delay(200);

            double percent = 0.1 + (100 / sortedPageIdList.Count) / 100;
            int count = 1;

            foreach (var item in sortedPageIdList)
            {
                CustomBitmapImage cbi = new CustomBitmapImage();
                BitmapImage b = data.createbitmapsource(data.getBlob(item));
                cbi.OrginalImage = b;
                data.savaBitmapimageToFile(b, count);
              
                list.Add(cbi);
                progressDialog.SetMessage("Seite: " + count + " wurde geladen");
                progressDialog.SetProgress(0.99 / sortedPageIdList.Count * count);
                await Task.Delay(2000);
                count++;
            }
            progressDialog.SetMessage("fertig geladen");
            progressDialog.SetProgress(1.0);
            await Task.Delay(500);
            await progressDialog.CloseAsync();
            this.Images = list;
            this.onProjectOpenFinish();
        }

        public ImageSource getBitmapSource()
        {
           
           return data.createbitmapsource( data.getBlob(data.getPIDFromPagenumber(actualPageNumber)));
        }

        public void getonePicture()
        {
            try
            {
                for (int i = 1; i < MaxPageNumber; i++)
                {
                    BitmapImage b = data.createbitmapsource(data.getBlob(data.getPIDFromPagenumber(i)));
                    data.savaBitmapimageToFile(b, i);
                }
            }
            catch (Exception ex)
            {
                // log errors
            }
           
        }
   
    }
}