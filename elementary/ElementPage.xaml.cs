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
using elementary.ViewModel;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace elementary
{
    /// <summary>
    /// Page for displaying the full details of an element.
    /// </summary>
    public sealed partial class ElementPage : Page
    {
        /// <summary>
        /// The data source for the Page.
        /// </summary>
        private ElementDetails Element { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ElementPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the requested element when the Page is navigated to.
        /// </summary>
        /// <param name="e">
        /// The event arguments which contains the ElementDetails as the Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Element = e.Parameter as ElementDetails;
        }

        /// <summary>
        /// Launches a Web page when one of the buttons is pressed.
        /// </summary>
        /// <param name="sender">The pressed Button.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var url = (sender as FrameworkElement).Tag as string;
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
