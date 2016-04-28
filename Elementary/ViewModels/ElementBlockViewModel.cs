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

namespace Elementary.ViewModels
{
    /// <summary>
    /// Represents a block on the periodic table.
    /// </summary>
    public class ElementBlockViewModel : ElementBaseViewModel
    {
        /// <summary>
        /// Gets the string to display under the element symbol. For stable elements, this is the
        /// atomic weight with up to 4 decimal places. For unstable elements, this is the atomic
        /// weight as a whole number surrounded by square braces.
        /// </summary>
        public string Subtext
        {
            get
            {
                if (Element.Unstable)
                {
                    return string.Format("[{0:d}]", (long)Element.Weight);
                }
                return Element.Weight.ToString("0.####");
            }
        }

        /// <summary>
        /// Gets the row number of the periodic table to place the block.
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// Gets the column number of the periodic table to place the block.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="element">The Element to represent.</param>
        public ElementBlockViewModel(Element element) : base(element)
        {
            if (element.Group == 0)
            {
                if (element.Period == 6)
                {
                    Row = 9;
                    Column = (int)element.Number - 54;
                }
                else if (element.Period == 7)
                {
                    Row = 10;
                    Column = (int)element.Number - 86;
                }
            }
            else
            {
                Row = (int)element.Period;
                Column = (int)element.Group;
            }
        }
    }
}
