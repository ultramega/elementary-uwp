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
using Elementary.UI;
using Elementary.ViewModels;
using System;
using System.ComponentModel;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Elementary
{
    /// <summary>
    /// Page that displays the Periodic Table.
    /// </summary>
    public sealed partial class PeriodicTablePage : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// The amount to zoom in or out for programmatic zooms.
        /// </summary>
        private static readonly float _zoomStep = 0.5f;

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// The format string for the title of the element details dialog.
        /// </summary>
        private readonly string _detailsTitleFormat =
            ResourceLoader.GetForCurrentView().GetString("Title/ElementDetails");

        /// <summary>
        /// The current title of the element details dialog.
        /// </summary>
        private string _detailsTitle;

        /// <summary>
        /// Gets the title of the element details dialog or sets the name of the element to include
        /// in the title.
        /// </summary>
        private string DetailsTitle
        {
            get
            {
                return _detailsTitle;
            }
            set
            {
                _detailsTitle = string.Format(_detailsTitleFormat, value);
                PropertyChanged(this, new PropertyChangedEventArgs("DetailsTitle"));
            }
        }

        /// <summary>
        /// Gets whether the view can be zoomed in.
        /// </summary>
        public bool CanZoomIn
        {
            get
            {
                return Zoomer.ZoomFactor < Zoomer.MaxZoomFactor;
            }
        }

        /// <summary>
        /// Gets whether the view can be zoomed out.
        /// </summary>
        public bool CanZoomOut
        {
            get
            {
                return Zoomer.ZoomFactor > Zoomer.MinZoomFactor;
            }
        }

        /// <summary>
        /// Gets the ViewModel for the subtext value ComboBox.
        /// </summary>
        private SubtextValueViewModel SubtextValue { get; } = new SubtextValueViewModel();

        /// <summary>
        /// Gets the ViewModel for the block colors ComboBox.
        /// </summary>
        private BlockColorViewModel BlockColors { get; } = new BlockColorViewModel();

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeriodicTablePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Locks the screen in landscape mode when the Page is navigated to.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DisplayInformation.AutoRotationPreferences =
                DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;
        }

        /// <summary>
        /// Unlocks the screen orientation when the Page is navigated away from.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        /// <summary>
        /// Opens the ContentDialog to display the details of an element when it is selected.
        /// </summary>
        /// <param name="sender">The PeriodicTableControl.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnBlockClick(object sender, BlockClickEventArgs e)
        {
            var element = new ElementDetailsViewModel(e.ClickedElement);
            DetailsTitle = element.Name;
            DetailsDialog.DataContext = element;
            await DetailsDialog.ShowAsync();
        }

        /// <summary>
        /// Called when the zoom in button is clicked.
        /// </summary>
        /// <param name="sender">The zoom in Button.</param>
        /// <param name="e">The event arguments.</param>
        private void OnZoomIn(object sender, RoutedEventArgs e)
        {
            Zoomer.ChangeView(null, null, Zoomer.ZoomFactor + Zoomer.ZoomFactor * _zoomStep);
        }

        /// <summary>
        /// Called when the zoom out button is clicked.
        /// </summary>
        /// <param name="sender">The zoom out Button.</param>
        /// <param name="e">The event arguments.</param>
        private void OnZoomOut(object sender, RoutedEventArgs e)
        {
            Zoomer.ChangeView(null, null, Zoomer.ZoomFactor - Zoomer.ZoomFactor * _zoomStep);
        }

        /// <summary>
        /// Updates the zoom buttons when the view changes.
        /// </summary>
        /// <param name="sender">The ScrollViewer.</param>
        /// <param name="e">The event arguments.</param>
        private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("CanZoomIn"));
            PropertyChanged(this, new PropertyChangedEventArgs("CanZoomOut"));
        }
    }
}
