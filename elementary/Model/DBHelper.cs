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
using elementary.ViewModel;
using SQLitePCL;
using System.Collections;
using Windows.ApplicationModel;

namespace elementary.Model
{
    /// <summary>
    /// Helpers for accessing the element database.
    /// </summary>
    class DBHelper
    {
        /// <summary>
        /// The SQLite connection, created on demand.
        /// </summary>
        private static SQLiteConnection _db;

        /// <summary>
        /// Gets the SQLite connection.
        /// </summary>
        private static SQLiteConnection DB
        {
            get
            {
                if (_db == null)
                {
                    var path = Package.Current.InstalledLocation.Path + "\\Assets\\elements.db";
                    _db = new SQLiteConnection(path);
                }
                return _db;
            }
        }

        /// <summary>
        /// Get the full details for an element.
        /// </summary>
        /// <param name="id">The database ID for the element.</param>
        /// <returns>The ElementDetails ViewModel.</returns>
        public static ElementDetails GetElement(long id)
        {
            using (var stmt = DB.Prepare("SELECT * FROM elements WHERE _id = ?;"))
            {
                stmt.Bind(1, id);
                if (SQLiteResult.ROW == stmt.Step())
                {
                    Element el = new Element();
                    el._ID = (long)stmt["_id"];
                    el.Number = (long)stmt["num"];
                    el.Symbol = (string)stmt["sym"];
                    el.Group = (long)stmt["g"];
                    el.Period = (long)stmt["p"];
                    el.Block = (string)stmt["b"];
                    el.Weight = (double)stmt["w"];
                    el.Density = (double?)stmt["dens"];
                    el.Melt = (double?)stmt["melt"];
                    el.Boil = (double?)stmt["boil"];
                    el.Heat = (double?)stmt["heat"];
                    el.Negativity = (double?)stmt["neg"];
                    el.Abundance = (double?)stmt["ab"];
                    el.Category = (long)stmt["cat"];
                    el.Configuration = (string)stmt["ec"];
                    el.Electrons = (string)stmt["eps"];
                    el.Unstable = (long)stmt["uns"] == 1;
                    el.Video = (string)stmt["vid"];
                    return new ElementDetails(el);
                }
            }
            return null;
        }

        /// <summary>
        /// Get the details for the element list.
        /// </summary>
        /// <returns>An array of ElementListItem ViewModels.</returns>
        public static ElementListItem[] GetElementList()
        {
            ArrayList ret = new ArrayList();
            using (var stmt = DB.Prepare("SELECT _id, num, sym, b, cat FROM elements;"))
            {
                while (SQLiteResult.ROW == stmt.Step())
                {
                    Element el = new Element();
                    el._ID = (long)stmt["_id"];
                    el.Number = (long)stmt["num"];
                    el.Symbol = (string)stmt["sym"];
                    el.Block = (string)stmt["b"];
                    el.Category = (long)stmt["cat"];
                    ret.Add(new ElementListItem(el));
                }
            }
            return (ElementListItem[])ret.ToArray(typeof(ElementListItem));
        }

        /// <summary>
        /// Get the details for the table of elements.
        /// </summary>
        /// <returns>An array of ElementBlock ViewModels.</returns>
        public static ElementBlock[] GetElementTable()
        {
            ArrayList ret = new ArrayList();
            using (var stmt = DB.Prepare("SELECT _id, num, sym, g, p, b, w, cat, uns FROM elements;"))
            {
                while (SQLiteResult.ROW == stmt.Step())
                {
                    Element el = new Element();
                    el._ID = (long)stmt["_id"];
                    el.Number = (long)stmt["num"];
                    el.Symbol = (string)stmt["sym"];
                    el.Group = (long)stmt["g"];
                    el.Period = (long)stmt["p"];
                    el.Block = (string)stmt["b"];
                    el.Weight = (double)stmt["w"];
                    el.Category = (long)stmt["cat"];
                    el.Unstable = (long)stmt["uns"] == 1;
                    ret.Add(new ElementBlock(el));
                }
            }
            return (ElementBlock[])ret.ToArray(typeof(ElementBlock));
        }
    }
}
