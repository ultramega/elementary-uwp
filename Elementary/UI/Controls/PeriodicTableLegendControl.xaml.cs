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
using Elementary.Utilities;
using Elementary.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Elementary.UI.Controls
{
    /// <summary>
    /// Control for displaying the legend for the element block colors.
    /// </summary>
    public sealed partial class PeriodicTableLegendControl : UserControl
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTableLegendControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the legend items when the Control is loaded.
        /// </summary>
        /// <param name="sender">This Control.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadLegend();
            Settings.SettingChanged += OnSettingChanged;
        }

        /// <summary>
        /// Reloads the legend when the setting for element colors is changed.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="value">The new value for the setting.</param>
        private void OnSettingChanged(Settings.Key key, object value)
        {
            if (key == Settings.Key.ElementColors)
            {
                LoadLegend();
            }
        }

        /// <summary>
        /// Loads the legend data.
        /// </summary>
        private void LoadLegend()
        {
            var grid = Content as Grid;
            grid.Children.Clear();
            string[] names;
            var useCategory = Settings.ElementColors == "cat";
            if (useCategory)
            {
                names = ElementUtilities.Categories;
            }
            else
            {
                names = new string[] { "s", "p", "d", "f" };
            }
            var colSpan = 4 - (int)Math.Ceiling(names.Length / 4d);
            for (var i = 0; i < names.Length; i++)
            {
                var key = useCategory ? (long)i : names[i] as object;
                var item = new LegendItemViewModel()
                {
                    Text = names[i],
                    Background = ElementUtilities.GetBlockColor(key),
                    Row = i % 4,
                    Column = i / 4,
                    ColumnSpan = colSpan
                };
                grid.Children.Add(MakeItem(item));
            }
        }

        /// <summary>
        /// Creates an item for the legend.
        /// </summary>
        /// <param name="item">The data source for the item.</param>
        /// <returns>The element to place in the table.</returns>
        private FrameworkElement MakeItem(LegendItemViewModel item)
        {
            var element = LegendEntryTemplate.LoadContent() as FrameworkElement;
            element.DataContext = item;
            return element;
        }
    }
}
