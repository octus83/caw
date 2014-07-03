using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using GhostscriptSharp;
using GhostscriptSharp.Settings;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls.Dialogs;

namespace bgx_caw_backend
{
    public partial class BackEnd
    {
        private void flo_import_btn_dxf_Click(object sender, RoutedEventArgs e)
        {
            DXFDialog = new FolderBrowserDialog();

            if (DXFDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                propertyChanged("DXFDialog");
                DXF_parser = new DXF_Parser(new DirectoryInfo(DXFDialog.SelectedPath));
                ImportDiagramm = DXF_parser.Diagramm;
            }
        }

        private void flo_import_btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            PDFDialog = new OpenFileDialog();

            if (PDFDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                propertyChanged("PDFDialog");
            }
        }

        private async void flo_import_tle_import_Click(object sender, RoutedEventArgs e)
        {
            if (ImportDiagramm == null || PDFDialog == null)
            {
                await this.ShowMessageAsync("Fehler", "Es muss ein DXF-Ordner und ein PDF-File ausgewählt sein");
                return;
            }

            String diagrammPath = System.IO.Path.Combine(ProgrammPath, ImportDiagramm.ID);
            String pagePath;

            try
            {
                PDFReader = new PdfReader(PDFDialog.FileName);

                if (ImportDiagramm.pages_List.Count == PDFReader.NumberOfPages)
                {
                    this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
                    var progressDialog = await this.ShowProgressAsync("Schaltplan wird importiert", "Diagram Ordner " + ImportDiagramm.ID + " wird erstellt");

                    progressDialog.SetProgress(0.1);

                    //Create DiagrammFolder (ID)                    
                    System.IO.Directory.CreateDirectory(diagrammPath);

                    await Task.Delay(1);

                    int counter = 1;

                    //Create PageFolder (P_id) containing Overlays                
                    foreach (Page page in ImportDiagramm.pages_List)
                    {
                        //Ordner
                        progressDialog.SetMessage("Page " + page.P_id + " wird erstellt");
                        
                        pagePath = System.IO.Path.Combine(diagrammPath, page.P_id);
                        System.IO.Directory.CreateDirectory(pagePath);

                        //Bild generieren und kopieren
                        GetPdfThumbnail(PDFDialog.FileName, System.IO.Path.Combine(pagePath, counter + ".jpg"), counter);

                        //Bild als Blob in Page-Objekt einfügen
                        page.Image = GetPhoto(System.IO.Path.Combine(pagePath, counter + ".jpg"));

                        await Task.Delay(1);

                        counter++;
                        progressDialog.SetProgress((0.7 / ImportDiagramm.pages_List.Count) * counter);
                    }

                    progressDialog.SetProgress(0.9);

                    progressDialog.SetMessage("Diagramm in Datenbank anlegen");
                    await Task.Delay(0);
                    progressDialog.SetProgress(1.0);

                    using (DB_CAW db_caw = new DB_CAW())
                    {
                        db_caw.addDiagramm(ImportDiagramm);
                    }

                    await progressDialog.CloseAsync();

                    propertyChanged("DiagrammsList");

                    Directory.Delete(diagrammPath, true);

                    flo_bottom.IsOpen = false;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Das PDF hat " + PDFReader.NumberOfPages + " Seiten und das DXF " + ImportDiagramm.pages_List.Count);
                }
            }
            catch(Exception exc)
            {
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
            finally
            {
                ImportDiagramm = null;
                DXF_parser = null;
                DXFDialog = null;
                PDFDialog = null;
            }

        }

        // **** Read Image into Byte Array from Filesystem
        public static byte[] GetPhoto(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] photo = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return photo;
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
                        // Render at 300x300 dpi
                        Height = 300,
                        Width = 300
                    },
                    Size = new GhostscriptPageSize
                    {
                        // The dimentions of the incoming PDF must be
                        // specified.
                        Native = GhostscriptPageSizes.a4
                    }
                }
            );
        }                    

        private void flo_import_tle_del_Click(object sender, RoutedEventArgs e)
        {
            DXFDialog = null;
            PDFDialog = null;
            ImportDiagramm = null;
        }
    }
}
