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
namespace Elementary.Model
{
    /// <summary>
    /// Represents an orbital in an electron configuration.
    /// </summary>
    public class Orbital
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shell">The orbital shell.</param>
        /// <param name="type">The orbital type.</param>
        /// <param name="electrons">The number of electrons.</param>
        internal Orbital(int shell, char type, int electrons)
        {
            Shell = shell;
            Type = type;
            Electrons = electrons;
        }

        /// <summary>
        /// The shell number.
        /// </summary>
        public int Shell { get; private set; }

        /// <summary>
        /// The orbital type.
        /// </summary>
        public char Type { get; private set; }

        /// <summary>
        /// The number of electrons in this orbital.
        /// </summary>
        public int Electrons { get; private set; }
    }
}
