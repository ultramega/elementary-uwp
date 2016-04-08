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
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace elementary.Control
{
    /// <summary>
    /// Control for displaying the Periodic Table.
    /// </summary>
    public sealed partial class PeriodicTableControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when an element block in the table is clicked.
        /// </summary>
        public event BlockClickEventHandler BlockClick = delegate { };

        /// <summary>
        /// Handler for the BlockClick event.
        /// </summary>
        /// <param name="sender">The PeriodicTableControl that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void BlockClickEventHandler(object sender, BlockClickEventArgs e);

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Gets or sets the width and height of the block cells.
        /// </summary>
        private GridLength BlockSize { get; set; }

        /// <summary>
        /// Gets or sets the width of the row and column headers.
        /// </summary>
        private GridLength HeaderWidth { get; set; }

        /// <summary>
        /// Gets or sets the distance between the table and the Lantha/Actinides.
        /// </summary>
        private GridLength SpacerWidth { get; set; }

        /// <summary>
        /// Gets or sets the FontSize of the row and column headers.
        /// </summary>
        private double HeaderFontSize { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the FontSize of the title.
        /// </summary>
        private double TitleFontSize { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the Margin around the color legend.
        /// </summary>
        private Thickness LegendMargin { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTableControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the blocks into the Grid.
        /// </summary>
        /// <param name="sender">This UserControl.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var children = (Content as Panel).Children;
            foreach (var block in DBHelper.GetElementTable())
            {
                var element = BlockTemplate.LoadContent() as PeriodicTableBlockControl;
                element.Element = block;
                children.Add(element);
            }
        }

        /// <summary>
        /// Calculates and sets the properties of the Control when the size changes.
        /// </summary>
        /// <param name="sender">This PeriodicTableBlock.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var blockSize = Math.Min(e.NewSize.Width / 18.3, e.NewSize.Height / 9.9);

            HeaderFontSize = blockSize / 4;
            TitleFontSize = blockSize / 2;
            BlockSize = new GridLength(blockSize);
            HeaderWidth = new GridLength(blockSize / 3);
            SpacerWidth = new GridLength(blockSize / 4);
            LegendMargin = new Thickness(blockSize / 3, blockSize * 0.90, blockSize / 3, blockSize / 4);

            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        /// <summary>
        /// Raises an BlockClick event when an item is clicked.
        /// </summary>
        /// <param name="sender">The clicked item.</param>
        /// <param name="e">The event arguments.</param>
        private void OnItemClicked(object sender, RoutedEventArgs e)
        {
            BlockClick(this, new BlockClickEventArgs((sender as FrameworkElement).Tag as Element));
        }
    }
}
