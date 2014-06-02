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

namespace bgx_caw
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow :MetroWindow
    {
        private String _id;
        private int actualPageNumber = 0;
        private int maxPageNumber = 15;

        public String ID
        {
            get
            {
                return this._id;
            }
            set
            {
             this._id = value;
             MainLabel.Text = value;  
            }
        }
        public MainWindow()
        {
            InitializeComponent();           
        }

        private void Button_open_project(object sender, RoutedEventArgs e)
        {
            OpenProjectWindow opw = new OpenProjectWindow(this);
            opw.ShowDialog();
        }

        private void Button_next_page(object sender, RoutedEventArgs e)
        {
            nextPage();
        }

        private void Button_previous_page(object sender, RoutedEventArgs e)
        {
            previousPage();
        }

        private void Button_open_info(object sender, RoutedEventArgs e)
        {
            flo_right_info.IsOpen = true;
        }

        private void Tile_Potentiale(object sender, RoutedEventArgs e)
        {
            buildPotentialFlyout(); 
        }
        private void Tile_Potential_Click(object sender, EventArgs e)
        {
            stack_right_sites.Children.Clear();
            Tile tile = sender as Tile;
            DB_CAW t = new DB_CAW();
            List<String> list = new List<String>();
           list= t.makeQuery(Query.getPotentialPagenumbersFromPotentialNames(tile.Title, this.ID));
           flo_right_sites.IsOpen = true;
           foreach (var item in list)
           {
               Tile t1 = new Tile();
               t1.Title = item;
               t1.TiltFactor = 2;
               t1.Width = 150;
               t1.Height = 50;
               tile.Click += new RoutedEventHandler(Tile_Site_Click);
               stack_right_sites.Children.Add(t1);
               

           }
        }

        private void Tile_Site_Click(object sender, EventArgs e)
        {
            Tile tile = sender as Tile;
            try
            {
              goToPage(Convert.ToInt32(tile.Title)); 
            }
            catch (Exception)
            {               
                throw;
            }
        }

        private void buildPotentialFlyout()
        {
            stack_left_potential.Children.Clear();
            if (actualPageNumber > 0)
            {
                flo_left_potential.IsOpen = true;
                DB_CAW t = new DB_CAW();
                List<String> list = new List<String>();
                list = t.makeQuery(Query.getPotentialNamesFromPageNumber(actualPageNumber, this.ID));
                foreach (var item in list)
                {
                    Tile tile = new Tile();
                    tile.Title = item;
                    tile.TiltFactor = 2;
                    tile.Width = 150;
                    tile.Height = 50;
                    tile.Click += new RoutedEventHandler(Tile_Potential_Click);
                    stack_left_potential.Children.Add(tile);

                }
            }
        }

        private void buildSiteFylout()
        {
            stack_right_sites.Children.Clear();

        }

        private void nextPage()
        {
            if (actualPageNumber <= maxPageNumber)
            {
                actualPageNumber++;
                PageNumberLabel.Text = actualPageNumber.ToString();
                if (flo_left_potential.IsOpen)
                {
                    buildPotentialFlyout();
                }
            }
        }

        private void previousPage()
        {
            if (actualPageNumber > 1)
            {
                actualPageNumber--;
                PageNumberLabel.Text = actualPageNumber.ToString();
                if (flo_left_potential.IsOpen)
                {
                    buildPotentialFlyout();
                }
            }
        }
        private void goToPage(int page)
        {
            actualPageNumber = page;
        }
    }
}
