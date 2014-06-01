using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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
                dxf_parser = new DXF_Parser(new DirectoryInfo(DXFDialog.SelectedPath));
                ImportDiagramm = dxf_parser.Diagramm;
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
            //String projectFolder = @"e:\programme\caw\projects\";
            String diagrammPath = System.IO.Path.Combine(ProgrammPath, ImportDiagramm.ID);
            String pagePath;

            try
            {
                pdfReader = new PdfReader(PDFDialog.FileName);

                if (ImportDiagramm.pages_List.Count == pdfReader.NumberOfPages)
                {
                    this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
                    var progressDialog = await this.ShowProgressAsync("Schaltplan wird importiert", "Diagram Ordner " + ImportDiagramm.ID + " wird erstellt");

                    progressDialog.SetProgress(0.1);

                    //Create DiagrammFolder (ID)                    
                    System.IO.Directory.CreateDirectory(diagrammPath);

                    await Task.Delay(200);

                    int counter = 0;

                    //Create PageFolder (P_id) containing Overlays                
                    foreach (Page page in ImportDiagramm.pages_List)
                    {
                        progressDialog.SetMessage("Page Ordner " + page.P_id + " wird erstellt");
                        pagePath = System.IO.Path.Combine(diagrammPath, page.P_id);
                        System.IO.Directory.CreateDirectory(pagePath);

                        await Task.Delay(100);

                        counter++;
                        progressDialog.SetProgress((0.7 / ImportDiagramm.pages_List.Count) * counter);
                    }

                    //Kopiere pdf in project - Ordner
                    progressDialog.SetMessage("PDF wird kopiert");
                    System.IO.File.Copy(PDFDialog.FileName, System.IO.Path.Combine(diagrammPath, ImportDiagramm.ID + ".pdf"));

                    await Task.Delay(300);
                    progressDialog.SetProgress(0.9);

                    progressDialog.SetMessage("Diagramm in Datenbank anlegen");
                    await Task.Delay(300);
                    progressDialog.SetProgress(1.0);

                    using (DB_CAW db_caw = new DB_CAW())
                    {
                        db_caw.addDiagramm(ImportDiagramm);
                    }

                    await progressDialog.CloseAsync();

                    propertyChanged("DiagrammsList");

                    flo_bottom.IsOpen = false;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Das PDF hat " + pdfReader.NumberOfPages + " Seiten und das DXF " + ImportDiagramm.pages_List.Count);
                }
            }
            catch
            {

            }
            finally
            {
                ImportDiagramm = null;
                dxf_parser = null;
                DXFDialog = null;
                PDFDialog = null;
            }

        }

        private void flo_import_tle_del_Click(object sender, RoutedEventArgs e)
        {
            DXFDialog = null;
            PDFDialog = null;
            ImportDiagramm = null;
        }
    }
}
