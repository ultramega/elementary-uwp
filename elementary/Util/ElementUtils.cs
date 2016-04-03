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
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace elementary.Util
{
    /// <summary>
    /// Helpers for the periodic table.
    /// </summary>
    public class ElementUtils
    {
        /// <summary>
        /// The reference to the ResourceLoader for the Elements resources.
        /// </summary>
        private static readonly ResourceLoader _res = ResourceLoader.GetForCurrentView("Elements");

        /// <summary>
        /// Map of element categories to block backgrounds.
        /// </summary>
        private static readonly Dictionary<object, Brush> _backgrounds = new Dictionary<object, Brush>()
        {
            { 0L, new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xF1, 0x2F)) },
            { 1L, new SolidColorBrush(Color.FromArgb(0xFF, 0x79, 0xB5, 0xFF)) },
            { 2L, new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xAA, 0x00)) },
            { 3L, new SolidColorBrush(Color.FromArgb(0xFF, 0xF3, 0xF3, 0x00)) },
            { 4L, new SolidColorBrush(Color.FromArgb(0xFF, 0x55, 0xCC, 0x88)) },
            { 5L, new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xDD, 0xBB)) },
            { 6L, new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0xBB, 0xAA)) },
            { 7L, new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0x99, 0x99)) },
            { 8L, new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xB7, 0x9B)) },
            { 9L, new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xAA, 0xCC)) },
            { "s", new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0x99, 0xFF)) },
            { "p", new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xCC, 0x00)) },
            { "d", new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x99, 0x99)) },
            { "f", new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xCC, 0x33)) }
        };

        /// <summary>
        /// The list of element category names.
        /// </summary>
        private static string[] _categories;

        /// <summary>
        /// Gets the list of element category names.
        /// </summary>
        public static string[] Categories
        {
            get
            {
                if (_categories == null)
                {
                    _categories = _res.GetString("Categories").Split(';');
                }
                return _categories.Clone() as string[];
            }
        }

        /// <summary>
        /// Gets the background Brush for an element block based on the key.
        /// </summary>
        /// <param name="key">The key associated with the color.</param>
        /// <returns>The Brush to use for the block background.</returns>
        public static Brush GetBlockColor(object key)
        {
            if (!_backgrounds.ContainsKey(key))
            {
                return null;
            }
            return _backgrounds[key];
        }

        /// <summary>
        /// Gets the background Brush for the given Element.
        /// </summary>
        /// <param name="el">The Element.</param>
        /// <returns>The Brush to use for the block background.</returns>
        public static Brush GetBlockColor(Element el)
        {
            if (Settings.ElementColors == "block")
            {
                return GetBlockColor(el.Block);
            }
            return GetBlockColor(el.Category);
        }

        /// <summary>
        /// Gets the name of the element based on the atomic number.
        /// </summary>
        /// <param name="num">The atomic number of the element.</param>
        /// <returns>The name of the element.</returns>
        public static string GetElementName(long num)
        {
            return _res.GetString(string.Format("Name/{0:d3}", num));
        }

        /// <summary>
        /// Gets the URL to the Wikipedia page for the element based on the atomic number.
        /// </summary>
        /// <param name="num">The atomic number of the element.</param>
        /// <returns>The URL to the Wikipedia page for the element.</returns>
        public static string GetElementWiki(long num)
        {
            var lang = _res.GetString("Wiki/Lang");
            var page = _res.GetString(string.Format("Wiki/{0:d3}", num));
            return string.Format("https://{0}.wikipedia.org/wiki/{1}", lang, page);
        }
    }
}
