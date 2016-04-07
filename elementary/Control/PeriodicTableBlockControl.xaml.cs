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
using elementary.ViewModel;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace elementary.Control
{
    /// <summary>
    /// Control for displaying an element block in the PeriodicTableControl.
    /// </summary>
    public sealed partial class PeriodicTableBlockControl : Button, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Gets or sets the element data.
        /// </summary>
        public ElementBlock Element
        {
            get { return (ElementBlock)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(ElementBlock), typeof(PeriodicTableBlockControl), null);

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
        public PeriodicTableBlockControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Calculates and sets the properties of the Control when the size changes.
        /// </summary>
        /// <param name="sender">This PeriodicTableBlock.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var blockSize = Math.Min(e.NewSize.Width, e.NewSize.Height);

            NumberFontSize = blockSize / 4.5;
            SymbolFontSize = blockSize / 2.2;
            SubtextFontSize = blockSize / 5;

            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }
    }
}
