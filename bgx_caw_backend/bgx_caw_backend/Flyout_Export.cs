﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls.Dialogs;

namespace bgx_caw_backend
{
    public partial class BackEnd
    {
        private async void flo_Export_tle_Export_Click(object sender, RoutedEventArgs e)
        {
            flo_Export.IsOpen = false;

            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var progressDialog = await this.ShowProgressAsync("Schaltplan wird exportiert", "Erstelle PDF");

            List<byte[]> blobList;

            await Task.Delay(10);

            progressDialog.SetProgress(0.1);

            //Create a new document
            iTextSharp.text.Document Doc = new iTextSharp.text.Document();
            Doc.SetPageSize(PageSize.A4.Rotate());

            //Store the document on the desktop
            string PDFOutput = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RecentDiagramm.ID+".pdf");
            PdfWriter writer = PdfWriter.GetInstance(Doc, new FileStream(PDFOutput, FileMode.Create, FileAccess.Write, FileShare.Read));

            //Open the PDF for writing
            Doc.Open();

            progressDialog.SetMessage("Hole Bildinformationen aus Datenbank");

            await Task.Delay(10);

            progressDialog.SetProgress(0.2);

            using (DB_CAW db_caw = new DB_CAW())
            {               
                blobList = db_caw.getAllBLOBFromDiagramm(RecentDiagramm.ID);         
            }

            progressDialog.SetMessage("Erstelle PDF");

            await Task.Delay(10);

            progressDialog.SetProgress(0.2);

            foreach (byte[] blob in blobList)
            {
                //Insert a page
                Doc.NewPage();
                //Add image
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(blob);

                image.ScaleToFit(PageSize.A4.Rotate().Width-40, PageSize.A4.Rotate().Height-60);

                Doc.Add(new iTextSharp.text.Jpeg(image));
                //byteArrayToImage(blob).Save("e:\\" + counter + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //counter++;

                progressDialog.SetProgress(0.2);
            }

            //Close the PDF
            Doc.Close();

            await progressDialog.CloseAsync();
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
    }
}
