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
using System.ComponentModel;
using Windows.ApplicationModel.Resources;

namespace Elementary.ViewModels
{
    /// <summary>
    /// The ViewModel backing the block color ComboBox.
    /// </summary>
    class BlockColorViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The reference to the ResourceLoader for the Elements resources.
        /// </summary>
        private static readonly ResourceLoader _resources = ResourceLoader.GetForCurrentView();

        /// <summary>
        /// The list of available options.
        /// </summary>
        private static string[] _items = new string[]
        {
            _resources.GetString("Settings/ElementColors/Category/Content"),
            _resources.GetString("Settings/ElementColors/Block/Content")
        };

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        public int SelectedItemIndex
        {
            get
            {
                return Settings.ElementColors == "cat" ? 0 : 1;
            }
            set
            {
                Settings.ElementColors = value == 0 ? "cat" : "block";
            }
        }

        /// <summary>
        /// Gets the list of items.
        /// </summary>
        public string[] Items
        {
            get
            {
                return _items;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BlockColorViewModel()
        {
            Settings.SettingChanged += OnSettingChanged;
        }

        /// <summary>
        /// Updates the selected item when the setting for element colors is changed.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="value">The new value for the setting.</param>
        private void OnSettingChanged(Settings.Key key, object value)
        {
            if (key == Settings.Key.ElementColors)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(null));
            }
        }
    }
}

