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
using Elementary.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Elementary.UI.Controls
{
    /// <summary>
    /// Control for displaying an element block in the PeriodicTableControl.
    /// </summary>
    public sealed partial class PeriodicTableBlockControl : Button
    {
        /// <summary>
        /// Gets or sets the element data.
        /// </summary>
        public ElementBlockViewModel Element
        {
            get { return (ElementBlockViewModel)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(ElementBlockViewModel),
                typeof(PeriodicTableBlockControl), null);

        /// <summary>
        /// Gets or sets the FontSize of the atomic number.
        /// </summary>
        public double NumberFontSize
        {
            get { return (double)GetValue(NumberFontSizeProperty); }
            set { SetValue(NumberFontSizeProperty, value); }
        }
        public static readonly DependencyProperty NumberFontSizeProperty =
            DependencyProperty.Register("NumberFontSize", typeof(double),
                typeof(PeriodicTableBlockControl), new PropertyMetadata(13.0));

        /// <summary>
        /// Gets or sets the FontSize of the element symbol.
        /// </summary>
        public double SymbolFontSize
        {
            get { return (double)GetValue(SymbolFontSizeProperty); }
            set { SetValue(SymbolFontSizeProperty, value); }
        }
        public static readonly DependencyProperty SymbolFontSizeProperty =
            DependencyProperty.Register("SymbolFontSize", typeof(double),
                typeof(PeriodicTableBlockControl), new PropertyMetadata(27.0));

        /// <summary>
        /// Gets or sets the FontSize of the block subtext.
        /// </summary>
        public double SubtextFontSize
        {
            get { return (double)GetValue(SubtextFontSizeProperty); }
            set { SetValue(SubtextFontSizeProperty, value); }
        }
        public static readonly DependencyProperty SubtextFontSizeProperty =
            DependencyProperty.Register("SubtextFontSize", typeof(double),
                typeof(PeriodicTableBlockControl), new PropertyMetadata(12.0));

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTableBlockControl()
        {
            InitializeComponent();
        }
    }
}
