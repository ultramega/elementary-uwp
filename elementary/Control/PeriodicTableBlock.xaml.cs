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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace elementary.Control
{
    /// <summary>
    /// Displays a clickable block in the periodic table.
    /// </summary>
    public sealed partial class PeriodicTableBlock : UserControl
    {
        /// <summary>
        /// Occurs when a click event is detected on this block.
        /// </summary>
        public event RoutedEventHandler Clicked;

        /// <summary>
        /// Gets or sets the ElementBlock represented by this block.
        /// </summary>
        public ElementBlock Element
        {
            get { return (ElementBlock)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(ElementBlock), typeof(PeriodicTableBlock), new PropertyMetadata(0));

        /// <summary>
        /// The TextBlock to display the element number.
        /// </summary>
        private readonly TextBlock _txtNumber;

        /// <summary>
        /// The TextBlock to display the element symbol.
        /// </summary>
        private readonly TextBlock _txtSymbol;

        /// <summary>
        /// The TextBlock to display the subtext of the block.
        /// </summary>
        private readonly TextBlock _txtSubtext;

        /// <summary>
        /// The main element of this user control.
        /// </summary>
        private readonly Grid _content;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTableBlock()
        {
            InitializeComponent();
            _content = Content as Grid;
            _txtNumber = (TextBlock)_content.FindName("txtNumber");
            _txtSymbol = (TextBlock)_content.FindName("txtSymbol");
            _txtSubtext = (TextBlock)_content.FindName("txtSubtext");

            SizeChanged += OnSizeChanged;
            PointerPressed += OnDown;
        }

        /// <summary>
        /// Calculates and sets the properties of the Control when the size changes.
        /// </summary>
        /// <param name="sender">This PeriodicTableBlock.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var content = Content as FrameworkElement;
            var blockSize = Math.Min(content.ActualWidth, content.ActualHeight);

            _txtNumber.FontSize = blockSize / 4;
            _txtNumber.Margin = new Thickness(blockSize / 20, 0, 0, 0);

            _txtSymbol.FontSize = blockSize / 2;
            _txtSymbol.Margin = new Thickness(0, 0, 0, blockSize / 12);

            _txtSubtext.FontSize = blockSize / 5;
            _txtSubtext.Margin = new Thickness(0, 0, 0, blockSize / 20);
        }

        /// <summary>
        /// Activates the selection indicator when the pointer is pressed.
        /// </summary>
        /// <param name="sender">This PeriodicTableBlock.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDown(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "statePressed", true);
            PointerReleased += OnUp;
            PointerReleased += OnClicked;
            PointerExited += OnUp;
        }

        /// <summary>
        /// Deactivates the selection indicator when the pointer is released or moved out of the
        /// block.
        /// </summary>
        /// <param name="sender">This PeriodicTableBlock.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUp(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "stateNormal", true);
            PointerReleased -= OnUp;
            PointerReleased -= OnClicked;
            PointerExited -= OnUp;
        }

        /// <summary>
        /// Handler for the Clicked event.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnClicked(object sender, PointerRoutedEventArgs e)
        {
            Clicked(sender, new RoutedEventArgs());
        }
    }
}
