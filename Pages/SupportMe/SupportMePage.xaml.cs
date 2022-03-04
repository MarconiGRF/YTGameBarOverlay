using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YTGameBarOverlay;
using YTGameBarWidget.Pages;
using YTGameBarWidget.Pages.PageObjects;
using YTGameBarWidget.Utilities;

namespace YTGameBarWidget
{
    /// <summary>
    /// A "Support me" page to let users show some love to YTGBO <3.
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
