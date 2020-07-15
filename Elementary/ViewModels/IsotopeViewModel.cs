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
using Windows.ApplicationModel.Resources;

namespace Elementary.ViewModels
{
    /// <summary>
    /// Contains data backing an isotope list item.
    /// </summary>
    public class IsotopeViewModel
    {
        /// <summary>
        /// The localized string for unknown values.
        /// </summary>
        private readonly string _unknownValue =
            ResourceLoader.GetForCurrentView().GetString("Unknown");

        /// <summary>
        /// Gets the Isotope being represented by this ViewModel.
        /// </summary>
        public Isotope Isotope { get; private set; }

        /// <summary>
        /// Gets the isotope symbol as a string.
        /// </summary>
        public string Symbol
        {
            get
            {
                return Isotope.Symbol;
            }
        }

        /// <summary>
        /// Gets the isotope mass as a string.
        /// </summary>
        public string Mass
        {
            get
            {
                return Isotope.Mass.ToString("0.########");
            }
        }

        /// <summary>
        /// Gets the isotopic composition as a string.
        /// </summary>
        public string Ic
        {
            get
            {
                if (Isotope.Ic.HasValue)
                {
                    return Isotope.Ic.Value.ToString("0.########");
                }
                else
                {
                    return _unknownValue;
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="isotope">The Isotope to represent.</param>
        public IsotopeViewModel(Isotope isotope)
        {
            Isotope = isotope;
        }
    }
}
