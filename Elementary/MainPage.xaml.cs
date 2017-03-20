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
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Elementary
{
    /// <summary>
    /// Page that serves as a container for all other Pages of the application.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Reference to the MainPage singleton instance.
        /// </summary>
        public static MainPage Current { get; private set; }

        /// <summary>
        /// Gets or sets the title to display in the CommandBar.
        /// </summary>
        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }
        public static readonly DependencyProperty PageTitleProperty =
            DependencyProperty.Register("PageTitle", typeof(string), typeof(MainPage), null);

        /// <summary>
        /// Gets the ViewModel for the subtext value ComboBox.
        /// </summary>
        private SubtextValueViewModel SubtextValue { get; } = new SubtextValueViewModel();

        /// <summary>
        /// Gets the application version name as a string.
        /// </summary>
        private string AboutVersion
        {
            get
            {
                var version = Package.Current.Id.Version;
                return string.Join(".", new object[] { version.Major, version.Minor, version.Build, version.Revision });
            }
        }

        /// <summary>
        /// Gets the support email link.
        /// </summary>
        private string AboutEmailLink
        {
            get
            {
                return "mailto:elementary@ultramegasoft.com?subject=Elementary " + AboutVersion;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            Current = this;

            DarkThemeSetting.IsChecked = Settings.DarkTheme;

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

            ShowControlsSetting.IsChecked = Settings.ShowControls;

            Settings.SettingChanged += OnSettingChanged;
        }

        /// <summary>
        /// Updates the interface when a preference is changed.
        /// </summary>
        /// <param name="key">The key for the setting that has changed.</param>
        /// <param name="value">The new value for the setting.</param>
        private void OnSettingChanged(Settings.Key key, object value)
        {
            if (key == Settings.Key.DarkTheme)
            {
                RequestedTheme = (bool)value ? ElementTheme.Dark : ElementTheme.Light;
            }
            else if (key == Settings.Key.ElementColors)
            {
                if (Settings.ElementColors == "cat")
                {
                    (ElementColorsSetting.Children[0] as RadioButton).IsChecked = true;
                }
                else
                {
                    (ElementColorsSetting.Children[1] as RadioButton).IsChecked = true;
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
                ContentFrame.Navigate(typeof(ElementListPage), null,
                    new DrillInNavigationTransitionInfo());
            }
            else
            {
                ContentFrame.Navigate(typeof(PeriodicTablePage), null,
                    new DrillInNavigationTransitionInfo());
            }
            ContentFrame.BackStack.Clear();
        }

        /// <summary>
        /// Sets the theme preference when the settings button is clicked.
        /// </summary>
        /// <param name="sender">The ToggleMenuFlyoutItem.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDarkThemeToggled(object sender, RoutedEventArgs e)
        {
            SettingsButton.Flyout.Hide();
            Settings.DarkTheme = DarkThemeSetting.IsChecked.Value;
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

        /// <summary>
        /// Sets the preference for whether to show the controls under the Periodic Table when the
        /// setting CheckBox is toggled.
        /// </summary>
        /// <param name="sender">The CheckBox.</param>
        /// <param name="e">The event arguments.</param>
        private void OnShowControlsToggled(object sender, RoutedEventArgs e)
        {
            Settings.ShowControls = ShowControlsSetting.IsChecked.Value;
        }

        /// <summary>
        /// Opens the about dialog when the about button is clicked.
        /// </summary>
        /// <param name="sender">The AppBarButton.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnAboutButtonClicked(object sender, RoutedEventArgs e)
        {
            await AboutDialog.ShowAsync();
        }

        /// <summary>
        /// Hides the list view toggle button when the content Frame is navigated away from the
        /// relevant Pages.
        /// </summary>
        /// <param name="sender">The content Frame.</param>
        /// <param name="e">The event arguments.</param>
        private void OnContentFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(PeriodicTablePage)
                || e.SourcePageType == typeof(ElementListPage))
            {
                ListViewButton.Visibility = Visibility.Visible;
            }
            else
            {
                ListViewButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
