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
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;

namespace Elementary.ViewModels
{
    /// <summary>
    /// The ViewModel containing the full details of an element.
    /// </summary>
    public class ElementDetailsViewModel : ElementBaseViewModel
    {
        /// <summary>
        /// The localized string for unknown values.
        /// </summary>
        private readonly string _unknownValue =
            ResourceLoader.GetForCurrentView().GetString("Unknown");

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        public string Name
        {
            get
            {
                return ElementUtilities.GetElementName(Element.Number);
            }
        }

        /// <summary>
        /// Gets the element group, period, and block.
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
        /// Gets the atomic weight of the element as a string.
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
        /// Gets the density of the element as a string.
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
        /// Gets the melting point of the element as a string.
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
        /// Gets the boiling point of the element as a string.
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
        /// Gets the heat of the element as a string.
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
        /// Gets the electronegativity of the element as a string.
        /// </summary>
        public string Negativity
        {
            get
            {
                if (Element.Negativity == null)
                {
                    return _unknownValue;
                }
                return Element.Negativity.Value.ToString("0.########");
            }
        }

        /// <summary>
        /// Gets the abundance of the element as a string.
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
        /// Gets the element category.
        /// </summary>
        public string Category
        {
            get
            {
                return ElementUtilities.Categories[Element.Category];
            }
        }

        /// <summary>
        /// Gets the electron configuration of the element.
        /// </summary>
        public string Configuration
        {
            get
            {
                var super = new char[] { '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹' };
                var ret = new StringBuilder();
                if (Element.Configuration.BaseElement != null)
                {
                    ret.Append('[').Append(Element.Configuration.BaseElement).Append("] ");
                }
                foreach (var o in Element.Configuration.Orbitals)
                {
                    ret.Append(o.Shell).Append(o.Type);
                    foreach (var d in o.Electrons.ToString().ToCharArray())
                    {
                        ret.Append(super[int.Parse(d.ToString())]);
                    }
                    ret.Append(' ');
                }
                ret.Remove(ret.Length - 1, 1);
                return ret.ToString();
            }
        }

        /// <summary>
        /// Gets the electrons per shell of the element.
        /// </summary>
        public string Electrons
        {
            get
            {
                return string.Join(", ", Element.Electrons);
            }
        }

        /// <summary>
        /// Gets the electrons per shell of the element in a vertical line.
        /// </summary>
        public string BlockElectrons
        {
            get
            {
                return string.Join("\n", Element.Electrons);
            }
        }

        /// <summary>
        /// Gets the list of common isotopes.
        /// </summary>
        public List<IsotopeViewModel> IsotopeList
        {
            get
            {
                var ret = new List<IsotopeViewModel>();
                foreach(var isotope in Isotopes.List[Element.Number])
                {
                    ret.Add(new IsotopeViewModel(isotope));
                }
                return ret;
            }
        }

        /// <summary>
        /// Gets the Uri to the Wikipedia page for the element.
        /// </summary>
        public Uri Wiki
        {
            get
            {
                return new Uri(ElementUtilities.GetElementWiki(Element.Number));
            }
        }

        /// <summary>
        /// Gets the Uri to the YouTube video associated with the element.
        /// </summary>
        public Uri Video
        {
            get
            {
                return new Uri(string.Format("https://www.youtube.com/watch?v={0}",
                    Element.Video));
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="element">The Element to represent.</param>
        public ElementDetailsViewModel(Element element) : base(element)
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
                RaisePropertyChanged("Melt");
                RaisePropertyChanged("Boil");
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
