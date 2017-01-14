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
using System.ComponentModel;
using Windows.ApplicationModel.Resources;

namespace Elementary.ViewModels
{
    /// <summary>
    /// The ViewModel backing the ComboBox for selecting what to display on the bottom of each
    /// block of the Periodic Table.
    /// </summary>
    class SubtextValueViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The list of available options.
        /// </summary>
        private static string[] _items =
            ResourceLoader.GetForCurrentView().GetString("SubtextValues").Split(';');

        /// <summary>
        /// The keys associated with each option.
        /// </summary>
        private static string[] _keys = new string[]
        {
            "Weight",
            "Density",
            "Melt",
            "Boil",
            "Heat",
            "Negativity",
            "Abundance"
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
                var setting = Settings.SubtextValue;
                for (var i = 0; i < _keys.Length; i++)
                {
                    if (_keys[i] == setting)
                    {
                        return i;
                    }
                }
                return 0;
            }
            set
            {
                if (value < 0 || value >= _keys.Length)
                {
                    return;
                }
                Settings.SubtextValue = _keys[value];
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
        public SubtextValueViewModel()
        {
            UpdateTempUnit();
            Settings.SettingChanged += OnSettingChanged;
        }

        /// <summary>
        /// Updates the unit to display for temperatures.
        /// </summary>
        private void UpdateTempUnit()
        {
            string label;
            switch (Settings.TemperatureUnits)
            {
                case "C":
                    label = "℃";
                    break;
                case "F":
                    label = "℉";
                    break;
                default:
                    label = "K";
                    break;
            }
            _items[2] = _items[2].Substring(0, _items[2].Length - 2) + label + ')';
            _items[3] = _items[3].Substring(0, _items[3].Length - 2) + label + ')';
        }

        /// <summary>
        /// Updates the temperature units when the setting for temperature units is changed.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="value">The new value for the setting.</param>
        private void OnSettingChanged(Settings.Key key, object value)
        {
            if (key == Settings.Key.TemperatureUnits)
            {
                UpdateTempUnit();
                PropertyChanged(this, new PropertyChangedEventArgs(null));
            }
            else if (key == Settings.Key.SubtextValue)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedItemIndex"));
            }
        }
    }
}
