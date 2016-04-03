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
using elementary.Util;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace elementary.Control
{
    /// <summary>
    /// Control for displaying the legend for the element block colors.
    /// </summary>
    public sealed partial class PeriodicTableLegend : UserControl
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTableLegend()
        {
            InitializeComponent();
            LoadLegend();
            SizeChanged += OnSizeChanged;
            Settings.SettingChanged += OnSettingChanged;
        }

        /// <summary>
        /// Reloads the legend when the setting for element colors is changed.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="val">The new value for the setting.</param>
        private void OnSettingChanged(Settings.Key key, object val)
        {
            if (key == Settings.Key.ElementColors)
            {
                LoadLegend();
            }
        }

        /// <summary>
        /// Calculates and sets the FontSize when the size of the Control changes.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            FontSize = e.NewSize.Height / 9;
        }

        /// <summary>
        /// Loads the legend data.
        /// </summary>
        private void LoadLegend()
        {
            var g = Content as Grid;
            g.Children.Clear();
            string[] names;
            if (Settings.ElementColors == "cat")
            {
                names = ElementUtils.Categories;
            }
            else
            {
                names = new string[] { "s", "p", "d", "f" };
            }
            var colSpan = 4 - (int)Math.Ceiling(names.Length / 4d);
            for (var i = 0; i < names.Length; i++)
            {
                var item = MakeItem((long)i, names[i]);
                Grid.SetRow(item, i % 4);
                Grid.SetColumn(item, i / 4);
                Grid.SetColumnSpan(item, colSpan);
                g.Children.Add(item);
            }
        }

        /// <summary>
        /// Creates an item for the legend.
        /// </summary>
        /// <param name="key">The key for the color.</param>
        /// <param name="name">The name assigned to the color.</param>
        /// <returns>The item to place in the table.</returns>
        private FrameworkElement MakeItem(object key, string name)
        {
            var b = new Binding()
            {
                Mode = BindingMode.OneWay,
                Source = FontSize
            };

            var text = new TextBlock();
            text.Text = name;
            text.SetBinding(FontSizeProperty, b);
            text.VerticalAlignment = VerticalAlignment.Center;
            text.Margin = new Thickness(6, 0, 6, 0);
            text.Foreground = new SolidColorBrush(Colors.Black);

            var border = new Border();
            border.Background = ElementUtils.GetBlockColor(key);
            border.Margin = new Thickness(1);
            border.Child = text;

            return border;
        }
    }
}
