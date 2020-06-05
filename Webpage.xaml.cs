using System;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A webview page able to display the desired video.
    /// </summary>
    public sealed partial class Webpage : Page
    {
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
        /// Navigates to the given Video URL and show tips as soon as frame navigates to this Webpage.
        /// </summary>
        /// <param name="e">The Video URI.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.VideoUIWebpage.Navigate((Uri)e.Parameter);
            Thread t = new Thread(new ThreadStart(PresentPage));
            t.Start();

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Presents VideoUI. 
        /// 
        /// Hiding the WebView, showing the tips and finally presents Webview.
        /// </summary>
        private async void PresentPage()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                            {
                                this.VideoUIWebpage.Visibility = Visibility.Collapsed;
                                EnterStoryboard.Begin();
                            }
                    );

            Thread.Sleep(3500);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            ExitStoryboard.Begin();
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
                this.Frame.Navigate(typeof(YoutubeGameBarOverlay.MainPage));
            }
        }
    }
}
