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
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace elementary
{
    /// <summary>
    /// Page for displaying the full details of an element as a standalone Page.
    /// </summary>
    public sealed partial class ElementDetailsPage : Page
    {
        /// <summary>
        /// Whether this should be shown in master/detail format.
        /// </summary>
        private bool ShouldGoToWideState
        {
            get
            {
                return Window.Current.Bounds.Width >= 720;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ElementDetailsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the requested element when the Page is navigated to.
        /// </summary>
        /// <param name="e">
        /// The event arguments which contains the element database ID as the Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var titleFormat = ResourceLoader.GetForCurrentView().GetString("Title/ElementDetails");
            var title = string.Format(titleFormat, (e.Parameter as ElementDetails).Name);
            MainPage.Current.PageTitle = title;

            (Content as Frame).Navigate(typeof(ElementPage), e.Parameter);

            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += OnBackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        /// <summary>
        /// Called when the Page is navigated away from.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            MainPage.Current.PageTitle = null;

            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= OnBackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// Navigates back to the list when the back button is pressed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            Frame.GoBack(new DrillInNavigationTransitionInfo());
        }

        /// <summary>
        /// Navigates to the master/detail Page.
        /// </summary>
        /// <param name="useTransition">Whether to show the navigation transition.</param>
        private void NavigateToWide(bool useTransition)
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            if (useTransition)
            {
                Frame.GoBack(new EntranceNavigationTransitionInfo());
            }
            else
            {
                Frame.GoBack(new SuppressNavigationTransitionInfo());
            }
        }

        /// <summary>
        /// Called when the Page is loaded.
        /// </summary>
        /// <param name="sender">The Page.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (ShouldGoToWideState)
            {
                NavigateToWide(true);
            }
            Window.Current.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// Called when the Page is unloaded.
        /// </summary>
        /// <param name="sender">The Page.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= OnSizeChanged;
        }

        /// <summary>
        /// Checks the screen width when the Window size changes and navigates to the master/detail
        /// view if the screen is wide enough.
        /// </summary>
        /// <param name="sender">The Window.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (ShouldGoToWideState)
            {
                Window.Current.SizeChanged -= OnSizeChanged;
                NavigateToWide(false);
            }
        }
    }
}
