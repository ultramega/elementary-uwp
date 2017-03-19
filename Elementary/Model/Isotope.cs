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
using System.Text;

namespace Elementary.Model
{
    /// <summary>
    /// Represents an isotope of an element.
    /// </summary>
    public class Isotope
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="number">The mass number of the isotope.</param>
        /// <param name="symbol">The symbol for the isotope.</param>
        /// <param name="mass">The relative atomic mass of the isotope.</param>
        internal Isotope(int number, string symbol, double mass)
        {
            Number = number;
            _symbol = symbol;
            Mass = mass;
        }

        /// <summary>
        /// The mass number of the isotope.
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// The symbol for the isotope.
        /// </summary>
        private string _symbol;

        /// <summary>
        /// Gets the formatted symbol for the isotope.
        /// </summary>
        public string Symbol
        {
            get
            {
                var super = new char[] { '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹' };
                var ret = new StringBuilder();
                foreach (var c in Number.ToString().Split())
                {
                    ret.Append(super[int.Parse(c)]);
                }
                ret.Append(_symbol);
                return ret.ToString();
            }
        }

        /// <summary>
        /// The relative atomic mass of the isotope.
        /// </summary>
        public double Mass { get; private set; }
    }
}
