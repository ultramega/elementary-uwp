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
using elementary.Model;
using elementary.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Resources;

namespace elementary.ViewModel
{
    /// <summary>
    /// The ViewModel containing the full details of an element.
    /// </summary>
    public class ElementDetails : ElementBase, INotifyPropertyChanged
    {
        /// <summary>
        /// The localized string for unknown values.
        /// </summary>
        private readonly string _unknownValue = ResourceLoader.GetForCurrentView().GetString("Unknown");

        /// <summary>
        /// The name of the element.
        /// </summary>
        public string Name
        {
            get
            {
                return ElementUtilities.GetElementName(Element.Number);
            }
        }

        /// <summary>
        /// The element group, period, and block.
        /// </summary>
        public string GPB
        {
            get
            {
                var group = Element.Group == 0 ? "n/a" : Element.Group.ToString();
                return string.Format("{0}, {1}, {2}", group, Element.Period, Element.Block);
            }
        }

        /// <summary>
        /// The atomic weight of the element as a string.
        /// </summary>
        public string Weight
        {
            get
            {
                if (Element.Unstable)
                {
                    return string.Format("[{0:d}]", (long)Element.Weight);
                }
                return Element.Weight.ToString("0.########");
            }
        }

        /// <summary>
        /// The density of the element as a string.
        /// </summary>
        public string Density
        {
            get
            {
                if (Element.Density == null)
                {
                    return _unknownValue;
                }
                return string.Format("{0:0.########} g/cm³", Element.Density);
            }
        }

        /// <summary>
        /// The melting point of the element as a string.
        /// </summary>
        public string Melt
        {
            get
            {
                if (Element.Melt == null)
                {
                    return _unknownValue;
                }
                return GetTemperatureString((double)Element.Melt);
            }
        }

        /// <summary>
        /// The boiling point of the element as a string.
        /// </summary>
        public string Boil
        {
            get
            {
                if (Element.Boil == null)
                {
                    return _unknownValue;
                }
                return GetTemperatureString((double)Element.Boil);
            }
        }

        /// <summary>
        /// The heat of the element as a string.
        /// </summary>
        public string Heat
        {
            get
            {
                if (Element.Heat == null)
                {
                    return _unknownValue;
                }
                return string.Format("{0:0.########} J/g·K", Element.Heat);
            }
        }

        /// <summary>
        /// The electronegativity of the element as a string.
        /// </summary>
        public string Negativity
        {
            get
            {
                if (Element.Negativity == null)
                {
                    return _unknownValue;
                }
                return string.Format("{0:0.########} V", Element.Negativity);
            }
        }

        /// <summary>
        /// The abundance of the element as a string.
        /// </summary>
        public string Abundance
        {
            get
            {
                if (Element.Abundance == null)
                {
                    return _unknownValue;
                }
                if (Element.Abundance < 0.001)
                {
                    return "<0.001 mg/kg";
                }
                return string.Format("{0:0.########} mg/kg", Element.Abundance);
            }
        }

        /// <summary>
        /// The element category.
        /// </summary>
        public string Category
        {
            get
            {
                return ElementUtilities.Categories[Element.Category];
            }
        }

        /// <summary>
        /// The electron configuration of the element.
        /// </summary>
        public string Configuration
        {
            get
            {
                return Regex.Replace(Element.Configuration, "(?<=[spdf])([0-9]+)", delegate (Match match)
                {
                    var map = new Dictionary<char, char>()
                    {
                        { '0', '⁰' }, { '1', '¹' }, { '2', '²' }, { '3', '³' }, { '4', '⁴' },
                        { '5', '⁵' }, { '6', '⁶' }, { '7', '⁷' }, { '8', '⁸' }, { '9', '⁹' }
                    };
                    var ret = new StringBuilder();
                    foreach (var c in match.Value.ToCharArray())
                    {
                        ret.Append(map[c]);
                    }
                    return ret.ToString();
                });
            }
        }

        /// <summary>
        /// The electrons per shell of the element.
        /// </summary>
        public string Electrons
        {
            get
            {
                return Element.Electrons.Replace(",", ", ");
            }
        }

        /// <summary>
        /// The electrons per shell of the element in a vertical line.
        /// </summary>
        public string BlockElectrons
        {
            get
            {
                return Element.Electrons.Replace(",", "\n");
            }
        }

        /// <summary>
        /// The URL to the Wikipedia page for the element.
        /// </summary>
        public string Wiki
        {
            get
            {
                return ElementUtilities.GetElementWiki(Element.Number);
            }
        }

        /// <summary>
        /// The URL to the YouTube video associated with the element.
        /// </summary>
        public string Video
        {
            get
            {
                return string.Format("https://www.youtube.com/watch?v={0}", Element.Video);
            }
        }

        /// <summary>
        /// Occurs when a mutable property changes.
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="element">The Element to represent.</param>
        public ElementDetails(Element element) : base(element)
        {
            Settings.SettingChanged += OnSettingChanged;
        }

        /// <summary>
        /// Triggers the PropertyChanged event for the temperature field when the setting for
        /// temperature units is changed.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="value">The new value for the setting.</param>
        private void OnSettingChanged(Settings.Key key, object value)
        {
            if (key == Settings.Key.TemperatureUnits)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Melt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Boil"));
            }
        }

        /// <summary>
        /// Gets the temperature string converted from Kelvin to the unit set in the application
        /// settings.
        /// </summary>
        /// <param name="kelvinValue">The temperature value in Kelvin.</param>
        /// <returns>The converted temperature as a string with unit.</returns>
        private static string GetTemperatureString(double kelvinValue)
        {
            switch (Settings.TemperatureUnits)
            {
                case "C":
                    return string.Format("{0} °C", UnitUtilities.KelvinToCelsius(kelvinValue));
                case "F":
                    return string.Format("{0} °F", UnitUtilities.KelvinToFahrenheit(kelvinValue));
            }
            return string.Format("{0} K", kelvinValue);
        }
    }
}
