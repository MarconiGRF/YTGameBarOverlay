using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private HistoryPageResources LangResources;
        private Cabinet Cabinet;
        private List<HistoryEntry> HistoryEntries;
        public HistoryPage()
        {
            LangResources = BabelTower.getTranslatedResources<HistoryPageResources>();
            ColorResources = Painter.GetTheme();
            Cabinet = new Cabinet();

            GetEntries();
            InitializeComponent();
        }

        private void GetEntries()
        {
            this.HistoryEntries = Cabinet.GetEntries();
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
    }
}
