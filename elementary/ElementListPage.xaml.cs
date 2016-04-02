/*
  The MIT License (MIT)
  Copyright © 2016 Steve Guidetti

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the “Software”), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
*/
using elementary.Model;
using elementary.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace elementary
{
    /// <summary>
    /// Page that shows the elements in a sortable and filterable list.
    /// </summary>
    public sealed partial class ElementListPage : Page
    {
        /// <summary>
        /// The fields available to use to sort the list.
        /// </summary>
        private enum SortField
        {
            Number,
            Name
        }

        /// <summary>
        /// The full unsorted list of elements.
        /// </summary>
        private readonly ElementListItem[] _rawList;

        /// <summary>
        /// The data source for the ListView.
        /// </summary>
        private ObservableCollection<ElementListItem> ListItems { get; } = new ObservableCollection<ElementListItem>();

        /// <summary>
        /// The field being used for sorting.
        /// </summary>
        private SortField _sortField = SortField.Number;

        /// <summary>
        /// Whether the list is sorted in ascending order.
        /// </summary>
        private bool _sortAsc = true;

        /// <summary>
        /// The text to use to filter the list.
        /// </summary>
        private string _filter = "";

        /// <summary>
        /// The currently selected item in the list.
        /// </summary>
        private ElementListItem _selection;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ElementListPage()
        {
            InitializeComponent();
            _rawList = DBHelper.GetElementList();
        }

        /// <summary>
        /// Loads the list when the page is navigated to.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadList();
        }

        /// <summary>
        /// Loads the sorted and filtered list.
        /// </summary>
        private void LoadList()
        {
            var list = _rawList.Where(Matches);

            if (_sortField == SortField.Name)
            {
                list = list.OrderBy(e => e.Name);
            }
            else
            {
                list = list.OrderBy(e => e.Element.Number);
            }

            if (!_sortAsc)
            {
                list = list.Reverse();
            }

            ListItems.Clear();
            foreach (var e in list)
            {
                ListItems.Add(e);
                if (e == _selection)
                {
                    MasterList.SelectedItem = e;
                }
            }
        }

        /// <summary>
        /// Updates the layout when the visual state changes.
        /// </summary>
        /// <param name="sender">The VisualStateGroup.</param>
        /// <param name="e">The event arguments.</param>
        private void OnStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if(e.NewState == WideState)
            {
                MasterList.SelectedItem = _selection;
            }
            else if(_selection != null)
            {
                var element = DBHelper.GetElement(_selection.Element._ID);
                Frame.Navigate(typeof(ElementDetailsPage), element, new SuppressNavigationTransitionInfo());
            }
        }

        /// <summary>
        /// Determines whether an element matches the current filter. Returns true if the symbol or
        /// name begins with the filter text. Case insensitive.
        /// </summary>
        /// <param name="e">The ElementListItem to test.</param>
        /// <returns>Whether the element matches the filter.</returns>
        private bool Matches(ElementListItem e)
        {
            var symbol = e.Symbol.ToLower().StartsWith(_filter);
            var name = e.Name.ToLower().StartsWith(_filter);
            return symbol || name;
        }

        /// <summary>
        /// Sets the selected item when a list item is selected and loads the detail Frame. Ignores
        /// empty selections. Occurs only in the wide state.
        /// </summary>
        /// <param name="sender">The ListView.</param>
        /// <param name="e">The event arguments which includes the SelectedItem.</param>
        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (MasterList.SelectedItem != null)
            {
                _selection = MasterList.SelectedItem as ElementListItem;
                var element = DBHelper.GetElement(_selection.Element._ID);
                DetailsFrame.Navigate(typeof(ElementPage), element);
            }
        }

        /// <summary>
        /// Sets the selected item when a list item is clicked and navigates the main Frame to the
        /// details Page. Occurs only in the narrow state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            _selection = e.ClickedItem as ElementListItem;
            var element = DBHelper.GetElement(_selection.Element._ID);
            Frame.Navigate(typeof(ElementDetailsPage), element, new DrillInNavigationTransitionInfo());
        }

        /// <summary>
        /// Sets the sorting parameters when a sort button is clicked and reloads the list.
        /// </summary>
        /// <param name="sender">The ToggleMenuFlyoutItem.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSortButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender == SortNumber)
            {
                SortNumber.IsChecked = true;
                SortName.IsChecked = false;
                _sortAsc = _sortField == SortField.Number ? !_sortAsc : true;
                _sortField = SortField.Number;
            }
            else
            {
                SortNumber.IsChecked = false;
                SortName.IsChecked = true;
                _sortAsc = _sortField == SortField.Name ? !_sortAsc : true;
                _sortField = SortField.Name;
            }
            LoadList();
        }

        /// <summary>
        /// Sets the filter text when the input changes and reloads the list.
        /// </summary>
        /// <param name="sender">The filter TextBox.</param>
        /// <param name="e">The event arguments.</param>
        private void OnFilterChanged(object sender, TextChangedEventArgs e)
        {
            _filter = (sender as TextBox).Text.ToLower();
            LoadList();
        }
    }
}
