using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls.Dialogs;

namespace bgx_caw_backend
{
    public partial class BackEnd
    {
        private void flo_export_btn_fld_Click(object sender, RoutedEventArgs e)
        {
            ExportDialog = new FolderBrowserDialog();

            if (ExportDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                propertyChanged("ExportDialog");
            }
        }

        private async void flo_Export_tle_Export_Click(object sender, RoutedEventArgs e)
        {
            Boolean exportMarkings = false;

            flo_Export.IsOpen = false;

            //Sollen Markierungen exportiert werden?
            if(flo_Export_chb_rst.IsChecked == true)
            {
                exportMarkings = true;
            }

            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var progressDialog = await this.ShowProgressAsync("Schaltplan wird exportiert", "Erstelle PDF");

            List<byte[]> blobList;

            await Task.Delay(10);

            progressDialog.SetProgress(0.1);

            //Create a new document
            iTextSharp.text.Document Doc = new iTextSharp.text.Document(PageSize.A4.Rotate());

            //Store the document in the choosen Folder
            string PDFOutput = Path.Combine(ExportDialog.SelectedPath, RecentDiagramm.JobNumber + "_" + RecentDiagramm.SerialNumber + "_" + RecentDiagramm.FieldName + ".pdf");
            PdfWriter writer = PdfWriter.GetInstance(Doc, new FileStream(PDFOutput, FileMode.Create, FileAccess.Write, FileShare.Read));

            //Open the PDF for writing
            Doc.Open();

            progressDialog.SetMessage("Hole Bildinformationen aus Datenbank");

            await Task.Delay(10);

            progressDialog.SetProgress(0.1);

            using (DB_CAW db_caw = new DB_CAW())
            {               
                blobList = db_caw.getAllBLOBFromDiagramm(RecentDiagramm.ID, exportMarkings);         
            }

            progressDialog.SetMessage("Erstelle PDF");

            await Task.Delay(20);

            progressDialog.SetProgress(0.2);

            int counter = 0;

            foreach (byte[] blob in blobList)
            {
                //Insert a page
                Doc.NewPage();
                //Add image
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(blob);

                image.ScaleToFit(PageSize.A4.Rotate().Width+55, PageSize.A4.Rotate().Height-50);

                Doc.Add(new iTextSharp.text.Jpeg(image));

                progressDialog.SetProgress(0.2+((0.8 / blobList.Count) * counter++));
                await Task.Delay(20);
            }
            //Close the PDF
            Doc.Close();

            progressDialog.SetProgress(1.0);
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
