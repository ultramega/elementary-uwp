﻿/*
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
using Elementary.Model;
using Elementary.ViewModels;
using System;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading;

namespace Elementary.UI.Controls
{
    /// <summary>
    /// Control for displaying the Periodic Table.
    /// </summary>
    public sealed partial class PeriodicTableControl : UserControl, INotifyPropertyChanged,
        IDisposable
    {
        /// <summary>
        /// The Timer for delaying resizing operations.
        /// </summary>
        private readonly Timer _resizeTimer;

        /// <summary>
        /// The AsyncOperation for calling methods on the main thread.
        /// </summary>
        private readonly AsyncOperation _async;

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
        /// Gets or sets the FontSize of the legend.
        /// </summary>
        private double LegendFontSize { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the FontSize of the atomic number.
        /// </summary>
        private double NumberFontSize { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the FontSize of the element symbol.
        /// </summary>
        private double SymbolFontSize { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the FontSize of the block subtext.
        /// </summary>
        private double SubtextFontSize { get; set; } = 1.0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTableControl()
        {
            InitializeComponent();
            _async = AsyncOperationManager.CreateOperation(null);
            _resizeTimer = new Timer(e =>
              {
                  _async.Post(d =>
                  {
                      CalculateSizes();
                  }, null);
              }, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Loads the blocks into the Grid when the Control is loaded.
        /// </summary>
        /// <param name="sender">This UserControl.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CalculateSizes();
            var children = (Content as Panel).Children;
            foreach (var item in Elements.List)
            {
                var element = BlockTemplate.LoadContent() as PeriodicTableBlockControl;
                element.Element = new ElementBlockViewModel(item);
                element.NumberFontSize = NumberFontSize;
                element.SymbolFontSize = SymbolFontSize;
                element.SubtextFontSize = SubtextFontSize;
                children.Add(element);
            }

            Window.Current.SizeChanged += OnWindowSizeChanged;
        }

        /// <summary>
        /// Remove the Window resize event handler when the Control is unloaded.
        /// </summary>
        /// <param name="sender">This UserControl.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= OnWindowSizeChanged;
            _resizeTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Recalculates the sizes after a delay when the window is resized.
        /// </summary>
        /// <param name="sender">The Window.</param>
        /// <param name="e">The event arguments.</param>
        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            _resizeTimer.Change(200, Timeout.Infinite);
        }

        /// <summary>
        /// Calculates and sets the properties of the Control when the size changes.
        /// </summary>
        private void CalculateSizes()
        {
            var blockSize = Math.Min((Parent as FrameworkElement).ActualWidth / 18.3,
                (Parent as FrameworkElement).ActualHeight / 9.9);

            HeaderFontSize = blockSize / 4;
            TitleFontSize = blockSize / 2;
            BlockSize = new GridLength(blockSize);
            HeaderWidth = new GridLength(blockSize / 3);
            SpacerWidth = new GridLength(blockSize / 4);
            LegendMargin =
                new Thickness(blockSize / 3, blockSize * 0.90, blockSize / 3, blockSize / 4);

            NumberFontSize = blockSize / 4.5;
            SymbolFontSize = blockSize / 2.2;
            SubtextFontSize = blockSize / 5;
            LegendFontSize = blockSize / 4;

            Width = blockSize * 18.3;
            Height = blockSize * 9.9;

            PropertyChanged(this, new PropertyChangedEventArgs(null));
            foreach (object block in (Content as Panel).Children)
            {
                if (block is PeriodicTableBlockControl)
                {
                    (block as PeriodicTableBlockControl).NumberFontSize = NumberFontSize;
                    (block as PeriodicTableBlockControl).SymbolFontSize = SymbolFontSize;
                    (block as PeriodicTableBlockControl).SubtextFontSize = SubtextFontSize;
                }
            }
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

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        public void Dispose()
        {
            _resizeTimer.Dispose();
        }
    }
}
