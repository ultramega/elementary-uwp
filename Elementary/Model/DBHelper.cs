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
using Elementary.ViewModels;
using SQLitePCL;
using System.Collections;
using Windows.ApplicationModel;

namespace Elementary.Model
{
    /// <summary>
    /// Helpers for accessing the element database.
    /// </summary>
    class DBHelper
    {
        /// <summary>
        /// The SQLite connection, created on demand.
        /// </summary>
        private static SQLiteConnection _connection;

        /// <summary>
        /// Gets the SQLite connection.
        /// </summary>
        private static SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var path = Package.Current.InstalledLocation.Path + "\\Assets\\elements.db";
                    _connection = new SQLiteConnection(path);
                }
                return _connection;
            }
        }

        /// <summary>
        /// Get the full details for an element.
        /// </summary>
        /// <param name="id">The database ID for the element.</param>
        /// <returns>The ElementDetails ViewModel.</returns>
        public static ElementDetails GetElement(long id)
        {
            using (var statement = Connection.Prepare("SELECT * FROM elements WHERE _id = ?;"))
            {
                statement.Bind(1, id);
                if (SQLiteResult.ROW == statement.Step())
                {
                    Element element = new Element();
                    element._ID = (long)statement["_id"];
                    element.Number = (long)statement["num"];
                    element.Symbol = (string)statement["sym"];
                    element.Group = (long)statement["g"];
                    element.Period = (long)statement["p"];
                    element.Block = (string)statement["b"];
                    element.Weight = (double)statement["w"];
                    element.Density = (double?)statement["dens"];
                    element.Melt = (double?)statement["melt"];
                    element.Boil = (double?)statement["boil"];
                    element.Heat = (double?)statement["heat"];
                    element.Negativity = (double?)statement["neg"];
                    element.Abundance = (double?)statement["ab"];
                    element.Category = (long)statement["cat"];
                    element.Configuration = (string)statement["ec"];
                    element.Electrons = (string)statement["eps"];
                    element.Unstable = (long)statement["uns"] == 1;
                    element.Video = (string)statement["vid"];
                    return new ElementDetails(element);
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
            ArrayList result = new ArrayList();
            using (var statement = Connection.Prepare("SELECT _id, num, sym, b, cat FROM elements;"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    Element element = new Element();
                    element._ID = (long)statement["_id"];
                    element.Number = (long)statement["num"];
                    element.Symbol = (string)statement["sym"];
                    element.Block = (string)statement["b"];
                    element.Category = (long)statement["cat"];
                    result.Add(new ElementListItem(element));
                }
            }
            return (ElementListItem[])result.ToArray(typeof(ElementListItem));
        }

        /// <summary>
        /// Get the details for the table of elements.
        /// </summary>
        /// <returns>An array of ElementBlock ViewModels.</returns>
        public static ElementBlock[] GetElementTable()
        {
            ArrayList result = new ArrayList();
            using (var statement = Connection.Prepare("SELECT _id, num, sym, g, p, b, w, cat, uns FROM elements;"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    Element element = new Element();
                    element._ID = (long)statement["_id"];
                    element.Number = (long)statement["num"];
                    element.Symbol = (string)statement["sym"];
                    element.Group = (long)statement["g"];
                    element.Period = (long)statement["p"];
                    element.Block = (string)statement["b"];
                    element.Weight = (double)statement["w"];
                    element.Category = (long)statement["cat"];
                    element.Unstable = (long)statement["uns"] == 1;
                    result.Add(new ElementBlock(element));
                }
            }
            return (ElementBlock[])result.ToArray(typeof(ElementBlock));
        }
    }
}
