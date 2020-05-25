using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A webview page able to display the desired video.
    /// </summary>
    public sealed partial class Webpage : Page
    {
        public WebView VideoUIWebpage;
        public Webpage()
        {
            this.InitializeComponent();
            this.VideoUIWebpage = _videoUIWebpage;
        }

        /// <summary>
        /// Navigates to the given Video URL and show tips as soon as frame navigates to this Webpage.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.VideoUIWebpage.Navigate((Uri)e.Parameter);
            Thread t = new Thread(new ThreadStart(PresentPage));
            t.Start();

            base.OnNavigatedTo(e);
        }

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
        /// In case Backspace is pressed, executes navigates back to the main screen.
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
