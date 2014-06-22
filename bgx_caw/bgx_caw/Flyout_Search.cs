using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace bgx_caw
{
    public partial class MainWindow
    {
        private List<Part> _completedParts; 

        private void Tile_Search(object sender, EventArgs e)
        {
            closeAllLeftFlyouts();
            flo_left_search.IsOpen = true;
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
        private void rec_magni_MouseLeftButtonDown(Object sender, MouseEventArgs e)
        {
            stack_right_sites.Children.Clear();
            List<Page> list = new List<Page>();
            list = data.getPagenumbersFromPartlNames(txtAuto.Text);

            closeAllRightFlyouts();

            flo_right_sites.IsOpen = true;
            foreach (var item in list)
            {
                int page = (item.PageInDiagramm + 1);
                MyTile t1 = new MyTile();
                t1.TitleFontSize = 15;
                t1.Title = page.ToString() + " " + item.Title;
                t1.Data = item;
                t1.TiltFactor = 2;
                t1.Width = 150;
                t1.Height = 50;
                t1.Click += new RoutedEventHandler(Tile_Site_Click);
                stack_right_sites.Children.Add(t1);
            }
        }
    }
}
