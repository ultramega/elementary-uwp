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
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Elementary
{
    /// <summary>
    /// The settings for the application.
    /// </summary>
    sealed class Settings
    {
        /// <summary>
        /// The keys to identify the available settings.
        /// </summary>
        public enum Key
        {
            DarkTheme,
            TemperatureUnits,
            ElementColors,
            SubtextValue
        }

        /// <summary>
        /// Occurs when an application setting changes.
        /// </summary>
        public static event OnSettingChangedHandler SettingChanged = delegate { };

        /// <summary>
        /// Event handler for when an application setting changes.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="value">The new value for the setting.</param>
        public delegate void OnSettingChangedHandler(Key key, object value);

        /// <summary>
        /// Reference to the values of the RoamingSettings.
        /// </summary>
        private static IPropertySet _settings = ApplicationData.Current.RoamingSettings.Values;

        /// <summary>
        /// Constructor.
        /// </summary>
        static Settings()
        {
            ApplicationData.Current.DataChanged += OnDataChanged;
        }

        /// <summary>
        /// Notifies listeners that the settings may have changed.
        /// </summary>
        /// <param name="sender">The ApplicationData.</param>
        /// <param name="args">The event arguments.</param>
        private static void OnDataChanged(ApplicationData sender, object args)
        {
            SettingChanged(Key.DarkTheme, DarkTheme);
            SettingChanged(Key.TemperatureUnits, TemperatureUnits);
            SettingChanged(Key.ElementColors, ElementColors);
            SettingChanged(Key.SubtextValue, SubtextValue);
        }

        /// <summary>
        /// Gets or sets the value of the setting for whether to use the dark theme.
        /// </summary>
        public static bool DarkTheme
        {
            get
            {
                var ret = _settings["darkTheme"];
                return ret != null ? (bool)ret : false;
            }
            set
            {
                _settings["darkTheme"] = value;
                SettingChanged(Key.DarkTheme, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the setting for the unit to use for displaying
        /// temperatures.
        /// </summary>
        public static string TemperatureUnits
        {
            get
            {
                var ret = _settings["temperatureUnits"] as string;
                return ret != null ? ret : "K";
            }
            set
            {
                _settings["temperatureUnits"] = value;
                SettingChanged(Key.TemperatureUnits, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the setting for the property to use for determining the
        /// color of the element blocks.
        /// </summary>
        public static string ElementColors
        {
            get
            {
                var ret = _settings["elementColors"] as string;
                return ret != null ? ret : "cat";
            }
            set
            {
                _settings["elementColors"] = value;
                SettingChanged(Key.ElementColors, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the setting for the property to display on the bottom of each
        /// block.
        /// </summary>
        public static string SubtextValue
        {
            get
            {
                var ret = _settings["subtextValue"] as string;
                return ret != null ? ret : "Weight";
            }
            set
            {
                _settings["subtextValue"] = value;
                SettingChanged(Key.SubtextValue, value);
            }
        }
    }
}
