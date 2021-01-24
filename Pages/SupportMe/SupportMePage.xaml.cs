using System;
using System.Diagnostics;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SupportMePage : Page
    {
        private ThemeResources ColorResources;
        private SupportMeResources LangResources;
        public SupportMePage()
        {
            LangResources = BabelTower.getTranslatedResources<SupportMeResources>();
            ColorResources = Painter.GetTheme();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            this.InitializeComponent();
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
        /// Event Handler for clicks on the Back Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WarnPage), new WarnPayload(LangResources.ThankYou, typeof(MainPage), 2000));
            Windows.System.Launcher.LaunchUriAsync(new Uri(Constants.DonationURL));
        }
    }
}
