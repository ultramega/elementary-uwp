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
using Elementary.Model;
using Elementary.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;

namespace Elementary.ViewModels
{
    /// <summary>
    /// Base for all Element ViewModels.
    /// </summary>
    public abstract class ElementBaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a mutable property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Gets the Element being represented by this ViewModel.
        /// </summary>
        public Element Element { get; private set; }

        /// <summary>
        /// Gets the element number as a string.
        /// </summary>
        public string Number
        {
            get
            {
                return Element.Number.ToString();
            }
        }

        /// <summary>
        /// Gets the element symbol.
        /// </summary>
        public string Symbol
        {
            get
            {
                return Element.Symbol;
            }
        }

        /// <summary>
        /// Gets the background color of the block.
        /// </summary>
        public Brush Color
        {
            get
            {
                return ElementUtilities.GetBlockColor(Element);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="element">The Element to represent.</param>
        public ElementBaseViewModel(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            Element = element;
            Settings.SettingChanged += OnSettingChanged;
        }

        /// <summary>
        /// Triggers the PropertyChanged event for the indicated property.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Sets the Color when the setting for element colors is changed.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="value">The new value for the setting.</param>
        private void OnSettingChanged(Settings.Key key, object value)
        {
            if (key == Settings.Key.ElementColors)
            {
                RaisePropertyChanged("Color");
            }
        }
    }
}
