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
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace elementary
{
    /// <summary>
    /// Page that serves as a container for all other Pages of the application.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;

            foreach (RadioButton setting in TemperatureUnitsSetting.Children)
            {
                if (setting.Tag as string == Settings.TemperatureUnits)
                {
                    setting.IsChecked = true;
                    break;
                }
            }

            foreach (RadioButton setting in ElementColorsSetting.Children)
            {
                if (setting.Tag as string == Settings.ElementColors)
                {
                    setting.IsChecked = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Toggles between the Periodic Table and list view when the list view button is toggled.
        /// </summary>
        /// <param name="sender">The AppBarToggleButton.</param>
        /// <param name="e">The event arguments.</param>
        private void OnListViewToggled(object sender, RoutedEventArgs e)
        {
            if ((sender as AppBarToggleButton).IsChecked == true)
            {
                ContentFrame.Navigate(typeof(ElementListPage), null, new DrillInNavigationTransitionInfo());
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
            }
            else
            {
                ContentFrame.GoBack();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;
            }
        }

        /// <summary>
        /// Sets the preference for temperature units when a RadioButton for temperature units is
        /// checked.
        /// </summary>
        /// <param name="sender">The RadioButton.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTemperatureUnitsChecked(object sender, RoutedEventArgs e)
        {
            Settings.TemperatureUnits = (sender as FrameworkElement).Tag as string;
        }

        /// <summary>
        /// Sets the preference for element colors when a RadioButton for element colors is
        /// checked.
        /// </summary>
        /// <param name="sender">The RadioButton.</param>
        /// <param name="e">The event arguments.</param>
        private void OnElementColorsChecked(object sender, RoutedEventArgs e)
        {
            Settings.ElementColors = (sender as FrameworkElement).Tag as string;
        }
    }
}
