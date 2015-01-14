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
using System.IO;

namespace bgx_caw
{
    /// <summary>
    /// Klasse die ein Fenster öffnet um ein 
    /// Projekt/Diagramm auszuwählen
    /// </summary>
    public partial class OpenProjectWindow : Window
    {

        Logger logger;
        /// <summary>
        /// Liste aller gefunden Projekte/Diagramme der Datenbank
        /// </summary>
        private List<Diagramm> diagrammsList;
        /// <summary>
        /// Referenz auf das Hauptfenster
        /// </summary>
        private MainWindow caller;
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenProjectWindow()
        {
            InitializeComponent();
            refreshDiagrammList();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="caller"></param>
        public OpenProjectWindow(MainWindow caller)
        {
            logger = new Logger();
            InitializeComponent();
            refreshDiagrammList();
            this.caller = caller;
        }
        /// <summary>
        /// Erzeugt die Projekt/Diagramm Liste
        /// </summary>
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
        /// Erzeugt die Liste der Projekte/Diagramme
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
        /// <summary>
        /// Click Event dess Projekt/Diagramm Öffnen Buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenProject(object sender, RoutedEventArgs e)
        {
            if (projectList.SelectedIndex != -1)
            {
              
                if (caller.ProjectState == State.ProjectSelected)
                {
                    caller.cleanProject();
                    logger.log("old project cleared", "OpenProjectWindow.xaml.cs -> Button_OpenProject");
                }
                this.Close();
                logger.log("New Project opened", "OpenProjectWindow.xaml.cs -> Button_OpenProject");
                caller.DiagrammId = diagrammsList.ElementAt(projectList.SelectedIndex).ID;
                logger.log("with Diagramm ID: " + caller.DiagrammId, "OpenProjectWindow.xaml.cs -> Button_OpenProject");
                caller.onProjectOpen();
            }
        }
    
       
       
       

    }
}

