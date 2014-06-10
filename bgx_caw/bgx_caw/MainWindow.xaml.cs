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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace bgx_caw
{
    public  enum State
    {
        None,
        ProjectSelected,
        NoProjectSelected
    }
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow :MetroWindow 
    {
        private String _id;
        private int actualPageNumber = 0;
        private int maxPageNumber = 15;
        private Data data;
        private List<BitmapImage> _images;
        private List<Part> _completedParts;
        private State _projectState;

      
        
        public State ProjectState
        {
            get
            {
                return this._projectState;
            }
            set
            {
                this._projectState = value;
            }
        }
        public List<BitmapImage> Images
        {
            get
        {
            return this._images;
        }
            set
            {
                this._images = value;
            }
        }
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
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            ProjectState = State.NoProjectSelected;
           
        }

        private void txtAuto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ProjectState == State.ProjectSelected)
            {
                if (_completedParts == null)
                {
                    _completedParts = data.getCompletePartList();
                }
                string typedString = txtAuto.Text;
                List<String> autoList = new List<String>();
                autoList.Clear();

                foreach (var item in _completedParts)
                {
                    if (!string.IsNullOrEmpty(txtAuto.Text))
                    {
                        if (item.BMK.StartsWith(typedString))
                        {
                            autoList.Add(item.BMK);
                           
                        }
                    }
                }
                if (autoList.Count > 0)
                {
                    lbSuggestion.ItemsSource = autoList;
                    lbSuggestion.Visibility = Visibility.Visible;
                }
                else if (txtAuto.Text.Equals(""))
                {
                    lbSuggestion.ItemsSource = null;
                    lbSuggestion.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lbSuggestion.ItemsSource = null;
                    lbSuggestion.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void lbSuggestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSuggestion.ItemsSource != null)
            {
                lbSuggestion.Visibility = Visibility.Collapsed;
                txtAuto.TextChanged -= new TextChangedEventHandler(txtAuto_TextChanged);
                if (lbSuggestion.SelectedIndex != -1)
                {
                    txtAuto.Text = lbSuggestion.SelectedItem.ToString();
                }
                txtAuto.TextChanged += new TextChangedEventHandler(txtAuto_TextChanged);
            }
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
            if (ProjectState == State.ProjectSelected)
            {
                flo_right_info.IsOpen = true;
            }
            else
            {
                MessageBox.Show("No Project selected");
            }

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

           closeAllRightFlyouts();
  
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

        private void Tile_Search(object sender, EventArgs e)
        {
            closeAllLeftFlyouts();
            flo_left_search.IsOpen = true;
        }

        private void buildPotentialFlyout()
        {
            stack_left_potential.Children.Clear();
            if (actualPageNumber > 0)
            {
                closeAllLeftFlyouts();
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
                updatePagenumberLabel(actualPageNumber.ToString());
                changePicture();
                checkWhichFlyoutIsOpen();
            }
        }

        private void previousPage()
        {
            if (actualPageNumber > 1)
            {
                actualPageNumber--;
                updatePagenumberLabel(actualPageNumber.ToString());
                changePicture();
                checkWhichFlyoutIsOpen();
            }
        }
        private void goToPage(int page)
        {
            if (page <= maxPageNumber && page > 0)
            {
                actualPageNumber = page;
                updatePagenumberLabel(actualPageNumber.ToString());
                changePicture();
                checkWhichFlyoutIsOpen();
            }
        }
        private void checkWhichFlyoutIsOpen()
        {
            if (flo_left_parts.IsOpen)
            {
                buildPartFlyout();
            }
            else if (flo_left_potential.IsOpen)
            {
                buildPotentialFlyout();
            }
        }
        private void updatePagenumberLabel(String update)
        {
            sitenumberLabel.Content = "Seite " + update + " von " + maxPageNumber;
        }
        private void changePicture()
        {
            if (actualPageNumber < _images.Count && _images != null)
            {
                showImage.Source = _images.ElementAt(actualPageNumber);
                showImage.RenderTransform = new ScaleTransform
                        (1, 1, 0.5, 0.5);
            }
            else
            {
                MessageBox.Show("no pictures found");
            }
        }
        public void onProjectOpen()
        {
            goToPage(1);
        }

        private void closeAllLeftFlyouts()
        {
            flo_left_potential.IsOpen = false;
            flo_left_search.IsOpen = false;
            flo_left_parts.IsOpen = false;
        }
        private void closeAllRightFlyouts()
        {
            flo_right_info.IsOpen = false;
            flo_right_sites.IsOpen = false;
        }
        private void Tile_Part(Object sender, EventArgs e)
        {
            buildPartFlyout();
        }

        private void buildPartFlyout(){
            stack_left_parts.Children.Clear();
            if (actualPageNumber > 0)
            {
                closeAllLeftFlyouts();
                flo_left_parts.IsOpen = true;
                List<Part> list = new List<Part>();
                list = data.getPartFomPageNumber(actualPageNumber);
               
                foreach (var item in list)
                {
                    MyTile tile = new MyTile();
                    tile.Title = item.BMK;
                    tile.Data = item;
                    tile.TiltFactor = 2;
                    tile.Width = 150;
                    tile.Height = 50;
                    tile.Click += new RoutedEventHandler(Tile_Part_Click);
                    stack_left_parts.Children.Add(tile);
                }
            }
        }
        private void Tile_Part_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Liste mit seiten");
        }
    }
}
