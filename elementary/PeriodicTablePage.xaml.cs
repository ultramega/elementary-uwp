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
using elementary.Control;
using elementary.Model;
using elementary.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace elementary
{
    /// <summary>
    /// Page that displays the Periodic Table.
    /// </summary>
    public sealed partial class PeriodicTablePage : Page
    {
        /// <summary>
        /// The property determining the FontSize of the row and column headers.
        /// </summary>
        public double HeaderSize
        {
            get { return (double)GetValue(HeaderSizeProperty); }
            set { SetValue(HeaderSizeProperty, value); }
        }
        public static readonly DependencyProperty HeaderSizeProperty =
            DependencyProperty.Register("HeaderSize", typeof(double), typeof(PeriodicTablePage), new PropertyMetadata(1.0));

        /// <summary>
        /// The property determining the FontSize of the title.
        /// </summary>
        public double TitleSize
        {
            get { return (double)GetValue(TitleSizeProperty); }
            set { SetValue(TitleSizeProperty, value); }
        }
        public static readonly DependencyProperty TitleSizeProperty =
            DependencyProperty.Register("TitleSize", typeof(double), typeof(PeriodicTablePage), new PropertyMetadata(1.0));

        /// <summary>
        /// The property determining the distance between the table and the Lantha/Actinides.
        /// </summary>
        public double SpacerWidth
        {
            get { return (double)GetValue(SpacerWidthProperty); }
            set { SetValue(SpacerWidthProperty, value); }
        }
        public static readonly DependencyProperty SpacerWidthProperty =
            DependencyProperty.Register("SpacerWidth", typeof(double), typeof(PeriodicTablePage), new PropertyMetadata(1.0));

        /// <summary>
        /// The property determining the Margin around the row and column headers.
        /// </summary>
        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }
        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register("HeaderMargin", typeof(Thickness), typeof(PeriodicTablePage), null);

        /// <summary>
        /// The property determining the Margin around the color legend.
        /// </summary>
        public Thickness LegendMargin
        {
            get { return (Thickness)GetValue(LegendMarginProperty); }
            set { SetValue(LegendMarginProperty, value); }
        }
        public static readonly DependencyProperty LegendMarginProperty =
            DependencyProperty.Register("LegendMargin", typeof(Thickness), typeof(PeriodicTablePage), null);

        /// <summary>
        /// The reference to the Grid containing the Periodic Table.
        /// </summary>
        private readonly Grid _ptGrid;

        /// <summary>
        /// The reference to the ContentDialog for displaying the details of an element.
        /// </summary>
        private readonly ContentDialog _detailsDialog;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTablePage()
        {
            InitializeComponent();
            _ptGrid = FindName("ptGrid") as Grid;
            _detailsDialog = FindName("detailsDialog") as ContentDialog;
            SizeChanged += OnSizeChanged;
            LoadBlocks();
        }

        /// <summary>
        /// Calculates and sets the properties when the size of the Page changes.
        /// </summary>
        /// <param name="sender">The Page.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var blockSize = Math.Min(ActualWidth / 18, ActualHeight / 9);
            HeaderSize = blockSize / 4;
            TitleSize = blockSize / 2;
            SpacerWidth = blockSize / 4;
            HeaderMargin = new Thickness(0, 0, blockSize / 5, blockSize / 5);
            LegendMargin = new Thickness(blockSize / 3, blockSize * 0.75, blockSize / 3, blockSize / 4);
        }

        /// <summary>
        /// Loads the ElementBlocks into the Grid.
        /// </summary>
        private void LoadBlocks()
        {
            foreach (ElementBlock el in DBHelper.GetElementTable())
            {
                var block = new PeriodicTableBlock() { Element = el };
                block.Clicked += BlockClicked; ;
                Grid.SetRow(block, el.Row);
                Grid.SetColumn(block, el.Col);
                _ptGrid.Children.Add(block);
            }
        }

        /// <summary>
        /// Opens the ContentDialog to display the details of an element when it is selected.
        /// </summary>
        /// <param name="sender">The clicked PeriodicTableBlock.</param>
        /// <param name="e">The event arguments.</param>
        private async void BlockClicked(object sender, RoutedEventArgs e)
        {
            var id = (sender as PeriodicTableBlock).Element.Element._ID;
            (_detailsDialog.Content as Frame).Navigate(typeof(ElementDetailsPage), id);
            await _detailsDialog.ShowAsync();
        }
    }
}
