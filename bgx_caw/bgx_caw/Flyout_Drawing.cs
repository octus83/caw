﻿using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace bgx_caw
{
    public enum DrawState
    {
        None,
        green,
        red,
        mark
    }
    public partial class MainWindow
    {
        private DrawState _drawState = DrawState.None;
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

        private void win_Comm_btn_Drawing_Click(object sender, EventArgs e)
        {
            closeAllRightFlyouts();
            closeAllLeftFlyouts();
            flo_up_draw.IsOpen = true;
        }
        private void toggle_btn_red_click(object sender, EventArgs e)
        {          
            this.drawState = DrawState.red;
        }
        private void toggle_btn_green_click(object sender, EventArgs e)
        {        
            this.drawState = DrawState.green;
        }
        private void toggle_btn_mark_click(object sender, EventArgs e)
        {
            this.drawState = DrawState.mark;
        }
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
        private void canvas_click(object sender, EventArgs e)
        {
            saveCanvas();

        }

        public void saveCanvas()
        {
            closeAllTopFlyouts();
            this.drawState = DrawState.None;
            // Save current canvas transform
            Transform transform = view.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            view.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(view.ActualWidth, view.ActualHeight);
            Size customSize = new Size(1920.0, 1080.0);
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

            data.savaCustomBitmapimageToFile(bitmapImage, actualPageNumber);
            Console.WriteLine("Bild erfolgreich gespeichert");
            showCustomPicturesSaveDialog();
            data.saveCustomBLOBInDB(actualPageNumber);
           
        }

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

       
    }
}
