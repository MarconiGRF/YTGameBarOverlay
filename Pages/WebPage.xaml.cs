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
        private MainPage MainPageInstance;
        private Thread UIUpdateThread;
        public Webpage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Cleans Webpage's content after frame navigates from it.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
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
                if (this.MainPageInstance == null)
                {
                    this.MainPageInstance = information.MainPage;
                }

                this.VideoUIWebpage.Navigate(information.VideoURI);
                RunUIUpdateByMethod(PresentPage);

                base.OnNavigatedTo(e);
            }
        }

        /// <summary>
        /// Asynchronously runs an UI updated defined by the given method using the UIUpdate thread.
        /// </summary>
        /// <param name="uiMethod">The UI update method to be executed.</param>
        private void RunUIUpdateByMethod(Action uiMethod)
        {
            this.UIUpdateThread = new Thread(new ThreadStart(uiMethod));
            this.UIUpdateThread.Start();
        }

        /// <summary>
        /// Presents VideoUI. 
        /// 
        /// Hides the WebView, showing the tips and finally presents Webview.
        /// </summary>
        private async void PresentPage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                            {
                                this.VideoUIWebpage.Visibility = Visibility.Collapsed;
                                EnterTips.Begin();
                            }
                    );

            Thread.Sleep(3500);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            ExitTips.Begin();
                            this.VideoUIWebpage.Visibility = Visibility.Visible;
                        }
                    );
        }

        /// <summary>
        /// Handles the keypresses on Webpage's main grid.
        /// In case Backspace is pressed, navigates back to the main screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="keyArgs"></param>
        private async void HandleBackspacePress(object sender, KeyRoutedEventArgs keyArgs)
        {
            if (keyArgs.Key == Windows.System.VirtualKey.Back)
            {
                this.Frame.Navigate(typeof(MainPage));
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
            this.MainPageInstance.mediaURL = redirectUrl;

            if (this.MainPageInstance.IsMediaURLValid() == true)
            {
                this.Frame.Navigate(typeof(WarnPage), new WarnPayload(Constants.Warn.Loading));


                this.VideoUIWebpage.Navigate(this.MainPageInstance.GetProperVideoUIUri());
            }
            else
            {
                this.Frame.Navigate(typeof(WarnPage), new WarnPayload(Constants.Error.URLNotValid));
            }
            
            args.Handled = true;
        }
    }
}
