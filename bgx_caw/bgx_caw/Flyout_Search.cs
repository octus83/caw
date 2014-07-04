using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Globalization;

namespace bgx_caw
{/// <summary>
    /// Logik des Suchen Flyouts
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Liste mit allen Bauteilen,Potentialname und Seitentitel eines Projektes/Diagramms
        /// </summary>
        private List<String> completeSearchSuggestionList;
        /// <summary>
        /// Click Event des Suchen Tile aus dem Info Flyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Search(object sender, EventArgs e)
        {
            closeAllRightFlyouts();
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
                if (completeSearchSuggestionList == null)
                {
                    completeSearchSuggestionList = data.generateCompleteSearchSuggestionList();
                }
                string typedString = txtAuto.Text;
                List<String> autoList = new List<String>();
                autoList.Clear();

                foreach (var item in completeSearchSuggestionList)
                {
                    if (!string.IsNullOrEmpty(txtAuto.Text))
                    {
                        if (item.StartsWith(typedString,true,null))
                        {
                            autoList.Add(item);

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
            list.AddRange(data.getPagenumbersFromPotentialNames(txtAuto.Text));
            list.AddRange(data.getPagenumbersFromSiteTitle(txtAuto.Text));
            buildPagenumberFlyout(list);         
        }
    }
}
