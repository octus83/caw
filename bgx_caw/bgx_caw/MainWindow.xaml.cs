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
        private Data data;
        public int MaxPageNumber
        {
            get
            {
                return this.maxPageNumber;
            }
            set
            {
                this.maxPageNumber = value;
            }

        }

        public String ID
        {
            get
            {
                return this._id;
            }
            set
            {
             this._id = value;
             this.data = new Data(value);
             maxPageNumber = data.getPageCout();
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

        private void Tile_Potential(object sender, RoutedEventArgs e)
        {
            buildPotentialFlyout(); 
        }
        private void Tile_Potential_Click(object sender, EventArgs e)
        {
           stack_right_sites.Children.Clear();
           MyTile tile = sender as MyTile;
           Potential p = (Potential)tile.Data;
           List<Page> list = new List<Page>();
           list = data.getPagenumbersFromPotentialNames(tile.Title);
           flo_right_info.IsOpen = false;
  
           flo_right_sites.IsOpen = true;
           foreach (var item in list)
           {
               MyTile t1 = new MyTile();
               t1.Title = item.PageInDiagramm.ToString()+" "+item.Title;
               t1.Data = item;
               t1.TiltFactor = 2;
               t1.Width = 150;
               t1.Height = 50;
               t1.Click += new RoutedEventHandler(Tile_Site_Click);
               stack_right_sites.Children.Add(t1);              
           }
        }

        private void Tile_Site_Click(object sender, EventArgs e)
        {    
            MyTile tile = sender as MyTile;
            Page p = (Page)tile.Data;    
            goToPage(p.PageInDiagramm);                      
        }

        private void buildPotentialFlyout()
        {
            stack_left_potential.Children.Clear();
            if (actualPageNumber > 0)
            {
                flo_left_potential.IsOpen = true;
              
                List<Potential> list = new List<Potential>();
                list = data.getPotentialFromPageNumber(actualPageNumber);
                foreach (var item in list)
                {
                    MyTile tile = new MyTile();
                    tile.Title = item.Name;
                    tile.Data = item;
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
            PageNumberLabel.Text = actualPageNumber.ToString();
            if (flo_left_potential.IsOpen)
            {
                buildPotentialFlyout();
            }
        }
    }
}
