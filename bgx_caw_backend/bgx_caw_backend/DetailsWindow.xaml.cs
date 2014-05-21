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
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace bgx_caw_backend
{
    /// <summary>
    /// Interaktionslogik für DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : MetroWindow
    {
        public DetailsWindow()
        {
            InitializeComponent();
        }

        public DetailsWindow(String id)
        {
            InitializeComponent();

            using (DB_CAW db_caw = new DB_CAW())
            {
                Diagramm diagramm = db_caw.getDiagramm(id);

                lbl_ID.Content = diagramm.ID;
                lbl_SerialNumber.Content = diagramm.SerialNumber;
                lbl_JobNumber.Content = diagramm.JobNumber;
                lbl_Projectnumber.Content = diagramm.ProjectNumber;
                lbl_ProjectName.Content = diagramm.ProjectName;
                lbl_Date_Init.Content = diagramm.Date_Init.ToString();
                lbl_Date_LastChange.Content = diagramm.Date_LastChange.ToString();
                lbl_IsActive.Content = diagramm.IsActive;
                lbl_Customer.Content = diagramm.Customer;
                lbl_EndCustomer.Content = diagramm.Endcustomer;
                lbl_Adress1.Content = diagramm.SiteRow1;
                lbl_Adress2.Content = diagramm.SiteRow2;
                lbl_Adress3.Content = diagramm.SiteRow3;
                lbl_SourceFolder.Content = diagramm.SourceFolder;
            }
        }
    }
}
