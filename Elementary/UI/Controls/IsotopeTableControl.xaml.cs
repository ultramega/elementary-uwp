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
using Elementary.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Elementary.UI.Controls
{
    public sealed partial class IsotopeTableControl : UserControl
    {
        /// <summary>
        /// Gets or sets the isotope data.
        /// </summary>
        public List<IsotopeViewModel> IsotopeList
        {
            get { return (List<IsotopeViewModel>)GetValue(IsotopeListProperty); }
            set { SetValue(IsotopeListProperty, value); }
        }
        public static readonly DependencyProperty IsotopeListProperty =
            DependencyProperty.Register("IsotopeList", typeof(List<IsotopeViewModel>),
                typeof(IsotopeTableControl), null);

        /// <summary>
        /// Constructor.
        /// </summary>
        public IsotopeTableControl()
        {
            InitializeComponent();
            RegisterPropertyChangedCallback(IsotopeListProperty, OnIsotopeListChanged);
        }

        /// <summary>
        /// Called when the IsotopeList property is changed.
        /// </summary>
        /// <param name="sender">This Control.</param>
        /// <param name="dp">The IsotopeListProperty.</param>
        private void OnIsotopeListChanged(DependencyObject sender, DependencyProperty dp)
        {
            var table = (Grid)Content;
            for (var i = table.Children.Count - 1; i > 2; i--)
            {
                table.Children.RemoveAt(i);
            }
            var row = 2;
            foreach (var item in IsotopeList)
            {
                table.RowDefinitions.Add(new RowDefinition());

                var symbol = new TextBlock();
                Grid.SetRow(symbol, row);
                symbol.HorizontalAlignment = HorizontalAlignment.Right;
                symbol.IsTextSelectionEnabled = true;
                symbol.Text = item.Symbol;

                var mass = new TextBlock();
                Grid.SetRow(mass, row++);
                Grid.SetColumn(mass, 2);
                mass.IsTextSelectionEnabled = true;
                mass.Text = item.Mass;

                table.Children.Add(symbol);
                table.Children.Add(mass);
            }
        }
    }
}
