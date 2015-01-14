using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace bgx_caw
{
    /// <summary>
    /// Enum welches den Zeichnen Zustand beschreibt
    /// </summary>
    public enum DrawState
    {
        None,
        green,
        red,
        mark
    }
    /// <summary>
    /// Teilklasse die die Logik des Zeichnen Flyouts beinhaltet
    /// </summary>
    public partial class MainWindow
    {
        Canvas viewHidden = new Canvas();
        ImageBrush showImageHidden = new ImageBrush();

        private int maxPixelWidth = 4000;
        /// <summary>
        /// Maximale Custom picture With
        /// Speicher Canvas kann nicht höher als 4000 pixel speichern
        /// </summary>
        private int customPictureWidth = 4000;
        private int customPictureHeight;

        private DrawState _drawState = DrawState.None;
        /// <summary>
        /// Zeichnen zustand
        /// </summary>
        public DrawState drawState
        {
            set
            {
                if (drawState == value)
                {
                    this._drawState = DrawState.None;
                }
                else
                {
                    this._drawState = value;
                }
                setToggleButtonBackgroundcolor();
            }
            get
            {
                return this._drawState;
            }

        }
        /// <summary>
        /// Zeichnen Button Click Event 
        /// Öffnet das Zeichnen Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Comm_btn_Drawing_Click(object sender, EventArgs e)
        {
            openDrawFlyout();
        }
        /// <summary>
        /// Öffnet das Zeichnen Flyout
        /// </summary>
        private void openDrawFlyout()
        {
            closeAllRightFlyouts();
            closeAllLeftFlyouts();
            flo_up_draw.IsOpen = true;
        }
        /// <summary>
        /// Button Click Event für die Farbe Rotim Zeichnen Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggle_btn_red_click(object sender, EventArgs e)
        {          
            this.drawState = DrawState.red;
        }
        /// <summary>
        /// Button Click Event für die Farbe Grün im Zeichnen Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggle_btn_green_click(object sender, EventArgs e)
        {        
            this.drawState = DrawState.green;
        }
        /// <summary>
        /// Button Click Event für die Farbe Grau im Zeichnen Fylout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggle_btn_mark_click(object sender, EventArgs e)
        {
            this.drawState = DrawState.mark;
        }
        /// <summary>
        /// Logik für die Hintergrundfarben der Farbbuttons
        /// Es darf immer nur eine Farbe Aktiv sein
        /// </summary>
        private void setToggleButtonBackgroundcolor()
        {
                switch (drawState)
                {
                    case DrawState.None:
                        toggle_btn_mark.Background = Brushes.Black;
                        toggle_btn_green.Background = Brushes.Black;
                        toggle_btn_red.Background = Brushes.Black;
                        break;
                    case DrawState.green:
                        toggle_btn_green.Background = Brushes.Green;
                        toggle_btn_mark.Background = Brushes.Black;
                        toggle_btn_red.Background = Brushes.Black;
                        break;
                    case DrawState.red:
                        toggle_btn_red.Background = Brushes.Red;
                        toggle_btn_mark.Background = Brushes.Black;
                        toggle_btn_green.Background = Brushes.Black;
                        break;
                    case DrawState.mark:
                        toggle_btn_mark.Background = Brushes.Gray;
                        toggle_btn_green.Background = Brushes.Black;
                        toggle_btn_red.Background = Brushes.Black;
                        break;
                    default:
                        toggle_btn_mark.Background = Brushes.Black;
                        toggle_btn_green.Background = Brushes.Black;
                        toggle_btn_red.Background = Brushes.Black;
                        break;
                }
        }
        /// <summary>
        /// Speichern eines Bildes Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_click(object sender, EventArgs e)
        {
            saveCanvas();
        }
        /// <summary>
        /// Speichert das Aktuelle Bild
        /// 
        /// </summary>
  /*      public void saveCanvas()
        {
            
            closeAllTopFlyouts();
            this.drawState = DrawState.None;
            // Save current canvas transform
            Transform transform = view.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            view.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(view.ActualWidth, view.ActualHeight);
            // Measure and arrange the surface
            // VERY IMPORTANT
            view.Measure(size);
            view.Arrange(new Rect(size));

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width,
           (int)size.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(view);

            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
             //   bitmapImage.DecodePixelHeight = 1080;
             //   bitmapImage.DecodePixelWidth = 1920;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            //Speichert das Bild als File
            data.savaCustomBitmapimageToFile(bitmapImage, actualPageNumber);
            Console.WriteLine("Bild erfolgreich gespeichert");
            showCustomPicturesSaveDialog();
            data.saveCustomBLOBInDB(actualPageNumber);
           
        } */


       public void saveCanvas()
        {

            closeAllTopFlyouts();
            this.drawState = DrawState.None;
            // Save current canvas transform
            Transform transform = view.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            viewHidden.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(customPictureWidth, customPictureHeight);
            // Measure and arrange the surface
            // VERY IMPORTANT
            viewHidden.Measure(size);
            viewHidden.Arrange(new Rect(size));
            logger.log("Size: X: " + size.Width + " Y: " + size.Height, "Flyout_drawings.cs");
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width,
           (int)size.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(viewHidden);

            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            //Speichert das Bild als File
            String path= data.savaCustomBitmapimageToFile(bitmapImage, actualPageNumber);
            logger.log("Bild erfolgreich gespeichert","Flyout_Drawing.cs -> saveCanvas");
          //showCustomPicturesSaveDialog();
            data.saveCustomBLOBInDB(actualPageNumber, path);

        }



        /// <summary>
        /// Asynchrone Funktion die die Bild Speicherung Visuell darstellt
        /// Hat nicht mit der wirklichen Bildspeicher dauer zu tun
        /// </summary>
        public async void showCustomPicturesSaveDialog()
        {
            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var progressDialog = await this.ShowProgressAsync("Speichern...", "Bild wird erstellt");
            for (int i = 0; i < 10; i++)
            {
                if (i == 5)
                {
                    progressDialog.SetMessage("Bild wird in Datenbank geladen");
                }
                 progressDialog.SetProgress((0.99 / 10) * i);
                 await Task.Delay(200);
            }
            progressDialog.SetProgress(1.0);
            progressDialog.SetMessage("Bild erfolgreich gespeichert");
            await Task.Delay(1000);
            await progressDialog.CloseAsync();
        }

        public int getHeightofPicture()
        {
            int h = 5;

            if (checkIfFileExist(getCustomPicturesPath(actualPageNumber)))
            {
               h =  getBitmapImageFromUri(getCustomPicturesPath(actualPageNumber)).PixelHeight;
            }
            else
            {
              h =  getBitmapImageFromUri(getOrginalPicturesPath(actualPageNumber)).PixelHeight;

            }
            logger.log("Höhe: " + h, "Flyout_Drawing.cs");
            return h;
        }

        public int getWidthofPicture()
        {
            int w = 5;

            if (checkIfFileExist(getCustomPicturesPath(actualPageNumber)))
            {
                w = getBitmapImageFromUri(getCustomPicturesPath(actualPageNumber)).PixelWidth;
            }
            else
            {
                w = getBitmapImageFromUri(getOrginalPicturesPath(actualPageNumber)).PixelWidth;

            }
            logger.log("Breite: " + w, "Flyout_Drawing.cs");
            return w;
        }

        private void setPicturesSize()
        {
           int h = getHeightofPicture();
           int w  =getWidthofPicture();
           double faktor = (double)w / maxPixelWidth;
           customPictureHeight = (int)((double)h / faktor);
           customPictureWidth = maxPixelWidth;
           logger.log("Setting CustomPictureHeight to: " + customPictureHeight + " and customPictureWidth to: " + customPictureWidth, "Flyout_Drawing.cs");
           viewHidden.Background = showImageHidden;
           viewHidden.Height = customPictureHeight;
           viewHidden.Width = customPictureWidth;        
        }
                
       

       
    }
}
