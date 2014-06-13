using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

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
    }
}
