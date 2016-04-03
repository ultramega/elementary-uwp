using Windows.Foundation.Collections;
using Windows.Storage;

namespace elementary
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
            ElementColors
        }

        /// <summary>
        /// Occurs when an application setting changes.
        /// </summary>
        public static event OnSettingChangedHandler SettingChanged;

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
    }
}
