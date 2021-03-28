using System;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarOverlay;
using YoutubeGameBarWidget.Pages;
using YoutubeGameBarWidget.Pages.PageObjects;
using YoutubeGameBarWidget.Utilities;

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A webview page able to display the desired video using Youtube Game Bar's VideoUI.
    /// </summary>
    public sealed partial class Webpage : Page
    {
        WebPageResources LangResources;
        ThemeResources ColorResources;
        public Webpage()
        {
            LangResources = BabelTower.getTranslatedResources<WebPageResources>();
            ColorResources = Painter.GetTheme();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            InitializeComponent();
        }

        /// <summary>
        /// Cleans Webpage's content after frame navigates from it.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            VideoUIWebpage.Source = new Uri("about: blank");
            UnloadObject(VideoUIWebpage);
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Navigates to the given Video URL and calls tips presentation as soon as the frame navigates to this WebPage.
        /// </summary>
        /// <param name="e">The navigation events, may containing an InformationPayload.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                InformationPayload information = (InformationPayload)e.Parameter;
                VideoUIWebpage.Navigate(information.VideoURI);

                if ((string) Utils.GetSettingValue(Constants.Settings.ShowTips["varname"]) == Constants.Settings.ShowTips["True"])
                {
                    Painter.RunUIUpdateByMethod(ShowTips);
                }

                base.OnNavigatedTo(e);
            }
        }

        /// <summary>
        /// Presents VideoUI. 
        /// 
        /// Hides the WebView, showing the tips and finally presents Webview.
        /// </summary>
        private async void ShowTips()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                            {
                                VideoUIWebpage.Visibility = Visibility.Collapsed;
                                EnterTips.Begin();
                            }
                    );

            Thread.Sleep(3500);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            ExitTips.Begin();
                            VideoUIWebpage.Visibility = Visibility.Visible;
                        }
                    );
        }

        /// <summary>
        /// Handles the keypresses on Webpage's main grid.
        /// In case Backspace is pressed, navigates back to the main screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="keyArgs"></param>
        private void HandleBackspacePress(object sender, KeyRoutedEventArgs keyArgs)
        {
            if (keyArgs.Key == Windows.System.VirtualKey.Back)
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        /// <summary>
        /// Handles redirects to inside the Webview, displaying the appropiate message an acting based on the redirect URI.
        /// In case of a valid URL selected by user, loads it on VideoUI. Otherwise, warn user about invalid link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void LinkRedirectHandler(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            string redirectUrl = args.Uri.ToString();

            if (Validator.IsMediaURLValid(redirectUrl) == true)
            {
                Frame.Navigate(typeof(WarnPage), new WarnPayload(LangResources.LoadingWarn, typeof(Webpage), 2500));

                VideoUIWebpage.Navigate(Utils.GetProperVideoUIUri(redirectUrl));
            }
            else
            {
                Frame.Navigate(typeof(WarnPage), new WarnPayload(LangResources.InvalidURLWarn, typeof(Webpage), 1500));
            }
            
            args.Handled = true;
        }

        /// <summary>
        /// Handles the codes sent by the webpage catch by the ScriptNotify event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScriptNotifyHandler(object sender, NotifyEventArgs e)
        {
            if (e.Value == "goback")
            {
                Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
