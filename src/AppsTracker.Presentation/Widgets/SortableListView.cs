﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppsTracker.Widgets
{
    public class SortableListView : ListView
    {
        private SortableGridViewColumn lastSortedColumn = null;
        private GridViewColumnHeader lastSortedColumnHeader = null;
        private ListSortDirection lastDirection = ListSortDirection.Ascending;

        public string ColumnHeaderAscendingTemplate
        {
            get { return (string)GetValue(ColumnHeaderAscendingTemplateProperty); }
            set { SetValue(ColumnHeaderAscendingTemplateProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderAscendingTemplateProperty =
            DependencyProperty.Register("ColumnHeaderAscendingTemplate", typeof(string), typeof(SortableListView), new UIPropertyMetadata(""));



        public string ColumnHeaderDescendingTemplate
        {
            get { return (string)GetValue(ColumnHeaderDescendingTemplateProperty); }
            set { SetValue(ColumnHeaderDescendingTemplateProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderDescendingTemplateProperty =
            DependencyProperty.Register("ColumnHeaderDescendingTemplate", typeof(string), typeof(SortableListView), new UIPropertyMetadata(""));



        public string ColumnHeaderNotSortedTemplate
        {
            get { return (string)GetValue(ColumnHeaderNotSortedTemplateProperty); }
            set { SetValue(ColumnHeaderNotSortedTemplateProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeaderNotSortedTemplateProperty =
            DependencyProperty.Register("ColumnHeaderNotSortedTemplate", typeof(string), typeof(SortableListView), new UIPropertyMetadata(""));

        protected override void OnInitialized(EventArgs e)
        {
            this.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClickedHandler));
            var gridView = this.View as GridView;

            if (gridView != null)
            {
                SortableGridViewColumn sortableGridViewColumn = null;
                foreach (var gridViewColumn in gridView.Columns)
                {
                    sortableGridViewColumn = gridViewColumn as SortableGridViewColumn;
                    if (sortableGridViewColumn != null)
                    {
                        if (sortableGridViewColumn.IsDefaultSortColumn)
                        {
                            break;
                        }
                        sortableGridViewColumn = null;
                    }
                }

                if (sortableGridViewColumn != null)
                {
                    lastSortedColumn = sortableGridViewColumn;
                    Sort(sortableGridViewColumn.SortPropertyName, ListSortDirection.Ascending);
                    this.SelectedIndex = 0;
                }
            }
            base.OnInitialized(e);
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                var sortableGridViewColumn = headerClicked.Column as SortableGridViewColumn;
                if (sortableGridViewColumn != null && !string.IsNullOrEmpty(sortableGridViewColumn.SortPropertyName))
                {
                    ListSortDirection direction;
                    if (lastSortedColumn == null
                 || String.IsNullOrEmpty(lastSortedColumn.SortPropertyName)
                 || !String.Equals(sortableGridViewColumn.SortPropertyName, lastSortedColumn.SortPropertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }
                    string sortPropertyName = sortableGridViewColumn.SortPropertyName;
                    Sort(sortPropertyName, direction);
                    lastSortedColumn = sortableGridViewColumn;
                    lastSortedColumnHeader = headerClicked;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            lastDirection = direction;
            var dataView = CollectionViewSource.GetDefaultView(this.ItemsSource);

            if (dataView == null || dataView.SortDescriptions == null)
                return;

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);

            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
    }
}
