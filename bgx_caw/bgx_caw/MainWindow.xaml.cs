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

            //Test
        }

        private void Button_open_project(object sender, RoutedEventArgs e)
        {
            OpenProjectWindow opw = new OpenProjectWindow(this);
            opw.ShowDialog();
        }

        private void Button_next_page(object sender, RoutedEventArgs e)
        {
            if (actualPageNumber <= maxPageNumber)
            {
                actualPageNumber++;
                PageNumberLabel.Text = actualPageNumber.ToString();
            }
        }

        private void Button_previous_page(object sender, RoutedEventArgs e)
        {
            if (actualPageNumber > 1)
            {
                actualPageNumber--;
                PageNumberLabel.Text = actualPageNumber.ToString();
            }
        }

        private void Button_open_info(object sender, RoutedEventArgs e)
        {
            flo_right.IsOpen = true;
        }

        private void Tile_Potentiale(object sender, RoutedEventArgs e)
        {
            flo_left_pot.IsOpen = true;
            DB_CAW t = new DB_CAW();
            List<String> list = new List<String>();
           list = t.makeQuery(Query.getPotentialNamesFromPageNumber(actualPageNumber));


            foreach (var item in list)
            {         
                Tile tile = new Tile();
                tile.Title = item;
                tile.TiltFactor=2;
                tile.Width = 150;
                tile.Height = 50;
                tile.Click += new RoutedEventHandler(Tile_Click);
                fly_left_pot_stackpanel.Children.Add(tile);

            }
           
        }
        private void Tile_Click(object sender, EventArgs e)
        {
            Tile tile = sender as Tile;
            DB_CAW t = new DB_CAW();
            List<String> list = new List<String>();
           list= t.makeQuery(Query.getPotentialPagenumbersFromPotentialNames(tile.Title));

           foreach (var item in list)
           {
               MessageBox.Show(item);
           }

        }
    }
}
