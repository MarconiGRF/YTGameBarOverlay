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
        public HistoryPageResources LangResources;
        private bool IsFirstOpening;
        private Cabinet Cabinet;
        private List<HistoryEntry> HistoryEntries;
        private ThemeResources ColorResources;
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
            IsFirstOpening = true;
            Painter.RunUIUpdateByMethod(StartLoading);
            GetEntries();

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Gets the entries from database, divides them on groups based on TimeStamp and orders it descending and finally sets as source for UI elements.
        /// If no entries were found a message about empty history is shown.
        /// </summary>
        private void GetEntries()
        {
            HistoryEntries = Cabinet.GetEntries();

            if (HistoryEntries.Count == 0)
            {
                Painter.RunUIUpdateByMethod(ShowEmptyMessage);
            }
            else
            {
                IEnumerable<IGrouping<string, HistoryEntry>> entries = from he in this.HistoryEntries orderby he.Id descending group he by he.Timestamp;
                GroupedEntries.Source = entries;

                Painter.RunUIUpdateByMethod(FinishLoading);
            }
        }

        /// <summary>
        /// Handles the selection changed event for History List.
        /// In case of first opening the page (where the selectiion changed fires up) it selects null so no action must be taken, it also updates the pristine flag.
        /// In case user selecting a item (where the pristine flag is false) it will follow the normal workflow to playback the media.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsFirstOpening)
            {
                HistoryList.SelectedItem = null;
                IsFirstOpening = false;
            }
            else 
            {
                HistoryEntry selected = HistoryList.SelectedItem as HistoryEntry;
                selected.Id = 0;
                await Cabinet.SaveEntry(selected);

                StartPlayback(selected.MediaURL);
            }
        }

        /// <summary>
        /// Playbacks the selected video on the WebPage.
        /// </summary>
        /// <param name="mediaURL">The Media URL to be played.</param>
        private void StartPlayback(string mediaURL)
        {
            InformationPayload information = new InformationPayload(Utils.GetProperVideoUIUri(mediaURL));

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Frame.Navigate(typeof(Webpage), information);
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
        /// Event Handler for clicks on the Clear History Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            Painter.RunUIUpdateByMethod(StartLoading);
            bool operationSuccess = await Cabinet.DeleteAll();

            if (operationSuccess)
            {
                GetEntries();
            }
            else{
                Frame.Navigate(typeof(WarnPage), new WarnPayload(LangResources.ErrorCleaningHistory, typeof(HistoryPage), 2700));
            }
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

        /// <summary>
        /// Auxiliary method to asynchronously update UI to show "Empty Message" elementss.
        /// </summary>
        public async void ShowEmptyMessage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        HistoryList.Visibility = Visibility.Collapsed;
                        LoadingRing.IsEnabled = false;
                        LoadingRing.IsActive = false;
                        EmptyMessage.Visibility = Visibility.Visible;
                    }
                );

        }
    }
}
