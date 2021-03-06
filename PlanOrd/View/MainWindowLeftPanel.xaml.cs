﻿using System.Windows.Controls;

namespace PlanOrd.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindowLeftPanel.xaml
    /// </summary>
    public partial class MainWindowLeftPanel : DockPanel
    {
        public MainWindowLeftPanel()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listB = sender as ListBox;
            if (listB == null || e.AddedItems.Count == 0)
                return;
            listB.SelectedItem = e.AddedItems[0];
        }
    }
}
