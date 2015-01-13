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
    /// <summary>
    /// Programm State
    /// </summary>
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

        /// <summary>
        /// Aktuelle Seitenzahl wenn ein Schaltplan geöffnet ist
        /// </summary>
        private int actualPageNumber = -1;
        /// <summary>
        /// Vorherige Seite
        /// </summary>
        private int lastActualPageNumber = -1;

        /// <summary>
        ///  Data Klasse referenz
        /// </summary>
        private Data data;

        private Logger logger = new Logger();

        /// <summary>
        /// Setzt ein lock auf die Aktuelle Seite die grade aus
        /// der Datenbank geholt wird
        /// </summary>
        private int lockPage = -1;

        /// <summary>
        /// config für Datenbanknamen , Programm pfad und DatenbankAdresse
        /// </summary>
        Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Datenbank Adresse
        /// </summary>
        public String DataSource
        {
            get
            {
                if (config.AppSettings.Settings["DataSource"] == null)
                {                 
                    return null;
                }
                else
                {
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
        /// <summary>
        /// Datenbank Name
        /// </summary>
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
        /// <summary>
        /// Programm Pfad in dem das Programm arbeitet.
        /// (Bilder aus der Datenbank dort hinein lädt)
        /// </summary>
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
        /// <summary>
        /// Programmzustand 
        /// </summary>
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
        /// <summary>
        /// Maximale Seizenzahl eines Diagramms
        /// </summary>
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
        /// <summary>
        /// Diagramm ID (Primary Key) eines Diagramms aus der Datenbank
        /// </summary>
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
        /// <summary>
        /// Constructor 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ProjectState = State.NoProjectSelected;          
        }
        /// <summary>
        /// Projekt Öffnen Button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Comm_btn_Project_Open_Click(object sender, RoutedEventArgs e)
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
      

      /// <summary>
      /// Rechte Maus Taste Event auf dem Anzeige Container
      /// Öffnet das Info Flyout
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private void renderContainer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (drawState == DrawState.None)
                {
                    openInfo();
                }
                else
                {
                    openDrawFlyout();
                }
            }
        }
      
        /// <summary>
        /// Click auf dem Seitenzahl Label Event
        /// Öffnet das Jump to Page Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sitenumberLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            flo_bott_jump.IsOpen = true;
            jump_to_page_textBox.Focus();
        }

        /// <summary>
        /// Mausrad event für Seitenwechsel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clipBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (drawState == DrawState.None)
                {
                    if (e.Delta > 0)
                    {
                        previousPage();
                    }
                    else if (e.Delta < 0)
                    {
                        nextPage();
                    }
                }
            }
        }

        /// <summary>
        /// Öffnet das Info Flyout auf der rechten Seite
        /// </summary>
        private void openInfo()
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (!flo_right_info.IsOpen)
                {
                    closeAllRightFlyouts();
                    flo_right_info.IsOpen = true;
                }
            }
        }
        /// <summary>
        /// Nächste Seite eines Diagramms
        /// </summary>
        private void nextPage()
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (actualPageNumber < this.MaxPageNumber)
                {
                    setActualPageNumber(actualPageNumber + 1);
                    updatePagenumberLabel(actualPageNumber.ToString());
                    changePicture();
                    checkWhichFlyoutIsOpen();
                    findTileToActualPagenumber();
                }
            }
        }
        /// <summary>
        /// vorherige Seite eines Diagramms
        /// </summary>
        private void previousPage()
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (actualPageNumber > 1)
                {
                    setActualPageNumber(actualPageNumber - 1);
                    updatePagenumberLabel(actualPageNumber.ToString());
                    changePicture();
                    checkWhichFlyoutIsOpen();
                    findTileToActualPagenumber();
                }
            }
        }
        /// <summary>
        /// Spring zu Seite X eines Diagramms
        /// </summary>
        /// <param name="page"></param>
        private void goToPage(int page)
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (page <= this.MaxPageNumber && page > 0)
                {
                    flo_up_draw.IsOpen = false;
                    setActualPageNumber(page);
                    updatePagenumberLabel(actualPageNumber.ToString());
                    changePicture();
                    checkWhichFlyoutIsOpen();
                    findTileToActualPagenumber();
                }
            }
        }

        private void setActualPageNumber(int page)
        {
            lastActualPageNumber = actualPageNumber;
            actualPageNumber = page;
        }
        /// <summary>
        /// Überprüft welches Flyout offen ist und 
        /// erstellt dementsprechend die Informationen
        /// </summary>
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
        /// <summary>
        /// Aktualisierung der Seitenzahl  
        /// </summary>
        /// <param name="update"></param>
        private void updatePagenumberLabel(String update)
        {
            sitenumberLabel.Content = update + "/" + this.MaxPageNumber;
        }
        /// <summary>
        /// Wechsel eines Bildes anhand der Aktuellen Seitenzahl
        /// </summary>
        private void changePicture()
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (actualPageNumber != lockPage)
                {
                    // clear possible drawings
                    view.Children.Clear();
                    viewHidden.Children.Clear();
                    if (actualPageNumber <= this.MaxPageNumber && actualPageNumber > 0)
                    {
                        //Speicher aufräumen da das cachen von bild dateien sonst zu viel Arbeitsspeicher frisst
                        GC.Collect();
                        // Überprüfung ob eine Orginal Bilddatei vorhanden ist
                        // ansonsten wird sie nachgeladen
                        if (!checkIfFileExist(getOrginalPicturesPath(actualPageNumber)))
                        {
                            lockPage = actualPageNumber;
                            loadPictureToFileystemWithPriority(actualPageNumber);
                            lockPage = -1;
                            
                        }
                        BitmapImage laodedImage;
                        if (checkIfFileExist(getCustomPicturesPath(actualPageNumber)))
                        {
                            if (showImage.ImageSource != null)
                            {
                                showImage.ImageSource = null;
                            }
                            laodedImage = getBitmapImageFromUri(getCustomPicturesPath(actualPageNumber));
                            showImage.ImageSource = laodedImage;
                            showImageHidden.ImageSource = laodedImage;
                        }
                        else
                        {
                            if (showImage.ImageSource != null)
                            {
                                showImage.ImageSource = null;
                            }
                            laodedImage = getBitmapImageFromUri(getOrginalPicturesPath(actualPageNumber));
                            showImage.ImageSource = laodedImage;
                            showImageHidden.ImageSource = laodedImage;
                            
                        }

                    }
                    else
                    {
                        MessageBox.Show("no pictures found");
                    }
                }
                else
                {
                    Console.Write("LOCK PAGE Ausgeführt");
                    Thread.Sleep(1000);
                    changePicture();
                }
            }
        }

        /// <summary>
        /// Erzeugt ein BitmapImage aus einem Dateipfad
        /// Cached das Bild damit kein lock auf das Bild entsteht
        /// (Prozess der auf das Bild zugreift)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private BitmapImage getBitmapImageFromUri(String path)
        {
           try
            {
                BitmapImage myRetVal = null;
                if (path != null)
                {
                    BitmapImage image = new BitmapImage();
                    using (FileStream stream = File.OpenRead(path))
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                    }
                    myRetVal = image;
                }
                return myRetVal;
            }
            catch (Exception ex)
            {
                logger.log("Fehler beim Laden der Seite: " + actualPageNumber, "MainWindows.Xaml.cs -> getBitmapImageFromUri");
                logger.log(ex.ToString(), "MainWindows.Xaml.cs -> getBitmapImageFromUri");
                MessageBox.Show("Fehler beim Laden der Seite: " + actualPageNumber);
                throw;
            }
        }
        /// <summary>
        /// Schließt alle Flyouts die sich links öffnen
        /// </summary>
        private void closeAllLeftFlyouts()
        {
            flo_left_potential.IsOpen = false;
            flo_left_search.IsOpen = false;
            flo_left_parts.IsOpen = false;
            renderContainer.Margin = new Thickness(75, 45, 75, 0);
        }
        /// <summary>
        /// schließt alle Fylouts die sich rechts öffnen
        /// </summary>
        private void closeAllRightFlyouts()
        {
            flo_right_info.IsOpen = false;
            flo_right_sites.IsOpen = false;
        }
        /// <summary>
        /// schließt alle Fylouts 
        /// </summary>
        private void closeAllFlyouts()
        {
            closeAllLeftFlyouts();
            closeAllRightFlyouts();
            closeAllTopFlyouts();
            closeAllBottomFlyouts();
        }
        /// <summary>
        /// schließt den Seiten Flyout
        /// </summary>
        private void closeSitesFlyout()
        {
            if (flo_right_sites.IsOpen)
            {
                flo_right_sites.IsOpen = false;
            }
        }
        /// <summary>
        /// schließt alle Flyouts die sich oben öffnen
        /// </summary>
        private void closeAllTopFlyouts()
        {
            flo_up_draw.IsOpen = false;
            flo_Settings.IsOpen = false;
        }
        /// <summary>
        /// schließt alle Fylouts die sich unten öffnen
        /// </summary>
        private void closeAllBottomFlyouts()
        {
            flo_bott_jump.IsOpen = false;
        }
        /// <summary>
        /// 2 Way Databinding zwischen View und Model
        /// </summary>
        /// <param name="name"></param>
        protected void propertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
//---------------------Public functions -----------------------------------------------

        /// <summary>
        /// setzt das Programm auf default Einstellungen 
        /// bevor  ein neues Projekt geöffnet wird
        /// </summary>
        public void cleanProject()
        {
            ProjectState = State.NoProjectSelected;
            this.actualPageNumber = -1;
            this.MaxPageNumber = -1;
            this._diagrammId = null;
            this.drawState = DrawState.None;
            closeAllFlyouts();
        }
        /// <summary>
        /// Öffnet ein neues Projekt/Diagramm
        /// </summary>
        public void onProjectOpen()
        {
           ProjectState = State.ProjectSelected; 
           Thread thread = new Thread(new ThreadStart(getAllPictures));
           thread.Start();
            onProjectOpenFinish();
        }
        /// <summary>
        /// Wenn ein neues Projekt/Diagramm geöffnet wurde
        /// </summary>
        public void onProjectOpenFinish()
        {
            goToPage(1);
            setPicturesSize();
            sitenumberLabel.Visibility = Visibility.Visible;
            win_Comm_btn_Drawing.Visibility = Visibility.Visible;          
        }
        /// <summary>
        /// Lädt alle Bilder aus der Datenbank in das
        /// Programmverzeichnis falls sie noch nicht 
        /// vorhanden sind
        /// </summary>
        public void getAllPictures()
        {
            try
            {
                for (int i = 1; i <= MaxPageNumber; i++)
                {
                    if (!checkIfFileExist(getOrginalPicturesPath(i)))
                    {
                        if (i != lockPage)
                        { 
                        lockPage = i;
                        loadPictureToFilesystem(i);
                        lockPage = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }
        /// <summary>
        /// Lädt Bild X mit priorität ins Programmverzeichnis
        /// </summary>
        /// <param name="pagenumber"></param>
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
        /// <summary>
        /// Lädt Bild X ins Programmverzeichnis
        /// </summary>
        /// <param name="pagenumber"></param>
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
        /// <summary>
        /// Überprüfung ob ein Bild schon vorhanden ist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool checkIfFileExist(String path)
        {           
            if (File.Exists(path))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Generierung eines Dateipfades für ein Orginalbild
        /// </summary>
        /// <param name="pagenumber"></param>
        /// <returns></returns>
        public String getOrginalPicturesPath(int pagenumber)
        {
            return System.IO.Path.Combine(ProgrammPath, DiagrammId, data.getPIDFromPagenumber(pagenumber), pagenumber + "orginal.jpg"); 
        }
        /// <summary>
        /// Generierung eines Dateipfades für ein custom Bild
        /// </summary>
        /// <param name="pagenumber"></param>
        /// <returns></returns>
        public String getCustomPicturesPath(int pagenumber)
        {
            return System.IO.Path.Combine(ProgrammPath, DiagrammId, data.getPIDFromPagenumber(pagenumber), pagenumber + "custom.jpg"); 
        }      
        /// <summary>
        /// Jump to Page Button Click Event
        /// Versuch zu Seite X des Projektes/Diagramms zu springen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Jump_To_Page_Click(object sender, RoutedEventArgs e)
        {
            int page;
            try
            {
                page = Convert.ToInt32(jump_to_page_textBox.Text);
                if (page > 0 && page <= MaxPageNumber)
                {
                    goToPage(page);
                    closeAllBottomFlyouts();
                }
                else
                {
                    MessageBox.Show("Falsche Seitenzahl eingabe");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Falsche Seitenzahl eingabe");
         
            }
            
        }
        /// <summary>
        /// Öffnet die Keyboard Tastatur 
        /// </summary>
        private static void openOnScreenKeyboard()
        {
            System.Diagnostics.Process.Start("C:\\Program Files\\Common Files\\Microsoft shared\\ink\\TabTip.exe");
        }
        /// <summary>
        /// schließt die Keyboard Tastatur
        /// </summary>
        private static void killOnScreenKeyboard()
        {
            if (System.Diagnostics.Process.GetProcessesByName("TabTip").Count() > 0)
            {
                System.Diagnostics.Process asd = System.Diagnostics.Process.GetProcessesByName("TabTip").First();
                asd.Kill();
            }

        }     
    }
}