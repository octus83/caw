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
using MahApps.Metro.Controls;

namespace bgx_caw
{
    /// <summary>
    /// Interaction logic for OpenProjectWindow.xaml
    /// </summary>
    public partial class OpenProjectWindow : Window
    {
        private List<Diagramm> diagrammsList;
        private MainWindow caller;
        public OpenProjectWindow()
        {
            InitializeComponent();
            refreshDiagrammList();
        }
        public OpenProjectWindow(MainWindow caller)
        {
            InitializeComponent();
            refreshDiagrammList();
            this.caller = caller;
        }

        private void refreshDiagrammList()
        {
            try
            {
                using (DB_CAW db_caw = new DB_CAW())
                {
                    diagrammsList = db_caw.getDiagramms();
                    projectList.ItemsSource = buildListBoxList(diagrammsList);
                    
               
                }
            }
            catch (Exception exc)
            {
                this.Close();
                System.Windows.MessageBox.Show(exc.Message + exc.StackTrace);
            }
        }
        /// <summary>
        /// produces the List for the ListBox out of the diagrammList
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<String> buildListBoxList(List<Diagramm> list)
        {
            List<String> listBoxString = new List<String>();
            foreach (var item in list)
            {
                listBoxString.Add(item.ProjectName+" "+item.ProjectNumber);
            }
            return listBoxString;
        }

        private void Button_OpenProject(object sender, RoutedEventArgs e)
        {
            if (projectList.SelectedIndex != -1)
            {
                int selectedIndex = projectList.SelectedIndex;
                caller.ID = diagrammsList.ElementAt(selectedIndex).ID;
                this.Close();
            }
        }
    }
}
