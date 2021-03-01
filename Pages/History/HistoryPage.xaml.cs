using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarOverlay;
using YoutubeGameBarWidget.Pages;
using YoutubeGameBarWidget.Pages.PageObjects;
using YoutubeGameBarWidget.Utilities;

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A History page to show user's list of recent media playback.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {
        private ThemeResources ColorResources;
        public HistoryPageResources LangResources;
        private Cabinet Cabinet;
        private List<HistoryEntry> HistoryEntries;
        public HistoryPage()
        {
            LangResources = BabelTower.getTranslatedResources<HistoryPageResources>();
            ColorResources = Painter.GetTheme();
            Cabinet = new Cabinet();

            InitializeComponent();
        }

        /// <summary>
        /// Checks if contents have already been loaded then presents the page as soon as frame navigates to Changelog Page.
        /// </summary>
        /// <param name="e">The navigation arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Painter.RunUIUpdateByMethod(StartLoading);
            GetEntries();
            Painter.RunUIUpdateByMethod(FinishLoading);

            base.OnNavigatedTo(e);
        }

        private void GetEntries()
        {
            HistoryEntries = Cabinet.GetEntries();
            GroupedEntries.Source = from he in this.HistoryEntries group he by he.Timestamp;
        }

        /// <summary>
        /// Event Handler for clicks on the Back Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a Started Loading ocasion.
        /// </summary>
        public async void StartLoading()
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        HistoryList.Visibility = Visibility.Collapsed;
                        LoadingRing.IsEnabled = true;
                        LoadingRing.IsActive = true;
                    }
                );

        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a Finished Loading ocasion.
        /// </summary>
        public async void FinishLoading()
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        LoadingRing.IsEnabled = false;
                        LoadingRing.IsActive = false;
                        HistoryList.Visibility = Visibility.Visible;
                    }
                );

        }
    }
}
