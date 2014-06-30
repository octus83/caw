using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace bgx_caw
{/// <summary>
    /// Logik des Suchen Flyouts
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Liste mit allen Bauteilen eines Projektes/Diagramms
        /// </summary>
        private List<Part> _completedParts;
        /// <summary>
        /// Click Event des Suchen Tile aus dem Info Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Search(object sender, EventArgs e)
        {
            closeAllLeftFlyouts();
            flo_left_search.IsOpen = true;
            txtAuto.Focus();
        }
        /// <summary>
        /// Event bei Änderung des Suchfeldes im suchen Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Event für die Auswahl eines Elemtes
        /// der vorschlags such elemte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Löst das Suchevent aus welches ein Seiten Flyout öffnen
        /// und alle Seiten anzeigt auf die die suche zutrifft.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Search_GO_Click(object sender, RoutedEventArgs e)
        {
            List<Page> list = new List<Page>();
            list = data.getPagenumbersFromPartlNames(txtAuto.Text);
            buildPagenumberFlyout(list);         
        }
    }
}
